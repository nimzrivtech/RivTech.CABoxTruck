using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskFilingService
    {
        Task<IEnumerable<RiskFilingDTO>> GetByRiskDetailIdAsync(Guid riskId);

        Task<int> InsertRangeAsync(IEnumerable<RiskFilingDTO> model);

        Task<int> RemoveByRiskDetailIdAsync(Guid riskId);
    }
}