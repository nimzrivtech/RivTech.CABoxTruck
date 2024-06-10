using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEntitySubsidiaryService
    {
        Task<EntitySubsidiaryDTO> InsertorUpdateSubsidiaryAsync(SaveEntitySubsidiaryDTO model);
        Task<EntitySubsidiaryDTO> InsertorUpdateCompanyAsync(SaveEntitySubsidiaryDTO model);
        Task<EntitySubsidiaryDTO> GetByEntityIdAsync(Guid id);
        Task<bool> RemoveSubsidiaryAllAsync(Guid id);
        Task<bool> RemoveCompanyAllAsync(Guid id);
        Task<bool> RemoveAsync(Guid id);
    }
}
