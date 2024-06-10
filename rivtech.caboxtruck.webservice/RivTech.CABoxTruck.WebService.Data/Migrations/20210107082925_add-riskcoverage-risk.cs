using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class addriskcoveragerisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RiskCoverage_RiskId",
                table: "RiskCoverage");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoverage_RiskId",
                table: "RiskCoverage",
                column: "RiskId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RiskCoverage_RiskId",
                table: "RiskCoverage");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoverage_RiskId",
                table: "RiskCoverage",
                column: "RiskId");
        }
    }
}
