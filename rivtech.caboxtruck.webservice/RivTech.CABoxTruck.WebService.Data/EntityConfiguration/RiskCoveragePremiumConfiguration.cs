using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskCoveragePremiumConfiguration : IEntityTypeConfiguration<RiskCoveragePremium>
    {
        public void Configure(EntityTypeBuilder<RiskCoveragePremium> builder)
        {
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.AddProcessDate).IsRequired();
            builder.Property(e => e.AddedBy).IsRequired();

            builder.Property(e => e.AutoLiabilityPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.CargoPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PhysicalDamagePremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ComprehensivePremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.CollisionPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.VehicleLevelPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.GeneralLiabilityPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TrailerInterchangeAL).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NonOwnedTrailerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgPerVehicle).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.RiskMgrFeeAL).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.RiskMgrFeePD).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PolicyTotals).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.Premium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.Fees).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TaxesAndStampingFees).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.DepositAmount).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.InstallmentFee).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PerInstallment).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalAmounDueFull).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalAmountDueInstallment).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AccountDriverFactor).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AdditionalInsuredPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PremiumTaxesFees).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PrimaryNonContributoryPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.SubTotalDue).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalAnnualPremiumWithPremiumTax).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalOtherPolicyLevelPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.WaiverOfSubrogationPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TaxDetailsId).IsRequired(false); //.HasConversion(g => g.ToString(), str => Guid.Parse(str));
            builder.Property(e => e.PerInstallmentPremium).HasColumnType("decimal(12, 2)"); //.HasConversion(g => g.ToString(), str => Guid.Parse(str));
            builder.Property(e => e.PerInstallmentFee).HasColumnType("decimal(12, 2)"); //.HasConversion(g => g.ToString(), str => Guid.Parse(str));
            builder.Property(e => e.PerInstallmentTax).HasColumnType("decimal(12, 2)"); //.HasConversion(g => g.ToString(), str => Guid.Parse(str));
            builder.Property(e => e.HiredPhysicalDamage).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TrailerInterchange).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalInstallmentFee).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ALManuscriptPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PDManuscriptPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedALManuscriptPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedPDManuscriptPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedGeneralLiabilityPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedTrailerInterchangePremiumAL).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedNonOwnedTrailerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.MinimumPremiumEarned).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ShortRatePremium).HasColumnType("decimal(12, 2)");

            builder.Ignore(e => e.TotalPremium);
        }
    }
}
