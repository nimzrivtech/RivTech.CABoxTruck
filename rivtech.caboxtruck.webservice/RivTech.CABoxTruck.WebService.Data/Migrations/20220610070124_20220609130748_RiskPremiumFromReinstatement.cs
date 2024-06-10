using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class _20220609130748_RiskPremiumFromReinstatement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FromReinstatement",
                table: "RiskVehiclePremiumTransaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromReinstatement",
                table: "RiskVehiclePremium",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromReinstatement",
                table: "RiskPolicyPremiumTransaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromReinstatement",
                table: "RiskPolicyPremium",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromReinstatement",
                table: "RiskDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastReinstatedRiskDetailId",
                table: "Risk",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromReinstatement",
                table: "RiskVehiclePremiumTransaction");

            migrationBuilder.DropColumn(
                name: "FromReinstatement",
                table: "RiskVehiclePremium");

            migrationBuilder.DropColumn(
                name: "FromReinstatement",
                table: "RiskPolicyPremiumTransaction");

            migrationBuilder.DropColumn(
                name: "FromReinstatement",
                table: "RiskPolicyPremium");

            migrationBuilder.DropColumn(
                name: "FromReinstatement",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "LastReinstatedRiskDetailId",
                table: "Risk");
        }
    }
}
