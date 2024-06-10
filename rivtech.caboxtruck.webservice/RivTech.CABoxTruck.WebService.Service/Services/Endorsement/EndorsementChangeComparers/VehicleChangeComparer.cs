using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class VehicleChangeComparer : IChangeComparer
    {
        private readonly ILvLimitsRepository _lvLimitsRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;

        public VehicleChangeComparer(
            ILvLimitsRepository lvLimitsRepository,
            IRiskCoverageRepository riskCoverageRepository
        )
        {
            _lvLimitsRepository = lvLimitsRepository;
            _riskCoverageRepository = riskCoverageRepository;
        }

        public Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var endorsementChangeCollection = new EndorsementChangeCollection();
            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            bool hasChanges = false;

            var newVehicles = newRiskDTO.Vehicles.Where(n => !oldRiskDTO.Vehicles.Any(o => o.MainId == n.MainId));
            if (newVehicles.Any())
            {
                endorsementChangeCollection.Add(new VehicleAddChangeItem
                {
                    Vehicles = newVehicles.ToArray()
                });
                hasChanges = true;
            }

            foreach (var newRiskVehicle in newRiskDTO.Vehicles)
            {
                var previousVehicle = oldRiskDTO.Vehicles.FirstOrDefault(x => x.MainId == newRiskVehicle.MainId);
                var hasPreviousVehicle = previousVehicle != null;
                var isNewVehicleVersionDeleted = newRiskVehicle.DeletedDate != null;
                var isPreviousVehicleVersionDeleted = hasPreviousVehicle && previousVehicle.DeletedDate != null;

                if (isNewVehicleVersionDeleted)
                {
                    if (!isPreviousVehicleVersionDeleted)
                    {
                        endorsementChangeCollection.Add(new VehicleRemoveChangeItem
                        {
                            VIN = newRiskVehicle.VIN,
                            MainId = newRiskVehicle.MainId,
                            ExpirationDate = newRiskVehicle.ExpirationDate.Value
                        });
                        hasChanges = true;
                    }
                    continue;
                }

                bool isJustReinstated = hasPreviousVehicle && !isNewVehicleVersionDeleted && isPreviousVehicleVersionDeleted;
                if (isJustReinstated)
                {
                    endorsementChangeCollection.Add(new VehicleReinstateChangeItem
                    {
                        VIN = newRiskVehicle.VIN,
                        MainId = newRiskVehicle.MainId,
                    });
                    hasChanges = true;
                }

                bool existInPreviousRisk = oldRiskDTO.Vehicles.Any(old => old.MainId == newRiskVehicle.MainId);
                if (existInPreviousRisk)
                {
                    var oldVeh = oldRiskDTO.Vehicles.First(x => x.MainId == newRiskVehicle.MainId);
                    var change = CompareUpdatedVehicle(newRiskVehicle, oldVeh, newRiskDTO, isJustReinstated);
                    if (change != null)
                    {
                        endorsementChangeCollection.Add(change);
                        hasChanges = true;
                    }
                }
            }

            if (!hasChanges)
                return Task.FromResult<IEnumerable<IEndorsementChangeItem>>(null);

            return Task.FromResult(endorsementChangeCollection.AsEnumerable());
        }

        private IEndorsementChangeItem CompareUpdatedVehicle(VehicleDTO newVehicle, VehicleDTO oldVehicle, RiskDTO newRiskDTO, bool isJustReinstated)
        {
            var changeItem = new VehicleUpdateChangeItem();
            changeItem.MainId = oldVehicle.MainId;
            changeItem.VIN = newVehicle.VIN;
            bool isEndorsement1 = newRiskDTO.EndorsementNumber == 1;
            var riskCov = _riskCoverageRepository.GetRiskCoverageBySelectedBindOption(newRiskDTO.Id.Value).Result;

            var compLimits = _lvLimitsRepository.GetAsync(x => x.LimitType.Equals(LimitType.Comprehensive)).Result;
            var compNoCoverage = compLimits.SingleOrDefault(x => x.LimitValue.Equals("No Coverage"));

            var collLimits = _lvLimitsRepository.GetAsync(x => x.LimitType.Equals(LimitType.Collision)).Result;
            var collNoCoverage = collLimits.SingleOrDefault(x => x.LimitValue.Equals("No Coverage"));

            var cargoLimits = _lvLimitsRepository.GetAsync(x => x.LimitType.Equals(LimitType.Cargo)).Result;
            var cargoNoCoverage = cargoLimits.SingleOrDefault(x => x.LimitValue.Equals("NONE"));

            var refLimits = _lvLimitsRepository.GetAsync(x => x.LimitType.Equals(LimitType.Cargo)).Result;
            var refNoCoverage = refLimits.SingleOrDefault(x => x.LimitValue.Equals("NONE"));

            if (newVehicle.EffectiveDate != oldVehicle.EffectiveDate) changeItem.EffectiveDate = newVehicle.EffectiveDate;
            if (!isJustReinstated && newVehicle.ExpirationDate != oldVehicle.ExpirationDate) changeItem.ExpirationDate = newVehicle.ExpirationDate;
            if (newVehicle.Year != oldVehicle.Year) changeItem.Year = newVehicle.Year;
            if (newVehicle.Make != oldVehicle.Make) changeItem.Make = newVehicle.Make;
            if (newVehicle.Model != oldVehicle.Model) changeItem.Model = newVehicle.Model;
            if (newVehicle.RegisteredState != oldVehicle.RegisteredState) changeItem.RegisteredState = newVehicle.RegisteredState;
            if (newVehicle.Style != oldVehicle.Style) changeItem.Style = newVehicle.Style;
            if (newVehicle.VehicleType != oldVehicle.VehicleType) changeItem.VehicleType = newVehicle.VehicleType;
            if (newVehicle.GrossVehicleWeightId != oldVehicle.GrossVehicleWeightId) changeItem.GrossVehicleWeightId = newVehicle.GrossVehicleWeightId;
            if (newVehicle.VehicleDescriptionId != oldVehicle.VehicleDescriptionId) changeItem.VehicleDescriptionId = newVehicle.VehicleDescriptionId;
            if (newVehicle.VehicleBusinessClassId != oldVehicle.VehicleBusinessClassId) changeItem.VehicleBusinessClassId = newVehicle.VehicleBusinessClassId;
            if (newVehicle.IsAutoLiability != oldVehicle.IsAutoLiability) changeItem.IsAutoLiability = newVehicle.IsAutoLiability;
            if (newVehicle.StatedAmount != oldVehicle.StatedAmount) changeItem.StatedAmount = newVehicle.StatedAmount;

            if (newVehicle.IsCoverageFireTheft != oldVehicle.IsCoverageFireTheft)
            {
                changeItem.IsCoverageFireTheft = newVehicle.IsCoverageFireTheft;
            }
            if (newVehicle.CoverageFireTheftDeductibleId != oldVehicle.CoverageFireTheftDeductibleId)
            {
                changeItem.CoverageFireTheftDeductibleId = newVehicle.CoverageFireTheftDeductibleId ?? (short?)compNoCoverage.Id;
            }
            //if (isEndorsement1 && changeItem.IsCoverageFireTheft is null)
            //{
            //    changeItem.IsCoverageFireTheft = (newVehicle.CoverageFireTheftDeductibleId != compNoCoverage.Id && newVehicle.CoverageFireTheftDeductibleId != null);
            //}
            if (newVehicle.IsCoverageCollision != oldVehicle.IsCoverageCollision)
            {
                changeItem.IsCoverageCollision = newVehicle.IsCoverageCollision;
            }
            if (newVehicle.CoverageCollisionDeductibleId != oldVehicle.CoverageCollisionDeductibleId)
            {
                changeItem.CoverageCollisionDeductibleId = newVehicle.CoverageCollisionDeductibleId ?? (short?)collNoCoverage.Id;
            }
            //if(isEndorsement1 && changeItem.IsCoverageCollision is null)
            //{
            //    changeItem.IsCoverageCollision = (newVehicle.CoverageCollisionDeductibleId != collNoCoverage.Id && newVehicle.CoverageCollisionDeductibleId != null);
            //}

            if (newVehicle.HasCoverageCargo != oldVehicle.HasCoverageCargo)
                changeItem.HasCoverageCargo = newVehicle.HasCoverageCargo;
            //if (isEndorsement1 && changeItem.HasCoverageCargo is null)
            //{
            //    changeItem.HasCoverageCargo = riskCov.CargoLimitId != null && riskCov.CargoLimitId != cargoNoCoverage.Id;
            //    changeItem.IsEndorsement1CargoChange = true;
            //}

            if (newVehicle.HasCoverageRefrigeration != oldVehicle.HasCoverageRefrigeration)
                changeItem.HasCoverageRefrigeration = newVehicle.HasCoverageRefrigeration;
            //if (isEndorsement1 && changeItem.HasCoverageRefrigeration is null)
            //{
            //    changeItem.HasCoverageRefrigeration = riskCov.RefCargoLimitId != null && riskCov.RefCargoLimitId != refNoCoverage.Id;
            //    changeItem.IsEndorsement1ReeferChange = true;
            //}

            if (newVehicle.UseClassId != oldVehicle.UseClassId) changeItem.UseClassId = newVehicle.UseClassId;
            if (newVehicle.Radius != oldVehicle.Radius) changeItem.Radius = newVehicle.Radius;
            if (newVehicle.GaragingAddressId != oldVehicle.GaragingAddressId) changeItem.GaragingAddressId = newVehicle.GaragingAddressId;
            if (newVehicle.PrimaryOperatingZipCode != oldVehicle.PrimaryOperatingZipCode) changeItem.PrimaryOperatingZipCode = newVehicle.PrimaryOperatingZipCode;
            if (newVehicle.State != oldVehicle.State) changeItem.State = newVehicle.State;
            if (newVehicle.RadiusTerritory != oldVehicle.RadiusTerritory) changeItem.RadiusTerritory = newVehicle.RadiusTerritory;

            return VehicleUpdateHasChanges(changeItem) ? changeItem : null;
        }

        private bool VehicleUpdateHasChanges(VehicleUpdateChangeItem changeItem)
        {
            return changeItem != null && (
                 changeItem.ExpirationDate != null
                 || changeItem.Year != null
                 || changeItem.Make != null
                 || changeItem.Model != null
                 || changeItem.RegisteredState != null
                 || changeItem.Style != null
                 || changeItem.VehicleType != null
                 || changeItem.GrossVehicleWeightId != null
                 || changeItem.VehicleDescriptionId != null
                 || changeItem.VehicleBusinessClassId != null
                 || changeItem.IsAutoLiability != null
                 || changeItem.StatedAmount != null
                 || changeItem.UseClassId != null
                 || changeItem.Radius != null
                 || changeItem.GaragingAddressId != null
                 || changeItem.PrimaryOperatingZipCode != null
                 || changeItem.State != null
                 || changeItem.RadiusTerritory != null
                 || changeItem.CoverageFireTheftDeductibleId != null
                 || changeItem.IsCoverageFireTheft != null
                 || changeItem.CoverageCollisionDeductibleId != null
                 || changeItem.IsCoverageCollision != null
                 || changeItem.HasCoverageCargo != null
                 || changeItem.HasCoverageRefrigeration != null
                 || changeItem.IsEndorsement1CargoChange != null
                 || changeItem.IsEndorsement1ReeferChange != null
             );
        }
    }
}
