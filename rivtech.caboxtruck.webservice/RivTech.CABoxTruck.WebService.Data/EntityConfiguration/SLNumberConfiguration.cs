using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class SLNumberConfiguration : IEntityTypeConfiguration<SLNumber>
    {
        public void Configure(EntityTypeBuilder<SLNumber> builder)
        {
            builder.Property(e => e.StateCode).HasColumnType("varchar(2)").HasMaxLength(2);
            builder.Property(e => e.LicenseNumber).HasColumnType("varchar(15)").HasMaxLength(15);
        }
    }
}
