using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Underwriting_Questions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskSpecificUnderwritingQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreWorkersCompensationProvided = table.Column<bool>(type: "bit", nullable: false),
                    WorkersCompensationCarrier = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    AreAllEquipmentOperatedUnderApplicantsAuthority = table.Column<bool>(type: "bit", nullable: false),
                    HasInsuranceBeenObtainedThruAssignedRiskPlan = table.Column<bool>(type: "bit", nullable: false),
                    ObtainedThruAssignedRiskPlanExplanation = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    HasAnyCompanyProvidedNoticeOfCancellation = table.Column<bool>(type: "bit", nullable: false),
                    NoticeOfCancellationExplanation = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    HasFiledForBankruptcy = table.Column<bool>(type: "bit", nullable: false),
                    FilingForBankruptcyExplanation = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    HasOperatingAuthoritySuspended = table.Column<bool>(type: "bit", nullable: false),
                    SuspensionExplanation = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    HasVehicleCountBeenAffectedByCovid19 = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfVehiclesRunningDuringCovid19 = table.Column<int>(type: "int", nullable: false),
                    AreAnswersConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificUnderwritingQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskSpecificUnderwritingQuestion_Risk_Id",
                        column: x => x.Id,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificUnderwritingQuestion");
        }
    }
}
