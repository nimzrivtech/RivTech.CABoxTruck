using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class NEWCABT970_UpdateFormsSorting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlScripts.Update_FormsSorting);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
