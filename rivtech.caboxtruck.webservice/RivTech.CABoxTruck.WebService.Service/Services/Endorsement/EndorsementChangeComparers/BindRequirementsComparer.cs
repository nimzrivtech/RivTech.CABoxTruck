using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class BindRequirementsComparer : IChangeComparer
    {
        private readonly IBindingService _bindingService;
        public BindRequirementsComparer(IBindingService bindingService)
        {
            _bindingService = bindingService;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var newBindRequirements = newRiskDTO.BindingRequirements;

            foreach (var newBindRequirement in newBindRequirements)
            {
                var bindRequirementsChangeItem = new BindRequirementsChangeItem();

                bindRequirementsChangeItem.Id = newBindRequirement.Id;
                bindRequirementsChangeItem.IsPreBind = newBindRequirement.IsPreBind;
                bindRequirementsChangeItem.BindRequirementsOptionId = newBindRequirement.BindRequirementsOptionId;
                bindRequirementsChangeItem.BindStatusId = newBindRequirement.BindStatusId;
                bindRequirementsChangeItem.FilePath = newBindRequirement.FilePath;
                bindRequirementsChangeItem.Comments = newBindRequirement.Comments;
                bindRequirementsChangeItem.RelevantDocumentDesc = newBindRequirement.RelevantDocumentDesc;
                bindRequirementsChangeItem.FileUploads = newBindRequirement.FileUploads;
                items.Add(bindRequirementsChangeItem);
            }


            return items.Count() > 0 ? items : null;
        }
    }
}
