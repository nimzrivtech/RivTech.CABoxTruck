using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using System;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Premiums
{
    public class RiskPolicyCoveragePremiumConfiguration : IEntityTypeConfiguration<PolicyCoveragePremium>
    {
        public void Configure(EntityTypeBuilder<PolicyCoveragePremium> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("RiskPolicyPremiumCoverage");

            var coveragePeriod = builder.OwnsOne(x => x.CoveragePeriod);
            coveragePeriod.Property(x => x.Start).HasColumnName("EffectiveDate").HasColumnType("date").IsRequired();
            coveragePeriod.Property(x => x.End).HasColumnName("ExpirationDate").HasColumnType("date").IsRequired();

            var premium = builder.OwnsOne(c => c.Premium);
            premium.Property(x => x.Annual).HasColumnType("decimal(12,2)").IsRequired().HasDefaultValue(0);
            premium.Property(x => x.Prorated).HasColumnType("decimal(15,5)").IsRequired().HasDefaultValue(0);//.HasConversion(x => Truncate(x), y => y);
        }

        //private decimal Truncate(decimal num)
        //{
        //    var converted = Math.Truncate(100 * num) / 100; ;
        //    Console.Write($"Truncating {num} -> {converted}..");
        //    return converted;
        //}
    }
}
