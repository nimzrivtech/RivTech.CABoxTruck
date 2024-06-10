using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateFormRiskFormColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "RiskForm");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Form",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Form");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "RiskForm",
                type: "bit",
                nullable: true);
        }
    }
}
