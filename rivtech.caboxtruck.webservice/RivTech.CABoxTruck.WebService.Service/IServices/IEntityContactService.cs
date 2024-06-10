using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEntityContactService
    {
        Task<string> InsertEntityAsync(EntityContactDTO model);
        Task<bool> InsertEntityBulkAsync(List<EntityContactDTO> contacts);
        Task<List<EntityContactDTO>> GetAllAsync();
        Task<List<EntityContactDTO>> GetAllAsync(Guid id);
        Task<EntityContactDTO> GetEntityAsync(Guid id);
        Task<string> UpdateEntityAsync(EntityContactDTO model);
        Task<string> RemoveEntityAsync(Guid id);
    }
}
