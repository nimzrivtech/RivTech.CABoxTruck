using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskStatusHistoryRepository : BaseRepository<RiskStatusHistory, Guid>, IRiskStatusHistoryRepository
    {
        public RiskStatusHistoryRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


