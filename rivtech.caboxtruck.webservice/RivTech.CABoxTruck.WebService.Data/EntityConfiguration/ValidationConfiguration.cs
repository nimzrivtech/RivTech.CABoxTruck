using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class ValidationConfiguration : IEntityTypeConfiguration<Validation>
    {
        public void Configure(EntityTypeBuilder<Validation> builder)
        {
            builder.Property(e => e.Value).HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).HasColumnType("varchar(100)").HasMaxLength(100);

            builder.HasOne(x => x.ValidationType).WithMany().HasForeignKey(x => x.ValidationTypeId).IsRequired();
        }
    }
}
