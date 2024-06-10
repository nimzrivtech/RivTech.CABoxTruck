using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class riskcoverage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoveragePlan",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProgramId = table.Column<int>(nullable: false),
                    StateId = table.Column<short>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoveragePlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoveredSymbol",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProgramId = table.Column<int>(nullable: false),
                    StateId = table.Column<short>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoveredSymbol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskCoverage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: false),
                    CoveragePlanId = table.Column<short>(nullable: true),
                    AutoBILimitId = table.Column<short>(nullable: true),
                    AutoPDLimitId = table.Column<short>(nullable: true),
                    LiabilityDeductibleId = table.Column<short>(nullable: true),
                    IsPIPRequested = table.Column<bool>(nullable: false),
                    MedPayLimitId = table.Column<short>(nullable: true),
                    PIPLimitId = table.Column<short>(nullable: true),
                    IsGuestPIP = table.Column<bool>(nullable: false),
                    UMBILimitId = table.Column<short>(nullable: true),
                    UMPDLimitId = table.Column<short>(nullable: true),
                    UIMBILimitId = table.Column<short>(nullable: true),
                    UIMPDLimitId = table.Column<short>(nullable: true),
                    IsUMStacking = table.Column<bool>(nullable: false),
                    CoveredSymbolsId = table.Column<short>(nullable: true),
                    IsHiredAutoLiability = table.Column<bool>(nullable: false),
                    IsNonOwnedLiability = table.Column<bool>(nullable: false),
                    FireTheftDeductibleId = table.Column<short>(nullable: true),
                    CompDeductibleId = table.Column<short>(nullable: true),
                    CollDeductibleId = table.Column<short>(nullable: true),
                    NonTruckingLiabilityLimitId = table.Column<short>(nullable: true),
                    GaragekeepersLegalLiabilityLimitId = table.Column<short>(nullable: true),
                    GaragekeepersDirectPrimaryLimitId = table.Column<short>(nullable: true),
                    GaragekeepersDirectExcessLimitId = table.Column<short>(nullable: true),
                    OnHookLegalLiabilityLimitId = table.Column<short>(nullable: true),
                    OnHookDirectPrimaryLimitId = table.Column<short>(nullable: true),
                    OnHookDirectExcessLimitId = table.Column<short>(nullable: true),
                    TrailerInterchangeLimitId = table.Column<short>(nullable: true),
                    IsDriveOtherCar = table.Column<bool>(nullable: false),
                    RentalReimbursementLimitId = table.Column<short>(nullable: true),
                    IsRoadsideAssistance = table.Column<bool>(nullable: false),
                    RentalReimbursementDPLimitId = table.Column<short>(nullable: true),
                    GLBILimitId = table.Column<short>(nullable: true),
                    GLPDLimitId = table.Column<short>(nullable: true),
                    GLCompletedOperationsLimitId = table.Column<short>(nullable: true),
                    GLPersonalAdvertisingInjuryLimitId = table.Column<short>(nullable: true),
                    GLDamageToRentalPremisesLimitid = table.Column<short>(nullable: true),
                    GLMedicalExpenseLimitId = table.Column<short>(nullable: true),
                    GLIsOperationAtAStorageImpound = table.Column<bool>(nullable: false),
                    GLPayrollAmount = table.Column<int>(nullable: true),
                    GLClaimsLastFiveYears = table.Column<int>(nullable: true),
                    CargoLimitId = table.Column<short>(nullable: true),
                    RefCargoLimitId = table.Column<short>(nullable: true),
                    CargoClaimsLastFiveYears = table.Column<int>(nullable: true),
                    AccidentalDeathLimitId = table.Column<int>(nullable: true),
                    EssentialServicesLimitId = table.Column<int>(nullable: true),
                    FuneralBenefitsLimitId = table.Column<int>(nullable: true),
                    IncomeLossLimitId = table.Column<int>(nullable: true),
                    EndorsementNumber = table.Column<int>(nullable: true),
                    OptionNumber = table.Column<short>(nullable: true),
                    IsExcludeDriversUnder25WithActivity = table.Column<bool>(nullable: false),
                    IsExcludeDriversWithAFMovingViolations = table.Column<bool>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<long>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCoverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskCoverage_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: false),
                    HistoryYear = table.Column<int>(nullable: true),
                    PolicyTermStartDate = table.Column<DateTime>(nullable: true),
                    PolicyTermEndDate = table.Column<DateTime>(nullable: true),
                    InsuranceCarrierOrBroker = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LiabilityLimits = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LiabilityDeductible = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CollisionDeductible = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ComprehensiveDeductible = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CargoLimits = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    RefrigeratedCargoLimits = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    AutoLiabilityPremiumPerVehicle = table.Column<int>(nullable: true),
                    PhysicalDamagePremiumPerVehicle = table.Column<int>(nullable: true),
                    TotalPremiumPerVehicle = table.Column<int>(nullable: true),
                    NumberOfVehicles = table.Column<int>(nullable: true),
                    NumberOfPowerUnits = table.Column<int>(nullable: true),
                    NumberOfSpareVehicles = table.Column<int>(nullable: true),
                    NumberOfDrivers = table.Column<int>(nullable: true),
                    LossRunDateForEachTerm = table.Column<DateTime>(nullable: true),
                    GrossRevenues = table.Column<int>(nullable: true),
                    TotalFleetMileage = table.Column<int>(nullable: true),
                    NumberOfOneWayTrips = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskHistory_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoverage_RiskId",
                table: "RiskCoverage",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskHistory_RiskId",
                table: "RiskHistory",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoveragePlan");

            migrationBuilder.DropTable(
                name: "CoveredSymbol");

            migrationBuilder.DropTable(
                name: "RiskCoverage");

            migrationBuilder.DropTable(
                name: "RiskHistory");
        }
    }
}
