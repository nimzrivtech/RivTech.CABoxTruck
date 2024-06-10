using System;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notification;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission
{
    public interface ITransactionService
    {
        Task<RiskDTO> IssueSubmission(Guid riskDetailId);
        Task<RiskDTO> ResetPolicyChanges(Guid riskDetailId);
        Task<RiskDTO> ResetPolicyChanges(Guid riskDetailId, NotificationFilterDTO notificationFilterDTO);
        Task<System.Collections.Generic.List<string>> GetPolicyChanges(Guid riskDetailId);
        Task<RiskDTO> IssueEndorsement(Guid riskDetailId, DateTime effectiveDate, string endorsementText, string policyChanges); 
        Task<RiskDTO> IssueEndorsement(Guid riskDetailId, DateTime effectiveDate, string endorsementText, string policyChanges, NotificationFilterDTO notificationFilterDTO);
        Task<RiskDTO> CancelPolicy(Guid riskDetailId, DateTime effectiveDate, string cancellationReasonType, bool isShortRatePremium, bool? isFlatCancel = null, bool isAutoCancel = false);
        Task<RiskDTO> ReinstatePolicy(Guid riskDetailId);
        Task<RiskDTO> RewritePolicyToSubmission(Guid riskDetailId, DateTime effectiveDate, bool? isFlatCancel = true);
        Task CancelPolicyForRewrite(Guid riskDetailId, DateTime effectiveDate, bool? isFlatCancel = true);
        Task AutoPolicyCancellation(DateTime effectiveDate);
        Task AutoNoticeOfCancellation(DateTime effectiveDate);
        Task AutoRescindNoticeOfCancellation(DateTime effectiveDate);
        Task AutoReinstateCancelledPolicy(DateTime effectiveDate);
        Task ManualCancellationNotice(Guid riskId, Guid riskDetailId, DateTime noticeDate, DateTime effectiveDate, string cancellationReasonType);
        Task<RiskDTO> ManualRescindPolicy(Guid riskDetailId);
        Task ManualRescindPNOCByRiskIdDataFix(Guid riskId);
        Task<RiskDTO> RenewPolicy(string policyNumber);
        Task<bool> ExpirePolicy(string policyNumber);
        Task<RiskDTO> CopySubmission(Guid riskDetailId, DateTime effectiveDate, bool? isFlatCancel = true);
    }
}
