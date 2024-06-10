using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class NEWCABT463_UpdateAmountFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StatedAmount",
                table: "Vehicle",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<string>(
                name: "UnitDescription",
                table: "RiskSpecificSafetyDevice",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualIncomeDerivedFromLeaseWithoutDriver",
                table: "RiskSpecificDriverInfo",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualIncomeDerivedFromLease",
                table: "RiskSpecificDriverInfo",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualCostOfHireWithoutDriver",
                table: "RiskSpecificDriverInfo",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualCostOfHire",
                table: "RiskSpecificDriverInfo",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NumberOfInstallments",
                table: "RiskCoverage",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StatedAmount",
                table: "Vehicle",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<string>(
                name: "UnitDescription",
                table: "RiskSpecificSafetyDevice",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualIncomeDerivedFromLeaseWithoutDriver",
                table: "RiskSpecificDriverInfo",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualIncomeDerivedFromLease",
                table: "RiskSpecificDriverInfo",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualCostOfHireWithoutDriver",
                table: "RiskSpecificDriverInfo",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnnualCostOfHire",
                table: "RiskSpecificDriverInfo",
                type: "decimal(11,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");
        }
    }
}
