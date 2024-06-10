using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskStatusHistoryConfiguration : IEntityTypeConfiguration<RiskStatusHistory>
    {
        public void Configure(EntityTypeBuilder<RiskStatusHistory> builder)
        {
            builder.Property(e => e.ChangedField).HasColumnType("varchar(200)").HasMaxLength(200);
            builder.Property(e => e.OldValue).HasColumnType("varchar(200)").HasMaxLength(200);
            builder.Property(e => e.NewValue).HasColumnType("varchar(200)").HasMaxLength(200);

            builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }
}