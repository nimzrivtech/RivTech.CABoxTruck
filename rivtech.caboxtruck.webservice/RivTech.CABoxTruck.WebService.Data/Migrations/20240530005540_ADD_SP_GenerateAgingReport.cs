using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class ADDSPGenerateAgingReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ADD_SP_GENERATEAGINGREPORT);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_SP_GENERATEAGINGREPORT);
        }

		const string ADD_SP_GENERATEAGINGREPORT = @"CREATE OR ALTER PROCEDURE [dbo].[GenerateAgingReport]
				@ToDate datetime = NULL
			AS
			BEGIN
				SET NOCOUNT ON;

				DECLARE @RiskIdCount AS INT = 0;
				
				SET @ToDate= CAST(@ToDate  AS DATE)

				DROP TABLE IF EXISTS #RiskIds, #AgingTable;

				CREATE TABLE #AgingTable (
					RowId INT,
					RiskId UNIQUEIDENTIFIER,
					InvoiceNumber VARCHAR(200),
					InvoiceDate DATETIME,
					DueDate DATETIME,
					Balance DECIMAL(18,2),
					PastDueAmount DECIMAL(18,2),
					CurrentAmountInvoiced DECIMAL(18,2),
					PaymentAmount DECIMAL(18,2),
					Age INT) 

				SELECT DISTINCT bi.RiskId, b.PaymentTypesId
				INTO #RiskIds
				FROM     dbo.Billing_Invoice AS bi INNER JOIN
				dbo.Binding AS b ON bi.RiskId = b.RiskId	
				WHERE  CAST(InvoiceDate AS DATE) <= @ToDate AND VoidDate IS NULL;

				

				SET @RiskIdCount = @@ROWCOUNT;

				WHILE @RiskIdCount > 0
				BEGIN
					DECLARE @OriginalPaymentTotal AS DECIMAL(18,2) = 0,
							@PaymentTotal AS DECIMAL(18,2) = 0,
							@PositiveInvoiceAmount AS DECIMAL(18,2) = 0,
							@NegativePaymentTotal AS DECIMAL(18,2) = 0,
							@TempRiskId AS UNIQUEIDENTIFIER,
							@InvoiceCount AS INT = 0,
							@TempInvoiceId AS INT = 1,
							@Balance AS DECIMAL(18,2) = 0,
							@PaymentTypesId AS INT;

					DROP TABLE IF EXISTS #Invoices; 
					
					CREATE TABLE #Invoices (
					RowId INT,
					InvoiceNumber VARCHAR(200),
					CurrentAmountInvoiced DECIMAL(18,5),
					InvoiceDate DATETIME,
					DueDate DATETIME,
					RiskId UNIQUEIDENTIFIER) 

					SELECT TOP 1 @TempRiskId = RiskId , @PaymentTypesId =PaymentTypesId FROM #RiskIds;

					SELECT @OriginalPaymentTotal = ISNULL(SUM(Amount), 0)
					FROM Billing_Payment
					WHERE PaymentTypeId != 'A' AND CAST(EffectiveDate AS DATE) <= @ToDate AND (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate) AND RiskId = @TempRiskId;

					SET @PaymentTotal = @OriginalPaymentTotal;

					IF @PaymentTypesId != 3
						BEGIN
						INSERT INTO #Invoices
						SELECT 
									ROW_NUMBER() OVER(ORDER BY bi.CreatedDate ASC) AS 'RowId',
									bi.InvoiceNumber,
									CurrentAmountInvoiced,
									ISNULL(CAST(bins.InvoiceOnDate AS DATE), bi.InvoiceDate),
									bi.DueDate,
									bi.RiskId
								FROM Billing_Invoice bi LEFT OUTER JOIN
								dbo.Billing_Installment AS bins ON bi.Id = bins.InvoiceId AND bi.RiskId = bins.RiskId
								WHERE bi.RiskId = @TempRiskId AND ISNULL(CAST(bins.InvoiceOnDate AS DATE), bi.InvoiceDate) <= @ToDate AND bi.VoidDate IS NULL;
						END
					ELSE
						BEGIN
							INSERT INTO #Invoices
								SELECT DISTINCT
									ROW_NUMBER() OVER(ORDER BY bi.CreatedDate ASC) AS 'RowId',
									bi.InvoiceNumber,
									( DueTbl.InvoicedAmount )- (CASE WHEN ALtbl.ALP IS NULL THEN 0 ELSE ALtbl.ALP * (CAST(com.BrokerCommisionAL AS DECIMAL(18,5)) / 100) END + CASE WHEN PDtbl.PDP IS NULL 
									THEN 0 ELSE PDtbl.PDP * (CAST(com.BrokerCommisionPD AS DECIMAL(18,5)) / 100) END) CurrentAmountInvoiced,
									ISNULL(CAST(bins.InvoiceOnDate AS DATE), bi.InvoiceDate),
									bi.DueDate,
									bi.RiskId
							FROM     dbo.Billing_Invoice AS bi INNER JOIN
								(SELECT DISTINCT rd.RiskId, rc.BrokerCommisionAL, rc.BrokerCommisionPD
								FROM      dbo.RiskDetail AS rd INNER JOIN
													dbo.RiskCoverage AS rc ON rd.Id = rc.RiskDetailId) AS com ON bi.RiskId = com.RiskId INNER JOIN
								(SELECT bi.RiskId, SUM(bid.InvoicedAmount) AS InvoicedAmount, bid.InvoiceId
								FROM      dbo.Billing_Invoice AS bi INNER JOIN
													dbo.Billing_InvoiceDetail AS bid ON bi.Id = bid.InvoiceId
								GROUP BY bi.RiskId, bid.InvoiceId) AS DueTbl ON bi.RiskId = DueTbl.RiskId AND bi.Id = DueTbl.InvoiceId  LEFT OUTER JOIN
								dbo.Billing_Installment AS bins ON bi.Id = bins.InvoiceId AND bi.RiskId = bins.RiskId LEFT OUTER JOIN
								(SELECT bi.RiskId, SUM(bid.InvoicedAmount) AS Tax, bid.InvoiceId
								FROM      dbo.Billing_Invoice AS bi INNER JOIN
													dbo.Billing_InvoiceDetail AS bid ON bi.Id = bid.InvoiceId
								WHERE   (bid.AmountSubTypeId IN ('SLT', 'SF', 'ALSF', 'PDSF', 'REINF'))
								GROUP BY bi.RiskId, bid.InvoiceId) AS Taxtbl ON bi.Id = Taxtbl.InvoiceId AND bi.RiskId = Taxtbl.RiskId LEFT OUTER JOIN
								(SELECT bi.RiskId, SUM(bid.InvoicedAmount) AS PDP, bid.InvoiceId
								FROM      dbo.Billing_Invoice AS bi INNER JOIN
													dbo.Billing_InvoiceDetail AS bid ON bi.Id = bid.InvoiceId
								WHERE  (bid.AmountSubTypeId IN ('PDP', 'TIE', 'HPDE', 'PDM', 'PDPMPA', 'PDMMPA', 'HPDEMPA', 'TIEMPA', 'PDPSRP', 'PDMSRP'))
								GROUP BY bi.RiskId,  bid.InvoiceId
								) AS PDtbl ON bi.Id = PDtbl.InvoiceId AND bi.RiskId = PDtbl.RiskId LEFT OUTER JOIN
								(SELECT bi.RiskId, SUM(bid.InvoicedAmount) AS ALP, bid.InvoiceId
								FROM      dbo.Billing_Invoice AS bi INNER JOIN
													dbo.Billing_InvoiceDetail AS bid ON bi.Id = bid.InvoiceId
								WHERE  (bid.AmountSubTypeId IN ('ALP', 'AIP', 'WS', 'PNC', 'CP', 'ALMP', 'GLP', 'TIP', 'NOTP', 'ALPMPA', 'CPMPA', 'ALMPMPA', 'GLPMPA', 'TIPMPA', 'NOTPMPA', 'AIPMPA', 'HPDEMPA', 'WSMPA', 'ALPSRP', 'CPSRP', 'ALMPSRP', 'GLPSRP', 
													'TIPSRP', 'NOTPSRP'))
								GROUP BY bi.RiskId, bid.InvoiceId
								) AS ALtbl ON bi.Id = ALtbl.InvoiceId AND bi.RiskId = ALtbl.RiskId
							WHERE bi.RiskId = @TempRiskId AND ISNULL(CAST(bins.InvoiceOnDate AS DATE), bi.InvoiceDate) <= @ToDate AND bi.VoidDate IS NULL;
								
						END
					SET @InvoiceCount = @@ROWCOUNT;

					SELECT @NegativePaymentTotal = SUM(CurrentAmountInvoiced) 
					FROM #Invoices 
					WHERE CurrentAmountInvoiced < 0;

					SELECT @PositiveInvoiceAmount = SUM(CurrentAmountInvoiced) 
					FROM #Invoices 
					WHERE CurrentAmountInvoiced > 0;
	
					IF(@OriginalPaymentTotal > 0)
					BEGIN
						SET @PaymentTotal = @PaymentTotal - ISNULL(@NegativePaymentTotal, 0);
					END

					IF((ISNULL(@NegativePaymentTotal,0) + ISNULL(@PositiveInvoiceAmount,0)) > 0)
					BEGIN
						WHILE @InvoiceCount > 0
						BEGIN
								INSERT INTO #AgingTable (RowId, RiskId, InvoiceNumber, InvoiceDate, Balance, PastDueAmount, DueDate, CurrentAmountInvoiced, PaymentAmount, Age)
								SELECT 
									RowId,
									RiskId,
									InvoiceNumber,
									InvoiceDate,
									(IIF(@PaymentTotal >= CurrentAmountInvoiced, IIF(CurrentAmountInvoiced < 0, CurrentAmountInvoiced, 0), (CurrentAmountInvoiced - @PaymentTotal))) 'Balance',
									@Balance 'PastDueAmount',
									DueDate,
									CurrentAmountInvoiced,
									(IIF(@PaymentTotal > CurrentAmountInvoiced,  IIF(CurrentAmountInvoiced <= 0, 0, CurrentAmountInvoiced), @PaymentTotal)) 'PaymentAmount',
									IIF(((IIF((@PaymentTotal >= CurrentAmountInvoiced AND CurrentAmountInvoiced > 0), 0,((CurrentAmountInvoiced - @PaymentTotal)) + @Balance) = 0 AND @Balance > 0 AND CurrentAmountInvoiced > 0) OR CurrentAmountInvoiced = 0), 0, DATEDIFF(day, CAST(DueDate AS DATE), @ToDate)) 'Age'
								FROM #Invoices WHERE RowId = @TempInvoiceId AND (CurrentAmountInvoiced > 0 OR @OriginalPaymentTotal = 0);


								SELECT 
									@PaymentTotal = @PaymentTotal - (IIF(@PaymentTotal > (CurrentAmountInvoiced),  (CurrentAmountInvoiced), @PaymentTotal))
								FROM #Invoices WHERE RowId = @TempInvoiceId  AND CurrentAmountInvoiced > 0;

								SELECT 
									@Balance = Balance + PastDueAmount
								FROM #AgingTable WHERE RowId = @TempInvoiceId;

							SET @TempInvoiceId = @TempInvoiceId +1;
							SET @InvoiceCount = @InvoiceCount -1;
						END
					END

					DELETE TOP (1) FROM #RiskIds;

					SET @RiskIdCount = @RiskIdCount -1;
	
					DROP TABLE IF EXISTS #Invoices;
				END

				SELECT 
					ag.RiskId,
					r.PolicyNumber 'Policy Number',
					convert(varchar(10), cast(bi.EffectiveDate as date), 101) 'Policy Inception',
					e.CompanyName 'Insured',
					ag.InvoiceNumber 'Invoice #', 
					convert(varchar(10), cast(ag.InvoiceDate as date), 101)  AS InvoiceDate, 
					CASE WHEN ag.Age < 0 THEN 0 ELSE ag.Balance END  AS  'Past Due Amount',
					PaymentAmount,
					convert(varchar(10), cast(ag.DueDate as date), 101)  'Due Date', 
					ag.CurrentAmountInvoiced 'Total Amount Due',
					(IIF(ag.Age < 0, IIF(CAST(ag.Balance AS NVARCHAR(40)) != '0.00', CAST(ag.Balance AS NVARCHAR(40)), ''), '')) AS 'Current',
					(IIF((ag.Age >= 0 AND ag.Age <= 30 ), IIF(CAST(ag.Balance AS NVARCHAR(40)) != '0.00', CAST(ag.Balance AS NVARCHAR(40)), ''), '')) AS '0-30',
					(IIF(ag.Age >= 31 AND ag.Age <= 60, IIF(CAST(ag.Balance AS NVARCHAR(40)) != '0.00', CAST(ag.Balance AS NVARCHAR(40)), ''), '')) AS '31-60',
					(IIF(ag.Age >= 61 AND ag.Age <= 90, IIF(CAST(ag.Balance AS NVARCHAR(40)) != '0.00', CAST(ag.Balance AS NVARCHAR(40)), ''), '')) AS '61-90',
					(IIF(ag.Age >= 91, IIF(CAST(ag.Balance AS NVARCHAR(40)) != '0.00', CAST(ag.Balance AS NVARCHAR(40)), ''), '')) AS '91+'
				FROM #AgingTable ag
				LEFT JOIN Risk r ON r.Id = ag.RiskId
				LEFT JOIN BrokerInfo bi ON bi.RiskId = r.Id
				LEFT JOIN RiskDetail rd ON rd.Id = r.CurrentRiskDetailId
				LEFT JOIN Insured i ON i.Id = rd.InsuredId
				LEFT JOIN Entity e ON e.Id = i.EntityId
				WHERE  ag.Balance <> 0-- AND ag.Balance < 1 AND ag.Balance > 0.00
				ORDER BY e.CompanyName, ag.InvoiceNumber, ag.InvoiceDate

				DROP TABLE IF EXISTS #RiskIds, #AgingTable;


			END";

        const string DELETE_SP_GENERATEAGINGREPORT = @"DROP PROCEDURE [dbo].[GenerateAgingReport]";
    }
}
