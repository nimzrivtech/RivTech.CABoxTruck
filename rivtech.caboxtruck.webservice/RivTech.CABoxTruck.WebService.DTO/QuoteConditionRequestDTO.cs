using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class QuoteConditionRequestDTO
    {
        public Guid Id { get; set; }
        public RiskCoverageDTO RiskCoverage { get; set; }
        public RiskCoverageDTO PreviousRiskCoverage { get; set; }
        public bool IsPolicy { get; set; }
    }
}
