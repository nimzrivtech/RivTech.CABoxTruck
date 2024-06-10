using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBrokerInfoService
    {
        Task<List<BrokerInfoDTO>> GetAllAsync();
        Task<BrokerInfoDTO> GetByRiskDetailIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);
        Task<BrokerInfoDTO> GetByIdAsync(Guid id);
        Task<BrokerInfoDTO> GetByRiskDetailIdAllIncludeAsync(Guid id);
        Task<List<EntityAddressDTO>> GetAddressesAsync(Guid entityId);
        Task<BrokerInfoDTO> InsertAsync(BrokerInfoDTO model);
        Task<BrokerInfoDTO> UpdateAsync(BrokerInfoDTO model);
        Task<string> RemoveAsync(Guid id);
    }
}
