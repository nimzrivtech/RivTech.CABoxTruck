using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskCoveragePremiumAvg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskCoveragePremiumAvg",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskCoveragePremiumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvgAlVehicleTotalPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgAlVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgAlVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgApdVehicleTotalPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgApdVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgApdVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgMtcRefVehicleTotalPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgMtcRefVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgMtcRefVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgTotalVehiclePremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgTotalVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AvgTotalVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    NumberOfVehicles = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    NumberOfPowerUnits = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    NumberOfTrailerOthers = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AlVehicleTotalPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AlVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    AlVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ApdVehicleTotalPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ApdVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ApdVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    MtcRefVehicleTotalPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    MtcRefVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    MtcRefVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalVehiclePremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalVehiclePowerPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalVehicleTrailerOtherPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalAIPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalManuPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddProcessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<long>(type: "bigint", nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCoveragePremiumAvg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskCoveragePremiumAvg_RiskCoveragePremium_RiskCoveragePremiumId",
                        column: x => x.RiskCoveragePremiumId,
                        principalTable: "RiskCoveragePremium",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskCoveragePremiumAvg_RiskCoveragePremiumId",
                table: "RiskCoveragePremiumAvg",
                column: "RiskCoveragePremiumId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskCoveragePremiumAvg");
        }
    }
}
