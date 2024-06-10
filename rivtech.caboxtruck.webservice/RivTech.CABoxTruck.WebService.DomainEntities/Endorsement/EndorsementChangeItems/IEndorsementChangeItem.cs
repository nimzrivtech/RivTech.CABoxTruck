using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems
{
    public interface IEndorsementChangeItem
    {
        string Type { get; }
        string DisplayText { get; }
    }
}
