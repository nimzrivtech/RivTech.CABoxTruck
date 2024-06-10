using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IAgencyService
    {
        Task<List<AgencyDTO>> GetAllAsync();

        Task<bool> IsExistAsync(Guid id);
        Task<AgencyDTO> GetByIdAsync(Guid id);
        Task<AgencyDTO> InsertAsync(AgencyDTO model);
        Task<AgencyDTO> UpdateAsync(AgencyDTO model);
        Task<string> RemoveAsync(Guid id);

        Task<List<AgencyDTO>> GetAllIncludeAsync();
        Task<AgencyDTO> GetByIdIncludeAsync(Guid id);

        Task<long> InsertRangeAsync(List<AgencyDTO> model);
    }
}
