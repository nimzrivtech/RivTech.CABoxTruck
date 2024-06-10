using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskCoverageConfiguration : IEntityTypeConfiguration<RiskCoverage>
    {
        public void Configure(EntityTypeBuilder<RiskCoverage> builder)
        {
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.AddProcessDate).IsRequired();
            builder.Property(e => e.AddedBy).IsRequired();

            builder.Property(e => e.DepositPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.DepositPct).HasColumnType("decimal(12,2)");

            builder.Property(e => e.ApdExperienceRF).HasColumnType("decimal(12,6)");
            builder.Property(e => e.ApdScheduleRF).HasColumnType("decimal(12,6)");
            // builder.Property(e => e.BrokerCommisionAL).HasColumnType("decimal(12,2)");
            // builder.Property(e => e.BrokerCommisionPD).HasColumnType("decimal(12,2)");
            builder.Property(e => e.CargoExperienceRF).HasColumnType("decimal(12,6)");
            builder.Property(e => e.CargoScheduleRF).HasColumnType("decimal(12,6)");
            builder.Property(e => e.GlExperienceRF).HasColumnType("decimal(12,6)");
            builder.Property(e => e.GlScheduleRF).HasColumnType("decimal(12,6)");
            builder.Property(e => e.LiabilityExperienceRatingFactor).HasColumnType("decimal(12,6)");
            builder.Property(e => e.LiabilityScheduleRatingFactor).HasColumnType("decimal(12,6)");

            // builder.HasOne(x => x.RiskCoveragePremium).WithOne();
            builder.Ignore(e => e.CompFTDeductibleId);
            builder.Property(e => e.IsPolicyFeeSingleUnit).HasColumnType("bit");
        }
    }
}
