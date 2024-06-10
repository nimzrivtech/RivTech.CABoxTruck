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
    public class FormLossPayeeComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public FormLossPayeeComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            FormLossPayeeChangeItem formLossPayeeChangeItem = null;

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == newRiskDTO.Id.Value && e.FormId == "LossPayeeEndorsement");
            var oldRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == oldRiskDTO.Id.Value && e.FormId == "LossPayeeEndorsement");

            if (newRiskForm.Other != oldRiskForm.Other)
            {
                if (newRiskForm.Other != null)
                {
                    formLossPayeeChangeItem = JsonConvert.DeserializeObject<FormLossPayeeChangeItem>(newRiskForm.Other);
                }
                hasChanges = true;
            }

            if (hasChanges)
            {
                items.Add(formLossPayeeChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
