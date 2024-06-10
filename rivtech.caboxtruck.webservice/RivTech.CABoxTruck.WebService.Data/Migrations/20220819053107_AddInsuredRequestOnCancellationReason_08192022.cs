using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddInsuredRequestOnCancellationReason_08192022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_CANCELLATION_REASON);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_CANCELLATION_REASON = @"
            INSERT INTO [dbo].[CancellationReasons] ([Id], [Description], [IsActive]) VALUES(9, 'Insured Request', 1)
        ";
    }
}
