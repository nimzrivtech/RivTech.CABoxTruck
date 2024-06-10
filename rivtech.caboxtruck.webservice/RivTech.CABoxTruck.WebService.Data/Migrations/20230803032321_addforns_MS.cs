using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addfornsMS : Migration
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
            Insert into [Form] values('MississippiUninsuredMotoristCoverageNS','Mississippi Uninsured Motorist Coverage, Bodily Injury & Property Damage - Non Stacked', 88, 1, 'CA 21 80 01 21', 'MS/Mississippi Uninsured Motorist Coverage - Nonstacked.docx', 88, 0, 0, GETDATE(),0, 1)
            GO

            
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'MississippiUninsuredMotoristCoverageNS'
            GO
        ";
    }
}
