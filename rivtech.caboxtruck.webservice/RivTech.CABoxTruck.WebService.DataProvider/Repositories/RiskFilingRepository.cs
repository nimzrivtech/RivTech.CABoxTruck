using System;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskFilingRepository : BaseRepository<RiskFiling, Guid>, IRiskFilingRepository
    {
        public RiskFilingRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}