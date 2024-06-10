using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class ClaimsHistoryConfiguration : IEntityTypeConfiguration<ClaimsHistory>
    {
        public void Configure(EntityTypeBuilder<ClaimsHistory> builder)
        {
            builder.Property(e => e.GroundUpNet).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.BiPiPmpIncLossTotal).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.BiPiPmpIncLoss100KCap).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PropertyDamageIncLoss).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NetLoss).HasColumnType("decimal(12, 2)");

            builder.Property(e => e.PhysicalDamageNetLoss).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.CargoNetLoss).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.GeneralLiabilityNetLoss).HasColumnType("decimal(12, 2)");

            builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }
}