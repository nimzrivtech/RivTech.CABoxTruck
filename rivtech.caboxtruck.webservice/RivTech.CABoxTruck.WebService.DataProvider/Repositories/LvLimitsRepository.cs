using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class LvLimitsRepository : BaseRepository<LvLimits, int>, ILvLimitsRepository
    {
        public LvLimitsRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


