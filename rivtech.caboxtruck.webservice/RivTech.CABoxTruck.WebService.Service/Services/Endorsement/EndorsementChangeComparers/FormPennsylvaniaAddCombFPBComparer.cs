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
    public class FormPennsylvaniaAddCombFPBComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public FormPennsylvaniaAddCombFPBComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            FormPennsylvaniaAddCombFPBChangeItem formPennsylvaniaAddCombFPBChangeItem = null;

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == newRiskDTO.Id.Value && e.FormId == "PennsylvaniaAddCombFirstPartyBenefitsEndorsement");
            var oldRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == oldRiskDTO.Id.Value && e.FormId == "PennsylvaniaAddCombFirstPartyBenefitsEndorsement");

            if ((newRiskForm.Other != null && oldRiskForm.Other == null) || (newRiskForm.Other == null && oldRiskForm.Other != null) ||
                (newRiskForm.IsSelected == true && oldRiskForm.IsSelected == false) || (newRiskForm.IsSelected == false && oldRiskForm.IsSelected == true))
            {
                hasChanges = true;
            }

            if (!hasChanges && newRiskForm.Other != null && oldRiskForm.Other != null)
            {
                var newForm = JsonConvert.DeserializeObject<FormPAaddcombFPBDTO>(newRiskForm?.Other);
                var oldForm = JsonConvert.DeserializeObject<FormPAaddcombFPBDTO>(oldRiskForm?.Other);
                if (newForm.AddedFirstPartyBenefits != oldForm.AddedFirstPartyBenefits)
                    hasChanges = true;
                if (newForm.MedicalExpenseBenefits != oldForm.MedicalExpenseBenefits)
                    hasChanges = true;
                if (newForm.WorkLossUpTo != oldForm.WorkLossUpTo)
                    hasChanges = true;
                if (newForm.WorkLossPerMonth != oldForm.WorkLossPerMonth)
                    hasChanges = true;
                if (newForm.FuneralExpenseBenefits != oldForm.FuneralExpenseBenefits)
                    hasChanges = true;
                if (newForm.AccidentalDeathBenefits != oldForm.AccidentalDeathBenefits)
                    hasChanges = true;
                if (newForm.CombinationFirstPartyBenefits != oldForm.CombinationFirstPartyBenefits)
                    hasChanges = true;
                if (newForm.MaximumTotalLimitAllBenefits != oldForm.MaximumTotalLimitAllBenefits)
                    hasChanges = true;
                if (newForm.CombFuneralExpenseBenefits != oldForm.CombFuneralExpenseBenefits)
                    hasChanges = true;
                if (newForm.CombAccidentalDeathBenefits != oldForm.CombAccidentalDeathBenefits)
                    hasChanges = true;
            }

            if (hasChanges)
            {
                formPennsylvaniaAddCombFPBChangeItem = JsonConvert.DeserializeObject<FormPennsylvaniaAddCombFPBChangeItem>(newRiskForm.Other);
                items.Add(formPennsylvaniaAddCombFPBChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
