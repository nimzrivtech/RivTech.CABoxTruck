using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificUnderwritingQuestionService
    {
        Task<string> InsertUnderwritingQuestionAsync(RiskSpecificUnderwritingQuestionDto model);
        Task<List<RiskSpecificUnderwritingQuestionDto>> GetAllAsync();
        Task<RiskSpecificUnderwritingQuestionDto> GetUnderwritingQuestionAsync(Guid id);
        Task<string> UpdateUnderwritingQuestionAsync(Guid riskId, RiskSpecificUnderwritingQuestionDto model);
        Task<string> RemoveUnderwritingQuestionAsync(Guid id);
    }
}
