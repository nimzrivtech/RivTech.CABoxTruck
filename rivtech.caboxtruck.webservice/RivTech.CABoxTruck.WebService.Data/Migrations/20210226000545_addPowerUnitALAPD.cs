using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class addPowerUnitALAPD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfPowerUnitsAL",
                table: "RiskHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPowerUnitsAPD",
                table: "RiskHistory",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPowerUnitsAL",
                table: "RiskHistory");

            migrationBuilder.DropColumn(
                name: "NumberOfPowerUnitsAPD",
                table: "RiskHistory");
        }
    }
}
