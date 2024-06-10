using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class VehiclePremiumRatingFactor : BaseEntity<long>
    {
        public Guid RiskDetailId { get; set; }
        public string VehicleId { get; set; }
        public decimal? ALPremium { get; set; }
        public decimal? TotalPremium { get; set; }
        public decimal? TotalALPremium { get; set; }
        public decimal? TotalALAddOns { get; set; }
        public decimal? TotalPDPremium { get; set; }
        public decimal? BIPremium { get; set; }
        public decimal? PropDamagePremium { get; set; }
        public decimal? MedPayPremium { get; set; }
        public decimal? PIPremium { get; set; }
        public decimal? PIOtherPremium { get; set; }
        public decimal? UMUIM_BI_Premium { get; set; }
        public decimal? UMUIM_PD_Premium { get; set; }
        public decimal? CompPremium { get; set; }
        public decimal? FireTheftPremium { get; set; }
        public decimal? CollisionPremium { get; set; }
        public decimal? CargoPremium { get; set; }
        public decimal? RefrigerationPremium { get; set; }
        public decimal? NTLPremium { get; set; }
        public decimal? BIRiskMgmtFee { get; set; }
        public decimal? PDRiskMgmtFee { get; set; }
        public decimal? StatedAmount { get; set; }
        public decimal? TIVPercentage { get; set; }
        public decimal? PerVehiclePremium { get; set; }
        public decimal? Radius { get; set; }
        public decimal? RadiusTerritory { get; set; }
        public int OptionId { get; set; }

        public static VehiclePremiumRatingFactor Empty(Guid id)
        {
            return new VehiclePremiumRatingFactor
            {
                VehicleId = id.ToString(),
                TotalALPremium = 0,
                TotalPDPremium = 0,
                CargoPremium = 0,
                CompPremium = 0,
                CollisionPremium = 0,
                RefrigerationPremium = 0
            };
        }
    }
}
