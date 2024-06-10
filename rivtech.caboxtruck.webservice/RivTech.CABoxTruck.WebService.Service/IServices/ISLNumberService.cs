using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ISLNumberService
    {
        Task<List<SLNumberDTO>> GetAllAsync();
        Task<SLNumberDTO> GetByIdAsync(Int16 id);
        Task<List<SLNumberDTO>> GetByCreditOfficeIdAsync(Int16? creditId);
        Task<SLNumberDTO> GetByCreditOfficeIdStateAsync(Int16? creditId, string state);

        Task<bool> IsExistAsync(Int16 id);
    }
}
