using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskDetailVehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_RiskDetail_RiskDetailId",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_RiskDetailId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "RiskDetailId",
                table: "Vehicle");

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousVehicleVersionId",
                table: "Vehicle",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RiskDetailVehicle",
                columns: table => new
                {
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSkipped = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskDetailVehicle", x => new { x.RiskDetailId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_RiskDetailVehicle_RiskDetail_RiskDetailId",
                        column: x => x.RiskDetailId,
                        principalTable: "RiskDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskDetailVehicle_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_PreviousVehicleVersionId",
                table: "Vehicle",
                column: "PreviousVehicleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskDetailVehicle_VehicleId",
                table: "RiskDetailVehicle",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Vehicle_PreviousVehicleVersionId",
                table: "Vehicle",
                column: "PreviousVehicleVersionId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Vehicle_PreviousVehicleVersionId",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "RiskDetailVehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_PreviousVehicleVersionId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PreviousVehicleVersionId",
                table: "Vehicle");

            migrationBuilder.AddColumn<Guid>(
                name: "RiskDetailId",
                table: "Vehicle",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_RiskDetailId",
                table: "Vehicle",
                column: "RiskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_RiskDetail_RiskDetailId",
                table: "Vehicle",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id");
        }
    }
}
