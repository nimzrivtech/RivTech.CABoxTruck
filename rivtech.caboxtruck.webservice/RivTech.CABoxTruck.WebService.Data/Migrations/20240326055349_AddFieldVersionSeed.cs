using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldVersionSeed : Migration
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
            INSERT INTO FieldVersion (Id,FieldName,Description,EffectiveDate) VALUES(NEWID(),'ExcludeDriverFromSchedule','Update the field Exclude Driver from KO to Exclude Driver from Schedule','03-26-2024')
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM FieldVersion
            GO
        ";
    }
}
