using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskPolicyContactRepository : BaseRepository<RiskPolicyContact, Guid>, IRiskPolicyContactRepository
    {
        public RiskPolicyContactRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<RiskPolicyContact>> GetAllIncludeAsync(Guid riskDetailId)
        {
            return await _context.RiskPolicyContact
                                .Include(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses)
                                        .ThenInclude(config => config.Address)
                                .Where(x => x.RiskDetailId == riskDetailId && x.DeletedDate == null)
                                .ToListAsync();
        }

        public async Task<RiskPolicyContact> GetIncludeAsync(Guid id)
        {
            return await _context.RiskPolicyContact
                                .Include(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses)
                                        .ThenInclude(config => config.Address)
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<RiskPolicyContact> GetByRiskDetailIdIncludeAsync(Guid id, short typeId)
        {
            return await _context.RiskPolicyContact
                                .Include(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses)
                                        .ThenInclude(config => config.Address)
                                .Where(x => x.RiskDetailId == id && x.TypeId == typeId)
                                .FirstOrDefaultAsync();
        }
    }
}
