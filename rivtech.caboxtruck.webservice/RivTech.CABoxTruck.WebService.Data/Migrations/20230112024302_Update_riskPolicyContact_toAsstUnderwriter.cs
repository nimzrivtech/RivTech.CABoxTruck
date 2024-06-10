using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_riskPolicyContact_toAsstUnderwriter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_POLICYCONTACTS_DATA);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(REVERT_POLICYCONTACTS_DATA);
        }

        const string UPDATE_POLICYCONTACTS_DATA = @"
            UPDATE dbo.PolicyContacts SET Position = 'Assistant Underwriter' WHERE Position = 'Submission Specialist'
        ";

        const string REVERT_POLICYCONTACTS_DATA = @"
            UPDATE dbo.PolicyContacts SET Position = 'Submission Specialist' WHERE Position = 'Assistant Underwriter'
        ";
    }
}
