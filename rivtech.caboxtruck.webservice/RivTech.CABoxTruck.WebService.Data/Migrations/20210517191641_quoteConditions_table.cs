using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class quoteConditions_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuoteConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuoteConditionsOptionId = table.Column<short>(type: "smallint", nullable: true),
                    Describe = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RelevantDocumentDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuoteConditions_QuoteConditionsOption_QuoteConditionsOptionId",
                        column: x => x.QuoteConditionsOptionId,
                        principalTable: "QuoteConditionsOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteConditions_QuoteConditionsOptionId",
                table: "QuoteConditions",
                column: "QuoteConditionsOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuoteConditions");
        }
    }
}
