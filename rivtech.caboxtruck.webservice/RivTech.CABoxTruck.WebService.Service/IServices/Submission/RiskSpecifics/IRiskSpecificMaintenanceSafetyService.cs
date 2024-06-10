using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificMaintenanceSafetyService
    {
        Task<List<RiskResponseDTO>> SaveMaintenanceSafety(RiskSpecificMaintenanceSafetyDTO maintenanceSafety);
        Task<RiskSpecificMaintenanceSafetyDTO> GetMaintenanceSafety(Guid riskId);
    }
}
