using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskHistory : BaseEntity<Guid>
    {
        public Guid RiskDetailId { get; set; }
        public int? HistoryYear { get; set; }
        public DateTime? PolicyTermStartDate { get; set; }
        public DateTime? PolicyTermEndDate { get; set; }
        public string InsuranceCarrierOrBroker { get; set; }
        public decimal? LiabilityLimits { get; set; }
        public decimal? LiabilityDeductible { get; set; }
        public decimal? AutoLiabilityPremiumPerVehicle { get; set; }
        public decimal? PhysicalDamagePremiumPerVehicle { get; set; }
        public decimal? TotalPremiumPerVehicle { get; set; }
        public int? NumberOfVehicles { get; set; }
        public DateTime? LossRunDateForEachTerm { get; set; }
        public int? NumberOfPowerUnits { get; set; }
        public decimal? NumberOfPowerUnitsAL { get; set; }
        public decimal? NumberOfPowerUnitsAPD { get; set; }
        public int? NumberOfSpareVehicles { get; set; }
        public int? NumberOfDrivers { get; set; }
        public decimal? CollisionDeductible { get; set; }
        public decimal? ComprehensiveDeductible { get; set; }
        public decimal? CargoLimits { get; set; }
        public decimal? RefrigeratedCargoLimits { get; set; }
        public decimal? GrossRevenues { get; set; }
        public int? TotalFleetMileage { get; set; }
        public int? NumberOfOneWayTrips { get; set; }

        public RiskDetail RiskDetail { get; set; }
    }
}