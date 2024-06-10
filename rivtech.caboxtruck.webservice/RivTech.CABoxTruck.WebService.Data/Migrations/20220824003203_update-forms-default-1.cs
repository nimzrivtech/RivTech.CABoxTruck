using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class updateformsdefault1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_FORMS_GL);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_FORMS_GL = @"
            UPDATE FORM SET IsDefault = 1 where Id = 'GeneralLiabilityEndorsement'
        ";
    }
}
