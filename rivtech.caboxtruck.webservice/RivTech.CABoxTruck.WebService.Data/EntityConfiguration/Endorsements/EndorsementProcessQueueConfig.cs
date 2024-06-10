using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration.Endorsements
{
    public class EndorsementProcessQueueConfig : IEntityTypeConfiguration<EndorsementProcessQueue>
    {
        public void Configure(EntityTypeBuilder<EndorsementProcessQueue> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.RiskId).IsRequired();
            builder.Property(q => q.QueueTime).IsRequired();
            builder.Property(q => q.StartTime);
            builder.Property(q => q.EndTime);
            builder.Property(q => q.IsArchived);
            builder.Property(q => q.IsError);
            builder.Property(q => q.ProcessType);
            builder.Property(q => q.RiskDetailId);
            builder.Property(q => q.EffectiveDate);
            builder.Property(q => q.EndorsementText);
            builder.Property(q => q.PolicyChanges);
        }
    }
}
