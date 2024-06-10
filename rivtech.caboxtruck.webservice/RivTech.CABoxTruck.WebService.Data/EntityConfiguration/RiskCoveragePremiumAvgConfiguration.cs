using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskCoveragePremiumAvgConfiguration : IEntityTypeConfiguration<RiskCoveragePremiumAvg>
    {
        public void Configure(EntityTypeBuilder<RiskCoveragePremiumAvg> builder)
        {
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.AddProcessDate).IsRequired();
            builder.Property(e => e.AddedBy).IsRequired();

            builder.Property(e => e.AvgAlVehicleTotalPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgAlVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgAlVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgApdVehicleTotalPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgApdVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgApdVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgMtcRefVehicleTotalPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgMtcRefVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgMtcRefVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgTotalVehiclePremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgTotalVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AvgTotalVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NumberOfVehicles).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NumberOfPowerUnits).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NumberOfTrailerOthers).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AlVehicleTotalPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AlVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AlVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ApdVehicleTotalPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ApdVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ApdVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.MtcRefVehicleTotalPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.MtcRefVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.MtcRefVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalVehiclePremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalVehiclePowerPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalVehicleTrailerOtherPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalAIPremium).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalManuPremium).HasColumnType("decimal(12, 2)");
        }
    }
}
