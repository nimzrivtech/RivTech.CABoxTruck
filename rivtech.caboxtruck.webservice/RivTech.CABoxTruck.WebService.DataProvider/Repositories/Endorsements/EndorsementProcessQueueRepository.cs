using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Endorsements
{
    public class EndorsementProcessQueueRepository : IEndorsementProcessQueueRepository
    {
        private readonly AppDbContext _dbContext;

        public EndorsementProcessQueueRepository(
            AppDbContext dbContext
        ) {
            _dbContext = dbContext;
        }

        public async Task<EndorsementProcessQueue> CreateAsync(EndorsementProcessQueue queue)
        {
            var entity = _dbContext.EndorsementProcessQueue.Add(queue);
            await _dbContext.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task DeleteAsync(EndorsementProcessQueue queue)
        {
            _dbContext.EndorsementProcessQueue.Remove(queue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<EndorsementProcessQueue> GetActiveByRiskIdAsync(Guid riskId)
        {
            var queue = await _dbContext.EndorsementProcessQueue.AsNoTracking()
                        .FirstOrDefaultAsync(q => q.RiskId == riskId && q.IsArchived == false);
            return queue;
        }

        public async Task<IEnumerable<EndorsementProcessQueue>> GetUnstartedQueuesAsync()
        {
            var queues = await _dbContext.EndorsementProcessQueue.AsNoTracking()
                            .Where(q => q.StartTime == null)
                            .ToListAsync();
            return queues;
        }

        public async Task UpdateAsync(EndorsementProcessQueue queue)
        {
            _dbContext.EndorsementProcessQueue.Update(queue);
            await _dbContext.SaveChangesAsync();
        }
    }
}
