using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateVehicleOptionsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Include",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Vehicle");

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "Vehicle");

            migrationBuilder.AddColumn<bool>(
                name: "Include",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "Vehicle",
                type: "int",
                nullable: true);
        }
    }
}
