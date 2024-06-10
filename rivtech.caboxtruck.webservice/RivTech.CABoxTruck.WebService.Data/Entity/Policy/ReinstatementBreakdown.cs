using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Policy
{
    public class ReinstatementBreakdown : BaseEntity<long>
    {
        public Guid RiskDetailId { get; set; }
        public string AmountSubTypeId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal MinimumPremiumAdjustment { get; set; } = 0;
        public decimal ShortRatePremium { get; set; } = 0;

        public static ReinstatementBreakdown FromCancellation(CancellationBreakdown cancellationBreakdown, Guid reinstatedRiskDetailId)
        {
            return new ReinstatementBreakdown
            {
                RiskDetailId = reinstatedRiskDetailId,
                AmountSubTypeId = cancellationBreakdown.AmountSubTypeId,
                Description = cancellationBreakdown.Description,
                Amount = cancellationBreakdown.AmountReduction * -1,
                MinimumPremiumAdjustment = cancellationBreakdown.MinimumPremiumAdjustment * -1,
                ShortRatePremium = cancellationBreakdown.ShortRatePremium * -1,
            };
        }
    }
}
