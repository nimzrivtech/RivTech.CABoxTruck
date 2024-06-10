using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class InsuranceCompanyConfiguration : IEntityTypeConfiguration<InsuranceCompany>
    {
        public void Configure(EntityTypeBuilder<InsuranceCompany> builder)
        {
            builder.Property(e => e.CreatedBy).IsRequired();

            builder.HasOne(e => e.Entity).WithOne()
                .HasForeignKey<InsuranceCompany>(x => x.EntityId).IsRequired();
        }
    }
}