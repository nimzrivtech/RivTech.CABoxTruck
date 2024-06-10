using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class FormVersionConfiguration : IEntityTypeConfiguration<FormVersion>
    {
        public void Configure(EntityTypeBuilder<FormVersion> builder)
        {
            builder.Property(e => e.Id).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.FileName).HasColumnType("varchar(500)").HasMaxLength(500);
        }
    }
}