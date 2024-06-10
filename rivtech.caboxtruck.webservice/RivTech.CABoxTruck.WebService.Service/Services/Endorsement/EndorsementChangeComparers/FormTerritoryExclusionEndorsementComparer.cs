using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class FormTerritoryExclusionEndorsementComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public FormTerritoryExclusionEndorsementComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            FormTerritoryExclusionEndorsementChangeItem formTerritoryExclusionEndorsementChangeItem = null;

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == newRiskDTO.Id.Value && e.FormId == "TerritoryExclusion");
            var oldRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == oldRiskDTO.Id.Value && e.FormId == "TerritoryExclusion");

            if (newRiskForm.Other != oldRiskForm.Other)
            {
                if (newRiskForm.Other != null)
                {
                    formTerritoryExclusionEndorsementChangeItem = JsonConvert.DeserializeObject<FormTerritoryExclusionEndorsementChangeItem>(newRiskForm.Other);
                }
                hasChanges = true;
            }

            if (hasChanges)
            {
                items.Add(formTerritoryExclusionEndorsementChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
