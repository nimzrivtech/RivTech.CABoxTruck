using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificRadiusOfOperationService
    {
        Task<string> InsertRadiusOfOperationAsync(RiskSpecificRadiusOfOperationDto model);
        Task<List<RiskSpecificRadiusOfOperationDto>> GetAllAsync();
        Task<RiskSpecificRadiusOfOperationDto> GetRadiusOfOperationAsync(Guid id);
        Task<string> UpdateRadiusOfOperationAsync(Guid riskId, RiskSpecificRadiusOfOperationDto model);
        Task<string> RemoveRadiusOfOperationAsync(Guid id);
    }
}
