using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddRiskFilingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskFiling",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: false),
                    FilingTypeId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFiling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskFiling_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskFiling_RiskId",
                table: "RiskFiling",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskFiling");
        }
    }
}
