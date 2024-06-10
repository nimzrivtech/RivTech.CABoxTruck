using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class UserNotificationArchiveConfiguration : IEntityTypeConfiguration<UserNotificationArchive>
    {
        public void Configure(EntityTypeBuilder<UserNotificationArchive> builder)
        {
            builder.Property(e => e.IsRead).HasDefaultValue(0).IsRequired();
            builder.Property(e => e.DateCreated).IsRequired();
        }
    }
}
