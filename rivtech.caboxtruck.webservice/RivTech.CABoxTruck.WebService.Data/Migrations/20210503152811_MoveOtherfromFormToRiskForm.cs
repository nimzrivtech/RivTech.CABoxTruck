using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class MoveOtherfromFormToRiskForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskDetail_Risk_RiskId",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Form");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelected",
                table: "RiskForm",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "RiskForm",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "RiskForm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RiskId",
                table: "RiskDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskDetail_Risk_RiskId",
                table: "RiskDetail",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskDetail_Risk_RiskId",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "RiskForm");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelected",
                table: "RiskForm",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "RiskForm",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RiskId",
                table: "RiskDetail",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "Form",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskDetail_Risk_RiskId",
                table: "RiskDetail",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
