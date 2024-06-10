using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class ProducerSLNumberUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[SLNumber] SET [ProducerSLNumber] = '3203160' WHERE [StateCode] = 'VT'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[SLNumber] SET [ProducerSLNumber] = '3543612' WHERE [StateCode] = 'VT'");
        }
    }
}
