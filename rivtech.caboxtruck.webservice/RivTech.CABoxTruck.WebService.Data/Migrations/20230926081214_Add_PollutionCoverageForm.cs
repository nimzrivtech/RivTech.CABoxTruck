using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPollutionCoverageForm : Migration
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
            INSERT INTO [Form] VALUES('PollutionCoverage','Pollution Coverage', 38, 1, 'CA 99 48 10 13', 'Pollution Coverage.docx', 38, 0, 0, GETDATE(), 0, 1)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM [Form] WHERE Id = 'PollutionCoverage'
            GO
        ";
    }
}
