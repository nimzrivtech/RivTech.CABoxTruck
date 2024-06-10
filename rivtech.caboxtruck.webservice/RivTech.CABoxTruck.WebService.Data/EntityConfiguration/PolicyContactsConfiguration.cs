using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class PolicyContactsConfiguration : IEntityTypeConfiguration<PolicyContacts>
    {
        public void Configure(EntityTypeBuilder<PolicyContacts> builder)
        {
            builder.Property(e => e.Name).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Email).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Position).HasColumnType("varchar(100)").HasMaxLength(100);
        }
    }
}
