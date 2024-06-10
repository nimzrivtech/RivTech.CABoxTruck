using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskFilingConfiguration : IEntityTypeConfiguration<RiskFiling>
    {
        public void Configure(EntityTypeBuilder<RiskFiling> builder)
        {
            builder.Property(e => e.FilingTypeId).HasColumnType("smallint");
            builder.HasOne(e => e.RiskDetail)
                .WithMany(e => e.RiskFilings)
                .HasForeignKey(e => e.RiskDetailId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
