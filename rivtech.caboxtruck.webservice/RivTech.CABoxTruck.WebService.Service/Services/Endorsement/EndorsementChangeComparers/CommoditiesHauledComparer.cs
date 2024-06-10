using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class CommoditiesHauledComparer : IChangeComparer
    {
        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var commoditiesHauledChangeItem = new CommoditiesHauledChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var commoditiesHauledDtos = newRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled;

            if (newRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled.Count() != oldRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled.Count())
                hasChanges = true;

            if (!hasChanges && newRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled.Count() > 0 && oldRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled.Count() > 0)
            {
                var compareList = new List<string>();
                foreach (var item in oldRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled)
                {
                    compareList.Add(
                        $"{item.Commodity}" +
                        $"{item.PercentageOfCarry}"
                        );
                }
                foreach (var item in newRiskDTO?.GeneralLiabilityCargo.CommoditiesHauled)
                {
                    var compareItem = $"{item.Commodity}" +
                        $"{item.PercentageOfCarry}";
                    var isExist = compareList.Any(x => x == compareItem);
                    if (!isExist)
                        hasChanges = true;
                }
            }

            if (hasChanges)
            {
                commoditiesHauledChangeItem.CommoditiesHauled = commoditiesHauledDtos;
                items.Add(commoditiesHauledChangeItem);
            }
            else
            {
                items = null;
            }

            return await Task.FromResult(items);
        }
    }

}
