using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class AgencyConfiguration : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.AgencyId).IsRequired(false);
            builder.HasOne(e => e.Entity).WithOne().HasForeignKey<Agency>(e => e.EntityId);
        }
    }


    public class SubAgencyConfiguration : IEntityTypeConfiguration<SubAgency>
    {
        public void Configure(EntityTypeBuilder<SubAgency> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.AgencyId).IsRequired(false);
            builder.HasOne(e => e.Entity).WithOne().HasForeignKey<SubAgency>(e => e.EntityId);
        }
    }
}
