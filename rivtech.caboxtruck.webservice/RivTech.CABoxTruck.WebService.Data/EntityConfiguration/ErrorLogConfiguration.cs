using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.Property(a => a.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(a => a.UserId);

            builder.Property(e => e.ErrorCode)
                .IsUnicode(false)
                .HasMaxLength(20);

            builder.Property(e => e.ErrorMessage)
                .HasColumnType("varchar(MAX)");

            builder.Property(e => e.JsonMessage)
                .HasColumnType("varchar(MAX)");

            builder.Property(e => e.Action)
                .IsUnicode(false)
                .HasMaxLength(300);

            builder.Property(e => e.Method)
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.Property(e => e.CreatedDate);
        }
    }
}
