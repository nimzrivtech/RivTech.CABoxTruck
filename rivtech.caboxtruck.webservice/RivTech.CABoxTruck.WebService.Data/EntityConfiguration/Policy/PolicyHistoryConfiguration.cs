using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class PolicyHistoryConfiguration : IEntityTypeConfiguration<PolicyHistory>
    {
        public void Configure(EntityTypeBuilder<PolicyHistory> builder)
        {
            builder.Property(e => e.CurrentPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NewPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PremiumChange).HasColumnType("decimal(12, 2)");
        }
    }
}

