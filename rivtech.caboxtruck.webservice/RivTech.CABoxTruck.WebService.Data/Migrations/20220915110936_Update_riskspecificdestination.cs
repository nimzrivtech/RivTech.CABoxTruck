using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_riskspecificdestination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PercentageOfTravel",
                table: "RiskSpecificDestination",
                newName: "PercentageOfTravelGiven");

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageOfTravelActual",
                table: "RiskSpecificDestination",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageOfTravelActual",
                table: "RiskSpecificDestination");

            migrationBuilder.RenameColumn(
                name: "PercentageOfTravelGiven",
                table: "RiskSpecificDestination",
                newName: "PercentageOfTravel");
        }
    }
}
