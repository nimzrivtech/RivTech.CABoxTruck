using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Create_SafetyDevice_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LvRiskSpecificSafetyDeviceCategory",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvRiskSpecificSafetyDeviceCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskSpecificSafetyDevice",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<Guid>(nullable: false),
                    SafetyDeviceCategoryId = table.Column<short>(nullable: false),
                    IsInPlace = table.Column<bool>(nullable: false),
                    UnitDescription = table.Column<string>(nullable: true),
                    YearsInPlace = table.Column<short>(nullable: false),
                    PercentageOfFleet = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskSpecificSafetyDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskSpecificSafetyDevice_LvRiskSpecificSafetyDeviceCategory_SafetyDeviceCategoryId",
                        column: x => x.SafetyDeviceCategoryId,
                        principalTable: "LvRiskSpecificSafetyDeviceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskSpecificSafetyDevice_SafetyDeviceCategoryId",
                table: "RiskSpecificSafetyDevice",
                column: "SafetyDeviceCategoryId");

            migrationBuilder.Sql(INSERT_LvRiskSpecificSafetyDeviceCategory_DATA);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskSpecificSafetyDevice");

            migrationBuilder.DropTable(
                name: "LvRiskSpecificSafetyDeviceCategory");
        }

        const string INSERT_LvRiskSpecificSafetyDeviceCategory_DATA = @"
            insert into dbo.LvRiskSpecificSafetyDeviceCategory([Description], IsActive)
            values('Cameras', 1),
            ('Accident Event Recorders (AER`s)', 1),
            ('Location Tracking Device', 1),
            ('Geographic Driving History Data', 1),
            ('Mileage Tracking Device', 1),
            ('Brake Warning System', 1),
            ('Distracted Driving Warning System', 1),
            ('Speed Warning System', 1),
            ('Monitoring of Harsh Braking/Speeding/Accelerating via Telematics Device', 1),
            ('Any Active Accident Avoidance Technology', 1),
            ('Reflective Tape', 1),
            ('Step Stools', 1),
            ('1-800-HOWSMYDRIVING Program', 1),
            ('No Texting/Cell Phone/Handheld Device Usage While Driving', 1),
            ('Electronic Logging Devices (ELDs)', 1); ";
    }
}
