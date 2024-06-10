using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class BindRequirementsModifier : ChangeModifierBase<BindRequirementsChangeItem>
    {
        private readonly IBindingService _bindingService;
        private readonly IMapper _mapper;

        public BindRequirementsModifier(
            IBindingService bindingService,
            IMapper mapper
        ) {
            _bindingService = bindingService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            BindingRequirementsDTO bindRequirementDTO = data.RiskDTO.BindingRequirements.FirstOrDefault(x => x.Id == change.Id);
            if (bindRequirementDTO is null)
                return;

            bindRequirementDTO.IsPreBind = change.IsPreBind ?? bindRequirementDTO.IsPreBind;
            bindRequirementDTO.BindRequirementsOptionId = change.BindRequirementsOptionId ?? bindRequirementDTO.BindRequirementsOptionId;
            bindRequirementDTO.BindStatusId = change.BindStatusId ?? bindRequirementDTO.BindStatusId;
            bindRequirementDTO.Comments = change.Comments ?? bindRequirementDTO.Comments;
            bindRequirementDTO.RelevantDocumentDesc = change.RelevantDocumentDesc ?? bindRequirementDTO.RelevantDocumentDesc;
            bindRequirementDTO.FileUploads = change.FileUploads ?? bindRequirementDTO.FileUploads;

            await _bindingService.AddUpdateAsync(bindRequirementDTO);
        }
    }
}
