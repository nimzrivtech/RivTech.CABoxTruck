using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateSLnumberOROH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            update SLNumber set LicenseNumber = '100272853' where StateCode = 'OR' and CreditedOfficeId = 2
            update SLNumber set LicenseNumber = '1084350' where StateCode = 'OH' and CreditedOfficeId = 2

            GO
        ";

        const string SQL_DELETE = @"
            update SLNumber set LicenseNumber = 'N/A' where StateCode = 'OR' and CreditedOfficeId = 2
            update SLNumber set LicenseNumber = 'N/A' where StateCode = 'OH' and CreditedOfficeId = 2

            GO
        ";
    }
}
