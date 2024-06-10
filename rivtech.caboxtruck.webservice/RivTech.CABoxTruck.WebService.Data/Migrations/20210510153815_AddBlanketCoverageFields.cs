using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddBlanketCoverageFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdditionalInsured",
                table: "RiskDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryNonContributory",
                table: "RiskDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWaiverOfSubrogation",
                table: "RiskDetail",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdditionalInsured",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "IsPrimaryNonContributory",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "IsWaiverOfSubrogation",
                table: "RiskDetail");
        }
    }
}
