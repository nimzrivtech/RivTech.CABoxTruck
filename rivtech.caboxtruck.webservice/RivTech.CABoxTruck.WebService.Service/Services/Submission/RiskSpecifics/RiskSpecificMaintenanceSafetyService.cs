using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics
{
    public class RiskSpecificMaintenanceSafetyService : IRiskSpecificMaintenanceSafetyService
    {
        private readonly IRiskResponseService _riskResponseService;
        private readonly IRiskSpecificSafetyDeviceService _safetyDeviceService;

        public RiskSpecificMaintenanceSafetyService(
            IRiskResponseService riskResponseService,
            IRiskSpecificSafetyDeviceService safetyDeviceService
        )
        {
            _riskResponseService = riskResponseService;
            _safetyDeviceService = safetyDeviceService;
        }

        public async Task<List<RiskResponseDTO>> SaveMaintenanceSafety(RiskSpecificMaintenanceSafetyDTO maintenanceSafety)
        {
            await _safetyDeviceService.SaveRiskSpecificSafetyDevice(maintenanceSafety.SafetyDevices.SafetyDevice, maintenanceSafety.RiskDetailId);

            var excludeSafetyDevice = maintenanceSafety.SafetyDevices;
            excludeSafetyDevice.SafetyDevice = null;
            var safetyRiskResponse = await _riskResponseService.MapToRiskResponse(excludeSafetyDevice, maintenanceSafety.RiskDetailId);
            await _riskResponseService.SaveRiskResponse(safetyRiskResponse, LvQuestionSection.SafetyDevice.Id, maintenanceSafety.RiskDetailId);

            var maintenanceRiskResponse = await _riskResponseService.MapToRiskResponse(maintenanceSafety.MaintenanceQuestion, maintenanceSafety.RiskDetailId);
            return await _riskResponseService.SaveRiskResponse(maintenanceRiskResponse, LvQuestionSection.MaintenanceQuestion.Id, maintenanceSafety.RiskDetailId);
        }

        public async Task<RiskSpecificMaintenanceSafetyDTO> GetMaintenanceSafety(Guid riskId)
        {
            var maintenanceSafety = new RiskSpecificMaintenanceSafetyDTO();
            var response = await _riskResponseService.GetRiskResponseAllInclude(riskId, LvQuestionSection.MaintenanceQuestion.Id);
            maintenanceSafety.MaintenanceQuestion = _riskResponseService.MapToEntityType(new RiskSpecificMaintenanceQuestionDTO(), response);
            response = await _riskResponseService.GetRiskResponseAllInclude(riskId, LvQuestionSection.SafetyDevice.Id);
            maintenanceSafety.SafetyDevices = _riskResponseService.MapToEntityType(new RiskSpecificSafetyDevicesDTO(), response);
            maintenanceSafety.SafetyDevices.SafetyDevice = await _safetyDeviceService.GetRiskSpecificSafetyDevice(riskId);
            return maintenanceSafety;
        }
    }
}
