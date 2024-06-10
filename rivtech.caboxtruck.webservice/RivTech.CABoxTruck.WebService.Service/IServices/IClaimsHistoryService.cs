using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IClaimsHistoryService
    {
        Task<List<ClaimsHistoryDTO>> GetByRiskDetailIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<List<ClaimsHistoryDTO>> InsertAsync(List<ClaimsHistoryDTO> riskHistory);
        Task<List<ClaimsHistoryDTO>> UpdateAsync(List<ClaimsHistoryDTO> riskHistory);
    }
}