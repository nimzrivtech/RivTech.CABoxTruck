using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificCommoditiesHauledService
    {
        Task<RiskSpecificCommoditiesHauledDto> InsertAsync(RiskSpecificCommoditiesHauledDto model);
        Task<bool> UpdateAsync(RiskSpecificCommoditiesHauledDto model);
        Task<bool> RemoveAsync(Guid id);
        Task<List<RiskSpecificCommoditiesHauledDto>> GetByRiskDetailIdAsync(Guid riskDetailId);
    }
}