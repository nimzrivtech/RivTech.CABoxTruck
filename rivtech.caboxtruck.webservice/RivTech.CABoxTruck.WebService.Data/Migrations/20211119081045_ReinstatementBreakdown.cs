using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class ReinstatementBreakdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReinstatementBreakdown",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountSubTypeId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    MinimumPremiumAdjustment = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ShortRatePremium = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReinstatementBreakdown", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[FeeKind] ([Id], [Description], [IsActive])
                    VALUES	('REINF', 'Reinstatement Fee', 1)
            ");

            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[NonTaxableFee] ([StateCode], [FeeKindId], [EffectiveDate])
                    VALUES	('NJ', 'REINF', '2021-01-01 00:00:00.0000000'),
                            ('CA', 'REINF', '2021-01-01 00:00:00.0000000')
            ");

            migrationBuilder.Sql("DELETE FROM NonTaxableFee where FeeKindId = 'REINSTAT'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM NonTaxableFee where StateCode = 'CA' and FeeKindId = 'REINF'
                DELETE FROM NonTaxableFee where StateCode = 'NJ' and FeeKindId = 'REINF'
            ");

            migrationBuilder.Sql(@"
                DELETE FROM FeeKind where Id = 'REINF'
            ");

            migrationBuilder.DropTable(
                name: "ReinstatementBreakdown");
        }
    }
}
