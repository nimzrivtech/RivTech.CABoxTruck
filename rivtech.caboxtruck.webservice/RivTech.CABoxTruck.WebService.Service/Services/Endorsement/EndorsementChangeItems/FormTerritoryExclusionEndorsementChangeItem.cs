using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormTerritoryExclusionEndorsementChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormTerritoryExclusionEndorsementChangeItem);

        public string DisplayText => "";

        public string States { get; set; }

        public bool? StatesDefault { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
