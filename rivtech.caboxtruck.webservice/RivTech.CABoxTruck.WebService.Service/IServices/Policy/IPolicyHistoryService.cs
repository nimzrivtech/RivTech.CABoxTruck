using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IPolicyHistoryService
    {
        Task<PolicyHistoryDTO> GetFirstIssuance(Guid riskId);
        Task<PolicyHistoryDTO> GetLastActiveByRiskId(Guid riskId);
        Task<PolicyHistoryDTO> GetLastCancelledByRiskId(Guid riskId);
        Task<List<PolicyHistoryDTO>> GetEndorsementHistoryByRiskId(Guid riskId);
        Task<int> CreatePolicyHistory(Guid riskId, RiskDetail forEndorsementRiskDetail, Guid previousRiskDetailId, RiskCoveragePremium riskCovPrem, DateTime effectiveDate, string policyStatus, string details = null);
    }
}
