using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class AuditLogRepository : BaseRepository<Data.Entity.AuditLog, long>, IAuditLogRepository
    {
        public AuditLogRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
