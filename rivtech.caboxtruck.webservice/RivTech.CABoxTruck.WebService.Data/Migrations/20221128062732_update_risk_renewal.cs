using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_risk_renewal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFromRenewal",
                table: "Risk",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromSN",
                table: "Risk",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPolicyNumber",
                table: "Risk",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RenewalDate",
                table: "Risk",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFromRenewal",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsFromSN",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PreviousPolicyNumber",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RenewalDate",
                table: "Risk");
        }
    }
}
