using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehicleTransactionUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskVehiclePremium_Vehicle_VehicleId",
                table: "RiskVehiclePremium");

            migrationBuilder.DropIndex(
                name: "IX_RiskVehiclePremium_VehicleId",
                table: "RiskVehiclePremium");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "RiskVehiclePremium");

            migrationBuilder.AddColumn<decimal>(
                name: "PremiumChangeAL",
                table: "RiskVehiclePremiumTransaction",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PremiumChangeCargo",
                table: "RiskVehiclePremiumTransaction",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PremiumChangePD",
                table: "RiskVehiclePremiumTransaction",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumChangeAL",
                table: "RiskVehiclePremiumTransaction");

            migrationBuilder.DropColumn(
                name: "PremiumChangeCargo",
                table: "RiskVehiclePremiumTransaction");

            migrationBuilder.DropColumn(
                name: "PremiumChangePD",
                table: "RiskVehiclePremiumTransaction");

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleId",
                table: "RiskVehiclePremium",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskVehiclePremium_VehicleId",
                table: "RiskVehiclePremium",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskVehiclePremium_Vehicle_VehicleId",
                table: "RiskVehiclePremium",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
