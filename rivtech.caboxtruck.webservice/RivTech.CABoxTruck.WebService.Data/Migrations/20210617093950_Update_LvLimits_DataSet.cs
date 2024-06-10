using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_LvLimits_DataSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM LvLimits");
            migrationBuilder.Sql(SqlScripts.Seed_LvLimits);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
