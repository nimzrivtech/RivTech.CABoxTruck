using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class VehicleProratedPremiums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VehicleProratedPremiumId",
                table: "Vehicle",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VehicleProratedPremium",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ALProratedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PDProratedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CargoProratedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollisionProratedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComprehensiveProratedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefrigerationProratedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleProratedPremium", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_VehicleProratedPremiumId",
                table: "Vehicle",
                column: "VehicleProratedPremiumId",
                unique: true,
                filter: "[VehicleProratedPremiumId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleProratedPremium_VehicleProratedPremiumId",
                table: "Vehicle",
                column: "VehicleProratedPremiumId",
                principalTable: "VehicleProratedPremium",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleProratedPremium_VehicleProratedPremiumId",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "VehicleProratedPremium");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_VehicleProratedPremiumId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "VehicleProratedPremiumId",
                table: "Vehicle");
        }
    }
}
