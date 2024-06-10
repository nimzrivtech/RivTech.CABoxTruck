using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class risk_specific_driver_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskSpecificDriverInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TotalNumberOfDrivers = table.Column<int>(nullable: false),
                    DriversHired = table.Column<int>(nullable: false),
                    DriversTerminated = table.Column<int>(nullable: false),
                    MvrPullingFrequencyId = table.Column<int>(nullable: false),
                    IsBusinessPrincipalADriverInPolicy = table.Column<bool>(nullable: false),
                    AreVolunteersUsedInBusiness = table.Column<bool>(nullable: false),
                    PercentageOfStaff = table.Column<decimal>(type: "decimal(3, 2)", nullable: false),
                    IsHiringFromOthers = table.Column<bool>(nullable: false),
                    AnnualCostOfHire = table.Column<decimal>(type: "decimal(11, 2)", nullable: false),
                    IsHiringFromOthersWithoutDriver = table.Column<bool>(nullable: false),
                    AnnualCostOfHireWithoutDriver = table.Column<decimal>(type: "decimal(11, 2)", nullable: false),
                    IsLeasingToOthers = table.Column<bool>(nullable: false),
                    AnnualIncomeDerivedFromLease = table.Column<decimal>(type: "decimal(11, 2)", nullable: false),
                    IsLeasingToOthersWithoutDriver = table.Column<bool>(nullable: false),
                    AnnualIncomeDerivedFromLeaseWithoutDriver = table.Column<decimal>(type: "decimal(11, 2)", nullable: false),
                    IsThereAssumedLiabilityByContract = table.Column<bool>(nullable: false),
                    DoDriversTakeVehiclesHome = table.Column<bool>(nullable: false),
                    AreVehiclesSolelyOwnedByApplicant = table.Column<bool>(nullable: false),
                    WillBeAddingDeletingVehiclesDuringTerm = table.Column<bool>(nullable: false),
                    AddingDeletingVehiclesDuringTermDescription = table.Column<string>(type: "varchar(250)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificDriverInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificDriverInfo");
        }
    }
}
