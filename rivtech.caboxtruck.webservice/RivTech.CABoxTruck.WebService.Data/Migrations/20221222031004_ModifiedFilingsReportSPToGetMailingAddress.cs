using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class ModifiedFilingsReportSPToGetMailingAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_GenerateFilingsReport);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(REVERT_GenerateFilingsReport);
        }

        const string UPDATE_GenerateFilingsReport = @"
            CREATE OR ALTER   PROCEDURE[dbo].[GenerateFilingsReport]
			AS
			BEGIN

				SET NOCOUNT ON;

				SELECT 
					rpd.*,
					CASE WHEN billing.PastDueAmount IS NULL THEN '' ELSE billing.PastDueAmount END AS 'PastDueAmount',
					CASE WHEN ISNULL(billing.PastDueDate, '') = '' THEN '' ELSE CONVERT(VARCHAR, billing.PastDueDate) END AS 'PastDueDate'
				FROM
				(
					SELECT 
						r.Id,
						r.PolicyNumber, 
						e.CompanyName 'InsuredName', 
						CASE WHEN rd.IsFilingRequired = 1 THEN 'Yes' ELSE 'No' END AS 'Filings',
						CONCAT(COALESCE(a.StreetAddress1 + ' ', ''),
								COALESCE(a.StreetAddress2 + ' ', ''),
								COALESCE(a.City + ' ',''),
								COALESCE(a.[State] + ' ', ''),
								COALESCE(a.ZipCode, '')) 'MailingAddress',
						bi.EffectiveDate 'PolicyEffectiveDate',
						bi.ExpirationDate 'PolicyExpirationDate',
						CASE WHEN pt.[Description] IS NULL THEN '' ELSE pt.[Description] END AS 'PaymentPlan',
						CASE WHEN ae3.FullName IS NULL THEN '' ELSE ae3.FullName END AS 'AgentName',
						CASE WHEN ae3.WorkPhone IS NULL THEN '' ELSE ae3.WorkPhone END AS 'AgentPhoneNumber',
						CASE WHEN ae3.WorkEmailAddress IS NULL THEN '' ELSE ae3.WorkEmailAddress END AS 'AgentEmailAddress',
						CONCAT(COALESCE(aa.StreetAddress1 + ' ', ''),
								COALESCE(aa.StreetAddress2 + ' ', ''),
								COALESCE(aa.City + ' ',''),
								COALESCE(aa.[StateCode] + ' ', ''),
								COALESCE(aa.ZipCode, '')) 'AgencyAddress',
						CASE WHEN ae2.CompanyName IS NULL THEN '' ELSE ae2.CompanyName END AS 'SubAgencyName',
						CASE WHEN ae.CompanyName IS NULL THEN '' ELSE ae.CompanyName END AS 'AgencyName',
						CASE WHEN pc.[Name] IS NULL THEN '' ELSE pc.[Name] END AS 'UnderwriterName',
						'Texas Insurance Co' 'Carrier'
					FROM 
						Risk r
					INNER JOIN RiskDetail rd ON rd.Id = r.CurrentRiskDetailId
					INNER JOIN Insured i ON i.Id = rd.InsuredId
					INNER JOIN Entity e ON e.Id = i.EntityId
					INNER JOIN EntityAddress ea ON ea.EntityId = e.Id AND ea.AddressTypeId = 4
					INNER JOIN [Address] a ON a.Id = ea.AddressId
					INNER JOIN BrokerInfo bi ON bi.RiskId = r.Id
					INNER JOIN [Binding] b ON b.RiskId = r.Id
					INNER JOIN PaymentTypes pt ON pt.Id = b.PaymentTypesId
					LEFT JOIN Agency_Agency ag ON ag.Id = bi.AgencyId
					LEFT JOIN Agency_Entity ae ON ae.Id = ag.EntityId
					LEFT JOIN Agency_EntityAddress aea ON ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
					LEFT JOIN Agency_Address aa ON aea.AddressId = aa.Id
					LEFT JOIN Agency_SubAgency asa ON asa.AgencyId = ag.Id
					LEFT JOIN Agency_Entity ae2 ON ae2.Id = asa.EntityId
					LEFT JOIN Agency_Agent agt ON agt.Id = bi.AgentId
					LEFT JOIN Agency_Entity ae3 ON ae3.Id = agt.EntityId
					LEFT JOIN PolicyContacts pc ON pc.Id = bi.ProductUWUserId
					WHERE 
						r.CurrentRiskDetailId IS NOT NULL
					AND
						r.[Status] != 'Canceled'
				) rpd
				CROSS APPLY 
				(
					SELECT 
						fb.PastDueDate, 
						fb.PastDueAmount, 
						fb.DueDate, 
						fb.AmountDue, 
						fb.PremiumPaymentAmount 
					FROM 
						[dbo].[fnBilling](GETDATE()) as fb
					WHERE 
						fb.Id = rpd.Id
				) billing
				ORDER BY 
					InsuredName
			END";

        const string REVERT_GenerateFilingsReport = @"
		CREATE OR ALTER   PROCEDURE[dbo].[GenerateFilingsReport]
			AS
			BEGIN

				SET NOCOUNT ON;

				SELECT 
					rpd.*,
					CASE WHEN billing.PastDueAmount IS NULL THEN '' ELSE billing.PastDueAmount END AS 'PastDueAmount',
					CASE WHEN ISNULL(billing.PastDueDate, '') = '' THEN '' ELSE CONVERT(VARCHAR, billing.PastDueDate) END AS 'PastDueDate'
				FROM
				(
					SELECT 
						r.Id,
						r.PolicyNumber, 
						e.CompanyName 'InsuredName', 
						CASE WHEN rd.IsFilingRequired = 1 THEN 'Yes' ELSE 'No' END AS 'Filings',
						CONCAT(COALESCE(a.StreetAddress1 + ' ', ''),
								COALESCE(a.StreetAddress2 + ' ', ''),
								COALESCE(a.City + ' ',''),
								COALESCE(a.[State] + ' ', ''),
								COALESCE(a.ZipCode, '')) 'MailingAddress',
						bi.EffectiveDate 'PolicyEffectiveDate',
						bi.ExpirationDate 'PolicyExpirationDate',
						CASE WHEN pt.[Description] IS NULL THEN '' ELSE pt.[Description] END AS 'PaymentPlan',
						CASE WHEN ae3.FullName IS NULL THEN '' ELSE ae3.FullName END AS 'AgentName',
						CASE WHEN ae3.WorkPhone IS NULL THEN '' ELSE ae3.WorkPhone END AS 'AgentPhoneNumber',
						CASE WHEN ae3.WorkEmailAddress IS NULL THEN '' ELSE ae3.WorkEmailAddress END AS 'AgentEmailAddress',
						CONCAT(COALESCE(aa.StreetAddress1 + ' ', ''),
								COALESCE(aa.StreetAddress2 + ' ', ''),
								COALESCE(aa.City + ' ',''),
								COALESCE(aa.[StateCode] + ' ', ''),
								COALESCE(aa.ZipCode, '')) 'AgencyAddress',
						CASE WHEN ae2.CompanyName IS NULL THEN '' ELSE ae2.CompanyName END AS 'SubAgencyName',
						CASE WHEN ae.CompanyName IS NULL THEN '' ELSE ae.CompanyName END AS 'AgencyName',
						CASE WHEN pc.[Name] IS NULL THEN '' ELSE pc.[Name] END AS 'UnderwriterName',
						'Texas Insurance Co' 'Carrier'
					FROM 
						Risk r
					INNER JOIN RiskDetail rd ON rd.Id = r.CurrentRiskDetailId
					INNER JOIN Insured i ON i.Id = rd.InsuredId
					INNER JOIN Entity e ON e.Id = i.EntityId
					INNER JOIN EntityAddress ea ON ea.EntityId = e.Id AND ea.AddressTypeId = 4
					INNER JOIN [Address] a ON a.Id = ea.AddressId
					INNER JOIN BrokerInfo bi ON bi.RiskId = r.Id
					INNER JOIN [Binding] b ON b.RiskId = r.Id
					INNER JOIN PaymentTypes pt ON pt.Id = b.PaymentTypesId
					LEFT JOIN Agency_Agency ag ON ag.Id = bi.AgencyId
					LEFT JOIN Agency_Entity ae ON ae.Id = ag.EntityId
					LEFT JOIN Agency_EntityAddress aea ON ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
					LEFT JOIN Agency_Address aa ON aea.AddressId = aa.Id
					LEFT JOIN Agency_SubAgency asa ON asa.AgencyId = ag.Id
					LEFT JOIN Agency_Entity ae2 ON ae2.Id = asa.EntityId
					LEFT JOIN Agency_Agent agt ON agt.Id = bi.AgentId
					LEFT JOIN Agency_Entity ae3 ON ae3.Id = agt.EntityId
					LEFT JOIN PolicyContacts pc ON pc.Id = bi.ProductUWUserId
					WHERE 
						r.CurrentRiskDetailId IS NOT NULL
					AND
						r.[Status] != 'Canceled'
				) rpd
				CROSS APPLY 
				(
					SELECT 
						fb.PastDueDate, 
						fb.PastDueAmount, 
						fb.DueDate, 
						fb.AmountDue, 
						fb.PremiumPaymentAmount 
					FROM 
						[dbo].[fnBilling](GETDATE()) as fb
					WHERE 
						fb.Id = rpd.Id
				) billing
				ORDER BY 
					InsuredName
			END";
    }
}
