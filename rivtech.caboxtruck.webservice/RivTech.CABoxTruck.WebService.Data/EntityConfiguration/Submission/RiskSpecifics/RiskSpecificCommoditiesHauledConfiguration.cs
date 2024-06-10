using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificCommoditiesHauledConfiguration : IEntityTypeConfiguration<RiskSpecificCommoditiesHauled>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificCommoditiesHauled> builder)
        {
            builder.Property(e => e.Commodity).HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            builder.Property(e => e.PercentageOfCarry).HasColumnType("decimal(5, 2)").IsRequired();
            builder.HasOne(e => e.RiskDetail)
                .WithMany(e => e.CommoditiesHauled)
                .HasForeignKey(e => e.RiskDetailId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}