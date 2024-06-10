using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ILog4NetService
    {
        public Task InfoAsync(AuditLogDTO auditLog);
        public Task ErrorAsync(ErrorLogDTO errorLog);
        public void Error(string errorMessage, string jsonMessage, [CallerMemberName] string callerMemberName = "");
        public void Error(Exception exception, [CallerMemberName] string callerMemberName = "");
    }
}
