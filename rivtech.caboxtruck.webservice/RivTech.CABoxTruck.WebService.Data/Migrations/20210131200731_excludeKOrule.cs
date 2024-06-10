using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class excludeKOrule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isExcludeKO",
                table: "Driver",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "koReason",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isExcludeKO",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "koReason",
                table: "Driver");
        }
    }
}
