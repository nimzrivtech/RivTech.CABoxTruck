using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class DriverPremiumRatingFactor : BaseEntity<long>
    {
        public Guid RiskDetailId { get; set; }
        public string DriverID { get; set; }
        public string FiveYearPoints { get; set; }
        public string DriverKey { get; set; }
        public string PointsFactor { get; set; }
        public string YearsExperienceFactor { get; set; }
        public string AgeFactor { get; set; }
        public string OutOfStateFactor { get; set; }
        public string TotalFactor { get; set; }
        public string Included { get; set; }
        public string TotalFactorIncl { get; set; }
        public string FactorIncl_AddDriver { get; set; }
        public string UnderAge25 { get; set; }
        public string UnderAge25AndClean { get; set; }
        public string HasMultipleAFOrMovingViolation { get; set; }
        public int OptionId { get; set; }
    }
}
