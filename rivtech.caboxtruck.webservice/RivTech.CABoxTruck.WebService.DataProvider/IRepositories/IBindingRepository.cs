using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IBindingRepository : IRepository<Binding, Guid>
    {
        Task<IEnumerable<Binding>> GetAllIncludeAsync();
        Task<IEnumerable<Binding>> GetAllIncludeAsync(Expression<Func<Binding, bool>> expression);
        Task<Binding> GetByRiskAsync(Guid riskId);
    }

    public interface IBindingRequirementsRepository : IRepository<BindingRequirements, Guid>
    {
        Task<IEnumerable<BindingRequirements>> GetAllIncludeAsync();
        Task<IEnumerable<BindingRequirements>> GetAllIncludeAsync(Expression<Func<BindingRequirements, bool>> expression);
    }

    public interface IQuoteConditionsRepository : IRepository<QuoteConditions, Guid>
    {
        Task<IEnumerable<QuoteConditions>> GetAllIncludeAsync();
        Task<IEnumerable<QuoteConditions>> GetAllIncludeAsync(Expression<Func<QuoteConditions, bool>> expression);
    }
}
