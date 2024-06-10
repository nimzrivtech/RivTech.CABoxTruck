using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission
{
    public class Risk: BaseEntity<Guid>
    {
        public string SubmissionNumber { get; set; }
        public string QuoteNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicySuffix { get; set; }
        public string Status { get; set; }
        public int RaterVersionId { get; set; }
        public bool IsRaterVersionLocked { get; set; }
        public DateTime? PendingCancellationDate { get; set; }
        public Guid? CurrentRiskDetailId { get; set; }
        public RiskDetail CurrentRiskDetail { get; set; }
        /// <summary>
        /// RiskDetail Id of 'For Endorsement'/Canceled.
        /// </summary>
        public Guid? ForEndorsementRiskDetailId { get; set; }
        //public RiskDetail ForEndorsementRiskDetail { get; set; }
        public int CurrentEndorsementNumber { get; set; }
        public List<RiskDetail> RiskDetails { get; set; }

        public BrokerInfo BrokerInfo { get; set; }

        //public List<BindingRequirements> BindingRequirements { get; set; }
        public Binding Binding { get; set; }
        public Guid? LastReinstatedRiskDetailId { get; set; }
        public DateTime? PendingCancellationNoticeDate { get; set; }
        public string CancellationReasonType { get; set; }
        public bool IsPendingCancellation { get; set; }
        public bool IsRenewed { get; set; }

        public int NextEndorsementNumber() => CurrentEndorsementNumber + 1;
        
        public bool UpdateXPRaterVersion { get; set; }
        public bool EndorsementEffectiveDatePrompted { get; set; }
        public bool IsRenewal { get; set; }
        public bool IsFromRenewal { get; set; }
        public bool IsFromSN { get; set; }
        public DateTime? RenewalDate { get; set; }
        public string PreviousPolicyNumber { get; set; }
        public Guid? RenewalFromRiskId { get; set; }
        public bool IsFromReset { get; set; }
        public int? AssistantUnderwriterId { get; set; }
        public int? PsrUserId { get; set; }
        public int? TeamLeadUserId { get; set; }
        public int? InternalUWUserId { get; set; }
        public int? ProductUWUserId { get; set; }

        public bool IsForRenewal(DateTime date)
        {
            var daysBeforeExpiration = (BrokerInfo.ExpirationDate.Value - date).TotalDays;
            bool isExpired = daysBeforeExpiration < 0;
            return !IsRenewed && !isExpired && daysBeforeExpiration <= 90;
        }

        public bool IsForExpired(DateTime date)
        {
            var daysBeforeExpiration = (BrokerInfo.ExpirationDate.Value - date).TotalDays;
            bool isExpired = daysBeforeExpiration < 0;
            return isExpired;
        }

        public void IssuanceUpdate(RiskDetail currentRiskDetail, RiskDetail forEndorsementRiskDetail)
        {
            if (currentRiskDetail.EndorsementNumber != 0)
            {
                CurrentEndorsementNumber++;
            }
            CurrentRiskDetailId = currentRiskDetail.Id;
            ForEndorsementRiskDetailId = forEndorsementRiskDetail.Id;
        }

        public bool IsIssued() => CurrentRiskDetailId != null;

        public void LockRaterVerison()
        {
            if(!IsRaterVersionLocked)
                IsRaterVersionLocked = true;
        }
    }
}
