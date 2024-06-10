using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class addCreatedDateSubmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskAdditionalInterest_Risk_RiskId",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RiskResponse");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskResponse");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_RiskResponse_RiskId",
                table: "RiskResponse",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAdditionalInterest_Risk_RiskId",
                table: "RiskAdditionalInterest",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskResponse_Risk_RiskId",
                table: "RiskResponse",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskAdditionalInterest_Risk_RiskId",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskResponse_Risk_RiskId",
                table: "RiskResponse");

            migrationBuilder.DropIndex(
                name: "IX_RiskResponse_RiskId",
                table: "RiskResponse");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Risk");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RiskResponse",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskResponse",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAdditionalInterest_Risk_RiskId",
                table: "RiskAdditionalInterest",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id");
        }
    }
}
