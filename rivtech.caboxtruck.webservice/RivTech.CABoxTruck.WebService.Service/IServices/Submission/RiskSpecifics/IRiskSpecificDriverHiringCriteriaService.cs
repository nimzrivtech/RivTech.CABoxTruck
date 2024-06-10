using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificDriverHiringCriteriaService
    {
        Task<string> InsertDriverHiringCriteriaAsync(RiskSpecificDriverHiringCriteriaDto model);
        Task<List<RiskSpecificDriverHiringCriteriaDto>> GetAllAsync();
        Task<RiskSpecificDriverHiringCriteriaDto> GetDriverHiringCriteriaAsync(Guid id);
        Task<string> UpdateDriverHiringCriteriaAsync(RiskSpecificDriverHiringCriteriaDto model);
        Task<string> RemoveDriverHiringCriteriaAsync(Guid id);
    }
}
