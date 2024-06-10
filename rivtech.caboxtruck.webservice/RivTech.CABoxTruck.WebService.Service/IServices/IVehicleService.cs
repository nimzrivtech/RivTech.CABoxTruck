using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.DTO.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IVehicleService
    {
        Task<List<VehicleDTO>> GetAllAsync();
        Task<List<VehicleDTO>> GetByRiskDetailIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);
        Task<VehicleDTO> GetByIdAsync(Guid id);
        Task<VehicleDTO> InsertAsync(VehicleDTO model);
        Task<List<VehicleDTO>> InsertRangeAsync(List<VehicleDTO> model);
        Task<VehicleDTO> UpdateAsync(VehicleDTO model, bool updateCoverage = false, RiskCoverageDTO oldLimitsData = null);
        Task<List<VehicleDTO>> UpdateAddressesAsync(List<VehicleDTO> model);
        Task<List<VehicleDTO>> UpdateOtherCoveragesAsync(List<VehicleDTO> model, RiskCoverageDTO oldLimitsData);

        Task<List<VehicleDTO>> UpdateListOptionAsync(List<VehicleDTO> model);

        Task<string> RemoveAsync(Guid id, bool? fromEndorsement = false);

        Task<List<VehicleDTO>> GetByRiskDetailIdIncludeAsync(Guid id, bool? withDeleted = false, bool fromUI = false);
        Task<List<VehicleDTO>> GetAllIncludeAsync();
        Task<List<VehicleDTO>> GetByIdIncludeAsync(Guid id);
        Task<List<VehicleDTO>> GetPreviousVehicles(Guid id);

        Task<List<VehicleDTO>> AddFromExcelFileAsync(Stream fileStream);

        Task<VehicleDTO> ReinstateVehicleAsync(Guid id);

        Task UpdatePremiums(Guid riskDetailId, short? optionNumber = null);
        Task UpdateVehicleEffectiveDateExpirationDate(Guid riskDetailId, DateTime effectiveDate, DateTime expirationDate);
        Task UpdateMainUseAsync(Guid riskDetailId, short mainUseId);
        Task UpdateAccountCategoryAsync(Guid riskDetailId, short value);
        Task RemovePremiumRatingFactorOfUncheckedsAsync(Guid riskDetailId, int optionNumber);
        Task<List<VehicleDTO>> GetByRiskDetailIdForEndorsementAsync(Guid riskDetailId, bool? fromPolicy = false);
        Task<bool> IsOldVersion(Guid vehicleId);
        Task<DateTime?> GetEffectiveDateFromPreviousRiskDetail(VehicleDTO vehicle, Guid riskId);
        Task ClearPremiums(Guid id);
        Task<string> RemoveAllVinAsync(Guid riskDetailId);
        Task<List<VehicleDTO>> GetByRiskDetailIdIncludePolicyAsync(Guid id, bool? withDeleted = false, bool fromUI = false);
        Task UpdateVehicleEffectiveDateAsync(VehicleDTO dto);

        Task<DateTime?> GetVehicleEffectiveDate(VehicleDTO vehicle, Guid currentRiskDetailId);
        Task UpdateVehicleHeader(VehicleHeaderDTO dto);
    }
}
