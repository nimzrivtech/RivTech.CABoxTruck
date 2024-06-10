using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class BusinessDetailsChangeItem : IEndorsementChangeItem
    {
        public short? YearsInBusiness { get; set; }

        public string Type => nameof(BusinessDetailsChangeItem);

        public string DisplayText => "";
    }
}
