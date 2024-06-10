using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class linkrisktodriverinfoanddriverhiringcriteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverHiringCriteria_Risk_Id",
                table: "RiskSpecificDriverHiringCriteria",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverInfo_Risk_Id",
                table: "RiskSpecificDriverInfo",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverHiringCriteria_Risk_Id",
                table: "RiskSpecificDriverHiringCriteria");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverInfo_Risk_Id",
                table: "RiskSpecificDriverInfo");
        }
    }
}
