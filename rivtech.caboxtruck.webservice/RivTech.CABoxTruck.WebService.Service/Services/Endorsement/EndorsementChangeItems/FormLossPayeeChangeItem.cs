using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormLossPayeeChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormLossPayeeChangeItem);

        public string DisplayText => "";
        public string DescriptionOfAutos { get; set; }
    }
}
