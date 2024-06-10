using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskCoveragePremium_AddOptionIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "RiskCoveragePremium",
                type: "int",
                nullable: true);

            migrationBuilder.DropIndex(
                name: "IX_RiskCoveragePremium_RiskCoverageId",
                table: "RiskCoveragePremium");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoveragePremium_RiskCoverageId",
                table: "RiskCoveragePremium",
                columns: new string[] { "RiskCoverageId", "OptionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RiskCoveragePremium_RiskCoverageId",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "RiskCoveragePremium");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoveragePremium_RiskCoverageId",
                table: "RiskCoveragePremium",
                column: "RiskCoverageId",
                unique: true);
        }
    }
}
