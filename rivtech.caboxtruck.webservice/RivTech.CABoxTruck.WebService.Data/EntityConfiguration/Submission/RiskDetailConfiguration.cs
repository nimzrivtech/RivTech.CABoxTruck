using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskDetailConfiguration : IEntityTypeConfiguration<RiskDetail>
    {
        public void Configure(EntityTypeBuilder<RiskDetail> builder)
        {
            builder.Property(e => e.SubmissionNumber).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.Property(e => e.QuoteNumber).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.Property(e => e.Term).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.PreviousExperienceCompany).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.AgentCommission).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.DOTNumberLastFiveYearsDetail).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.PastOperatingAuthorityStates).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.CurrentOperatingAuthorityStates).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.DriverMaxHoursDayDriving).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.DriverMaxHoursDayOnDuty).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.DriverMaxHoursWeekDriving).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.DriverMaxHoursWeekOnDuty).HasColumnType("decimal(6, 2)");
            builder.Property(e => e.HoursOfService).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Status).HasColumnType("varchar(20)").HasMaxLength(100);
            builder.Property(e => e.FromReinstatement);

            builder.HasOne(e => e.Insured).WithMany()
                    .HasForeignKey(e => e.InsuredId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.AdditionalInterests).WithOne();
            builder.HasOne(x => x.DriverHeader).WithOne();
            builder.HasMany(x => x.RiskCoverages).WithOne();
            builder.HasMany(x => x.RiskResponses).WithOne();

            builder.Ignore(x => x.Vehicles);
            builder.Ignore(x => x.Drivers);
        }
    }
}
