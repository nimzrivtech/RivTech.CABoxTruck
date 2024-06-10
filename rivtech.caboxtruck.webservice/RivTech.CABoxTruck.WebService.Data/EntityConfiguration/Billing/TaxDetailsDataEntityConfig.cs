using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Billing
{
    public class TaxDetailsDataEntityConfig : IEntityTypeConfiguration<TaxDetailsData>
    {
        public void Configure(EntityTypeBuilder<TaxDetailsData> builder)
        {
            builder.ToTable("TaxDetails");
            builder.Property(x => x.TotalSLTax).HasColumnType("decimal(12,2)");
            builder.Property(x => x.TotalStampingFee).HasColumnType("decimal(12,2)");
            builder.Property(x => x.TotalTax).HasColumnType("decimal(12,2)");
            builder.OwnsMany(x => x.TaxDetailsBreakdowns,
                tdb => {
                    tdb.ToTable("TaxDetailsBreakdown");
                    tdb.HasKey(x => x.Id);
                    tdb.WithOwner().HasForeignKey(x => x.TaxDetailsId);
                    tdb.Property(x => x.TaxAmountTypeId);
                    tdb.Property(x => x.TaxAmountSubtypeId);
                    tdb.Property(x => x.Amount).HasColumnType("decimal(12, 2)");
                }); 
        }
    }
}
