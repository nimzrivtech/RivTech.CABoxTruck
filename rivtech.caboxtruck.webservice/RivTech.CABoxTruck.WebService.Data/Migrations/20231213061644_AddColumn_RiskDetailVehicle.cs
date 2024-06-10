using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnRiskDetailVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountPowerUnit",
                table: "RiskDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountTrailer",
                table: "RiskDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
               name: "CountOthers",
               table: "RiskDetail",
               type: "int",
               nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "CountPowerUnit",
               table: "RiskDetail");

            migrationBuilder.DropColumn(
              name: "CountTrailer",
              table: "RiskDetail");

            migrationBuilder.DropColumn(
              name: "CountOthers",
              table: "RiskDetail");
        }
    }
}
