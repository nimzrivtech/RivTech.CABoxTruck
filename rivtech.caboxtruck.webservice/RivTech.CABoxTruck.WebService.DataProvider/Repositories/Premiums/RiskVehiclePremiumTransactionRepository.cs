using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Premiums
{
    public class RiskVehiclePremiumTransactionRepository : IRiskVehiclePremiumTransactionRepository
    {
        private readonly AppDbContext _db;

        public RiskVehiclePremiumTransactionRepository(
            AppDbContext db
        ) {
            _db = db;
        }

        public async Task<RiskVehiclePremiumTransaction> CreateAsync(RiskVehiclePremiumTransaction newPremiumTransaction)
        {
            var entry = await _db.AddAsync(newPremiumTransaction);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(RiskVehiclePremiumTransaction newPremiumTransaction)
        {
            _db.RiskVehiclePremiumTransaction.Remove(newPremiumTransaction);
            await _db.SaveChangesAsync();
        }

        public async Task<RiskVehiclePremiumTransaction> GetAsync(Guid newRiskDetailId, Guid vehicleMainId, bool fromReinstatement = false)
        {
            var item = await _db.RiskVehiclePremiumTransaction.FirstOrDefaultAsync(x => 
                        x.RiskDetailId == newRiskDetailId 
                        && x.VehicleMainId == vehicleMainId
                        && x.FromReinstatement == fromReinstatement);
            return item;
        }

        public async Task<IEnumerable<RiskVehiclePremiumTransaction>> GetAsync(Guid riskDetailId, bool fromReinstatement = false)
        {
            var item = _db.RiskVehiclePremiumTransaction.Where(x => x.RiskDetailId == riskDetailId && x.FromReinstatement == fromReinstatement);
            return await Task.FromResult(item);
        }
    }
}
