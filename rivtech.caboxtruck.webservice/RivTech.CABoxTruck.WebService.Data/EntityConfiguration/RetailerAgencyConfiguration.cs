using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RetailerAgencyConfiguration : IEntityTypeConfiguration<RetailerAgency>
    {
        public void Configure(EntityTypeBuilder<RetailerAgency> builder)
        {
            builder.HasOne(x => x.Retailer).WithMany(x => x.LinkedAgencies).HasForeignKey(x => x.RetailerId);
        }
    }
}