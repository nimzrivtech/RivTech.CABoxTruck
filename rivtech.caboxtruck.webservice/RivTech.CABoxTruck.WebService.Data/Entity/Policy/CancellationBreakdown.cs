using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Policy
{
    public class CancellationBreakdown : BaseEntity<long> 
    {
        public Guid RiskDetailId { get; set; }
        public string AmountSubTypeId { get; set; }
        public string Description { get; set; }
        public decimal ActiveAmount { get; set; } = 0;
        public decimal CancelledAmount { get; set; } = 0;
        public decimal AmountReduction { get; set; } = 0;
        public decimal AnnualMinimumPremium { get; set; } = 0;
        public decimal MinimumPremium { get; set; } = 0;
        public decimal MinimumPremiumAdjustment { get; set; } = 0;
        public decimal ShortRatePremium { get; set; } = 0;
    }
}
