using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class BindingRepository : BaseRepository<Binding, Guid>, IBindingRepository
    {
        public BindingRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<Binding>> GetAllIncludeAsync()
        {
            return await _context.Set<Binding>()
                                .Include(config => config.BindOption)
                                .Include(config => config.PaymentTypes)
                                .Include(config => config.BindOption)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Binding>> GetAllIncludeAsync(Expression<Func<Binding, bool>> expression)
        {
            return await _context.Set<Binding>()
                                .Where(expression)
                                .Include(config => config.BindOption)
                                .Include(config => config.PaymentTypes)
                                 .Include(config => config.BindOption)
                                .ToListAsync();
        }

        public Task<Binding> GetByRiskAsync(Guid riskId)
        {
            return FindAsync(x => x.RiskId == riskId);
        }
    }

    public class BindingRequirementsRepository : BaseRepository<BindingRequirements, Guid>, IBindingRequirementsRepository
    {
        public BindingRequirementsRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<BindingRequirements>> GetAllIncludeAsync()
        {
            return await _context.Set<BindingRequirements>()
                                .Include(config => config.BindRequirementsOption)
                                .Include(config => config.BindStatus)
                                //.Include(config => config.RiskDetail)
                                .ToListAsync();
        }

        public async Task<IEnumerable<BindingRequirements>> GetAllIncludeAsync(Expression<Func<BindingRequirements, bool>> expression)
        {
            return await _context.Set<BindingRequirements>()
                                .Where(expression)
                                  .Include(config => config.BindRequirementsOption)
                                .Include(config => config.BindStatus)
                                //.Include(config => config.RiskDetail)
                                .ToListAsync();
        }
    }

    public class QuoteConditionsRepository : BaseRepository<QuoteConditions, Guid>, IQuoteConditionsRepository
    {
        public QuoteConditionsRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<QuoteConditions>> GetAllIncludeAsync()
        {
            return await _context.Set<QuoteConditions>()
                                .Include(config => config.QuoteConditionsOption)
                                //.Include(config => config.RiskDetail)
                                .ToListAsync();
        }

        public async Task<IEnumerable<QuoteConditions>> GetAllIncludeAsync(Expression<Func<QuoteConditions, bool>> expression)
        {
            return await _context.Set<QuoteConditions>()
                                .Where(expression)
                                  .Include(config => config.QuoteConditionsOption)
                                //.Include(config => config.RiskDetail)
                                .ToListAsync();
        }
    }
}


