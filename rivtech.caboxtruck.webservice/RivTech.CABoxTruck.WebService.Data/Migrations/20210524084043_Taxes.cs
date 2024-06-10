using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Taxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaxDetailsId",
                table: "RiskCoveragePremium",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeeKind",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeKind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonTaxableFee",
                columns: table => new
                {
                    StateCode = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    FeeKindId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonTaxableFee", x => new { x.StateCode, x.FeeKindId, x.EffectiveDate });
                });

            migrationBuilder.CreateTable(
                name: "StateTax",
                columns: table => new
                {
                    StateCode = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaxSubtype = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(8,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateTax", x => new { x.StateCode, x.TaxType, x.TaxSubtype, x.EffectiveDate });
                });

            migrationBuilder.CreateTable(
                name: "TaxDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalSLTax = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TotalStampingFee = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxDetailsBreakdown",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxAmountTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxAmountSubtypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetailsBreakdown", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxDetailsBreakdown_TaxDetails_TaxDetailsId",
                        column: x => x.TaxDetailsId,
                        principalTable: "TaxDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetailsBreakdown_TaxDetailsId",
                table: "TaxDetailsBreakdown",
                column: "TaxDetailsId");

            migrationBuilder.Sql(SqlScripts.Seed_FeeKind);
            migrationBuilder.Sql(SqlScripts.Seed_NonTaxableFee);
            migrationBuilder.Sql(SqlScripts.Seed_StateTax);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeKind");

            migrationBuilder.DropTable(
                name: "NonTaxableFee");

            migrationBuilder.DropTable(
                name: "StateTax");

            migrationBuilder.DropTable(
                name: "TaxDetailsBreakdown");

            migrationBuilder.DropTable(
                name: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "TaxDetailsId",
                table: "RiskCoveragePremium");
        }
    }
}
