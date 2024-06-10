using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskResponseRepository : BaseRepository<RiskResponse, Guid>, IRiskResponseRepository
    {
        public RiskResponseRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


