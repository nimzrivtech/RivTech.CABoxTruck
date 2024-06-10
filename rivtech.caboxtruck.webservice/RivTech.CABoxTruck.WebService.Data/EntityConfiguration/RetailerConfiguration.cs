using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RetailerConfiguration : IEntityTypeConfiguration<Retailer>
    {
        public void Configure(EntityTypeBuilder<Retailer> builder)
        {
            builder.HasOne(e => e.Entity).WithOne().HasForeignKey<Retailer>(e => e.EntityId);
            builder.HasMany(a => a.LinkedAgencies).WithOne(b => b.Retailer).HasForeignKey(c => c.RetailerId);
        }
    }
}