using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsTN : Migration
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
            Insert into [Form] values('TennesseeCarolinaChanges','Tennessee Changes', 82, 1, 'CA 01 46 10 13', 'TN/Tennessee Changes.docx', 82, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('TennesseeUninsuredMotoristCoverage','Tennessee Uninsured Motorist Coverage', 83, 1, 'CA 21 20 02 22', 'TN/Tennessee Uninsured Motorist Coverage.docx', 83, 0, 0, GETDATE(),0, 1)
            GO

            
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'TennesseeCarolinaChanges'
            Delete from [Form] where Id = 'TennesseeUninsuredMotoristCoverage'
            GO
        ";
    }
}
