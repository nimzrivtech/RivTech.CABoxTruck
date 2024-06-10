using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class BusinessNameComparer : IChangeComparer
    {
        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var businessNameChangeItem = new BusinessNameChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            if (newRiskDTO.NameAndAddress.BusinessName != oldRiskDTO.NameAndAddress.BusinessName)
            {
                businessNameChangeItem.BusinessName = newRiskDTO.NameAndAddress.BusinessName;
                hasChanges = true;
            }

            if (hasChanges)
            {
                items.Add(businessNameChangeItem);
            }
            else
            {
                items = null;
            }

            return await Task.FromResult(items);
        }
    }
}
