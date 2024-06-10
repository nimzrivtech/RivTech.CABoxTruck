using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(e => e.State).HasColumnType("varchar(3)").HasMaxLength(3);
            builder.HasOne(e => e.Entity).WithOne().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.DriverIncidents).WithOne().HasForeignKey(x => x.DriverId);
        }

        public void Configure(EntityTypeBuilder<DriverIncidents> builder)
        {
        }

        public void Configure(EntityTypeBuilder<DriverHeader> builder)
        {
            builder.Property(x => x.driverVehicleRatio).HasColumnType("decimal(12,5)");
        }
    }
}
