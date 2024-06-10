using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Rater
{
    public class PremiumRatingFactorsDTO
    {
        public Guid RiskDetailId { get; set; }
        public Guid RiskCoverageId { get; set; }
        public string AccountDriverFactor { get; set; }
        public string AdditionalInsuredPremium { get; set; }
        public string GeneralLiabilityPremium { get; set; }
        //public string PremiumTaxesFees { get; set; }
        public string PrimaryNonContributoryPremium { get; set; }
        public string RiskManagementFeeAL { get; set; }
        public string RiskManagementFeeAPD { get; set; }
        //public string SubTotalDue { get; set; }
        //public string TotalAnnualPremiumWithPremiumTax { get; set; }
        public string TotalCargoPremium { get; set; }
        public string TotalLiabilityPremium { get; set; }
        //public string TotalOtherPolicyLevelPremium { get; set; }
        public string TotalPhysDamPremium { get; set; }
        public string ComprehensivePremium { get; set; }
        public string CollisionPremium { get; set; }
        // public string TotalVehicleLevelPremium { get; set; }
        public string WaiverOfSubrogationPremium { get; set; }
        public string HiredPhysicalDamage { get; set; }
        public string TrailerInterchange { get; set; }
        public string ALManuscriptPremium { get; set; }
        public string PDManuscriptPremium { get; set; }
        public int? OptionId { get; set; }
        public List<PremiumRaterDriverFactorsDTO> DriverFactors { get; set; }
        public List<PremiumRaterVehicleFactorsDTO> VehicleFactors { get; set; }
        public decimal? DriverVehicleRatio { get; set; }

        public string TrailerInterchangePremium { get; set; }
        public string NonOwnedTrailerPremium { get; set; }
    }
}
