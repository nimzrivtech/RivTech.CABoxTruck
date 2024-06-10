using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskPolicyContactConfiguration : IEntityTypeConfiguration<RiskPolicyContact>
    {
        public void Configure(EntityTypeBuilder<RiskPolicyContact> builder)
        {
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();

            builder.HasOne(e => e.Entity).WithMany()
                    .HasForeignKey(e => e.EntityId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
