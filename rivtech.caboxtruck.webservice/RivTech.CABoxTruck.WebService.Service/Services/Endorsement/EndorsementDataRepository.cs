using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public class EndorsementDataRepository : IEndorsementDataRepository
    {
        private readonly IRiskService _riskService;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IEndorsementChangeDeserializer _endorsementChangeDeserializer;
        private readonly IRiskRepository _riskRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IDriverService _driverService;
        private readonly IEndorsementProcessStatusLogService _statusLogService;

        public EndorsementDataRepository(
            IRiskService riskService,
            IRiskDetailRepository riskDetailRepository,
            IEndorsementChangesService endorsementChangesService,
            IEndorsementChangeDeserializer endorsementChangeDeserializer,
            IRiskRepository riskRepository,
            IVehicleService vehicleService,
            IDriverService driverService,
            IEndorsementProcessStatusLogService statusLogService
        )
        {
            _riskService = riskService;
            _riskDetailRepository = riskDetailRepository;
            _endorsementChangesService = endorsementChangesService;
            _endorsementChangeDeserializer = endorsementChangeDeserializer;
            _riskRepository = riskRepository;
            _vehicleService = vehicleService;
            _driverService = driverService;
            _statusLogService = statusLogService;
        }

        public async Task<IEndorsementData> LoadAsync(EndorsementPoint endorsementPoint)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdNoTrackingAsync(endorsementPoint.RiskDetailId);
            var riskDTO = await _riskService.GetByIdIncludeAsync(endorsementPoint.RiskDetailId);

            EndorsementData endorsementData = new EndorsementData(riskDetail, endorsementPoint.EndorsementChanges);

            endorsementData.EffectiveDate = endorsementPoint.EffectiveDate;
            endorsementData.RiskDTO = riskDTO;

            return endorsementData;
        }

        public async Task<IEndorsementData> LoadForEndorsementFromRiskAsync(EndorsementPoint endorsementPoint)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(endorsementPoint.RiskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var forEndorsementRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.ForEndorsementRiskDetailId.Value);
            var riskDTO = await _riskService.GetByIdIncludeAsync(forEndorsementRiskDetail.Id);

            var endorsementData = new EndorsementData(forEndorsementRiskDetail, endorsementPoint.EndorsementChanges)
            {
                RiskDTO = riskDTO
            };

            return endorsementData;
        }

        public async Task<IEndorsementData> UpdateAsync(IEndorsementData endorsementData)
        {
            EndorsementData data = (EndorsementData)endorsementData;
            var savedRiskDetail = await _riskDetailRepository.FindAsync(endorsementData.RiskDetailId);
            savedRiskDetail.EndorsementNumber = data.RiskDetail.EndorsementNumber;
            savedRiskDetail.SourceRiskDetailId = data.RiskDetail.SourceRiskDetailId;
            savedRiskDetail.EndorsementEffectiveDate = data.RiskDetail.EndorsementEffectiveDate;

            data.RiskDetail.VersionNumber = await _riskDetailRepository.GetNextVersionNumberOfEndorsementNumAsync(data.RiskId, savedRiskDetail.EndorsementNumber);
            savedRiskDetail.VersionNumber = data.RiskDetail.VersionNumber;

            //await _endorsementChangesService.UpdateEndorsementChangesAsync(
            //    endorsementData.EndorsementChanges,
            //    data.RiskDetail.RiskId,
            //    endorsementData.EndorsementNumber);

            await _riskDetailRepository.SaveChangesAsync();

            return endorsementData;
        }

        public async Task<RiskEndorsementPoints> GetRiskEndorsementPoints(Guid riskId)
        {
            //var risk = await _riskRepository.GetRiskByIdAsync(riskId);
            RiskDetail currentRiskDetail = await _riskDetailRepository.GetLastRiskDetailByRiskIdAndCreatedDateAsync(riskId);
            RiskDTO riskDTO = await _riskService.GetByIdIncludeAsync(currentRiskDetail.Id);

            var endorsementPoints = new List<EndorsementPoint>();
            do
            {
                currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(currentRiskDetail.Id);
                EndorsementChanges currentEndorsementChanges = await _endorsementChangesService.GetEndorsementChangesByRiskId(riskId, currentRiskDetail.EndorsementNumber);

                IEndorsementChangeCollection changeCollection = new EndorsementChangeCollection();
                if (currentEndorsementChanges != null)
                    changeCollection = _endorsementChangeDeserializer.Deserialize(currentEndorsementChanges);

                var point = new EndorsementPoint(
                    currentRiskDetail.Id,
                    currentRiskDetail.EndorsementEffectiveDate ?? riskDTO.BrokerInfo.EffectiveDate.Value,
                    changeCollection,
                    currentRiskDetail.EndorsementNumber);

                endorsementPoints.Add(point);

                if (currentRiskDetail.SourceRiskDetailId != null)
                    currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(currentRiskDetail.SourceRiskDetailId.Value);
                else
                    currentRiskDetail = null;
            }
            while (currentRiskDetail != null);

            endorsementPoints.Reverse();
            return new RiskEndorsementPoints(endorsementPoints);
        }


        public async Task UpdateVehiclesEffectiveDates(IEndorsementData clonedEndorsementData, DateTime effectiveDate, bool isOutOfSequence)
        {
            var data = (EndorsementData)clonedEndorsementData;
            var risk = await _riskRepository.FindAsync(data.RiskId);

            await _statusLogService.LogAsync(risk.Id, $"Updating vehicles' effectivity dates of Endorsement {clonedEndorsementData.EndorsementNumber}...");

            var changes = await _endorsementChangesService.GetChangesByRiskAndEndorsement(risk.Id, risk.NextEndorsementNumber());
            var vehicleChanges = changes.Where(x => x is VehicleUpdateChangeItem || x is VehicleAddChangeItem || x is VehicleRemoveChangeItem || x is VehicleReinstateChangeItem).ToList();
            var vehicleUpdateChanges = vehicleChanges.Where(x => x is VehicleUpdateChangeItem).Select(x => x as VehicleUpdateChangeItem);
            var vehicleAddChanges = vehicleChanges.Where(x => x is VehicleAddChangeItem).Select(x => x as VehicleAddChangeItem);
            var vehicleRemoveChanges = vehicleChanges.Where(x => x is VehicleRemoveChangeItem).Select(x => x as VehicleRemoveChangeItem);
            var vehicleReinstateChanges = vehicleChanges.Where(x => x is VehicleReinstateChangeItem).Select(x => x as VehicleReinstateChangeItem);

            foreach (var vehicle in data.RiskDTO.Vehicles)
            {
                bool isUpdated = vehicleUpdateChanges.Any(x => x?.MainId == vehicle.MainId);
                bool isAdded = vehicleAddChanges.Any(x => x?.Vehicles?.Any(v => v.VIN == vehicle.VIN) ?? false);
                bool isRemoved = vehicleRemoveChanges.Any(x => x?.MainId == vehicle.MainId);
                bool isReinstated = vehicleReinstateChanges.Any(x => x?.MainId == vehicle.MainId) ;

                if (risk.NextEndorsementNumber() == 1)
                {
                    var updateChangeItem = vehicleUpdateChanges?.FirstOrDefault();
                    if(updateChangeItem != null 
                        && (updateChangeItem.IsEndorsement1CargoChange ?? false) 
                        && (updateChangeItem.IsEndorsement1ReeferChange ?? false))
                    {
                        isUpdated = false;
                    }
                }

                var hasChange = isUpdated || isAdded || isRemoved || isReinstated;
                if (hasChange)
                {
                    if (isUpdated || isAdded || isReinstated)
                    {
                        vehicle.EffectiveDate = effectiveDate;
                    }
                    else if (isRemoved)
                    {
                        vehicle.ExpirationDate = effectiveDate;
                    }
                }
                else
                {
                    vehicle.EffectiveDate = await _vehicleService.GetVehicleEffectiveDate(vehicle, risk.CurrentRiskDetailId.Value);
                }
                vehicle.isEndorsement = true;

                await _vehicleService.UpdateVehicleEffectiveDateAsync(vehicle);

                //await _statusLogService.LogAsync(risk.Id, $"Updated VIN {vehicle.VIN} effective date.");
            }


            await _statusLogService.LogAsync(risk.Id, $"Done updating vehicles' effectivity dates of Endorsement {clonedEndorsementData.EndorsementNumber}.");
        }

        public async Task UpdateDriversEffectiveDates(IEndorsementData clonedEndorsementData, DateTime effectiveDate, bool isOutOfSequence)
        {
            var data = (EndorsementData)clonedEndorsementData;
            var risk = await _riskRepository.FindAsync(data.RiskId);

            await _statusLogService.LogAsync(risk.Id, $"Updating drivers' effectivity dates of Endorsement {clonedEndorsementData.EndorsementNumber}.");

            var changes = await _endorsementChangesService.GetChangesByRiskAndEndorsement(risk.Id, risk.NextEndorsementNumber());
            var driverChanges = changes.Where(x => x is DriverUpdateChangeItem || x is DriverAddChangeItem || x is DriverRemoveChangeItem || x is DriverReinstateChangeItem);
            var driverUpdateChanges = changes.Where(x => x is DriverUpdateChangeItem).Select(x => x as DriverUpdateChangeItem);
            var driverAddChanges = changes.Where(x => x is DriverAddChangeItem).Select(x => x as DriverAddChangeItem);
            var driverRemoveChanges = changes.Where(x => x is DriverRemoveChangeItem).Select(x => x as DriverRemoveChangeItem);
            var driverReinstateChanges = changes.Where(x => x is DriverReinstateChangeItem).Select(x => x as DriverReinstateChangeItem);

            foreach (var driver in data.RiskDTO.RiskDetailDrivers)
            {
                bool isUpdated = driverUpdateChanges.FirstOrDefault(x => x?.LicenseNumber == driver.LicenseNumber) != null;
                bool isAdded = driverAddChanges.FirstOrDefault(x => x?.Drivers?.Any(v => v.LicenseNumber == driver.LicenseNumber) ?? false) != null;
                bool isRemoved = driverRemoveChanges.FirstOrDefault(x => x?.DriverId == driver.PreviousDriverVersionId) != null;
                bool isReinstated = driverReinstateChanges.FirstOrDefault(x => x?.DriverId == driver.PreviousDriverVersionId) != null;

                var hasChange = isUpdated || isAdded || isRemoved || isReinstated;
                if (hasChange)
                {
                    if (isUpdated || isAdded || isReinstated)
                    {
                        driver.EffectiveDate = effectiveDate;
                    }
                    else if (isRemoved)
                    {
                        driver.ExpirationDate = effectiveDate;
                    }
                }
                else
                {
                    driver.EffectiveDate = await _driverService.GetDriverEffectiveDate(driver, risk.CurrentRiskDetailId.Value);
                }
                //driver.isEndorsement = true;

                await _driverService.UpdateDriverEffectiveDateAsync(driver);

                //await _statusLogService.LogAsync(risk.Id, $"Updated Driver {driver.FirstName} {driver.LastName} effectivity dates.");
            }
            
            await _statusLogService.LogAsync(risk.Id, $"Done updating drivers' effectivity dates Endorsement {clonedEndorsementData.EndorsementNumber}.");
        }
    }
}
