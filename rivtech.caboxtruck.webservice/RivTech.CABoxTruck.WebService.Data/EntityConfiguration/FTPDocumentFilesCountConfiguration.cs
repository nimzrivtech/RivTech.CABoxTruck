using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class FTPDocumentFilesCountConfiguration : IEntityTypeConfiguration<FTPDocumentFilesCount>
    {
        public void Configure(EntityTypeBuilder<FTPDocumentFilesCount> builder)
        {
            builder.Property(a => a.Id).HasDefaultValueSql("newid()");

            builder.Property(a => a.InvoiceCount);
            builder.HasIndex(a => a.InvoiceCount);

            builder.Property(a => a.RegularCount);
            builder.HasIndex(a => a.RegularCount);

            builder.Property(a => a.CertifiedCount);
            builder.HasIndex(a => a.CertifiedCount);
        }
    }
}
