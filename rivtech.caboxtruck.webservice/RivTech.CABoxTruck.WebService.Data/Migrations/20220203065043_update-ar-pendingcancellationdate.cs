using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class updatearpendingcancellationdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_FN_BILLING);
            migrationBuilder.Sql(UPDATE_SP_AR);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

		const string UPDATE_FN_BILLING = @"
            SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			ALTER   FUNCTION [dbo].[fnBilling]
			(   
				@asOf Date = NULL
			)
			RETURNS TABLE 
			AS
			RETURN 
			(
				select Risk.Id,
					CASE WHEN PastInvoiced.PastBilled - Payment.PremiumPaymentAmount <= 0 THEN NULL ELSE PastInvoiced.PastDueDate END as PastDueDate,
					CASE WHEN PastInvoiced.PastBilled - Payment.PremiumPaymentAmount < 0 THEN 0 ELSE PastInvoiced.PastBilled - Payment.PremiumPaymentAmount END as PastDueAmount,
					CurrentInvoiced.DueDate,
					CurrentInvoiced.TotalBilled - Payment.PremiumPaymentAmount as AmountDue,
					Payment.PremiumPaymentAmount
				from Risk
				cross apply (select isnull(SUM(InvoiceDetail.InvoicedAmount), 0) PastBilled,
							MAX(Invoice.DueDate) PastDueDate from [Billing_Invoice] Invoice
							inner join [Billing_InvoiceDetail] InvoiceDetail on Invoice.Id = InvoiceDetail.InvoiceId
					where Invoice.RiskId = Risk.Id and Invoice.VoidDate IS NULL
					and (@asOf IS NULL or Invoice.DueDate < @asOf)
				) PastInvoiced
				cross apply (select isnull(SUM(InvoiceDetail.InvoicedAmount), 0) TotalBilled,
							MAX(Invoice.DueDate) DueDate from [Billing_Invoice] Invoice
							inner join [Billing_InvoiceDetail] InvoiceDetail on Invoice.Id = InvoiceDetail.InvoiceId
					where Invoice.RiskId = Risk.Id and Invoice.VoidDate IS NULL
					and (@asOf IS NULL or Invoice.InvoiceDate <= @asOf)
				) CurrentInvoiced
				cross apply (select isnull(SUM(PaymentDetail.Amount), 0) PremiumPaymentAmount from [Billing_Payment] Payment
					inner join [Billing_PaymentDetail] PaymentDetail on Payment.Id = PaymentDetail.PaymentId
					where Payment.RiskId = Risk.Id
					and (@asOf IS NULL or Payment.EffectiveDate <= @asOf)
				) Payment 
			)
        ";

		const string UPDATE_SP_AR = @"
            SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE OR ALTER PROCEDURE [dbo].[SearchAccountsReceivable]

					@sortNameBy varchar(100),
					@receivableType varchar(30),
					@broker varchar(100),
					@pastDueOnly bit,
					@noPaymentsMade bit,
					@valuationDate [date] = NULL,
					@sql nvarchar(max) = null,
					@param nvarchar(max) = null
				AS
				BEGIN

					SET NOCOUNT ON;

				 set @param = ''
				IF @receivableType = 'All'
					BEGIN
					 SET @param = 'WHERE (PremiumDue <> 0)'
					END
				ELSE IF @receivableType = 'Amount Due'
					BEGIN
					 SET @param= 'WHERE (PremiumDue > 0)'
					END
				ELSE IF @receivableType = 'Refund'
					BEGIN
					 SET @param= 'WHERE (PremiumDue < 0)'
					END

				IF @broker <> 'All'
					BEGIN
					 SET @param = @param + ' AND (Broker like ''%'  + @broker + '%'')'
					END

				IF @pastDueOnly = 1
					BEGIN
					 SET @param = @param + ' AND (PastDueAmount > 0)'
					END

				IF @noPaymentsMade = 1
					BEGIN
					 SET @param = @param + ' AND (TotalPayment = 0)'
					END

				IF @sortNameBy = 'Insured Name'
					BEGIN
					 SET @param = @param + ' ORDER BY Broker, Insured'
					END
				ELSE 
					BEGIN
					 SET @param = @param + ' ORDER BY Broker, PolicyNumber'
					END

				set @sql = 
				'
				select * from (
					select 
						rpd.*,
						billing.PastDueAmount as PastDueAmount,
						billing.PastDueDate as PastDueDate,
						billing.DueDate as CurrentDueDate,
						billing.AmountDue as CurrentAmountDue,
						CASE WHEN billing.PremiumPaymentAmount = 0 THEN ''X'' ELSE '''' END AS NoPayments,
						billing.AmountDue as PremiumDue,
						billing.PremiumPaymentAmount as TotalPayment
					from
						(select result.*
							from 
							(
								select 
								r.Id as PolicyDetailID,
								r.PolicyNumber,
								r.PendingCancellationDate as PendingCancellation,
								rd.CancellationDate,
								ie.CompanyName as Insured,
								ie.WorkPhoneExtension as InsuredPhone,
								bi.EffectiveDate as Inception,
								ae.CompanyName as [Broker],
								CASE WHEN bind.PaymentTypesId = 3 THEN ''Yes'' ELSE ''No'' END AS Financed
								from RiskDetail rd
								left join Risk r on r.Id = rd.RiskId
									AND rd.Id = 
									(
										SELECT top 1 z.Id
										FROM RiskDetail z 
										WHERE z.RiskId = r.Id ORDER BY z.UpdatedDate DESC
									)
								left join Insured i on rd.InsuredId = i.Id
								left join Entity ie on i.EntityId = ie.Id
								left join BrokerInfo bi on bi.RiskId = rd.RiskId
								left join [Binding] bind on bind.RiskId = rd.RiskId
								left join Agency_Agency agency on agency.Id = bi.AgencyId
								left join Agency_Entity ae on ae.Id = agency.EntityId
							) result
							group by  result.Broker, result.CancellationDate, result.Insured, result.InsuredPhone,
											result.Inception, result.PendingCancellation, result.PolicyDetailID, result.PolicyNumber,result.Financed
						) rpd
						cross apply (
							select bb.PastDueDate, bb.PastDueAmount, bb.DueDate, bb.AmountDue, bb.PremiumPaymentAmount from [dbo].[fnBilling](@valuationDate) as bb
							where bb.Id = rpd.PolicyDetailID
						) billing
				) acctsReceivable
				' + @param

				SET @sql = REPLACE(@sql, '@valuationDate', ISNULL('''' +  CONVERT(VARCHAR(20), @valuationDate) + '''', 'NULL'))
				print(@sql)
				  EXEC sp_executesql @sql
				END
        ";
	}
}
