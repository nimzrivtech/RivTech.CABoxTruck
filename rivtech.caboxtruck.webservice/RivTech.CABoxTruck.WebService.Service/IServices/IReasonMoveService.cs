using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IReasonMoveService
    {
        Task<List<ReasonMoveDTO>> GetAllAsync();

        Task<bool> IsExistAsync(Int16 id);
        Task<ReasonMoveDTO> GetByIdAsync(Int16 id);
        Task<ReasonMoveDTO> InsertAsync(ReasonMoveDTO model);
        Task<ReasonMoveDTO> UpdateAsync(ReasonMoveDTO model);
        Task<string> RemoveAsync(Int16 id);
    }
}
