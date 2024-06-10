using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RiskData = RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskService
    {
        Task<bool> IsExistAsync(Guid id);
        Task<RiskDTO> InsertRiskAsync(RiskDTO model);
        Task<List<RiskDTO>> GetAllAsync();
        Task<RiskDTO> GetRiskAsync(Guid id);
        Task<string> UpdateRiskAsync(RiskDTO model);
        Task<bool> UpdateRiskStatusAsync(RiskDTO model);
        Task<bool> UpdateRiskStatusOnlyAsync(Guid riskDetailId, string status);
        Task<string> UpdatePendingEndorsementAsync(Guid riskDetailId);
        Task<string> RemoveRiskAsync(Guid id);
        Task<List<RiskDTO>> GetAllIncludeAsync();
        Task<RiskDTO> GetByIdIncludeAsync(Guid riskDetailId, bool withAgency = true);
        Task<RiskDetail> GetSubmissionRiskDetailAsync(Guid riskId);

        Task<Guid> GetRiskDetailIdByStatusAsync(Guid riskId, string status);
        Task<PaymentPortalSearchResponseDTO> SearchRiskByPolicyAndZipCodeAsync(string policyNumber, string zipCode);
        Task ProcessRiskDetailsForIssuance(Guid riskId, RiskDetail riskDetail, BrokerInfo brokerInfo);
        Task UpdateForEndorsementRiskDetailId(RiskData risk);
        Task ProcessRiskDetailsForReinstatement(Guid riskId, RiskDetail riskDetailToIssue, BrokerInfo brokerInfo);
        Task UpdatePolicyNumberAsync(Guid riskId, string policyNumber);
        Task<Guid> GetLatestRiskDetailVersionAsync(Guid riskId);
        Task AddRiskStatusHistoryIsPendingCancellationAsync(Guid riskDetailId, bool oldStatus, bool newStatus);
        Task AddRiskStatusHistoryPNOCNoticeDateAsync(Guid riskDetailId, DateTime? oldStatus, DateTime newStatus);
        Task<string[]> GetPoliciesForRenewalAsync(DateTime date); 
        Task<string[]> GetExpiredPoliciesAsync(DateTime date); 
    }
}
