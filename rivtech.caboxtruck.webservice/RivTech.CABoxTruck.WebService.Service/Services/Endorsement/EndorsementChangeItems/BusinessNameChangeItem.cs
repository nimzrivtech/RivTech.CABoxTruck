using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class BusinessNameChangeItem : IEndorsementChangeItem
    {
        public const string TYPE = nameof(BusinessNameChangeItem);

        public string Type => TYPE;

        public string DisplayText => $"Changed Business Name to {BusinessName}";

        public string BusinessName { get;  set; }
    }
}
