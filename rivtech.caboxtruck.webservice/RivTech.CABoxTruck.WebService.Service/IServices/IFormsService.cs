using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IFormsService
    {
        Task<List<RiskFormDTO>> GetByRiskIdAsync(Guid id);
        Task<List<RiskFormDTO>> GetSelectedByRiskIdAsync(Guid riskId);
        Task<List<RiskFormDTO>> UpdateAsync(List<RiskFormDTO> riskForms);
        Task<bool> UpdateSelectedAsync(List<RiskFormDTO> riskForms);
        Task<RiskFormDTO> InsertUserFormAsync(RiskFormDTO riskForms);
        Task<List<RiskFormDTO>> InsertDefaultsAsync(Guid riskId);
        Task<RiskFormDTO> SingleFormUpdateAsync(RiskFormDTO riskForm);
        Task<RiskFormDTO> UpdateOtherAsync<T>(RiskFormOtherDTO riskForm);
        Task SaveFormsContentAsync(RiskDetail riskDetail, Guid? id = null, DateTime? effectiveDate = null, 
            List<DriverDTO> initialExcludedDrivers = null, bool isEffectiveDateChanged = false);
        Task<bool> RemoveFormAsync(Guid id);
        Task<RiskFormDTO> UpdateDefaultStatesAsync(Guid id);
        Task<bool> HasInitialDriverExclusionFormAsync(Guid riskId);
        Task<RiskFormDTO> RiskFormUpdateAsync(RiskFormDTO riskForm);
        Task<List<SymbolTenResponseDTO>> GetSymbolTenResponses(Guid riskDetailId);
        Task<List<RiskFormDTO>> InsertDefaultsNewAsync(Guid riskId);

    }
}