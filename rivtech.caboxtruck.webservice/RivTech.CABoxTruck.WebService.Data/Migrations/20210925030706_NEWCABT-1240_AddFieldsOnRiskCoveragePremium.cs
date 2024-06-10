using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class NEWCABT1240_AddFieldsOnRiskCoveragePremium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HiredPhysicalDamage",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TrailerInterchange",
                table: "RiskCoveragePremium",
                type: "decimal(12,2)",
                nullable: true);

            // include new NotesCategoryOption
            migrationBuilder.Sql(INSERT_NEW_NOTECATEGORY_OPTION);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiredPhysicalDamage",
                table: "RiskCoveragePremium");

            migrationBuilder.DropColumn(
                name: "TrailerInterchange",
                table: "RiskCoveragePremium");
        }

        const string INSERT_NEW_NOTECATEGORY_OPTION = @"
            INSERT INTO [dbo].[NoteCategoryOption] ([Id], [Description], [IsActive]) VALUES (8, 'OFAC', 1);
        ";
    }
}
