using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskTypeService
    {
        Task<string> InsertRiskTypeAsync(RiskTypeDTO model);
        Task<List<RiskTypeDTO>> GetAllAsync();
        Task<RiskTypeDTO> GetRiskTypeAsync(byte id);
        Task<string> UpdateRiskTypeAsync(RiskTypeDTO model);
        Task<string> RemoveRiskTypeAsync(byte id);
    }
}
