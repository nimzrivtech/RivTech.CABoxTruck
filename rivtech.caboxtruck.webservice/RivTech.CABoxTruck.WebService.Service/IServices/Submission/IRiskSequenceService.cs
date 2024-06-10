using System;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission
{
    public interface IRiskSequenceService
    {
        string GenerateSubmissionNumber();
        string GenerateQuoteNumber();
        string GeneratePolicyNumber(string state, DateTime expirationDate, DateTime inceptionDate);
        string UpdatePolicyNumber(string policyNumber, DateTime expirationDate, DateTime inceptionDate, string state);
        string GenerateRenewalPolicyNumber(string policyNumber);
        string GenerateRenewalPolicyNumber(string policyNumber, string state, DateTime expirationDate, DateTime inceptionDate);
        bool validatePolicyNumber(string policyNumber);
    }
}
