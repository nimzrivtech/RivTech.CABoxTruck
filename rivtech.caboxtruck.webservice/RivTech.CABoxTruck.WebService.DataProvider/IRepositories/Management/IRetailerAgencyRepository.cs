using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRetailerAgencyRepository : IRepository<RetailerAgency, Guid>
    {
        Task<List<RetailerAgency>> GetRetailerAsync(Guid agencyId);
    }
}