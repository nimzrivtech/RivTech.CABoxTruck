using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class CargoChangeModifier : ChangeModifierBase<CargoChangeItem>
    {
        private readonly IRiskResponseService _riskResponseService;

        public CargoChangeModifier(
            IRiskResponseService riskResponseService
        ) {
            _riskResponseService = riskResponseService;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            if (!IsChangeItemPropertiesEmpty(change) && data.RiskDTO.GeneralLiabilityCargo.RiskDetailId != Guid.Empty)
            {
                data.RiskDTO.GeneralLiabilityCargo.Cargo.AreRequiredToCarryCargoInsurance = change.AreRequiredToCarryCargoInsurance ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.AreRequiredToCarryCargoInsurance;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.RequireRefrigerationBreakdownCoverage = change.RequireRefrigerationBreakdownCoverage ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.RequireRefrigerationBreakdownCoverage;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.HaulHazardousMaterials = change.HaulHazardousMaterials ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.HaulHazardousMaterials;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.HaveRefrigeratedUnits = change.HaveRefrigeratedUnits ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.HaveRefrigeratedUnits;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.AreCommoditiesStoredInTruckOvernight = change.AreCommoditiesStoredInTruckOvernight ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.AreCommoditiesStoredInTruckOvernight;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.AreCommoditiesStoredInTruckOvernightExplain = change.AreCommoditiesStoredInTruckOvernightExplain ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.AreCommoditiesStoredInTruckOvernightExplain;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.HadCargoLoss = change.HadCargoLoss ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.HadCargoLoss;
                data.RiskDTO.GeneralLiabilityCargo.Cargo.HadCargoLossExplain = change.HadCargoLossExplain ?? data.RiskDTO.GeneralLiabilityCargo.Cargo.HadCargoLossExplain;

                var cargo = await _riskResponseService.MapToRiskResponse(data.RiskDTO.GeneralLiabilityCargo.Cargo, data.RiskDTO.GeneralLiabilityCargo.RiskDetailId);
                await _riskResponseService.SaveRiskResponse(cargo, LvQuestionSection.Cargo.Id, data.RiskDTO.GeneralLiabilityCargo.RiskDetailId);
            }
        }
    }
}
