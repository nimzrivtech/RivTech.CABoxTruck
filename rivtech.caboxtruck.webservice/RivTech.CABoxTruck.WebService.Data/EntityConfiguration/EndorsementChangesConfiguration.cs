using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class EndorsementChangesConfiguration : IEntityTypeConfiguration<EndorsementChanges>
    {
        public void Configure(EntityTypeBuilder<EndorsementChanges> builder)
        {
        }
    }
}