using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class PolicyHistory : BaseEntity<long>
    {
        public Guid RiskDetailId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string TransactionNumber { get; set; }
        public string EndorsementNumber { get; set; }
        public decimal? CurrentPremium { get; set; }
        public decimal? NewPremium { get; set; }
        public decimal? ProratedPremium { get; set; }
        public decimal? PremiumChange { get; set; }
        public string Details { get; set; }
        public string PolicyStatus { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string PreviousRiskDetailId { get; set; }
        public string RiskId { get; set; }
    }
}
