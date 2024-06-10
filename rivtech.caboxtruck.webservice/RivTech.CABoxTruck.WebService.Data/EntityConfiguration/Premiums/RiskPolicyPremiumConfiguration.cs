using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Premiums
{
    public class RiskPolicyPremiumConfiguration : IEntityTypeConfiguration<RiskPolicyPremium>
    {
        public void Configure(EntityTypeBuilder<RiskPolicyPremium> builder)
        {
            builder.HasKey(rvp => rvp.Id);
            builder.Property(rvp => rvp.RiskDetailId);
            builder.Property(rvp => rvp.PremiumType);
            builder.Property(rvp => rvp.Order);
            builder.Property(rvp => rvp.FromReinstatement);
            builder.HasOne(rvp => rvp.CoveragePremium)
                .WithMany(rvp => rvp.RiskPolicyPremiums)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
