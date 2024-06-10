using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_BDXReport_EarnedPremiumFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_BorderauxReport);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(REVERT_BorderauxReport);
        }

        const string UPDATE_BorderauxReport = @"
            CREATE OR ALTER   PROCEDURE[dbo].[GenerateBorderauxReport]
				@fromDate datetime = NULL,
				@toDate datetime = NULL
			AS
			BEGIN

                SET NOCOUNT ON;

				select
					brpt.[Policy Number] 'PolicyNumber',
					brpt.[Endorsement Number] 'EndorsementNumber',
					brpt.[Transaction Number] 'TransactionNumber',
					brpt.[Insured Name] 'InsuredName',
					brpt.[Risk State] 'RiskState',
					brpt.[Transaction Type] 'TransactionType',
					brpt.[Transaction Date] 'TransactionDate',
					brpt.[Transaction Effective Date] 'TransactionEffectiveDate',
					brpt.[Effective Date] 'EffectiveDate',
					brpt.[Expiration Date] 'ExpirationDate',
					brpt.[Garaging Address] 'GaragingAddress',
					brpt.[Garaging City] 'GaragingCity',
					brpt.[Garaging State] 'GaragingState',
					brpt.[Garaging ZipCode] 'GaragingZipCode',
					brpt.[Mailing Address] 'MailingAddress',
					brpt.[Mailing City] 'MailingCity',
					brpt.[Mailing State] 'MailingState',
					brpt.[Mailing ZipCode] 'MailingZipCode',
					brpt.[Vehicle/Unit Count] 'VehicleUnitCount',
					brpt.[Total Premium] 'TotalPremium',
					brpt.[Auto Liability Premium] 'AutoLiabilityPremium',
					brpt.[Auto Liability Limit] 'AutoLiabilityLimit',
					brpt.[Auto Liability Deductible] 'AutoLiabilityDeductible',
					brpt.[AL Manuscript Premium] 'ALManuscriptPremium',
					brpt.[General Liability Premium] 'GeneralLiabilityPremium',
					brpt.[PIP Limit] 'PIPLimit',
					brpt.[Auto Medical Payment Limit] 'AutoMedicalPaymentLimit',
					brpt.Symbol,
					brpt.[Collision Premium] 'CollisionPremium',
					brpt.[Collision Deductible] 'CollisionDeductible',
					brpt.[Comp/FT Premium] 'CompFTPremium',
					brpt.[Comp/FT Deductible] 'CompFTDeductible',
					brpt.[PD Manuscript Premium] 'PDManuscriptPremium',
					brpt.[Motor Truck Cargo Premium] 'CargoPremium',
					brpt.[Hired PD Premium] 'HiredPDPremium',
					brpt.[Hired PD Limit] 'HiredPDLimit',
					brpt.[Hired PD Deductible] 'HiredPDDeductible',
					brpt.[Trailer Premium] 'TrailerPremium',
					brpt.[Primary and Non-Contributory Premium] 'PNCPremium',
					brpt.[Short Rate Premium] 'ShortRatePremium',
					brpt.[Motor Truck Cargo Limit] 'CargoLimit',
					brpt.[Uninsured Motorists BI Limit/Deductible] 'UMBILimit',
					brpt.[Uninsured Motorists PD Limit/Deductible] 'UMPDLimit',
					brpt.[Underinsured Motorists BI Limit/Deductible] 'UIMBILimit',
					brpt.[Underinsured Motorists PD Limit/Deductible] 'UIMPDLimit',
					brpt.[Additional Insured Amount] 'AdditionalInsuredAmount',
					brpt.[Total Insured Value] 'TotalInsuredValue',
					brpt.[Service Fee] 'ServiceFee',
					brpt.[Stamping Fees] 'StampingFee',
					brpt.[SL Tax] 'SLTax',
					brpt.[Waiver of Subrogation] 'WaiverOfSubrogation',
					brpt.[Total Tax/Fees] 'TotalTaxFees',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Earned Premium] +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Auto Liability Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Cargo Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[AL Manuscript Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[General Liability Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[PD Manuscript Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Collision Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Comp/FT Premium Adjustment]), 0)
					END as varchar(50)), '') as 'EarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[AL Earned Premium]
					END as varchar(50)), '') as 'ALEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Collision Earned Premium]
					END as varchar(50)), '') as 'CollisionEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Comp/FT Earned Premium]
					END as varchar(50)), '') as 'CompFTEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Cargo Earned Premium]
					END as varchar(50)), '') as 'CargoEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[GL Earned Premium]
					END as varchar(50)), '') as 'GLEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[AL Manuscript Earned Premium]
					END as varchar(50)), '') as 'ALManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[PD Manuscript Earned Premium]
					END as varchar(50)), '') as 'PDManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Hired PD Earned Premium]
					END as varchar(50)), '') as 'HiredPDEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Trailer Earned Premium]
					END as varchar(50)), '') as 'TrailerEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Primary and Non-Contributory Earned Premium]
					END as varchar(50)), '') as 'PNCEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Short Rate Earned Premium]
					END as varchar(50)), '') as 'ShortRateEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Waiver of Subrogation Earned Premium]
					END as varchar(50)), '') as 'WaiverEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							coalesce(brpt.[Auto Liability Premium Adjustment], 0) +
							coalesce(brpt.[Cargo Premium Adjustment], 0) +
							coalesce(brpt.[AL Manuscript Premium Adjustment], 0) +
							coalesce(brpt.[General Liability Premium Adjustment], 0) +
							coalesce(brpt.[PD Manuscript Premium Adjustment], 0) +
							coalesce(brpt.[Collision Premium Adjustment], 0) +
							coalesce(brpt.[Comp/FT Premium Adjustment], 0)
					END as varchar(50)), '') as 'MinimumPremiumAdjustment',
					ISNULL(CAST(brpt.[Auto Liability Premium Adjustment] as varchar(50)), '') 'AutoLiabilityPremiumAdjustment',
					ISNULL(CAST(brpt.[Collision Premium Adjustment] as varchar(50)), '') 'CollisionPremiumAdjustment',
					ISNULL(CAST(brpt.[Comp/FT Premium Adjustment] as varchar(50)), '') 'CompFTPremiumAdjustment',
					ISNULL(CAST(brpt.[Cargo Premium Adjustment] as varchar(50)), '') 'CargoPremiumAdjustment',
					ISNULL(CAST(brpt.[AL Manuscript Premium Adjustment] as varchar(50)), '') 'ALManuscriptPremiumAdjustment',
					ISNULL(CAST(brpt.[General Liability Premium Adjustment] as varchar(50)), '') 'GeneralLiabilityPremiumAdjustment',
					ISNULL(CAST(brpt.[PD Manuscript Premium Adjustment] as varchar(50)), '') 'PDManuscriptPremiumAdjustment',
					brpt.[Broker Agency] 'BrokerAgency',
					brpt.Broker_Billing_Address 'Broker_Billing_Address',
					brpt.[Billing Address] 'BillingAddress',
					brpt.[Billing City] 'BillingCity',
					brpt.[Billing StateCode] 'BillingStateCode',
					brpt.[Billing ZipCode] 'BillingZipcode',
					brpt.Broker_Business_Address 'Broker_Business_Address',
					brpt.[Business Address] 'BusinessAddress',
					brpt.[Business City] 'BusinessCity',
					brpt.[Business StateCode] 'BusinessStateCode',
					brpt.[Business ZipCode] 'BusinessZipcode',
					brpt.[Broker Commision AL %] 'BrokerCommissionALPercent',
					brpt.[Broker Commision PD %] 'BrokerCommissionPDPercent',
					brpt.[Broker Commission AL] 'BrokerCommissionAL',
					brpt.[Broker Commission PD] 'BrokerCommissionPD',
					brpt.[Total Commission] 'TotalCommission',
					brpt.[ARS Commission %] 'ARSCommissionPercent',
					brpt.[ARS Commission] 'ARSCommission',
					brpt.[Payment Plan] 'PaymentPlan',
					brpt.[Credited Office] 'CreditedOffice'
				from
				(
					select
						rpt.[Policy Number],
						rpt.[Endorsement Number],
						rpt.[Transaction Number],
						rpt.[Insured Name],
						rpt.[Risk State],
						rpt.[Transaction Type],
						rpt.[Transaction Date],
						rpt.[Transaction Effective Date],
						rpt.[Effective Date],
						rpt.[Expiration Date],
						CASE WHEN rpt.[Garaging Address] IS NULL THEN '' ELSE rpt.[Garaging Address] END as 'Garaging Address',
						CASE WHEN rpt.[Garaging City] IS NULL THEN '' ELSE rpt.[Garaging City] END as 'Garaging City',
						CASE WHEN rpt.[Garaging State] IS NULL THEN '' ELSE rpt.[Garaging State] END as 'Garaging State',
						CASE WHEN rpt.[Garaging ZipCode] IS NULL THEN '' ELSE rpt.[Garaging ZipCode] END as 'Garaging ZipCode',
						CASE WHEN rpt.[Mailing Address] IS NULL THEN '' ELSE rpt.[Mailing Address] END as 'Mailing Address',
						CASE WHEN rpt.[Mailing City] IS NULL THEN '' ELSE rpt.[Mailing City] END as 'Mailing City',
						CASE WHEN rpt.[Mailing State] IS NULL THEN '' ELSE rpt.[Mailing State] END as 'Mailing State',
						CASE WHEN rpt.[Mailing ZipCode] IS NULL THEN '' ELSE rpt.[Mailing ZipCode] END as 'Mailing ZipCode',
						CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(18,2),rpt.[Vehicle/Unit Count]) END as 'Vehicle/Unit Count',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) as 'Total Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium]), 0) as 'Auto Liability Premium',
						CASE WHEN rpt.[Auto Liability Limit] IS NULL THEN '' ELSE rpt.[Auto Liability Limit] END as 'Auto Liability Limit',
						CASE WHEN rpt.[Auto Liability Deductible] IS NULL THEN '' ELSE rpt.[Auto Liability Deductible] END as 'Auto Liability Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium]), 0) as 'AL Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium]), 0) as 'General Liability Premium',
						CASE WHEN rpt.[PIP Limit] IS NULL THEN '' ELSE rpt.[PIP Limit] END as 'PIP Limit',
						CASE WHEN rpt.[Auto Medical Payment Limit] IS NULL THEN '' ELSE rpt.[Auto Medical Payment Limit] END as 'Auto Medical Payment Limit',
						rpt.Symbol,
						--CASE WHEN rpt.[Physical Damage Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Physical Damage Premium] ) END as 'Physical Damage Premium', --TODO: Removed since PD Premium is not required in the report
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Premium]), 0) as 'Collision Premium',
						CASE WHEN rpt.[Collision Deductible] IS NULL THEN '' ELSE rpt.[Collision Deductible] END as 'Collision Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium]), 0) as 'Comp/FT Premium',
						CASE WHEN rpt.[Comp/FT Deductible] IS NULL THEN '' ELSE rpt.[Comp/FT Deductible] END as 'Comp/FT Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium]), 0) as 'PD Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Motor Truck Cargo Premium]), 0) as 'Motor Truck Cargo Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium]), 0) as 'Hired PD Premium',
						CASE WHEN rpt.[Hired PD Limit] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Limit]) END as 'Hired PD Limit',
						CASE WHEN rpt.[Hired PD Deductible] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Deductible]) END as 'Hired PD Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0) as 'Trailer Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0) as 'Primary and Non-Contributory Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Premium]), 0) as 'Short Rate Premium',
						CASE WHEN rpt.[Motor Truck Cargo Limit] IS NULL THEN '' ELSE rpt.[Motor Truck Cargo Limit] END as 'Motor Truck Cargo Limit',
						CASE WHEN rpt.[Uninsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists BI Limit/Deductible] END as 'Uninsured Motorists BI Limit/Deductible',
						CASE WHEN rpt.[Uninsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists PD Limit/Deductible] END as 'Uninsured Motorists PD Limit/Deductible',
						CASE WHEN rpt.[Underinsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists BI Limit/Deductible] END as 'Underinsured Motorists BI Limit/Deductible',
						CASE WHEN rpt.[Underinsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists PD Limit/Deductible] END as 'Underinsured Motorists PD Limit/Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0) as 'Additional Insured Amount',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Insured Value]), 0) as 'Total Insured Value',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Service Fee]), 0) as 'Service Fee',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Stamping Fees]), 0) as 'Stamping Fees',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[SL Tax]), 0) as 'SL Tax',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0) as 'Waiver of Subrogation',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Tax/Fees]), 0) as 'Total Tax/Fees',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[GL Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation Earned Premium]), 0) 
						as 'Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Earned Premium]), 0) as 'AL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Collision Earned Premium]), 0) as 'Collision Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Comp/FT Earned Premium]), 0) as 'Comp/FT Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Cargo Earned Premium]), 0) as 'Cargo Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[GL Earned Premium]), 0) as'GL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Manuscript Earned Premium]), 0) as'AL Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[PD Manuscript Earned Premium]), 0) as 'PD Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Hired PD Earned Premium]), 0) as 'Hired PD Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Trailer Earned Premium]), 0) as 'Trailer Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Primary and Non-Contributory Earned Premium]), 0) as 'Primary and Non-Contributory Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Short Rate Earned Premium]), 0) as 'Short Rate Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Waiver of Subrogation Earned Premium]), 0) as 'Waiver of Subrogation Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Minimum Premium Adjustment]), 0) as 'Minimum Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[AL Earned Premium] < (rpt.[Auto Liability Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Auto Liability Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[AL Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0)
								END
						END as 'Auto Liability Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE
							CASE WHEN rpt.[Risk Status] ='Canceled'
								THEN 
									CASE WHEN (rpt.[Collision Earned Premium] < (rpt.[Collision Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
												rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
												rpt.[Transaction Effective Date] < @toDate)
										THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Collision Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[Collision Earned Premium], 0))
										ELSE 0
									END
								ELSE 0
							END
						END as 'Collision Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE
							CASE WHEN rpt.[Risk Status] ='Canceled'
								THEN 
									CASE WHEN (rpt.[Comp/FT Earned Premium] < (rpt.[Comp/FT Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
												rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
												rpt.[Transaction Effective Date] < @toDate)
										THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Comp/FT Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[Comp/FT Earned Premium], 0))
										ELSE 0
									END
								ELSE 0
							END
						END as 'Comp/FT Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[Cargo Earned Premium] < (rpt.[Motor Truck Cargo Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Motor Truck Cargo Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[Cargo Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0)
								END
						END as 'Cargo Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[AL Manuscript Earned Premium] < (rpt.[AL Manuscript Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[AL Manuscript Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[AL Manuscript Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0)
								END
						END as 'AL Manuscript Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[GL Earned Premium] < (rpt.[General Liability Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[General Liability Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[GL Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0)
								END
						END as 'General Liability Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN NULL
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[PD Manuscript Earned Premium] < (rpt.[PD Manuscript Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[PD Manuscript Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[PD Manuscript Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[pd Manuscript Premium Adjustment]), 0)
								END
						END as 'PD Manuscript Premium Adjustment',
						CASE WHEN rpt.[Broker Agency] IS NULL THEN '' ELSE rpt.[Broker Agency] END as 'Broker Agency',
						CASE WHEN rpt.Broker_Billing_Address IS NULL THEN '' ELSE rpt.Broker_Billing_Address END as 'Broker_Billing_Address',
						CASE WHEN rpt.[Billing Address] IS NULL THEN '' ELSE rpt.[Billing Address] END as 'Billing Address',
						CASE WHEN rpt.[Billing City] IS NULL THEN '' ELSE rpt.[Billing City] END as 'Billing City',
						CASE WHEN rpt.[Billing StateCode] IS NULL THEN '' ELSE rpt.[Billing StateCode] END as 'Billing StateCode',
						CASE WHEN rpt.[Billing Zipcode] IS NULL THEN '' ELSE rpt.[Billing Zipcode] END as 'Billing Zipcode',
						CASE WHEN rpt.Broker_Business_Address IS NULL THEN '' ELSE rpt.Broker_Business_Address END as 'Broker_Business_Address',
						CASE WHEN rpt.[Business Address] IS NULL THEN '' ELSE rpt.[Business Address] END as 'Business Address',
						CASE WHEN rpt.[Business City] IS NULL THEN '' ELSE rpt.[Business City] END as 'Business City',
						CASE WHEN rpt.[Business StateCode] IS NULL THEN '' ELSE rpt.[Business StateCode] END as 'Business StateCode',
						CASE WHEN rpt.[Business Zipcode] IS NULL THEN '' ELSE rpt.[Business Zipcode] END as 'Business Zipcode',
						rpt.[Broker Commision AL %] as 'Broker Commision AL %',
						rpt.[Broker Commision PD %] as 'Broker Commision PD %',
						coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) as 'Broker Commission AL',
						coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'Broker Commission PD',
						coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'Total Commission',
						CASE WHEN rpt.CreditedOfficeId = 3 
							THEN ('2.5%')
							ELSE ('0%') 
						END AS 'ARS Commission %',
						CASE WHEN rpt.CreditedOfficeId = 3 
							THEN 
								CONVERT(decimal(18,2), ((coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0)) * (2.5/100)))
							ELSE 0
						END AS 'ARS Commission',
						rpt.[Payment Plan],
						rpt.[Credited Office]
					from
						(
							select 
							--rpd.*,
							rpd.PolicyNumber as 'Policy Number',
							rpd.Status as 'Risk Status',
							rpd.[Endorsement Number],
							totaltransactions.transactionnumber as 'Transaction Number',
							rpd.[Insured Name],
							rpd.[Risk State],
							rpd.[Transaction Type],
							rpd.[Transaction Date],
							rpd.[Transaction Effective Date],
							rpd.EffectiveDate as 'Effective Date',
							rpd.ExpirationDate as 'Expiration Date',
							rpd.[Garaging Address],
							rpd.[Garaging City],
							rpd.[Garaging State],
							rpd.[Garaging ZipCode],
							rpd.[Mailing Address],
							rpd.[Mailing City],
							rpd.[Mailing State],
							rpd.[Mailing ZipCode],
							rpd.[Vehicle/Unit Count],
							rpd.[Auto Liability Limit],
							rpd.[Auto Liability Deductible],
							rpd.[PIP Limit],
							rpd.[Auto Medical Payment Limit],
							rpd.Symbol,
							rpd.[Collision Deductible],
							rpd.[Comp/FT Deductible],
							rpd.[Motor Truck Cargo Limit],
							rpd.[Uninsured Motorists BI Limit/Deductible],
							rpd.[Uninsured Motorists PD Limit/Deductible],
							rpd.[Underinsured Motorists BI Limit/Deductible],
							rpd.[Underinsured Motorists PD Limit/Deductible],
							rpd.[Hired PD Limit],
							rpd.[Hired PD Deductible],
							rpd.[Broker Agency],
							rpd.Broker_Billing_Address,
							rpd.[Billing Address],
							rpd.[Billing City],
							rpd.[Billing StateCode],
							rpd.[Billing Zipcode],
							rpd.Broker_Business_Address,
							rpd.[Business Address],
							rpd.[Business City],
							rpd.[Business StateCode],
							rpd.[Business Zipcode],
							rpd.[Broker Commision AL %],
							rpd.[Broker Commision PD %],
							rpd.CreditedOfficeId,
							rpd.[Credited Office],
							rpd.[Payment Plan],
							totalpremium.totalamount as 'Total Premium',
							totalalpremium.totalamount as 'Auto Liability Premium',
							--totalpdpremium.totalamount as 'Physical Damage Premium', --TODO: Remove if needed since PD Premium is currently not part of the report
							totalglprem.totalamount as 'General Liability Premium',
							(coalesce(totalcol.totalamount, 0) - coalesce(previoustotalcol.totalamount, 0)) as 'Collision Premium',
							(coalesce(totalcompft.totalamount, 0) - coalesce(previoustotalcompft.totalamount, 0)) as 'Comp/FT Premium',
							totalaiprem.totalamount as 'Additional Insured Amount',
							totaltiv.totalamount as 'Total Insured Value',
							totalsf.totalamount as 'Service Fee',
							totalstampingfee.totalamount as 'Stamping Fees',
							totaltax.totalamount as 'SL Tax',
							totalwaiver.totalamount as 'Waiver of Subrogation',
							totalcargoprem.totalamount as 'Motor Truck Cargo Premium',
							totalhiredpdendstpremium.totalamount as 'Hired PD Premium',
							totaltrailerinterchangepremium.totalamount as 'Trailer Premium',
							totalpncpremium.totalamount as 'Primary and Non-Contributory Premium',
							totalshortratepremium.totalamount as 'Short Rate Premium',
							totalalmprem.totalamount as 'AL Manuscript Premium',
							totalpdmprem.totalamount as 'PD Manuscript Premium',
							coalesce(totalminpremiumadjustment.totalamount, 0) as 'Minimum Premium Adjustment',
							coalesce(totalalminpremiumadjustment.totalamount, 0) as 'Auto Liability Premium Adjustment',
							coalesce(totalcargominpremiumadjustment.totalamount, 0) as 'Cargo Premium Adjustment',
							coalesce(totalalmpminpremiumadjustment.totalamount, 0) as 'AL Manuscript Premium Adjustment',
							coalesce(totalglminpremiumadjustment.totalamount, 0) as 'General Liability Premium Adjustment',
							coalesce(totalpdmpminpremiumadjustment.totalamount, 0) as 'PD Manuscript Premium Adjustment',
							(coalesce(totalsf.totalamount, 0) + coalesce(totaltax.totalamount, 0) + coalesce(totalstampingfee.totalamount, 0)) as 'Total Tax/Fees',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalpremium.totalamount, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'AL Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcompft.totalamount, 0) - coalesce(previoustotalcompft.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'Comp/FT Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcol.totalamount, 0) - coalesce(previoustotalcol.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'Collision Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalcargoprem.totalamount, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'Cargo Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalglprem.totalamount, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'GL Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalmprem.totalamount, 0)
							END as 'AL Manuscript Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpdmprem.totalamount, 0)
							END as 'PD Manuscript Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalhiredpdendstpremium.totalamount, 0)
							END as 'Hired PD Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totaltrailerinterchangepremium.totalamount, 0)
							END as 'Trailer Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpncpremium.totalamount, 0)
							END as 'Primary and Non-Contributory Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalshortratepremium.totalamount, 0)
							END as 'Short Rate Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalwaiver.totalamount, 0)
							END as 'Waiver of Subrogation Earned Premium',
							((coalesce(totalalpremium.totalamount, 0) + 
								coalesce(totalcargoprem.totalamount, 0) +
								coalesce(totalglprem.totalamount, 0) +
								coalesce(totalalmprem.totalamount, 0))
								* (cast(rpd.[Broker Commision AL %] as decimal(18,2)) / 100.0)) as CommisionALAmount,
							((coalesce(totalpdpremium.totalamount, 0) + 
								coalesce(totaltrailerinterchangepremium.totalamount, 0) + 
								coalesce(totalhiredpdendstpremium.totalamount, 0) +
								coalesce(totalpdmprem.totalamount, 0))
								* (cast(rpd.[Broker Commision PD %] as decimal(18,2)) / 100.0)) as CommisionPDAmount,
							DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate) as 'Total days',
							DATEDIFF(DAY, rpd.[Effective Date], GETDATE()) as 'Total days passed',
							totalpremium.totalamount / (DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate)) as 'Prem per day',
							MinimumEarned as 'Minimum Earned',
							CancellationDate as 'Cancellation Date'
							from
							(
								select 
									r.Id as 'RiskId',
									ph.PreviousRiskDetailId as 'RiskDetailId',
									r.PolicyNumber,
									r.CurrentRiskDetailId,
									r.Status,
									(select CancellationDate from RiskDetail 
										inner join Risk r_temp on r_temp.PolicyNumber = r.PolicyNumber
										where RiskDetail.Id = r.ForEndorsementRiskDetailId) as 'CancellationDate',
									b.MinimumEarned,
									CASE
										WHEN ph.EndorsementNumber = 0 or 
												ph.PolicyStatus = 'Canceled' or 
												ph.PolicyStatus = 'Reinstated' or 
												rd.Status = 'Rewrite' or
												(ph.PolicyStatus = 'Rewrite' and rd.Status = 'Canceled') --Canceled/Rewrite
										THEN ''
										ELSE
										ph.EndorsementNumber
									END as 'Endorsement Number',
									ph.EndorsementNumber as 'EndorsementNumberInDB',
									ie.CompanyName as 'Insured Name',
									businessaddr.State as 'Risk State',
									ph.DateCreated as 'Transaction Date',
									ph.EffectiveDate as 'Transaction Effective Date',
									CASE
										WHEN ph.EndorsementNumber = 0
										THEN 'New'
										WHEN ph.PolicyStatus = 'Reinstated'
										THEN 'Reinstatement'
										WHEN ph.PolicyStatus = 'Canceled'
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and (ph.PolicyStatus = 'Rewrite' and rd.Status = 'Canceled') --Canceled/Rewrite
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and rd.Status = 'Rewrite'
										THEN 'New'
										ELSE 'Endorsement'
									END as 'Transaction Type',
									CASE 
										WHEN ph.EndorsementNumber = 0
										THEN bi.EffectiveDate
										ELSE
											ph.EffectiveDate
									END as 'Effective Date',
									bi.ExpirationDate,
									rd.Id,
									garageaddr.StreetAddress1 as 'Garaging Address',
									garageaddr.City as 'Garaging City',
									garageaddr.State as 'Garaging State',
									garageaddr.ZipCode as 'Garaging ZipCode',
									mailingaddr.StreetAddress1 as 'Mailing Address',
									mailingaddr.City as 'Mailing City',
									mailingaddr.State as 'Mailing State',
									mailingaddr.ZipCode as 'Mailing ZipCode',
									CASE
										WHEN ph.EndorsementNumber = 0
										THEN all_vehicle.vcount
										ELSE (LEN(ph.Details) - LEN(REPLACE(ph.Details, 'Added VIN ''', ''))) / LEN('Added VIN ''')
									END as 'Vehicle/Unit Count',
									(select LimitDisplay from LvLimits where Id = rc.AutoBILimitId) as 'Auto Liability Limit',
									(select LimitDisplay from LvLimits where Id = rc.LiabilityDeductibleId) as 'Auto Liability Deductible',
									(select LimitDisplay from LvLimits where Id = rc.PIPLimitId) as 'PIP Limit',
									(select LimitDisplay from LvLimits where Id = rc.MedPayLimitId) as 'Auto Medical Payment Limit',
									CASE
										WHEN rc.ALSymbolId = 1 THEN '7, 8 & 9'
										WHEN rc.ALSymbolId = 2 THEN '7 & 9'
										WHEN rc.ALSymbolId = 3 THEN '2, 8 & 9'
										WHEN rc.ALSymbolId = 4 THEN '1) Any Auto'
										WHEN rc.ALSymbolId = 5 THEN '2) All Owned Autos'
										WHEN rc.ALSymbolId = 6 THEN '3) Owned Private Passenger Autos'
										WHEN rc.ALSymbolId = 7 THEN '4) Owned Autos Other Than Private Passenger'
										WHEN rc.ALSymbolId = 8 THEN '5) All Owned Autos Which Require No-Fault Coverage'
										WHEN rc.ALSymbolId = 9 THEN '6) Owned Autos Subject to Compulsory UM Law'
										WHEN rc.ALSymbolId = 10 THEN '7) Autos Specified on  Schedule'
										WHEN rc.ALSymbolId = 11 THEN '8) Hired Autos'
										WHEN rc.ALSymbolId = 12 THEN '9) Non-Owned Autos'
									END as 'Symbol',
									(select LimitDisplay from LvLimits where Id = rc.CollDeductibleId) as 'Collision Deductible',
									(select LimitDisplay 
										from LvLimits 
										where Id = 
										CASE
											WHEN (rc.CompDeductibleId IS NULL)
											THEN rc.FireDeductibleId
											WHEN rc.FireDeductibleId IS NULL
											THEN rc.CompDeductibleId
										END
										) as 'Comp/FT Deductible',
									(select LimitDisplay from LvLimits where Id = rc.CargoLimitId) as 'Motor Truck Cargo Limit',
									(select LimitDisplay from LvLimits where Id = rc.UMBILimitId) as 'Uninsured Motorists BI Limit/Deductible',
									(select LimitDisplay from LvLimits where Id = rc.UMPDLimitId) as 'Uninsured Motorists PD Limit/Deductible',
									(select LimitDisplay from LvLimits where Id = rc.UIMBILimitId) as 'Underinsured Motorists BI Limit/Deductible',
									(select LimitDisplay from LvLimits where Id = rc.UIMPDLimitId) as 'Underinsured Motorists PD Limit/Deductible',
									bi.EffectiveDate,
									ae.CompanyName as 'Broker Agency',
									CASE WHEN ae.IsIndividual = 1 THEN 'YES' ELSE 'NO' END as 'Individual',
									aa.StreetAddress1 as 'Broker_Billing_Address',
									aa.StreetAddress2 as 'Billing Address',
									aa.City as 'Billing City',
									aa.StateCode as 'Billing StateCode',
									aa.ZipCode as 'Billing Zipcode',
									aa2.StreetAddress1 as 'Broker_Business_Address',
									aa2.StreetAddress2 as 'Business Address',
									aa2.City as 'Business City',
									aa2.StateCode as 'Business StateCode',
									aa2.ZipCode as 'Business Zipcode',
									rc.BrokerCommisionAL as 'Broker Commision AL %',
									rc.BrokerCommisionPD as 'Broker Commision PD %',
									b.CreditedOfficeId,
									(select [Description] from CreditedOffices where Id = b.CreditedOfficeId) as 'Credited Office',
									(select [Description] from PaymentTypes where Id = b.PaymentTypesId) as 'Payment Plan',
									ph.Details,
									(select SUBSTRING(value,
											PATINDEX('%[0-9]%', value),
											(CASE WHEN PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), '')) = 0
											THEN LEN(value) ELSE (PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), ''))) - 1
											END)) as result
										from STRING_SPLIT(rf.Other, ',') where value like '%physdamLimit%') as 'Hired PD Limit',
									(select SUBSTRING(value,
											PATINDEX('%[0-9]%', value),
											(CASE WHEN PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), '')) = 0
											THEN LEN(value) ELSE (PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), ''))) - 1
											END)) as result 
										from STRING_SPLIT(rf.Other, ',') where value like '%deductible%') as 'Hired PD Deductible'
								from PolicyHistory ph
								inner join Risk r on ph.RiskId = r.Id
								inner join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1 --TODO: To be confirmed if IsProcessed is to be removed from query so all policies are captured
								inner join [Binding] b on b.RiskId = r.Id
								inner join BrokerInfo bi on bi.RiskId = r.Id
								inner join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
								inner join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
								inner join RiskForm rf on rf.RiskDetailId = rd.Id and rf.FormId = 'HiredPhysicalDamage'
								inner join Insured i on rd.InsuredId = i.Id
								inner join Entity ie on i.EntityId = ie.Id
								inner join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
								inner join [Address] businessaddr on business.AddressId = businessaddr.Id
								left join EntityAddress garage on garage.EntityId = ie.Id and garage.AddressTypeId = 3 and garage.IsMainGaragingAddress = 1
								left join [Address] garageaddr on garage.AddressId = garageaddr.Id
								left join EntityAddress mailing on mailing.EntityId = ie.Id and mailing.AddressTypeId = 4
								left join [Address] mailingaddr on mailing.AddressId = mailingaddr.Id
								left join Agency_Agency agency on agency.Id = bi.AgencyId
								left join Agency_Entity ae on ae.Id = agency.EntityId
								left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
								left join Agency_Address aa on aea.AddressId = aa.Id
								left join Agency_EntityAddress aea2 on ae.Id = aea2.EntityId and aea2.AddressTypeId = 2
								left join Agency_Address aa2 on aea2.AddressId = aa2.Id
								left join BusinessTypes bt on ie.BusinessTypeID = bt.Id
								left join (
									select rdv.RiskDetailId, COUNT(rdv.RiskDetailId) as vcount from RiskDetailVehicle rdv
									group by rdv.RiskDetailId
								) all_vehicle on all_vehicle.RiskDetailId = rd.Id
								where ph.DateCreated IS NOT NULL
							) rpd
							cross apply
							(
								select COUNT(*) as transactionnumber from PolicyHistory ph
								where ph.RiskId = rpd.RiskId and ph.DateCreated <= rpd.[Transaction Date]
							) totaltransactions
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction bt
								join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
								where btd.AmountTypeId = 'P' and 
									bt.RiskId = rpd.RiskId and
									bt.TransactionTypeId = 
									CASE	
										WHEN rpd.[Transaction Type] = 'New' THEN 'N'
										WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
										WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
										ELSE 'E'
									END
									and
									bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction bt
								join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
								where btd.AmountSubTypeId = 'AIP' and btd.AmountTypeId = 'P' and
									bt.RiskId = rpd.RiskId and
									bt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalaiprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALP' or 
										btd.AmountSubTypeId = 'AIP' or 
										btd.AmountSubTypeId = 'WS' or 
										btd.AmountSubTypeId = 'PNC') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalalpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDP') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalpdpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'TIE') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totaltrailerinterchangepremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'HPDE') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalhiredpdendstpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PNC') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalpncpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALPSRP' or 
										btd.AmountSubTypeId = 'CPSRP' or 
										btd.AmountSubTypeId = 'ALMPSRP' or 
										btd.AmountSubTypeId = 'GLPSRP' or 
										btd.AmountSubTypeId = 'PDPSRP' or 
										btd.AmountSubTypeId = 'PDMSRP') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalshortratepremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALSF' or btd.AmountSubTypeId = 'PDSF') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalsf
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'SF') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalstampingfee
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'SLT') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totaltax
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'WS') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalwaiver
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALMP') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalalmprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDM') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalpdmprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'CP') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalcargoprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'GLP') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalglprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALPMPA' or 
										btd.AmountSubTypeId = 'CPMPA' or 
										btd.AmountSubTypeId = 'ALMPMPA' or 
										btd.AmountSubTypeId = 'GLPMPA' or
										btd.AmountSubTypeId = 'PDPMPA' or 
										btd.AmountSubTypeId = 'PDMMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalalminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'CPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalcargominpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALMPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalalmpminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'GLPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalglminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalpdminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDMMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalpdmpminpremiumadjustment
							cross apply
							(
								select SUM(rvpt.CurrentCollPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcol
							cross apply
							(
								select SUM(rvpt.CurrentCollPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								cross apply (
									select top 1 * from RiskDetail rd
									where rd.Id = rvpt.RiskDetailId
									order by rd.CreatedDate desc
								) RiskDetail
								inner join (
									select top 1 * from PolicyHistory ph
									where ph.RiskId = rpd.RiskId and
											ph.DateCreated < rpd.[Transaction Date] and 
											ph.EndorsementNumber = (rpd.EndorsementNumberInDB - (1 - IIF(rpd.[Transaction Type] = 'Reinstatement', 1,0)))
									order by ph.DateCreated desc
								) as PolicyHistory on PolicyHistory.PreviousRiskDetailId = RiskDetail.Id
								where rvpt.FromReinstatement = 
									CASE
										WHEN PolicyHistory.PolicyStatus = 'Reinstated'
										THEN 1
										ELSE 0
									END
							) previoustotalcol
							cross apply
							(
								select SUM(rvpt.CurrentCompPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcompft
							cross apply
							(
								select SUM(rvpt.CurrentCompPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								cross apply (
									select top 1 * from RiskDetail rd
									where rd.Id = rvpt.RiskDetailId
									order by rd.CreatedDate desc
								) RiskDetail
								inner join (
									select top 1 * from PolicyHistory ph
									where ph.RiskId = rpd.RiskId and
											ph.DateCreated < rpd.[Transaction Date] and 
											ph.EndorsementNumber = (rpd.EndorsementNumberInDB - (1 - IIF(rpd.[Transaction Type] = 'Reinstatement', 1,0)))
									order by ph.DateCreated desc
								) as PolicyHistory on PolicyHistory.PreviousRiskDetailId = RiskDetail.Id
								where rvpt.FromReinstatement = 
									CASE
										WHEN PolicyHistory.PolicyStatus = 'Reinstated'
										THEN 1
										ELSE 0
									END
							) previoustotalcompft
							cross apply
							(
								select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle rdv
								join Vehicle v on rdv.VehicleId = v.Id
								where rpd.RiskDetailId = rdv.RiskDetailId and (rpd.[Endorsement Number] = 0 or rpd.Details LIKE '%Added VIN ''' + v.VIN + '''%')
							) totaltiv
						) rpt
					--where rpt.RiskId = '46498d0f-4102-4d70-9bf8-d6256176cfa0'
					where rpt.[Policy Number] IS NOT NULL and rpt.[Insured Name] IS NOT NULL
					and rpt.[Transaction Date] >= @fromDate
					and rpt.[Transaction Date] <= @toDate
				) brpt
				order by brpt.[Policy Number], brpt.[Transaction Number]

			END";
        const string REVERT_BorderauxReport = @"
            CREATE OR ALTER   PROCEDURE[dbo].[GenerateBorderauxReport]
				@fromDate datetime = NULL,
				@toDate datetime = NULL
			AS
			BEGIN

                SET NOCOUNT ON;

				select
					brpt.[Policy Number] 'PolicyNumber',
					brpt.[Endorsement Number] 'EndorsementNumber',
					brpt.[Transaction Number] 'TransactionNumber',
					brpt.[Insured Name] 'InsuredName',
					brpt.[Risk State] 'RiskState',
					brpt.[Transaction Type] 'TransactionType',
					brpt.[Transaction Date] 'TransactionDate',
					brpt.[Transaction Effective Date] 'TransactionEffectiveDate',
					brpt.[Effective Date] 'EffectiveDate',
					brpt.[Expiration Date] 'ExpirationDate',
					brpt.[Garaging Address] 'GaragingAddress',
					brpt.[Garaging City] 'GaragingCity',
					brpt.[Garaging State] 'GaragingState',
					brpt.[Garaging ZipCode] 'GaragingZipCode',
					brpt.[Mailing Address] 'MailingAddress',
					brpt.[Mailing City] 'MailingCity',
					brpt.[Mailing State] 'MailingState',
					brpt.[Mailing ZipCode] 'MailingZipCode',
					brpt.[Vehicle/Unit Count] 'VehicleUnitCount',
					brpt.[Total Premium] 'TotalPremium',
					brpt.[Auto Liability Premium] 'AutoLiabilityPremium',
					brpt.[Auto Liability Limit] 'AutoLiabilityLimit',
					brpt.[Auto Liability Deductible] 'AutoLiabilityDeductible',
					brpt.[AL Manuscript Premium] 'ALManuscriptPremium',
					brpt.[General Liability Premium] 'GeneralLiabilityPremium',
					brpt.[PIP Limit] 'PIPLimit',
					brpt.[Auto Medical Payment Limit] 'AutoMedicalPaymentLimit',
					brpt.Symbol,
					brpt.[Collision Premium] 'CollisionPremium',
					brpt.[Collision Deductible] 'CollisionDeductible',
					brpt.[Comp/FT Premium] 'CompFTPremium',
					brpt.[Comp/FT Deductible] 'CompFTDeductible',
					brpt.[PD Manuscript Premium] 'PDManuscriptPremium',
					brpt.[Motor Truck Cargo Premium] 'CargoPremium',
					brpt.[Hired PD Premium] 'HiredPDPremium',
					brpt.[Hired PD Limit] 'HiredPDLimit',
					brpt.[Hired PD Deductible] 'HiredPDDeductible',
					brpt.[Trailer Premium] 'TrailerPremium',
					brpt.[Primary and Non-Contributory Premium] 'PNCPremium',
					brpt.[Short Rate Premium] 'ShortRatePremium',
					brpt.[Motor Truck Cargo Limit] 'CargoLimit',
					brpt.[Uninsured Motorists BI Limit/Deductible] 'UMBILimit',
					brpt.[Uninsured Motorists PD Limit/Deductible] 'UMPDLimit',
					brpt.[Underinsured Motorists BI Limit/Deductible] 'UIMBILimit',
					brpt.[Underinsured Motorists PD Limit/Deductible] 'UIMPDLimit',
					brpt.[Additional Insured Amount] 'AdditionalInsuredAmount',
					brpt.[Total Insured Value] 'TotalInsuredValue',
					brpt.[Service Fee] 'ServiceFee',
					brpt.[Stamping Fees] 'StampingFee',
					brpt.[SL Tax] 'SLTax',
					brpt.[Waiver of Subrogation] 'WaiverOfSubrogation',
					brpt.[Total Tax/Fees] 'TotalTaxFees',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Earned Premium] +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Auto Liability Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Cargo Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[AL Manuscript Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[General Liability Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Physical Damage Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[PD Manuscript Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Collision Premium Adjustment]), 0) +
							coalesce(CONVERT(DECIMAL(18,2),brpt.[Comp/FT Premium Adjustment]), 0)
					END as varchar(50)), '') as 'EarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[AL Earned Premium]
					END as varchar(50)), '') as 'ALEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Collision Earned Premium]
					END as varchar(50)), '') as 'CollisionEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Comp/FT Earned Premium]
					END as varchar(50)), '') as 'CompFTEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Cargo Earned Premium]
					END as varchar(50)), '') as 'CargoEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[GL Earned Premium]
					END as varchar(50)), '') as 'GLEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[AL Manuscript Earned Premium]
					END as varchar(50)), '') as 'ALManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[PD Manuscript Earned Premium]
					END as varchar(50)), '') as 'PDManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Hired PD Earned Premium]
					END as varchar(50)), '') as 'HiredPDEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Trailer Earned Premium]
					END as varchar(50)), '') as 'TrailerEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Primary and Non-Contributory Earned Premium]
					END as varchar(50)), '') as 'PNCEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Short Rate Earned Premium]
					END as varchar(50)), '') as 'ShortRateEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE brpt.[Waiver of Subrogation Earned Premium]
					END as varchar(50)), '') as 'WaiverEarnedPremium',
					CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN brpt.[Minimum Premium Adjustment]
						ELSE 
							coalesce(brpt.[Auto Liability Premium Adjustment], 0) +
							coalesce(brpt.[Cargo Premium Adjustment], 0) +
							coalesce(brpt.[AL Manuscript Premium Adjustment], 0) +
							coalesce(brpt.[General Liability Premium Adjustment], 0) +
							coalesce(brpt.[Physical Damage Premium Adjustment], 0) +
							coalesce(brpt.[PD Manuscript Premium Adjustment], 0) +
							coalesce(brpt.[Collision Premium Adjustment], 0) +
							coalesce(brpt.[Comp/FT Premium Adjustment], 0)
					END as 'MinimumPremiumAdjustment',
					brpt.[Auto Liability Premium Adjustment] 'AutoLiabilityPremiumAdjustment',
					brpt.[Collision Premium Adjustment] 'CollisionPremiumAdjustment',
					brpt.[Comp/FT Premium Adjustment] 'CompFTPremiumAdjustment',
					brpt.[Cargo Premium Adjustment] 'CargoPremiumAdjustment',
					brpt.[AL Manuscript Premium Adjustment] 'ALManuscriptPremiumAdjustment',
					brpt.[General Liability Premium Adjustment] 'GeneralLiabilityPremiumAdjustment',
					brpt.[Physical Damage Premium Adjustment] 'PhysicalDamagePremiumAdjustment',
					brpt.[PD Manuscript Premium Adjustment] 'PDManuscriptPremiumAdjustment',
					brpt.[Broker Agency] 'BrokerAgency',
					brpt.Broker_Billing_Address 'Broker_Billing_Address',
					brpt.[Billing Address] 'BillingAddress',
					brpt.[Billing City] 'BillingCity',
					brpt.[Billing StateCode] 'BillingStateCode',
					brpt.[Billing ZipCode] 'BillingZipcode',
					brpt.Broker_Business_Address 'Broker_Business_Address',
					brpt.[Business Address] 'BusinessAddress',
					brpt.[Business City] 'BusinessCity',
					brpt.[Business StateCode] 'BusinessStateCode',
					brpt.[Business ZipCode] 'BusinessZipcode',
					brpt.[Broker Commision AL %] 'BrokerCommissionALPercent',
					brpt.[Broker Commision PD %] 'BrokerCommissionPDPercent',
					brpt.[Broker Commission AL] 'BrokerCommissionAL',
					brpt.[Broker Commission PD] 'BrokerCommissionPD',
					brpt.[Total Commission] 'TotalCommission',
					brpt.[ARS Commission %] 'ARSCommissionPercent',
					brpt.[ARS Commission] 'ARSCommission',
					brpt.[Payment Plan] 'PaymentPlan',
					brpt.[Credited Office] 'CreditedOffice'
				from
				(
					select
						rpt.[Policy Number],
						rpt.[Endorsement Number],
						rpt.[Transaction Number],
						rpt.[Insured Name],
						rpt.[Risk State],
						rpt.[Transaction Type],
						rpt.[Transaction Date],
						rpt.[Transaction Effective Date],
						rpt.[Effective Date],
						rpt.[Expiration Date],
						CASE WHEN rpt.[Garaging Address] IS NULL THEN '' ELSE rpt.[Garaging Address] END as 'Garaging Address',
						CASE WHEN rpt.[Garaging City] IS NULL THEN '' ELSE rpt.[Garaging City] END as 'Garaging City',
						CASE WHEN rpt.[Garaging State] IS NULL THEN '' ELSE rpt.[Garaging State] END as 'Garaging State',
						CASE WHEN rpt.[Garaging ZipCode] IS NULL THEN '' ELSE rpt.[Garaging ZipCode] END as 'Garaging ZipCode',
						CASE WHEN rpt.[Mailing Address] IS NULL THEN '' ELSE rpt.[Mailing Address] END as 'Mailing Address',
						CASE WHEN rpt.[Mailing City] IS NULL THEN '' ELSE rpt.[Mailing City] END as 'Mailing City',
						CASE WHEN rpt.[Mailing State] IS NULL THEN '' ELSE rpt.[Mailing State] END as 'Mailing State',
						CASE WHEN rpt.[Mailing ZipCode] IS NULL THEN '' ELSE rpt.[Mailing ZipCode] END as 'Mailing ZipCode',
						CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(18,2),rpt.[Vehicle/Unit Count]) END as 'Vehicle/Unit Count',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) as 'Total Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium]), 0) as 'Auto Liability Premium',
						CASE WHEN rpt.[Auto Liability Limit] IS NULL THEN '' ELSE rpt.[Auto Liability Limit] END as 'Auto Liability Limit',
						CASE WHEN rpt.[Auto Liability Deductible] IS NULL THEN '' ELSE rpt.[Auto Liability Deductible] END as 'Auto Liability Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium]), 0) as 'AL Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium]), 0) as 'General Liability Premium',
						CASE WHEN rpt.[PIP Limit] IS NULL THEN '' ELSE rpt.[PIP Limit] END as 'PIP Limit',
						CASE WHEN rpt.[Auto Medical Payment Limit] IS NULL THEN '' ELSE rpt.[Auto Medical Payment Limit] END as 'Auto Medical Payment Limit',
						rpt.Symbol,
						--CASE WHEN rpt.[Physical Damage Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Physical Damage Premium] ) END as 'Physical Damage Premium', --TODO: Removed since PD Premium is not required in the report
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Premium]), 0) as 'Collision Premium',
						CASE WHEN rpt.[Collision Deductible] IS NULL THEN '' ELSE rpt.[Collision Deductible] END as 'Collision Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium]), 0) as 'Comp/FT Premium',
						CASE WHEN rpt.[Comp/FT Deductible] IS NULL THEN '' ELSE rpt.[Comp/FT Deductible] END as 'Comp/FT Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium]), 0) as 'PD Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Motor Truck Cargo Premium]), 0) as 'Motor Truck Cargo Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium]), 0) as 'Hired PD Premium',
						CASE WHEN rpt.[Hired PD Limit] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Limit]) END as 'Hired PD Limit',
						CASE WHEN rpt.[Hired PD Deductible] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Deductible]) END as 'Hired PD Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0) as 'Trailer Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0) as 'Primary and Non-Contributory Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Premium]), 0) as 'Short Rate Premium',
						CASE WHEN rpt.[Motor Truck Cargo Limit] IS NULL THEN '' ELSE rpt.[Motor Truck Cargo Limit] END as 'Motor Truck Cargo Limit',
						CASE WHEN rpt.[Uninsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists BI Limit/Deductible] END as 'Uninsured Motorists BI Limit/Deductible',
						CASE WHEN rpt.[Uninsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists PD Limit/Deductible] END as 'Uninsured Motorists PD Limit/Deductible',
						CASE WHEN rpt.[Underinsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists BI Limit/Deductible] END as 'Underinsured Motorists BI Limit/Deductible',
						CASE WHEN rpt.[Underinsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists PD Limit/Deductible] END as 'Underinsured Motorists PD Limit/Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0) as 'Additional Insured Amount',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Insured Value]), 0) as 'Total Insured Value',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Service Fee]), 0) as 'Service Fee',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Stamping Fees]), 0) as 'Stamping Fees',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[SL Tax]), 0) as 'SL Tax',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0) as 'Waiver of Subrogation',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Tax/Fees]), 0) as 'Total Tax/Fees',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[GL Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation Earned Premium]), 0) 
						as 'Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Earned Premium]), 0) as 'AL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Collision Earned Premium]), 0) as 'Collision Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Comp/FT Earned Premium]), 0) as 'Comp/FT Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Cargo Earned Premium]), 0) as 'Cargo Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[GL Earned Premium]), 0) as'GL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Manuscript Earned Premium]), 0) as'AL Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[PD Manuscript Earned Premium]), 0) as 'PD Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Hired PD Earned Premium]), 0) as 'Hired PD Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Trailer Earned Premium]), 0) as 'Trailer Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Primary and Non-Contributory Earned Premium]), 0) as 'Primary and Non-Contributory Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Short Rate Earned Premium]), 0) as 'Short Rate Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Waiver of Subrogation Earned Premium]), 0) as 'Waiver of Subrogation Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Minimum Premium Adjustment]), 0) as 'Minimum Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0)
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[AL Earned Premium] < (rpt.[Auto Liability Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Auto Liability Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[AL Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0)
								END
						END as 'Auto Liability Premium Adjustment',
						CASE WHEN rpt.[Risk Status] ='Canceled'
							THEN 
								CASE WHEN (rpt.[Collision Earned Premium] < (rpt.[Collision Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
											rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
											rpt.[Transaction Effective Date] < @toDate)
									THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Collision Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[Collision Earned Premium], 0))
									ELSE 0
								END
							ELSE 0
						END as 'Collision Premium Adjustment',
						CASE WHEN rpt.[Risk Status] ='Canceled'
							THEN 
								CASE WHEN (rpt.[Comp/FT Earned Premium] < (rpt.[Comp/FT Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
											rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
											rpt.[Transaction Effective Date] < @toDate)
									THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Comp/FT Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[Comp/FT Earned Premium], 0))
									ELSE 0
								END
							ELSE 0
						END as 'Comp/FT Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0)
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[Cargo Earned Premium] < (rpt.[Motor Truck Cargo Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[Motor Truck Cargo Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[Cargo Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0)
								END
						END as 'Cargo Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0)
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[AL Manuscript Earned Premium] < (rpt.[AL Manuscript Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[AL Manuscript Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[AL Manuscript Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0)
								END
						END as 'AL Manuscript Premium Adjustment',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation' OR rpt.[Transaction Type] = 'Reinstatement'
							THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0)
							ELSE 
								CASE WHEN rpt.[Risk Status] ='Canceled'
									THEN 
										CASE WHEN (rpt.[GL Earned Premium] < (rpt.[General Liability Premium] * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) AND
													rpt.[Transaction Effective Date] < rpt.[Cancellation Date] AND
													rpt.[Transaction Effective Date] < @toDate)
											THEN CONVERT(DECIMAL(18,2), (coalesce(rpt.[General Liability Premium], 0) * (coalesce(rpt.[Minimum Earned], 0) / 100.0)) - coalesce(rpt.[GL Earned Premium], 0))
											ELSE 0
										END
									ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0)
								END
						END as 'General Liability Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Premium Adjustment]), 0) as 'Physical Damage Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium Adjustment]), 0) as 'PD Manuscript Premium Adjustment',
						CASE WHEN rpt.[Broker Agency] IS NULL THEN '' ELSE rpt.[Broker Agency] END as 'Broker Agency',
						CASE WHEN rpt.Broker_Billing_Address IS NULL THEN '' ELSE rpt.Broker_Billing_Address END as 'Broker_Billing_Address',
						CASE WHEN rpt.[Billing Address] IS NULL THEN '' ELSE rpt.[Billing Address] END as 'Billing Address',
						CASE WHEN rpt.[Billing City] IS NULL THEN '' ELSE rpt.[Billing City] END as 'Billing City',
						CASE WHEN rpt.[Billing StateCode] IS NULL THEN '' ELSE rpt.[Billing StateCode] END as 'Billing StateCode',
						CASE WHEN rpt.[Billing Zipcode] IS NULL THEN '' ELSE rpt.[Billing Zipcode] END as 'Billing Zipcode',
						CASE WHEN rpt.Broker_Business_Address IS NULL THEN '' ELSE rpt.Broker_Business_Address END as 'Broker_Business_Address',
						CASE WHEN rpt.[Business Address] IS NULL THEN '' ELSE rpt.[Business Address] END as 'Business Address',
						CASE WHEN rpt.[Business City] IS NULL THEN '' ELSE rpt.[Business City] END as 'Business City',
						CASE WHEN rpt.[Business StateCode] IS NULL THEN '' ELSE rpt.[Business StateCode] END as 'Business StateCode',
						CASE WHEN rpt.[Business Zipcode] IS NULL THEN '' ELSE rpt.[Business Zipcode] END as 'Business Zipcode',
						rpt.[Broker Commision AL %] as 'Broker Commision AL %',
						rpt.[Broker Commision PD %] as 'Broker Commision PD %',
						coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) as 'Broker Commission AL',
						coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'Broker Commission PD',
						coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'Total Commission',
						CASE WHEN rpt.CreditedOfficeId = 3 
							THEN ('2.5%')
							ELSE ('0%') 
						END AS 'ARS Commission %',
						CASE WHEN rpt.CreditedOfficeId = 3 
							THEN 
								CONVERT(decimal(18,2), ((coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0)) * (2.5/100)))
							ELSE 0
						END AS 'ARS Commission',
						rpt.[Payment Plan],
						rpt.[Credited Office]
					from
						(
							select 
							--rpd.*,
							rpd.PolicyNumber as 'Policy Number',
							rpd.Status as 'Risk Status',
							rpd.[Endorsement Number],
							totaltransactions.transactionnumber as 'Transaction Number',
							rpd.[Insured Name],
							rpd.[Risk State],
							rpd.[Transaction Type],
							rpd.[Transaction Date],
							rpd.[Transaction Effective Date],
							rpd.EffectiveDate as 'Effective Date',
							rpd.ExpirationDate as 'Expiration Date',
							rpd.[Garaging Address],
							rpd.[Garaging City],
							rpd.[Garaging State],
							rpd.[Garaging ZipCode],
							rpd.[Mailing Address],
							rpd.[Mailing City],
							rpd.[Mailing State],
							rpd.[Mailing ZipCode],
							rpd.[Vehicle/Unit Count],
							rpd.[Auto Liability Limit],
							rpd.[Auto Liability Deductible],
							rpd.[PIP Limit],
							rpd.[Auto Medical Payment Limit],
							rpd.Symbol,
							rpd.[Collision Deductible],
							rpd.[Comp/FT Deductible],
							rpd.[Motor Truck Cargo Limit],
							rpd.[Uninsured Motorists BI Limit/Deductible],
							rpd.[Uninsured Motorists PD Limit/Deductible],
							rpd.[Underinsured Motorists BI Limit/Deductible],
							rpd.[Underinsured Motorists PD Limit/Deductible],
							rpd.[Hired PD Limit],
							rpd.[Hired PD Deductible],
							rpd.[Broker Agency],
							rpd.Broker_Billing_Address,
							rpd.[Billing Address],
							rpd.[Billing City],
							rpd.[Billing StateCode],
							rpd.[Billing Zipcode],
							rpd.Broker_Business_Address,
							rpd.[Business Address],
							rpd.[Business City],
							rpd.[Business StateCode],
							rpd.[Business Zipcode],
							rpd.[Broker Commision AL %],
							rpd.[Broker Commision PD %],
							rpd.CreditedOfficeId,
							rpd.[Credited Office],
							rpd.[Payment Plan],
							totalpremium.totalamount as 'Total Premium',
							totalalpremium.totalamount as 'Auto Liability Premium',
							--totalpdpremium.totalamount as 'Physical Damage Premium', --TODO: Remove if needed since PD Premium is currently not part of the report
							totalglprem.totalamount as 'General Liability Premium',
							(coalesce(totalcol.totalamount, 0) - coalesce(previoustotalcol.totalamount, 0)) as 'Collision Premium',
							(coalesce(totalcompft.totalamount, 0) - coalesce(previoustotalcompft.totalamount, 0)) as 'Comp/FT Premium',
							totalaiprem.totalamount as 'Additional Insured Amount',
							totaltiv.totalamount as 'Total Insured Value',
							totalsf.totalamount as 'Service Fee',
							totalstampingfee.totalamount as 'Stamping Fees',
							totaltax.totalamount as 'SL Tax',
							totalwaiver.totalamount as 'Waiver of Subrogation',
							totalcargoprem.totalamount as 'Motor Truck Cargo Premium',
							totalhiredpdendstpremium.totalamount as 'Hired PD Premium',
							totaltrailerinterchangepremium.totalamount as 'Trailer Premium',
							totalpncpremium.totalamount as 'Primary and Non-Contributory Premium',
							totalshortratepremium.totalamount as 'Short Rate Premium',
							totalalmprem.totalamount as 'AL Manuscript Premium',
							totalpdmprem.totalamount as 'PD Manuscript Premium',
							coalesce(totalminpremiumadjustment.totalamount, 0) as 'Minimum Premium Adjustment',
							coalesce(totalalminpremiumadjustment.totalamount, 0) as 'Auto Liability Premium Adjustment',
							coalesce(totalcargominpremiumadjustment.totalamount, 0) as 'Cargo Premium Adjustment',
							coalesce(totalalmpminpremiumadjustment.totalamount, 0) as 'AL Manuscript Premium Adjustment',
							coalesce(totalglminpremiumadjustment.totalamount, 0) as 'General Liability Premium Adjustment',
							coalesce(totalpdminpremiumadjustment.totalamount, 0) as 'Physical Damage Premium Adjustment',
							coalesce(totalpdmpminpremiumadjustment.totalamount, 0) as 'PD Manuscript Premium Adjustment',
							(coalesce(totalsf.totalamount, 0) + coalesce(totaltax.totalamount, 0) + coalesce(totalstampingfee.totalamount, 0)) as 'Total Tax/Fees',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalpremium.totalamount, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'AL Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcompft.totalamount, 0) - coalesce(previoustotalcompft.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'Comp/FT Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcol.totalamount, 0) - coalesce(previoustotalcol.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'Collision Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalcargoprem.totalamount, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'Cargo Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalglprem.totalamount, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN rpd.Status = 'Canceled'
											THEN
												CASE WHEN rpd.CancellationDate <= @toDate
													THEN rpd.CancellationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											ELSE
												CASE WHEN rpd.ExpirationDate <= @toDate
													THEN rpd.ExpirationDate
													ELSE DATEADD(day, 1, @toDate)
												END
											END
										)
									)
								) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							END as 'GL Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalmprem.totalamount, 0)
							END as 'AL Manuscript Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpdmprem.totalamount, 0)
							END as 'PD Manuscript Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalhiredpdendstpremium.totalamount, 0)
							END as 'Hired PD Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totaltrailerinterchangepremium.totalamount, 0)
							END as 'Trailer Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpncpremium.totalamount, 0)
							END as 'Primary and Non-Contributory Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalshortratepremium.totalamount, 0)
							END as 'Short Rate Earned Premium',
							CASE WHEN (rpd.Status = 'Canceled' AND rpd.[Transaction Effective Date] > rpd.CancellationDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalwaiver.totalamount, 0)
							END as 'Waiver of Subrogation Earned Premium',
							((coalesce(totalalpremium.totalamount, 0) + 
								coalesce(totalcargoprem.totalamount, 0) +
								coalesce(totalglprem.totalamount, 0) +
								coalesce(totalalmprem.totalamount, 0))
								* (cast(rpd.[Broker Commision AL %] as decimal(18,2)) / 100.0)) as CommisionALAmount,
							((coalesce(totalpdpremium.totalamount, 0) + 
								coalesce(totaltrailerinterchangepremium.totalamount, 0) + 
								coalesce(totalhiredpdendstpremium.totalamount, 0) +
								coalesce(totalpdmprem.totalamount, 0))
								* (cast(rpd.[Broker Commision PD %] as decimal(18,2)) / 100.0)) as CommisionPDAmount,
							DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate) as 'Total days',
							DATEDIFF(DAY, rpd.[Effective Date], GETDATE()) as 'Total days passed',
							totalpremium.totalamount / (DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate)) as 'Prem per day',
							MinimumEarned as 'Minimum Earned',
							CancellationDate as 'Cancellation Date'
							from
							(
								select 
									r.Id as 'RiskId',
									ph.PreviousRiskDetailId as 'RiskDetailId',
									r.PolicyNumber,
									r.CurrentRiskDetailId,
									r.Status,
									(select CancellationDate from RiskDetail 
										inner join Risk r_temp on r_temp.PolicyNumber = r.PolicyNumber
										where RiskDetail.Id = r.ForEndorsementRiskDetailId) as 'CancellationDate',
									b.MinimumEarned,
									CASE
										WHEN ph.EndorsementNumber = 0 or 
												ph.PolicyStatus = 'Canceled' or 
												ph.PolicyStatus = 'Reinstated' or 
												rd.Status = 'Rewrite' or
												(ph.PolicyStatus = 'Rewrite' and rd.Status = 'Canceled') --Canceled/Rewrite
										THEN ''
										ELSE
										ph.EndorsementNumber
									END as 'Endorsement Number',
									ph.EndorsementNumber as 'EndorsementNumberInDB',
									ie.CompanyName as 'Insured Name',
									businessaddr.State as 'Risk State',
									ph.DateCreated as 'Transaction Date',
									ph.EffectiveDate as 'Transaction Effective Date',
									CASE
										WHEN ph.EndorsementNumber = 0
										THEN 'New'
										WHEN ph.PolicyStatus = 'Reinstated'
										THEN 'Reinstatement'
										WHEN ph.PolicyStatus = 'Canceled'
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and (ph.PolicyStatus = 'Rewrite' and rd.Status = 'Canceled') --Canceled/Rewrite
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and rd.Status = 'Rewrite'
										THEN 'New'
										ELSE 'Endorsement'
									END as 'Transaction Type',
									CASE 
										WHEN ph.EndorsementNumber = 0
										THEN bi.EffectiveDate
										ELSE
											ph.EffectiveDate
									END as 'Effective Date',
									bi.ExpirationDate,
									rd.Id,
									garageaddr.StreetAddress1 as 'Garaging Address',
									garageaddr.City as 'Garaging City',
									garageaddr.State as 'Garaging State',
									garageaddr.ZipCode as 'Garaging ZipCode',
									mailingaddr.StreetAddress1 as 'Mailing Address',
									mailingaddr.City as 'Mailing City',
									mailingaddr.State as 'Mailing State',
									mailingaddr.ZipCode as 'Mailing ZipCode',
									CASE
										WHEN ph.EndorsementNumber = 0
										THEN all_vehicle.vcount
										ELSE (LEN(ph.Details) - LEN(REPLACE(ph.Details, 'Added VIN ''', ''))) / LEN('Added VIN ''')
									END as 'Vehicle/Unit Count',
									(select LimitDisplay from LvLimits where Id = rc.AutoBILimitId) as 'Auto Liability Limit',
									(select LimitDisplay from LvLimits where Id = rc.LiabilityDeductibleId) as 'Auto Liability Deductible',
									(select LimitDisplay from LvLimits where Id = rc.PIPLimitId) as 'PIP Limit',
									(select LimitDisplay from LvLimits where Id = rc.MedPayLimitId) as 'Auto Medical Payment Limit',
									CASE
										WHEN rc.ALSymbolId = 1 THEN '7, 8 & 9'
										WHEN rc.ALSymbolId = 2 THEN '7 & 9'
										WHEN rc.ALSymbolId = 3 THEN '2, 8 & 9'
										WHEN rc.ALSymbolId = 4 THEN '1) Any Auto'
										WHEN rc.ALSymbolId = 5 THEN '2) All Owned Autos'
										WHEN rc.ALSymbolId = 6 THEN '3) Owned Private Passenger Autos'
										WHEN rc.ALSymbolId = 7 THEN '4) Owned Autos Other Than Private Passenger'
										WHEN rc.ALSymbolId = 8 THEN '5) All Owned Autos Which Require No-Fault Coverage'
										WHEN rc.ALSymbolId = 9 THEN '6) Owned Autos Subject to Compulsory UM Law'
										WHEN rc.ALSymbolId = 10 THEN '7) Autos Specified on  Schedule'
										WHEN rc.ALSymbolId = 11 THEN '8) Hired Autos'
										WHEN rc.ALSymbolId = 12 THEN '9) Non-Owned Autos'
									END as 'Symbol',
									(select LimitDisplay from LvLimits where Id = rc.CollDeductibleId) as 'Collision Deductible',
									(select LimitDisplay 
										from LvLimits 
										where Id = 
										CASE
											WHEN (rc.CompDeductibleId IS NULL)
											THEN rc.FireDeductibleId
											WHEN rc.FireDeductibleId IS NULL
											THEN rc.CompDeductibleId
										END
										) as 'Comp/FT Deductible',
									(select LimitDisplay from LvLimits where Id = rc.CargoLimitId) as 'Motor Truck Cargo Limit',
									(select LimitDisplay from LvLimits where Id = rc.UMBILimitId) as 'Uninsured Motorists BI Limit/Deductible',
									(select LimitDisplay from LvLimits where Id = rc.UMPDLimitId) as 'Uninsured Motorists PD Limit/Deductible',
									(select LimitDisplay from LvLimits where Id = rc.UIMBILimitId) as 'Underinsured Motorists BI Limit/Deductible',
									(select LimitDisplay from LvLimits where Id = rc.UIMPDLimitId) as 'Underinsured Motorists PD Limit/Deductible',
									bi.EffectiveDate,
									ae.CompanyName as 'Broker Agency',
									CASE WHEN ae.IsIndividual = 1 THEN 'YES' ELSE 'NO' END as 'Individual',
									aa.StreetAddress1 as 'Broker_Billing_Address',
									aa.StreetAddress2 as 'Billing Address',
									aa.City as 'Billing City',
									aa.StateCode as 'Billing StateCode',
									aa.ZipCode as 'Billing Zipcode',
									aa2.StreetAddress1 as 'Broker_Business_Address',
									aa2.StreetAddress2 as 'Business Address',
									aa2.City as 'Business City',
									aa2.StateCode as 'Business StateCode',
									aa2.ZipCode as 'Business Zipcode',
									rc.BrokerCommisionAL as 'Broker Commision AL %',
									rc.BrokerCommisionPD as 'Broker Commision PD %',
									b.CreditedOfficeId,
									(select [Description] from CreditedOffices where Id = b.CreditedOfficeId) as 'Credited Office',
									(select [Description] from PaymentTypes where Id = b.PaymentTypesId) as 'Payment Plan',
									ph.Details,
									(select SUBSTRING(value,
											PATINDEX('%[0-9]%', value),
											(CASE WHEN PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), '')) = 0
											THEN LEN(value) ELSE (PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), ''))) - 1
											END)) as result
										from STRING_SPLIT(rf.Other, ',') where value like '%physdamLimit%') as 'Hired PD Limit',
									(select SUBSTRING(value,
											PATINDEX('%[0-9]%', value),
											(CASE WHEN PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), '')) = 0
											THEN LEN(value) ELSE (PATINDEX('%[^0-9]%', STUFF(value, 1, (PATINDEX('%[0-9]%', value) - 1), ''))) - 1
											END)) as result 
										from STRING_SPLIT(rf.Other, ',') where value like '%deductible%') as 'Hired PD Deductible'
								from PolicyHistory ph
								inner join Risk r on ph.RiskId = r.Id
								inner join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1 --TODO: To be confirmed if IsProcessed is to be removed from query so all policies are captured
								inner join [Binding] b on b.RiskId = r.Id
								inner join BrokerInfo bi on bi.RiskId = r.Id
								inner join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
								inner join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
								inner join RiskForm rf on rf.RiskDetailId = rd.Id and rf.FormId = 'HiredPhysicalDamage'
								inner join Insured i on rd.InsuredId = i.Id
								inner join Entity ie on i.EntityId = ie.Id
								inner join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
								inner join [Address] businessaddr on business.AddressId = businessaddr.Id
								left join EntityAddress garage on garage.EntityId = ie.Id and garage.AddressTypeId = 3 and garage.IsMainGaragingAddress = 1
								left join [Address] garageaddr on garage.AddressId = garageaddr.Id
								left join EntityAddress mailing on mailing.EntityId = ie.Id and mailing.AddressTypeId = 4
								left join [Address] mailingaddr on mailing.AddressId = mailingaddr.Id
								left join Agency_Agency agency on agency.Id = bi.AgencyId
								left join Agency_Entity ae on ae.Id = agency.EntityId
								left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
								left join Agency_Address aa on aea.AddressId = aa.Id
								left join Agency_EntityAddress aea2 on ae.Id = aea2.EntityId and aea2.AddressTypeId = 2
								left join Agency_Address aa2 on aea2.AddressId = aa2.Id
								left join BusinessTypes bt on ie.BusinessTypeID = bt.Id
								left join (
									select rdv.RiskDetailId, COUNT(rdv.RiskDetailId) as vcount from RiskDetailVehicle rdv
									group by rdv.RiskDetailId
								) all_vehicle on all_vehicle.RiskDetailId = rd.Id
								where ph.DateCreated IS NOT NULL
							) rpd
							cross apply
							(
								select COUNT(*) as transactionnumber from PolicyHistory ph
								where ph.RiskId = rpd.RiskId and ph.DateCreated <= rpd.[Transaction Date]
							) totaltransactions
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction bt
								join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
								where btd.AmountTypeId = 'P' and 
									bt.RiskId = rpd.RiskId and
									bt.TransactionTypeId = 
									CASE	
										WHEN rpd.[Transaction Type] = 'New' THEN 'N'
										WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
										WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
										ELSE 'E'
									END
									and
									bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction bt
								join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
								where btd.AmountSubTypeId = 'AIP' and btd.AmountTypeId = 'P' and
									bt.RiskId = rpd.RiskId and
									bt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalaiprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALP' or 
										btd.AmountSubTypeId = 'AIP' or 
										btd.AmountSubTypeId = 'WS' or 
										btd.AmountSubTypeId = 'PNC') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalalpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDP') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalpdpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'TIE') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totaltrailerinterchangepremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'HPDE') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalhiredpdendstpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PNC') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalpncpremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALPSRP' or 
										btd.AmountSubTypeId = 'CPSRP' or 
										btd.AmountSubTypeId = 'ALMPSRP' or 
										btd.AmountSubTypeId = 'GLPSRP' or 
										btd.AmountSubTypeId = 'PDPSRP' or 
										btd.AmountSubTypeId = 'PDMSRP') and btd.AmountTypeId = 'P' and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalshortratepremium
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALSF' or btd.AmountSubTypeId = 'PDSF') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalsf
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'SF') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalstampingfee
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'SLT') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totaltax
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'WS') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalwaiver
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALMP') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalalmprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDM') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalpdmprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'CP') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalcargoprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'GLP') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) 
							) totalglprem
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALPMPA' or 
										btd.AmountSubTypeId = 'CPMPA' or 
										btd.AmountSubTypeId = 'ALMPMPA' or 
										btd.AmountSubTypeId = 'GLPMPA' or
										btd.AmountSubTypeId = 'PDPMPA' or 
										btd.AmountSubTypeId = 'PDMMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalalminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'CPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalcargominpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'ALMPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalalmpminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'GLPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalglminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDPMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalpdminpremiumadjustment
							cross apply
							(
								select SUM(btd.Amount) as totalamount from Billing_Transaction btt
								join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
								where (btd.AmountSubTypeId = 'PDMMPA') and 
									btt.RiskId = rpd.RiskId and
									btt.TransactionTypeId = 
										CASE	
											WHEN rpd.[Transaction Type] = 'New' THEN 'N'
											WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
											WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
											ELSE 'E'
										END
									and
									btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date])
							) totalpdmpminpremiumadjustment
							cross apply
							(
								select SUM(rvpt.CurrentCollPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcol
							cross apply
							(
								select SUM(rvpt.CurrentCollPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								cross apply (
									select top 1 * from RiskDetail rd
									where rd.Id = rvpt.RiskDetailId
									order by rd.CreatedDate desc
								) RiskDetail
								inner join (
									select top 1 * from PolicyHistory ph
									where ph.RiskId = rpd.RiskId and
											ph.DateCreated < rpd.[Transaction Date] and 
											ph.EndorsementNumber = (rpd.EndorsementNumberInDB - (1 - IIF(rpd.[Transaction Type] = 'Reinstatement', 1,0)))
									order by ph.DateCreated desc
								) as PolicyHistory on PolicyHistory.PreviousRiskDetailId = RiskDetail.Id
								where rvpt.FromReinstatement = 
									CASE
										WHEN PolicyHistory.PolicyStatus = 'Reinstated'
										THEN 1
										ELSE 0
									END
							) previoustotalcol
							cross apply
							(
								select SUM(rvpt.CurrentCompPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcompft
							cross apply
							(
								select SUM(rvpt.CurrentCompPremium) as totalamount from RiskVehiclePremiumTransaction rvpt
								cross apply (
									select top 1 * from RiskDetail rd
									where rd.Id = rvpt.RiskDetailId
									order by rd.CreatedDate desc
								) RiskDetail
								inner join (
									select top 1 * from PolicyHistory ph
									where ph.RiskId = rpd.RiskId and
											ph.DateCreated < rpd.[Transaction Date] and 
											ph.EndorsementNumber = (rpd.EndorsementNumberInDB - (1 - IIF(rpd.[Transaction Type] = 'Reinstatement', 1,0)))
									order by ph.DateCreated desc
								) as PolicyHistory on PolicyHistory.PreviousRiskDetailId = RiskDetail.Id
								where rvpt.FromReinstatement = 
									CASE
										WHEN PolicyHistory.PolicyStatus = 'Reinstated'
										THEN 1
										ELSE 0
									END
							) previoustotalcompft
							cross apply
							(
								select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle rdv
								join Vehicle v on rdv.VehicleId = v.Id
								where rpd.RiskDetailId = rdv.RiskDetailId and (rpd.[Endorsement Number] = 0 or rpd.Details LIKE '%Added VIN ''' + v.VIN + '''%')
							) totaltiv
						) rpt
				--where rpt.RiskId = '46498d0f-4102-4d70-9bf8-d6256176cfa0'
				where rpt.[Policy Number] IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				and rpt.[Transaction Date] >= @fromDate
				and rpt.[Transaction Date] <= @toDate
				) brpt
				order by brpt.[Policy Number], brpt.[Transaction Number]

			END";

    }
}
