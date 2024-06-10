using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuoteConditionOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            update QuoteConditionsOption set IsActive = 0 where Id in (1,2,3)
            GO
        ";

        const string SQL_DELETE = @"
            update QuoteConditionsOption set IsActive = 1 where Id in (1,2,3)
            GO
        ";
    }
}
