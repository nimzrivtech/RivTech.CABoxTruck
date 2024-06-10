using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehiclePremiumFieldsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VIN",
                table: "Vehicle",
                type: "varchar(17)",
                maxLength: 17,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Vehicle",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AutoLiabilityPremium",
                table: "Vehicle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoPremium",
                table: "Vehicle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PhysicalDamagePremium",
                table: "Vehicle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Radius",
                table: "Vehicle",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RadiusTerritory",
                table: "Vehicle",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPremium",
                table: "Vehicle",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoLiabilityPremium",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "CargoPremium",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PhysicalDamagePremium",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Radius",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "RadiusTerritory",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "TotalPremium",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "VIN",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(17)",
                oldMaxLength: 17,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);
        }
    }
}
