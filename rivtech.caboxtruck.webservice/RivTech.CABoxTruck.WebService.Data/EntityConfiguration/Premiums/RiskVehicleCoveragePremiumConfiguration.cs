using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Premiums
{
    public class RiskVehicleCoveragePremiumConfiguration : IEntityTypeConfiguration<VehicleCoveragePremium>
    {
        public void Configure(EntityTypeBuilder<VehicleCoveragePremium> builder)
        {
            builder.ToTable("RiskVehiclePremiumCoverage");
            builder.HasKey(x => x.Id);

            var coveragePeriod = builder.OwnsOne(rvp => rvp.CoveragePeriod);
            coveragePeriod.Property(cp => cp.Start).HasColumnName("EffectiveDate").IsRequired().HasColumnType("date");
            coveragePeriod.Property(cp => cp.End).HasColumnName("ExpirationDate").IsRequired().HasColumnType("date");

            var premiumCoverage = builder.OwnsOne(rvp => rvp.Premium);
            premiumCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").IsRequired().HasDefaultValue(0);
            premiumCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").IsRequired().HasDefaultValue(0);

            var alCoverage = builder.OwnsOne(rvp => rvp.ALPremium);
            alCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").IsRequired().HasDefaultValue(0);
            alCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").IsRequired().HasDefaultValue(0);

            var pdCoverage = builder.OwnsOne(rvp => rvp.PDPremium);
            pdCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").HasDefaultValue(0);
            pdCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").HasDefaultValue(0);

            var cargoCoverage = builder.OwnsOne(rvp => rvp.CargoPremium);
            cargoCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").HasDefaultValue(0);
            cargoCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").HasDefaultValue(0);

            var compftCoverage = builder.OwnsOne(rvp => rvp.CompFTPremium);
            compftCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").HasDefaultValue(0);
            compftCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").HasDefaultValue(0);

            var collCoverage = builder.OwnsOne(rvp => rvp.CollisionPremium);
            collCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").HasDefaultValue(0);
            collCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").HasDefaultValue(0);

            var refrigerationCoverage = builder.OwnsOne(rvp => rvp.RefrigerationPremium);
            refrigerationCoverage.Property(x => x.Annual).HasColumnType("decimal(12,2)").HasDefaultValue(0);
            refrigerationCoverage.Property(x => x.Prorated).HasColumnType("decimal(15,5)").HasDefaultValue(0);
        }
    }
}
