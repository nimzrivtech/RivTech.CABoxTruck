using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsFL : Migration
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
            Insert into [Form] values('FloridaChanges','Florida Changes', 64, 1, 'CA 01 28 01 21', 'FL/Florida Changes.docx', 64, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('FloridaPersonalInjuryProtection','Florida Personal Injury Protection', 65, 1, 'CA 22 10 01 21', 'FL/Florida Personal Injury Protection Coverage.docx', 65, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('FloridaUninsuredMotoristsCoverageNS','Florida Uninsured Motorists Coverage - Nonstacked', 66, 1, 'CA 21 72 06 17', 'FL/Florida UM Coverage Form - Nonstacked.docx', 66, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'FloridaChanges'
            Delete from [Form] where Id = 'FloridaPersonalInjuryProtection'
            Delete from [Form] where Id = 'FloridaUninsuredMotoristsCoverageNS'
            GO
        ";
    }
}
