using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IDriverService
    {
        Task<List<DriverDTO>> GetAllAsync();
        Task<List<DriverDTO>> GetByRiskDetailIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);
        Task<DriverDTO> GetByIdAsync(Guid id);
        Task<DriverDTO> InsertAsync(DriverDTO model);
        Task<DriverDTO> UpdateAsync(DriverDTO model, bool? fromEndorsement = false);

        Task<List<DriverDTO>> UpdateListOptionAsync(List<DriverDTO> model);

        Task<string> RemoveAsync(Guid id, bool fromOOS = false);
        Task<List<DriverDTO>> GetByRiskDetailIdIncludeAllAsync(Guid id);
        Task<List<DriverDTO>> GetByRiskDetailIdIncludeAsync(Guid id);
        Task<List<DriverDTO>> GetAllIncludeAsync();
        Task<List<DriverDTO>> GetByIdIncludeAsync(Guid id);

        Task<DriverHeaderDTO> AddUpdateAsync(DriverHeaderDTO model);

        Task<List<DriverDTO>> AddFromExcelFileAsync(Stream fileStream);

        Task<List<DriverDTO>> InsertRangeAsync(List<DriverDTO> model);
        Task<List<DriverDTO>> GetPreviousDrivers(Guid id);
        Task<DriverDTO> ReinstateDriverAsync(Guid driverId);
        Task<List<DriverDTO>> GetDriversByLastNameAndLicenseNumberAsync(string lastName, string licenseNumber);
        Task UpdateDriverEffectiveDateExpirationDate(Guid riskDetailId, DateTime effectiveDate, DateTime expirationDate);
        Task<DateTime?> GetEffectiveDateFromPreviousRiskDetail(DriverDTO driver, Guid riskId);
        Task UpdateDriverExclusionEndorsementFormAsync(Guid riskDetailId);
        Task<DriverHeaderDTO> GetHeaderAsync(Guid riskDetailId);
        Task ClearFactors(Guid id);
        Task<string> RemoveAllDriverAsync(Guid id, bool fromOOS = false);
        Task<List<DriverDTO>> GetByRiskDetailIdIncludeAllExportAsync(Guid id);
        Task<DateTime?> GetDriverEffectiveDate(DriverDTO driver, Guid currentRiskDetailId);
        Task UpdateDriverEffectiveDateAsync(DriverDTO model);
    }
}
