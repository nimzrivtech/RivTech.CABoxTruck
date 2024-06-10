using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class ClaimsHistory : BaseEntity<Guid>
    {
        public Guid RiskDetailId { get; set; }
        public int? Order { get; set; }

        public DateTime? LossRunDate { get; set; }
        public string GroundUpNet { get; set; }
        public decimal? BiPiPmpIncLossTotal { get; set; }
        public decimal? BiPiPmpIncLoss100KCap { get; set; }
        public decimal? PropertyDamageIncLoss { get; set; }
        public int? ClaimCountBiPip { get; set; }
        public int? ClaimCountPropertyDamage { get; set; }
        public int? OpenClaimsUnder500 { get; set; }
        public decimal? NetLoss { get; set; }
        public int? TotalClaimCount { get; set; }
        public int? CargoNumberOfClaims { get; set; }
        public int? GlNumberOfClaims { get; set; }

        public decimal? PhysicalDamageNetLoss { get; set; }
        public int? PhysicalDamageClaimCount { get; set; }
        public decimal? CargoNetLoss { get; set; }
        public int? CargoClaimCount { get; set; }
        public decimal? GeneralLiabilityNetLoss { get; set; }
        public int? GeneralLiabilityClaimCount { get; set; }

        public RiskDetail RiskDetail { get; set; }
    }
}