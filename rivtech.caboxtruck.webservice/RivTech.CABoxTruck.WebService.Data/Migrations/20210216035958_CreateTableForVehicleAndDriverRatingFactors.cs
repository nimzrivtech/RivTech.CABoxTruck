using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class CreateTableForVehicleAndDriverRatingFactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehiclePremiumRatingFactor",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false).Annotation("SqlServer:Identity", "1,1"),
                    RiskId = table.Column<Guid>(nullable: false),
                    VIN = table.Column<string>(nullable: false),
                    ALPremium = table.Column<decimal>(nullable: true),
                    TotalPremium = table.Column<decimal>(nullable: true),
                    TotalALPremium = table.Column<decimal>(nullable: true),
                    TotalALAddOns = table.Column<decimal>(nullable: true),
                    TotalPDPremium = table.Column<decimal>(nullable: true),
                    BIPremium = table.Column<decimal>(nullable: true),
                    PropDamagePremium = table.Column<decimal>(nullable: true),
                    MedPayPremium = table.Column<decimal>(nullable: true),
                    PIPremium = table.Column<decimal>(nullable: true),
                    PIOtherPremium = table.Column<decimal>(nullable: true),
                    UMUIM_BI_Premium = table.Column<decimal>(nullable: true),
                    UMUIM_PD_Premium = table.Column<decimal>(nullable: true),
                    CompPremium = table.Column<decimal>(nullable: true),
                    FireTheftPremium = table.Column<decimal>(nullable: true),
                    CollisionPremium = table.Column<decimal>(nullable: true),
                    CargoPremium = table.Column<decimal>(nullable: true),
                    NTLPremium = table.Column<decimal>(nullable: true),
                    BIRiskMgmtFee = table.Column<decimal>(nullable: true),
                    PDRiskMgmtFee = table.Column<decimal>(nullable: true),
                    StatedAmount = table.Column<decimal>(nullable: true),
                    TIVPercentage = table.Column<decimal>(nullable: true),
                    OptionId = table.Column<short>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePremiumRatingFactor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePremiumRatingFactor_RiskCoverage_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskCoverage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverPremiumRatingFactor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1,1"),
                    RiskId = table.Column<Guid>(nullable: true),
                    DriverID = table.Column<string>(nullable: true),
                    FiveYearPoints = table.Column<string>(nullable: true),
                    DriverKey = table.Column<string>(nullable: true),
                    PointsFactor = table.Column<string>(nullable: true),
                    YearsExperienceFactor = table.Column<string>(nullable: true),
                    AgeFactor = table.Column<string>(nullable: true),
                    OutOfStateFactor = table.Column<string>(nullable: true),
                    TotalFactor = table.Column<string>(nullable: true),
                    Included = table.Column<string>(nullable: true),
                    TotalFactorIncl = table.Column<string>(nullable: true),
                    FactorIncl_AddDriver = table.Column<string>(nullable: true),
                    UnderAge25 = table.Column<string>(nullable: true),
                    UnderAge25AndClean = table.Column<string>(nullable: true),
                    HasMultipleAFOrMovingViolation = table.Column<string>(nullable: true),
                    OptionId = table.Column<short>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPremiumRatingFactor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverPremiumRatingFactor_RiskCoverage_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskCoverage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehiclePremiumRatingFactor");

            migrationBuilder.DropTable(
                name: "DriverPremiumRatingFactor");
        }
    }
}
