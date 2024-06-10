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
    public class FormLossPayeeModifier : ChangeModifierBase<FormLossPayeeChangeItem>
    {
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        public FormLossPayeeModifier(IFormsService formsService, IMapper mapper)
        {
            _formsService = formsService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            var riskFormDTO = data.RiskDTO.RiskForms.FirstOrDefault(x => x.FormId == "LossPayeeEndorsement");

            if (!string.IsNullOrWhiteSpace(riskFormDTO.Other))
            {
                var jsonData = JsonConvert.DeserializeObject<FormLossPayeeChangeItem>(riskFormDTO.Other);

                jsonData.DescriptionOfAutos = change.DescriptionOfAutos ?? jsonData.DescriptionOfAutos;
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
