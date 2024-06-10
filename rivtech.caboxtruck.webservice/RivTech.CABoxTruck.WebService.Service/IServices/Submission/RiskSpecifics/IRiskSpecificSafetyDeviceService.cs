using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificSafetyDeviceService
    {
        Task<List<RiskSpecificSafetyDeviceDTO>> GetRiskSpecificSafetyDevice(Guid riskId);
        Task<RiskSpecificSafetyDeviceDTO> InsertRiskSpecificSafetyDevice(RiskSpecificSafetyDeviceDTO safetyDevice);
        Task<RiskSpecificSafetyDeviceDTO> UpdateSpecificSafetyDevice(RiskSpecificSafetyDeviceDTO safetyDevice, Guid riskId);
        void DeleteSpecificSafetyDevice(RiskSpecificSafetyDeviceDTO safetyDevice);
        Task<List<RiskSpecificSafetyDeviceDTO>> SaveRiskSpecificSafetyDevice(List<RiskSpecificSafetyDeviceDTO> safetyDevices, Guid riskId);
    }
}
