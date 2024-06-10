using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class CancellationBreakdownRepository : BaseRepository<CancellationBreakdown, long>, ICancellationBreakdownRepository
    {
        public CancellationBreakdownRepository(AppDbContext context) 
            : base(context) 
        {
        }

        public async Task<IEnumerable<CancellationBreakdown>> GetAllByAmountTypeAsync(string amountType)
        {
            return await _context.CancellationBreakdown.Where(x => x.AmountSubTypeId == amountType).ToListAsync();
        }

        public async Task<IEnumerable<CancellationBreakdown>> GetAllByRiskDetailIdAmountTypeAsync(Guid riskDetailId, string amountType)
        {
            return await _context.CancellationBreakdown.Where(x => x.RiskDetailId == riskDetailId && x.AmountSubTypeId == amountType).ToListAsync();
        }

        public IEnumerable<CancellationBreakdown> GetByRiskDetailId(Guid riskDetailId)
        {
            return _context.CancellationBreakdown.Where(x => x.RiskDetailId == riskDetailId).ToList()
                ?? new List<CancellationBreakdown>();
        }
    }
}
