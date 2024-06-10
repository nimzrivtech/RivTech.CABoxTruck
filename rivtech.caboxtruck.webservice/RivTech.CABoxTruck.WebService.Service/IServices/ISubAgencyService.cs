using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ISubAgencyService
    {
        Task<List<SubAgencyDTO>> GetAllAsync();
        Task<List<SubAgencyDTO>> GetByAgencyIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);
        Task<SubAgencyDTO> GetByIdAsync(Guid id);
        Task<SubAgencyDTO> InsertAsync(SubAgencyDTO model);
        Task<SubAgencyDTO> UpdateAsync(SubAgencyDTO model);
        Task<string> RemoveAsync(Guid id);

        Task<List<SubAgencyDTO>> GetByAgencyIdIncludeAsync(Guid id);
        Task<List<SubAgencyDTO>> GetAllIncludeAsync();
        Task<SubAgencyDTO> GetByIdIncludeAsync(Guid id);
    }
}
