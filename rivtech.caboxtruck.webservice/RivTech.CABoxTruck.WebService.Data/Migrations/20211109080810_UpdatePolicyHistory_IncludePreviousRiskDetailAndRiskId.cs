using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyHistory_IncludePreviousRiskDetailAndRiskId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PreviousRiskDetailId",
                table: "PolicyHistory",
                type: "varchar(100)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RiskId",
                table: "PolicyHistory",
                type: "varchar(100)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousRiskDetailId",
                table: "PolicyHistory");

            migrationBuilder.DropColumn(
                name: "RiskId",
                table: "PolicyHistory");
        }
    }
}
