using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems
{
    /// <summary>
    ///  Collection interface of IEndorsementChangeItem.
    /// </summary>
    public interface IEndorsementChangeCollection : IEnumerable<IEndorsementChangeItem> 
    {
        void Add(IEndorsementChangeItem changeItem);
    }
}
