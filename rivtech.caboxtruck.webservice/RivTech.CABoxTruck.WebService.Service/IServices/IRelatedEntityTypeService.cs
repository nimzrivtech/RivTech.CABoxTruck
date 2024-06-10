using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRelatedEntityTypeService
    {
        Task<string> InsertRelatedEntityTypeAsync(RelatedEntityTypeDTO model);
        Task<List<RelatedEntityTypeDTO>> GetAllAsync();
        Task<RelatedEntityTypeDTO> GetRelatedEntityTypeAsync(byte id);
        Task<string> UpdateRelatedEntityTypeAsync(RelatedEntityTypeDTO model);
        Task<string> RemoveRelatedEntityTypeAsync(byte id);
    }
}
