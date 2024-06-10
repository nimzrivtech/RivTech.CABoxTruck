using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificUnderwritingQuestionConfiguration : IEntityTypeConfiguration<RiskSpecificUnderwritingQuestion>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificUnderwritingQuestion> builder)
        {
            builder.Property(x => x.WorkersCompensationCarrier).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.WorkersCompensationProvidedExplanation).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.EquipmentsOperatedUnderAuthorityExplanation).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.ObtainedThruAssignedRiskPlanExplanation).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.NoticeOfCancellationExplanation).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.FilingForBankruptcyExplanation).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.SuspensionExplanation).HasColumnType("varchar(1000)").HasMaxLength(1000);
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Risk)
                   .WithOne(x => x.UnderwritingQuestions)
                   .HasForeignKey<RiskSpecificUnderwritingQuestion>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
