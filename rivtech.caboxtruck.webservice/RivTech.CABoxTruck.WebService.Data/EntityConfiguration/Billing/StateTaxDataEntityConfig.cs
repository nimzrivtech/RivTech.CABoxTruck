using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Billing
{
    public class StateTaxDataEntityConfig : IEntityTypeConfiguration<StateTaxData>
    {
        public void Configure(EntityTypeBuilder<StateTaxData> builder)
        {
            builder.ToTable("StateTax");
            builder.HasKey(x => new { x.StateCode, x.TaxType, x.TaxSubtype, x.EffectiveDate });
            builder.Property(x => x.StateCode).HasColumnType("varchar(3)").HasMaxLength(3);
            builder.Property(x => x.TaxType);
            builder.Property(x => x.TaxSubtype);
            builder.Property(x => x.Rate).HasColumnType("decimal(8,3)");
            builder.Property(x => x.EffectiveDate);
        }
    }
}
