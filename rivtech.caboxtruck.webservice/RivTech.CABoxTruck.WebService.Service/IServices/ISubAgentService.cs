using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ISubAgentService
    {
        Task<List<AgentSubAgencyDTO>> GetAllAsync();
        Task<List<AgentSubAgencyDTO>> GetByAgencyIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);
        Task<AgentSubAgencyDTO> GetByIdAsync(Guid id);
        Task<AgentSubAgencyDTO> InsertAsync(AgentSubAgencyDTO model);
        Task<AgentSubAgencyDTO> UpdateAsync(AgentSubAgencyDTO model);
        Task<string> RemoveAsync(Guid id);

        Task<List<AgentSubAgencyDTO>> GetByAgencyIdIncludeAsync(Guid id);
        Task<List<AgentSubAgencyDTO>> GetAllIncludeAsync();
        Task<AgentSubAgencyDTO> GetByIdIncludeAsync(Guid id);
    }
}
