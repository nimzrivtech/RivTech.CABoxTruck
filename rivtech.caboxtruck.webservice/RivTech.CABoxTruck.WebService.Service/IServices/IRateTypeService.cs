using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRateTypeService
    {
        Task<string> InsertRateTypeAsync(RateTypeDTO model);
        Task<List<RateTypeDTO>> GetAllAsync();
        Task<RateTypeDTO> GetRateTypeAsync(byte id);
        Task<string> UpdateRateTypeAsync(RateTypeDTO model);
        Task<string> RemoveRateTypeAsync(byte id);
    }
}
