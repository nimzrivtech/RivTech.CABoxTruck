using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IAgencyRepository : IRepository<Agency, Guid>
    {
        Task<IEnumerable<Agency>> GetAllIncludeAsync();
        Task<IEnumerable<Agency>> GetAllIncludeAsync(Expression<Func<Agency, bool>> expression);
        Task<Agency> GetAgency(Guid agencyId);
        Task<List<Agency>> GetAllAgenciesByIds(List<Guid> ids);
    }

    public interface ISubAgencyRepository : IRepository<SubAgency, Guid>
    {
        Task<IEnumerable<SubAgency>> GetAllIncludeAsync();
        Task<IEnumerable<SubAgency>> GetAllIncludeAsync(Expression<Func<SubAgency, bool>> expression);
    }
}
