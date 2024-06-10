using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UWQuestionsNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EquipmentsOperatedUnderAuthorityExplanation",
                table: "RiskSpecificUnderwritingQuestion",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkersCompensationProvidedExplanation",
                table: "RiskSpecificUnderwritingQuestion",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentsOperatedUnderAuthorityExplanation",
                table: "RiskSpecificUnderwritingQuestion");

            migrationBuilder.DropColumn(
                name: "WorkersCompensationProvidedExplanation",
                table: "RiskSpecificUnderwritingQuestion");
        }
    }
}
