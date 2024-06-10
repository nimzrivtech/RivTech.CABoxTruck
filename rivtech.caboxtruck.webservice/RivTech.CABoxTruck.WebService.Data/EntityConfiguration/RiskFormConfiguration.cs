using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskFormConfiguration : IEntityTypeConfiguration<RiskForm>
    {
        public void Configure(EntityTypeBuilder<RiskForm> builder)
        {
            builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
            builder.HasOne(x => x.Form).WithMany().HasForeignKey(x => x.FormId);
        }
    }
}