using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IAuditLogRepository : IRepository<Data.Entity.AuditLog, long>
    {
    }
}
