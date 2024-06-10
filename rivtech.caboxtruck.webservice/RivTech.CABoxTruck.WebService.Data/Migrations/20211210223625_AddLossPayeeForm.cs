using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddLossPayeeForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlText);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        private static string SqlText =>
            @"INSERT INTO [dbo].[Form] ([Id], [Name] ,[SortOrder],[IsActive],[FormNumber] ,[FileName],[DecPageOrder],[IsSupplementalDocument],[CreatedBy],[CreatedDate],[IsMandatory],[IsDefault])
            VALUES('LossPayeeEndorsement', 'Loss Payee Endorsement', 36, 1, 'RPCA032', 'Loss Payee Endorsement.docx', 36, 0, 0, GETDATE(), 0, 0)";
    }
}
