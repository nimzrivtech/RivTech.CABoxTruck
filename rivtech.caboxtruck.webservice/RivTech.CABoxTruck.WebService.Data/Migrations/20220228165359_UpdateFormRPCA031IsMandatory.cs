using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdateFormRPCA031IsMandatory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Form SET IsMandatory = 1 WHERE FormNumber = 'RPCA031'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Form SET IsMandatory = 0 WHERE FormNumber = 'RPCA031'");
        }
    }
}
