using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Endorsements;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Endorsements
{
    public class EndorsementProcessStatusLogRepository : IEndorsementProcessStatusLogRepository
    {
        private readonly AppDbContext _dbContext;

        public EndorsementProcessStatusLogRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<EndorsementProcessStatusLog>> GetAsync(Guid riskId)
        {
            return await _dbContext.EndorsementProcessStatusLog.AsNoTracking().Where("riskId = @0", riskId).ToListAsync();
        }

        public async Task<EndorsementProcessStatusLog> CreateAsync(EndorsementProcessStatusLog statusLog)
        {
            var entity = _dbContext.EndorsementProcessStatusLog.Add(statusLog);
            await _dbContext.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task DeleteAsync(IEnumerable<EndorsementProcessStatusLog> logs)
        {
            foreach(var log in logs)
            {
                _dbContext.EndorsementProcessStatusLog.Remove(log);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid riskId)
        {
            return await _dbContext.EndorsementProcessStatusLog
                            .AsNoTracking()
                            .AnyAsync(x => x.RiskId == riskId);
        }
    }
}
