using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTaxFormsAZ : Migration
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
            INSERT INTO StateTax VALUES ('AZ','SL','Main','2021-01-01 00:00:00.0000000',3.000)
            INSERT INTO StateTax VALUES ('AZ','SF','P','2021-01-01 00:00:00.0000000',0.200)
        
            INSERT INTO [FeeDetails] VALUES ('AZ','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('AZ','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('AZ','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('AZ','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('AZ','REINF','Reinstatement Fee',250.00,1)
            INSERT INTO [FeeDetails] VALUES ('AZ','REINF2 ','Reinstatement Fee',500.00,1)
            GO

            INSERT INTO [Form] VALUES('ArizonaChanges','Arizona Changes', 79, 1, 'CA 01 75 10 13', 'AZ/Arizona Changes.docx', 79, 0, 0, GETDATE(), 1, 1)
            INSERT INTO [Form] VALUES('ArizonaUIMCoverage','Arizona Underinsured Motorist Coverage', 80, 1, 'CA 21 40 10 13', 'AZ/Arizona Underinsured Motorist Coverage.docx', 80, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('ArizonaUMCoverage','Arizona Uninsured Motorist Coverage', 81, 1, 'CA 21 39 10 13', 'AZ/Arizona Uninsured Motorist Coverage.docx', 81, 0, 0, GETDATE(), 0, 1)
            GO
            
            UPDATE [SLNumber] SET LicenseNumber = '1800013867' WHERE StateCode = 'AZ' and CreditedOfficeId = 2
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM StateTax WHERE StateCode in ('AZ')
            DELETE FROM FeeDetails WHERE StateCode in ('AZ')
            GO

            DELETE FROM [Form] WHERE Id = 'ArizonaChanges'
            DELETE FROM [Form] WHERE Id = 'ArizonaUIMCoverage'
            DELETE FROM [Form] WHERE Id = 'ArizonaUMCoverage'
            GO
            
            UPDATE [SLNumber] SET LicenseNumber = 'N/A' WHERE StateCode = 'AZ' and CreditedOfficeId = 2
            GO
        ";
    }
}
