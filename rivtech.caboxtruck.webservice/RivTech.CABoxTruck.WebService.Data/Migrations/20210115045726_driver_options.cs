using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class driver_options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Include",
                table: "Driver");

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "Driver");

            migrationBuilder.AddColumn<bool>(
                name: "Include",
                table: "Driver",
                type: "bit",
                nullable: true);
        }
    }
}
