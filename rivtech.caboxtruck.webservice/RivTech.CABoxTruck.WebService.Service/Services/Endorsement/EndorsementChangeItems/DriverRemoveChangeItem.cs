using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class DriverRemoveChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(DriverRemoveChangeItem);

        public string DisplayText => "";

        public Guid DriverId { get; set; }
        public DateTime DeletedDate { get; set; }
        public string LicenseNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
