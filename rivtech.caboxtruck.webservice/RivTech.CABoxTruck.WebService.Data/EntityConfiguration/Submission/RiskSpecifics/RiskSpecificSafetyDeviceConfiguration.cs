using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificSafetyDeviceConfiguration : IEntityTypeConfiguration<RiskSpecificSafetyDevice>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificSafetyDevice> builder)
        {
            builder.Property(e => e.RiskDetailId).HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.SafetyDeviceCategoryId).HasColumnType("smallint").IsRequired();
            builder.Property(e => e.IsInPlace).HasColumnType("bit");
            builder.Property(e => e.UnitDescription).HasColumnType("varchar(1000)");
            builder.Property(e => e.YearsInPlace).HasColumnType("smallint");
            builder.Property(e => e.PercentageOfFleet).HasColumnType("decimal(12,2)");

            //builder.HasOne(x => x.Risk).WithMany().HasForeignKey(x => x.RiskId).IsRequired();
            builder.HasOne(x => x.SafetyDeviceCategory).WithMany().HasForeignKey(x => x.SafetyDeviceCategoryId).IsRequired();
        }
    }
}
