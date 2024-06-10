using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Premiums
{
    public class RiskPolicyPremiumTransactionConfiguration : IEntityTypeConfiguration<RiskPolicyPremiumTransaction>
    {
        public void Configure(EntityTypeBuilder<RiskPolicyPremiumTransaction> builder)
        {
            builder.HasKey(rvp => rvp.Id);
            builder.Property(rvp => rvp.RiskDetailId);
            builder.Property(rvp => rvp.PremiumType);
            builder.Property(rvp => rvp.PremiumChange).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.CurrentPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.PreviousPremium).HasColumnType("decimal(12,2)");
            builder.Property(rvp => rvp.FromReinstatement);

            builder.Ignore(rvpt => rvpt.RiskPolicyPremiums);
            builder.Ignore(rvpt => rvpt.PreviousTransaction);
        }
    }
}
