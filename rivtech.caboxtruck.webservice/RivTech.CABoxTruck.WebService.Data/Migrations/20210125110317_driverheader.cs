using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class driverheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverHeader",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driverRatingTab = table.Column<bool>(type: "bit", nullable: true),
                    accidentInfo = table.Column<bool>(type: "bit", nullable: true),
                    mvrHeaderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    applyFilter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accDriverFactor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverHeader_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverHeader_RiskId",
                table: "DriverHeader",
                column: "RiskId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverHeader");
        }
    }
}
