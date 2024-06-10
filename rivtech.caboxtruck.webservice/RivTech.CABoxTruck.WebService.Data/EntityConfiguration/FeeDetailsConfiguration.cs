using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class FeeDetailsConfiguration : IEntityTypeConfiguration<FeeDetails>
    {
        public void Configure(EntityTypeBuilder<FeeDetails> builder)
        {
            builder.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
        }
    }
}
