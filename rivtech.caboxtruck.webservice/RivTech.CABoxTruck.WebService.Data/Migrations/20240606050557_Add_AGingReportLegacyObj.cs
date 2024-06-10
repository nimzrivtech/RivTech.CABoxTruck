using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAGingReportLegacyObj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CREATE_TABLETYPE_Policy);
            migrationBuilder.Sql(CREATE_TABLETYPE_Aging);
            migrationBuilder.Sql(CREATE_SP_GenerateTICLiveryAgingReport);
            migrationBuilder.Sql(CREATE_SP_GenerateTICBoxtruckAgingReport);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ROLLBACK_TABLETYPE_Policy);
            migrationBuilder.Sql(ROLLBACK_TABLETYPE_Aging);
            migrationBuilder.Sql(ROLLBACK_SP_TICLivery);
            migrationBuilder.Sql(ROLLBACK_SP_TICBoxtruck);
        }


        const string CREATE_TABLETYPE_Policy = @"
            if not EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'tblPolicy')
            BEGIN
                CREATE TYPE [dbo].[tblPolicy] AS TABLE(
	                [PolicyDetailID] [int] NULL
                )
               
            END
        ";

        const string CREATE_TABLETYPE_Aging = @"
             if not EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'tblAging')
            BEGIN
                CREATE TYPE [dbo].[tblAging] AS TABLE(
	                [RowId] [int] NOT NULL,
	                [PolicyDetailID] [int] NULL,
	                [InvoiceNumber] [varchar](200) NULL,
	                [InvoiceDate] [datetime] NULL,
	                [DueDate] [datetime] NULL,
	                [Balance] [decimal](18, 2) NULL,
	                [PastDueAmount] [decimal](18, 2) NULL,
	                [InvoiceCurrentDueAmount] [decimal](18, 4) NULL,
	                [CurrentPaymentAmount] [decimal](18, 4) NULL,
	                [PaymentAmount] [decimal](18, 2) NULL,
	                [TotalAmountDue] [decimal](18, 2) NULL,
	                [Age] [int] NULL
                )
                
            END
        ";

        const string ROLLBACK_TABLETYPE_Policy = @"
           DROP TYPE [dbo].[tblPolicy]
           GO
        ";

        const string ROLLBACK_TABLETYPE_Aging = @"
           DROP TYPE [dbo].[tblAging]
           GO
        ";

        const string CREATE_SP_GenerateTICLiveryAgingReport = @"
            
			CREATE OR ALTER PROCEDURE [dbo].[GenerateTICLiveryAgingReport]
				@ToDate AS DATETIME
			AS
			BEGIN
					DECLARE @PolicyDetailIdCount AS INT = 0,
									@PolicyDetailIds [dbo].[tblPolicy] ,
									@AgingTable  [dbo].[tblAging] 
									
		
					SET @ToDate= CAST(@ToDate  AS DATE)

					INSERT INTO @PolicyDetailIds
					SELECT DISTINCT [PolicyDetailID]
					FROM TICLivery_Invoice
					WHERE  (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate) 
		
					SET @PolicyDetailIdCount = @@ROWCOUNT;

	

					WHILE @PolicyDetailIdCount > 0
					BEGIN
						DECLARE @PaymentTotal AS DECIMAL(18,2) = 0,
								@OriginalPaymentTotal AS DECIMAL(18,2) = 0,
								@NegativePaymentTotal AS DECIMAL(18,2) = 0,
								@PositiveInvoiceAmount AS DECIMAL(18,2) = 0,
								@TempRiskId AS INT,
								@InvoiceCount AS INT = 0,
								@TempInvoiceId AS INT = 1,
								@Balance AS DECIMAL(18,2) = 0;

						DROP TABLE IF EXISTS #Invoices; 
	
						SELECT TOP 1 @TempRiskId = PolicyDetailID FROM @PolicyDetailIds;

						SELECT @OriginalPaymentTotal = ISNULL(SUM(PaymentAmount), 0)
						FROM TICLivery_Payment
						WHERE  CAST(PaymentDate AS DATE) <= @ToDate AND (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate)  AND PolicyDetailID = @TempRiskId;
			
						SET @PaymentTotal = @OriginalPaymentTotal

						SELECT 
							ROW_NUMBER() OVER(ORDER BY INvoiceDate ASC) AS 'RowId',
							InvoiceNumber,
							[InvoiceCurrentDueAmount],
							InvoiceDate,
							[InvoiceDueDate],
							PolicyDetailID
						INTO #Invoices
						FROM TICLivery_Invoice
						WHERE PolicyDetailID = @TempRiskId AND (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate)  AND CAST(InvoiceDate AS DATE)<= @ToDate 

						SET @InvoiceCount = @@ROWCOUNT;

			
						SELECT @NegativePaymentTotal = SUM(InvoiceCurrentDueAmount) 
						FROM #Invoices 
						WHERE InvoiceCurrentDueAmount < 0 
			
						SELECT @PositiveInvoiceAmount = SUM(InvoiceCurrentDueAmount) 
						FROM #Invoices 
						WHERE InvoiceCurrentDueAmount > 0 


						IF(@OriginalPaymentTotal > 0)
						BEGIN
						SET @PaymentTotal = @PaymentTotal - ISNULL(@NegativePaymentTotal, 0)
						END

						IF((ISNULL(@NegativePaymentTotal,0) + ISNULL(@PositiveInvoiceAmount,0)) > 0)
						BEGIN
							WHILE @InvoiceCount > 0
							BEGIN
									INSERT INTO @AgingTable (RowId, PolicyDetailID, InvoiceNumber, InvoiceDate, Balance, PastDueAmount, DueDate, InvoiceCurrentDueAmount, CurrentPaymentAmount, PaymentAmount, TotalAmountDue, Age)
									SELECT
										RowId,
										[PolicyDetailID],
										InvoiceNumber,
										InvoiceDate,
										(IIF(@PaymentTotal >= [InvoiceCurrentDueAmount], IIF([InvoiceCurrentDueAmount] < 0, [InvoiceCurrentDueAmount], 0), ([InvoiceCurrentDueAmount] - @PaymentTotal))) 'Balance',
							
										@Balance 'PastDueAmount',
										[InvoiceDueDate],
										[InvoiceCurrentDueAmount],
										@PaymentTotal 'CurrentPaymentAmount',
										(IIF(@PaymentTotal > [InvoiceCurrentDueAmount],  IIF([InvoiceCurrentDueAmount] <= 0, 0, [InvoiceCurrentDueAmount]), @PaymentTotal)) 'PaymentAmount',
										@Balance + (IIF(@PaymentTotal >= [InvoiceCurrentDueAmount], IIF([InvoiceCurrentDueAmount] < 0, [InvoiceCurrentDueAmount], 0), ([InvoiceCurrentDueAmount] - @PaymentTotal))) 'TotalAmountDue',
										IIF(((IIF((@PaymentTotal >= [InvoiceCurrentDueAmount] AND [InvoiceCurrentDueAmount] > 0), 0,(([InvoiceCurrentDueAmount] - @PaymentTotal)) + @Balance) = 0 AND @Balance > 0 AND [InvoiceCurrentDueAmount] > 0) OR [InvoiceCurrentDueAmount] = 0), 0, DATEDIFF(day, CAST([InvoiceDueDate] AS DATE), @ToDate)) 'Age'
									FROM #Invoices WHERE RowId = @TempInvoiceId  AND( [InvoiceCurrentDueAmount] > 0 OR @OriginalPaymentTotal = 0);

									SELECT 
										@PaymentTotal =@PaymentTotal- (IIF(@PaymentTotal > ([InvoiceCurrentDueAmount]),  ([InvoiceCurrentDueAmount]), @PaymentTotal))
									FROM #Invoices WHERE RowId = @TempInvoiceId AND [InvoiceCurrentDueAmount] > 0 ;

									SELECT 
										@Balance = Balance + PastDueAmount
									FROM @AgingTable WHERE RowId = @TempInvoiceId;

								SET @TempInvoiceId = @TempInvoiceId +1;
								SET @InvoiceCount = @InvoiceCount -1;
							END
					END

						DELETE TOP (1) FROM @PolicyDetailIds;

						SET @PolicyDetailIdCount = @PolicyDetailIdCount -1;
	
						DROP TABLE IF EXISTS #Invoices;
					END

					SELECT 
						PolicyNumber AS 'Policy Number',
						convert(varchar(10), cast(InceptionDate as date), 101)  AS 'Policy Inception',
						CompanyName AS 'Insured',
						InvoiceNumber AS 'Invoice #', 
						convert(varchar(10), cast(InvoiceDate as date), 101)  AS 'Invoice Date', 
						CASE WHEN Age < 0 THEN 0 ELSE CAST(Balance AS DECIMAL(12,2)) END  AS 'Past Due Amount',
						convert(varchar(10), cast(DueDate as date), 101)   AS 'Due Date', 
						CAST(InvoiceCurrentDueAmount AS DECIMAL(12,2)) AS 'Total Amount Due',
						(IIF(Age < 0, IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS 'Current',
						(IIF((Age >= 0 AND Age <= 30 ), IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS '0-30',
						(IIF(Age >= 31 AND Age <= 60, IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS '31-60',
						(IIF(Age >= 61 AND Age <= 90, IIF(CAST(Balance AS NVARCHAR(40)) != '0.00', CAST(Balance AS NVARCHAR(40)), ''), '')) AS '61-90',
						(IIF(Age >= 91, IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS '91+'
						--Balance
					FROM @AgingTable AS Atbl
					LEFT OUTER JOIN
										  (SELECT dtl.ID, dtl.PolicyNumber, dtl.InceptionDate, dtl.CompanyName
										   FROM      TICLivery_vw_Insured dtl ) AS Details ON Atbl.PolicyDetailID = Details.ID
					WHERE Balance <> 0 
		
					ORDER BY CompanyName, InvoiceNumber, InvoiceDate


					END

        ";

        const string CREATE_SP_GenerateTICBoxtruckAgingReport = @"
            
			CREATE OR ALTER PROCEDURE [dbo].[GenerateTICBoxtruckAgingReport]
				@ToDate AS DATETIME
			AS
			BEGIN
					DECLARE @PolicyDetailIdCount AS INT = 0,
									@PolicyDetailIds [dbo].[tblPolicy] ,
									@AgingTable  [dbo].[tblAging] 
		
					SET @ToDate= CAST(@ToDate  AS DATE)

					INSERT INTO @PolicyDetailIds
					SELECT DISTINCT [PolicyDetailID]
					FROM TICBoxtruck_Invoice
					WHERE  (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate) 
		
					SET @PolicyDetailIdCount = @@ROWCOUNT;

	

					WHILE @PolicyDetailIdCount > 0
					BEGIN
						DECLARE @PaymentTotal AS DECIMAL(18,2) = 0,
								@OriginalPaymentTotal AS DECIMAL(18,2) = 0,
								@NegativePaymentTotal AS DECIMAL(18,2) = 0,
								@PositiveInvoiceAmount AS DECIMAL(18,2) = 0,
								@TempRiskId AS INT,
								@InvoiceCount AS INT = 0,
								@TempInvoiceId AS INT = 1,
								@Balance AS DECIMAL(18,2) = 0;

						DROP TABLE IF EXISTS #Invoices; 
	
						SELECT TOP 1 @TempRiskId = PolicyDetailID FROM @PolicyDetailIds;

						SELECT @OriginalPaymentTotal = ISNULL(SUM(PaymentAmount), 0)
						FROM TICBoxtruck_Payment
						WHERE  CAST(PaymentDate AS DATE) <= @ToDate AND (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate)  AND PolicyDetailID = @TempRiskId;
			
						SET @PaymentTotal = @OriginalPaymentTotal

						SELECT 
							ROW_NUMBER() OVER(ORDER BY INvoiceDate ASC) AS 'RowId',
							InvoiceNumber,
							[InvoiceCurrentDueAmount],
							InvoiceDate,
							[InvoiceDueDate],
							PolicyDetailID
						INTO #Invoices
						FROM TICBoxtruck_Invoice
						WHERE PolicyDetailID = @TempRiskId AND (VoidDate IS NULL OR CAST(VoidDate AS DATE) > @ToDate)  AND CAST(InvoiceDate AS DATE)<= @ToDate 

						SET @InvoiceCount = @@ROWCOUNT;

			
						SELECT @NegativePaymentTotal = SUM(InvoiceCurrentDueAmount) 
						FROM #Invoices 
						WHERE InvoiceCurrentDueAmount < 0 
			
						SELECT @PositiveInvoiceAmount = SUM(InvoiceCurrentDueAmount) 
						FROM #Invoices 
						WHERE InvoiceCurrentDueAmount > 0 


						IF(@OriginalPaymentTotal > 0)
						BEGIN
						SET @PaymentTotal = @PaymentTotal - ISNULL(@NegativePaymentTotal, 0)
						END

						IF((ISNULL(@NegativePaymentTotal,0) + ISNULL(@PositiveInvoiceAmount,0)) > 0)
						BEGIN
							WHILE @InvoiceCount > 0
							BEGIN
									INSERT INTO @AgingTable (RowId, PolicyDetailID, InvoiceNumber, InvoiceDate, Balance, PastDueAmount, DueDate, InvoiceCurrentDueAmount, CurrentPaymentAmount, PaymentAmount, TotalAmountDue, Age)
									SELECT
										RowId,
										[PolicyDetailID],
										InvoiceNumber,
										InvoiceDate,
										(IIF(@PaymentTotal >= [InvoiceCurrentDueAmount], IIF([InvoiceCurrentDueAmount] < 0, [InvoiceCurrentDueAmount], 0), ([InvoiceCurrentDueAmount] - @PaymentTotal))) 'Balance',
							
										@Balance 'PastDueAmount',
										[InvoiceDueDate],
										[InvoiceCurrentDueAmount],
										@PaymentTotal 'CurrentPaymentAmount',
										(IIF(@PaymentTotal > [InvoiceCurrentDueAmount],  IIF([InvoiceCurrentDueAmount] <= 0, 0, [InvoiceCurrentDueAmount]), @PaymentTotal)) 'PaymentAmount',
										@Balance + (IIF(@PaymentTotal >= [InvoiceCurrentDueAmount], IIF([InvoiceCurrentDueAmount] < 0, [InvoiceCurrentDueAmount], 0), ([InvoiceCurrentDueAmount] - @PaymentTotal))) 'TotalAmountDue',
										IIF(((IIF((@PaymentTotal >= [InvoiceCurrentDueAmount] AND [InvoiceCurrentDueAmount] > 0), 0,(([InvoiceCurrentDueAmount] - @PaymentTotal)) + @Balance) = 0 AND @Balance > 0 AND [InvoiceCurrentDueAmount] > 0) OR [InvoiceCurrentDueAmount] = 0), 0, DATEDIFF(day, CAST([InvoiceDueDate] AS DATE), @ToDate)) 'Age'
									FROM #Invoices WHERE RowId = @TempInvoiceId  AND( [InvoiceCurrentDueAmount] > 0 OR @OriginalPaymentTotal = 0);

									SELECT 
										@PaymentTotal =@PaymentTotal- (IIF(@PaymentTotal > ([InvoiceCurrentDueAmount]),  ([InvoiceCurrentDueAmount]), @PaymentTotal))
									FROM #Invoices WHERE RowId = @TempInvoiceId AND [InvoiceCurrentDueAmount] > 0 ;

									SELECT 
										@Balance = Balance + PastDueAmount
									FROM @AgingTable WHERE RowId = @TempInvoiceId;

								SET @TempInvoiceId = @TempInvoiceId +1;
								SET @InvoiceCount = @InvoiceCount -1;
							END
					END

						DELETE TOP (1) FROM @PolicyDetailIds;

						SET @PolicyDetailIdCount = @PolicyDetailIdCount -1;
	
						DROP TABLE IF EXISTS #Invoices;
					END

					SELECT 
						PolicyNumber AS 'Policy Number',
						convert(varchar(10), cast(InceptionDate as date), 101)  AS 'Policy Inception',
						CompanyName AS 'Insured',
						InvoiceNumber AS 'Invoice #', 
						convert(varchar(10), cast(InvoiceDate as date), 101)  AS 'Invoice Date', 
						CASE WHEN Age < 0 THEN 0 ELSE CAST(Balance AS DECIMAL(12,2)) END  AS 'Past Due Amount',
						convert(varchar(10), cast(DueDate as date), 101)   AS 'Due Date', 
						CAST(InvoiceCurrentDueAmount AS DECIMAL(12,2)) AS 'Total Amount Due',
						(IIF(Age < 0, IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS 'Current',
						(IIF((Age >= 0 AND Age <= 30 ), IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS '0-30',
						(IIF(Age >= 31 AND Age <= 60, IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS '31-60',
						(IIF(Age >= 61 AND Age <= 90, IIF(CAST(Balance AS NVARCHAR(40)) != '0.00', CAST(Balance AS NVARCHAR(40)), ''), '')) AS '61-90',
						(IIF(Age >= 91, IIF(CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)) != '0.00', CAST(CAST(Balance AS DECIMAL(12,2)) AS NVARCHAR(40)), ''), '')) AS '91+'
						--Balance
					FROM @AgingTable AS Atbl
					LEFT OUTER JOIN
										  (SELECT dtl.ID, dtl.PolicyNumber, dtl.InceptionDate, dtl.CompanyName
										   FROM      TICBoxtruck_vw_Insured dtl ) AS Details ON Atbl.PolicyDetailID = Details.ID
					WHERE Balance <> 0 
		
					ORDER BY CompanyName, InvoiceNumber, InvoiceDate


					END

        ";

        const string ROLLBACK_SP_TICLivery = @"
           DROP PROCEDURE [dbo].[GenerateTICLiveryAgingReport]
			GO

        ";

        const string ROLLBACK_SP_TICBoxtruck = @"
           DROP PROCEDURE [dbo].[GenerateTICBoxtruckAgingReport]
			GO

        ";
    }
}
