using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskCoverage_AddRatingFactorFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LiabilityExperienceRatingFactor",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LiabilityScheduleRatingFactor",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiabilityExperienceRatingFactor",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "LiabilityScheduleRatingFactor",
                table: "RiskCoverage");
        }
    }
}
