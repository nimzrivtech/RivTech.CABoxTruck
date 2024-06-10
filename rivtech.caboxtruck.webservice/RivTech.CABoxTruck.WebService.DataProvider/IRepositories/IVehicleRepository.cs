using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IVehicleRepository : IRepository<Vehicle, Guid>
    {
        Task<IEnumerable<Vehicle>> GetAllIncludeAsync();
        Task<IEnumerable<Vehicle>> GetAllIncludeAsync(Expression<Func<Vehicle, bool>> expression);
        Task<IEnumerable<Vehicle>> GetByRiskDetailAsync(Guid riskDetailId);
        Task<IEnumerable<Vehicle>> GetByRiskDetailAsync(Guid riskDetailId, bool? fromPolicy = false, short? optionNumber = 1);
        Task<Vehicle> GetByRiskDetailAndMainIdAsync(Guid riskDetailId, Guid mainId);
        Vehicle GetById(Guid vehicleId);

        /// <summary>
        /// Returns the the vehicle from previously issued RiskDetail.
        /// </summary>
        Task<IEnumerable<Vehicle>> GetPreviousVehiclesByRiskDetail(RiskDetail riskDetail, bool? fromPolicy = false);

        Task<bool> IsFromSubmission(Guid vehicleId);
        bool IsOldVersion(Guid? id);
        VehiclePremiumRatingFactor GetRatingFactors(Guid vehicleId, Guid riskDetailId);
        Task<bool> UpdateRatingFactors(VehiclePremiumRatingFactor ratingFactors);
        Task<IEnumerable<VehiclePremiumRatingFactor>> GetPremiumRatingFactorsByRiskAsync(Guid riskDetailId, int optionNumber);
        Task DeletePremiumRatingFactorAsync(long id);
        Task<Vehicle> GetVersionFromPreviousRiskDetailVersion(Guid? riskDetailId, Guid mainId);
        Task<Vehicle> GetByIdAsync(Guid vehicleId);
    }
}
