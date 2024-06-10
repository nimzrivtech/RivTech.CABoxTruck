using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTaxFormsCO : Migration
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
            INSERT INTO StateTax VALUES ('CO','SL','Main','2021-01-01 00:00:00.0000000',3.000)
        
            INSERT INTO [FeeDetails] VALUES ('CO','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','REINF','Reinstatement Fee',250.00,1)
            INSERT INTO [FeeDetails] VALUES ('CO','REINF2 ','Reinstatement Fee',500.00,1)
            GO

            INSERT INTO [Form] VALUES('ColoradoChanges','Colorado Changes', 72, 1, 'CA 01 13 10 13', 'CO/Colorado Changes.docx', 72, 0, 0, GETDATE(), 1, 1)
            INSERT INTO [Form] VALUES('ColoradoUMBICoverage','Colorado Uninsured Motorist Coverage - Bodily Injury', 73, 1, 'CA 21 50 07 17', 'CO/Colorado Uninsured Motorist Bodily Injury Coverage.docx', 73, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('ColoradoUMPDCoverage','Colorado Uninsured Motorist Coverage - Property Damage', 74, 1, 'CA 21 26 10 13', 'CO/Colorado Uninsured Motorist Property Damage Coverage.docx', 74, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('ColoradoMedicalPaymentCoverage','Colorado Auto Medical Payments Coverage', 75, 1, 'CA 04 40 10 13', 'CO/Colorado Auto Medical Payments Coverage.docx', 75, 0, 0, GETDATE(), 0, 1)
            GO
            
            UPDATE [SLNumber] SET LicenseNumber = '482792' WHERE StateCode = 'CO' and CreditedOfficeId = 2
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM StateTax WHERE StateCode in ('CO')
            DELETE FROM FeeDetails WHERE StateCode in ('CO')
            GO

            DELETE FROM [Form] WHERE Id = 'ColoradoChanges'
            DELETE FROM [Form] WHERE Id = 'ColoradoUMBICoverage'
            DELETE FROM [Form] WHERE Id = 'ColoradoUMPDCoverage'
            DELETE FROM [Form] WHERE Id = 'ColoradoMedicalPaymentCoverage'
            GO
            
            UPDATE [SLNumber] SET LicenseNumber = 'N/A' WHERE StateCode = 'CO' and CreditedOfficeId = 2
            GO
        ";
    }
}
