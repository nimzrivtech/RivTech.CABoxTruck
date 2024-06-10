using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateHowToReportClaimForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Form SET [Name] = 'How to Report a Claim' WHERE Id = 'HowToReportAClaim'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
