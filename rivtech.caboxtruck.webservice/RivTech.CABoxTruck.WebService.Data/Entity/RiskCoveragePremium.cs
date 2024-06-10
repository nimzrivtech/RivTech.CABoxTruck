using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskCoveragePremium : BaseEntity<Guid>
    {
        public RiskCoveragePremium()
        {
            this.EffectiveDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now.AddYears(5);
            this.AddProcessDate = DateTime.Now;
        }
        public Guid RiskCoverageId { get; set; }
        public decimal? AutoLiabilityPremium { get; set; }
        public decimal? CargoPremium { get; set; }
        public decimal? PhysicalDamagePremium { get; set; }
        public decimal? ComprehensivePremium { get; set; }
        public decimal? CollisionPremium { get; set; }
        public decimal? VehicleLevelPremium { get; set; }
        public decimal? GeneralLiabilityPremium { get; set; }
        public decimal? TrailerInterchangeAL { get; set; }
        public decimal? NonOwnedTrailerPremium { get; set; }
        public decimal? AvgPerVehicle { get; set; }
        public decimal? RiskMgrFeeAL { get; set; }
        public decimal? RiskMgrFeePD { get; set; }
        public decimal? PolicyTotals { get; set; }
        public decimal? Premium { get; set; }
        public decimal? Fees { get; set; }
        public decimal? TaxesAndStampingFees { get; set; }
        public decimal? InstallmentFee { get; set; }
        public decimal? PerInstallment { get; set; }
        public decimal? TotalAmounDueFull { get; set; }
        public decimal? TotalAmountDueInstallment { get; set; }

        public decimal? AccountDriverFactor { get; set; }
        public decimal? AdditionalInsuredPremium { get; set; }
        public decimal? PremiumTaxesFees { get; set; }
        public decimal? PrimaryNonContributoryPremium { get; set; }
        public decimal? SubTotalDue { get; set; }
        public decimal? TotalAnnualPremiumWithPremiumTax { get; set; }
        public decimal? TotalOtherPolicyLevelPremium { get; set; }
        public decimal? WaiverOfSubrogationPremium { get; set; }
        public int? OptionId { get; set; }
        public Guid? TaxDetailsId { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public Int64? RemovedBy { get; set; }
        public decimal? DepositAmount { get; set; }
        public decimal? PerInstallmentPremium { get; set; }
        public decimal? PerInstallmentFee { get; set; }
        public decimal? PerInstallmentTax { get; set; }
        public decimal? HiredPhysicalDamage { get; set; }
        public decimal? TrailerInterchange { get; set; }
        public decimal? TotalInstallmentFee { get; set; }
        public decimal? ALManuscriptPremium { get; set; }
        public decimal? PDManuscriptPremium { get; set; }
        public decimal? ProratedALManuscriptPremium { get; set; }
        public decimal? ProratedPDManuscriptPremium { get; set; }
        public decimal? ProratedGeneralLiabilityPremium { get; set; }
        public decimal? ProratedTrailerInterchangePremiumAL { get; set; }
        public decimal? ProratedNonOwnedTrailerPremium { get; set; }
        public decimal? ProratedPremium { get; set; }
        [Precision(18, 2)]
        public decimal? ARSCommissionAmount { get; set; }

        [NotMapped]
        public decimal? GLPremium
        {
            get => ((ProratedGeneralLiabilityPremium ?? 0) == 0) ? GeneralLiabilityPremium : ProratedGeneralLiabilityPremium;
        }

        [NotMapped]
        public decimal? ManuscriptALPremium
        {
            get => ((ProratedALManuscriptPremium ?? 0) == 0) ? ALManuscriptPremium : ProratedALManuscriptPremium; // values can be negative
        }

        [NotMapped]
        public decimal? ManuscriptPDPremium
        {
            get => ((ProratedPDManuscriptPremium ?? 0) == 0) ? PDManuscriptPremium : ProratedPDManuscriptPremium; // values can be negative
        }

        public decimal? TotalPremium
        {
            get => ((ProratedPremium ?? 0) == 0) ? Premium : ProratedPremium;
        }
        public decimal MinimumPremiumEarned { get; set; }
        public decimal ShortRatePremium { get; set; }

        internal void ClearPremiums()
        {
            Premium = null;
            PremiumTaxesFees = null;
            AutoLiabilityPremium = null;
            PhysicalDamagePremium = null;
            CargoPremium = null;
            GeneralLiabilityPremium = null;
            TrailerInterchangeAL = null;
            NonOwnedTrailerPremium = null;
            ComprehensivePremium = null;
            CollisionPremium = null;
            ProratedPremium = null;
            ShortRatePremium = 0;
            VehicleLevelPremium = null;
            AvgPerVehicle = null;

            AdditionalInsuredPremium = null;
            PrimaryNonContributoryPremium = null;
            WaiverOfSubrogationPremium = null;

            DepositAmount = null;
            PerInstallmentPremium = null;
            TotalAnnualPremiumWithPremiumTax = null;
            TotalOtherPolicyLevelPremium = null;
            TotalAmounDueFull = null;

            TaxesAndStampingFees = null;
            InstallmentFee = null;
            Fees = null;

            RiskMgrFeeAL = null;
            RiskMgrFeePD = null;

            PerInstallmentFee = null;
            PerInstallmentPremium = null;
            PerInstallmentTax = null;
            PerInstallment = null;
            TotalInstallmentFee = null;
            TotalAmountDueInstallment = null;

            ARSCommissionAmount = null;
        }

        public RiskCoveragePremiumAvg RiskCoveragePremiumAvg { get; set; }
    }
}
