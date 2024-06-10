using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums
{
    public interface IRiskPolicyPremiumRepository
    {
        Task<RiskPolicyPremiumCollection> GetCurrentAsync(Guid riskId, string premiumType);
        Task<RiskPolicyPremiumCollection> GetByRiskDetailAsync(Guid riskDetailId, string premiumType, bool fromReinstatement = false);
        Task UpdateAsync(RiskPolicyPremiumCollection currentRiskVehiclePremium);
        Task DeleteAsync(RiskPolicyPremiumCollection riskPolicyPremiums);
        Task SaveAsync(RiskPolicyPremiumCollection riskPolicyPremiums);
    }
}
