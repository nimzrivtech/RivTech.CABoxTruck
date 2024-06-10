using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
        }
    }
}
