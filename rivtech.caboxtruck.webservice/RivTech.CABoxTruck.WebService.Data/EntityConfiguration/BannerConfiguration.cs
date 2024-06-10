using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.Property(a => a.Id).HasDefaultValueSql("newid()");

            builder.Property(a => a.Description)
                .IsUnicode(false)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.FileName)
                .IsUnicode(false)
                .HasMaxLength(200);

            builder.Property(a => a.FilePath)
                .IsUnicode(false)
                .HasMaxLength(500);

            builder.Property(a => a.PageLink)
                .IsUnicode(false)
                .HasMaxLength(500);

            builder.Property(a => a.DisplayOrder);
            builder.Property(a => a.CreatedBy);
            builder.Property(a => a.CreatedDate);
            builder.Property(a => a.IsActive);
            builder.Property(a => a.RemovedBy);
            builder.Property(a => a.RemovedDate);
        }
    }
}
