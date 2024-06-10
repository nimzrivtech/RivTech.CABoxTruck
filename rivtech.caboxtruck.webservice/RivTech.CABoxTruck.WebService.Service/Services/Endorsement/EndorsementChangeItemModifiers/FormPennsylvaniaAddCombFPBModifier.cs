using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class FormPennsylvaniaAddCombFPBModifier : ChangeModifierBase<FormPennsylvaniaAddCombFPBChangeItem>
    {
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        public FormPennsylvaniaAddCombFPBModifier(IFormsService formsService, IMapper mapper)
        {
            _formsService = formsService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            var riskFormDTO = data.RiskDTO.RiskForms.FirstOrDefault(x => x.FormId == "PennsylvaniaAddCombFirstPartyBenefitsEndorsement");

            if (!string.IsNullOrWhiteSpace(riskFormDTO.Other))
            {
                var jsonData = JsonConvert.DeserializeObject<FormPennsylvaniaAddCombFPBChangeItem>(riskFormDTO.Other);

                jsonData.AddedFirstPartyBenefits = change.AddedFirstPartyBenefits ?? jsonData.AddedFirstPartyBenefits;
                jsonData.MedicalExpenseBenefits = change.MedicalExpenseBenefits ?? jsonData.MedicalExpenseBenefits;
                jsonData.WorkLossUpTo = change.WorkLossUpTo ?? jsonData.WorkLossUpTo;
                jsonData.WorkLossPerMonth = change.WorkLossPerMonth ?? jsonData.WorkLossPerMonth;
                jsonData.FuneralExpenseBenefits = change.FuneralExpenseBenefits ?? jsonData.FuneralExpenseBenefits;
                jsonData.AccidentalDeathBenefits = change.AccidentalDeathBenefits ?? jsonData.AccidentalDeathBenefits;
                jsonData.CombinationFirstPartyBenefits = change.CombinationFirstPartyBenefits ?? jsonData.CombinationFirstPartyBenefits;
                jsonData.MaximumTotalLimitAllBenefits = change.MaximumTotalLimitAllBenefits ?? jsonData.MaximumTotalLimitAllBenefits;
                jsonData.CombFuneralExpenseBenefits = change.CombFuneralExpenseBenefits ?? jsonData.CombFuneralExpenseBenefits;
                jsonData.CombAccidentalDeathBenefits = change.CombAccidentalDeathBenefits ?? jsonData.CombAccidentalDeathBenefits;

                riskFormDTO.Other = JsonConvert.SerializeObject(jsonData);
            }
            else
            { 
                riskFormDTO.Other = null; 
            }

            await _formsService.SingleFormUpdateAsync(riskFormDTO);
        }
    }
}
