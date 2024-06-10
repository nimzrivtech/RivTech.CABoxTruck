using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission
{
    public class RiskConfiguration : IEntityTypeConfiguration<Risk>
    {
        public void Configure(EntityTypeBuilder<Risk> builder)
        {
            builder.HasMany(x => x.RiskDetails).WithOne();
            builder.Property(e => e.QuoteNumber).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.HasOne(x => x.BrokerInfo)
                .WithOne();

            builder.Property(e => e.Id).HasValueGenerator<PrimaryIdGenerator>();
            builder.Ignore(e => e.CurrentRiskDetail);
            //builder.HasOne(x => x.CurrentRiskDetail)
            //    .WithOne(x => x.Risk)
            //    .HasForeignKey<Risk>(x => x.CurrentRiskDetailId)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);
            //builder.HasOne(x => x.ForEndorsementRiskDetail)
            //    .WithOne(x => x.Risk)
            //    .HasForeignKey<Risk>(x => x.ForEndorsementRiskDetailId)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Binding).WithOne();
            //builder.HasMany(x => x.BindingRequirements).WithOne();
            builder.Property(e => e.PolicyNumber).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.PolicySuffix).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.Property(e => e.CurrentRiskDetailId).HasColumnType("uniqueidentifier");
            builder.Property(e => e.ForEndorsementRiskDetailId).HasColumnType("uniqueidentifier");
            builder.Property(e => e.CurrentEndorsementNumber);
            builder.Property(e => e.LastReinstatedRiskDetailId);
            builder.Property(e => e.PendingCancellationNoticeDate);
            builder.Property(e => e.CancellationReasonType).HasColumnType("varchar(3)").HasMaxLength(3);
            builder.Property(e => e.IsPendingCancellation).HasDefaultValue(false);
            builder.Property(e => e.RaterVersionId);
            builder.Property(e => e.IsRaterVersionLocked);
            builder.Property(e => e.UpdateXPRaterVersion);
            builder.Property(e => e.EndorsementEffectiveDatePrompted);
            builder.Property(e => e.IsFromReset);
        }
    }
}
