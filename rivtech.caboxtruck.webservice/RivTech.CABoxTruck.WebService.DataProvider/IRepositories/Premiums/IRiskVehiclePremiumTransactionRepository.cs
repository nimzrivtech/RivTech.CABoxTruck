using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums
{
    public interface IRiskVehiclePremiumTransactionRepository
    {
        Task<RiskVehiclePremiumTransaction> GetAsync(Guid newRiskDetailId, Guid vehicleMainId, bool fromReinstatement = false);
        Task<IEnumerable<RiskVehiclePremiumTransaction>> GetAsync(Guid riskDetailId, bool fromReinstatement = false);
        Task<RiskVehiclePremiumTransaction> CreateAsync(RiskVehiclePremiumTransaction newPremiumTransaction);
        Task DeleteAsync(RiskVehiclePremiumTransaction newPremiumTransaction);
    }
}
