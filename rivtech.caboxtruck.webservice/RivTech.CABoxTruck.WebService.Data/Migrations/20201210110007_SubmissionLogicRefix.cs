using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class SubmissionLogicRefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsBrokeredAccount",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsIncumbentAgency",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "SubAgencyId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "YearsWithAgency",
                table: "Risk");

            migrationBuilder.AlterColumn<bool>(
                name: "IsNewBusiness",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFilingRequired",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDifferentUSDOTNumberLastFiveYears",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsARatedCarrierRequired",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "InsuredId",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FirstIssueDate",
                table: "Risk",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Agency",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    AgencyId = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agency_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    AgencyId = table.Column<Guid>(nullable: true),
                    SubAgencyId = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agent_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LvReasonMove",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvReasonMove", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubAgency",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    AgencyId = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubAgency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubAgency_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubAgent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    AgencyId = table.Column<Guid>(nullable: true),
                    SubAgencyId = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubAgent_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_AgencyId",
                table: "BrokerInfo",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_AgentId",
                table: "BrokerInfo",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_RiskId",
                table: "BrokerInfo",
                column: "RiskId",
                unique: true,
                filter: "[RiskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_SubAgencyId",
                table: "BrokerInfo",
                column: "SubAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_SubAgentId",
                table: "BrokerInfo",
                column: "SubAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Agency_EntityId",
                table: "Agency",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agent_EntityId",
                table: "Agent",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubAgency_EntityId",
                table: "SubAgency",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubAgent_EntityId",
                table: "SubAgent",
                column: "EntityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_Agency_AgencyId",
                table: "BrokerInfo",
                column: "AgencyId",
                principalTable: "Agency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_Agent_AgentId",
                table: "BrokerInfo",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_Risk_RiskId",
                table: "BrokerInfo",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_SubAgency_SubAgencyId",
                table: "BrokerInfo",
                column: "SubAgencyId",
                principalTable: "SubAgency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_SubAgent_SubAgentId",
                table: "BrokerInfo",
                column: "SubAgentId",
                principalTable: "SubAgent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.Sql(INSERT_REASONMOVELV_DATA);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Agency_AgencyId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Agent_AgentId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Risk_RiskId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_SubAgency_SubAgencyId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_SubAgent_SubAgentId",
                table: "BrokerInfo");

            migrationBuilder.DropTable(
                name: "Agency");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "LvReasonMove");

            migrationBuilder.DropTable(
                name: "SubAgency");

            migrationBuilder.DropTable(
                name: "SubAgent");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_AgencyId",
                table: "BrokerInfo");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_AgentId",
                table: "BrokerInfo");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_RiskId",
                table: "BrokerInfo");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_SubAgencyId",
                table: "BrokerInfo");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_SubAgentId",
                table: "BrokerInfo");

            migrationBuilder.AlterColumn<bool>(
                name: "IsNewBusiness",
                table: "Risk",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFilingRequired",
                table: "Risk",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDifferentUSDOTNumberLastFiveYears",
                table: "Risk",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsARatedCarrierRequired",
                table: "Risk",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InsuredId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FirstIssueDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgencyId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsBrokeredAccount",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncumbentAgency",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubAgencyId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<short>(
                name: "YearsWithAgency",
                table: "Risk",
                type: "smallint",
                nullable: true);
        }

        const string INSERT_REASONMOVELV_DATA = @"
                    insert dbo.LvReasonMove ([Description], IsActive)
                    values ('Required Coverage Not Available', 1),
                    ('Change in Exposure (routes, vehicle types)', 1),
                    ('Driver Not approved', 1),
                    ('State and/or DOT filings not available', 1),
                    ('Pricing', 1),
                    ('Non-Pay Cancellation; reinstatement not available', 1);
                ";
    }
}
