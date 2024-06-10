using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety
{
    public class RiskSpecificMaintenanceSafetyDTO
    {
        public Guid RiskDetailId { get; set; }
        public RiskSpecificMaintenanceQuestionDTO MaintenanceQuestion { get; set; }
        public RiskSpecificSafetyDevicesDTO SafetyDevices { get; set; }
    }
}
