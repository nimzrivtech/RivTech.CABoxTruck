using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskHistoryRepository : BaseRepository<RiskHistory, Guid>, IRiskHistoryRepository
    {
        public RiskHistoryRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


