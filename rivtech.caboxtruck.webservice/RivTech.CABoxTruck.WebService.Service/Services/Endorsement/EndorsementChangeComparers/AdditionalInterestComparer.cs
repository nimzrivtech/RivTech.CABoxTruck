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
    public class AdditionalInterestComparer : IChangeComparer
    {
        private readonly IBindingService _bindingService;
        public AdditionalInterestComparer(IBindingService bindingService)
        {
            _bindingService = bindingService;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;
            if (newRiskDTO?.AdditionalInterest.Count() != oldRiskDTO?.AdditionalInterest.Count())
                hasChanges = true;

            if (!hasChanges && newRiskDTO?.AdditionalInterest.Count() > 0 && oldRiskDTO?.AdditionalInterest.Count() > 0)
            {
                var compareList = new List<string>();
                foreach (var item in oldRiskDTO?.AdditionalInterest)
                {
                    compareList.Add(
                        $"{item.AdditionalInterestTypeId}" +
                        $"{item.Name}" +
                        $"{item.Address}" +
                        $"{item.City}" +
                        $"{item.State}" +
                        $"{item.ZipCode}" +
                        $"{item.EffectiveDate}" +
                        $"{item.ExpirationDate}" +
                        $"{item.IsActive}" +
                        $"{item.IsAutoLiability}" +
                        $"{item.IsGeneralLiability}" +
                        $"{item.IsPrimaryNonContributory}" +
                        $"{item.IsWaiverOfSubrogation}"
                        );
                }
                foreach (var item in newRiskDTO?.AdditionalInterest)
                {
                    var compareItem = $"{item.AdditionalInterestTypeId}" +
                        $"{item.Name}" +
                        $"{item.Address}" +
                        $"{item.City}" +
                        $"{item.State}" +
                        $"{item.ZipCode}" +
                        $"{item.EffectiveDate}" +
                        $"{item.ExpirationDate}" +
                        $"{item.IsActive}" +
                        $"{item.IsAutoLiability}" +
                        $"{item.IsGeneralLiability}" +
                        $"{item.IsPrimaryNonContributory}" +
                        $"{item.IsWaiverOfSubrogation}";
                    var isExist = compareList.Any(x => x == compareItem);
                    if (!isExist)
                        hasChanges = true;
                }
            }

            if (hasChanges)
            {
                var additionalInterestChangeItem = new AdditionalInterestChangeItem();
                additionalInterestChangeItem.AdditionalInterests = newRiskDTO.AdditionalInterest;
                items.Add(additionalInterestChangeItem);
            }

            return items.Count() > 0 ? items : null;
        }
    }
}
