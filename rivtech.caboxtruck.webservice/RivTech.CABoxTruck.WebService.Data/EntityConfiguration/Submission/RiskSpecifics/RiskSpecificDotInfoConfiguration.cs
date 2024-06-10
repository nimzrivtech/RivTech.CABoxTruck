using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificDotInfoConfiguration : IEntityTypeConfiguration<RiskSpecificDotInfo>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificDotInfo> builder)
        {
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.RiskDetail)
                   .WithOne(x => x.DotInfo)
                   .HasForeignKey<RiskSpecificDotInfo>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
