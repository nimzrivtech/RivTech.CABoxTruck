using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskResponseService
    {
        Task<List<RiskResponseDTO>> GetRiskResponse(Guid riskId);
        Task<List<RiskResponseDTO>> GetRiskResponseAllInclude(Guid riskId, Int16 sectionId);
        Task<RiskResponseDTO> InsertRiskResponse(RiskResponseDTO safetyDevice);
        Task<RiskResponseDTO> UpdateSpecificSafetyDevice(RiskResponseDTO safetyDevice, Guid riskId);
        Task<List<RiskResponseDTO>> SaveRiskResponse(List<RiskResponseDTO> riskResponses, Int16 sectionId, Guid riskId);
        Task<List<RiskResponseDTO>> MapToRiskResponse<TEntity>(TEntity entity, Guid riskId);
        TEntity MapToEntityType<TEntity>(TEntity entity, List<RiskResponseDTO> riskResponses);
        Task<bool> Delete(Guid riskId, List<int> sectionIds);
    }
}