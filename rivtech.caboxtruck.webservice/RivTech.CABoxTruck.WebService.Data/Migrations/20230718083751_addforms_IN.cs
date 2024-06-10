using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsIN : Migration
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
            Insert into [Form] values('IndianaChanges','Indiana Changes', 76, 1, 'CA 01 19 10 13', 'IN/Indiana Changes.docx', 76, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('IndianaUninsuredMotoristCoverage','Indiana Uninsured Motorist Coverage', 77, 1, 'CA 21 44 12 15', 'IN/Indiana Uninsured Motorist Coverage.docx', 77, 0, 0, GETDATE(),0, 1)
            Insert into [Form] values('IndianaUnderinsuredMotoristCoverage','Indiana Underinsured Motorist Coverage', 78, 1, 'CA 31 16 12 15', 'IN/Indiana Underinsured Motorist Coverage.docx', 78, 0, 0, GETDATE(),0, 1)
            GO

            
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'IndianaChanges'
            Delete from [Form] where Id = 'IndianaUninsuredMotoristCoverage'
            Delete from [Form] where Id = 'IndianaUnderinsuredMotoristCoverage'
            GO
        ";
    }
}
