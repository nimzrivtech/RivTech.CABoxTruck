using System;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RetailerAgentRepository : BaseRepository<RetailerAgent, Guid>, IRetailerAgentRepository
    {
        public RetailerAgentRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


