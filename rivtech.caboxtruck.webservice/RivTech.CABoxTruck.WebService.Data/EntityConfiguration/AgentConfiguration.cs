using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.AgencyId).IsRequired(false);
            builder.Property(e => e.SubAgencyId).IsRequired(false);
            builder.HasOne(e => e.Entity).WithOne().HasForeignKey<Agent>(e => e.EntityId);
        }
    }

    public class SubAgentConfiguration : IEntityTypeConfiguration<SubAgent>
    {
        public void Configure(EntityTypeBuilder<SubAgent> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.AgencyId).IsRequired(false);
            builder.Property(e => e.SubAgencyId).IsRequired(false);
            builder.HasOne(e => e.Entity).WithOne().HasForeignKey<SubAgent>(e => e.EntityId);
        }
    }
}
