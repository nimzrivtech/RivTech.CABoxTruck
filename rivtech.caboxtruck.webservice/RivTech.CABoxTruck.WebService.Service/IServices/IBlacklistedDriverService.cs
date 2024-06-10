using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.DTO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBlacklistedDriverService
    {
        Task<BlacklistedDriverDTO> AddOrUpdateAsync(BlacklistedDriverDTO model);
        Task<IEnumerable<BlacklistedDriverDTO>> SearchDriver(string driverName, string driverLicenseNumber);
        Task<bool> ValidateDriver(string driverName, string driverLicenseNumber);
    }
}
