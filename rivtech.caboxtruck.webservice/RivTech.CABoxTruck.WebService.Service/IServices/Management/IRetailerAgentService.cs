using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRetailerAgentService
    {
        Task<List<RetailerAgentDTO>> GetAll();
        Task<List<RetailerAgentDTO>> GetByRetailerId(Guid id);

        Task<RetailerAgentDTO> GetById(Guid agentId);
        Task<RetailerAgentDTO> UpdateAsync(RetailerAgentDTO resource);
        Task<RetailerAgentDTO> InsertAsync(RetailerAgentDTO resource);
        Task<bool> RemoveAsync(Guid id);
    }
}