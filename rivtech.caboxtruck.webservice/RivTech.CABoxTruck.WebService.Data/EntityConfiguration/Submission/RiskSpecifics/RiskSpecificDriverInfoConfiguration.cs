using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificDriverInfoConfiguration : IEntityTypeConfiguration<RiskSpecificDriverInfo>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificDriverInfo> builder)
        {
            builder.Property(x => x.PercentageOfStaff).HasColumnType("decimal(5, 2)");
            builder.Property(x => x.AnnualCostOfHire).HasColumnType("decimal(12, 2)");
            builder.Property(x => x.AnnualCostOfHireWithoutDriver).HasColumnType("decimal(12, 2)");
            builder.Property(x => x.AnnualIncomeDerivedFromLease).HasColumnType("decimal(12, 2)");
            builder.Property(x => x.AnnualIncomeDerivedFromLeaseWithoutDriver).HasColumnType("decimal(12, 2)");
            builder.Property(x => x.AddingDeletingVehiclesDuringTermDescription).HasColumnType("varchar(1000)");
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Risk)
                   .WithOne(x => x.DriverInfo)
                   .HasForeignKey<RiskSpecificDriverInfo>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
