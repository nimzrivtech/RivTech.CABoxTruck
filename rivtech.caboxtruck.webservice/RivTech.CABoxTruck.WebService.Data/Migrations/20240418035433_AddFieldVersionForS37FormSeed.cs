using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldVersionForS37FormSeed : Migration
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
            INSERT INTO FieldVersion (Id,FieldName,Description,EffectiveDate) VALUES(NEWID(),'S37OptionalForms','Added 4 optional forms','04-17-2024')
            INSERT INTO FieldVersion (Id,FieldName,Description,EffectiveDate) VALUES(NEWID(),'S37KORulesUpdate','Update Sprint 37 KO Rules','04-17-2024')
            UPDATE Form SET IsDefault = 1 where Id = 'DriverExclusionUnder23'
            UPDATE Form SET IsDefault = 0 where Id = 'DriverExclusion'
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM FieldVersion
            UPDATE Form SET IsDefault = 0 where Id = 'DriverExclusionUnder23'
            UPDATE Form SET IsDefault = 1 where Id = 'DriverExclusion'
            GO
        ";
    }
}
