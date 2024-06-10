using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormSelectionChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormSelectionChangeItem);

        public string DisplayText => "";
        public string Id { get; set; }
        public bool? IsSelected { get; set; }
        public string Other { get; set; }
    }
}
