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
    public class FormMCS90Modifier : ChangeModifierBase<FormMCS90ChangeItem>
    {
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        public FormMCS90Modifier(IFormsService formsService, IMapper mapper)
        {
            _formsService = formsService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            var riskFormDTO = data.RiskDTO.RiskForms.FirstOrDefault(x => x.FormId == "MCS90");

            if (!string.IsNullOrWhiteSpace(riskFormDTO.Other))
            {
                var jsonData = JsonConvert.DeserializeObject<FormMCS90ChangeItem>(riskFormDTO.Other);

                jsonData.UsDotNumber = change.UsDotNumber ?? jsonData.UsDotNumber;
                jsonData.DateReceived = change.DateReceived ?? jsonData.DateReceived;
                jsonData.IssuedTo = change.IssuedTo ?? jsonData.IssuedTo;
                jsonData.State = change.State ?? jsonData.State;
                jsonData.DatedAt = change.DatedAt ?? jsonData.DatedAt;
                jsonData.PolicyDateDay = change.PolicyDateDay ?? jsonData.PolicyDateDay;
                jsonData.PolicyDateMonth = change.PolicyDateMonth ?? jsonData.PolicyDateMonth;
                jsonData.PolicyDateYear = change.PolicyDateYear ?? jsonData.PolicyDateYear;
                jsonData.PolicyNumber = change.PolicyNumber ?? jsonData.PolicyNumber;
                jsonData.EffectiveDate = change.EffectiveDate ?? jsonData.EffectiveDate;
                jsonData.InsuranceCompany = change.InsuranceCompany ?? jsonData.InsuranceCompany;
                jsonData.CounterSignedBy = change.CounterSignedBy ?? jsonData.CounterSignedBy;
                jsonData.AccidentLimit = change.AccidentLimit ?? jsonData.AccidentLimit;

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
