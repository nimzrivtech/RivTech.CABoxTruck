using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Billing
{
    public class NonTaxableFeeDataEntityConfig : IEntityTypeConfiguration<NonTaxableFeeData>
    {
        public void Configure(EntityTypeBuilder<NonTaxableFeeData> builder)
        {
            builder.ToTable("NonTaxableFee");
            builder.HasKey(x => new { x.StateCode, x.FeeKindId, x.EffectiveDate });
            builder.Property(x => x.StateCode).HasColumnType("varchar(3)").HasMaxLength(3); 
            builder.Property(x => x.FeeKindId);
            builder.Property(x => x.EffectiveDate);
        }
    }
}
