using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskList_BrokerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: true),
                    AgencyId = table.Column<Guid>(nullable: true),
                    AgentId = table.Column<Guid>(nullable: true),
                    SubAgencyId = table.Column<Guid>(nullable: true),
                    SubAgentId = table.Column<Guid>(nullable: true),
                    YearsWithAgency = table.Column<short>(nullable: true),
                    ReasonMoveId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    IsIncumbentAgency = table.Column<bool>(nullable: true),
                    IsBrokeredAccount = table.Column<bool>(nullable: true),
                    IsMidtermMove = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: true),
                    SubmissionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    InsuredName = table.Column<string>(nullable: true),
                    Broker = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskList", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerInfo");

            migrationBuilder.DropTable(
                name: "RiskList");
        }
    }
}
