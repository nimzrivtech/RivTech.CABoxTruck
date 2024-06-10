using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateRiskDestinationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "RiskSpecificDestination");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "RiskSpecificDestination");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "RiskSpecificDestination",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "RiskSpecificDestination");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "RiskSpecificDestination",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "RiskSpecificDestination",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");
        }
    }
}
