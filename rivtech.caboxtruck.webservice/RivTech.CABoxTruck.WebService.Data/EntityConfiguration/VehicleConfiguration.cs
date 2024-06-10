using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.Property(e => e.VIN).HasColumnType("varchar(17)").HasMaxLength(17);
            builder.Property(e => e.State).HasColumnType("varchar(3)").HasMaxLength(3);
            builder.Property(e => e.RegisteredState).HasColumnType("varchar(3)").HasMaxLength(3);
            builder.Property(e => e.PrimaryOperatingZipCode).HasColumnType("varchar(15)").HasMaxLength(15);
            builder.Property(e => e.StatedAmount).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.Radius).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.RadiusTerritory).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.AutoLiabilityPremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CargoPremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.RefrigerationPremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PhysicalDamagePremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.ComprehensivePremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CollisionPremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.TotalPremium).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CreatedBy).HasDefaultValue(0);

            builder.Ignore(e => e.VehiclePremiumRatingFactor);
            builder.Ignore(e => e.LatestVehiclePremium);
            builder.Ignore(e => e.VehiclePremiums);
            builder.Ignore(e => e.RiskVehiclePremiumTransaction);
            builder.Ignore(e => e.EndorsementNumber);
            //builder.HasOne(x => x.VehicleProratedPremium)
            //    .WithOne(x => x.Vehicle)
            //    .HasForeignKey<Vehicle>(x => x.VehicleProratedPremiumId);

            //builder.HasOne(e => e.RiskDetail)
            //    .WithMany(e => e.Vehicles)
            //    .HasForeignKey(e => e.RiskDetailId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
