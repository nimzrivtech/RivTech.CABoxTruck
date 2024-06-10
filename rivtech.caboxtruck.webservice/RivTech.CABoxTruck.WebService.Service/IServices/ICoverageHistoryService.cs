using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ICoverageHistoryService
    {
        Task<RiskHistoryListDTO> GetByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
        Task<RiskHistoryListDTO> InsertAsync(RiskHistoryListDTO riskHistory);
        Task<RiskHistoryListDTO> UpdateAsync(RiskHistoryListDTO riskHistory);
        Task DeleteFieldsAsync(Guid riskId, bool isVehicle = false, bool isDriver = false, bool isComprehensive = false, bool isCollision = false, bool isCargo = false, bool isRef = false);
        Task UpdatePowerUnitFieldsAsync(RiskHistoryDTO riskHistory);
    }
}
