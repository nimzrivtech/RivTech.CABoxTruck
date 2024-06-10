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
    public class ManuscriptComparer : IChangeComparer
    {
        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;
            if (newRiskDTO?.RiskManuscripts.Count() != oldRiskDTO?.RiskManuscripts.Count())
                hasChanges = true;

            if (!hasChanges && newRiskDTO?.RiskManuscripts.Count() > 0 && oldRiskDTO?.RiskManuscripts.Count() > 0)
            {
                var compareList = new List<string>();
                foreach (var item in oldRiskDTO?.RiskManuscripts)
                {
                    compareList.Add(
                        $"{item.Title}" +
                        $"{item.Premium}" +
                        $"{item.Description}" +
                        $"{item.EffectiveDate}" +
                        $"{item.ExpirationDate}" +
                        $"{item.IsProrate}" +
                        $"{item.PremiumType}" +
                        $"{item.DeletedDate}"
                        );
                }
                foreach (var item in newRiskDTO?.RiskManuscripts)
                {
                    var compareItem = $"{item.Title}" +
                        $"{item.Premium}" +
                        $"{item.Description}" +
                        $"{item.EffectiveDate}" +
                        $"{item.ExpirationDate}" +
                        $"{item.IsProrate}" +
                        $"{item.PremiumType}" +
                        $"{item.DeletedDate}";
                    var isExist = compareList.Any(x => x == compareItem);
                    if (!isExist)
                        hasChanges = true;
                }
            }

            if (hasChanges)
            {
                var manusciptChangeItem = new ManusciptChangeItem();
                manusciptChangeItem.RiskManuscripts = newRiskDTO.RiskManuscripts;
                items.Add(manusciptChangeItem);
            }


            return items.Count() > 0 ? items : null;
        }
    }
}
