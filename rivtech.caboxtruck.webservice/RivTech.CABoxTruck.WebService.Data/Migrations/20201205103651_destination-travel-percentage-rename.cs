using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class destinationtravelpercentagerename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TravelPercentage",
                table: "RiskSpecificDestination",
                newName: "PercentageOfTravel");

            migrationBuilder.AlterColumn<decimal>(
                name: "ZeroToFiftyMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TwoHundredPlusPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OwnerOperatorPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FiftyOneToTwoHundredMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(3, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                table: "DriverTrainingOption",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                table: "BackgroundCheckOption",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PercentageOfTravel",
                table: "RiskSpecificDestination",
                newName: "TravelPercentage");

            migrationBuilder.AlterColumn<decimal>(
                name: "ZeroToFiftyMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TwoHundredPlusPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OwnerOperatorPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FiftyOneToTwoHundredMilesPercentage",
                table: "RiskSpecificRadiusOfOperation",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3, 2)");

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                table: "DriverTrainingOption",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                table: "BackgroundCheckOption",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short));
        }
    }
}
