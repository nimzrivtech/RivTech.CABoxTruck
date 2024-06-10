using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class FTPDocumentConfiguration : IEntityTypeConfiguration<FTPDocument>
    {
        public void Configure(EntityTypeBuilder<FTPDocument> builder)
        {
            builder.Property(a => a.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(a => a.Category)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(a => a.Category);

            builder.Property(a => a.BatchId)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(a => a.BatchId);

            builder.Property(a => a.Description)
                .IsUnicode(false)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(a => a.FileName)
                .IsUnicode(false)
                .HasMaxLength(500);

            builder.Property(a => a.FilePath)
                .HasColumnType("varchar(MAX)");

            builder.Property(a => a.IsUploaded);

            builder.HasIndex(a => a.IsUploaded);

            builder.Property(a => a.Source)
                .IsUnicode(false)
                .HasMaxLength(50);
        }
    }
}
