using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_Risk_RiskDetail_Policy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "RiskDetail");

            migrationBuilder.DropColumn(
                name: "PolicySuffix",
                table: "RiskDetail");

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Risk",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicySuffix",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Risk",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PolicySuffix",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Risk");

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "RiskDetail",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicySuffix",
                table: "RiskDetail",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
