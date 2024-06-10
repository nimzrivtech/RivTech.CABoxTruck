using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety
{ 
    public class RiskSpecificSafetyDevicesDTO
    {
        public List<RiskSpecificSafetyDeviceDTO> SafetyDevice { get; set; }
        public List<SelectItemDTO> SafetyDeviceCategory { get; set; }

        public RiskSpecificSafetyDevicesDTO()
        {
            SafetyDevice = new List<RiskSpecificSafetyDeviceDTO>();
            SafetyDeviceCategory = new List<SelectItemDTO>();
        }
    }

    public class RiskSpecificSafetyDeviceDTO
    {
        public Int16 Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public Int16 SafetyDeviceCategoryId { get; set; }
        public bool? IsInPlace { get; set; }
        public string UnitDescription { get; set; }
        public Int16? YearsInPlace { get; set; }
        public decimal? PercentageOfFleet { get; set; }
    }
}
