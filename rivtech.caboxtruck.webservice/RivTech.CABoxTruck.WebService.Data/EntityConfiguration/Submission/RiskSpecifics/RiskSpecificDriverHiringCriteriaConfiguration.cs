using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificDriverHiringCriteriaConfiguration : IEntityTypeConfiguration<RiskSpecificDriverHiringCriteria>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificDriverHiringCriteria> builder)
        {
            builder.HasMany(x => x.BackGroundCheckIncludes).WithMany(x => x.RiskSpecificDriverHiringCriteria);
            builder.HasMany(x => x.DriverTrainingIncludes).WithMany(x => x.RiskSpecificDriverHiringCriteria);
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Risk)
                   .WithOne(x => x.DriverHiringCriteria)
                   .HasForeignKey<RiskSpecificDriverHiringCriteria>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
