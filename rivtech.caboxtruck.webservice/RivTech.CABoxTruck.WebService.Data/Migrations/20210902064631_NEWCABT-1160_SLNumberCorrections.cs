using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class NEWCABT1160_SLNumberCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProducerSLNumber",
                table: "SLNumber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProducerSLNumber",
                table: "Binding",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql("DELETE FROM [dbo].[SLNumber]");
            migrationBuilder.Sql(SqlScripts.SLNumberCorrections);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProducerSLNumber",
                table: "SLNumber");

            migrationBuilder.DropColumn(
                name: "ProducerSLNumber",
                table: "Binding");
        }
    }
}
