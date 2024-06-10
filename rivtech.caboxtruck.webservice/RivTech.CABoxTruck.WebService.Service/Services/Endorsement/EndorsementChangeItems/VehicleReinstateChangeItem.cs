using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class VehicleRemoveChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(VehicleRemoveChangeItem);

        public string DisplayText => "";

        public string VIN { get; set; }
        public Guid VehicleId { get; set; }
        public Guid MainId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
