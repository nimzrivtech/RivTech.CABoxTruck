using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class CancellationBreakdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CancellationBreakdown",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountSubTypeId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    ActiveAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CancelledAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    AmountReduction = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    AnnualMinimumPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    MinimumPremiumAdjustment = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ShortRatePremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationBreakdown", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CancellationBreakdown");
        }
    }
}
