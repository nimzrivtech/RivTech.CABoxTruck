using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class AppLogConfiguration : IEntityTypeConfiguration<AppLog>
    {
        public void Configure(EntityTypeBuilder<AppLog> builder)
        {
            builder.Property(e => e.UserId).HasColumnType("bigint").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Type).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.TableName).HasColumnType("varchar(100)").HasMaxLength(100);

            builder.Property(e => e.OldValues).HasColumnType("varchar(max)");
            builder.Property(e => e.NewValues).HasColumnType("varchar(max)");

            builder.Property(e => e.AffectedColumns).HasColumnType("varchar(max)");
            builder.Property(e => e.PrimaryKey).HasColumnType("varchar(max)");
        }
    }
}
