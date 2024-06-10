using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.DataSets
{
    public class CreditedOfficeConfiguration : IEntityTypeConfiguration<CreditedOffice>
    {
        public void Configure(EntityTypeBuilder<CreditedOffice> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(o => o.Description)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
