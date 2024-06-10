using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class CoveragePremium_ProratedPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProratedPremium",
                table: "Vehicle");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefrigerationPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Premium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PDPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CompFTPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CollisionPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CargoPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ALPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Premium_Prorated",
                table: "RiskPolicyPremiumCoverage",
                type: "decimal(15,5)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true,
                oldDefaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProratedPremium",
                table: "Vehicle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RefrigerationPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Premium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PDPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CompFTPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CollisionPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CargoPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ALPremium_Prorated",
                table: "RiskVehiclePremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Premium_Prorated",
                table: "RiskPolicyPremiumCoverage",
                type: "decimal(12,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,5)",
                oldNullable: true,
                oldDefaultValue: 0m);
        }
    }
}
