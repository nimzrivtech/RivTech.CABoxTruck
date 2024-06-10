using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class BindingConfiguration : IEntityTypeConfiguration<Binding>
    {
        public void Configure(EntityTypeBuilder<Binding> builder)
        {
            builder.HasOne(x => x.BindOption).WithMany();
            builder.HasOne(x => x.PaymentTypes).WithMany();
            builder.HasOne(x => x.CreditedOffice).WithMany();
            builder.Property(e => e.SurplusLIneNum).HasColumnType("nvarchar(100)").HasMaxLength(100);
            builder.Property(e => e.SLANum).HasColumnType("nvarchar(12)").HasMaxLength(12);
            builder.Property(e => e.MinimumEarned).HasColumnType("decimal(12, 2)");
            // builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }

    public class BindingRequirementsConfiguration : IEntityTypeConfiguration<BindingRequirements>
    {
        public void Configure(EntityTypeBuilder<BindingRequirements> builder)
        {
            builder.HasOne(x => x.BindRequirementsOption).WithMany();
            builder.HasOne(x => x.BindStatus).WithMany();

            builder.Property(e => e.Comments).HasColumnType("nvarchar(1000)").HasMaxLength(1000);
            builder.Property(e => e.RelevantDocumentDesc).HasColumnType("nvarchar(300)").HasMaxLength(300);
            // builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }

    public class QuoteConditionsConfiguration : IEntityTypeConfiguration<QuoteConditions>
    {
        public void Configure(EntityTypeBuilder<QuoteConditions> builder)
        {
            builder.HasOne(x => x.QuoteConditionsOption).WithMany();
            builder.Property(e => e.Describe).HasColumnType("nvarchar(300)").HasMaxLength(300);
            builder.Property(e => e.Comments).HasColumnType("nvarchar(1000)").HasMaxLength(1000);
            builder.Property(e => e.RelevantDocumentDesc).HasColumnType("nvarchar(300)").HasMaxLength(300);
            // builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
        }
    }
}
