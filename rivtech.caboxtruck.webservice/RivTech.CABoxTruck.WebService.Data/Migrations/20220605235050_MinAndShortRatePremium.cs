using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class MinAndShortRatePremium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinimumPremiumEarned",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ShortRatePremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumPremiumEarned",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ShortRatePremium",
                table: "RiskCoveragePremium");
        }
    }
}
