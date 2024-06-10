using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Alter_RiskHistory_NumberOfPowerUnits_AL_And_PD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "NumberOfPowerUnitsAPD",
                table: "RiskHistory",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NumberOfPowerUnitsAL",
                table: "RiskHistory",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfPowerUnitsAPD",
                table: "RiskHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfPowerUnitsAL",
                table: "RiskHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);
        }
    }
}
