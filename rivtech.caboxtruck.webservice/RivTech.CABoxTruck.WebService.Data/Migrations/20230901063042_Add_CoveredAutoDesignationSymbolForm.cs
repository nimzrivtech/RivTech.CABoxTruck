using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCoveredAutoDesignationSymbolForm : Migration
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
            INSERT INTO [Form] VALUES('CoveredAutoDesignationSymbol','Covered Auto Designation Symbol', 38, 1, 'CA 99 54 10 13', 'Covered Auto Designation Symbol.docx', 38, 0, 0, GETDATE(), 0, 0)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM [Form] WHERE Id = 'CoveredAutoDesignationSymbol'
            GO
        ";
    }
}
