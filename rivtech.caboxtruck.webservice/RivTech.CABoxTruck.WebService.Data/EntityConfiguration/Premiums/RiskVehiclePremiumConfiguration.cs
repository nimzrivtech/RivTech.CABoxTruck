using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Premiums
{
    public class RiskVehiclePremiumConfiguration : IEntityTypeConfiguration<RiskVehiclePremium>
    {
        public void Configure(EntityTypeBuilder<RiskVehiclePremium> builder)
        {
            builder.HasKey(rvp => rvp.Id);
            builder.Property(rvp => rvp.RiskDetailId);
            builder.Property(rvp => rvp.VehicleMainId);
            builder.Property(rvp => rvp.Order);
            builder.Property(rvp => rvp.FromReinstatement);
            builder.HasOne(rvp => rvp.CoveragePremium).WithMany(cp => cp.RiskVehiclePremiums)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
