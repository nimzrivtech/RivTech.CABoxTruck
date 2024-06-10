using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(a => a.UserId);
            builder.Property(e => e.Description).HasColumnType("varchar(MAX)");
            builder.Property(e => e.AuditType)
                .IsUnicode(false)
                .HasMaxLength(10);

            builder.Property(e => e.Action).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Method).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.CreatedDate).IsRequired();
        }
    }
}
