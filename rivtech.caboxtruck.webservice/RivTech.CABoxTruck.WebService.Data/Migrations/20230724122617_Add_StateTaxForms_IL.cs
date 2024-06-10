using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTaxFormsIL : Migration
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
            INSERT INTO StateTax VALUES ('IL','SL','Main','2021-01-01 00:00:00.0000000',3.500)
            INSERT INTO StateTax VALUES ('IL','SF','P','2021-01-01 00:00:00.0000000',0.040)

            INSERT INTO NonTaxableFee VALUES ('IL', 'INSTF', '2021-01-01 00:00:00.0000000')
            INSERT INTO NonTaxableFee VALUES ('IL', 'REINF', '2021-01-01 00:00:00.0000000')
            INSERT INTO NonTaxableFee VALUES ('IL', 'RMFAL', '2021-01-01 00:00:00.0000000')
            INSERT INTO NonTaxableFee VALUES ('IL', 'RMFPD', '2021-01-01 00:00:00.0000000')
        
            INSERT INTO [FeeDetails] VALUES ('IL','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','REINF','Reinstatement Fee',250.00,1)
            INSERT INTO [FeeDetails] VALUES ('IL','REINF2 ','Reinstatement Fee',500.00,1)
            GO

            INSERT INTO [Form] VALUES('IllinoisChanges','Illinois Changes', 84, 1, 'CA 01 20 01 15', 'IL/Illinois Changes.docx', 84, 0, 0, GETDATE(), 1, 1)
            INSERT INTO [Form] VALUES('IllinoisUMCoverage','Illinois Uninsured Motorist Coverage', 85, 1, 'CA 21 30 01 15', 'IL/Illinois Uninsured Motorist Coverage.docx', 85, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('IllinoisUMPDCoverage','Illinois Uninsured Motorist Coverage - Property Damage', 86, 1, 'CA 21 53 10 13', 'IL/Illinois Uninsured Motorist Property Damage Coverage.docx', 86, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('IllinoisUIMCoverage','Illinois Underinsured Motorist Coverage', 87, 1, 'CA 21 38 10 13', 'IL/Illinois Underinsured Motorist Coverage.docx', 87, 0, 0, GETDATE(), 0, 1)
            GO

            UPDATE [SLNumber] SET ProducerSLNumber = '8742354' WHERE StateCode = 'IL' and CreditedOfficeId IN (1,2,3)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM StateTax WHERE StateCode in ('IL')
            DELETE FROM FeeDetails WHERE StateCode in ('IL')
            GO

            DELETE FROM [Form] WHERE Id = 'IllinoisChanges'
            DELETE FROM [Form] WHERE Id = 'IllinoisUMCoverage'
            DELETE FROM [Form] WHERE Id = 'IllinoisUMPDCoverage'
            DELETE FROM [Form] WHERE Id = 'IllinoisUIMCoverage'
            GO

            UPDATE [SLNumber] SET ProducerSLNumber = '8653594' WHERE StateCode = 'IL' and CreditedOfficeId IN (1,2,3)
            GO
        ";
    }
}
