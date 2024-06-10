using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IAgentService
    {
        Task<List<AgentDTO>> GetAllAsync();
        Task<List<AgentDTO>> GetByAgencyIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);
        Task<AgentDTO> GetByIdAsync(Guid id);
        Task<AgentDTO> InsertAsync(AgentDTO model);
        Task<AgentDTO> UpdateAsync(AgentDTO model);
        Task<string> RemoveAsync(Guid id);

        Task<List<AgentDTO>> GetByAgencyIdIncludeAsync(Guid id);
        Task<List<AgentDTO>> GetAllIncludeAsync();
        Task<AgentDTO> GetByIdIncludeAsync(Guid id);
    }
}
