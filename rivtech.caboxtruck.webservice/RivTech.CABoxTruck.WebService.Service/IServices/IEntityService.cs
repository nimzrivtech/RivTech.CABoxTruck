using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEntityService
    {
        Task<string> InsertEntityAsync(EntityDTO model);
        Task<List<EntityDTO>> GetAllAsync();
        Task<EntityDTO> GetEntityAsync(Guid id);
        Task<string> UpdateEntityAsync(EntityDTO model);
        Task<string> RemoveEntityAsync(Guid id);
    }
}
