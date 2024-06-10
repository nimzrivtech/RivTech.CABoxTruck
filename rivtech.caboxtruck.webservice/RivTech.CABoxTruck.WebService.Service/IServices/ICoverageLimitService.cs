using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ICoverageLimitService
    {
        Task<List<RiskCoverageDTO>> GetAllAsync();
        Task<RiskCoverageDTO> GetByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
        Task<SaveRiskCoverageDTO> InsertAsync(SaveRiskCoverageDTO model);
        Task<SaveRiskCoverageDTO> UpdateAsync(SaveRiskCoverageDTO model);
        Task<SaveRiskCoverageDTO> UpdateRatingFactorAsync(SaveRiskCoverageDTO model);
        Task UpdateLimitDataAsync(SaveRiskCoverageDTO model);
    }
}
