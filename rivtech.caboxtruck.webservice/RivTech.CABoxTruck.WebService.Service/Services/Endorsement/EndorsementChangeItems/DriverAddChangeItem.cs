using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class DriverAddChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(DriverAddChangeItem);
        public string DisplayText => "";

        public IEnumerable<DriverDTO> Drivers { get; set; }
    }
}
