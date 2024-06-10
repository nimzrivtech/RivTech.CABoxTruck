using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class FieldVersionConfiguration : IEntityTypeConfiguration<FieldVersion>
    {
        public void Configure(EntityTypeBuilder<FieldVersion> builder)
        {
            builder.Property(e => e.Id).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.FieldName).HasColumnType("varchar(500)").HasMaxLength(500);
            builder.Property(e => e.Description).HasColumnType("varchar(1000)").HasMaxLength(1000);
        }
    }
}