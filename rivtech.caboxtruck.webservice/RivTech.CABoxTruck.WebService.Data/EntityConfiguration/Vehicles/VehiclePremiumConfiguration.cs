using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Vehicles
{
    public class VehiclePremiumConfiguration : IEntityTypeConfiguration<VehiclePremium>
    {
        public void Configure(EntityTypeBuilder<VehiclePremium> builder)
        {
            builder.HasKey(vpc => vpc.Id);
            builder.Property(vpc => vpc.RiskDetailId);
            builder.Property(vpc => vpc.VehicleId);
            builder.Property(vpc => vpc.TransactionDate);

            //builder.HasOne(x => x.Previous);

            builder.Property(vpc => vpc.TotalAnnualPremium).HasColumnType("decimal(12, 2)");
            builder.Property(vpc => vpc.GrossProratedPremium).HasColumnType("decimal(12, 2)");
            builder.Property(vpc => vpc.NetProratedPremium).HasColumnType("decimal(12, 2)");

            var ALPremium = builder.OwnsOne(vpc => vpc.AL);
            ALPremium.Property(x => x.Annual).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            ALPremium.Property(x => x.GrossProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            ALPremium.Property(x => x.NetProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);

            var PDPremium = builder.OwnsOne(vpc => vpc.PD);
            PDPremium.Property(x => x.Annual).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            PDPremium.Property(x => x.GrossProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            PDPremium.Property(x => x.NetProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);

            var CargoPremium = builder.OwnsOne(vpc => vpc.Cargo);
            CargoPremium.Property(x => x.Annual).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            CargoPremium.Property(x => x.GrossProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            CargoPremium.Property(x => x.NetProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);

            var CompFTPremium = builder.OwnsOne(vpc => vpc.CompFT);
            CompFTPremium.Property(x => x.Annual).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            CompFTPremium.Property(x => x.GrossProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            CompFTPremium.Property(x => x.NetProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);

            var CollisionPremium = builder.OwnsOne(vpc => vpc.Collision);
            CollisionPremium.Property(x => x.Annual).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            CollisionPremium.Property(x => x.GrossProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            CollisionPremium.Property(x => x.NetProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);

            var RefrigerationPremium = builder.OwnsOne(vpc => vpc.Refrigeration);
            RefrigerationPremium.Property(x => x.Annual).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            RefrigerationPremium.Property(x => x.GrossProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);
            RefrigerationPremium.Property(x => x.NetProrated).HasColumnType("decimal(12, 2)").IsRequired().HasDefaultValue(0);

            builder.Property(x => x.CreatedDate);
            builder.Property(x => x.CreatedBy);
        }
    }
}
