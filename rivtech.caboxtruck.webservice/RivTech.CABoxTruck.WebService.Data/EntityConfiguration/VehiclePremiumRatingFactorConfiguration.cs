using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class VehiclePremiumRatingFactorConfiguration : IEntityTypeConfiguration<VehiclePremiumRatingFactor>
    {
        public void Configure(EntityTypeBuilder<VehiclePremiumRatingFactor> builder)
        {
            builder.Property(e => e.RiskDetailId).HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.ALPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.BIPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.BIRiskMgmtFee).HasColumnType("decimal(12,2)");
            builder.Property(e => e.CargoPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.CollisionPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.CompPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.FireTheftPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.RefrigerationPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.MedPayPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.NTLPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.PDRiskMgmtFee).HasColumnType("decimal(12,2)");
            builder.Property(e => e.PIOtherPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.PIPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.PerVehiclePremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.PropDamagePremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.Radius).HasColumnType("decimal(12,2)");
            builder.Property(e => e.RadiusTerritory).HasColumnType("decimal(12,2)");
            builder.Property(e => e.StatedAmount).HasColumnType("decimal(12,2)");
            builder.Property(e => e.TIVPercentage).HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalALAddOns).HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalALPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalPDPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalPremium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.UMUIM_BI_Premium).HasColumnType("decimal(12,2)");
            builder.Property(e => e.UMUIM_PD_Premium).HasColumnType("decimal(12,2)");
        }
    }
}
