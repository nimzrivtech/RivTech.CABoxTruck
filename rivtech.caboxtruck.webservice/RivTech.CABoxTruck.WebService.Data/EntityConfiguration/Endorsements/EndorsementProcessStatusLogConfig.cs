using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity.Endorsements;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Endorsements
{
    public class EndorsementProcessStatusLogConfig : IEntityTypeConfiguration<EndorsementProcessStatusLog>

    {
        public void Configure(EntityTypeBuilder<EndorsementProcessStatusLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RiskId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.DateTime).IsRequired();
        }
    }
}
