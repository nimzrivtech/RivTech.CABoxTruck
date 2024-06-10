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
    public class FormTrailerInterchangeCoverageComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public FormTrailerInterchangeCoverageComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            FormTrailerInterchangeCoverageChangeItem FormTrailerInterchangeCoverageChangeItem = null;

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == newRiskDTO.Id.Value && e.FormId == "TrailerInterchangeCoverage");
            var oldRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == oldRiskDTO.Id.Value && e.FormId == "TrailerInterchangeCoverage");

            if ((newRiskForm.Other != null && oldRiskForm.Other == null) || (newRiskForm.Other == null && oldRiskForm.Other != null) ||
                (newRiskForm.IsSelected == true && oldRiskForm.IsSelected == false) || (newRiskForm.IsSelected == false && oldRiskForm.IsSelected == true))
            {
                hasChanges = true;
            }

            if (!hasChanges && newRiskForm.Other != null && oldRiskForm.Other != null)
            {
                var newForm = JsonConvert.DeserializeObject<TrailerInterchangeDTO>(newRiskForm?.Other);
                var oldForm = JsonConvert.DeserializeObject<TrailerInterchangeDTO>(oldRiskForm?.Other);
                if (newForm.CompreLimitOfInsurance != oldForm.CompreLimitOfInsurance)
                    hasChanges = true;
                if (newForm.CompreDeductible != oldForm.CompreDeductible)
                    hasChanges = true;
                if (newForm.ComprePremium != oldForm.ComprePremium)
                    hasChanges = true;
                if (newForm.LossLimitOfInsurance != oldForm.LossLimitOfInsurance)
                    hasChanges = true;
                if (newForm.LossDeductible != oldForm.LossDeductible)
                    hasChanges = true;
                if (newForm.LossPremium != oldForm.LossPremium)
                    hasChanges = true;
                if (newForm.CollisionLimitOfInsurance != oldForm.CollisionLimitOfInsurance)
                    hasChanges = true;
                if (newForm.CollisionDeductible != oldForm.CollisionDeductible)
                    hasChanges = true;
                if (newForm.CollisionPremium != oldForm.CollisionPremium)
                    hasChanges = true;
                if (newForm.FireLimitOfInsurance != oldForm.FireLimitOfInsurance)
                    hasChanges = true;
                if (newForm.FireDeductible != oldForm.FireDeductible)
                    hasChanges = true;
                if (newForm.FirePremium != oldForm.FirePremium)
                    hasChanges = true;
                if (newForm.FireTheftLimitOfInsurance != oldForm.FireTheftLimitOfInsurance)
                    hasChanges = true;
                if (newForm.FireTheftDeductible != oldForm.FireTheftDeductible)
                    hasChanges = true;
                if (newForm.FireTheftPremium != oldForm.FireTheftPremium)
                    hasChanges = true;
            }

            if (hasChanges)
            {
                FormTrailerInterchangeCoverageChangeItem = JsonConvert.DeserializeObject<FormTrailerInterchangeCoverageChangeItem>(newRiskForm.Other ?? "");
                items.Add(FormTrailerInterchangeCoverageChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
