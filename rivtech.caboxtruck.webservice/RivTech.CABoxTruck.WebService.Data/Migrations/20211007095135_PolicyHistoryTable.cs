using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class PolicyHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndorsementNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    NewPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    ProratedPremium = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PremiumChange = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyHistory");
        }
    }
}
