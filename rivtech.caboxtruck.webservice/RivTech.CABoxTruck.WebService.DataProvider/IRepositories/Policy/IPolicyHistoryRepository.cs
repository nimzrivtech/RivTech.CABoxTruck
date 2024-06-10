using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IPolicyHistoryRepository : IRepository<PolicyHistory, long>
    {
        Task<PolicyHistory> GetFirstIssuance(Guid riskId);
        Task<PolicyHistory> GetLatestByRisk(Guid riskId);
        Task<PolicyHistory> GetPreviousHistory(long id);
        Task<PolicyHistory> GetPreviousActiveHistory(long id);
        Task<PolicyHistory> GetLastActive(Guid riskId);
        Task<PolicyHistory> GetLastCancelled(Guid riskId);
        Task<IEnumerable<PolicyHistory>> GetByRiskDetailAsync(Guid riskDetailId);
        Task<IEnumerable<PolicyHistory>> GetByRiskId(Guid riskId);
    }
}
