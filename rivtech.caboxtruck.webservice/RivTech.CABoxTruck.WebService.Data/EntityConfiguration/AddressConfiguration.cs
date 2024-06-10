using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(e => e.StreetAddress1).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.StreetAddress2).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(x => x.ZipCodeExt).HasColumnType("varchar(10)").HasMaxLength(200);
            builder.Property(x => x.ZipCode).HasColumnType("varchar(5)").HasMaxLength(5);
            builder.Property(e => e.City).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.State).HasColumnType("varchar(3)").HasMaxLength(3);
            builder.Property(e => e.County).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Longitude).HasColumnType("decimal(11, 8)");
            builder.Property(x => x.Latitude).HasColumnType("decimal(10, 8)");
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
        }
    }
}
