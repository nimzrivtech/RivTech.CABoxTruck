using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Vehicles
{
    public class RiskDetailVehicleConfiguration : IEntityTypeConfiguration<RiskDetailVehicle>
    {
        public void Configure(EntityTypeBuilder<RiskDetailVehicle> builder)
        {
            builder.HasKey(rdv => new { rdv.RiskDetailId, rdv.VehicleId });

            //builder.HasOne(rdv => rdv.RiskDetail)
            //    .WithMany(rdv => rdv.RiskDetailVehicles)
            //    .HasForeignKey(rdv => rdv.RiskDetailId);

            //builder.HasOne(rdv => rdv.Vehicle)
            //    .WithMany(rdv => rdv.RiskDetailVehicles)
            //    .HasForeignKey(rdv => rdv.VehicleId);
        }
    }
}
