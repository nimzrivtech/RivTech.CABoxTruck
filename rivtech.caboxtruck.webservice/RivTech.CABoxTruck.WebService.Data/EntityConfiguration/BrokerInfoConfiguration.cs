using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class BrokerInfoConfiguration : IEntityTypeConfiguration<BrokerInfo>
    {
        public void Configure(EntityTypeBuilder<BrokerInfo> builder)
        {
            //builder.HasOne(x => x.Agent).WithMany();
            //builder.HasOne(x => x.SubAgent).WithMany();//.HasForeignKey(b => b.SubAgencyId);

            //builder.HasOne(x => x.Agency).WithMany();//.HasForeignKey(b => b.AgencyId);
            //builder.HasOne(x => x.SubAgency).WithMany();//.HasForeignKey(b => b.SubAgencyId);
        }
    }
}
