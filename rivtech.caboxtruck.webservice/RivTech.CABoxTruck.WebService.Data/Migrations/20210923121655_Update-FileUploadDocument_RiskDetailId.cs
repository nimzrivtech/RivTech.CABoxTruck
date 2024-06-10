using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateFileUploadDocument_RiskDetailId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BindingRequirements_Risk_RiskId",
                table: "BindingRequirements");

            migrationBuilder.DropIndex(
                name: "IX_BindingRequirements_RiskId",
                table: "BindingRequirements");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "QuoteConditions",
                newName: "RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "BindingRequirements",
                newName: "RiskDetailId");

            migrationBuilder.AlterColumn<Guid>(
                name: "tableRefId",
                table: "FileUploadDocument",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "RiskDetailId",
                table: "FileUploadDocument",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadDocument_RiskDetailId",
                table: "FileUploadDocument",
                column: "RiskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploadDocument_RiskDetail_RiskDetailId",
                table: "FileUploadDocument",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploadDocument_RiskDetail_RiskDetailId",
                table: "FileUploadDocument");

            migrationBuilder.DropIndex(
                name: "IX_FileUploadDocument_RiskDetailId",
                table: "FileUploadDocument");

            migrationBuilder.DropColumn(
                name: "RiskDetailId",
                table: "FileUploadDocument");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "QuoteConditions",
                newName: "RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "BindingRequirements",
                newName: "RiskId");

            migrationBuilder.AlterColumn<Guid>(
                name: "tableRefId",
                table: "FileUploadDocument",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BindingRequirements_RiskId",
                table: "BindingRequirements",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_BindingRequirements_Risk_RiskId",
                table: "BindingRequirements",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
