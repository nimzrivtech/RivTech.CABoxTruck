using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;


namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IQuoteOptionsService
    {
        Task<List<RiskCoverageDTO>> GetByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
        Task<RiskCoverageListDTO> InsertAsync(RiskCoverageListDTO model);
        Task<RiskCoverageListDTO> UpdateAsync(RiskCoverageListDTO model);
        Task<List<RiskCoverageDTO>> GetRiskCoverageAndPremiumByIdAsync(Guid id);
        Task<RiskCoverageDTO> CopyOptionAsync (Guid riskDetailId, int optionToCopy);
        Task DeleteOptionAsync(Guid riskDetailId, Guid optionId);
        Task UpdateHiredPhysicalDamage(RiskFormOtherDTO riskFormOther);
        Task UpdateTrailerInterchange(RiskFormOtherDTO riskFormOther);
        Task RemoveHiredPhysicalDamage(Guid riskDetailId);
        Task RemoveTrailerInterchange(Guid riskDetailId);
        Task UpdateManuscript(RiskManuscriptsDTO resource);
        Task CalculateAdditionalInterests(Guid riskDetailId);
        Task<List<RiskCoverageDTO>> GetRiskCoverageAndPremiumMetricsByRiskIdAsync(Guid riskId);

    }
}
