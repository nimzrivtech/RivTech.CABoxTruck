using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems
{
    public interface IEndorsementChangeItemModifier
    {
        Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem);
    }
}