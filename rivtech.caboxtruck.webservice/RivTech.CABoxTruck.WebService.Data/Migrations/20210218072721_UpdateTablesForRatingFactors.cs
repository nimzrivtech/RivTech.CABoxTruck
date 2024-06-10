using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateTablesForRatingFactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverPremiumRatingFactor");

            migrationBuilder.DropTable(
                name: "VehiclePremiumRatingFactor");

            migrationBuilder.CreateTable(
                name: "DriverPremiumRatingFactor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiveYearPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsExperienceFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutOfStateFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Included = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalFactorIncl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactorIncl_AddDriver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderAge25 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderAge25AndClean = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasMultipleAFOrMovingViolation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPremiumRatingFactor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehiclePremiumRatingFactor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ALPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalALPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalALAddOns = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPDPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BIPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PropDamagePremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MedPayPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PIPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PIOtherPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UMUIM_BI_Premium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UMUIM_PD_Premium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FireTheftPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CollisionPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CargoPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NTLPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BIRiskMgmtFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PDRiskMgmtFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StatedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TIVPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePremiumRatingFactor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverPremiumRatingFactor_Id",
                table: "DriverPremiumRatingFactor",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePremiumRatingFactor_Id",
                table: "VehiclePremiumRatingFactor",
                column: "Id",
                unique: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AccountDriverFactor",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalInsuredPremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PremiumTaxesFees",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrimaryNonContributoryPremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotalDue",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAnnualPremiumWithPremiumTax",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalOtherPolicyLevelPremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WaiverOfSubrogationPremium",
                table: "RiskCoveragePremium",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverPremiumRatingFactor");

            migrationBuilder.DropTable(
                name: "VehiclePremiumRatingFactor");

            migrationBuilder.DropColumn(
                name: "AccountDriverFactor",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "AdditionalInsuredPremium",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "PremiumTaxesFees",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "PrimaryNonContributoryPremium",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "SubTotalDue",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "TotalAnnualPremiumWithPremiumTax",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "TotalOtherPolicyLevelPremium",
                table: "RiskCoveragePremium"
            );
            migrationBuilder.DropColumn(
                name: "WaiverOfSubrogationPremium",
                table: "RiskCoveragePremium"
            );
        }
    }
}
