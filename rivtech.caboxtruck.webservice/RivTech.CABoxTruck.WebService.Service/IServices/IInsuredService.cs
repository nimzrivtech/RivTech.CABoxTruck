using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IInsuredService
    {
        Task<string> InsertInsuredAsync(InsuredDTO model);
        Task<List<InsuredDTO>> GetAllAsync();
        Task<InsuredDTO> GetInsuredAsync(Guid id);
        Task<string> UpdateInsuredAsync(InsuredDTO model);
        Task<string> RemoveInsuredAsync(Guid id);
    }
}
