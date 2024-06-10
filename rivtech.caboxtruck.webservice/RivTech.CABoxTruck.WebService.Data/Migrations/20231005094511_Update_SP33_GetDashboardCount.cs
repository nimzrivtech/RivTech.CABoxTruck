using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSP33GetDashboardCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_DashboardCount);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(REVERT_DashboardCount);
        }

        const string UPDATE_DashboardCount = @"
				ALTER   PROCEDURE [dbo].[GetDashboardCount]
					  AS
					  BEGIN
				SET NOCOUNT ON;
					with cteSubmission as
					(
						SELECT 		
								[Status],
						IsRenewal
								FROM
								(
									-- Policies
									SELECT 
										r.SubmissionNumber 'SubmissionNumber',
										r.[Status] 'Status',
										CASE WHEN r.[Status] ='Active'  OR r.[Status] ='Canceled' THEN CAST(0 AS bit) ELSE   r.IsRenewal  END 'IsRenewal'
									FROM RiskDetail rd
									INNER JOIN Risk r ON rd.Id = r.ForEndorsementRiskDetailId
							UNION
									-- Submissions
									SELECT 
										r.SubmissionNumber 'SubmissionNumber',
										rd.[Status] 'Status',
										r.IsRenewal   'IsRenewal' 
									FROM RiskDetail rd
									INNER JOIN Risk r ON rd.RiskId = r.Id AND r.[Status] = 'Pending'	
								) a
					)
					select [Status],
					count([Status]) [Count],
					IsRenewal 
					from cteSubmission
					group by [Status], IsRenewal

	
				END
				  ";
						const string REVERT_DashboardCount = @"
				ALTER   PROCEDURE [dbo].[GetDashboardCount]
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
				END
				  ";

    }
}
