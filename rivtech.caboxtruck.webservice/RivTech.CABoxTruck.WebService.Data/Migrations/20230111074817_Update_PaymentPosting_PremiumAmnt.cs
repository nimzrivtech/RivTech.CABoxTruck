using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_PaymentPosting_PremiumAmnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(PaymentPosting_ALTER_Up);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(PaymentPosting_ALTER_Down);
        }
        const string PaymentPosting_ALTER_Up = @"
			ALTER   PROCEDURE [dbo].[GeneratePaymentPosting]

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
					rpd.CompanyName,
					rpd.Broker,
					rpd.CreditedOfficeId,
					rpd.CheckNumber,
					rpd.PaymentDate,
					rpd.PaymentAmount,
					ISNULL(paymentpremiumPD.Amount, 0) AS PremiumAmountPD,					
					ISNULL(paymentpremiumAL.Amount, 0 ) AS PremiumAmountAL,
					dbo.fnPremiumAmount(rpd.PaymentId,rpd.PaymentTypesId) as Premium,
					paymenttax.Amount as Tax,
					paymentnonpremium.Amount as NonPremium,
					overpayment.Amount as OverPayment,
					rpd.BrokerCommisionAL,
					rpd.BrokerCommisionPD,
					(paymentpremiumAL.Amount * (cast(rpd.BrokerCommisionAL as decimal(18,2)) / 100.0)) as CommisionALAmount,
					(paymentpremiumPD.Amount * (cast(rpd.BrokerCommisionPD as decimal(18,2)) / 100.0)) as CommisionPDAmount,
					rpd.PaymentTypesId 
					 
               
				 
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
								rc.BrokerCommisionPD,
								b.CreditedOfficeId,
								b.PaymentTypesId
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
							left join RiskCoverage rc on rd.Id = rc.RiskDetailId and rc.OptionNumber = b.BindOptionId
							where p.InstrumentId <> ''A''
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
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount, pdp.AmountTypeId from Billing_PaymentDetail pdp
						where pdp.AmountTypeId = ''T''
						group by pdp.PaymentId, pdp.AmountTypeId
					) paymenttax on paymenttax.PaymentId = rpd.PaymentId
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetail pdp
						where pdp.AmountTypeId != ''P'' and  pdp.AmountTypeId != ''T'' and pdp.AmountTypeId != ''O'' and pdp.AmountTypeId != ''C''
						group by pdp.PaymentId
					) paymentnonpremium on paymentnonpremium.PaymentId = rpd.PaymentId
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount, pdp.AmountTypeId from Billing_PaymentDetail pdp
						where pdp.AmountTypeId = ''O''
						group by pdp.PaymentId, pdp.AmountTypeId
					) overpayment on overpayment.PaymentId = rpd.PaymentId
					where PaymentDate >= @fromDate and PaymentDate <= @toDate
			'
				
			SET @sql = REPLACE(@sql, '@fromDate', ISNULL('''' +  CONVERT(VARCHAR(20), @fromDate) + '''', 'NULL'));
			SET @sql = REPLACE(@sql, '@toDate', ISNULL('''' +  CONVERT(VARCHAR(20), @toDate) + '''', 'NULL'));

			print(@sql)
				EXEC sp_executesql @sql
			END
        

        ";

        const string PaymentPosting_ALTER_Down = @"
			ALTER   PROCEDURE [dbo].[GeneratePaymentPosting]

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
					rpd.CompanyName,
					rpd.Broker,
					rpd.CreditedOfficeId,
					rpd.CheckNumber,
					rpd.PaymentDate,
					rpd.PaymentAmount,
					ISNULL(paymentpremiumPD.Amount, 0) AS PremiumAmountPD,					
					ISNULL(paymentpremiumAL.Amount, 0 ) AS PremiumAmountAL,
					paymentpremium.Amount as Premium,
					paymenttax.Amount as Tax,
					paymentnonpremium.Amount as NonPremium,
					overpayment.Amount as OverPayment,
					rpd.BrokerCommisionAL,
					rpd.BrokerCommisionPD,
					(paymentpremiumAL.Amount * (cast(rpd.BrokerCommisionAL as decimal(18,2)) / 100.0)) as CommisionALAmount,
					(paymentpremiumPD.Amount * (cast(rpd.BrokerCommisionPD as decimal(18,2)) / 100.0)) as CommisionPDAmount,
					rpd.PaymentTypesId 
					 
               
				 
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
								rc.BrokerCommisionPD,
								b.CreditedOfficeId,
								b.PaymentTypesId
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
							left join RiskCoverage rc on rd.Id = rc.RiskDetailId and rc.OptionNumber = b.BindOptionId
							where p.InstrumentId <> ''A''
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
					 left join (
                    select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetail pdp
                    where pdp.AmountTypeId = ''P'' or pdp.AmountTypeId = ''C''
                    group by pdp.PaymentId
					) paymentpremium on paymentpremium.PaymentId = rpd.PaymentId
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount, pdp.AmountTypeId from Billing_PaymentDetail pdp
						where pdp.AmountTypeId = ''T''
						group by pdp.PaymentId, pdp.AmountTypeId
					) paymenttax on paymenttax.PaymentId = rpd.PaymentId
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetail pdp
						where pdp.AmountTypeId != ''P'' and  pdp.AmountTypeId != ''T'' and pdp.AmountTypeId != ''O'' and pdp.AmountTypeId != ''C''
						group by pdp.PaymentId
					) paymentnonpremium on paymentnonpremium.PaymentId = rpd.PaymentId
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount, pdp.AmountTypeId from Billing_PaymentDetail pdp
						where pdp.AmountTypeId = ''O''
						group by pdp.PaymentId, pdp.AmountTypeId
					) overpayment on overpayment.PaymentId = rpd.PaymentId
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
