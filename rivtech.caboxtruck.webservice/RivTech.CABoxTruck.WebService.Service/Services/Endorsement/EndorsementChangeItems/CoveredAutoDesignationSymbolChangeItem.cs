using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class CoveredAutoDesignationSymbolChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(CoveredAutoDesignationSymbolChangeItem);

        public string DisplayText => "";
        public string CoveredAutoText { get; set; }
        public string TextForEndorsement { get; set; }

    }
}
