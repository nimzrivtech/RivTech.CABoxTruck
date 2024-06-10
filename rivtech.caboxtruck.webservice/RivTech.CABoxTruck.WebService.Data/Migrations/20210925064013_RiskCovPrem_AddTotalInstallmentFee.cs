using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskCovPrem_AddTotalInstallmentFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalInstallmentFee",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalInstallmentFee",
                table: "RiskCoveragePremium");
        }
    }
}
