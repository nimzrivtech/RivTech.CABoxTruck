using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsmo : Migration
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
            Insert into [Form] values('MissouriChanges','Missouri Changes', 54, 1, 'CA 01 65 07 16', 'MO/Missouri Changes.docx', 54, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('MissouriUninsuredMotoristsCoverage','Missouri Uninsured Motorists Coverage', 55, 1, 'CA 21 04 10 13', 'MO/Missouri Uninsured Motorist Coverage.docx', 55, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('MissouriUnderinsuredMotoristsCoverage','Missouri Underinsured Motorists Coverage', 56, 1, 'CA 31 04 02 18', 'MO/Missouri Underinsured Motorist Coverage.docx', 56, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'MissouriChanges'
            Delete from [Form] where Id = 'MissouriUninsuredMotoristsCoverage'
            Delete from [Form] where Id = 'MissouriUnderinsuredMotoristsCoverage'
            GO
        ";
    }
}
