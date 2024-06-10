using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskPolicyPremium_Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId",
                table: "RiskPolicyPremium");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId",
                table: "RiskVehiclePremium");

            migrationBuilder.DropColumn(
                name: "PremiumType",
                table: "RiskPolicyPremiumCoverage");

            migrationBuilder.AlterColumn<int>(
                name: "CoveragePremiumId",
                table: "RiskVehiclePremium",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId",
                table: "RiskPolicyPremium",
                column: "CoveragePremiumId",
                principalTable: "RiskPolicyPremiumCoverage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId",
                table: "RiskVehiclePremium",
                column: "CoveragePremiumId",
                principalTable: "RiskVehiclePremiumCoverage",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId",
                table: "RiskPolicyPremium");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId",
                table: "RiskVehiclePremium");

            migrationBuilder.AlterColumn<int>(
                name: "CoveragePremiumId",
                table: "RiskVehiclePremium",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PremiumType",
                table: "RiskPolicyPremiumCoverage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId",
                table: "RiskPolicyPremium",
                column: "CoveragePremiumId",
                principalTable: "RiskPolicyPremiumCoverage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId",
                table: "RiskVehiclePremium",
                column: "CoveragePremiumId",
                principalTable: "RiskVehiclePremiumCoverage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
