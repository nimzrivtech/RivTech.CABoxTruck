using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class risktoradiusofoperationsrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificRadiusOfOperation_Risk_Id",
                table: "RiskSpecificRadiusOfOperation",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificRadiusOfOperation_Risk_Id",
                table: "RiskSpecificRadiusOfOperation");
        }
    }
}
