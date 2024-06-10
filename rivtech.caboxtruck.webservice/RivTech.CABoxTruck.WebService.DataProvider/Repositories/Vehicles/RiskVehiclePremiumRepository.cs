using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Vehicles
{
    public class RiskVehiclePremiumRepository : IRiskVehiclePremiumRepository
    {
        public Task CreateAsync(RiskVehiclePremiumCollection riskVehiclePremiums)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RiskVehiclePremiumCollection>> GetByRiskDetailAsync(Guid riskDetailId)
        {
            throw new NotImplementedException();
        }

        public Task<RiskVehiclePremiumCollection> GetByVehicleAsync(Guid vehicleMainId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RiskVehiclePremiumCollection riskVehiclePremiums)
        {
            throw new NotImplementedException();
        }
    }
}
