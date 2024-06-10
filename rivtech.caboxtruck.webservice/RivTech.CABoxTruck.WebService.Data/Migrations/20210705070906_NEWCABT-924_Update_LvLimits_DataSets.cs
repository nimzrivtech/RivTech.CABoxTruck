using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class NEWCABT924_Update_LvLimits_DataSets : Migration
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
