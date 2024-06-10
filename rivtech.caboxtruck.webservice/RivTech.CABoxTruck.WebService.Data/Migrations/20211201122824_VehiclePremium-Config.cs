using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehiclePremiumConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePremium_Vehicle_VehicleId",
                table: "VehiclePremium");

            migrationBuilder.DropIndex(
                name: "IX_VehiclePremium_VehicleId",
                table: "VehiclePremium");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VehiclePremium_VehicleId",
                table: "VehiclePremium",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePremium_Vehicle_VehicleId",
                table: "VehiclePremium",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
