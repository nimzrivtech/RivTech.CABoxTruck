using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class EntityAddressConfiguration : IEntityTypeConfiguration<Entity.EntityAddress>
    {
        public void Configure(EntityTypeBuilder<Entity.EntityAddress> builder)
        {
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired();

            builder.HasOne(x => x.Entity).WithMany(x => x.EntityAddresses).HasForeignKey(x => x.EntityId);
            builder.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId);

            //builder.Ignore(e => e.IsMainGaragingAddress);
        }
    }
}
