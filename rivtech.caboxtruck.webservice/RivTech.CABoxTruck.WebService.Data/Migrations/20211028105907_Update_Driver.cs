using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_Driver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Driver_RiskDetail_RiskDetailId",
                table: "Driver");

            migrationBuilder.DropIndex(
                name: "IX_Driver_RiskDetailId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "RiskDetailId",
                table: "Driver");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Driver_RiskDetail_RiskDetailId",
                table: "Driver");
        }
    }
}
