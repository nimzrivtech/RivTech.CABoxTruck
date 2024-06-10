using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public interface IEndorsementDataPremiumProrator
    {
        Task ProratePremiumsAsync(IEndorsementData endorsementData);
    }
}