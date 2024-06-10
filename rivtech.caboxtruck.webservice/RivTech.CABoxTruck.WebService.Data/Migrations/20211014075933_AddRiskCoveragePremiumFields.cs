using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddRiskCoveragePremiumFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ALManuscriptPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PDManuscriptPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PolicyHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ALManuscriptPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "PDManuscriptPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PolicyHistory",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
