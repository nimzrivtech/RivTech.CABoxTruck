using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Notes;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskNotesService
    {
        Task<List<RiskNotesDTO>> GetByRiskIdAsync(Guid riskId);

        Task<RiskNotesDTO> InsertorUpdateNotesAsync(RiskNotesDTO model);

        Task<bool> RemoveAsync(Guid id);
    }
}