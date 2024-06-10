using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class ReinstatementBreakdownConfiguration : IEntityTypeConfiguration<ReinstatementBreakdown>
    {
        public void Configure(EntityTypeBuilder<ReinstatementBreakdown> builder)
        {
            builder.Property(e => e.RiskDetailId);
            builder.Property(e => e.AmountSubTypeId).HasColumnType("nvarchar(50)");
            builder.Property(e => e.Description).HasColumnType("nvarchar(150)");
            builder.Property(e => e.Amount).HasColumnType("decimal(12,2)");
            builder.Property(e => e.MinimumPremiumAdjustment).HasColumnType("decimal(12,2)");
            builder.Property(e => e.ShortRatePremium).HasColumnType("decimal(12,2)");
        }
    }
}

