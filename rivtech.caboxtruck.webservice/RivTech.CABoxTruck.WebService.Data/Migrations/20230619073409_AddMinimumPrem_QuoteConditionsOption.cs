using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMinimumPremQuoteConditionsOption : Migration
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
            INSERT INTO QuoteConditionsOption VALUES (14,'Policy Subject to {VAL}% Minimum Premium', 1)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM QuoteConditionsOption WHERE Id = 14
            GO
        ";
    }
}
