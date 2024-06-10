using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class GetDashboardCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create or ALTER     PROCEDURE [dbo].[GetDashboardCount]
                                    AS
                                    BEGIN
                                    SET NOCOUNT ON;

	                                    with cteSubmission as
	                                    (
		                                    select RiskDetail.Status SubmissionStatus,
		                                    case when Risk.Status is null then 'Pending' else Risk.Status end PolicyStatus from Risk
		                                    cross apply (select top 1 RiskDetail.Status from RiskDetail where RiskDetail.RiskId = Risk.Id order by RiskDetail.UpdatedDate desc) RiskDetail
	                                    )
	                                    select SubmissionStatus as Status,
	                                    count(SubmissionStatus) [Count] from cteSubmission
	                                    group by PolicyStatus, SubmissionStatus

	                                    union all 
	                                    select 'Submission', COUNT(*) from cteSubmission
	                                    where PolicyStatus = 'Pending'

	                                    union all 
	                                    select 'Active', COUNT(*) from cteSubmission
	                                    where PolicyStatus IN ('InForce', 'Active')
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("if OBJECT_ID('GetDashboardCount') is not null drop procedure [GetDashboardCount]");
        }
    }
}
