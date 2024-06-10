using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class AppLogRepository : BaseRepository<Data.Entity.AppLog, long>, IAppLogRepository
    {
        public AppLogRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
