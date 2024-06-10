using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class add_UM_docs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string SQL_UPDATE = @"
            Insert into[Form] values('UninsuredMotoristCoverageBodilyInjury','Uninsured Motorist Coverage Bodily Injury', 38, 1, NULL, 'California Uninsured Motorist Coverage_Bodily Injury.docx', 38, 0, 0, GETDATE(),0, 1)
            GO
        ";

        //Insert into[Form] values('CaliforniaUninsuredMotoristCoverageBodilyInjury','California Uninsured Motorist Coverage Bodily Injury', 38, 0, 'CA 21 54 11 16', 'California Uninsured Motorist Coverage_Bodily Injury.docx', 38, 0, 0, GETDATE(),0, 1)
        //Insert into[Form] values('SplitBodilyInjuryUninsuredMotoristCoverage','Split Bodily Injury Uninsured Motorist Coverage', 38, 0, 'CA 21 02 11 06', 'Split Bodily Injury Uninsured Motorist Coverage.docx', 38, 0, 0, GETDATE(),0, 1)
    }
}
