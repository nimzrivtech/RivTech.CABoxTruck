using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class EnableMTCEndorsement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Form SET IsActive = 1 WHERE Id = 'MotorTruckCargoEndorsement'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
