using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(e => e.Category).HasColumnType("varchar(100)").IsRequired();
            builder.Property(e => e.NotificationId).IsRequired();
            builder.Property(e => e.Title).HasColumnType("varchar(400)").IsRequired();
            builder.Property(e => e.NotificationDetails).HasColumnType("varchar(6000)");
            builder.Property(e => e.UserId);
            builder.Property(e => e.CompletionPercentage);
            builder.Property(e => e.NotificationStatus).HasColumnType("varchar(400)");
            builder.Property(e => e.PolicyNumber).HasColumnType("varchar(200)");
            builder.Property(e => e.CompanyName).HasColumnType("varchar(400)");
            builder.Property(e => e.ErrorMessages).HasColumnType("varchar(6000)");
        }
    }
}
