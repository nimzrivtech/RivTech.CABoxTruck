using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class insertseedLvLimits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlScripts.Seed_LvLimits);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
