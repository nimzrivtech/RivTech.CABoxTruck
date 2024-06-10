using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class AddressesChangeItem : IEndorsementChangeItem
    {
        public const string TYPE = nameof(AddressesChangeItem);

        public string Type => TYPE;

        public string DisplayText => $"";
        public List<EntityAddressDTO> Addresses { get; set; }
    }
}
