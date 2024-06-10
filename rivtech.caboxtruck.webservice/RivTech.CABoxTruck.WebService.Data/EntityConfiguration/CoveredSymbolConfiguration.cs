using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class CoveredSymbolConfiguration : IEntityTypeConfiguration<CoveredSymbol>
    {
        public void Configure(EntityTypeBuilder<CoveredSymbol> builder)
        {
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.AddProcessDate).IsRequired();
        }
    }
}
