using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormExclusionRadiusEndorsementChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormExclusionRadiusEndorsementChangeItem);

        public string DisplayText => "";
        public int? Radius { get; set; }
    }
}
