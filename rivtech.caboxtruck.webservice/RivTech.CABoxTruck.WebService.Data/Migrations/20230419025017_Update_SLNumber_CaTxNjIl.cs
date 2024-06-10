using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSLNumberCaTxNjIl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_REVERT);

        }

        const string SQL_UPDATE = @"

            IF EXISTS(SELECT StateCode FROM SLNumber WHERE  (StateCode = 'CA'))
            BEGIN
            UPDATE SLNumber
            SET          LicenseNumber = '0K57443'
            WHERE  (StateCode = 'CA')
            END

            IF EXISTS(SELECT StateCode FROM SLNumber WHERE  (StateCode = 'NJ'))
            BEGIN
            UPDATE SLNumber
            SET          LicenseNumber = '1578697'
            WHERE  (StateCode = 'NJ')
            END

            IF EXISTS(SELECT StateCode FROM SLNumber WHERE  (StateCode = 'TX'))
            BEGIN
            UPDATE SLNumber
            SET          LicenseNumber = '2041189'
            WHERE  (StateCode = 'TX')
            END

        ";

        const string SQL_REVERT = @"

            IF EXISTS(SELECT StateCode FROM SLNumber WHERE  (StateCode = 'CA'))
            BEGIN
            UPDATE SLNumber
            SET          LicenseNumber = 'N/A'
            WHERE  (StateCode = 'CA') and CreditedOfficeId = 1
            END

            IF EXISTS(SELECT StateCode FROM SLNumber WHERE  (StateCode = 'NJ'))
            BEGIN
            UPDATE SLNumber
            SET         LicenseNumber = 'N/A'
            WHERE  (StateCode = 'NJ') and CreditedOfficeId = 2
            END

            IF EXISTS(SELECT StateCode FROM SLNumber WHERE  (StateCode = 'TX'))
            BEGIN
            UPDATE SLNumber
            SET          LicenseNumber = 'N/A'
            WHERE  (StateCode = 'TX') and CreditedOfficeId = 1
            END
        ";
    }
}
