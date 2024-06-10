using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Policy
{
    public class ReinstatementBreakdownRepository : BaseRepository<ReinstatementBreakdown, long>, IReinstatementBreakdownRepository
    {
        public ReinstatementBreakdownRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<ReinstatementBreakdown> GetByRiskDetailId(Guid riskDetailId)
        {
            return _context.ReinstatementBreakdown.Where(x => x.RiskDetailId == riskDetailId).ToList()
                ?? new List<ReinstatementBreakdown>();
        }
    }
}
