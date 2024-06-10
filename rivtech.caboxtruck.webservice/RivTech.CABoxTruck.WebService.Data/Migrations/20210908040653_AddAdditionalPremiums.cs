using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddAdditionalPremiums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ComprehensivePremium",
                table: "Vehicle",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RiskManuscripts",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CollisionPremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ComprehensivePremium",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComprehensivePremium",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "CollisionPremium",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "ComprehensivePremium",
                table: "RiskCoveragePremium");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RiskManuscripts",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
