using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Add_RiskDetailDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PreviousDriverVersionId",
                table: "Driver",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RiskDetailDriver",
                columns: table => new
                {
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSkipped = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskDetailDriver", x => new { x.RiskDetailId, x.DriverId });
                    table.ForeignKey(
                        name: "FK_RiskDetailDriver_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RiskDetailDriver_RiskDetail_RiskDetailId",
                        column: x => x.RiskDetailId,
                        principalTable: "RiskDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Driver_PreviousDriverVersionId",
                table: "Driver",
                column: "PreviousDriverVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskDetailDriver_DriverId",
                table: "RiskDetailDriver",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_Driver_PreviousDriverVersionId",
                table: "Driver",
                column: "PreviousDriverVersionId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Driver_Driver_PreviousDriverVersionId",
                table: "Driver");

            migrationBuilder.DropTable(
                name: "RiskDetailDriver");

            migrationBuilder.DropIndex(
                name: "IX_Driver_PreviousDriverVersionId",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "PreviousDriverVersionId",
                table: "Driver");
        }
    }
}
