using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddformsGA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"

            Insert into [Form] values('GeorgiaChanges','Georgia Changes', 51, 1, 'CA 01 09 10 13', 'GA/Georgia Changes.docx', 51, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('GeorgiaPolicyHolderNoticeRegardingUMCoverage','Georgia Policyholder Notice Regarding Uninsured Motorist Coverage', 52, 1, 'IL N 171 01 09', 'GA/Georgia Policyholder Notice Regarding Uninsured Motorist Coverage.docx', 52, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('GeorgiaUMCoverageReducedbyatFaultLiabilityLimits','Georgia Uninsured Motorist Coverage - Reduced by at-Fault Liability Limits', 53, 1, 'CA 21 11 10 13', 'GA/Georgia Uninsured Motorist Coverage Reduced by atFault Liability Limits.docx', 53, 0, 0, GETDATE(),0, 1)
            
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'GeorgiaChanges'
            Delete from [Form] where Id = 'GeorgiaPolicyHolderNoticeRegardingUMCoverage'
            Delete from [Form] where Id = 'GeorgiaUMCoverageReducedbyatFaultLiabilityLimits'
           
            GO
        ";
    }
}
