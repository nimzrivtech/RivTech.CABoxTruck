using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddClaimsHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimsHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    LossRunDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GroundUpNet = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    BiPiPmpIncLossTotal = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    BiPiPmpIncLoss100KCap = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PropertyDamageIncLoss = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ClaimCountBiPip = table.Column<int>(type: "int", nullable: true),
                    ClaimCountPropertyDamage = table.Column<int>(type: "int", nullable: true),
                    OpenClaimsUnder500 = table.Column<int>(type: "int", nullable: true),
                    NetLoss = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    TotalClaimCount = table.Column<int>(type: "int", nullable: true),
                    CargoNumberOfClaims = table.Column<int>(type: "int", nullable: true),
                    GlNumberOfClaims = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimsHistory_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimsHistory_RiskId",
                table: "ClaimsHistory",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimsHistory");
        }
    }
}
