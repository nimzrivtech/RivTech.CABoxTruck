using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addformspdcoverage : Migration
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
            Insert into [Form] values('PhysicalDamageCoverage','Physical Damage Coverage Form', 4, 1, 'RPCA036', 'Physical Damage Coverage Form.docx', 4, 0, 0, GETDATE(),1, 1)
            GO
        ";

        const string SQL_DELETE = @"
            Delete from [Form] where Id = 'PhysicalDamageCoverage'
            GO
        ";
    }
}
