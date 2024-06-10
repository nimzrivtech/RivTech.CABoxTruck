using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class SymbolTenResponseConfiguration : IEntityTypeConfiguration<SymbolTenResponse>
    {
        public void Configure(EntityTypeBuilder<SymbolTenResponse> builder)
        {
            builder.Property(e => e.IsActive).HasColumnType("bit").IsRequired();
            builder.Property(e => e.Description).HasColumnType("varchar(1000)").IsRequired();
        }
    }
}
