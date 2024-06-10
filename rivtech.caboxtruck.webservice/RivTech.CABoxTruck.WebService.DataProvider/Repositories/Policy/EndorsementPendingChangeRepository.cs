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
    public class EndorsementPendingChangeRepository : BaseRepository<EndorsementPendingChange, long>, IEndorsementPendingChangeRepository
    {
        public EndorsementPendingChangeRepository(AppDbContext context) 
            : base(context) 
        {
        }


        public IEnumerable<EndorsementPendingChange> GetByRiskDetail(Guid riskDetailId)
        {
            return _context.EndorsementPendingChanges.Where(x => x.RiskDetailId == riskDetailId).AsQueryable();
        }

        public async Task<IEnumerable<EndorsementPendingChange>> GetByRiskDetailAsync(Guid riskDetailId)
        {
            return await _context.EndorsementPendingChanges.Where(x => x.RiskDetailId == riskDetailId).ToListAsync();
        }

        public void Delete(EndorsementPendingChange endorsementPendingChange)
        {
            _context.Entry(endorsementPendingChange).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public async Task DeleteAsync(EndorsementPendingChange endorsementPendingChange)
        {
            _context.EndorsementPendingChanges.Remove(endorsementPendingChange);
            await _context.SaveChangesAsync();
        }
    }
}
