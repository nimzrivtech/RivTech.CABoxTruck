using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Drivers;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskDetailDriverConfiguration : IEntityTypeConfiguration<RiskDetailDriver>
    {
        public void Configure(EntityTypeBuilder<RiskDetailDriver> builder)
        {
            builder.HasKey(rdd => new { rdd.RiskDetailId, rdd.DriverId });

            builder.HasOne(rdd => rdd.RiskDetail)
                .WithMany(rdd => rdd.RiskDetailDrivers)
                .HasForeignKey(rdd => rdd.RiskDetailId);

            builder.HasOne(rdd => rdd.Driver)
                .WithMany(rdd => rdd.RiskDetailDrivers)
                .HasForeignKey(rdd => rdd.DriverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
