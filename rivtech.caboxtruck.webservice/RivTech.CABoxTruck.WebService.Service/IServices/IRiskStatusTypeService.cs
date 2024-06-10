using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskStatusTypeService
    {
        Task<string> InsertRiskStatusTypeAsync(RiskStatusTypeDTO model);
        Task<List<RiskStatusTypeDTO>> GetAllAsync();
        Task<RiskStatusTypeDTO> GetRiskStatusTypeAsync(byte id);
        Task<string> UpdateRiskStatusTypeAsync(RiskStatusTypeDTO model);
        Task<string> RemoveRiskStatusTypeAsync(byte id);
    }
}
