using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class FormSelectionComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public FormSelectionComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }
        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            //TODO: Fix/change implementation
            var newRiskForms = await _riskFormRepository.GetAsync(e => e.RiskDetailId == newRiskDTO.Id.Value);
            var oldRiskForms = await _riskFormRepository.GetAsync(e => e.RiskDetailId == oldRiskDTO.Id.Value);

            foreach (var newRiskForm in newRiskForms)
            {
                var formSelectionChangeItem = new FormSelectionChangeItem();
                var oldRiskForm = oldRiskForms.Where(x => x.FormId == newRiskForm?.FormId).FirstOrDefault();

                if (oldRiskForm != null)
                {
                    var hasChanges = false;

                    formSelectionChangeItem.Other = oldRiskForm.Other;

                    //if (newRiskForm?.Other != oldRiskForm.Other)
                    //{
                    //    formSelectionChangeItem.Other = newRiskForm?.Other;
                    //    hasChanges = true;
                    //}

                    if (newRiskForm.IsSelected != oldRiskForm.IsSelected)
                    {
                        formSelectionChangeItem.IsSelected = newRiskForm.IsSelected;
                        hasChanges = true;
                    }

                    if (hasChanges)
                    {
                        formSelectionChangeItem.Id = newRiskForm.FormId;
                        formSelectionChangeItem.IsSelected = newRiskForm.IsSelected;
                        items.Add(formSelectionChangeItem);
                    }
                }
            }

            return items.Count() > 0 ? items : null;
        }
    }
}
