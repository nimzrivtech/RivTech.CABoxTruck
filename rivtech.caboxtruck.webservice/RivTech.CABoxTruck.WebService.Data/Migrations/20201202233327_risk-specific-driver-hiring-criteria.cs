using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class riskspecificdriverhiringcriteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskSpecificDriverHiringCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = table.Column<bool>(nullable: false),
                    DoDriversHave5YearsDrivingExperience = table.Column<bool>(nullable: false),
                    IsAgreedToReportAllDriversToRivington = table.Column<bool>(nullable: false),
                    AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = table.Column<bool>(nullable: false),
                    AreDriversProperlyLicensedDOTcompliant = table.Column<bool>(nullable: false),
                    IsDisciplinaryPlanDocumented = table.Column<bool>(nullable: false),
                    IsThereDriverIncentiveProgram = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificDriverHiringCriteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundCheckOption",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false, defaultValue: (short)1),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RiskSpecificDriverHiringCriteriaId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundCheckOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackgroundCheckOption_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                        column: x => x.RiskSpecificDriverHiringCriteriaId,
                        principalTable: "RiskSpecificDriverHiringCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverTrainingOption",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false, defaultValue: (short)1),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RiskSpecificDriverHiringCriteriaId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTrainingOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverTrainingOption_RiskSpecificDriverHiringCriteria_RiskSpecificDriverHiringCriteriaId",
                        column: x => x.RiskSpecificDriverHiringCriteriaId,
                        principalTable: "RiskSpecificDriverHiringCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundCheckOption_RiskSpecificDriverHiringCriteriaId",
                table: "BackgroundCheckOption",
                column: "RiskSpecificDriverHiringCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverTrainingOption_RiskSpecificDriverHiringCriteriaId",
                table: "DriverTrainingOption",
                column: "RiskSpecificDriverHiringCriteriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundCheckOption");

            migrationBuilder.DropTable(
                name: "DriverTrainingOption");

            migrationBuilder.DropTable(
                name: "RiskSpecificDriverHiringCriteria");
        }
    }
}
