using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class removebrokerinfoagency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Agency_AgencyId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Agent_AgentId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_SubAgency_SubAgencyId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_SubAgent_SubAgentId",
                table: "BrokerInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
