using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTaxFormsSC : Migration
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
            INSERT INTO StateTax VALUES ('SC','SL','Main','2021-01-01 00:00:00.0000000',6.000)
        
            INSERT INTO [FeeDetails] VALUES ('SC','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','REINF','Reinstatement Fee',250.00,1)
            INSERT INTO [FeeDetails] VALUES ('SC','REINF2 ','Reinstatement Fee',500.00,1)
            GO

            INSERT INTO [Form] VALUES('SouthCarolinaChanges','South Carolina Changes', 90, 1, 'CA 01 50 04 22', 'SC/South Carolina Changes.docx', 90, 0, 0, GETDATE(), 1, 1)
            INSERT INTO [Form] VALUES('SouthCarolinaSplitUMCoverage','South Carolina Split Uninsured Motorist Coverage Limits', 91, 1, 'CA 21 89 12 13', 'SC/South Carolina Split Uninsured Motorist Coverage.docx', 91, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('SouthCarolinaSplitUIMCoverage','South Carolina Split Underinsured Motorist Coverage Limits', 92, 1, 'CA 21 90 12 13', 'SC/South Carolina Split Underinsured Motorist Coverage.docx', 92, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('SouthCarolinaUMCoverage','South Carolina Uninsured Motorist Coverage', 93, 1, 'CA 21 19 12 13', 'SC/South Carolina Uninsured Motorist Coverage.docx', 93, 0, 0, GETDATE(), 0, 1)
            INSERT INTO [Form] VALUES('SouthCarolinaUIMCoverage','South Carolina Underinsured Motorist Coverage', 94, 1, 'CA 21 88 12 13', 'SC/South Carolina Underinsured Motorist Coverage.docx', 94, 0, 0, GETDATE(), 0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM StateTax WHERE StateCode in ('SC')
            DELETE FROM FeeDetails WHERE StateCode in ('SC')
            GO

            DELETE FROM [Form] WHERE Id = 'SouthCarolinaChanges'
            DELETE FROM [Form] WHERE Id = 'SouthCarolinaSplitUMCoverage'
            DELETE FROM [Form] WHERE Id = 'SouthCarolinaSplitUIMCoverage'
            DELETE FROM [Form] WHERE Id = 'SouthCarolinaUMCoverage'
            DELETE FROM [Form] WHERE Id = 'SouthCarolinaUIMCoverage'
            GO
        ";
    }
}
