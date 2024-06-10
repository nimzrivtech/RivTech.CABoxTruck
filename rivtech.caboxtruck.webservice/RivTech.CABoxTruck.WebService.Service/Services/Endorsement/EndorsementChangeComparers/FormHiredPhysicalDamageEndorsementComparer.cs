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

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class FormHiredPhysicalDamageEndorsementComparer : IChangeComparer
    {
        private readonly IRiskFormRepository _riskFormRepository;
        public FormHiredPhysicalDamageEndorsementComparer(IRiskFormRepository riskFormRepository)
        {
            _riskFormRepository = riskFormRepository;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDetailObj, object oldRiskDetailObj)
        {
            var items = new EndorsementChangeCollection();
            var formHiredPhysicalDamageEndorsementChangeItem = new FormHiredPhysicalDamageEndorsementChangeItem();

            var newRiskDetail = (RiskDTO)newRiskDetailObj;
            var oldRiskDetail = (RiskDTO)oldRiskDetailObj;

            var hasChanges = false;

            var newRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == newRiskDetail.Id && e.FormId == "HiredPhysicalDamage");
            var oldRiskForm = await _riskFormRepository.FindAsync(e => e.RiskDetailId == oldRiskDetail.Id && e.FormId == "HiredPhysicalDamage");

            if ((newRiskForm.Other != null && oldRiskForm.Other == null) || (newRiskForm.Other == null && oldRiskForm.Other != null) ||
                (newRiskForm.IsSelected == true && oldRiskForm.IsSelected == false) || (newRiskForm.IsSelected == false && oldRiskForm.IsSelected == true))
            {
                hasChanges = true;
            }

            if (!hasChanges && newRiskForm?.Other != null && oldRiskForm?.Other != null)
            {
                var newForm = JsonConvert.DeserializeObject<HiredPhysdamDTO>(newRiskForm?.Other);
                var oldForm = JsonConvert.DeserializeObject<HiredPhysdamDTO>(oldRiskForm?.Other);

                if (newForm.EstimatedAdditionalPremium != oldForm.EstimatedAdditionalPremium)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.EstimatedAdditionalPremium = newForm.EstimatedAdditionalPremium;
                    hasChanges = true;
                }

                if (newForm.VehicleDays != oldForm.VehicleDays)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.VehicleDays = newForm.VehicleDays;
                    hasChanges = true;
                }

                if (newForm.RatePerDay != oldForm.RatePerDay)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.RatePerDay = newForm.RatePerDay;
                    hasChanges = true;
                }

                if (newForm.GrossCombinedWeight != oldForm.GrossCombinedWeight)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.GrossCombinedWeight = newForm.GrossCombinedWeight;
                    hasChanges = true;
                }

                if (newForm.AccidentLimitPerVehicle != oldForm.AccidentLimitPerVehicle)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.AccidentLimitPerVehicle = newForm.AccidentLimitPerVehicle;
                    hasChanges = true;
                }

                if (newForm.Deductible != oldForm.Deductible)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.Deductible = newForm.Deductible;
                    hasChanges = true;
                }

                if (newForm.PhysdamLimit != oldForm.PhysdamLimit)
                {
                    formHiredPhysicalDamageEndorsementChangeItem.PhysdamLimit = newForm.PhysdamLimit;
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                items.Add(formHiredPhysicalDamageEndorsementChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
