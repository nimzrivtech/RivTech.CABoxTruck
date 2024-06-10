using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormDriverRequirementsEndorsementChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormDriverRequirementsEndorsementChangeItem);

        public string DisplayText => "";
        public string Requirement { get; set; }
    }
}
