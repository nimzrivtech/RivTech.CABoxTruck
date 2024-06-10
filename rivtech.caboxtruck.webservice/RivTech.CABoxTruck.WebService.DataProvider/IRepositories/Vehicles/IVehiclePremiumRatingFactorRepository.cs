using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles
{
    public interface IVehiclePremiumRatingFactorRepository
    {
        Task DeleteAsync(VehiclePremiumRatingFactor vehiclePremiumRatingFactor);
        Task CreateAsync(params VehiclePremiumRatingFactor[] ratingFactors);
        Task DeleteAsync(Guid vehicleId, Guid riskDetailId);
        Task<IEnumerable<VehiclePremiumRatingFactor>> GetByRiskDetailAsync(Guid riskDetailId, int? option = null);
        Task<VehiclePremiumRatingFactor> GetByVehicleAsync(Guid vehicleId, Guid riskDetailId, int? option = null);
    }
}
