using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_um_forms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_REVERT);
        }

        const string SQL_UPDATE = @"
            UPDATE [Form]
		    SET Name = 'California Uninsured Motorist Coverage Bodily Injury' , FormNumber = 'CA 21 54 11 16'
		    WHERE Id = 'UninsuredMotoristCoverageBodilyInjury';

            Insert into [Form] values('SplitBodilyInjuryUninsuredMotoristCoverage','Split Bodily Injury Uninsured Motorist Coverage', 40, 1, 'CA 21 02 11 06', 'Split Bodily Injury Uninsured Motorist Coverage.docx', 40, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_REVERT = @"
            UPDATE [Form]
		    SET Name = 'Uninsured Motorist Coverage Bodily Injury' , FormNumber = NULL
		    WHERE Id = 'UninsuredMotoristCoverageBodilyInjury';
            
            DELETE FROM [Form] where Id = 'SplitBodilyInjuryUninsuredMotoristCoverage'
            GO
        ";
    }
}
