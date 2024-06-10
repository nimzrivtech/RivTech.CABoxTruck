using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQouteConditionDescribe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_REVERT);
        }

        const string SQL_UPDATE = @"
            ALTER TABLE QuoteConditions
            ALTER COLUMN Describe nvarchar(MAX)
        ";

        const string SQL_REVERT = @"
            ALTER TABLE QuoteConditions
            ALTER COLUMN Describe nvarchar(300)
        ";
    }
}
