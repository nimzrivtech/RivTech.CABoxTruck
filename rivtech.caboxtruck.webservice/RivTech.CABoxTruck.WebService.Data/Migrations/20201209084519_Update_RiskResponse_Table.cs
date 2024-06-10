using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_RiskResponse_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Question_ParentQuestionId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskResponse_Risk_RiskId",
                table: "RiskResponse");

            migrationBuilder.DropIndex(
                name: "IX_RiskResponse_RiskId",
                table: "RiskResponse");

            migrationBuilder.DropIndex(
                name: "IX_Question_ParentQuestionId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ParentQuestionId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Question");

            migrationBuilder.AlterColumn<Guid>(
                name: "RiskId",
                table: "RiskResponse",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_RiskResponse",
                table: "RiskResponse");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RiskResponse");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RiskResponse",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RiskResponse",
                table: "RiskResponse",
                column: "Id");

            migrationBuilder.AddColumn<short>(
                name: "RiskResponseTypeId",
                table: "RiskResponse",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_RiskResponse_RiskResponseTypeId",
                table: "RiskResponse",
                column: "RiskResponseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskResponse_LvRiskResponseType_RiskResponseTypeId",
                table: "RiskResponse",
                column: "RiskResponseTypeId",
                principalTable: "LvRiskResponseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskResponse_LvRiskResponseType_RiskResponseTypeId",
                table: "RiskResponse");

            migrationBuilder.DropIndex(
                name: "IX_RiskResponse_RiskResponseTypeId",
                table: "RiskResponse");

            migrationBuilder.DropColumn(
                name: "RiskResponseTypeId",
                table: "RiskResponse");

            migrationBuilder.AlterColumn<Guid>(
                name: "RiskId",
                table: "RiskResponse",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                table: "RiskResponse",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<short>(
                name: "Order",
                table: "Question",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "ParentQuestionId",
                table: "Question",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ProgramId",
                table: "Question",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "StateId",
                table: "Question",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskResponse_RiskId",
                table: "RiskResponse",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ParentQuestionId",
                table: "Question",
                column: "ParentQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_ParentQuestionId",
                table: "Question",
                column: "ParentQuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskResponse_Risk_RiskId",
                table: "RiskResponse",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
