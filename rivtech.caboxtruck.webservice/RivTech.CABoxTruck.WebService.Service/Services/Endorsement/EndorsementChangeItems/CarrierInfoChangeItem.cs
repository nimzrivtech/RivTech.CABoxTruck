using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class CarrierInfoChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(CarrierInfoChangeItem);

        public string DisplayText => "";
        public short? CreditedOfficeId { get; set; }
        public string SurplusLIneNum { get; set; }
    }
}
