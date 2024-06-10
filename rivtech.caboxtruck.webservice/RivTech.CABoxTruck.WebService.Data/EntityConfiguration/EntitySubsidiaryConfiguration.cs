using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class EntitySubsidiaryConfiguration : IEntityTypeConfiguration<Entity.EntitySubsidiary>
    {
        public void Configure(EntityTypeBuilder<Entity.EntitySubsidiary> builder)
        {
            builder.Property(e => e.Name).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Relationship).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.AddProcessDate).IsRequired();
            builder.Property(e => e.AddedBy).IsRequired();

            builder.HasOne(x => x.Entity).WithMany().HasForeignKey(x => x.EntityId);
        }
    }
}
