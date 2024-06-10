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
    public class FormSelectionModifier : ChangeModifierBase<FormSelectionChangeItem>
    {
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        public FormSelectionModifier(IFormsService formsService, IMapper mapper)
        {
            _formsService = formsService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            //var data = (EndorsementData)endorsementData;
            //var change = CastOriginalType(changeItem);

            //var riskFormDTO = data.RiskDTO.RiskForms.FirstOrDefault(x => x.FormId == change.Id);

            //riskFormDTO.IsSelected = change.IsSelected ?? riskFormDTO.IsSelected;
            //riskFormDTO.Other = change.Other;

            //await _formsService.RiskFormUpdateAsync(riskFormDTO);
        }
    }
}
