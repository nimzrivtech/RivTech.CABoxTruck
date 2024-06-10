using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificRadiusOfOperationConfiguration : IEntityTypeConfiguration<RiskSpecificRadiusOfOperation>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificRadiusOfOperation> builder)
        {
            builder.Property(x => x.ExposureDescription).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.FiftyOneToTwoHundredMilesPercentage).HasColumnType("decimal(5, 2)");
            builder.Property(x => x.TwoHundredPlusMilesPercentage).HasColumnType("decimal(5, 2)");
            builder.Property(x => x.ZeroToFiftyMilesPercentage).HasColumnType("decimal(5, 2)");
            builder.Property(x => x.OwnerOperatorPercentage).HasColumnType("decimal(5, 2)");
            builder.Property(x => x.AverageMilesPerVehicle).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Risk)
                   .WithOne(x => x.RadiusOfOperations)
                   .HasForeignKey<RiskSpecificRadiusOfOperation>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
