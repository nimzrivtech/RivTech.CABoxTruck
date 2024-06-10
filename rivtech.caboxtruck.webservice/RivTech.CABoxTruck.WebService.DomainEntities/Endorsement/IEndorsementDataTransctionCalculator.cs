using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementDataTransactionCalculator
    {
        Task CalculateChangesAsync(IEndorsementData lastProcessedEndorsementData, IEndorsementData previousEndorsementData);
    }
}
