using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddPaymentDetailCommissionExternalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ScriptUtils.RunScriptWithAzureChecking(SqlScripts.Create_BillingAgency_ExternalTable));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
