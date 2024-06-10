using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class RiskRaterVersionId : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaterVersionId",
                table: "Risk",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(SET_RATER_VERSION_IDS);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaterVersionId",
                table: "Risk");
        }


        const string SET_RATER_VERSION_IDS = @"
            update  r set 
                r.RaterVersionId = case 
                    when bi.EffectiveDate >= '2022-10-07' then 3
                    when bi.EffectiveDate >= '2022-02-15' then 2
                    when bi.EffectiveDate >= '2021-01-01' then 1
                end 
            from Risk r 
                join BrokerInfo bi on bi.RiskId = r.Id
        ";
    }
}
