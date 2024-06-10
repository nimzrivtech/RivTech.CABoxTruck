using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class CoveredAutoDesignationSymbolComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public CoveredAutoDesignationSymbolComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDetailObj, object oldRiskDetailObj)
        {
            var items = new EndorsementChangeCollection();
            var changeItem = new CoveredAutoDesignationSymbolChangeItem();

            var newRiskDetail = (RiskDTO)newRiskDetailObj;
            var oldRiskDetail = (RiskDTO)oldRiskDetailObj;

            var hasChanges = false;

            var newRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == newRiskDetail.Id && e.FormId == "CoveredAutoDesignationSymbol");
            var oldRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == oldRiskDetail.Id && e.FormId == "CoveredAutoDesignationSymbol");

            var newCoverageLimit = newRiskDetail.RiskCoverages.FirstOrDefault();
            var oldCoverageLimit = oldRiskDetail.RiskCoverages.FirstOrDefault();

            if ((newRiskForm?.Other != null && oldRiskForm?.Other == null) || (newRiskForm?.Other == null && oldRiskForm?.Other != null) ||
                (newRiskForm?.IsSelected == true && oldRiskForm?.IsSelected == false) || (newRiskForm?.IsSelected == false && oldRiskForm?.IsSelected == true))
            {
                hasChanges = true;
            }

            if (!hasChanges && newRiskForm?.Other != null && oldRiskForm?.Other != null)
            {
                var newForm = JsonConvert.DeserializeObject<CoveredAutoDesigSymbolDTO>(newRiskForm?.Other);
                var oldForm = JsonConvert.DeserializeObject<CoveredAutoDesigSymbolDTO>(oldRiskForm?.Other);

                if (newForm.CoveredAutoText != oldForm.CoveredAutoText)
                {
                    changeItem.CoveredAutoText = newForm.CoveredAutoText;
                    hasChanges = true;
                }

                if (newForm.TextForEndorsement != oldForm.TextForEndorsement)
                {
                    changeItem.TextForEndorsement = newForm.TextForEndorsement;
                    hasChanges = true;
                }

                if (oldCoverageLimit.ALSymbolId != newCoverageLimit.ALSymbolId &&
                    (newCoverageLimit.ALSymbolId == 14 || newCoverageLimit.ALSymbolId == 15))
                {
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                items.Add(changeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
