using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class riskcoveragepremium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ApdExperienceRF",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ApdScheduleRF",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BrokerCommisionAL",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BrokerCommisionPD",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoExperienceRF",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoScheduleRF",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositPct",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GlExperienceRF",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GlScheduleRF",
                table: "RiskCoverage",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NumberOfInstallments",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RiskCoveragePremium",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskCoverageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoLiabilityPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CargoPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalDamagePremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VehicleLevelPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GeneralLiabilityPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvgPerVehicle = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RiskMgrFeeAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RiskMgrFeePD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PolicyTotals = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Premium = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Fees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxesAndStampingFees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PerInstallment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmounDueFull = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmountDueFinanced = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddProcessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<long>(type: "bigint", nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCoveragePremium", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskCoveragePremium_RiskCoverage_RiskCoverageId",
                        column: x => x.RiskCoverageId,
                        principalTable: "RiskCoverage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoveragePremium_RiskCoverageId",
                table: "RiskCoveragePremium",
                column: "RiskCoverageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ApdExperienceRF",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "ApdScheduleRF",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "BrokerCommisionAL",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "BrokerCommisionPD",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "CargoExperienceRF",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "CargoScheduleRF",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "DepositPct",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GlExperienceRF",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "GlScheduleRF",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "NumberOfInstallments",
                table: "RiskCoverage");
        }
    }
}
