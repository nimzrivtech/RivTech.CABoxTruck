using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class driverinformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverPeriod");

            migrationBuilder.DropTable(
                name: "DriverSummary");

            migrationBuilder.DropTable(
                name: "RiskList");

            migrationBuilder.DropColumn(
                name: "IsIncludedInOption1",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "age",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "dob",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Driver");

            migrationBuilder.RenameColumn(
                name: "yrsDrivingExp",
                table: "Driver",
                newName: "YrsDrivingExp");

            migrationBuilder.RenameColumn(
                name: "yrsCommDrivingExp",
                table: "Driver",
                newName: "YrsCommDrivingExp");

            migrationBuilder.RenameColumn(
                name: "stateId",
                table: "Driver",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "outStateDriver",
                table: "Driver",
                newName: "OutStateDriver");

            migrationBuilder.RenameColumn(
                name: "yearHired",
                table: "Driver",
                newName: "HiredDate");

            migrationBuilder.RenameColumn(
                name: "IsIncludedInOption4",
                table: "Driver",
                newName: "IsOutOfState");

            migrationBuilder.RenameColumn(
                name: "IsIncludedInOption3",
                table: "Driver",
                newName: "IsMvr");

            migrationBuilder.RenameColumn(
                name: "IsIncludedInOption2",
                table: "Driver",
                newName: "Include");

            migrationBuilder.AddColumn<string>(
                name: "AccountDriverFactor",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgeFactor",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Driver",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MvrDate",
                table: "Driver",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalFactor",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearPoint",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DriverIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncidentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConvictionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverIncidents_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Driver_EntityId",
                table: "Driver",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Driver_RiskId",
                table: "Driver",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverIncidents_DriverId",
                table: "DriverIncidents",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_Entity_EntityId",
                table: "Driver",
                column: "EntityId",
                principalTable: "Entity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_Risk_RiskId",
                table: "Driver",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Driver_Entity_EntityId",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_Risk_RiskId",
                table: "Driver");

            migrationBuilder.DropTable(
                name: "DriverIncidents");

            migrationBuilder.DropIndex(
                name: "IX_Driver_EntityId",
                table: "Driver");

            migrationBuilder.DropIndex(
                name: "IX_Driver_RiskId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "AccountDriverFactor",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "AgeFactor",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "MvrDate",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "TotalFactor",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "YearPoint",
                table: "Driver");

            migrationBuilder.RenameColumn(
                name: "YrsDrivingExp",
                table: "Driver",
                newName: "yrsDrivingExp");

            migrationBuilder.RenameColumn(
                name: "YrsCommDrivingExp",
                table: "Driver",
                newName: "yrsCommDrivingExp");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Driver",
                newName: "stateId");

            migrationBuilder.RenameColumn(
                name: "OutStateDriver",
                table: "Driver",
                newName: "outStateDriver");

            migrationBuilder.RenameColumn(
                name: "IsOutOfState",
                table: "Driver",
                newName: "IsIncludedInOption4");

            migrationBuilder.RenameColumn(
                name: "IsMvr",
                table: "Driver",
                newName: "IsIncludedInOption3");

            migrationBuilder.RenameColumn(
                name: "Include",
                table: "Driver",
                newName: "IsIncludedInOption2");

            migrationBuilder.RenameColumn(
                name: "HiredDate",
                table: "Driver",
                newName: "yearHired");

            migrationBuilder.AddColumn<bool>(
                name: "IsIncludedInOption1",
                table: "Driver",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "age",
                table: "Driver",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dob",
                table: "Driver",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Driver",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DriverPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndorsementNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemoveProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<short>(type: "smallint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummaryYear = table.Column<short>(type: "smallint", nullable: true),
                    atFault = table.Column<int>(type: "int", nullable: true),
                    duiDwiDrug = table.Column<int>(type: "int", nullable: true),
                    equipment = table.Column<int>(type: "int", nullable: true),
                    majorLicenseIssue = table.Column<int>(type: "int", nullable: true),
                    majors = table.Column<int>(type: "int", nullable: true),
                    minors = table.Column<int>(type: "int", nullable: true),
                    naf = table.Column<int>(type: "int", nullable: true),
                    other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    speed = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "RiskList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Broker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsuredName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmissionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskList", x => x.Id);
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
    }
}
