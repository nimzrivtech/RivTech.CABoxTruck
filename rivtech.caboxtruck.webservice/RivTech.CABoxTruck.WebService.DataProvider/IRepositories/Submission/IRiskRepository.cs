using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission
{
    public interface IRiskRepository : IRepository<Risk, Guid>
    {
        Task<IEnumerable<RiskSummaryDto>> GetFilteredSubmissionSummaryListAsync(SubmissionFilterDto filter);
        Task<Risk> GetRiskByPolicyNumberAsync(string policyNumber);
        Task<List<Guid>> GetAllRiskIdForCancellationAsync(DateTime pendingCancellationDate);
        Task<List<Risk>> GetAllRiskIdForPendingCancellationAsync();
        Task<Risk> GetRiskByIdAsync(Guid id);
        /// <summary>
        /// Returns the Risk by Id without DbContext tracking. 
        /// </summary>
        Task<Risk> GetByIdNoTrackingAsync(Guid id);
        Task<List<Risk>> GetManualNoticeCancellationRisks(DateTime noticeDate);
        Task<Risk> GetBySubmissionAsync(string submission);
        Task<string[]> GetForRenewalsAsync(DateTime date); 
        Task<string[]> GetExpiredPoliciesAsync(DateTime date); 
        Task<List<FieldVersion>> GetFieldVersions(); 
    }
}
