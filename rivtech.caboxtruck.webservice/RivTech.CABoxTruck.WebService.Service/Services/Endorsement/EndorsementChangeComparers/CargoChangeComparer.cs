using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class CargoChangeComparer : IChangeComparer
    {
        private readonly IRiskSpecificGeneralLiabilityCargoService _generalLiabilityCargoService;
        public CargoChangeComparer(IRiskSpecificGeneralLiabilityCargoService generalLiabilityCargoService)
        {
            _generalLiabilityCargoService = generalLiabilityCargoService;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var cargoChangeItem = new CargoChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newCargo = newRiskDTO.GeneralLiabilityCargo;
            var oldCargo = oldRiskDTO.GeneralLiabilityCargo;

            if (newCargo.Cargo.AreRequiredToCarryCargoInsurance != oldCargo.Cargo.AreRequiredToCarryCargoInsurance)
            {
                cargoChangeItem.AreRequiredToCarryCargoInsurance = newCargo.Cargo.AreRequiredToCarryCargoInsurance;
                hasChanges = true;
            }

            if (newCargo.Cargo.RequireRefrigerationBreakdownCoverage != oldCargo.Cargo.RequireRefrigerationBreakdownCoverage)
            {
                cargoChangeItem.RequireRefrigerationBreakdownCoverage = newCargo.Cargo.RequireRefrigerationBreakdownCoverage;
                hasChanges = true;
            }

            if (newCargo.Cargo.HaulHazardousMaterials != oldCargo.Cargo.HaulHazardousMaterials)
            {
                cargoChangeItem.HaulHazardousMaterials = newCargo.Cargo.HaulHazardousMaterials;
                hasChanges = true;
            }

            if (newCargo.Cargo.HaveRefrigeratedUnits != oldCargo.Cargo.HaveRefrigeratedUnits)
            {
                cargoChangeItem.HaveRefrigeratedUnits = newCargo.Cargo.HaveRefrigeratedUnits;
                hasChanges = true;
            }

            if (newCargo.Cargo.AreCommoditiesStoredInTruckOvernight != oldCargo.Cargo.AreCommoditiesStoredInTruckOvernight)
            {
                cargoChangeItem.AreCommoditiesStoredInTruckOvernight = newCargo.Cargo.AreCommoditiesStoredInTruckOvernight;
                hasChanges = true;
            }

            if (newCargo.Cargo.AreCommoditiesStoredInTruckOvernightExplain != oldCargo.Cargo.AreCommoditiesStoredInTruckOvernightExplain)
            {
                cargoChangeItem.AreCommoditiesStoredInTruckOvernightExplain = newCargo.Cargo.AreCommoditiesStoredInTruckOvernightExplain;
                hasChanges = true;
            }

            if (newCargo.Cargo.HadCargoLoss != oldCargo.Cargo.HadCargoLoss)
            {
                cargoChangeItem.HadCargoLoss = newCargo.Cargo.HadCargoLoss;
                hasChanges = true;
            }

            if (newCargo.Cargo.HadCargoLossExplain != oldCargo.Cargo.HadCargoLossExplain)
            {
                cargoChangeItem.HadCargoLossExplain = newCargo.Cargo.HadCargoLossExplain;
                hasChanges = true;
            }

            if (hasChanges)
            {
                items.Add(cargoChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
