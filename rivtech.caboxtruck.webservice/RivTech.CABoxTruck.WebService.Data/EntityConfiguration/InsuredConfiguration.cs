using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class InsuredConfiguration : IEntityTypeConfiguration<Insured>
    {
        public void Configure(EntityTypeBuilder<Insured> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();

            builder.HasOne(e => e.Entity).WithOne(e => e.Insured)
                .HasForeignKey<Insured>(e => e.EntityId);
        }
    }
}
