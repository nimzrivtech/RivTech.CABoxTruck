using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddCommoditiesHauledTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskSpecificCommoditiesHauled",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Commodity = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PercentageOfCarry = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificCommoditiesHauled", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskSpecificCommoditiesHauled_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskSpecificCommoditiesHauled_RiskId",
                table: "RiskSpecificCommoditiesHauled",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificCommoditiesHauled");
        }
    }
}
