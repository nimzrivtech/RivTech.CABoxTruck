using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class _20211103005107_RiskCovPremium_ProratedPremiumFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProratedPremium",
                table: "RiskManuscripts",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProratedALManuscriptPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProratedGeneralLiabilityPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProratedPDManuscriptPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProratedPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProratedPremium",
                table: "RiskManuscripts");

            migrationBuilder.DropColumn(
                name: "ProratedALManuscriptPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ProratedGeneralLiabilityPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ProratedPDManuscriptPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ProratedPremium",
                table: "RiskCoveragePremium");
        }
    }
}
