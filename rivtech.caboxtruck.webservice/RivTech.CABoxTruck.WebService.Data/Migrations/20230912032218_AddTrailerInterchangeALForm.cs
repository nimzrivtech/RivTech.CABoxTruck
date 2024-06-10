using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrailerInterchangeALForm : Migration
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
            INSERT INTO [Form] VALUES('TrailerInterchangeCoverageAL','Trailer Interchange Coverage', 29, 1, 'RPCA029', 'Trailer Interchange Coverage AL.docx', 29, 0, 0, GETDATE(), 0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM [Form] WHERE Id = 'TrailerInterchangeCoverageAL'
            GO
        ";
    }
}
