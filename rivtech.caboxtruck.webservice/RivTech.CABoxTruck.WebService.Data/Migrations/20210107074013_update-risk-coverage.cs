using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class updateriskcoverage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "FireDeductibleId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UIMPDLimitId",
                table: "RiskCoverage",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FireDeductibleId",
                table: "RiskCoverage");

            migrationBuilder.DropColumn(
                name: "UIMPDLimitId",
                table: "RiskCoverage");
        }
    }
}
