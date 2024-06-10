using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_RiskDetailDriver_cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskDetailDriver_Driver_DriverId",
                table: "RiskDetailDriver");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskDetailDriver_Driver_DriverId",
                table: "RiskDetailDriver",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
