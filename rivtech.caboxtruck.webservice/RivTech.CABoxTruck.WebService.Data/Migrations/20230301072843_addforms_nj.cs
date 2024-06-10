using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsnj : Migration
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
            Insert into [Form] values('NewJerseyUMandUIMCoverage','New Jersey Uninsured and Underinsured Motorist Coverage', 46, 1, 'CA 21 14 10 16', 'NJ/New Jersey UM and UIM Coverage.docx', 46, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('NewJerseyChanges','New Jersey Changes', 48, 1, 'CA 01 88 10 13', 'NJ/New Jersey Changes.docx', 48, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('NewJerseyPersonalInjuryProtection','New Jersey Personal Injury Protection', 49, 1, 'CA 22 30 01 20', 'NJ/New Jersey Personal Injury Protection.docx', 49, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('NewJerseySplitUninsuredandUnderinsuredMotoristCoverage','New Jersey Split Uninsured/Underinsured Motorist Coverage Limits', 50, 1, 'CA 21 77 10 13', 'NJ/New Jersey Split UM and UIM Coverage.docx', 50, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'NewJerseyUMandUIMCoverage'
            Delete from [Form] where Id = 'NewJerseyChanges'
            Delete from [Form] where Id = 'NewJerseyPersonalInjuryProtection'
            Delete from [Form] where Id = 'NewJerseySplitUninsuredandUnderinsuredMotoristCoverage'
            GO
        ";
    }
}
