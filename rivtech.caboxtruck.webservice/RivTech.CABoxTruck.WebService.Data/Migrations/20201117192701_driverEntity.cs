using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class driverEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    dob = table.Column<DateTime>(nullable: false),
                    age = table.Column<short>(nullable: true),
                    yearHired = table.Column<short>(nullable: true),
                    outStateDriver = table.Column<bool>(nullable: true),
                    yrsDrivingExp = table.Column<short>(nullable: true),
                    yrsCommDrivingExp = table.Column<short>(nullable: true),
                    stateId = table.Column<short>(nullable: true),
                    IsIncludedInOption1 = table.Column<bool>(nullable: true),
                    IsIncludedInOption2 = table.Column<bool>(nullable: true),
                    IsIncludedInOption3 = table.Column<bool>(nullable: true),
                    IsIncludedInOption4 = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<short>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DriverId = table.Column<Guid>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    AddProcessDate = table.Column<DateTime>(nullable: true),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true),
                    EndorsementNumber = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverPeriod_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverSummary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DriverId = table.Column<Guid>(nullable: false),
                    SummaryYear = table.Column<short>(nullable: true),
                    majors = table.Column<int>(nullable: true),
                    duiDwiDrug = table.Column<int>(nullable: true),
                    minors = table.Column<int>(nullable: true),
                    atFault = table.Column<int>(nullable: true),
                    majorLicenseIssue = table.Column<int>(nullable: true),
                    naf = table.Column<int>(nullable: true),
                    equipment = table.Column<int>(nullable: true),
                    speed = table.Column<int>(nullable: true),
                    other = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<short>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverSummary_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverPeriod_DriverId",
                table: "DriverPeriod",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverSummary_DriverId",
                table: "DriverSummary",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverPeriod");

            migrationBuilder.DropTable(
                name: "DriverSummary");

            migrationBuilder.DropTable(
                name: "Driver");
        }
    }
}
