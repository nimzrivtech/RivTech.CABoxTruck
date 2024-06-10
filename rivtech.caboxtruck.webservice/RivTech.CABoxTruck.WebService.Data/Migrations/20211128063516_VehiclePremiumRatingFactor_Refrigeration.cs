using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehiclePremiumRatingFactor_Refrigeration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RefrigerationPremium",
                table: "VehiclePremiumRatingFactor",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VehiclePremiumRatingFactorId",
                table: "Vehicle",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePremium_VehicleId",
                table: "VehiclePremium",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_VehiclePremiumRatingFactorId",
                table: "Vehicle",
                column: "VehiclePremiumRatingFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehiclePremiumRatingFactor_VehiclePremiumRatingFactorId",
                table: "Vehicle",
                column: "VehiclePremiumRatingFactorId",
                principalTable: "VehiclePremiumRatingFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePremium_Vehicle_VehicleId",
                table: "VehiclePremium",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehiclePremiumRatingFactor_VehiclePremiumRatingFactorId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePremium_Vehicle_VehicleId",
                table: "VehiclePremium");

            migrationBuilder.DropIndex(
                name: "IX_VehiclePremium_VehicleId",
                table: "VehiclePremium");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_VehiclePremiumRatingFactorId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "RefrigerationPremium",
                table: "VehiclePremiumRatingFactor");

            migrationBuilder.DropColumn(
                name: "VehiclePremiumRatingFactorId",
                table: "Vehicle");
        }
    }
}
