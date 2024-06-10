using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems
{
    public interface IEndorsementChangeItemModifierResolver
    {
        /// <summary>
        /// Returns a modifier object based on the given change item.
        /// </summary>
        IEndorsementChangeItemModifier Resolve(IEndorsementChangeItem changeItem);
    }
}