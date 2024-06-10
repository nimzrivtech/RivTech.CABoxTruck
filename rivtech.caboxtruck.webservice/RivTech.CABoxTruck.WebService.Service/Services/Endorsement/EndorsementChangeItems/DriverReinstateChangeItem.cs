using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class DriverReinstateChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(DriverReinstateChangeItem);

        public string DisplayText => "";

        public Guid DriverId { get; set; }
        public string LicenseNumber { get; set; }
        public object FirstName { get; set; }
        public object LastName { get; set; }
    }
}
