using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class remove_fields_risk_coverages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccidentalDeathLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "CargoClaimsLastFiveYears",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "CoveragePlanId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "EssentialServicesLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "FireTheftDeductibleId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "FuneralBenefitsLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLClaimsLastFiveYears",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLCompletedOperationsLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLDamageToRentalPremisesLimitid",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLIsOperationAtAStorageImpound",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLMedicalExpenseLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLPDLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLPayrollAmount",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GLPersonalAdvertisingInjuryLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GaragekeepersDirectExcessLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GaragekeepersDirectPrimaryLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GaragekeepersLegalLiabilityLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IncomeLossLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsDriveOtherCar",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsExcludeDriversUnder25WithActivity",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsExcludeDriversWithAFMovingViolations",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsGuestPIP",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsHiredAutoLiability",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsNonOwnedLiability",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsPIPRequested",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsRoadsideAssistance",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "IsUMStacking",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "NonTruckingLiabilityLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "OnHookDirectExcessLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "OnHookDirectPrimaryLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "OnHookLegalLiabilityLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "RentalReimbursementDPLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "RentalReimbursementLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "TrailerInterchangeLimitId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "UIMPDLimitId",
                table: "RiskCoverage");

            migrationBuilder.AddColumn<short>(
                name: "NumAddtlInsured",
                table: "RiskCoverage",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PrimeNonContribEndorsement",
                table: "RiskCoverage",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "SpecifiedPerils",
                table: "RiskCoverage",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "WaiverSubrogation",
                table: "RiskCoverage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumAddtlInsured",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "PrimeNonContribEndorsement",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "SpecifiedPerils",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "WaiverSubrogation",
                table: "RiskCoverage");

            migrationBuilder.AddColumn<int>(
                name: "AccidentalDeathLimitId",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargoClaimsLastFiveYears",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "CoveragePlanId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EssentialServicesLimitId",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "FireTheftDeductibleId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FuneralBenefitsLimitId",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GLClaimsLastFiveYears",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GLCompletedOperationsLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GLDamageToRentalPremisesLimitid",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GLIsOperationAtAStorageImpound",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "GLMedicalExpenseLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GLPDLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GLPayrollAmount",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GLPersonalAdvertisingInjuryLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GaragekeepersDirectExcessLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GaragekeepersDirectPrimaryLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GaragekeepersLegalLiabilityLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeLossLimitId",
                table: "RiskCoverage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDriveOtherCar",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExcludeDriversUnder25WithActivity",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExcludeDriversWithAFMovingViolations",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestPIP",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHiredAutoLiability",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNonOwnedLiability",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPIPRequested",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRoadsideAssistance",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUMStacking",
                table: "RiskCoverage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "NonTruckingLiabilityLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OnHookDirectExcessLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OnHookDirectPrimaryLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OnHookLegalLiabilityLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RentalReimbursementDPLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RentalReimbursementLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TrailerInterchangeLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UIMPDLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);
        }
    }
}
