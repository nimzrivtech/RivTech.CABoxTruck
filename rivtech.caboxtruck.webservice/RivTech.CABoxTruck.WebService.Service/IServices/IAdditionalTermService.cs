using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IAdditionalTermService
    {
        Task<string> InsertAdditionalTermAsync(AdditionalTermDTO model);
        Task<List<AdditionalTermDTO>> GetAllAsync();
        Task<AdditionalTermDTO> GetAdditionalTermAsync(byte id);
        Task<string> UpdateAdditionalTermAsync(AdditionalTermDTO model);
        Task<string> RemoveAdditionalTermAsync(byte id);
    }
}
