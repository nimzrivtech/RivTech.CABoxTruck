using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class GeneralLiabilityModifier : ChangeModifierBase<GeneralLiabilityChangeItem>
    {
        private readonly IRiskResponseService _riskResponseService;

        public GeneralLiabilityModifier(
            IRiskResponseService riskResponseService
        )
        {
            _riskResponseService = riskResponseService;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            if (!IsChangeItemPropertiesEmpty(change) && data.RiskDTO.GeneralLiabilityCargo.RiskDetailId != Guid.Empty)
            {
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.ContractuallyRequiredToCarryGeneralLiabilityInsurance =
                    change.ContractuallyRequiredToCarryGeneralLiabilityInsurance ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.ContractuallyRequiredToCarryGeneralLiabilityInsurance;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HaveOperationsOtherThanTrucking = change.HaveOperationsOtherThanTrucking ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HaveOperationsOtherThanTrucking;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HaveOperationsOtherThanTruckingExplain = change.HaveOperationsOtherThanTruckingExplain ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HaveOperationsOtherThanTruckingExplain;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.OperationsAtStorageLotOrImpoundYard = change.OperationsAtStorageLotOrImpoundYard ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.OperationsAtStorageLotOrImpoundYard;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyStorageOfGoods = change.AnyStorageOfGoods ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyStorageOfGoods;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyWarehousing = change.AnyWarehousing ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyWarehousing;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyStorageOfVehiclesForOthers = change.AnyStorageOfVehiclesForOthers ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyStorageOfVehiclesForOthers;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyLeasingSpaceToOthers = change.AnyLeasingSpaceToOthers ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyLeasingSpaceToOthers;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyFreightForwarding = change.AnyFreightForwarding ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyFreightForwarding;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyStorageOfFuelsAndOrChemicals = change.AnyStorageOfFuelsAndOrChemicals ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.AnyStorageOfFuelsAndOrChemicals;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HasApplicantHadGeneralLiabilityLoss = change.HasApplicantHadGeneralLiabilityLoss ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HasApplicantHadGeneralLiabilityLoss;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HasApplicantHadGeneralLiabilityLossExplain = change.HasApplicantHadGeneralLiabilityLossExplain ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.HasApplicantHadGeneralLiabilityLossExplain;
                data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.GlAnnualPayroll = change.GlAnnualPayroll ?? data.RiskDTO.GeneralLiabilityCargo.GeneralLiability.GlAnnualPayroll;

                var generalLiability = await _riskResponseService.MapToRiskResponse(data.RiskDTO.GeneralLiabilityCargo.GeneralLiability, data.RiskDTO.GeneralLiabilityCargo.RiskDetailId);
                await _riskResponseService.SaveRiskResponse(generalLiability, LvQuestionSection.GeneralLiability.Id, data.RiskDTO.GeneralLiabilityCargo.RiskDetailId);
            }
        }
    }
}
