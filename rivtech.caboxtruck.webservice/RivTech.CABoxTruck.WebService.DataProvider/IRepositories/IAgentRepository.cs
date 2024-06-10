using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IAgentRepository : IRepository<Agent, Guid>
    {
        Task<IEnumerable<Agent>> GetAllIncludeAsync();
        Task<IEnumerable<Agent>> GetAllIncludeAsync(Expression<Func<Agent, bool>> expression);
        Task<Agent> GetAgent(Guid agentId);
        Task<List<Agent>> GetAllAgentsByIds(List<Guid> ids);
    }

    public interface ISubAgentRepository : IRepository<SubAgent, Guid>
    {
        Task<IEnumerable<SubAgent>> GetAllIncludeAsync();
        Task<IEnumerable<SubAgent>> GetAllIncludeAsync(Expression<Func<SubAgent, bool>> expression);
    }
}
