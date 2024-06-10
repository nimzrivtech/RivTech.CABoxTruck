using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Add_EndorsementPendingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndorsementPendingChanges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountSubTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PremiumBeforeEndorsement = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PremiumAfterEndorsement = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    PremiumDifference = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementPendingChanges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndorsementPendingChanges");
        }
    }
}
