using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class riskspecificradiusofoperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskSpecificRadiusOfOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ZeroToFiftyMilesPercentage = table.Column<decimal>(nullable: false),
                    FiftyOneToTwoHundredMilesPercentage = table.Column<decimal>(nullable: false),
                    TwoHundredPlusPercentage = table.Column<decimal>(nullable: false),
                    OwnerOperatorPercentage = table.Column<decimal>(nullable: false),
                    AnyDestinationToMexicoPlanned = table.Column<bool>(nullable: false),
                    AnyNyc5BoroughsExposure = table.Column<bool>(nullable: false),
                    ExposureDescription = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificRadiusOfOperation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificRadiusOfOperation");
        }
    }
}
