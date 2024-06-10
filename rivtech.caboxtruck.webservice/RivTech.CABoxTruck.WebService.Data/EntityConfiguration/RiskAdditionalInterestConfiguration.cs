using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskAdditionalInterestConfiguration : IEntityTypeConfiguration<RiskAdditionalInterest>
    {
        public void Configure(EntityTypeBuilder<RiskAdditionalInterest> builder)
        {
            builder.Property(e => e.AddProcessDate).IsRequired();
            builder.Property(e => e.AddedBy).IsRequired();
            builder.Property(e => e.EndorsementNumber).IsRequired();

            builder.HasOne(e => e.Entity).WithMany()
                    .HasForeignKey(e => e.EntityId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
