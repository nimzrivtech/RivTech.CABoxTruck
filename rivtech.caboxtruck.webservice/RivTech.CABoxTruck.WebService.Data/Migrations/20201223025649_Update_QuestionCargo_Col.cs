using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_QuestionCargo_Col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_LvRiskResponseType_RiskResponseTypeId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "LvRiskResponseType");

            migrationBuilder.DropIndex(
                name: "IX_Question_RiskResponseTypeId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "RiskResponseTypeId",
                table: "Question");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RiskResponse");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskResponse");

            migrationBuilder.AddColumn<short>(
                name: "RiskResponseTypeId",
                table: "Question",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "LvRiskResponseType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvRiskResponseType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_RiskResponseTypeId",
                table: "Question",
                column: "RiskResponseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_LvRiskResponseType_RiskResponseTypeId",
                table: "Question",
                column: "RiskResponseTypeId",
                principalTable: "LvRiskResponseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
