using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateFormsSorting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlScripts.Update_FormsSorting2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
