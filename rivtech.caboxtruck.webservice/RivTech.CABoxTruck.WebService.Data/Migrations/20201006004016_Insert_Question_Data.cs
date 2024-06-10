using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Insert_Question_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(INSERT_QUESTIONLV_DATA);       
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_QUESTIONLV_DATA);
        }

        const string INSERT_QUESTIONLV_DATA = @"
            insert into dbo.LvQuestionCategory ([Description], IsActive)
            values ('Coverage', 1),
            ('Risk specific', 1);

            insert dbo.LvQuestionSection ([Description], IsActive)
            values ('Requested Coverage', 1),
            ('Historical Coverage', 1),
            ('Coverage Question', 1),
            ('General Liability Coverage', 1),
            ('Cargo Coverage', 1),
            ('General Information', 1),
            ('Driver Information', 1),
            ('Driver Hiring Criteria', 1),
            ('Maintenance Question', 1),
            ('Underwriting Question', 1);

            insert dbo.LvQuestionType ([Description], IsActive)
            values ('Date', 1),
            ('Text input', 1),
            ('Text area', 1),
            ('Select dropdown', 1),
            ('Multi-select dropdown', 1),
            ('Switch', 1),
            ('Radiobutton', 1),
            ('Checkbox', 1);

            insert dbo.LvRiskResponseType ([Description], IsActive)
            values ('boolean', 1),
            ('int', 1),
            ('decimal', 1),
            ('string', 1),
            ('datetime', 1);
        ";

        const string DELETE_QUESTIONLV_DATA = @"
            delete dbo.LvQuestionCategory;
            delete dbo.LvQuestionSection;
            delete dbo.LvQuestionType;
            delete dbo.LvRiskResponseType;
        ";
    }
}
