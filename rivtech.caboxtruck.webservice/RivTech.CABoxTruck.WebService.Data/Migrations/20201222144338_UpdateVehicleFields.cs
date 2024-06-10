using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateVehicleFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompFireTheftRequested",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "VehicleLevelBusinessClass",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "accidentalDeathNJPA",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "autoBiLimit",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "autoPdLimit",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "averageMileage",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "cARGOCOLLLimits",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "cARGOCOMPLimits",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "classCodeMeaning",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "collRequested",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "collStatedAmount",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "compStatedAmount",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "essentialServicesNJ",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "funeralBenefitsNJPA",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "garagingZipCode",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "grossVehicleWeight",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "incomeLossNJPAVA",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "liabRequested",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "liabilityDeductible",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "medPay",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "nonTruckingLiability",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "overrideColDed",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "overrideCompDed",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "pip",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "radius",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "radiusTERR",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "uIMBI",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "uMBI",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "uMPD",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "uMStacking",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "useClass",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "useClassCode",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "vehicleDesc",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "Vehicle",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "vin",
                table: "Vehicle",
                newName: "VIN");

            migrationBuilder.RenameColumn(
                name: "vehicleType",
                table: "Vehicle",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "style",
                table: "Vehicle",
                newName: "Style");

            migrationBuilder.RenameColumn(
                name: "model",
                table: "Vehicle",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "make",
                table: "Vehicle",
                newName: "Make");

            migrationBuilder.RenameColumn(
                name: "refrigeratedCargoCOMPLimits",
                table: "Vehicle",
                newName: "PrimaryOperatingZipCode");

            migrationBuilder.RenameColumn(
                name: "refrigeratedCargoCOLLLimits",
                table: "Vehicle",
                newName: "OptionId");

            migrationBuilder.RenameColumn(
                name: "age",
                table: "Vehicle",
                newName: "VehicleDescriptionId");

            migrationBuilder.AddColumn<short>(
                name: "CoverageCollisionDeductibleId",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "CoverageFireTheftDeductibleId",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GaragingAddressId",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GrossVehicleWeightId",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCoverageCargo",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCoverageRefrigeration",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Include",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoLiability",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCoverageCollision",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCoverageFireTheft",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StatedAmount",
                table: "Vehicle",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<short>(
                name: "UseClassId",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "VehicleBusinessClassId",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_RiskId",
                table: "Vehicle",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Risk_RiskId",
                table: "Vehicle",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Risk_RiskId",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_RiskId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "CoverageCollisionDeductibleId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "CoverageFireTheftDeductibleId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "GaragingAddressId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "GrossVehicleWeightId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "HasCoverageCargo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "HasCoverageRefrigeration",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Include",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "IsAutoLiability",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "IsCoverageCollision",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "IsCoverageFireTheft",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "StatedAmount",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "UseClassId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "VehicleBusinessClassId",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Vehicle",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "Vehicle",
                newName: "vehicleType");

            migrationBuilder.RenameColumn(
                name: "VIN",
                table: "Vehicle",
                newName: "vin");

            migrationBuilder.RenameColumn(
                name: "Style",
                table: "Vehicle",
                newName: "style");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Vehicle",
                newName: "model");

            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Vehicle",
                newName: "make");

            migrationBuilder.RenameColumn(
                name: "VehicleDescriptionId",
                table: "Vehicle",
                newName: "age");

            migrationBuilder.RenameColumn(
                name: "PrimaryOperatingZipCode",
                table: "Vehicle",
                newName: "refrigeratedCargoCOMPLimits");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "Vehicle",
                newName: "refrigeratedCargoCOLLLimits");

            migrationBuilder.AddColumn<string>(
                name: "CompFireTheftRequested",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleLevelBusinessClass",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "accidentalDeathNJPA",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "autoBiLimit",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "autoPdLimit",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "averageMileage",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cARGOCOLLLimits",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cARGOCOMPLimits",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "classCodeMeaning",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "collRequested",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "collStatedAmount",
                table: "Vehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "compStatedAmount",
                table: "Vehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "essentialServicesNJ",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "funeralBenefitsNJPA",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "garagingZipCode",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "grossVehicleWeight",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "incomeLossNJPAVA",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "liabRequested",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "liabilityDeductible",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "medPay",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nonTruckingLiability",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "overrideColDed",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "overrideCompDed",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pip",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "radius",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "radiusTERR",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uIMBI",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uMBI",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uMPD",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uMStacking",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "useClass",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "useClassCode",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vehicleDesc",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
