using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Vehicles
{
    public class VehiclePremiumRatingFactorRepository : IVehiclePremiumRatingFactorRepository
    {
        private readonly AppDbContext _dbContext;

        public VehiclePremiumRatingFactorRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task CreateAsync(params VehiclePremiumRatingFactor[] ratingFactors)
        {
            foreach(var ratingFactor in ratingFactors)
            {
                await _dbContext.VehiclePremiumRatingFactor.AddAsync(ratingFactor);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(VehiclePremiumRatingFactor premiumFactor)
        {
            _dbContext.VehiclePremiumRatingFactor.Remove(premiumFactor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<VehiclePremiumRatingFactor>> GetByRiskDetailAsync(Guid riskDetailId, int? option = null)
        {
            return await _dbContext.VehiclePremiumRatingFactor.Where(x => x.RiskDetailId == riskDetailId).AsNoTracking().ToListAsync();
        }

        public async Task<VehiclePremiumRatingFactor> GetByVehicleAsync(Guid vehicleId, Guid riskDetailId, int? option = null)
        {
            if (option.HasValue)
                return await _dbContext.VehiclePremiumRatingFactor.AsNoTracking().FirstOrDefaultAsync(x => x.VehicleId == vehicleId.ToString() && x.RiskDetailId == riskDetailId && x.OptionId == option);
            return await _dbContext.VehiclePremiumRatingFactor.AsNoTracking().FirstOrDefaultAsync(x => x.VehicleId == vehicleId.ToString() && x.RiskDetailId == riskDetailId);
        }

        public async Task DeleteAsync(Guid vehicleId, Guid riskDetailId)
        {
            var premiumFactors = await _dbContext.VehiclePremiumRatingFactor
                .Where(x => x.VehicleId == vehicleId.ToString()
                    && x.RiskDetailId == riskDetailId)
                .ToListAsync();

            foreach(var premiumFactor in premiumFactors)
            {
                _dbContext.VehiclePremiumRatingFactor.Remove(premiumFactor);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
