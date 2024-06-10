using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Notes;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskManuscriptsService
    {
        Task<List<RiskManuscriptsDTO>> GetByRiskIdAsync(Guid riskId);
        Task<List<RiskManuscriptsDTO>> GetByRiskIdAllAsync(Guid riskId);

        Task<RiskManuscriptsDTO> InsertAsync(RiskManuscriptsDTO model);

        Task<RiskManuscriptsDTO> UpdateAsync(RiskManuscriptsDTO model);

        Task<bool> UpdateDatesAsync(RiskManuscriptsDTO model);

        Task<bool> RemoveAsync(Guid id, bool fromEndorsement, DateTime expirationDate);
        Task<RiskManuscriptsDTO> InsertManuscriptAsync(RiskManuscriptsDTO resource);
        Task<ManuscriptPagingDTO> GetByRiskIdAllAsync(ManuscriptFilterDTO manuscriptFilter);
    }
}