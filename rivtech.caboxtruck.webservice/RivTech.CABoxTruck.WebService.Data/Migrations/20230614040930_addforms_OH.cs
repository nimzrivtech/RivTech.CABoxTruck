using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformsOH : Migration
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
            Insert into [Form] values('OhioChanges','Ohio Changes', 70, 1, 'CA 01 05 12 98', 'OH/Ohio Changes.docx', 70, 0, 0, GETDATE(),1, 1)
            Insert into [Form] values('OhioUMandUIMCoverage','Ohio Uninsured and Underinsured Motorists Coverage - Bodily Injury ', 71, 1, 'CA 21 33 08 17', 'OH/Ohio UM and UIM Coverage.docx', 71, 0, 0, GETDATE(),0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'OhioChanges'
            Delete from [Form] where Id = 'OhioUMandUIMCoverage'
            GO
        ";
    }
}
