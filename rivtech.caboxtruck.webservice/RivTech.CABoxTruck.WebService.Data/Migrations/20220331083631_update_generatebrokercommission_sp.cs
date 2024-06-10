using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class update_generatebrokercommission_sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CREATE_SP_AR);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

		const string CREATE_SP_AR = @"
            SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE OR ALTER PROCEDURE [dbo].[GenerateBrokerCommission]

					@fromDate datetime = NULL,
					@toDate datetime = NULL,
					@sql nvarchar(max) = null
				AS
				BEGIN

				SET NOCOUNT ON;

				set @sql = 
				'
					select 
						rpd.Id,
						rpd.PolicyNumber,
						rpd.Broker,
						rpd.CompanyName,
						rpd.CheckNumber,
						rpd.VoidDate,
						rpd.PaymentDate,
						rpd.BrokerCommisionAL,
						rpd.BrokerCommisionPD,
						(ISNULL(paymentpremiumAL.Amount, 0 ) + ISNULL(paymentpremiumPD.Amount, 0)) as PaymentAmount,
						(paymentpremiumAL.Amount * (cast(rpd.BrokerCommisionAL as decimal(18,2)) / 100.0)) as CommisionALAmount,
						(paymentpremiumPD.Amount * (cast(rpd.BrokerCommisionPD as decimal(18,2)) / 100.0)) as CommisionPDAmount
					from
						(select rr.*
							from
							(
								select 
									r.Id, 
									p.Id as PaymentId,
									r.PolicyNumber,
									ae.CompanyName as Broker,
									ie.CompanyName,
									p.Reference as CheckNumber,
									p.Amount as PaymentAmount,
									p.VoidDate,
									p.CreatedDate as PaymentDate,
									rc.BrokerCommisionAL,
									rc.BrokerCommisionPD
								from Billing_Payment p
								left join Risk r on r.Id = p.RiskId
								left join Binding b on r.Id = b.RiskId
								left join BrokerInfo bi on bi.RiskId = p.RiskId
								left join RiskDetail rd on rd.RiskId = r.Id
											AND rd.Id =
											(
												SELECT top 1 z.Id
												FROM RiskDetail z 
												WHERE z.RiskId = r.Id ORDER BY z.UpdatedDate DESC
											)
								left join Insured i on rd.InsuredId = i.Id
								left join Entity ie on i.EntityId = ie.Id
								left join Agency_Agency agency on agency.Id = bi.AgencyId
								left join Agency_Entity ae on ae.Id = agency.EntityId
								left join RiskCoverage rc on rd.Id = rc.RiskDetailId
								where b.PaymentTypesId != 3 and r.Status = ''Active''
							) rr
						) rpd
						left join (
							select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetail pdp
							where pdp.AmountTypeId = ''P'' and pdp.AmountSubTypeId = ''ALP''
							group by pdp.PaymentId
						) paymentpremiumAL on paymentpremiumAL.PaymentId = rpd.PaymentId
						left join (
							select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetail pdp
							where pdp.AmountTypeId = ''P'' and pdp.AmountSubTypeId = ''PDP''
							group by pdp.PaymentId
						) paymentpremiumPD on paymentpremiumPD.PaymentId = rpd.PaymentId
					where PaymentDate >= @fromDate and PaymentDate <= @toDate
				'
				
				SET @sql = REPLACE(@sql, '@fromDate', ISNULL('''' +  CONVERT(VARCHAR(20), @fromDate) + '''', 'NULL'));
				SET @sql = REPLACE(@sql, '@toDate', ISNULL('''' +  CONVERT(VARCHAR(20), @toDate) + '''', 'NULL'));

				print(@sql)
				  EXEC sp_executesql @sql
				END
		";
	}
}
