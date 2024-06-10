using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Insert_Question_SafetyDeviceData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(INSERT_QUESTION_DATA);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_QUESTION_DATA);
        }

        const string INSERT_QUESTION_DATA = @"
            insert into dbo.LvQuestionSection ([Description], IsActive)
            values ('Safety Device', 1);

            declare @effectiveDate datetime = '2020-01-01',
	            @expirationDate datetime = '2030-01-01',
	            @currentDate datetime = getdate(),
	            --category id
	            @riskSpecificCatId smallint = (select s.Id from dbo.LvQuestionCategory s where s.[Description] = 'Risk specific'),
	            --section id
	            @maintenanceQuestionSecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'Safety Device');	

            insert into dbo.Question (EffectiveDate, ExpirationDate, Categoryid, SectionId, CreatedAt, LastUpdatedAt, [description])
            values(@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'safetyDeviceCategory');
        ";

        const string DELETE_QUESTION_DATA = @"            
            delete dbo.LvQuestionSection
            where [Description] = 'Safety Device';

            delete dbo.Question
            where [Description] = 'safetyDeviceCategory';
        ";
    }
}
