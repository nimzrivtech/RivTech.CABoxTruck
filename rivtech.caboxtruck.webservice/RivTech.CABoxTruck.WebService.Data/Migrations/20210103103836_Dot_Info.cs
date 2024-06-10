using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Dot_Info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChameleonIssuesOptions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChameleonIssuesOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaferRatingOptions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaferRatingOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrafficLightRatingOptions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficLightRatingOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskSpecificDotInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoesApplicantRequireDotNumber = table.Column<bool>(type: "bit", nullable: false),
                    DoesApplicantPlanToStayInterstate = table.Column<bool>(type: "bit", nullable: false),
                    DoesApplicantHaveInsterstateAuthority = table.Column<bool>(type: "bit", nullable: false),
                    DotRegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDotCurrentlyActive = table.Column<bool>(type: "bit", nullable: false),
                    ChameleonIssuesId = table.Column<short>(type: "smallint", nullable: true),
                    CurrentSaferRatingId = table.Column<short>(type: "smallint", nullable: true),
                    IssCabRatingId = table.Column<short>(type: "smallint", nullable: true),
                    IsThereAnyPolicyLevelAccidents = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfPolicyLevelAccidents = table.Column<int>(type: "int", nullable: false),
                    IsUnsafeDrivingChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsHoursOfServiceChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsDriverFitnessChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsControlledSubstanceChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsVehicleMaintenanceChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsCrashChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificDotInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskSpecificDotInfo_ChameleonIssuesOptions_ChameleonIssuesId",
                        column: x => x.ChameleonIssuesId,
                        principalTable: "ChameleonIssuesOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskSpecificDotInfo_Risk_Id",
                        column: x => x.Id,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskSpecificDotInfo_SaferRatingOptions_CurrentSaferRatingId",
                        column: x => x.CurrentSaferRatingId,
                        principalTable: "SaferRatingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskSpecificDotInfo_TrafficLightRatingOptions_IssCabRatingId",
                        column: x => x.IssCabRatingId,
                        principalTable: "TrafficLightRatingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskSpecificDotInfo_ChameleonIssuesId",
                table: "RiskSpecificDotInfo",
                column: "ChameleonIssuesId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskSpecificDotInfo_CurrentSaferRatingId",
                table: "RiskSpecificDotInfo",
                column: "CurrentSaferRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskSpecificDotInfo_IssCabRatingId",
                table: "RiskSpecificDotInfo",
                column: "IssCabRatingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificDotInfo");

            migrationBuilder.DropTable(
                name: "ChameleonIssuesOptions");

            migrationBuilder.DropTable(
                name: "SaferRatingOptions");

            migrationBuilder.DropTable(
                name: "TrafficLightRatingOptions");
        }
    }
}
