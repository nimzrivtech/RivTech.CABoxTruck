using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class LvReasonMovefiguration : IEntityTypeConfiguration<LvReasonMove>
    {
        public void Configure(EntityTypeBuilder<LvReasonMove> builder)
        {
        }
    }
}

//const string INSERT_REASONMOVELV_DATA = @"
//            insert dbo.LvReasonMove ([Description], IsActive)
//            values ('Required Coverage Not Available', 1),
//            ('Change in Exposure (routes, vehicle types)', 1),
//            ('Driver Not approved', 1),
//            ('State and/or DOT filings not available', 1),
//            ('Pricing', 1),
//            ('Non-Pay Cancellation; reinstatement not available', 1);
//        ";


//migrationBuilder.Sql(INSERT_REASONMOVELV_DATA);