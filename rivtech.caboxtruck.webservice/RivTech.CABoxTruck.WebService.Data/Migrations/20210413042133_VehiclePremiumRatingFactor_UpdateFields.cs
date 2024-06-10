using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehiclePremiumRatingFactor_UpdateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VIN",
                table: "VehiclePremiumRatingFactor",
                newName: "VehicleId");

            migrationBuilder.AddColumn<decimal>(
                name: "PerVehiclePremium",
                table: "VehiclePremiumRatingFactor",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Radius",
                table: "VehiclePremiumRatingFactor",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RadiusTerritory",
                table: "VehiclePremiumRatingFactor",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerVehiclePremium",
                table: "VehiclePremiumRatingFactor");

            migrationBuilder.DropColumn(
                name: "Radius",
                table: "VehiclePremiumRatingFactor");

            migrationBuilder.DropColumn(
                name: "RadiusTerritory",
                table: "VehiclePremiumRatingFactor");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "VehiclePremiumRatingFactor",
                newName: "VIN");
        }
    }
}
