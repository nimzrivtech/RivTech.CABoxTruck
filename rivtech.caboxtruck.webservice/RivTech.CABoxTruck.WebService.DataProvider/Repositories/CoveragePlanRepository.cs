using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class CoveragePlanRepository : BaseRepository<CoveragePlan, Int16>, ICoveragePlanRepository
    {
        public CoveragePlanRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


