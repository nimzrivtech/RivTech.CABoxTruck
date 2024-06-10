using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class IgnoreCurrentRiskDetailId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risk_RiskDetail_CurrentRiskDetailId",
                table: "Risk");

            migrationBuilder.DropIndex(
                name: "IX_Risk_CurrentRiskDetailId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CurrentRiskDetailId",
                table: "Risk");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRiskDetailId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risk_CurrentRiskDetailId",
                table: "Risk",
                column: "CurrentRiskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Risk_RiskDetail_CurrentRiskDetailId",
                table: "Risk",
                column: "CurrentRiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
