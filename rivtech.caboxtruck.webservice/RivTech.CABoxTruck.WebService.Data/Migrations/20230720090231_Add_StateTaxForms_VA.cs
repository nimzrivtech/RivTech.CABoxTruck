using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTaxFormsVA : Migration
    {
        /// <inheritdoc />
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
            INSERT INTO StateTax VALUES ('VA','SL','Main','2021-01-01 00:00:00.0000000',2.250)
        
            INSERT INTO [FeeDetails] VALUES ('VA','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','REINF','Reinstatement Fee',250.00,1)
            INSERT INTO [FeeDetails] VALUES ('VA','REINF2 ','Reinstatement Fee',500.00,1)
            GO

            INSERT INTO [Form] VALUES('VirginiaSplitUMCoverage','Virginia Split Uninsured Motorist Coverage Limits', 95, 1, 'CA 31 27 12 05', 'VA/Virginia Split Uninsured Motorist Coverage.docx', 95, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('VirginiaImportantNoticeUMCoverage','Virginia Important Notice Regarding Uninsured Motorists Coverage', 96, 1, 'IL N 144 01 17', 'VA/Virigina Important Notice Regarding Uninsured Motorist Coverage.docx', 96, 0, 0, GETDATE(), 0, 1)
            GO

            UPDATE [SLNumber] SET LicenseNumber = '138396' WHERE StateCode = 'VA' and CreditedOfficeId = 2
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM StateTax WHERE StateCode in ('VA')
            DELETE FROM FeeDetails WHERE StateCode in ('VA')
            GO

            DELETE FROM [Form] WHERE Id = 'VirginiaSplitUMCoverage'
            DELETE FROM [Form] WHERE Id = 'VirginiaImportantNoticeUMCoverage'
            GO

            UPDATE [SLNumber] SET LicenseNumber = 'N/A' WHERE StateCode = 'VA' and CreditedOfficeId = 2
            GO
        ";
    }
}
