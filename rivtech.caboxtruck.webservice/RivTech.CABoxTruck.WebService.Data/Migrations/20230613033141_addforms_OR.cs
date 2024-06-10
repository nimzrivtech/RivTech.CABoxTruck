using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsOR : Migration
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
            Insert into [Form] values('OregonChanges','Oregon Changes', 67, 1, 'CA 01 49 10 13', 'OR/Oregon Changes.docx', 67, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('OregonPersonalInjuryProtection','Oregon Personal Injury Protection', 68, 1, 'CA 22 36 11 16', 'OR/Oregon Personal Injury Protection.docx', 68, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('OregonUninsuredMotoristCoverage','Oregon Uninsured Motorist Coverage - Bodily Injury', 69, 1, 'CA 21 05 01 16', 'OR/Oregon Uninsured Motorist Coverage.docx', 69, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'OregonChanges'
            Delete from [Form] where Id = 'OregonPersonalInjuryProtection'
            Delete from [Form] where Id = 'OregonUninsuredMotoristCoverage'
            GO
        ";
    }
}
