using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addquoteconditionoptionssymbol10 : Migration
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
            INSERT INTO QuoteConditionsOption VALUES (23,'For symbols 2/8/9 to be effective, we need an executed contract indicating the required symbols', 1)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM QuoteConditionsOption WHERE Id = 23
            GO
        ";
    }
}
