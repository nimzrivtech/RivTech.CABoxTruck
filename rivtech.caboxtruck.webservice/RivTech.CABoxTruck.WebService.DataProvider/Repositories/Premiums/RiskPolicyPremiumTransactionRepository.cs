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
    public class RiskPolicyPremiumTransactionRepository : IRiskPolicyPremiumTransactionRepository
    {
        private readonly AppDbContext _db;

        public RiskPolicyPremiumTransactionRepository(
            AppDbContext db
        ) {
            _db = db;
        }

        public async Task<RiskPolicyPremiumTransaction> CreateAsync(RiskPolicyPremiumTransaction newPremiumTransaction)
        {
            var entry = await _db.AddAsync(newPremiumTransaction);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(RiskPolicyPremiumTransaction premiumTransaction)
        {
            _db.RiskPolicyPremiumTransaction.Remove(premiumTransaction);
            await _db.SaveChangesAsync();
        }

        public async Task<RiskPolicyPremiumTransaction> GetAsync(Guid riskDetailId, string premiumType, bool fromReinstatement = false)
        {
            return await _db.RiskPolicyPremiumTransaction.FirstOrDefaultAsync(x => 
                        x.RiskDetailId == riskDetailId 
                        && x.PremiumType == premiumType
                        && x.FromReinstatement == fromReinstatement);
        }
    }
}
