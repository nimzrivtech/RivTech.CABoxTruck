using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class FileUploadDocumentConfiguration : IEntityTypeConfiguration<FileUploadDocument>
    {
        public void Configure(EntityTypeBuilder<FileUploadDocument> builder)
        {
            builder.HasOne(x => x.FileCategory).WithMany();
            // builder.HasOne(x => x.CreditOffice).WithMany();
            builder.Property(e => e.Description).HasColumnType("nvarchar(300)").HasMaxLength(300);
            builder.Property(e => e.FileName).HasColumnType("nvarchar(550)").HasMaxLength(550);
            builder.Property(e => e.FilePath).HasColumnType("nvarchar(400)").HasMaxLength(400);
            builder.Property(e => e.MimeType).HasColumnType("nvarchar(150)").HasMaxLength(150);
            builder.Property(e => e.FileExtension).HasColumnType("nvarchar(50)").HasMaxLength(50);

            builder.HasOne(x => x.RiskDetail).WithMany(x => x.FileUploadDocuments).HasForeignKey(x => x.RiskDetailId);
        }
    }
}
