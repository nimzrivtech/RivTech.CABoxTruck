using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBrokerComissionARSCommission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(BrokerCommission_ALTER_Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(BrokerCommission_ALTER_Down);
        }

        const string BrokerCommission_ALTER_Up = @"
            ALTER   PROCEDURE [dbo].[GenerateBrokerCommission]

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
					ISNULL(commalcommision.Amount, 0) as CommisionALAmount,
					ISNULL(commpdcommision.Amount, 0) as CommisionPDAmount,
					ISNULL(commarscommision.Amount,0) as CommissionARS,
					rpd.PercentageARS
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
								rc.ARSCommission as PercentageARS
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
							where b.PaymentTypesId != 3
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
						select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetailCommission pdp
						where pdp.AmountTypeId = ''C'' AND (pdp.AmountSubTypeId = ''ALC'')
						group by pdp.PaymentId
					) commalcommision on commalcommision.PaymentId = rpd.PaymentId
					left join (
						select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetailCommission pdp
						where pdp.AmountTypeId = ''C'' AND (pdp.AmountSubTypeId = ''PDC'')
						group by pdp.PaymentId
					) commpdcommision on commpdcommision.PaymentId = rpd.PaymentId
					left join (
                        select pdp.PaymentId,SUM(pdp.Amount) as Amount from Billing_PaymentDetailCommission pdp
                        where pdp.AmountTypeId = ''C'' AND (pdp.AmountSubTypeId = ''ARSC'')
                        group by pdp.PaymentId
                    ) commarscommision on commarscommision.PaymentId = rpd.PaymentId
				where PaymentDate >= @fromDate and PaymentDate <= @toDate
			'
				
			SET @sql = REPLACE(@sql, '@fromDate', ISNULL('''' +  CONVERT(VARCHAR(20), @fromDate) + '''', 'NULL'));
			SET @sql = REPLACE(@sql, '@toDate', ISNULL('''' +  CONVERT(VARCHAR(20), @toDate) + '''', 'NULL'));

			print(@sql)
				EXEC sp_executesql @sql
			END
        ";

        const string BrokerCommission_ALTER_Down = @"
            ALTER   PROCEDURE [dbo].[GenerateBrokerCommission]

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
							left join RiskCoverage rc on rd.Id = rc.RiskDetailId and rc.OptionNumber = b.BindOptionId
							where b.PaymentTypesId != 3
                            and p.InstrumentId <> ''A''
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
