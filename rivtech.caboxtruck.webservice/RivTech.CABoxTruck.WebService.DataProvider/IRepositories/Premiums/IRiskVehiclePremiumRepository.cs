using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums
{
    public interface IRiskVehiclePremiumRepository
    {
        Task<RiskVehiclePremiumCollection> GetByVehicleAsync(Guid riskDetailId, Guid mainId, bool fromReinstatement = false);
        Task<IEnumerable<RiskVehiclePremiumCollection>> GetByRiskDetailAsync(Guid riskDetailId, bool fromReinstatement = false);
        Task SaveAsync(RiskVehiclePremiumCollection riskVehiclePremiums);
        Task DeleteAsync(RiskVehiclePremiumCollection currentRiskVehiclePremium);
        Task<RiskVehiclePremiumCollection> GetByVehicleByRiskAsync(Guid riskId, Guid riskDetailId, Guid vehicleMainId, bool fromReinstate = false);
    }
}
