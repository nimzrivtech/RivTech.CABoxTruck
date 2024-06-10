using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.ExcelManager;
using RivTech.CABoxTruck.WebService.Service.Services.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _repository;
        private readonly IDriverIncidentsRepository _repositoryIncident;
        private readonly IDriverHeaderRepository _repositoryHeader;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IRiskFactory _riskFactory;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IBindingService _bindingService;
        private readonly IDriverExclusionEndorsementFormService _driverExclusionEndorsementFormService;
        private readonly IMapper _mapper;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IAddressRepository _addressRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;

        public DriverService(IDriverRepository repository,
            IAddressRepository addressRepository,
            IDriverIncidentsRepository repositoryIncident,
            IDriverHeaderRepository repositoryHeader,
            IRiskDetailRepository riskDetailRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IRiskFactory riskFactory,
            IMapper mapper,
            IRiskRepository riskRepository,
            IRiskFormRepository riskFormRepository,
            IBindingService bindingService,
            IDriverExclusionEndorsementFormService driverExclusionEndorsementFormService,
            ICoverageHistoryService coverageHistoryService,
            IEntityAddressRepository entityAddressRepository


        )
        {
            _addressRepository = addressRepository;
            _repository = repository;
            _repositoryIncident = repositoryIncident;
            _repositoryHeader = repositoryHeader;
            _riskDetailRepository = riskDetailRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _riskFactory = riskFactory;
            _mapper = mapper;
            _riskRepository = riskRepository;
            _riskFormRepository = riskFormRepository;
            _bindingService = bindingService;
            _driverExclusionEndorsementFormService = driverExclusionEndorsementFormService;
            _coverageHistoryService = coverageHistoryService;
            _entityAddressRepository = entityAddressRepository;
        }

        public async Task<List<DriverDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return _mapper.Map<List<DriverDTO>>(result);
        }

        public async Task<List<DriverDTO>> GetByRiskDetailIdAsync(Guid id)
        {
            var result = await _repository.GetByRiskDetailAsync(id);
            return result.Select(obj => _mapper.Map<DriverDTO>(result)).ToList();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

        public async Task<DriverDTO> GetByIdAsync(Guid id)
        {
            var result = await _repository.FindAsync(x => x.Id == id, i => i.RiskDetailDrivers);
            if (result == null)
                return null;
            var model = _mapper.Map<DriverDTO>(result);
            model.RiskDetailId = result.RiskDetailDrivers
                ?.OrderByDescending(rdv => rdv.UpdatedDate)
                ?.Select(rdv => rdv.RiskDetailId)
                ?.FirstOrDefault() ?? Guid.Empty;
            return model;
        }

        public async Task<DriverDTO> InsertAsync(DriverDTO model)
        {
            var risk = await _riskDetailRepository.GetInsuredAsync((Guid)model.RiskDetailId);
            var entityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.AddressTypeId != null,
                                   i => i.Address);
            if (model.IsOutOfState == false)
            {
                foreach (var item in entityAddresses)
                {
                    if (item.AddressTypeId == (short)AddressTypesEnum.Business)
                    {
                        model.State = item.Address.State;
                    }
                }
            }

            if (model.BirthDate != null)
            {
                var age = DateTime.Now.Year - model.BirthDate.Value.Year;

                if (model.BirthDate.Value.AddYears(age) > DateTime.Now)
                {
                    age = age - 1;
                }
                if (age < 16)
                {
                    throw new Exception("Error Occured age is not valid");
                }
                model.Age = Convert.ToString(age);
            }

            var driver = _mapper.Map<Driver>(model);
            driver.CreatedDate = DateTime.Now;
            driver.TotalFactor = null;
            driver.YearPoint = null;
            // driver.Id = Guid.NewGuid(); //no need this.. auto populate onf relation Id

            //driver.EntityId = Guid.NewGuid(); //no need this.. auto populate onf relation Id 
            //driver.Entity = entity; //mapp using automapper

            if (driver.DriverIncidents.Count > 0) driver.DriverIncidents.ForEach(x =>
            {
                x.Id = Guid.NewGuid();
                x.UniqueId = Guid.NewGuid();
            });

            var riskDetail = await _riskDetailRepository.FindAsync(r => r.Id == model.RiskDetailId);
            driver.LinkRiskDetail(riskDetail);

            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            driver.EffectiveDate = riskDetail?.EndorsementEffectiveDate ?? (brokerInfo?.EffectiveDate ?? DateTime.Now);
            driver.ExpirationDate = brokerInfo?.ExpirationDate ?? DateTime.Now.AddYears(1);

            var result = await _repository.AddAsync(driver);
            await _repository.SaveChangesAsync();

            return _mapper.Map<DriverDTO>(driver);
        }

        public async Task<DriverDTO> UpdateAsync(DriverDTO model, bool? fromEndorsement = false)
        {
            
            try
            {
                var risk = await _riskDetailRepository.GetInsuredAsync((Guid)model.RiskDetailId);
                var entityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.AddressTypeId != null,
                                       i => i.Address);
                if (model.IsOutOfState == false)
                {
                    foreach (var item in entityAddresses)
                    {
                        if (item.AddressTypeId == (short)AddressTypesEnum.Business)
                        {
                            model.State = item.Address.State;
                        }
                    }
                }
                if (model.BirthDate != null)
                {
                    int age = DateTime.Now.Year - model.BirthDate.Value.Year;

                    if (model.BirthDate.Value.AddYears(age) > DateTime.Now)
                    {
                        age = age - 1;
                    }
                    if (age < 16)
                    {
                        throw new Exception("Error Occured age is not valid");
                    }
                    model.Age = Convert.ToString(age);
                }

                var updateEntityData = _mapper.Map<Driver>(model);
                var driver = await _repository.GetIncludeAsync(o => o.Id == model.Id);

                updateEntityData.LicenseNumber = driver.LicenseNumber;

                model.RiskDetailId = driver.RiskDetailDrivers
                ?.OrderByDescending(rdv => rdv.UpdatedDate)
                ?.Select(rdv => rdv.RiskDetailId)
                ?.FirstOrDefault() ?? Guid.Empty;

                var riskDetail = await _riskDetailRepository.FindAsync(rd => rd.Id == model.RiskDetailId);

                // Get Drivers from previous risk
                var previousRiskDetailDrivers = await _repository.GetPreviousRiskDetailDrivers(riskDetail.RiskId);
                bool isFromPreviousEndorsement = previousRiskDetailDrivers?.Contains(driver) ?? false;



                if (updateEntityData.TotalFactor != driver.TotalFactor || updateEntityData.YearPoint != driver.YearPoint)
                {
                    throw new Exception("Error Occured Total Factor and Year Point is Read Only");
                }

                if (fromEndorsement == true)
                {
                    var previousRiskDetailDriver = previousRiskDetailDrivers.FirstOrDefault(x =>
                        x.LicenseNumber == updateEntityData.LicenseNumber
                        && x.State == updateEntityData.State);

                    if (
                        previousRiskDetailDriver != null
                        && previousRiskDetailDriver.Options is null
                        && updateEntityData.Options is null
                        && previousRiskDetailDriver.Id != driver.Id
                    )
                    {
                        updateEntityData.YearPoint = null;
                        updateEntityData.AgeFactor = null;
                        updateEntityData.TotalFactor = null;
                    }
                }

                Driver previousDriver = null;
                if (driver.PreviousDriverVersionId.HasValue)
                    previousDriver = await _repository.GetIncludeAsync(o => o.Id == driver.PreviousDriverVersionId);

                bool justReinstated = (previousDriver?.IsDeleted ?? false) && !updateEntityData.IsDeleted;
                bool justDeleted = !(previousDriver?.IsDeleted ?? false) && updateEntityData.IsDeleted;
                bool shouldCreateNewVersion = isFromPreviousEndorsement && HasChanges(driver, updateEntityData);
                bool shouldRollBack = !isFromPreviousEndorsement &&
                                        !HasChanges(previousDriver, updateEntityData)
                                        && !justReinstated && !justDeleted;

                if (fromEndorsement == true && shouldCreateNewVersion)
                {
                    //driver.IsActive = false;
                    //driver.DeletedDate = DateTime.Now;
                    driver.UnlinkLatestRiskDetail();
                    _repository.Update(driver);

                    await _repository.SaveChangesAsync();

                    updateEntityData.Id = System.Guid.Empty;
                    foreach (var inci in updateEntityData.DriverIncidents)
                    {
                        var o = driver.DriverIncidents.Where(i => i.Id == inci.Id).FirstOrDefault();
                        inci.Id = System.Guid.Empty;
                        inci.DriverId = System.Guid.Empty;
                        inci.UniqueId = (o is null) ? Guid.NewGuid() : o.UniqueId;
                    }
                    updateEntityData.LinkRiskDetail(riskDetail);
                    updateEntityData.PreviousDriverVersion = driver;

                    await _repository.AddAsync(updateEntityData);
                    await _repository.SaveChangesAsync();

                    return _mapper.Map<DriverDTO>(updateEntityData);
                }

                if (fromEndorsement == true && shouldRollBack)
                {
                    var currentRiskDetail = driver.UnlinkLatestRiskDetail();
                    _repository.Remove(driver);

                    if (driver.DriverIncidents != null)
                        _repositoryIncident.RemoveRange(driver.DriverIncidents);

                    // Link back the previous vehicle to current riskdetail.
                    previousDriver.LinkRiskDetail(currentRiskDetail.RiskDetail);

                    await _repositoryIncident.SaveChangesAsync();
                    await _repository.SaveChangesAsync();
                    return _mapper.Map<DriverDTO>(driver);
                }

                driver.Entity = updateEntityData.Entity;
                driver.YrsDrivingExp = updateEntityData.YrsDrivingExp;
                driver.YrsCommDrivingExp = updateEntityData.YrsCommDrivingExp;
                driver.IsMvr = updateEntityData.IsMvr;
                driver.YearPoint = updateEntityData.YearPoint;
                driver.AgeFactor = updateEntityData.AgeFactor;
                driver.TotalFactor = updateEntityData.TotalFactor;
                driver.IsOutOfState = updateEntityData.IsOutOfState;
                driver.MvrDate = updateEntityData.MvrDate;
                driver.HireDate = updateEntityData.HireDate;
                driver.LicenseNumber = updateEntityData.LicenseNumber;
                driver.Options = updateEntityData.Options;
                driver.State = updateEntityData.State;
                driver.isExcludeKO = updateEntityData.isExcludeKO;
                driver.koReason = updateEntityData.koReason;
                driver.IsValidated = updateEntityData.IsValidated;
                driver.IsValidatedKO = updateEntityData.IsValidatedKO;
                driver.EffectiveDate = updateEntityData.EffectiveDate;
                driver.ExpirationDate = updateEntityData.ExpirationDate;
                var remove = driver.DriverIncidents.Where(i => !updateEntityData.DriverIncidents.Select(s => s.Id).Contains(i.Id));
                _repositoryIncident.RemoveRange(remove);

                #region populate incident
                foreach (var item in updateEntityData.DriverIncidents)
                {
                    var o = driver.DriverIncidents.Where(i => i.Id == item.Id).FirstOrDefault();

                    if (o != null)
                    {
                        o.IncidentType = item.IncidentType;
                        o.IncidentDate = item.IncidentDate;
                        o.ConvictionDate = item.ConvictionDate;
                        // o.UniqueId = item.UniqueId;
                    }
                    else
                    {
                        item.UniqueId = Guid.NewGuid();
                        driver.DriverIncidents.Add(item);
                        await _repositoryIncident.AddAsync(item);
                    }
                }
                #endregion _repositoryIncident incident

                _repository.Update(driver);

                await _repository.SaveChangesAsync();

                return _mapper.Map<DriverDTO>(driver);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateDriverEffectiveDateAsync(DriverDTO model)
        {
            var driver = _mapper.Map<Driver>(model);

            var savedDriver = await _repository.GetByIdAsync(model.Id.Value);

            Driver updatedDriver;
            if (savedDriver.IsEffectiveExpirationDateChanged(driver))
            {
                // Detached the same driver queried stored driver in db to avoid conflict when updating dbcontext.
                _repository.Detach(savedDriver);

                updatedDriver = _repository.Update(driver);
                await _repository.SaveChangesAsync();
            }
        }

        ///// <summary>
        ///// Returns the list of excluded drivers which were already bound/endorsed.
        ///// </summary>
        //private async IEnumerable<DriverDTO> GetExcludedEndorsedDrivers(Guid riskDetailId)
        //{
        //    var drivers = await GetByRiskDetailIdAsync(riskDetailId);
        //}

        private bool HasChanges(Driver driver1, Driver driver2)
        {
            if (driver1 is null)
                return true;

            if (driver2 is null)
                return true;

            if (_riskFactory.IsDriverChanged(driver1, driver2))
                return true;

            if ((!string.IsNullOrEmpty(driver1.Options) && string.IsNullOrEmpty(driver2.Options)) ||
                (string.IsNullOrEmpty(driver1.Options) && !string.IsNullOrEmpty(driver2.Options)))
                return true;

            return driver1.YrsDrivingExp != driver2.YrsDrivingExp
                || driver1.YrsCommDrivingExp != driver2.YrsCommDrivingExp
                || driver1.IsMvr != driver2.IsMvr
                || driver1.YearPoint != driver2.YearPoint
                || driver1.AgeFactor != driver2.AgeFactor
                || driver1.TotalFactor != driver2.TotalFactor
                || driver1.IsOutOfState != driver2.IsOutOfState
                || driver1.MvrDate != driver2.MvrDate
                || driver1.HireDate != driver2.HireDate
                || driver1.LicenseNumber != driver2.LicenseNumber
                || driver1.State != driver2.State
                || driver1.isExcludeKO != driver2.isExcludeKO
                || driver1.koReason != driver2.koReason
                || driver1.IsValidated != driver2.IsValidated
                || driver1.IsValidatedKO != driver2.IsValidatedKO;
        }

        //public async Task<DriverDTO> UpdateAsync(DriverDTO model)
        //{
        //    try
        //    {
        //        var driver = _mapper.Map<Driver>(model);
        //        var result = (await _repository.GetAllIncludeAsync(o => o.Id == driver.Id)).FirstOrDefault();
        //        result.Entity = driver.Entity;
        //        result.YrsDrivingExp = driver.YrsDrivingExp;
        //        result.YrsCommDrivingExp = driver.YrsCommDrivingExp;
        //        result.IsMvr = driver.IsMvr;
        //        result.YearPoint = driver.YearPoint;
        //        result.AgeFactor = driver.AgeFactor;
        //        result.TotalFactor = driver.TotalFactor;
        //        result.IsOutOfState = driver.IsOutOfState;
        //        result.MvrDate = driver.MvrDate;
        //        result.HireDate = driver.HireDate;
        //        result.LicenseNumber = driver.LicenseNumber;
        //        result.Options = driver.Options;
        //        result.State = driver.State;
        //        result.isExcludeKO = driver.isExcludeKO;
        //        result.koReason = driver.koReason;
        //        result.IsValidated = driver.IsValidated;
        //        result.IsValidatedKO = driver.IsValidatedKO;
        //        var remove = result.DriverIncidents.Where(i => !driver.DriverIncidents.Select(s => s.Id).Contains(i.Id));
        //        _repositoryIncident.RemoveRange(remove);

        //        #region populate incident
        //        foreach (var item in driver.DriverIncidents)
        //        {
        //            var o = result.DriverIncidents.Where(i => i.Id == item.Id).FirstOrDefault();

        //            if (o != null)
        //            {
        //                o.IncidentType = item.IncidentType;
        //                o.IncidentDate = item.IncidentDate;
        //                o.ConvictionDate = item.ConvictionDate;
        //            }
        //            else
        //            {
        //                result.DriverIncidents.Add(item);
        //                await _repositoryIncident.AddAsync(item);
        //            }
        //        }
        //        #endregion _repositoryIncident incident

        //        _repository.Update(result);

        //        await _repository.SaveChangesAsync();

        //        return _mapper.Map<DriverDTO>(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public async Task<List<DriverDTO>> UpdateListOptionAsync(List<DriverDTO> model)
        {
            var driverList = _mapper.Map<List<Driver>>(model);
            var result = (await _repository.GetAsync(o => driverList.Select(s => s.Id).Contains(o.Id)));
            var riskDetailId = model[0].RiskDetailId;

            //Clear options if 1 is not selected
            model.ForEach(m =>
            {
                if (!m.Options?.StartsWith("1") ?? false)
                    m.Options = null;
            });

            //lightweight saving of one field only. 
            result.ToList().ForEach(o =>
            {
                o.Options = model.Where(m => m.Id == o.Id).Select(s => s.Options ?? "").FirstOrDefault() ?? "";
                _repository.Entry<Driver>(o).Property(p => p.Options).IsModified = true;
            });

            await _repository.SaveChangesAsync();

            //Update Forms
            var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);
            var bind = await _bindingService.GetBindingByRiskIdAsync(riskDetail.RiskId);
            var optionId = (bind?.BindOptionId ?? 1).ToString();
            var drivers = result.Where(d => d.IsActive != false && (d.Options == null || !d.Options.Contains(optionId)));
            var driverExclusionForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == riskDetailId && x.FormId == "DriverExclusion");
            if (driverExclusionForm != null)
            {
                driverExclusionForm.IsSelected = false;
                if (drivers.Count() > 0)
                    driverExclusionForm.IsSelected = true;
                _riskFormRepository.Update(driverExclusionForm);
                await _riskFormRepository.SaveChangesAsync();
            }

            return model;
        }


        public async Task<string> RemoveAsync(Guid id, bool fromOOS = false)
        {
            var entity = _repository.GetById(id);
            //var backupDriverIncidents = new List<DriverIncidents>();
            var riskDetail = entity.RiskDetailDrivers
                   ?.OrderByDescending(x => x.UpdatedDate)
                   ?.Select(rdv => rdv.RiskDetail)
                   ?.FirstOrDefault();
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);

            var previousRiskDetailVehicles = await _repository.GetPreviousRiskDetailDrivers(riskDetail.RiskId);
            bool isFromPreviousRiskDetail = previousRiskDetailVehicles?.Contains(entity) ?? false;

            if (isFromPreviousRiskDetail || fromOOS)
            {
                Driver newDriverVersion = entity.CloneForEndorsement();
                newDriverVersion.DriverIncidents = new List<DriverIncidents>();
                foreach (var inci in entity.DriverIncidents)
                {
                    var newInci = inci.Clone();
                    newInci.Id = System.Guid.Empty;
                    newInci.DriverId = System.Guid.Empty;
                    //inci.UniqueId = (inci is null) ? Guid.NewGuid() : inci.UniqueId;
                    newDriverVersion.DriverIncidents.Add(newInci);
                }
                newDriverVersion.Entity = entity.Entity.CloneWithNewId();
                newDriverVersion.EntityId = System.Guid.Empty;
                newDriverVersion.Entity.Id = System.Guid.Empty;
                newDriverVersion.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                entity.UnlinkLatestRiskDetail();

                await _repository.AddAsync(newDriverVersion);
                //entity.DriverIncidents = backupDriverIncidents;
            }
            else
            {
                if (entity.PreviousDriverVersionId != null)
                {
                    entity.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                }
                else
                {
                    _repository.Remove(entity); // If from submission or just added in endrosement and not yet issued, hard delete.
                }
            }

            await _repository.SaveChangesAsync();
            return riskDetail.Id.ToString();
        }

        public async Task<List<DriverDTO>> GetByRiskDetailIdIncludeAllAsync(Guid id)
        {
            var result = await _repository.GetByRiskDetailAllAsync(id);
            var drivers = new List<DriverDTO>();

            // Previous RiskDetail
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(id);
            //var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
            var previousDrivers = new List<Driver>();
            //if (previousRiskDetails.ToList().Count > 0)
            //{
            //    var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).FirstOrDefault();
            //    previousDrivers = await _repository.GetByRiskDetailAllAsync(previousRiskDetailId);
            //}
            var risk = await _riskRepository.GetRiskByIdAsync(currentRiskDetail.RiskId);
            var previousRiskDetailId = risk.CurrentRiskDetailId;
            previousDrivers = await _repository.GetByRiskDetailAllAsync(previousRiskDetailId.Value);

            foreach (var item in result)
            {
                var driver = _mapper.Map<DriverDTO>(item);
                driver.RiskDetailId = item.RiskDetailDrivers
                    ?.Select(rdv => rdv.RiskDetail)
                    ?.OrderByDescending(rdv => rdv.CreatedDate)
                    ?.FirstOrDefault()?.Id ?? Guid.Empty;

                if (previousDrivers.Count > 0 && item.DeletedDate != null)
                {
                    var isOldDeleted = previousDrivers.All(oldObj => oldObj.Id != item.Id);
                    if (!isOldDeleted)
                        driver.canReinstate = true;
                }

                drivers.Add(driver);
            }

            if (drivers.Count() > 0)
                drivers = drivers.OrderBy(x => x.CreatedDate).ToList();

            return drivers;
        }

        public async Task<List<DriverDTO>> GetByRiskDetailIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetByRiskDetailAsync(id);
            return _mapper.Map<List<DriverDTO>>(result).ToList();
        }

        public async Task<List<DriverDTO>> GetAllIncludeAsync()
        {
            var result = await _repository.GetAllIncludeAsync();
            return _mapper.Map<List<DriverDTO>>(result).ToList();

        }

        public async Task<List<DriverDTO>> GetByIdIncludeAsync(Guid id)
        {
            var result = await _repository.GetAllIncludeAsync(o => o.Id == id);
            return _mapper.Map<List<DriverDTO>>(result).ToList();
        }
        public async Task<DriverDTO> ReinstateDriverAsync(Guid driverId)
        {
            var driver = _repository.GetById(driverId);

            var riskDetail = driver.RiskDetailDrivers?.Select(rdv => rdv.RiskDetail)
                   ?.OrderByDescending(x => x.UpdatedDate)
                   ?.FirstOrDefault();
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllIncludeAsync(riskDetail.RiskId);

            Driver newDriverVersion = driver.CloneForEndorsement();
            newDriverVersion.DriverIncidents = new List<DriverIncidents>();
            foreach (var inci in driver.DriverIncidents)
            {
                var newInci = inci.Clone();
                newInci.Id = System.Guid.Empty;
                newInci.DriverId = System.Guid.Empty;
                //inci.UniqueId = (inci is null) ? Guid.NewGuid() : inci.UniqueId;
                newDriverVersion.DriverIncidents.Add(newInci);
            }
            newDriverVersion.IsActive = true;
            newDriverVersion.DeletedDate = null;
            newDriverVersion.Entity = driver.Entity.CloneWithNewId();
            newDriverVersion.EntityId = System.Guid.Empty;
            newDriverVersion.Entity.Id = System.Guid.Empty;
            newDriverVersion.EffectiveDate = riskDetail.EndorsementEffectiveDate;
            newDriverVersion.ExpirationDate = brokerInfo.ExpirationDate;
            driver.UnlinkLatestRiskDetail();

            await _repository.AddAsync(newDriverVersion);
            await _repository.SaveChangesAsync();

            return _mapper.Map<DriverDTO>(newDriverVersion);
        }

        public async Task<List<DriverDTO>> InsertRangeAsync(List<DriverDTO> model)
        {
            var drivers = _mapper.Map<List<Driver>>(model);
            if (drivers.Count > 0)
            {
                var riskDetail = await _riskDetailRepository.FindAsync(r => r.Id == model[0].RiskDetailId);
                var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllIncludeAsync(riskDetail.RiskId);

                //delete of the existing driver
                var riskDetailId = await RemoveAllDriverAsync(model[0].RiskDetailId.Value);

                foreach (var driver in drivers)
                {
                    driver.Id = Guid.NewGuid();
                    driver.CreatedDate = DateTime.Now;
                    driver.LinkRiskDetail(riskDetail);

                    driver.EffectiveDate = riskDetail?.EndorsementEffectiveDate ?? (brokerInfo?.EffectiveDate ?? DateTime.Now);
                    driver.ExpirationDate = brokerInfo?.ExpirationDate ?? DateTime.Now.AddYears(1);
                }

                await _repository.AddRangeAsync(drivers);
                await _repository.SaveChangesAsync();
            }

            return _mapper.Map<List<DriverDTO>>(drivers);
        }

        public async Task<List<DriverDTO>> GetPreviousDrivers(Guid id)
        {
            var drivers = new List<Driver>();
            var currentDriver = _repository.GetById(id);
            while (currentDriver.PreviousDriverVersionId != null)
            {
                currentDriver = _repository.GetById(currentDriver.PreviousDriverVersionId.Value);
                drivers.Add(currentDriver);
            }

            var driversDto = new List<DriverDTO>();

            foreach (var item in drivers)
            {
                var driver = _mapper.Map<DriverDTO>(item);
                driver.RiskDetailId = item.RiskDetailDrivers
                    ?.Select(rdv => rdv.RiskDetail)
                    ?.OrderByDescending(rdv => rdv.CreatedDate)
                    ?.FirstOrDefault()?.Id ?? Guid.Empty;
                driversDto.Add(driver);
            }

            return driversDto.ToList();
        }

        #region Driver Download/upload
        public async Task<List<DriverDTO>> AddFromExcelFileAsync(Stream fileStream)
        {
            //IExcelManager excelManager = new InteropExcelManager(); //Requires MS Excel installed on Web API

            IExcelManager excelManager = new NpoiExcelManager(); // Check if will work without MS Excel installed
            excelManager.HeaderMap = HeaderMap;

            return await Task.Run(() => excelManager.AddDrivers(fileStream));
        }

        private List<PropertyHeader> _headerMap;
        public List<PropertyHeader> HeaderMap
        {
            get
            {
                return _headerMap ??= new List<PropertyHeader>
                {
                    //new PropertyHeader {PropertyType = typeof(string), PropertyName = "PolicyNumber", Header = "Policy Number"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "FirstName", Header = "First Name"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "MiddleName", Header = "Middle Name"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "LastName", Header = "Last Name"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "LicenseNumber", Header = "DL Number"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "StateFull", Header = "License State"},
                    new PropertyHeader {PropertyType = typeof(DateTime), PropertyName = "BirthDate", Header = "DOB"},
                    new PropertyHeader {PropertyType = typeof(short), PropertyName = "YrsDrivingExp", Header = "Years Driving Exp"},
                    new PropertyHeader {PropertyType = typeof(short), PropertyName = "YrsCommDrivingExp", Header = "Years Commercial Driving Exp"},
                };
            }
        }
        #endregion  Driver Download/upload
        #region DriverHeader


        public async Task<DriverHeaderDTO> GetHeaderAsync(Guid riskDetailId)
        {
            var header = await _repositoryHeader.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (!header.Any())
                return null;
            return _mapper.Map<DriverHeaderDTO>(header.First());
        }

        public async Task<DriverHeaderDTO> AddUpdateAsync(DriverHeaderDTO model)
        {
            var entity = (await _repositoryHeader.GetAsync(x => x.RiskDetailId == model.RiskDetailId)).FirstOrDefault();

            if (entity == null)
            {
                model.accDriverFactor = null;
                model.driverVehicleRatio = null;
                entity = await _repositoryHeader.AddAsync(_mapper.Map<DriverHeader>(model));
            }
            else
            {
                model.Id = entity.Id;
                model.accDriverFactor = entity.accDriverFactor != null ? entity.accDriverFactor : null;
                model.driverVehicleRatio = entity.driverVehicleRatio != null ? entity.driverVehicleRatio : null;
                entity = _repositoryHeader.Update(_mapper.Map<DriverHeader>(model));
            }
            await _repository.SaveChangesAsync();
            return _mapper.Map<DriverHeaderDTO>(entity);
        }

        #endregion DriverHeader

        public async Task<List<DriverDTO>> GetDriversByLastNameAndLicenseNumberAsync(string lastName, string licenseNumber)
        {
            var result = await _repository.GetDriversByLastNameAndLicenseNumberAsync(lastName, licenseNumber);

            List<DriverDTO> driverDTOs = _mapper.Map<List<DriverDTO>>(result.ToList());

            foreach (var driverDTO in driverDTOs)
            {
                var riskDetailId = result.SelectMany(x => x.RiskDetailDrivers)
                    .Where(x => x.DriverId == driverDTO.Id)
                    ?.OrderByDescending(rdv => rdv.UpdatedDate)
                    ?.Select(rdv => rdv.RiskDetailId)
                    ?.FirstOrDefault() ?? Guid.Empty;
                var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);
                var risk = await _riskRepository.GetRiskByIdAsync(riskDetail.RiskId);

                driverDTO.PolicyNumber = risk.PolicyNumber;
                driverDTO.SubmissionNumber = risk.SubmissionNumber;
            }

            return driverDTOs;
        }

        public async Task UpdateDriverEffectiveDateExpirationDate(Guid riskDetailId, DateTime effectiveDate, DateTime expirationDate)
        {
            var drivers = await _repository.GetByRiskDetailAsync(riskDetailId);

            if (drivers?.Count() > 0)
            {
                drivers = drivers.Select(c => { c.EffectiveDate = effectiveDate; c.ExpirationDate = expirationDate; return c; }).ToList();

                _repository.UpdateRange(drivers);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<DateTime?> GetEffectiveDateFromPreviousRiskDetail(DriverDTO driver, Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var previousDrivers = await _repository.GetByRiskDetailAllAsync(risk.CurrentRiskDetailId.Value);
            var previousDriver = previousDrivers.FirstOrDefault(x => x.Id == driver.Id);
            return previousDriver?.EffectiveDate;
        }

        public async Task<DateTime?> GetDriverEffectiveDate(DriverDTO driver, Guid currentRiskDetailId)
        {
            var previousDriver = await _repository.GetByRiskDetailAndIdAsync(currentRiskDetailId, driver.Id);
            return previousDriver?.EffectiveDate;
        }

        public async Task UpdateDriverExclusionEndorsementFormAsync(Guid riskDetailId)
        {
            await _driverExclusionEndorsementFormService.Update(riskDetailId);
        }


        public async Task ClearFactors(Guid riskDetailId)
        {
            var drivers = await _repository.GetByRiskDetailAsync(riskDetailId);
            foreach (var driver in drivers)
            {
                driver.ClearFactors();
            }
            await _repository.SaveChangesAsync();

            var driverHeader = await _repositoryHeader.FindAsync(x => x.RiskDetailId == riskDetailId);
            driverHeader.accDriverFactor = null;
            driverHeader.driverVehicleRatio = null;
            await _repositoryHeader.SaveChangesAsync();
        }

        public async Task<string> RemoveAllDriverAsync(Guid id, bool fromOOS = false)
        {
            var driverremoval = await _repository.GetByRiskDetailAsync(id);
            foreach (var forremoval in driverremoval)
            {
                var entity = _repository.GetById(forremoval.Id);
                //var backupDriverIncidents = new List<DriverIncidents>();
                var riskDetail = entity.RiskDetailDrivers?.Select(rdv => rdv.RiskDetail)
                       ?.OrderByDescending(x => x.UpdatedDate)
                       ?.FirstOrDefault();
                var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);

                var previousRiskDetailVehicles = await _repository.GetPreviousRiskDetailDrivers(riskDetail.RiskId);
                bool isFromPreviousRiskDetail = previousRiskDetailVehicles?.Contains(entity) ?? false;

                if (isFromPreviousRiskDetail || fromOOS)
                {
                    Driver newDriverVersion = entity.CloneForEndorsement();
                    newDriverVersion.DriverIncidents = new List<DriverIncidents>();
                    foreach (var inci in entity.DriverIncidents)
                    {
                        var newInci = inci.Clone();
                        newInci.Id = System.Guid.Empty;
                        newInci.DriverId = System.Guid.Empty;
                        //inci.UniqueId = (inci is null) ? Guid.NewGuid() : inci.UniqueId;
                        newDriverVersion.DriverIncidents.Add(newInci);
                    }
                    newDriverVersion.Entity = entity.Entity.CloneWithNewId();
                    newDriverVersion.EntityId = System.Guid.Empty;
                    newDriverVersion.Entity.Id = System.Guid.Empty;
                    newDriverVersion.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                    entity.UnlinkLatestRiskDetail();

                    await _repository.AddAsync(newDriverVersion);
                    //entity.DriverIncidents = backupDriverIncidents;
                }
                else
                {
                    if (entity.PreviousDriverVersionId != null)
                    {
                        entity.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                    }
                    else
                    {
                        _repository.Remove(entity); // If from submission or just added in endrosement and not yet issued, hard delete.
                    }
                }

                await _repository.SaveChangesAsync();

            }

            //Uncheck driver exclusion form
            var driverExclusionForm = await _riskFormRepository.FindAsync(x => x.RiskDetailId == id && x.FormId == "DriverExclusion");
            if (driverExclusionForm != null)
            {
                var riskDetail = await _riskDetailRepository.FindAsync(r => r.Id == id);
                var isNewDriverExclusionRuleApplicable = await IsFeatureApplicable(riskDetail.RiskId, "S37KORulesUpdate");
                driverExclusionForm.IsSelected = !isNewDriverExclusionRuleApplicable ? true : false;
                _riskFormRepository.Update(driverExclusionForm);
                await _riskFormRepository.SaveChangesAsync();
            }

            return id.ToString();
        }
        public async Task<List<DriverDTO>> GetByRiskDetailIdIncludeAllExportAsync(Guid id)
        {
            var result = await _repository.GetByRiskDetailAllAsync(id);
            var drivers = new List<DriverDTO>();

            // Previous RiskDetail
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(id);
            //var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetail.Id);
            var previousDrivers = new List<Driver>();

            var risk = await _riskRepository.GetRiskByIdAsync(currentRiskDetail.RiskId);
            var previousRiskDetailId = risk.CurrentRiskDetailId;
            previousDrivers = await _repository.GetByRiskDetailAllAsync(previousRiskDetailId.Value);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(currentRiskDetail.RiskId);



            //IEnumerable<DriverDTO> driverHistory = _mapper.Map<List<DriverDTO>>(previousDrivers);
            var driverHistory = new List<DriverDTO>();

            foreach (var item in result)
            {
                var driver = _mapper.Map<DriverDTO>(item);
                driver.RiskDetailId = item.RiskDetailDrivers
                    ?.Select(rdv => rdv.RiskDetail)
                    ?.OrderByDescending(rdv => rdv.CreatedDate)
                    ?.FirstOrDefault()?.Id ?? Guid.Empty;
                driver.EffectiveDate = driver.EffectiveDate ?? (brokerInfo?.EffectiveDate ?? currentRiskDetail?.EndorsementEffectiveDate);
                driver.ExpirationDate = driver.ExpirationDate ?? brokerInfo?.ExpirationDate;

                if (previousDrivers.Count > 0)
                {
                    driverHistory = await GetPreviousExportDrivers(item.Id, brokerInfo?.EffectiveDate, currentRiskDetail?.EndorsementEffectiveDate, driver.RiskDetailId);

                    drivers.AddRange(driverHistory);
                }

                drivers.Add(driver);
            }

            if (drivers.Count() > 0)
                drivers = drivers.OrderBy(x => x.LastName).ThenByDescending(x => x.CreatedDate).ThenByDescending(x => x.IsActive).ToList();

            return drivers;
        }

        public async Task<List<DriverDTO>> GetPreviousExportDrivers(Guid id, DateTime? effectivedate, DateTime? expirationdate, Guid? RiskDetailId)
        {
            var drivers = new List<Driver>();
            var currentDriver = _repository.GetById(id);
            while (currentDriver.PreviousDriverVersionId != null)
            {
                currentDriver = _repository.GetById(currentDriver.PreviousDriverVersionId.Value);
                drivers.Add(currentDriver);
            }

            var driversDto = new List<DriverDTO>();


            foreach (var item in drivers)
            {
                var driver = _mapper.Map<DriverDTO>(item);
                RiskDetailId = item.RiskDetailDrivers
                    ?.Select(rdv => rdv.RiskDetail)
                    ?.OrderByDescending(rdv => rdv.CreatedDate)
                    ?.FirstOrDefault()?.Id ?? Guid.Empty;


                driver.EffectiveDate = driver.EffectiveDate ?? effectivedate;
                driver.ExpirationDate = driver.ExpirationDate ?? currentDriver.ExpirationDate ?? expirationdate;

                driversDto.Add(driver);
            }

            return driversDto.ToList();
        }

        private async Task<bool> IsFeatureApplicable(Guid RiskId, string fieldName)
        {
            var fieldVersions = await _riskRepository.GetFieldVersions();
            var submissionRiskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(RiskId);
            var risk = await _riskRepository.FindAsync(RiskId);
            var isApplicable = fieldVersions.Any(v => v.FieldName == fieldName && submissionRiskDetail.RequestedQuoteDate >= v.EffectiveDate);

            if (submissionRiskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                return fieldVersions.Any(v => v.FieldName == fieldName);

            return isApplicable;
        }
    }
}
