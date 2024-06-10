using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddRiskAdditionalInterestNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryNonContributory",
                table: "RiskAdditionalInterest",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWaiverOfSubrogation",
                table: "RiskAdditionalInterest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimaryNonContributory",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropColumn(
                name: "IsWaiverOfSubrogation",
                table: "RiskAdditionalInterest");
        }
    }
}
