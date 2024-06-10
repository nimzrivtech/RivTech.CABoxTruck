using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificSafetyDevice : BaseEntity<Int16>
    {
        public Guid RiskDetailId { get; set; }
        public Int16 SafetyDeviceCategoryId { get; set; }
        public bool? IsInPlace { get; set; }
        public string UnitDescription { get; set; }
        public Int16? YearsInPlace { get; set; }
        public decimal? PercentageOfFleet { get; set; }

        public LvRiskSpecificSafetyDeviceCategory SafetyDeviceCategory { get; set; }
    }
}
