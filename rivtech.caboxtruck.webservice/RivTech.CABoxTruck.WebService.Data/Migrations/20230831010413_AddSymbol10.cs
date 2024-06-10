using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSymbol10 : Migration
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
            INSERT INTO Symbols VALUES (14, '10 - Use 2, 8 & 9 Pricing', 1);
            INSERT INTO Symbols VALUES (15, '10 - Use 7, 8 & 9 Pricing', 1);
        ";

        const string SQL_DELETE = @"
            DELETE FROM Symbols WHERE Id IN (14,15);
        ";
    }
}
