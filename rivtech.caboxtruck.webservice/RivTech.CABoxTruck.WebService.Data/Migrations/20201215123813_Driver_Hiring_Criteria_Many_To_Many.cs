using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Driver_Hiring_Criteria_Many_To_Many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackgroundCheckOptionRiskSpecificDriverHiringCriteria",
                columns: table => new
                {
                    BackGroundCheckIncludesId = table.Column<short>(type: "smallint", nullable: false),
                    RiskSpecificDriverHiringCriteriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundCheckOptionRiskSpecificDriverHiringCriteria", x => new { x.BackGroundCheckIncludesId, x.RiskSpecificDriverHiringCriteriaId });
                    table.ForeignKey(
                        name: "FK_BackgroundCheckOptionRiskSpecificDriverHiringCriteria_BackgroundCheckOptions_BackGroundCheckIncludesId",
                        column: x => x.BackGroundCheckIncludesId,
                        principalTable: "BackgroundCheckOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackgroundCheckOptionRiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                        column: x => x.RiskSpecificDriverHiringCriteriaId,
                        principalTable: "RiskSpecificDriverHiringCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverTrainingOptionRiskSpecificDriverHiringCriteria",
                columns: table => new
                {
                    DriverTrainingIncludesId = table.Column<short>(type: "smallint", nullable: false),
                    RiskSpecificDriverHiringCriteriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTrainingOptionRiskSpecificDriverHiringCriteria", x => new { x.DriverTrainingIncludesId, x.RiskSpecificDriverHiringCriteriaId });
                    table.ForeignKey(
                        name: "FK_DriverTrainingOptionRiskSpecificDriverHiringCriteria_DriverTrainingOptions_DriverTrainingIncludesId",
                        column: x => x.DriverTrainingIncludesId,
                        principalTable: "DriverTrainingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverTrainingOptionRiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                        column: x => x.RiskSpecificDriverHiringCriteriaId,
                        principalTable: "RiskSpecificDriverHiringCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundCheckOptionRiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOptionRiskSpecificDriverHiringCriteria",
                column: "RiskSpecificDriverHiringCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverTrainingOptionRiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOptionRiskSpecificDriverHiringCriteria",
                column: "RiskSpecificDriverHiringCriteriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundCheckOptionRiskSpecificDriverHiringCriteria");

            migrationBuilder.DropTable(
                name: "DriverTrainingOptionRiskSpecificDriverHiringCriteria");
        }
    }
}
