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
    public class CarrierInfoComparer : IChangeComparer
    {
        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var carrierInfoChangeItem = new CarrierInfoChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            if (newRiskDTO.Binding.CreditedOfficeId != oldRiskDTO.Binding.CreditedOfficeId)
            {
                carrierInfoChangeItem.CreditedOfficeId = newRiskDTO.Binding.CreditedOfficeId;
                hasChanges = true;
            }

            if (newRiskDTO.Binding.SurplusLIneNum != oldRiskDTO.Binding.SurplusLIneNum)
            {
                carrierInfoChangeItem.SurplusLIneNum = newRiskDTO.Binding.SurplusLIneNum;
                hasChanges = true;
            }

            if (hasChanges)
            {
                items.Add(carrierInfoChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
