using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class applicant_entity_updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessPrincipal",
                table: "Entity",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMainGarage",
                table: "Address",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Address",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessPrincipal",
                table: "Entity");

            migrationBuilder.DropColumn(
                name: "IsMainGarage",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Address");
        }
    }
}
