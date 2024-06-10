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
    public class AddressesComparer : IChangeComparer
    {
        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var addressesChangeItem = new AddressesChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newAddresses = newRiskDTO?.NameAndAddress?.Addresses;
            var newAddressesToIssue = newRiskDTO?.NameAndAddress?.Addresses;

            if (newAddresses != null)
            {
                foreach (var newAddress in newAddressesToIssue)
                {
                    var oldAddress = oldRiskDTO?.NameAndAddress?.Addresses.FirstOrDefault(x => x.AddressId == newAddress.AddressId);

                    //If old address is empty, then add the new address
                    if (oldAddress == null)
                    {
                        hasChanges = true;
                    }
                }
            }

            if (hasChanges)
            {
                addressesChangeItem.Addresses = newAddresses;
                items.Add(addressesChangeItem);
            }
            else
            {
                items = null;
            }

            return await Task.FromResult(items);
        }
    }
}
