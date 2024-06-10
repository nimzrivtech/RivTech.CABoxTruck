using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums
{
    public interface IRiskPolicyPremiumTransactionRepository
    {
        Task<RiskPolicyPremiumTransaction> CreateAsync(RiskPolicyPremiumTransaction newPremiumTransaction);
        Task DeleteAsync(RiskPolicyPremiumTransaction newPremiumTransaction);
        Task<RiskPolicyPremiumTransaction> GetAsync(Guid value, string premiumType, bool fromReinstatement = false);
    }
}
