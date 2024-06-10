using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRiskCoveragePremiumSP33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TrailerInterchangeAL",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NonOwnedTrailerPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProratedNonOwnedTrailerPremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProratedTrailerInterchangePremiumAL",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProratedNonOwnedTrailerPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ProratedTrailerInterchangePremiumAL",
                table: "RiskCoveragePremium");

            migrationBuilder.AlterColumn<decimal>(
                name: "TrailerInterchangeAL",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NonOwnedTrailerPremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);
        }
    }
}
