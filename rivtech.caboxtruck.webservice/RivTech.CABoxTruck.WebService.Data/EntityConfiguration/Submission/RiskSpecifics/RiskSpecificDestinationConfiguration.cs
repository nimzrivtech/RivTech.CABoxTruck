using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Submission.RiskSpecifics
{
    public class RiskSpecificDestinationConfiguration : IEntityTypeConfiguration<RiskSpecificDestination>
    {
        public void Configure(EntityTypeBuilder<RiskSpecificDestination> builder)
        {
            builder.Property(x => x.City).HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            builder.Property(x => x.State).HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            builder.Property(x => x.ZipCode).HasColumnType("varchar(10)").IsRequired();
            builder.Property(x => x.PercentageOfTravelGiven).HasColumnType("decimal(5, 2)").IsRequired();
            builder.Property(x => x.PercentageOfTravelActual).HasColumnType("decimal(5, 2)").IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
        }
    }
}
