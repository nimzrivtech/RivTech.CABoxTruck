using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class VehicleReinstateChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(VehicleReinstateChangeItem);

        public string DisplayText => "";

        public string VIN { get; set; }
        public Guid VehicleId { get; set; }
        public Guid MainId { get; set; }
    }
}
