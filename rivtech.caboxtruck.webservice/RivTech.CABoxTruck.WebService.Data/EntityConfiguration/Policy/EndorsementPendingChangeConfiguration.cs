using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class EndorsementPendingChangeConfiguration : IEntityTypeConfiguration<EndorsementPendingChange>
    {
        public void Configure(EntityTypeBuilder<EndorsementPendingChange> builder)
        {
            builder.Property(e => e.PremiumBeforeEndorsement).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PremiumAfterEndorsement).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PremiumDifference).HasColumnType("decimal(12, 2)");
        }
    }
}

