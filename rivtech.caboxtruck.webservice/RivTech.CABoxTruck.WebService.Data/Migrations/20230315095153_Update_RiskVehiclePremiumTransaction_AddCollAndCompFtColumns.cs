using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRiskVehiclePremiumTransactionAddCollAndCompFtColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PremiumChangeColl",
                table: "RiskVehiclePremiumTransaction",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PremiumChangeCompFt",
                table: "RiskVehiclePremiumTransaction",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumChangeColl",
                table: "RiskVehiclePremiumTransaction");

            migrationBuilder.DropColumn(
                name: "PremiumChangeCompFt",
                table: "RiskVehiclePremiumTransaction");
        }
    }
}
