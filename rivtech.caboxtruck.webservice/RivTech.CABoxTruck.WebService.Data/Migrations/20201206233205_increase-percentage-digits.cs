using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class increasepercentagedigits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ZeroToFiftyMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TwoHundredPlusPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OwnerOperatorPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FiftyOneToTwoHundredMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageOfStaff",
                table: "RiskSpecificDriverInfo",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageOfTravel",
                table: "RiskSpecificDestination",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ZeroToFiftyMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TwoHundredPlusPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OwnerOperatorPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FiftyOneToTwoHundredMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageOfStaff",
                table: "RiskSpecificDriverInfo",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageOfTravel",
                table: "RiskSpecificDestination",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");
        }
    }
}
