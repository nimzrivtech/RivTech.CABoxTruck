using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RetailerAgentConfiguration : IEntityTypeConfiguration<RetailerAgent>
    {
        public void Configure(EntityTypeBuilder<RetailerAgent> builder)
        {
            builder.HasOne(x => x.Retailer).WithMany().HasForeignKey(x => x.RetailerId);
            builder.HasOne(e => e.Entity).WithMany().HasForeignKey(e => e.EntityId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}