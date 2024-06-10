using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IApplicantAdditionalInterestService
    {
        Task<RiskAdditionalInterestDTO> GetAsync(Guid id);
        Task<IEnumerable<RiskAdditionalInterestDTO>> GetByRiskDetailIdAsync(Guid riskDetailId, bool fromEndorsement = false);
        Task<IEnumerable<RiskAdditionalInterestDTO>> GetByRiskDetailIdActiveAsync(Guid riskId);
        Task<Guid> InsertAsync(RiskAdditionalInterestDTO model);
        Task<Guid> UpdateAsync(RiskAdditionalInterestDTO model);
        Task<bool> RemoveAsync(Guid id, bool fromEndorsement, DateTime expirationDate);
        Task<bool> UpdateBlanketCoverage(BlanketCoverageDTO model);
        Task<AdditionalInterestPagingDTO> GetByRiskDetailIdAsync(AdditionalInterestFilterDTO AdditionalInterestFilter);
    }
}