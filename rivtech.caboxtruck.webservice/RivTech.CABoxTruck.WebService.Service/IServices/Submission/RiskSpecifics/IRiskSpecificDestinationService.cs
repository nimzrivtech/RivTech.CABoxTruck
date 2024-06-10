using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificDestinationService
    {
        Task<string> InsertDestinationAsync(RiskSpecificDestinationDto model);
        Task<List<RiskSpecificDestinationDto>> GetAllAsync(Guid riskId);
        Task<RiskSpecificDestinationDto> GetDestinationAsync(Guid id);
        Task<string> UpdateDestinationAsync(RiskSpecificDestinationDto model);
        Task<string> RemoveDestinationAsync(Guid id);
    }
}
