using System;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission
{
    public interface IRiskFactory
    {
        public Task<RiskDetail> GeneratePolicy(Guid riskDetailId, string policyNumber);
        public Task<List<RiskFormDTO>> GetRiskForms(Guid riskDetailId);
        public Task<List<RiskFormDTO>> GetRiskFormsWithManuscripts(Guid riskDetailId);
        public Task<List<RiskFormDTO>> GetRiskFormsWithConditionals(RiskDTO riskDetail, bool isEndorsement = false, List<RiskFormDTO> riskForms = null);
        Task<RiskDetail> GetPreviousPolicyDetail(Guid currentRiskDetailId, Guid riskId);
        Task<List<string>> GetPolicyChanges(Guid currentRiskDetailId, Guid riskId, bool includeAll = false);
        Task<List<AdditionalInterestChange>> GetAdditionalInterestChanges(Guid currentRiskDetailId, Guid riskId);
        Task<RiskDetail> GeneratePolicyDetailForEndorsement(Guid? riskDetailId, DateTime effectiveDate, RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk risk, Guid boundRiskDetailId);
        Task<RiskDetail> GeneratePolicyDetailForCancellation(Guid currentRiskDetailId, Guid riskId, DateTime effectiveDate);
        Task<RiskDetail> GeneratePolicyDetailForReinstatement(Guid currentRiskDetailId, Guid riskId);
        bool IsDriverChanged(Driver oldDriver, Driver newDriver);
        Task<RiskDetail> GenerateSubmissionDetailFromPolicy(Guid currentRiskDetailId, Guid riskId, string newSubmissionNumber, string newQuoteNumber);
        Task<RiskDetail> GenerateRenewalDetailFromPolicy(Guid currentRiskDetailId, Guid riskId, string newSubmissionNumber, string newQuoteNumber);
        Task<RiskDetail> GenerateSubmissionDetail(Guid currentRiskDetailId, Guid riskId, string newSubmissionNumber, string newQuoteNumber);
    }
}
