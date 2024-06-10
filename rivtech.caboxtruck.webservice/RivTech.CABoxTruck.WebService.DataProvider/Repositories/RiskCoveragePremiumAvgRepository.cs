using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskCoveragePremiumAvgRepository : BaseRepository<RiskCoveragePremiumAvg, Guid>, IRiskCoveragePremiumAvgRepository
    {
        public RiskCoveragePremiumAvgRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


