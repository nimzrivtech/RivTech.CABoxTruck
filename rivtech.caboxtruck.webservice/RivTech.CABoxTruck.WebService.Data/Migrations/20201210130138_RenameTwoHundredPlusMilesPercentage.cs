using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RenameTwoHundredPlusMilesPercentage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwoHundredPlusPercentage",
                table: "RiskSpecificRadiusOfOperation",
                newName: "TwoHundredPlusMilesPercentage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwoHundredPlusMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                newName: "TwoHundredPlusPercentage");
        }
    }
}
