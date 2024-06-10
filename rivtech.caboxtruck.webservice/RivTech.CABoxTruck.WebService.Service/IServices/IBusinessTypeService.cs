using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBusinessTypeService
    {
        Task<string> InsertBusinessTypeAsync(BusinessTypeDTO model);
        Task<List<BusinessTypeDTO>> GetAllAsync();
        Task<BusinessTypeDTO> GetBusinessTypeAsync(short id);
        Task<string> UpdateBusinessTypeAsync(BusinessTypeDTO model);
        Task<string> RemoveBusinessTypeAsync(short id);
    }
}
