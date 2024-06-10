using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class CancellationBreakdownConfiguration : IEntityTypeConfiguration<CancellationBreakdown>
    {
        public void Configure(EntityTypeBuilder<CancellationBreakdown> builder)
        {
            builder.Property(e => e.RiskDetailId);
            builder.Property(e => e.AmountSubTypeId).HasColumnType("nvarchar(50)");
            builder.Property(e => e.Description).HasColumnType("nvarchar(150)");
            builder.Property(e => e.ActiveAmount).HasColumnType("decimal(12,2)");
            builder.Property(e => e.CancelledAmount).HasColumnType("decimal(12,2)");
            builder.Property(e => e.AmountReduction).HasColumnType("decimal(12,2)");
            builder.Property(e => e.AnnualMinimumPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.MinimumPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.MinimumPremiumAdjustment).HasColumnType("decimal(12,2)");
            builder.Property(e => e.ShortRatePremium).HasColumnType("decimal(12,2)");
        }
    }
}

