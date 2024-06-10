using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskManuscriptsConfiguration : IEntityTypeConfiguration<RiskManuscripts>
    {
        public void Configure(EntityTypeBuilder<RiskManuscripts> builder)
        {
            builder.Property(e => e.Title).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Description).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(e => e.Premium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ProratedPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PremiumType).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }
}