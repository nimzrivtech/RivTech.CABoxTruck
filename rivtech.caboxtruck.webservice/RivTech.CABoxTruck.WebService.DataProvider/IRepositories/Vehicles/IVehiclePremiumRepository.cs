using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IVehiclePremiumRepository : IRepository<VehiclePremium, int>
    {
        Task<VehiclePremium> GetByRiskDetailVehicleIdAsync(Guid riskDetailId, Guid vehicleId);
        Task<IEnumerable<VehiclePremium>> GetByRiskDetail(Guid riskDetailId);
        Task<VehiclePremium> GetPrevious(VehiclePremium vehiclePremium);
        Task<IEnumerable<VehiclePremium>> GetByMainId(Guid mainId, bool? activeRiskDetailOnly = null);
        Task<VehiclePremium> GetLatestByMainId(Guid mainId);
        Task<VehiclePremium> GetPreviousActive(VehiclePremium vehiclePremium);
        //Task UpdateAsync(VehiclePremium vehiclePremium);
        VehiclePremium Update(VehiclePremium vehiclePremium); 
    }
}
