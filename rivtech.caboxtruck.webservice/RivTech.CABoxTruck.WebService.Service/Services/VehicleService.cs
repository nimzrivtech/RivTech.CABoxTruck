using AutoMapper;
using ClosedXML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Vehicles;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.ExcelManager;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehiclePremiumRepository _vehiclePremiumRepository;
        private readonly ILvLimitsRepository _lvLimitsRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IRiskVehiclePremiumService _riskVehiclePremiumService;
        private readonly IMapper _mapper;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskVehiclePremiumTransactionRepository _riskVehiclePremiumTransactionRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IAddressRepository _addressRepository;
        private readonly IVehiclePremiumRatingFactorRepository _vehiclePremiumRatingFactorRepository;
        private readonly ILimitsService _limitsService;
        private readonly IRiskFormRepository _riskFormRepository;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            IVehiclePremiumRepository vehiclePremiumRepository,
            ILvLimitsRepository lvLimitsRepository,
            IRiskDetailRepository riskDetailRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IPolicyHistoryRepository policyHistoryRepository,
            IRiskCoverageRepository riskCoverageRepository,
            IRiskVehiclePremiumService riskVehiclePremiumService,
            IMapper mapper,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskVehiclePremiumTransactionRepository riskVehiclePremiumTransactionRepository,
            IRiskRepository riskRepository,
            IEndorsementChangesService endorsementChangesService,
            IAddressRepository addressRepository,
            IVehiclePremiumRatingFactorRepository vehiclePremiumRatingFactorRepository,
            IRiskFormRepository riskFormRepository
        )
        {
            _vehicleRepository = vehicleRepository;
            _vehiclePremiumRepository = vehiclePremiumRepository;
            _lvLimitsRepository = lvLimitsRepository;
            _riskDetailRepository = riskDetailRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _policyHistoryRepository = policyHistoryRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _riskVehiclePremiumService = riskVehiclePremiumService;
            _mapper = mapper;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskVehiclePremiumTransactionRepository = riskVehiclePremiumTransactionRepository;
            _riskRepository = riskRepository;
            _endorsementChangesService = endorsementChangesService;
            _addressRepository = addressRepository;
            _vehiclePremiumRatingFactorRepository = vehiclePremiumRatingFactorRepository;
            _riskFormRepository = riskFormRepository;
        }

        public async Task<List<VehicleDTO>> GetAllAsync()
        {
            var result = await _vehicleRepository.GetAllAsync();

            foreach (var vehicle in result)
            {
                vehicle.RiskVehiclePremiumTransaction = await _riskVehiclePremiumTransactionRepository.GetAsync(vehicle.RiskDetailId, vehicle.MainId);
            }

            return _mapper.Map<List<VehicleDTO>>(result);
        }

        public async Task<List<VehicleDTO>> GetByRiskDetailIdAsync(Guid id)
        {
            var result = await _vehicleRepository.GetByRiskDetailAsync(id);

            foreach (var vehicle in result)
            {
                vehicle.RiskVehiclePremiumTransaction = await _riskVehiclePremiumTransactionRepository.GetAsync(vehicle.RiskDetailId, vehicle.MainId);
            }

            return result.Select(obj => _mapper.Map<VehicleDTO>(obj)).ToList();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _vehicleRepository.AnyAsync(x => x.Id == id);
        }

        public async Task<VehicleDTO> GetByIdAsync(Guid id)
        {
            var result = await _vehicleRepository.FindAsync(x => x.Id == id);

            result.RiskVehiclePremiumTransaction = await _riskVehiclePremiumTransactionRepository.GetAsync(result.RiskDetailId, result.MainId);

            return _mapper.Map<VehicleDTO>(result);
        }

        public async Task<VehicleDTO> InsertAsync(VehicleDTO model)
        {
            try
            {
                this.ValidateModel(model);
                var vehicle = _mapper.Map<Vehicle>(model);
                vehicle.Id = Guid.NewGuid();
                if (vehicle.MainId == Guid.Empty)
                    vehicle.MainId = Guid.NewGuid();
                vehicle.CreatedDate = DateTime.Now;
                await _vehicleRepository.AddAsync(vehicle);

                // Link risk detail.
                var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync((Guid)model.RiskDetailId);
                vehicle.LinkRiskDetail(riskDetail);

                var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
                vehicle.EffectiveDate = riskDetail?.EndorsementEffectiveDate ?? (brokerInfo?.EffectiveDate ?? DateTime.Now);
                vehicle.ExpirationDate = brokerInfo?.ExpirationDate ?? DateTime.Now.AddYears(1);
                vehicle.SetCompAndColIsManual(null, riskDetail?.RiskCoverages[0], true);
                //vehicle.VehicleProratedPremium = new VehicleProratedPremium(vehicle);

                //await UpdateDeductibles(vehicle, riskDetail);
                await _vehicleRepository.SaveChangesAsync();
                await UpdateVehicleTypeCount((Guid)model.RiskDetailId);

                return _mapper.Map<VehicleDTO>(vehicle);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VehicleDTO>> InsertRangeAsync(List<VehicleDTO> model)
        {
            var riskDetailId = model[0].RiskDetailId;
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync((Guid)riskDetailId);

            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);

            //remove saved vehicle to be overwrite
            var vinremoval = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId.Value, false);

            foreach (var forremoval in vinremoval)
            {
                //deletion
                _vehicleRepository.Remove(forremoval); // If from submission or just added in endrosement and not yet issued, hard delete.
                var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(riskDetail.Id, forremoval.MainId);
                if (riskVehiclePremiums != null)
                {
                    await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
                }
                await _vehicleRepository.SaveChangesAsync();
            }
            var vehicles = _mapper.Map<List<Vehicle>>(model);
            List<Vehicle> processedVehicles = new();

            foreach (var vehicle in vehicles)
            {
                

                vehicle.Id = Guid.NewGuid();
                vehicle.MainId = Guid.NewGuid();
                vehicle.CreatedDate = DateTime.Now;
                vehicle.CreatedBy = Data.Common.Services.GetCurrentUser();
                vehicle.LinkRiskDetail(riskDetail);
                //await UpdateDeductibles(vehicle, riskDetail);

                vehicle.EffectiveDate = riskDetail?.EndorsementEffectiveDate ?? (brokerInfo?.EffectiveDate ?? DateTime.Now);
                vehicle.ExpirationDate = brokerInfo?.ExpirationDate ?? DateTime.Now.AddYears(1);
                vehicle.IsValidated = vehicles.Where(x => x.VIN == vehicle.VIN & x.RegisteredState == vehicle.RegisteredState).Count() > 1 ? false : vehicle.IsValidated;
                vehicle.CoverageCollisionDeductibleId = vehicle.IsCoverageCollision == true ? vehicle.CoverageCollisionDeductibleId : null;
                vehicle.CoverageFireTheftDeductibleId = vehicle.IsCoverageFireTheft == true ? vehicle.CoverageFireTheftDeductibleId : null;
                vehicle.SetCompAndColIsManual(null, riskDetail?.RiskCoverages[0], true);
                //vehicle.IsValidated = vehicles.Where(x => x.VIN == vehicle.VIN & x.RegisteredState == vehicle.RegisteredState).Count() > 1 ? false : true;
                //var vehicleProratedPremium = new VehicleProratedPremium();
                //vehicleProratedPremium.Id = Guid.NewGuid();
                //vehicle.VehicleProratedPremium = vehicleProratedPremium;
                //vehicle.VehicleProratedPremiumId = vehicle.VehicleProratedPremium.Id;
                //vehicle.VehicleProratedPremium.VehicleId = vehicle.Id;
                //_vehicleRepository.Entry(vehicleProratedPremium).State = EntityState.Added;

            }

          

            await _vehicleRepository.AddRangeAsync(vehicles);
            await _vehicleRepository.SaveChangesAsync();
            await UpdateVehicleTypeCount((Guid)riskDetailId);

            return _mapper.Map<List<VehicleDTO>>(vehicles);
        }

        public async Task<VehicleDTO> UpdateAsync(VehicleDTO dto, bool updateCoverage = false, RiskCoverageDTO oldLimitsData = null)
        {
            try
            {
                if (dto?.Id is null)
                    throw new ArgumentException("Vehicle has no ID.");

                this.ValidateModel(dto);
                Vehicle vehicle = _mapper.Map<Vehicle>(dto);
                Vehicle savedVehicle = await _vehicleRepository.GetByIdAsync(dto.Id.Value);
                vehicle.MainId = savedVehicle.MainId;

                vehicle.PreviousVehicleVersionId = savedVehicle?.PreviousVehicleVersionId;
                vehicle.PreviousVehicleVersion = savedVehicle?.PreviousVehicleVersion;
                vehicle.RiskDetailVehicles = savedVehicle?.RiskDetailVehicles;
                vehicle.VehiclePremiums = savedVehicle?.VehiclePremiums;
                vehicle.VehiclePremiumRatingFactor = savedVehicle?.VehiclePremiumRatingFactor;

                var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(vehicle.RiskDetailId);
                vehicle.SetCompAndColIsManual(savedVehicle, riskDetail?.RiskCoverages[0]);

                if (updateCoverage) vehicle.Radius = savedVehicle?.Radius; // No Radius changes for Coverage updates

                // Get vehicles from previous risk
                var versionFromPreviousRiskDetail = await _vehicleRepository.GetVersionFromPreviousRiskDetailVersion(savedVehicle?.LatestRiskDetail.Id, vehicle.MainId);
                bool isSameVersionWithPreviousRiskDetail = versionFromPreviousRiskDetail?.Id == savedVehicle.Id;

                // Previous vehicle version
                Vehicle previousVersion = null;
                if (savedVehicle.PreviousVehicleVersionId != null)
                    previousVersion = await _vehicleRepository.GetByIdAsync(savedVehicle.PreviousVehicleVersionId.Value);

                bool isNew = savedVehicle.PreviousVehicleVersionId is null;
                bool justReinstated = (previousVersion?.IsDeleted ?? false) && !vehicle.IsDeleted;
                bool justDeleted = !(previousVersion?.IsDeleted ?? false) && vehicle.IsDeleted;
                bool shouldCreateNewVersion = dto.isEndorsement == true && isSameVersionWithPreviousRiskDetail && (savedVehicle?.HasChanges(vehicle) ?? false);

                //Update vehicle and reefer
                if (oldLimitsData != null)
                {
                    if (oldLimitsData.CargoLimitId != null && oldLimitsData.CargoLimitId != LimitConstants.CargoNoneCoverageId && savedVehicle.HasCoverageCargo == false)
                        vehicle.HasCoverageCargo = false;

                    if (oldLimitsData.RefCargoLimitId != null && oldLimitsData.RefCargoLimitId != LimitConstants.RefNoneCoverageId && savedVehicle.HasCoverageRefrigeration == false)
                        vehicle.HasCoverageRefrigeration = false;
                }

                bool shouldRollbackVersion = dto.isEndorsement == true && !isNew && !isSameVersionWithPreviousRiskDetail && !(versionFromPreviousRiskDetail?.HasChanges(vehicle) ?? false)
                                            && !justReinstated && !justDeleted;

                Vehicle updatedVehicle;
                if (shouldCreateNewVersion)
                {
                    //var vehicleRatingFactor = _vehicleRepository.GetRatingFactors(vehicle.Id, vehicle.LatestRiskDetail.Id);
                    // If update is from endorsement with no new version and has changes,
                    // then create a new version with new changes.
                    Vehicle newVehicleVersion = vehicle.CloneForEndorsement();
                    newVehicleVersion = UpdateLimitsData(newVehicleVersion);
                    savedVehicle.UnlinkLatestRiskDetail();
                    await _vehicleRepository.SaveChangesAsync();
                    _vehicleRepository.Detach(savedVehicle);

                    updatedVehicle = await _vehicleRepository.AddAsync(newVehicleVersion);
                    await _vehiclePremiumRepository.SaveChangesAsync();
                }
                else if (shouldRollbackVersion)
                {
                    // If update is from endorsement with new version already but no difference to previous,
                    // Then remove the new version and re-assign the previous version to current riskdetail.
                    var currentRiskDetail = savedVehicle.UnlinkLatestRiskDetail();
                    _vehicleRepository.Remove(savedVehicle);

                    var vehiclePremium = await _vehiclePremiumRepository.GetByRiskDetailVehicleIdAsync(currentRiskDetail.RiskDetailId, savedVehicle.Id);
                    if (vehiclePremium != null)
                        _vehiclePremiumRepository.Remove(vehiclePremium);

                    // Link back the previous vehicle to current riskdetail.
                    previousVersion.LinkRiskDetail(currentRiskDetail.RiskDetail);

                    updatedVehicle = previousVersion;
                    await _vehicleRepository.SaveChangesAsync();
                }
                else if (savedVehicle.HasChanges(vehicle)) // If from submission or no need to update versions, just update.
                {
                    vehicle = UpdateLimitsData(vehicle);

                    // Detached the same vehicle queried stored vehicle in db to avoid conflict when updating dbcontext.
                    _vehicleRepository.Detach(savedVehicle);

                    updatedVehicle = _vehicleRepository.Update(vehicle);
                    await _vehicleRepository.SaveChangesAsync();
                }
                else
                {
                    updatedVehicle = savedVehicle;
                }


                await UpdateVehicleTypeCount(vehicle.RiskDetailId);

                return _mapper.Map<VehicleDTO>(updatedVehicle);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateVehicleEffectiveDateAsync(VehicleDTO dto)
        {
            Vehicle vehicle = _mapper.Map<Vehicle>(dto);
            Vehicle savedVehicle = await _vehicleRepository.GetByIdAsync(dto.Id.Value);

            if (savedVehicle.IsEffectiveExpirationDateChanged(vehicle))
            {
               // Detached the same vehicle queried stored vehicle in db to avoid conflict when updating dbcontext.
                _vehicleRepository.Detach(savedVehicle);

                _vehicleRepository.Update(vehicle);
                await _vehicleRepository.SaveChangesAsync();
            }
        }
        public async Task UpdateVehicleHeader(VehicleHeaderDTO dto)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == dto.RiskDetailId);
            var risk = await _riskRepository.FindAsync(risk => risk.Id == riskDetail.RiskId);

            if (risk.Status != RiskStatus.Active)
            {
                if (riskDetail != null)
                {
                    var previousMainUseId = riskDetail.MainUseId;

                    riskDetail.MainUseId = dto.MainUseId.HasValue ? dto.MainUseId.Value : null;
                    riskDetail.MainGVWId = dto.MainGVWId.HasValue ? dto.MainGVWId.Value : null;
                    riskDetail.AccountCategoryId = dto.AccountCategoryId.HasValue ? dto.AccountCategoryId.Value : null;
                    riskDetail.AccountCategoryUWId = dto.AccountCategoryUWId.HasValue ? dto.AccountCategoryUWId.Value : null;
                    riskDetail.SecondaryBusinessGroupId = dto.SecondaryBusinessGroupId.HasValue ? dto.SecondaryBusinessGroupId.Value : null;
                    var result = _riskDetailRepository.Update(riskDetail);
                    await _riskDetailRepository.SaveChangesAsync();

                    // Update Courier Form
                    var form = await _riskFormRepository.FindAsync(x => x.RiskDetailId == riskDetail.Id && x.FormId == "CourierCoverageEndorsement");
                    if (form != null)
                    {
                        if (riskDetail.MainUseId == 1 && previousMainUseId != 1) form.IsSelected = true;
                        else if (riskDetail.MainUseId == 1 && form.IsSelected == true) form.IsSelected = true;
                        else form.IsSelected = false;

                        _riskFormRepository.Update(form);
                        await _riskFormRepository.SaveChangesAsync();
                    }
                }

                var riskCoverage = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == dto.RiskDetailId && x.OptionNumber == dto.OptionNumber);

                if (riskCoverage != null)
                {
                    riskCoverage.TrailerTypeLimitId = dto.TrailerTypeLimitId.HasValue ? dto.TrailerTypeLimitId.Value : null;
                    var result = _riskCoverageRepository.Update(riskCoverage);
                    await _riskCoverageRepository.SaveChangesAsync();
                } 
            }
            else
            {
                throw new ArgumentException("Active Policy's vehicle headers cannot be modified. Please contact an administrator.");
            }
        }

        private async Task UpdateVehicleTypeCount(Guid RiskDetailID)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(RiskDetailID);
            var cntVehtype = await _vehicleRepository.GetByRiskDetailAsync(RiskDetailID);
            riskDetail.CountPowerUnit = cntVehtype.Where(x => x.VehicleType == "Power Unit").Count();
            riskDetail.CountTrailer = cntVehtype.Where(x => x.VehicleType == "Trailer").Count();
            riskDetail.CountOthers = cntVehtype.Where(x => x.VehicleType == "Other").Count();
            await _riskDetailRepository.SaveChangesAsync();

        }
        public Task<bool> IsOldVersion(Guid vehicleId)
        {
            return Task.FromResult(_vehicleRepository.IsOldVersion(vehicleId));
        }

        private Vehicle UpdateLimitsData(Vehicle v)
        {
            if (!(v.IsCoverageCollision ?? false))
                v.CoverageCollisionDeductibleId = null;

            if (!(v.IsCoverageFireTheft ?? false))
                v.CoverageFireTheftDeductibleId = null;

            if (!(v.HasCoverageCargo ?? false))
                v.HasCoverageRefrigeration = false;
            return v;
        }

        private async Task UpdateDeductibles(VehicleDTO model, RiskCoverage riskCoverage, RiskCoverageDTO oldLimitsData, bool hasAllNoCargo, bool hasAllNoRef)
        {
            if (model.DeletedDate != null)
                return; // Cannot update coverage of deleted vehicle.

            #region Comp FT Limit

            if (riskCoverage.HasCompFtLimit())
            {
                bool oldLimitMatchesVehicle = oldLimitsData.CompFTDeductibleId == model.CoverageFireTheftDeductibleId;
                bool bothOldLimitAndVehicleHaveNoCompFt = !oldLimitsData.HasCompFtLimit() && !model.HasCompFtLimit();

                if ((oldLimitMatchesVehicle || bothOldLimitAndVehicleHaveNoCompFt) && !(model.IsManualCoverageFtLimit ?? false))
                {
                    model.SetCompFtDeductible(riskCoverage.CompFTDeductibleId);

                    if(bothOldLimitAndVehicleHaveNoCompFt)
                        model.IsManualCoverageFtLimit = null;
                }
                else
                {
                    model.IsManualCoverageFtLimit = true;
                }
            }
            else
            {
                model.ClearCompFtCoverage();
            }
            #endregion

            #region Collison Limit
            if(riskCoverage.HasCollisionLimit())
            {
                var oldLimitMatchesVehicle = (model.CoverageCollisionDeductibleId == oldLimitsData.CollDeductibleId);
                var bothOldLimitAndVehicleHaveNoCollision = (!oldLimitsData.HasCollisionLimit() && !model.HasCollisionLimit());

                if ((oldLimitMatchesVehicle || bothOldLimitAndVehicleHaveNoCollision) && !(model.IsManualCollisionLimit ?? false))
                {
                    model.SetCollisionDedutible(riskCoverage.CollDeductibleId);

                    if (bothOldLimitAndVehicleHaveNoCollision)
                        model.IsManualCollisionLimit = null;
                }
                else
                {
                    model.IsManualCollisionLimit = true;
                }
            }
            else
            {
                model.ClearCollisionCoverage();
            }
            #endregion

            var cargoLimits = await _lvLimitsRepository.GetAsync(x => x.LimitType.Equals(LimitType.Cargo));
            var cargoNoCoverage = cargoLimits.SingleOrDefault(x => x.LimitValue.Equals("NONE"));
            bool limitHasCargo = (riskCoverage.CargoLimitId != null) && (riskCoverage.CargoLimitId != cargoNoCoverage.Id);

            var refLimits = await _lvLimitsRepository.GetAsync(x => x.LimitType.Equals(LimitType.RefBreakdown));
            var refNoCoverage = refLimits.SingleOrDefault(x => x.LimitValue.Equals("NONE"));
            bool limitHasRef = (riskCoverage.RefCargoLimitId != null) && (riskCoverage.RefCargoLimitId != refNoCoverage.Id);
        }

        public async Task<List<VehicleDTO>> UpdateAddressesAsync(List<VehicleDTO> model)
        {
            foreach (var vehicleDTO in model)
            {
                await UpdateAsync(vehicleDTO);
            }
            //var vehicles = await _vehicleRepository.GetAsync(o => model.Select(s => s.Id).Contains(o.Id));

            //vehicles.ToList().ForEach(v =>
            //{
            //    var updatingVehicle = model.FirstOrDefault(m => m.Id == v.Id);
            //    v.GaragingAddressId = updatingVehicle?.GaragingAddressId;
            //    v.PrimaryOperatingZipCode = updatingVehicle?.PrimaryOperatingZipCode;
            //    _vehicleRepository.Entry(v).Property(p => p.GaragingAddressId).IsModified = true;
            //    _vehicleRepository.Entry(v).Property(p => p.PrimaryOperatingZipCode).IsModified = true;
            //});

            //await _vehicleRepository.SaveChangesAsync();
            return model;
        }

        public async Task<List<VehicleDTO>> UpdateOtherCoveragesAsync(List<VehicleDTO> vehicleList, RiskCoverageDTO oldLimitsData)
        {
            if (vehicleList.Count == 0)
                return await Task.FromResult(vehicleList);

            //var riskCoverage = await _riskCoverageRepository.GetRiskCoverageBySelectedBindOption(vehicleList[0].RiskDetailId.Value);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(oldLimitsData.RiskDetailId);
            var riskCoverage = riskDetail.RiskCoverages[0]; // Get option 1 RiskCoverage

            var activeVehicles = vehicleList.Where(x => x.DeletedDate == null);

            var vehicleNoCargoCount = activeVehicles.Count(x => x.HasCoverageCargo != true);
            var vehicleNoRefCount = activeVehicles.Count(x => x.HasCoverageRefrigeration != true);

            bool hasAllNoCargo = activeVehicles.Count() == vehicleNoCargoCount;
            bool hasAllNoRef = activeVehicles.Count() == vehicleNoRefCount;

            foreach (var vehicle in vehicleList)
            {
                await UpdateDeductibles(vehicle, riskCoverage, oldLimitsData, hasAllNoCargo, hasAllNoRef);
                await UpdateAsync(vehicle, true, oldLimitsData);
            }

            await _vehicleRepository.SaveChangesAsync();
            return vehicleList;
        }

        public async Task<List<VehicleDTO>> UpdateListOptionAsync(List<VehicleDTO> model)
        {
            var vehicleList = _mapper.Map<List<Vehicle>>(model);
            var result = (await _vehicleRepository.GetAsync(o => vehicleList.Select(s => s.Id).Contains(o.Id)));

            //lightweight saving of one field only. 
            result.ToList().ForEach(o =>
            {
                o.Options = model.Where(m => m.Id == o.Id).Select(s => s.Options ?? "").FirstOrDefault() ?? "";
                _vehicleRepository.Entry<Vehicle>(o).Property(p => p.Options).IsModified = true;
            });

            await _vehicleRepository.SaveChangesAsync();
            return model;
        }

        public async Task<string> RemoveAsync(Guid id, bool? fromEndorsement = false)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(vehicle.LatestRiskDetail.Id);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);

            // Get vehicles from previous risk
            IEnumerable<Vehicle> previousRiskDetailVehicles = null;
            if (riskDetail.SourceRiskDetailId.HasValue)
                previousRiskDetailVehicles = (await _vehicleRepository.GetByRiskDetailAsync(riskDetail.SourceRiskDetailId.Value, true)) ?? new List<Vehicle>();
            bool isFromPreviousRiskDetail = previousRiskDetailVehicles?.Contains(vehicle) ?? false;

            if (isFromPreviousRiskDetail) // If vehicle version same with previous risk detail, create new version
            {
                Vehicle newVehicleVersion = vehicle.CloneForEndorsement();
                newVehicleVersion.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                await _vehicleRepository.AddAsync(newVehicleVersion);

                var rdv = vehicle.UnlinkLatestRiskDetail();

                var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, rdv.RiskDetailId, vehicle.MainId);
                if (riskVehiclePremiums != null)
                {
                    await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
                }
                _vehicleRepository.Update(vehicle);
            }
            else
            {
                if (vehicle.PreviousVehicleVersionId != null) // If not same with previous riskdetail but with previous version, just soft  delete .
                {
                    Vehicle deletedVehicle = vehicle;
                    if (vehicle.PreviousVehicleVersion?.HasChanges(vehicle) ?? false)
                    {
                        // If changes has been made on new version, hard delete it and
                        // just clone the previous version to revert changes.
                        var latestRiskdetailVehicle = vehicle.UnlinkLatestRiskDetail();
                        // The false param here is to prevent linking to previous risk detail vehicle
                        deletedVehicle = vehicle.PreviousVehicleVersion.CloneForEndorsement(false);
                        deletedVehicle.LinkRiskDetail(latestRiskdetailVehicle.RiskDetail);
                        await _vehicleRepository.AddAsync(deletedVehicle);
                        await _vehiclePremiumRepository.SaveChangesAsync();

                        var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, latestRiskdetailVehicle.RiskDetailId, vehicle.MainId);
                        if (riskVehiclePremiums != null)
                        {
                            await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
                        }
                        _vehicleRepository.Remove(vehicle);
                    }
                    deletedVehicle.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                    _vehicleRepository.Update(deletedVehicle);
                }
                else
                {
                    // Hard delete vehicles which are added but have not been issued previously.
                    _vehicleRepository.Remove(vehicle); // If from submission or just added in endrosement and not yet issued, hard delete.
                    var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.Id, vehicle.MainId);
                    if (riskVehiclePremiums != null)
                    {
                        await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
                    }
                }
            }

            
            await _vehicleRepository.SaveChangesAsync();
            await UpdateVehicleTypeCount(vehicle.LatestRiskDetail.Id);
           
            return riskDetail.Id.ToString();
        }


        public async Task<VehicleDTO> ReinstateVehicleAsync(Guid vehicleId)
        {
            var vehicle = _vehicleRepository.GetById(vehicleId);
            if (!vehicle.IsDeleted)
                throw new InvalidOperationException("Cannot reinstate an active vehicle.");

            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(vehicle.RiskDetail.Id);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllIncludeAsync(riskDetail.RiskId);

            var reinstatedClone = vehicle.CloneForEndorsement();
            reinstatedClone.Reinstate();
            reinstatedClone.EffectiveDate = riskDetail.EndorsementEffectiveDate;
            reinstatedClone.ExpirationDate = brokerInfo.ExpirationDate;
            await _vehicleRepository.AddAsync(reinstatedClone);

            vehicle.UnlinkLatestRiskDetail();

            var vehiclePremium = await _vehiclePremiumRepository.GetByRiskDetailVehicleIdAsync(riskDetail.Id, vehicleId);
            if (vehiclePremium != null)
                _vehiclePremiumRepository.Remove(vehiclePremium);

            await _vehicleRepository.SaveChangesAsync();
            await UpdateVehicleTypeCount(riskDetail.Id);

            return _mapper.Map<VehicleDTO>(reinstatedClone);
        }

        public async Task<List<VehicleDTO>> GetByRiskDetailIdIncludeAsync(Guid riskDetailId, bool? withDeleted = false, bool fromUI = false)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);

            IEnumerable<Vehicle> vehicleHistory = new List<Vehicle>();

            if (withDeleted == true)
            {
                var previousActiveRiskDetail = await _riskDetailRepository.FindAsync(risk.CurrentRiskDetailId.Value);
                vehicleHistory = await _vehicleRepository.GetPreviousVehiclesByRiskDetail(previousActiveRiskDetail, withDeleted);
            }

            var result = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, withDeleted);

            List<VehicleDTO> vehicleDTOs = new List<VehicleDTO>();

            foreach (var vehicle in result)
            {
                var newVehicle = await GetByIdAsync(vehicle.Id);
                vehicleDTOs.Add(newVehicle);
            }

            if (fromUI)
            {
                foreach (var vehicleDTO in vehicleDTOs)
                {
                    var vehicle = result.First(x => x.Id == vehicleDTO.Id);

                    if (vehicleDTO.DeletedDate != null)
                    {
                        var previousVehicle = vehicleHistory.FirstOrDefault(x => x.MainId == vehicle.MainId);
                        vehicleDTO.CanReinstate = previousVehicle != null && previousVehicle.DeletedDate != null;

                    }
                    //vehicleDTO.hasPreviousVersions = (await GetPreviousVehicles(vehicleDTO.Id.Value)).Any();
                }
            }
            await UpdateVehicleTypeCount(riskDetailId);
            return vehicleDTOs.OrderBy(v => v.CreatedDate).ToList();
        }

        public async Task<List<VehicleDTO>> GetByRiskDetailIdIncludePolicyAsync(Guid riskDetailId, bool? withDeleted = false, bool fromUI = false)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);

            IEnumerable<Vehicle> vehicleHistory = new List<Vehicle>();
            if (withDeleted == true)
            {
                var previousActiveRiskDetail = await _riskDetailRepository.FindAsync(risk.CurrentRiskDetailId.Value);
                vehicleHistory = await _vehicleRepository.GetPreviousVehiclesByRiskDetail(previousActiveRiskDetail, withDeleted);

            }
            
            IEnumerable<VehicleDTO> vehiclesDeleted = _mapper.Map<List<VehicleDTO>>(vehicleHistory);
            List<VehicleDTO> vehicleDelDTOs = new List<VehicleDTO>();
            var result = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, withDeleted);

            List<VehicleDTO> vehicleDTOs = new List<VehicleDTO>();
           

            foreach (var vehicle in result)
            {
                var newVehicle = await GetByIdAsync(vehicle.Id);
                vehicleDTOs.Add(newVehicle);
            }

            if (fromUI)
            {
                try
                {
                    foreach (var vehicleDTO in vehicleDTOs)
                    {
                        var vehicle = result.First(x => x.Id == vehicleDTO.Id);
                        var previousVehicle =  await GetPreviousVehicles(vehicleDTO.Id.Value);
                           
                        vehicleDelDTOs.AddRange(previousVehicle);
                       
                    }
                }
                catch (Exception e)
                {

                    throw;
                }

            }
            vehicleDTOs.AddRange(vehicleDelDTOs);

            return vehicleDTOs.OrderBy(v => v.CreatedDate).ToList();
        }
        public async Task<List<VehicleDTO>> GetAllIncludeAsync()
        {
            var result = await _vehicleRepository.GetAllIncludeAsync();

            foreach (var vehicle in result)
            {
                vehicle.RiskVehiclePremiumTransaction = await _riskVehiclePremiumTransactionRepository.GetAsync(vehicle.RiskDetailId, vehicle.MainId);
            }

            return _mapper.Map<List<VehicleDTO>>(result).ToList();

        }

        public async Task<List<VehicleDTO>> GetByIdIncludeAsync(Guid id)
        {
            var result = await _vehicleRepository.GetAllIncludeAsync(o => o.Id == id);

            foreach (var vehicle in result)
            {
                vehicle.RiskVehiclePremiumTransaction = await _riskVehiclePremiumTransactionRepository.GetAsync(vehicle.RiskDetailId, vehicle.MainId);
            }

            return _mapper.Map<List<VehicleDTO>>(result).ToList();
        }

        public async Task<List<VehicleDTO>> GetPreviousVehicles(Guid vehicleId)
        {
            var previousVehiclesHistory = new List<Vehicle>();
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            var riskDetail = await _riskDetailRepository.FindAsync(vehicle.RiskDetailId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var policyHistories = await _policyHistoryRepository.GetByRiskId(riskDetail.RiskId);
            if (!policyHistories.Any())
                return new List<VehicleDTO>();

            // Get policy history
            var i = policyHistories.Select(x => Convert.ToInt32(x.EndorsementNumber)).Max();
            for (/**/; i >= 0; i--)
            {
                var history = policyHistories.First(x => x.EndorsementNumber == i.ToString());
                IEnumerable<IEndorsementChangeItem> changes = null;
                bool isInSubmission = false;

                if (i > 0)
                {
                    changes = await _endorsementChangesService.GetChangesByRiskAndEndorsement(risk.Id, i);
                }
                else
                {
                    var submissionVehicles = await _vehicleRepository.GetByRiskDetailAsync(Guid.Parse(history.PreviousRiskDetailId));
                    isInSubmission = submissionVehicles.Any(x => x.MainId == vehicle.MainId);
                }


                changes = changes?.Where(x =>
                    x is VehicleUpdateChangeItem
                    || x is VehicleAddChangeItem
                    || x is VehicleRemoveChangeItem
                    || x is VehicleReinstateChangeItem);

                bool isUpdated = changes?.Select(x => x as VehicleUpdateChangeItem)?.Any(x => x?.MainId == vehicle.MainId) ?? false;
                bool isAdded = changes?.Select(x => x as VehicleAddChangeItem)?.FirstOrDefault(x => x?.Vehicles?.Any(v => v.VIN == vehicle.VIN) ?? false) != null;
                bool isRemoved = changes?.Select(x => x as VehicleRemoveChangeItem)?.Any(x => x?.MainId == vehicle.MainId) ?? false;
                bool isReinstated = changes?.Select(x => x as VehicleReinstateChangeItem)?.Any(x => x?.MainId == vehicle.MainId) ?? false;
                bool wasCancelled = history.PolicyStatus == RiskStatus.Canceled;

                if (isUpdated || isAdded || isRemoved || isReinstated || isInSubmission || wasCancelled)
                {
                    var previousVehicles = await _vehicleRepository.GetByRiskDetailAsync(Guid.Parse(history.PreviousRiskDetailId), true);
                    var previousVehicle = previousVehicles.FirstOrDefault(x => (
                        (isAdded && x.VIN == vehicle.VIN)
                        || (wasCancelled && x.MainId == vehicle.MainId)
                        || ((isUpdated || isRemoved || isReinstated || isInSubmission) && x.MainId == vehicle.MainId)));

                    if (previousVehicle is null)
                        continue;

                    var previousHistory = previousVehiclesHistory.FirstOrDefault(x => x.Id == previousVehicle.Id);
                    if (previousHistory != null && (previousVehiclesHistory.LastOrDefault()?.HasChanges(previousVehicle) ?? false))
                    {
                        previousVehiclesHistory.Remove(previousHistory);
                    }

                    if (previousVehicle != null)
                    {
                        previousVehicle.EndorsementNumber = Convert.ToInt32(history.EndorsementNumber);
                        previousVehiclesHistory.Add(previousVehicle);
                    }
                }
            }

            if (previousVehiclesHistory.Any() && !(previousVehiclesHistory.FirstOrDefault()?.HasChanges(vehicle) ?? false))
            {
                previousVehiclesHistory = previousVehiclesHistory.Where(x => x != previousVehiclesHistory.First()).ToList();
            }

            return _mapper.Map<List<VehicleDTO>>(previousVehiclesHistory);
        }


        public async Task<List<VehicleDTO>> AddFromExcelFileAsync(Stream fileStream)
        {
            //IExcelManager excelManager = new InteropExcelManager(); //Requires MS Excel installed on Web API

            IExcelManager excelManager = new NpoiExcelManager(); // Check if will work without MS Excel installed
            excelManager.HeaderMap = HeaderMap;

            return await Task.Run(() => excelManager.AddVehicles(fileStream));
        }

        private List<PropertyHeader> _headerMap;
        public List<PropertyHeader> HeaderMap
        {
            get
            {
                return _headerMap ??= new List<PropertyHeader>
                {
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "VIN", Header = "VIN"},
                    new PropertyHeader {PropertyType = typeof(short), PropertyName = "Year", Header = "Year"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "Make", Header = "Make"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "Model", Header = "Model"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "StateFull", Header = "State"},
                    new PropertyHeader {PropertyType = typeof(decimal), PropertyName = "StatedAmount", Header = "TIV (stated Amount)"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "VehicleType", Header = "Vehicle Type"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "Description", Header = "Vehicle Description"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "BusinessClass", Header = "Vehicle Business Class"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "GrossVehicleWeight", Header = "Gross Vehicle Weight (lbs)"},
                    new PropertyHeader {PropertyType = typeof(decimal), PropertyName = "Radius", Header = "Vehicle Radius"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "CoverageFireTheft", Header = "Comprehensive / Fire & Theft (Yes or No)"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "CoverageCollision",Header = "Collision (Yes or No)"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "CoverageFireTheftDesc",Header = "Comprehensive / Fire & Theft Deductible"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "CoverageCollisionDeductible",Header = "Collision Deductible"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "CoverageCargo",Header = "Cargo ( Yes or No)"},
                    new PropertyHeader {PropertyType = typeof(string), PropertyName = "CoverageRefrigeration",Header = "Refrigeration (Yes or No)"},
                };
            }
        }

        public async Task UpdatePremiums(Guid riskDetailId, short? optionNumber = null)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var submissRiskDetail = await _riskDetailRepository.GetSubmissionRiskDetailAsync(riskDetail.RiskId);
            bool fromPolicy = riskDetail.Id != submissRiskDetail.Id;
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, fromPolicy, optionNumber);

            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var coveragePeriod = new DateRange(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);

            //var optionId = riskDetail?.RiskCoverages[0]?.OptionNumber != null ? (int)riskDetail?.RiskCoverages[0]?.OptionNumber : 1;

            foreach (var vehicle in vehicles)
            {
                await _riskVehiclePremiumService.UpdateVehiclePremiums(vehicle, riskDetailId, coveragePeriod, policyPeriod, optionNumber);
            }
        }

        //private async Task UpdatePremiumByPremiumFactor(VehiclePremiumRatingFactor premiumRatingFactor)
        //{
        //    var vehicleId = Guid.Parse(premiumRatingFactor.VehicleId);
        //    var vehicle = _vehicleRepository.GetById(vehicleId);
        //    var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllIncludeAsync(vehicle.LatestRiskDetail.RiskId);

        //    // Get existing premium change else create new
        //    VehiclePremium vehiclePremium = await _vehiclePremiumRepository.GetByRiskDetailVehicleIdAsync(vehicle.LatestRiskDetail.Id, vehicle.Id);
        //    if (vehiclePremium is null)
        //    {
        //        vehiclePremium = new VehiclePremium(vehicle.LatestRiskDetail.Id, vehicle.Id);
        //        await _vehiclePremiumRepository.AddAsync(vehiclePremium);
        //        await _vehiclePremiumRepository.SaveChangesAsync();
        //    }

        //    vehiclePremium.TransactionDate = DateTime.Now.Date;
        //    // Set previous VehiclePremium to current.
        //    VehiclePremium previousPremium = await _vehiclePremiumRepository.GetPrevious(vehiclePremium);
        //    VehiclePremium previousActivePremium = null;
        //    bool isPolicyReinstated = false;
        //    vehiclePremium.ChangePrevious(previousPremium);
        //    if (vehiclePremium.Previous != null)
        //    {
        //        var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == vehicle.LatestRiskDetail.RiskId.ToString());
        //        var previousPolicyHistory = await _policyHistoryRepository.GetLatestByRisk(vehicle.LatestRiskDetail.RiskId);
        //        isPolicyReinstated = policyHistories.OrderByDescending(x => x.DateCreated).First().PolicyStatus == RiskStatus.Reinstated;

        //        if (isPolicyReinstated)
        //        {
        //            var vehiclePremiums = await _vehiclePremiumRepository.GetByMainId(vehicle.MainId);
        //            previousActivePremium = await _vehiclePremiumRepository.GetPreviousActive(vehiclePremium);

        //            vehiclePremium.ChangePreviousGrossAL(previousActivePremium.AL.GrossProrated);
        //            vehiclePremium.ChangePreviousGrossPD(previousActivePremium.PD.GrossProrated);
        //            vehiclePremium.ChangePreviousGrossCargo(previousActivePremium.Cargo.GrossProrated);
        //            vehiclePremium.ChangePreviousGrossCompFT(previousActivePremium.CompFT.GrossProrated);
        //            vehiclePremium.ChangePreviousGrossCollision(previousActivePremium.Collision.GrossProrated);
        //            vehiclePremium.ChangePreviousGrossRefrigeration(previousActivePremium.Refrigeration.GrossProrated);
        //        }
        //    }

        //    // Update annual premiums.
        //    vehiclePremium.ChangeAnnualAL(premiumRatingFactor.TotalALPremium);
        //    vehiclePremium.ChangeAnnualPD(premiumRatingFactor.TotalPDPremium);
        //    vehiclePremium.ChangeAnnualCargo(premiumRatingFactor.CargoPremium);
        //    vehiclePremium.ChangeAnnualCompFT((premiumRatingFactor.CompPremium ?? 0) + (premiumRatingFactor.FireTheftPremium ?? 0));
        //    vehiclePremium.ChangeAnnualCollision(premiumRatingFactor.CollisionPremium);
        //    vehiclePremium.ChangeAnnualRefrigeration(premiumRatingFactor.RefrigerationPremium);

        //    // Prorate premiums
        //    DateTime premiumEffective = vehicle.LatestRiskDetail.EndorsementEffectiveDate?.Date ?? brokerInfo.EffectiveDate.Value.Date;
        //    DateTime premiumExpiration = brokerInfo.ExpirationDate.Value;
        //    vehiclePremium.Prorate(
        //        premiumEffective, premiumExpiration,  // Effective dates
        //        brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value); // Policy Effective dates

        //    if (isPolicyReinstated && previousActivePremium != null)
        //    {
        //        var previous = await _vehiclePremiumRepository.GetPrevious(vehiclePremium);

        //        vehiclePremium.ChangePreviousGrossAL(previous.AL.GrossProrated);
        //        vehiclePremium.ChangePreviousGrossPD(previous.PD.GrossProrated);
        //        vehiclePremium.ChangePreviousGrossCargo(previous.Cargo.GrossProrated);

        //        vehiclePremium.ChangePreviousGrossCompFT(previous.CompFT.GrossProrated);
        //        vehiclePremium.ChangePreviousGrossCollision(previous.Collision.GrossProrated);
        //        vehiclePremium.ChangePreviousGrossRefrigeration(previous.Refrigeration.GrossProrated);

        //        vehiclePremium.RecalculateNet();

        //        // reset previous reference.
        //        vehiclePremium.ChangePrevious(previous);
        //    }
        //    else
        //    {
        //        vehiclePremium.RecalculateNet();
        //    }


        //    //await _vehiclePremiumRepository.UpdateAsync(vehiclePremium);
        //    _vehiclePremiumRepository.Update(vehiclePremium);
        //    await _vehiclePremiumRepository.SaveChangesAsync();
        //}
        public async Task UpdateVehicleEffectiveDateExpirationDate(Guid riskDetailId, DateTime effectiveDate, DateTime expirationDate)
        {
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId);

            if (vehicles?.Count() > 0)
            {
                vehicles = vehicles.Select(c => { c.EffectiveDate = effectiveDate; c.ExpirationDate = expirationDate; return c; }).ToList();

                _vehicleRepository.UpdateRange(vehicles);
                await _vehicleRepository.SaveChangesAsync();
            }
        }


        public async Task UpdateMainUseAsync(Guid riskDetailId, short mainUseId)
        {
            var vehicles = await GetByRiskDetailIdIncludeAsync(riskDetailId);
            foreach (var vehicle in vehicles)
            {
                vehicle.UseClassId = mainUseId;
                await UpdateAsync(vehicle);
            }
        }


        public async Task UpdateAccountCategoryAsync(Guid riskDetailId, short accountCategoryId)
        {
            var vehicles = await GetByRiskDetailIdIncludeAsync(riskDetailId);
            foreach (var vehicle in vehicles)
            {
                vehicle.VehicleBusinessClassId = accountCategoryId;
                await UpdateAsync(vehicle);
            }
        }


        public async Task RemovePremiumRatingFactorOfUncheckedsAsync(Guid riskDetailId, int optionNumber)
        {
            IEnumerable<VehiclePremiumRatingFactor> premiumRatingFactors = await _vehicleRepository
                .GetPremiumRatingFactorsByRiskAsync(riskDetailId, optionNumber);

            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, false, (short)optionNumber);
            foreach (var vehicle in vehicles)
            {
                bool isVehicleSelectedInOption = vehicle.Options?.Contains(optionNumber.ToString()) ?? false;
                if (!isVehicleSelectedInOption)
                {
                    var premiumRatingFactor = premiumRatingFactors.FirstOrDefault(x => Guid.Parse(x.VehicleId) == vehicle.Id);
                    if (premiumRatingFactor != null)
                    {
                        await _vehicleRepository.DeletePremiumRatingFactorAsync(premiumRatingFactor.Id);
                    }
                }
            }
        }

        public async Task<List<VehicleDTO>> GetByRiskDetailIdForEndorsementAsync(Guid riskDetailId, bool? fromPolicy = false)
        {
            var result = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, fromPolicy);

            List<VehicleDTO> vehicleDTOs = new List<VehicleDTO>();

            foreach (var vehicle in result)
            {
                var newVehicle = await GetByIdAsync(vehicle.Id);
                vehicleDTOs.Add(newVehicle);
            }
            return vehicleDTOs.OrderBy(v => v.CreatedDate).ToList();
        }


        public async Task<DateTime?> GetEffectiveDateFromPreviousRiskDetail(VehicleDTO vehicle, Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var previousVehicle = await _vehicleRepository.GetByRiskDetailAndMainIdAsync(risk.CurrentRiskDetailId.Value, vehicle.MainId);
            return previousVehicle?.EffectiveDate;
        }

        public async Task<DateTime?> GetVehicleEffectiveDate(VehicleDTO vehicle, Guid currentRiskDetailId)
        {
            var previousVehicle = await _vehicleRepository.GetByRiskDetailAndMainIdAsync(currentRiskDetailId, vehicle.MainId);

            // Detached the same vehicle queried stored vehicle in db to avoid conflict when updating dbcontext.
            _vehicleRepository.Detach(previousVehicle);
            return previousVehicle?.EffectiveDate;
        }

        public async Task ClearPremiums(Guid riskDetailId)
        {
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId);

            foreach(var vehicle in vehicles)
            {
                vehicle.ClearPremiums();
                // Delete vehicle premium rating factor
                var vehiclePremiumRatingFactor = await _vehiclePremiumRatingFactorRepository.GetByVehicleAsync(vehicle.Id, riskDetailId);
                if (vehiclePremiumRatingFactor != null)
                    await _vehiclePremiumRatingFactorRepository.DeleteAsync(vehiclePremiumRatingFactor);
                // Delete vehicle premiums
                var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(riskDetailId, vehicle.MainId);
                await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
            }
        }
        public async Task<string> RemoveAllVinAsync(Guid riskDetailId)
        {
            var vinremoval = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, false);

            foreach (var forremoval in vinremoval)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(forremoval.Id);
                var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(vehicle.LatestRiskDetail.Id);
                var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);

               
                if (vehicle.PreviousVehicleVersionId != null) // If not same with previous riskdetail but with previous version, just soft  delete .
                    {
                        Vehicle deletedVehicle = vehicle;
                        if (vehicle.PreviousVehicleVersion?.HasChanges(vehicle) ?? false)
                        {
                            // If changes has been made on new version, hard delete it and
                            // just clone the previous version to revert changes.
                            var latestRiskdetailVehicle = vehicle.UnlinkLatestRiskDetail();
                            deletedVehicle = vehicle.PreviousVehicleVersion.CloneForEndorsement();
                            deletedVehicle.LinkRiskDetail(latestRiskdetailVehicle.RiskDetail);
                            await _vehicleRepository.AddAsync(deletedVehicle);
                            await _vehiclePremiumRepository.SaveChangesAsync();

                            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(latestRiskdetailVehicle.RiskDetailId, vehicle.MainId);
                            if (riskVehiclePremiums != null)
                            {
                                await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
                            }
                            _vehicleRepository.Remove(vehicle);
                        }
                        deletedVehicle.Delete(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate);
                        _vehicleRepository.Update(deletedVehicle);
                    }
                    else
                    {
                        // Hard delete vehicles which are added but have not been issued previously.
                        _vehicleRepository.Remove(vehicle); // If from submission or just added in endrosement and not yet issued, hard delete.
                        var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(riskDetail.Id, vehicle.MainId);
                        if (riskVehiclePremiums != null)
                        {
                            await _riskVehiclePremiumRepository.DeleteAsync(riskVehiclePremiums);
                        }
                    }
                //}

                await _vehicleRepository.SaveChangesAsync();
                await UpdateVehicleTypeCount(vehicle.LatestRiskDetail.Id);
            }

            return riskDetailId.ToString();
        }

        private void ValidateModel(VehicleDTO model)
        {
            // In sequence of Add / Edit Vehicle Form.
            if (!model.Year.HasValue) 
                throw new ArgumentException("Invalid Vehicle Year");

            if (string.IsNullOrWhiteSpace(model.Make)) 
                throw new ArgumentException("Invalid Vehicle Make");

            if (string.IsNullOrWhiteSpace(model.Model)) 
                throw new ArgumentException("Invalid Vehicle Model");

            if (string.IsNullOrWhiteSpace(model.VIN) || !this.ValidateVIN(model.VIN)) 
                throw new ArgumentException("Invalid Vehicle VIN");

            if (string.IsNullOrWhiteSpace(model.RegisteredState)) 
                throw new ArgumentException("Invalid Vehicle Registered State");

            if (string.IsNullOrWhiteSpace(model.VehicleType)) 
                throw new ArgumentException("Invalid Vehicle Type");

            if (!model.GrossVehicleWeightId.HasValue) 
                throw new ArgumentException("Invalid Vehicle GVW");

            if (!model.VehicleDescriptionId.HasValue) 
                throw new ArgumentException("Invalid Vehicle Description");

            if (!model.VehicleBusinessClassId.HasValue) 
                throw new ArgumentException("Invalid Vehicle Business Class");

            if (!model.UseClassId.HasValue) 
                throw new ArgumentException("Invalid Vehicle Use Class");

            if (!model.Radius.HasValue || model.Radius < 1) 
                throw new ArgumentException("Invalid Vehicle Radius");

            if (!model.GaragingAddressId.HasValue) 
                throw new ArgumentException("Invalid Vehicle Garaging Address");
        }

        private bool ValidateVIN(string vin)
        {
            Regex pattern = new Regex(@"^[A-HJ-NPR-Za-hj-npr-z0-9]*$");
            return pattern.IsMatch(vin);
        }
    }
}
