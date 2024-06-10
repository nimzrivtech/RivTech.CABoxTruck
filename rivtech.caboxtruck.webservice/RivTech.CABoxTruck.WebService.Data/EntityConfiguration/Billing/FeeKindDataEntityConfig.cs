using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Billing
{
    public class FeeKindDataEntityConfig : IEntityTypeConfiguration<FeeKindData>
    {
        public void Configure(EntityTypeBuilder<FeeKindData> builder)
        {
            builder.ToTable("FeeKind");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description);
            builder.Property(x => x.IsActive);
        }
    }
}
