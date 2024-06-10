using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Premiums
{
    public class RiskVehiclePremiumTransactionConfiguration : IEntityTypeConfiguration<RiskVehiclePremiumTransaction>
    {
        public void Configure(EntityTypeBuilder<RiskVehiclePremiumTransaction> builder)
        {
            builder.HasKey(rvp => rvp.Id);
            builder.Property(rvp => rvp.RiskDetailId);
            builder.Property(rvp => rvp.VehicleMainId);

            builder.Property(rvp => rvp.PremiumChange).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PremiumChangeAL).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PremiumChangePD).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PremiumChangeColl).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PremiumChangeCompFt).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PremiumChangeCargo).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PreviousPremium).HasColumnType("decimal(12,2)");

            builder.Property(rvp => rvp.CurrentALPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentPDPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentCargoPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentCompPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentCollPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentRefPremium).HasColumnType("decimal(12,2)");

            builder.Property(rvp => rvp.FromReinstatement);

            builder.Ignore(rvp => rvp.RiskVehiclePremiums);
        }
    }
}
