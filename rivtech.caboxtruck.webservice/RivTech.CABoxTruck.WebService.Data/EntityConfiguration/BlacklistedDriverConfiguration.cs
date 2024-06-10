using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class BlacklistedDriverConfiguration : IEntityTypeConfiguration<BlacklistedDriver>
    {
        public void Configure(EntityTypeBuilder<BlacklistedDriver> builder)
        {
            builder.Property(e => e.FirstName).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.LastName).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.DriverLicenseNumber).HasColumnType("varchar(50)").HasMaxLength(130);
            builder.Property(e => e.IsActive).HasColumnType("bit");
            builder.Property(e => e.Comments).HasColumnType("varchar(1000)").HasMaxLength(1000);
        }
    }
}
