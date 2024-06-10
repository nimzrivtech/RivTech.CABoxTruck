using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class VehicleAddChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(VehicleAddChangeItem);
        public string DisplayText => "";

        public VehicleDTO[] Vehicles { get; set; }
    }
}
