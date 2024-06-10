using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;

namespace RivTech.CABoxTruck.WebService.ExternalData.EntityConfiguration
{
    public class EmailQueueConfiguration : IEntityTypeConfiguration<EmailQueue>
    {
        public void Configure(EntityTypeBuilder<EmailQueue> builder)
        {
            builder.Property(e => e.Address).HasColumnType("nvarchar(4000)").IsRequired();
            builder.Property(e => e.BccAddress).HasColumnType("nvarchar(4000)");
            builder.Property(e => e.CcAddress).HasColumnType("nvarchar(4000)");
            builder.Property(e => e.FromAddress).HasColumnType("nvarchar(250)");
            builder.Property(e => e.Subject).HasColumnType("nvarchar(250)").IsRequired();
            builder.Property(e => e.Content).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(e => e.AttachmentUrl).HasColumnType("nvarchar(max)");
            builder.Property(e => e.AttachmentFileName).HasColumnType("nvarchar(250)");
            builder.Property(e => e.Attachment).HasColumnType("varbinary(max)");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime2").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime2").IsRequired();
            builder.Property(e => e.Type).HasColumnType("nvarchar(250)").IsRequired();
            builder.Property(e => e.IsSent).HasColumnType("bit").IsRequired();
        }
    }
}
