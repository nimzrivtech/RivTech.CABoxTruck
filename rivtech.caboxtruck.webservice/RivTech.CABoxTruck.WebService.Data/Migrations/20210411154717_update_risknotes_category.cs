using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_risknotes_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RiskNotes_CategoryId",
                table: "RiskNotes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskNotes_NoteCategoryOption_CategoryId",
                table: "RiskNotes",
                column: "CategoryId",
                principalTable: "NoteCategoryOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskNotes_NoteCategoryOption_CategoryId",
                table: "RiskNotes");

            migrationBuilder.DropIndex(
                name: "IX_RiskNotes_CategoryId",
                table: "RiskNotes");
        }
    }
}
