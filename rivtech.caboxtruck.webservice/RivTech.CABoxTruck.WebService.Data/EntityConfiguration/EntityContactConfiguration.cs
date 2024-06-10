using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class EntityContactConfiguration : IEntityTypeConfiguration<Entity.EntityContact>
    {
        public void Configure(EntityTypeBuilder<Entity.EntityContact> builder)
        {
            builder.Property(e => e.PhoneExtension).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.Property(e => e.EmailAddress).HasColumnType("varchar(60)").HasMaxLength(60);
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.AddProcessDate).IsRequired(); 
            builder.Property(e => e.AddedBy).IsRequired();

            builder.HasOne(x => x.Entity).WithMany().HasForeignKey(x => x.EntityId);
        }
    }
}
