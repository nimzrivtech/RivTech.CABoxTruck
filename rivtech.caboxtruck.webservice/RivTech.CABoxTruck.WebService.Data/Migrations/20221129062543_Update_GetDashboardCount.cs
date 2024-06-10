using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_GetDashboardCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create or ALTER PROCEDURE [dbo].[GetDashboardCount]
                                    AS
                                    BEGIN
									SET NOCOUNT ON;

										with cteSubmission as
										(
											select RiskDetail.Status SubmissionStatus,
											case when Risk.Status is null then 'Pending' else Risk.Status end PolicyStatus,
											Risk.IsRenewal
											from Risk
											cross apply (select top 1 RiskDetail.Status from RiskDetail where RiskDetail.RiskId = Risk.Id order by RiskDetail.UpdatedDate desc) RiskDetail
										)
										select SubmissionStatus as Status,
										count(SubmissionStatus) [Count],
										IsRenewal from cteSubmission
										group by PolicyStatus, SubmissionStatus, IsRenewal

										union all 
										select 'Submission', COUNT(*), IsRenewal from cteSubmission
										where PolicyStatus = 'Pending'
										group by PolicyStatus, IsRenewal

										union all 
										select 'Active', COUNT(*), IsRenewal from cteSubmission
										where PolicyStatus IN ('InForce', 'Active')
										group by PolicyStatus, IsRenewal
									END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("if OBJECT_ID('GetDashboardCount') is not null drop procedure [GetDashboardCount]");
        }
    }
}
