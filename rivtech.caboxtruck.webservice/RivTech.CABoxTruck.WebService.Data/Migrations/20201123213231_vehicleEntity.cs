using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class vehicleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: false),
                    age = table.Column<short>(nullable: true),
                    vin = table.Column<string>(nullable: true),
                    year = table.Column<short>(nullable: true),
                    make = table.Column<string>(nullable: true),
                    model = table.Column<string>(nullable: true),
                    style = table.Column<string>(nullable: true),
                    vehicleType = table.Column<string>(nullable: true),
                    grossVehicleWeight = table.Column<string>(nullable: true),
                    vehicleDesc = table.Column<string>(nullable: true),
                    averageMileage = table.Column<string>(nullable: true),
                    autoBiLimit = table.Column<string>(nullable: true),
                    autoPdLimit = table.Column<string>(nullable: true),
                    liabilityDeductible = table.Column<string>(nullable: true),
                    medPay = table.Column<string>(nullable: true),
                    pip = table.Column<string>(nullable: true),
                    uMBI = table.Column<string>(nullable: true),
                    uMPD = table.Column<string>(nullable: true),
                    uIMBI = table.Column<string>(nullable: true),
                    uMStacking = table.Column<string>(nullable: true),
                    nonTruckingLiability = table.Column<string>(nullable: true),
                    overrideColDed = table.Column<string>(nullable: true),
                    overrideCompDed = table.Column<string>(nullable: true),
                    collStatedAmount = table.Column<int>(nullable: true),
                    compStatedAmount = table.Column<int>(nullable: true),
                    cARGOCOMPLimits = table.Column<string>(nullable: true),
                    cARGOCOLLLimits = table.Column<string>(nullable: true),
                    refrigeratedCargoCOLLLimits = table.Column<int>(nullable: true),
                    refrigeratedCargoCOMPLimits = table.Column<int>(nullable: true),
                    accidentalDeathNJPA = table.Column<string>(nullable: true),
                    essentialServicesNJ = table.Column<string>(nullable: true),
                    funeralBenefitsNJPA = table.Column<string>(nullable: true),
                    incomeLossNJPAVA = table.Column<string>(nullable: true),
                    useClass = table.Column<string>(nullable: true),
                    radius = table.Column<string>(nullable: true),
                    radiusTERR = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    garagingZipCode = table.Column<string>(nullable: true),
                    useClassCode = table.Column<string>(nullable: true),
                    classCodeMeaning = table.Column<string>(nullable: true),
                    VehicleLevelBusinessClass = table.Column<string>(nullable: true),
                    liabRequested = table.Column<string>(nullable: true),
                    collRequested = table.Column<string>(nullable: true),
                    CompFireTheftRequested = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<short>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
