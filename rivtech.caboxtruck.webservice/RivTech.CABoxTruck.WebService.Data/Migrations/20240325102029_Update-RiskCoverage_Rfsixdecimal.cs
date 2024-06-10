using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRiskCoverageRfsixdecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "LiabilityScheduleRatingFactor",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LiabilityExperienceRatingFactor",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GlScheduleRF",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GlExperienceRF",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CargoScheduleRF",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CargoExperienceRF",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApdScheduleRF",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApdExperienceRF",
                table: "RiskCoverage",
                type: "decimal(12,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "LiabilityScheduleRatingFactor",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LiabilityExperienceRatingFactor",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GlScheduleRF",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GlExperienceRF",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CargoScheduleRF",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CargoExperienceRF",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApdScheduleRF",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApdExperienceRF",
                table: "RiskCoverage",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldNullable: true);
        }
    }
}
