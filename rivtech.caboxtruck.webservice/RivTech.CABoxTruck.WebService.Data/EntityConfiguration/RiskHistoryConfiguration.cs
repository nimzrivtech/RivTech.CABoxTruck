using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskHistoryConfiguration : IEntityTypeConfiguration<RiskHistory>
    {
        public void Configure(EntityTypeBuilder<RiskHistory> builder)
        {
            builder.Property(e => e.InsuranceCarrierOrBroker).HasColumnType("varchar(100)").HasMaxLength(100);

            builder.Property(e => e.LiabilityLimits).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.LiabilityDeductible).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.CollisionDeductible).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.ComprehensiveDeductible).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.CargoLimits).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.RefrigeratedCargoLimits).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.AutoLiabilityPremiumPerVehicle).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.PhysicalDamagePremiumPerVehicle).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.TotalPremiumPerVehicle).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.GrossRevenues).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NumberOfPowerUnitsAL).HasColumnType("decimal(12,2)");
            builder.Property(e => e.NumberOfPowerUnitsAPD).HasColumnType("decimal(12,2)");

            builder.Property(e => e.NumberOfPowerUnitsAL).HasColumnType("decimal(12, 2)");
            builder.Property(e => e.NumberOfPowerUnitsAPD).HasColumnType("decimal(12, 2)");

            builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }
}
