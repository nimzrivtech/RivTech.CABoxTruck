using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class NEWCABT1078_AgentCommentField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "QuoteConditions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentComments",
                table: "BindingRequirements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "BindingRequirements",
                type: "bit",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE [dbo].[BindingRequirements] SET [IsDefault] = 1 WHERE [BindRequirementsOptionId] IN (2,3,4,5,6)");
            migrationBuilder.Sql(@"UPDATE [dbo].[QuoteConditions] SET [IsDefault] = 1 WHERE [QuoteConditionsOptionId] <> 10");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "QuoteConditions");

            migrationBuilder.DropColumn(
                name: "AgentComments",
                table: "BindingRequirements");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "BindingRequirements");
        }
    }
}
