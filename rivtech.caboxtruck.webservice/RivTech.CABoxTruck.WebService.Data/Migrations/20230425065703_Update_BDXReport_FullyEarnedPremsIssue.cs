using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBDXReportFullyEarnedPremsIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_BorderauxReport);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(REVERT_BorderauxReport);
        }

        #region BDX Report

        const string UPDATE_BorderauxReport = @"
			CREATE OR ALTER   PROCEDURE[dbo].[GenerateBorderauxReport]
				@fromDate datetime = NULL,
				@toDate datetime = NULL
			AS
			BEGIN
				SET NOCOUNT ON;

				select distinct
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
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Earned Premium] 
							END
					END as varchar(50)), '') as 'EarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[AL Earned Premium]
							END
					END as varchar(50)), '') as 'ALEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Collision Earned Premium]
							END
					END as varchar(50)), '') as 'CollisionEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Comp/FT Earned Premium]
							END
					END as varchar(50)), '') as 'CompFTEarnedPremium',
					--ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
					--	THEN NULL
					--	ELSE brpt.[PD Earned Premium]
					--END as varchar(50)), '') as 'PD Earned Premium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Cargo Earned Premium]
							END
					END as varchar(50)), '') as 'CargoEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[GL Earned Premium]
							END
					END as varchar(50)), '') as 'GLEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[AL Manuscript Earned Premium]
							END
					END as varchar(50)), '') as 'ALManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[PD Manuscript Earned Premium]
							END	
					END as varchar(50)), '') as 'PDManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Hired PD Earned Premium]
							END
					END as varchar(50)), '') as 'HiredPDEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Trailer Earned Premium]
							END
					END as varchar(50)), '') as 'TrailerEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Primary and Non-Contributory Earned Premium]
							END
					END as varchar(50)), '') as 'PNCEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Short Rate Earned Premium]
							END
					END as varchar(50)), '') as 'ShortRateEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Waiver of Subrogation Earned Premium]
							END
					END as varchar(50)), '') as 'WaiverEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Additional Insured Earned Premium]
							END
					END as varchar(50)), '') as 'AIEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Minimum Premium Adjustment] 
					END as varchar(50)), '') 'MinimumPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Auto Liability Premium Adjustment] 
					END as varchar(50)), '') 'AutoLiabilityPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Collision Premium Adjustment] 
					END as varchar(50)), '') 'CollisionPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Comp/FT Premium Adjustment] 
					END as varchar(50)), '') 'CompFTPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Cargo Premium Adjustment] 
					END as varchar(50)), '') 'CargoPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[General Liability Premium Adjustment] 
					END as varchar(50)), '') 'GeneralLiabilityPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[AL Manuscript Premium Adjustment] 
					END as varchar(50)), '') 'ALManuscriptPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[PD Manuscript Premium Adjustment] 
					END as varchar(50)), '') 'PDManuscriptPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Hired PD Premium Adjustment] 
					END as varchar(50)), '') 'HiredPDPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Trailer Premium Adjustment] 
					END as varchar(50)), '') 'TrailerPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[PNC Premium Adjustment] 
					END as varchar(50)), '') 'PNCPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Waiver Premium Adjustment] 
					END as varchar(50)), '') 'WaiverPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Additional Insured Premium Adjustment] 
					END as varchar(50)), '') 'AIPremiumAdjustment',
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
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[AL Manuscript Premium], 0) + coalesce(brpt.[Motor Truck Cargo Premium], 0) + coalesce(brpt.[General Liability Premium], 0) + coalesce(brpt.[Auto Liability Premium], 0) +
					 coalesce(brpt.[Additional Insured Amount], 0) + coalesce(brpt.[Primary and Non-Contributory Premium], 0) + coalesce(brpt.[Waiver of Subrogation], 0) +
						coalesce(brpt.[Auto Liability Premium Adjustment], 0) + coalesce(brpt.[Cargo Premium Adjustment], 0) + 
						coalesce(brpt.[General Liability Premium Adjustment], 0) + coalesce(brpt.[AL Manuscript Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision AL %] as decimal(18,2)) / 100.0))) as 'BrokerCommissionAL',
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[Trailer Premium], 0) + coalesce(brpt.[Hired PD Premium], 0) + coalesce(brpt.[PD Manuscript Premium], 0) +
						coalesce(brpt.[Collision Premium], 0) + coalesce(brpt.[Comp/FT Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium Adjustment], 0) + 
						coalesce(brpt.[PD Manuscript Premium Adjustment], 0) +
						coalesce(brpt.[Collision Premium Adjustment], 0) + coalesce(brpt.[Comp/FT Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision PD %] as decimal(18,2)) / 100.0))) as 'BrokerCommissionPD',
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[AL Manuscript Premium], 0) + coalesce(brpt.[Motor Truck Cargo Premium], 0) + coalesce(brpt.[General Liability Premium], 0) + coalesce(brpt.[Auto Liability Premium], 0) +
					 coalesce(brpt.[Additional Insured Amount], 0) + coalesce(brpt.[Primary and Non-Contributory Premium], 0) + coalesce(brpt.[Waiver of Subrogation], 0) +
						coalesce(brpt.[Auto Liability Premium Adjustment], 0) + coalesce(brpt.[Cargo Premium Adjustment], 0) + 
						coalesce(brpt.[General Liability Premium Adjustment], 0) + coalesce(brpt.[AL Manuscript Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision AL %] as decimal(18,2)) / 100.0))) +
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[Trailer Premium], 0) + coalesce(brpt.[Hired PD Premium], 0) + coalesce(brpt.[PD Manuscript Premium], 0) +
						coalesce(brpt.[Collision Premium], 0) + coalesce(brpt.[Comp/FT Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium Adjustment], 0) + 
						coalesce(brpt.[PD Manuscript Premium Adjustment], 0) +
						coalesce(brpt.[Collision Premium Adjustment], 0) + coalesce(brpt.[Comp/FT Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision PD %] as decimal(18,2)) / 100.0))) as 'TotalCommission',
					brpt.[ARS Commission %] 'ARSCommissionPercent',
					brpt.[ARS Commission] 'ARSCommission',
					brpt.[Payment Plan] 'PaymentPlan',
					brpt.[Credited Office] 'CreditedOffice',
					brpt.Status,
					brpt.[Auto Liability Short Rate Premium] 'ALShortRatePremium',
					brpt.[Cargo Short Rate Premium] 'CargoShortRatePremium',
					brpt.[AL Manuscript Short Rate Premium] 'ALManuscriptShortRatePremium',
					brpt.[General Liability Short Rate Premium] 'GLShortRatePremium',
					brpt.[Physical Damage Short Rate Premium] 'PDShortRatePremium',
					brpt.[PD Manuscript Short Rate Premium] 'PDManuscriptShortRatePremium'
				from
				(
					select distinct
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
						ISNULL(rpt.[Garaging Address], '') as 'Garaging Address',
						ISNULL(rpt.[Garaging City], '') as 'Garaging City',
						ISNULL(rpt.[Garaging State], '') as 'Garaging State',
						ISNULL(rpt.[Garaging ZipCode],'') as 'Garaging ZipCode',
						ISNULL(rpt.[Mailing Address], '') as 'Mailing Address',
						ISNULL(rpt.[Mailing City],'') as 'Mailing City',
						ISNULL(rpt.[Mailing State],'') as 'Mailing State',
						ISNULL(rpt.[Mailing ZipCode],'') as 'Mailing ZipCode',
						CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(18,2),rpt.[Vehicle/Unit Count]) END as 'Vehicle/Unit Count',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) as 'Total Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium]), 0) as 'Auto Liability Premium',
						coalesce(CONVERT(DECIMAL(18,0), REPLACE(TRIM('$' from rpt.[Auto Liability Limit]), ',', '')), 0) as 'Auto Liability Limit',
						coalesce(CONVERT(DECIMAL(18,0), REPLACE(TRIM('$' from rpt.[Auto Liability Deductible]), ',', '')), 0) as 'Auto Liability Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium]), 0) as 'AL Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium]), 0) as 'General Liability Premium',
						iSNULL(rpt.[PIP Limit],'') as 'PIP Limit',
						ISNULL(rpt.[Auto Medical Payment Limit], '') as 'Auto Medical Payment Limit',
						rpt.Symbol,
						--CASE WHEN rpt.[Physical Damage Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Physical Damage Premium] ) END as 'Physical Damage Premium', --TODO: Removed since PD Premium is not required in the report
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Premium]), 0) as 'Collision Premium',
						ISNULL(rpt.[Collision Deductible],'') as 'Collision Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium]), 0) as 'Comp/FT Premium',
						ISNULL(rpt.[Comp/FT Deductible],'') as 'Comp/FT Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium]), 0) as 'PD Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Motor Truck Cargo Premium]), 0) as 'Motor Truck Cargo Premium',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.Details) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2), rpt.[Hired PD Premium]), 0)
									ELSE 0
								END
							 WHEN rpt.[Transaction Type] = 'Reinstatement'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.previousDetails) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2), rpt.[Hired PD Premium]), 0)
									ELSE 0
								END
							ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium]), 0)
						END as 'Hired PD Premium',
						CASE WHEN rpt.[Hired PD Limit] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Limit]) END as 'Hired PD Limit',
						CASE WHEN rpt.[Hired PD Deductible] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Deductible]) END as 'Hired PD Deductible',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.Details) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0)
									ELSE 0
								END
							 WHEN rpt.[Transaction Type] = 'Reinstatement'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.previousDetails) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0)
									ELSE 0
								END
							ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0)
						END as 'Trailer Premium',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.Details) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0)
									ELSE 0
								END
							 WHEN rpt.[Transaction Type] = 'Reinstatement'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.previousDetails) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0)
									ELSE 0
								END
							ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0)
						END as 'Primary and Non-Contributory Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Premium]), 0) as 'Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Short Rate Premium]), 0) as 'Auto Liability Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Short Rate Premium]), 0) as 'Cargo Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Short Rate Premium]), 0) as 'AL Manuscript Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Short Rate Premium]), 0) as 'General Liability Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Short Rate Premium]), 0) as 'Physical Damage Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Short Rate Premium]), 0) as 'PD Manuscript Short Rate Premium',
						ISNULL(rpt.[Motor Truck Cargo Limit], '') as 'Motor Truck Cargo Limit',
						ISNULL(rpt.[Uninsured Motorists BI Limit/Deductible], '') as 'Uninsured Motorists BI Limit/Deductible',
						ISNULL(rpt.[Uninsured Motorists PD Limit/Deductible], '') as 'Uninsured Motorists PD Limit/Deductible',
						ISNULL(rpt.[Underinsured Motorists BI Limit/Deductible], '') as 'Underinsured Motorists BI Limit/Deductible',
						ISNULL(rpt.[Underinsured Motorists PD Limit/Deductible], '') as 'Underinsured Motorists PD Limit/Deductible',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.Details) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0)
									ELSE 0
								END
							 WHEN rpt.[Transaction Type] = 'Reinstatement'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.previousDetails) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0)
									ELSE 0
								END
							ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0)
						END as 'Additional Insured Amount',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Insured Value]), 0) as 'Total Insured Value',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Service Fee]), 0) as 'Service Fee',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Stamping Fees]), 0) as 'Stamping Fees',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[SL Tax]), 0) as 'SL Tax',
						CASE WHEN rpt.[Transaction Type] = 'Cancellation'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.Details) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0)
									ELSE 0
								END
							 WHEN rpt.[Transaction Type] = 'Reinstatement'
							 THEN
								CASE WHEN CHARINDEX('Cancel Flat', rpt.previousDetails) > 0
									THEN coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0)
									ELSE 0
								END
							ELSE coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0)
						END as 'Waiver of Subrogation',
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
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Earned Premium]), 0)
						as 'Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Earned Premium]), 0) as 'AL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Collision Earned Premium]), 0) as 'Collision Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Comp/FT Earned Premium]), 0) as 'Comp/FT Earned Premium',
						--coalesce(CONVERT(DECIMAL(18,2), rpt.[PD Earned Premium]), 0) as 'PD Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Cargo Earned Premium]), 0) as 'Cargo Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[GL Earned Premium]), 0) as'GL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Manuscript Earned Premium]), 0) as'AL Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[PD Manuscript Earned Premium]), 0) as 'PD Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Hired PD Earned Premium]), 0) as 'Hired PD Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Trailer Earned Premium]), 0) as 'Trailer Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Primary and Non-Contributory Earned Premium]), 0) as 'Primary and Non-Contributory Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Short Rate Earned Premium]), 0) as 'Short Rate Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Waiver of Subrogation Earned Premium]), 0) as 'Waiver of Subrogation Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Additional Insured Earned Premium]), 0) as 'Additional Insured Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Minimum Premium Adjustment]), 0) as 'Minimum Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0) as 'Auto Liability Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Premium Adjustment]), 0) as 'Collision Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium Adjustment]), 0) as 'Comp/FT Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0) as 'Cargo Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0) as 'AL Manuscript Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0) as 'General Liability Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium Adjustment]), 0) as 'PD Manuscript Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Premium Adjustment]), 0) as 'Physical Damage Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium Adjustment]), 0) as 'Hired PD Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium Adjustment]), 0) as 'Trailer Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PNC Premium Adjustment]), 0) as 'PNC Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver Premium Adjustment]), 0) as 'Waiver Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Premium Adjustment]), 0) as 'Additional Insured Premium Adjustment',
						ISNULL(rpt.[Broker Agency], '') as 'Broker Agency',
						ISNULL(rpt.Broker_Billing_Address, '') as 'Broker_Billing_Address',
						ISNULL(rpt.[Billing Address], '') as 'Billing Address',
						ISNULL(rpt.[Billing City], '') as 'Billing City',
						ISNULL(rpt.[Billing StateCode], '') as 'Billing StateCode',
						ISNULL(rpt.[Billing Zipcode], '') as 'Billing Zipcode',
						ISNULL(rpt.Broker_Business_Address, '') as 'Broker_Business_Address',
						ISNULL(rpt.[Business Address], '') as 'Business Address',
						ISNULL(rpt.[Business City], '') as 'Business City',
						ISNULL(rpt.[Business StateCode], '') as 'Business StateCode',
						ISNULL(rpt.[Business Zipcode], '') as 'Business Zipcode',
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
								CONVERT(decimal(18,2), (coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) * (2.5/100)))
							ELSE 0
						END AS 'ARS Commission',
						rpt.[Payment Plan],
						rpt.[Credited Office],
						rpt.[Risk Status] as Status,
						rpt.[Latest Transaction Details]
					from
						(
							select 
							--rpd.*,
							rpd.PolicyNumber as 'Policy Number',
							rpd.Status as 'Risk Status',
							rpd.[Endorsement Number],
							rpd.[Transaction Number],
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
							totalpremium as 'Total Premium',
							totalalpremium as 'Auto Liability Premium',
							--totalpdpremium as 'Physical Damage Premium', --TODO: Remove if needed since PD Premium is currently not part of the report
							totalglprem as 'General Liability Premium',
							(coalesce(totalcollpremiumchange.totalamount, 0)) as 'Collision Premium',
							(coalesce(totalcompftpremiumchange.totalamount, 0)) as 'Comp/FT Premium',
							totalaiprem as 'Additional Insured Amount',
							CASE WHEN (rpd.[Transaction Type] = 'Cancellation' or rpd.[Transaction Type] = 'Reinstatement')
								THEN 0
								ELSE (coalesce(totaladdedtiv.totalamount, 0) - coalesce(totalremovedtiv.totalamount, 0))
							END as 'Total Insured Value',
							totalsf as 'Service Fee',
							totalstampingfee as 'Stamping Fees',
							totaltax as 'SL Tax',
							totalwaiver 'Waiver of Subrogation',
							totalcargoprem as 'Motor Truck Cargo Premium',
							totalhiredpdendstpremium as 'Hired PD Premium',
							totaltrailerinterchangepremium as 'Trailer Premium',
							totalpncpremium as 'Primary and Non-Contributory Premium',
							totalshortratepremium as 'Short Rate Premium',
							totalalshortratepremium as 'Auto Liability Short Rate Premium',
							totalcargoshortratepremium as 'Cargo Short Rate Premium',
							totalalmshortratepremium as 'AL Manuscript Short Rate Premium',
							totalglshortratepremium as 'General Liability Short Rate Premium',
							totalpdshortratepremium as 'Physical Damage Short Rate Premium',
							totalpdmshortratepremium as 'PD Manuscript Short Rate Premium',
							totalalmprem as 'AL Manuscript Premium',
							totalpdmprem as 'PD Manuscript Premium',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalmincancelpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalmincancelpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalminreinstatepremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalminreinstatepremiumadjustment.totalamount, 0))
							END as 'Minimum Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalalmincancelpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalalmincancelpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalalminreinstatepremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalalminreinstatepremiumadjustment.totalamount, 0))
							END as 'Auto Liability Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalcollcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalcollcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalcollreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalcollreinstateminpremiumadjustment.totalamount, 0))
							END as 'Collision Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalcompftcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalcompftcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalcompftreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalcompftreinstateminpremiumadjustment.totalamount, 0))
							END as 'Comp/FT Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalcargocancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalcargocancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalcargoreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalcargoreinstateminpremiumadjustment.totalamount, 0))
							END as 'Cargo Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalalmpcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalalmpcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalalmpreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalalmpreinstateminpremiumadjustment.totalamount, 0))
							END as 'AL Manuscript Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalglcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalglcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalglreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalglreinstateminpremiumadjustment.totalamount, 0))
							END as 'General Liability Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalpdmpcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalpdmpcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalpdmpreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalpdmpreinstateminpremiumadjustment.totalamount, 0))
							END as 'PD Manuscript Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalpdcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalpdcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalpdreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalpdreinstateminpremiumadjustment.totalamount, 0))
							END as 'Physical Damage Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalhpdecancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalhpdecancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalhpdereinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalhpdereinstateminpremiumadjustment.totalamount, 0))
							END as 'Hired PD Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totaltiecancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totaltiecancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totaltiereinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totaltiereinstateminpremiumadjustment.totalamount, 0))
							END as 'Trailer Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalpnccancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalpnccancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalpncreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalpncreinstateminpremiumadjustment.totalamount, 0))
							END as 'PNC Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalwaivercancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalwaivercancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalwaiverreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalwaiverreinstateminpremiumadjustment.totalamount, 0))
							END as 'Waiver Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalaicancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalaicancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalaireinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalaireinstateminpremiumadjustment.totalamount, 0))
							END as 'Additional Insured Premium Adjustment',
							(coalesce(totalsf, 0) + coalesce(totaltax, 0) + coalesce(totalstampingfee, 0)) as 'Total Tax/Fees',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalpremium, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcompftpremiumchange.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcollpremiumchange.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalcargoprem, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalglprem, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							--CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							--THEN 0
							--ELSE coalesce(totalpdpremium, 0) * (
							--		DATEDIFF
							--		(day, rpd.[Transaction Effective Date],
							--			(
							--				CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
							--				THEN
							--					CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
							--						THEN latestTransactionDetails.EffectiveDate
							--						ELSE DATEADD(day, 1, @toDate)
							--					END
							--				ELSE
							--					CASE WHEN rpd.ExpirationDate <= @toDate
							--						THEN rpd.ExpirationDate
							--						ELSE DATEADD(day, 1, @toDate)
							--					END
							--				END
							--			)
							--		)
							--	) / DATEDIFF(day, rpd.[Transaction Effective Date], rpd.ExpirationDate)
							--END as 'PD Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalmprem, 0)
							END as 'AL Manuscript Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpdmprem, 0)
							END as 'PD Manuscript Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalhiredpdendstpremium, 0)
							END as 'Hired PD Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totaltrailerinterchangepremium, 0)
							END as 'Trailer Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpncpremium, 0)
							END as 'Primary and Non-Contributory Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalshortratepremium, 0)
							END as 'Short Rate Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalwaiver, 0)
							END as 'Waiver of Subrogation Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalaiprem, 0)
							END as 'Additional Insured Earned Premium',
							((coalesce(totalalpremium, 0) + 
								coalesce(totalaiprem, 0) +
								coalesce(totalwaiver, 0) +
								coalesce(totalpncpremium, 0) +
								coalesce(totalcargoprem, 0) +
								coalesce(totalglprem, 0) +
								coalesce(totalalmprem, 0))
							* (cast(rpd.[Broker Commision AL %] as decimal(18,2)) / 100.0)) as CommisionALAmount,
							((coalesce(totalpdpremium, 0) + 
								coalesce(totaltrailerinterchangepremium, 0) + 
								coalesce(totalhiredpdendstpremium, 0) +
								coalesce(totalpdmprem, 0))
							* (cast(rpd.[Broker Commision PD %] as decimal(18,2)) / 100.0)) as CommisionPDAmount,
							DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate) as 'Total days',
							DATEDIFF(DAY, rpd.[Effective Date], GETDATE()) as 'Total days passed',
							totalpremium / (DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate)) as 'Prem per day',
							rpd.MinimumEarned as 'Minimum Earned',
							latestTransactionDetails.Details as 'Latest Transaction Details',
							rpd.Details,
							previousPolicyHistory.Details as previousDetails
							from
							(
								select 
									r.Id as 'RiskId',
									ph.Id as 'PolicyHistoryId',
									ph.PreviousRiskDetailId as 'RiskDetailId',
									r.PolicyNumber,
									r.Status,
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
									ph.TransactionNumber as 'Transaction Number',
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
								cross apply 
								(
									select TOP 1 * from EntityAddress 
									where EntityAddress.EntityId = ie.Id
									and EntityAddress.AddressTypeId = 2
									order by CreatedDate desc
								) business
								--inner join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
								inner join [Address] businessaddr on business.AddressId = businessaddr.Id
								left join EntityAddress garage on garage.EntityId = ie.Id and garage.AddressTypeId = 3 and garage.IsMainGaragingAddress = 1
								left join [Address] garageaddr on garage.AddressId = garageaddr.Id
								outer apply 
								(
									select TOP 1 * from EntityAddress 
									where EntityAddress.EntityId = ie.Id
									and EntityAddress.AddressTypeId = 4
									order by CreatedDate desc
								) mailing
								--left join EntityAddress mailing on mailing.EntityId = ie.Id and mailing.AddressTypeId = 4
								left join [Address] mailingaddr on mailing.AddressId = mailingaddr.Id
								left join Agency_Agency agency on agency.Id = bi.AgencyId
								left join Agency_Entity ae on ae.Id = agency.EntityId
								outer apply 
								(
									select TOP 1 * from Agency_EntityAddress 
									where Agency_EntityAddress.EntityId = ae.Id
									and Agency_EntityAddress.AddressTypeId = 1 
									order by CreatedDate desc
								) aea
								--left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
								left join Agency_Address aa on aea.AddressId = aa.Id
								outer apply 
								(
									select TOP 1 * from Agency_EntityAddress 
									where Agency_EntityAddress.EntityId = ae.Id
									and Agency_EntityAddress.AddressTypeId = 2 
									order by CreatedDate desc
								) aea2
								--left join Agency_EntityAddress aea2 on ae.Id = aea2.EntityId and aea2.AddressTypeId = 2 and aea2.RemovedProcessDate is null
								left join Agency_Address aa2 on aea2.AddressId = aa2.Id
								left join BusinessTypes bt on ie.BusinessTypeID = bt.Id
								left join (
									select rdv.RiskDetailId, COUNT(rdv.RiskDetailId) as vcount from RiskDetailVehicle rdv
									group by rdv.RiskDetailId
								) all_vehicle on all_vehicle.RiskDetailId = rd.Id
								where ph.DateCreated IS NOT NULL
									and ph.DateCreated >= @fromDate
									and ph.DateCreated <= @toDate
							) rpd
							left join
							(
								SELECT RiskId, PolicyHistoryId,  
									totalpremium, 
									totalaiprem, 
									totalalpremium, 
									totalpdpremium, 
									totaltrailerinterchangepremium, 
									totalhiredpdendstpremium, 
									totalpncpremium,
									totalalshortratepremium,
									totalcargoshortratepremium,
									totalalmshortratepremium,
									totalglshortratepremium,
									totalpdshortratepremium,
									totalpdmshortratepremium,
									totalsf,
									totalstampingfee,
									totaltax,
									totalwaiver,
									totalalmprem,
									totalpdmprem,
									totalcargoprem,
									totalglprem,
									totalshortratepremium,
									TransactionTypeId FROM   
								(
									SELECT RiskId, PolicyHistoryId, totalamount, TransType, TransactionTypeId
										FROM     dbo.wv_BDXUnionFees
								) t 
							PIVOT(
								SUM(totalamount) 
								FOR TransType IN (
									totalpremium, 
									totalaiprem, 
									totalalpremium, 
									totalpdpremium, 
									totaltrailerinterchangepremium, 
									totalhiredpdendstpremium, 
									totalpncpremium,
									totalalshortratepremium,
									totalcargoshortratepremium,
									totalalmshortratepremium,
									totalglshortratepremium,
									totalpdshortratepremium,
									totalpdmshortratepremium,
									totalsf,
									totalstampingfee,
									totaltax,
									totalwaiver,
									totalalmprem,
									totalpdmprem,
									totalcargoprem,
									totalglprem,
									totalshortratepremium)
							) AS pvtCharge
							) bt  ON bt.RiskId = rpd.RiskId and bt.PolicyHistoryId = rpd.PolicyHistoryId AND bt.TransactionTypeId = rpd.[Transaction Type] 
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId <> 'PDP'	
							) totalmincancelpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId <> 'PDP'
							) totalminreinstatepremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'ALP'			
							) totalalmincancelpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'ALP'			
							) totalalminreinstatepremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'CP'
							) totalcargocancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'CP'
							) totalcargoreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'ALMP'
							) totalalmpcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'ALMP'
							) totalalmpreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'GLP'
							) totalglcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'GLP'
							) totalglreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDP'
							) totalpdcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDP'
							) totalpdreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDM'
							) totalpdmpcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDM'
							) totalpdmpreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDPColl'
							) totalcollcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDPColl'
							) totalcollreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDPCompFt'
							) totalcompftcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDPCompFt'
							) totalcompftreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'HPDE'
							) totalhpdecancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'HPDE'
							) totalhpdereinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'TIE'
							) totaltiecancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'TIE'
							) totaltiereinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PNC'
							) totalpnccancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PNC'
							) totalpncreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'WS'
							) totalwaivercancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'WS'
							) totalwaiverreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'AIP'
							) totalaicancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'AIP'
							) totalaireinstateminpremiumadjustment
							cross apply
							(
								select SUM(rvpt.PremiumChangeColl) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcollpremiumchange
							cross apply
							(
								select SUM(rvpt.PremiumChangeCompFt) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcompftpremiumchange
							cross apply
							(
								select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle rdv
								join Vehicle v on rdv.VehicleId = v.Id
								where rpd.RiskDetailId = rdv.RiskDetailId and (rpd.[Endorsement Number] = 0 or rpd.Details LIKE '%Added VIN ''' + v.VIN + '''%')
							) totaladdedtiv
							cross apply
							(
								select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle rdv
								join Vehicle v on rdv.VehicleId = v.Id
								where rpd.RiskDetailId = rdv.RiskDetailId and rpd.Details LIKE '%Removed VIN ''' + v.VIN + '''%'
							) totalremovedtiv
							cross apply
							(
								select top 1 ph.Details, ph.EffectiveDate, ph.DateCreated,
									CASE
										WHEN ph.EndorsementNumber = 0
										THEN 'New'
										WHEN ph.PolicyStatus = 'Reinstated'
										THEN 'Reinstatement'
										WHEN ph.PolicyStatus = 'Canceled'
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and (ph.PolicyStatus = 'Rewrite' and rd_temp.Status = 'Canceled') --Canceled/Rewrite
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and rd_temp.Status = 'Rewrite'
										THEN 'New'
										ELSE 'Endorsement'
									END as PolicyStatus
									from PolicyHistory ph
								inner join Risk r_temp on r_temp.PolicyNumber = rpd.PolicyNumber and ph.RiskId = r_temp.Id
								inner join RiskDetail rd_temp on ph.PreviousRiskDetailId = rd_temp.Id
								where ph.DateCreated IS NOT NULL
									and ph.DateCreated >= @fromDate
									and ph.DateCreated <= @toDate
								order by ph.DateCreated desc
							) latestTransactionDetails
							outer apply
							(
								select top 1 ph.Details from PolicyHistory ph
								where ph.RiskId = rpd.RiskId and
								ph.TransactionNumber = rpd.[Transaction Number] - 1
							) previousPolicyHistory
						) rpt
					--where rpt.RiskId = '46498d0f-4102-4d70-9bf8-d6256176cfa0'
					where rpt.[Policy Number] IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				) brpt
				order by brpt.[Policy Number], brpt.[Transaction Date], brpt.[Transaction Number]			

			END
        ";
        const string REVERT_BorderauxReport = @"
			CREATE OR ALTER   PROCEDURE[dbo].[GenerateBorderauxReport]
				@fromDate datetime = NULL,
				@toDate datetime = NULL
			AS
			BEGIN
				SET NOCOUNT ON;

				select distinct
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
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Earned Premium] 
							END
					END as varchar(50)), '') as 'EarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[AL Earned Premium]
							END
					END as varchar(50)), '') as 'ALEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Collision Earned Premium]
							END
					END as varchar(50)), '') as 'CollisionEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Comp/FT Earned Premium]
							END
					END as varchar(50)), '') as 'CompFTEarnedPremium',
					--ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
					--	THEN NULL
					--	ELSE brpt.[PD Earned Premium]
					--END as varchar(50)), '') as 'PD Earned Premium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Cargo Earned Premium]
							END
					END as varchar(50)), '') as 'CargoEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[GL Earned Premium]
							END
					END as varchar(50)), '') as 'GLEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[AL Manuscript Earned Premium]
							END
					END as varchar(50)), '') as 'ALManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[PD Manuscript Earned Premium]
							END	
					END as varchar(50)), '') as 'PDManuscriptEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Hired PD Earned Premium]
							END
					END as varchar(50)), '') as 'HiredPDEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Trailer Earned Premium]
							END
					END as varchar(50)), '') as 'TrailerEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Primary and Non-Contributory Earned Premium]
							END
					END as varchar(50)), '') as 'PNCEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Short Rate Earned Premium]
							END
					END as varchar(50)), '') as 'ShortRateEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Waiver of Subrogation Earned Premium]
							END
					END as varchar(50)), '') as 'WaiverEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'Cancellation' OR brpt.[Transaction Type] = 'Reinstatement'
						THEN NULL
						ELSE 
							CASE WHEN CHARINDEX('Cancel Flat', brpt.[Latest Transaction Details]) > 0
								THEN 0
								ELSE brpt.[Additional Insured Earned Premium]
							END
					END as varchar(50)), '') as 'AIEarnedPremium',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Minimum Premium Adjustment] 
					END as varchar(50)), '') 'MinimumPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Auto Liability Premium Adjustment] 
					END as varchar(50)), '') 'AutoLiabilityPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Collision Premium Adjustment] 
					END as varchar(50)), '') 'CollisionPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Comp/FT Premium Adjustment] 
					END as varchar(50)), '') 'CompFTPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Cargo Premium Adjustment] 
					END as varchar(50)), '') 'CargoPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[General Liability Premium Adjustment] 
					END as varchar(50)), '') 'GeneralLiabilityPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[AL Manuscript Premium Adjustment] 
					END as varchar(50)), '') 'ALManuscriptPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[PD Manuscript Premium Adjustment] 
					END as varchar(50)), '') 'PDManuscriptPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Hired PD Premium Adjustment] 
					END as varchar(50)), '') 'HiredPDPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Trailer Premium Adjustment] 
					END as varchar(50)), '') 'TrailerPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[PNC Premium Adjustment] 
					END as varchar(50)), '') 'PNCPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Waiver Premium Adjustment] 
					END as varchar(50)), '') 'WaiverPremiumAdjustment',
					ISNULL(CAST(CASE WHEN brpt.[Transaction Type] = 'New' OR brpt.[Transaction Type] = 'Endorsement'
						THEN NULL
						ELSE brpt.[Additional Insured Premium Adjustment] 
					END as varchar(50)), '') 'AIPremiumAdjustment',
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
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[AL Manuscript Premium], 0) + coalesce(brpt.[Motor Truck Cargo Premium], 0) + coalesce(brpt.[General Liability Premium], 0) + coalesce(brpt.[Auto Liability Premium], 0) +
					 coalesce(brpt.[Additional Insured Amount], 0) + coalesce(brpt.[Primary and Non-Contributory Premium], 0) + coalesce(brpt.[Waiver of Subrogation], 0) +
						coalesce(brpt.[Auto Liability Premium Adjustment], 0) + coalesce(brpt.[Cargo Premium Adjustment], 0) + 
						coalesce(brpt.[General Liability Premium Adjustment], 0) + coalesce(brpt.[AL Manuscript Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision AL %] as decimal(18,2)) / 100.0))) as 'BrokerCommissionAL',
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[Trailer Premium], 0) + coalesce(brpt.[Hired PD Premium], 0) + coalesce(brpt.[PD Manuscript Premium], 0) +
						coalesce(brpt.[Collision Premium], 0) + coalesce(brpt.[Comp/FT Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium Adjustment], 0) + 
						coalesce(brpt.[PD Manuscript Premium Adjustment], 0) +
						coalesce(brpt.[Collision Premium Adjustment], 0) + coalesce(brpt.[Comp/FT Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision PD %] as decimal(18,2)) / 100.0))) as 'BrokerCommissionPD',
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[AL Manuscript Premium], 0) + coalesce(brpt.[Motor Truck Cargo Premium], 0) + coalesce(brpt.[General Liability Premium], 0) + coalesce(brpt.[Auto Liability Premium], 0) +
					 coalesce(brpt.[Additional Insured Amount], 0) + coalesce(brpt.[Primary and Non-Contributory Premium], 0) + coalesce(brpt.[Waiver of Subrogation], 0) +
						coalesce(brpt.[Auto Liability Premium Adjustment], 0) + coalesce(brpt.[Cargo Premium Adjustment], 0) + 
						coalesce(brpt.[General Liability Premium Adjustment], 0) + coalesce(brpt.[AL Manuscript Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision AL %] as decimal(18,2)) / 100.0))) +
					CONVERT(DECIMAL(18,2),(
					(coalesce(brpt.[Trailer Premium], 0) + coalesce(brpt.[Hired PD Premium], 0) + coalesce(brpt.[PD Manuscript Premium], 0) +
						coalesce(brpt.[Collision Premium], 0) + coalesce(brpt.[Comp/FT Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium], 0) +
						--coalesce(brpt.[Physical Damage Premium Adjustment], 0) + 
						coalesce(brpt.[PD Manuscript Premium Adjustment], 0) +
						coalesce(brpt.[Collision Premium Adjustment], 0) + coalesce(brpt.[Comp/FT Premium Adjustment], 0)) *
					(cast(brpt.[Broker Commision PD %] as decimal(18,2)) / 100.0))) as 'TotalCommission',
					brpt.[ARS Commission %] 'ARSCommissionPercent',
					brpt.[ARS Commission] 'ARSCommission',
					brpt.[Payment Plan] 'PaymentPlan',
					brpt.[Credited Office] 'CreditedOffice',
					brpt.Status,
					brpt.[Auto Liability Short Rate Premium] 'ALShortRatePremium',
					brpt.[Cargo Short Rate Premium] 'CargoShortRatePremium',
					brpt.[AL Manuscript Short Rate Premium] 'ALManuscriptShortRatePremium',
					brpt.[General Liability Short Rate Premium] 'GLShortRatePremium',
					brpt.[Physical Damage Short Rate Premium] 'PDShortRatePremium',
					brpt.[PD Manuscript Short Rate Premium] 'PDManuscriptShortRatePremium'
				from
				(
					select distinct
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
						ISNULL(rpt.[Garaging Address], '') as 'Garaging Address',
						ISNULL(rpt.[Garaging City], '') as 'Garaging City',
						ISNULL(rpt.[Garaging State], '') as 'Garaging State',
						ISNULL(rpt.[Garaging ZipCode],'') as 'Garaging ZipCode',
						ISNULL(rpt.[Mailing Address], '') as 'Mailing Address',
						ISNULL(rpt.[Mailing City],'') as 'Mailing City',
						ISNULL(rpt.[Mailing State],'') as 'Mailing State',
						ISNULL(rpt.[Mailing ZipCode],'') as 'Mailing ZipCode',
						CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(18,2),rpt.[Vehicle/Unit Count]) END as 'Vehicle/Unit Count',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) as 'Total Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium]), 0) as 'Auto Liability Premium',
						coalesce(CONVERT(DECIMAL(18,0), REPLACE(TRIM('$' from rpt.[Auto Liability Limit]), ',', '')), 0) as 'Auto Liability Limit',
						coalesce(CONVERT(DECIMAL(18,0), REPLACE(TRIM('$' from rpt.[Auto Liability Deductible]), ',', '')), 0) as 'Auto Liability Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium]), 0) as 'AL Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium]), 0) as 'General Liability Premium',
						iSNULL(rpt.[PIP Limit],'') as 'PIP Limit',
						ISNULL(rpt.[Auto Medical Payment Limit], '') as 'Auto Medical Payment Limit',
						rpt.Symbol,
						--CASE WHEN rpt.[Physical Damage Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Physical Damage Premium] ) END as 'Physical Damage Premium', --TODO: Removed since PD Premium is not required in the report
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Premium]), 0) as 'Collision Premium',
						ISNULL(rpt.[Collision Deductible],'') as 'Collision Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium]), 0) as 'Comp/FT Premium',
						ISNULL(rpt.[Comp/FT Deductible],'') as 'Comp/FT Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium]), 0) as 'PD Manuscript Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Motor Truck Cargo Premium]), 0) as 'Motor Truck Cargo Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium]), 0) as 'Hired PD Premium',
						CASE WHEN rpt.[Hired PD Limit] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Limit]) END as 'Hired PD Limit',
						CASE WHEN rpt.[Hired PD Deductible] IS NULL THEN '' ELSE CONCAT('$',rpt.[Hired PD Deductible]) END as 'Hired PD Deductible',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0) as 'Trailer Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0) as 'Primary and Non-Contributory Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Premium]), 0) as 'Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Short Rate Premium]), 0) as 'Auto Liability Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Short Rate Premium]), 0) as 'Cargo Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Short Rate Premium]), 0) as 'AL Manuscript Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Short Rate Premium]), 0) as 'General Liability Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Short Rate Premium]), 0) as 'Physical Damage Short Rate Premium',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Short Rate Premium]), 0) as 'PD Manuscript Short Rate Premium',
						ISNULL(rpt.[Motor Truck Cargo Limit], '') as 'Motor Truck Cargo Limit',
						ISNULL(rpt.[Uninsured Motorists BI Limit/Deductible], '') as 'Uninsured Motorists BI Limit/Deductible',
						ISNULL(rpt.[Uninsured Motorists PD Limit/Deductible], '') as 'Uninsured Motorists PD Limit/Deductible',
						ISNULL(rpt.[Underinsured Motorists BI Limit/Deductible], '') as 'Underinsured Motorists BI Limit/Deductible',
						ISNULL(rpt.[Underinsured Motorists PD Limit/Deductible], '') as 'Underinsured Motorists PD Limit/Deductible',
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
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation Earned Premium]), 0) +
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Earned Premium]), 0)
						as 'Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Earned Premium]), 0) as 'AL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Collision Earned Premium]), 0) as 'Collision Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Comp/FT Earned Premium]), 0) as 'Comp/FT Earned Premium',
						--coalesce(CONVERT(DECIMAL(18,2), rpt.[PD Earned Premium]), 0) as 'PD Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Cargo Earned Premium]), 0) as 'Cargo Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[GL Earned Premium]), 0) as'GL Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[AL Manuscript Earned Premium]), 0) as'AL Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[PD Manuscript Earned Premium]), 0) as 'PD Manuscript Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Hired PD Earned Premium]), 0) as 'Hired PD Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Trailer Earned Premium]), 0) as 'Trailer Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Primary and Non-Contributory Earned Premium]), 0) as 'Primary and Non-Contributory Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Short Rate Earned Premium]), 0) as 'Short Rate Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Waiver of Subrogation Earned Premium]), 0) as 'Waiver of Subrogation Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Additional Insured Earned Premium]), 0) as 'Additional Insured Earned Premium',
						coalesce(CONVERT(DECIMAL(18,2), rpt.[Minimum Premium Adjustment]), 0) as 'Minimum Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0) as 'Auto Liability Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Collision Premium Adjustment]), 0) as 'Collision Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium Adjustment]), 0) as 'Comp/FT Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0) as 'Cargo Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0) as 'AL Manuscript Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0) as 'General Liability Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium Adjustment]), 0) as 'PD Manuscript Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Premium Adjustment]), 0) as 'Physical Damage Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium Adjustment]), 0) as 'Hired PD Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium Adjustment]), 0) as 'Trailer Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[PNC Premium Adjustment]), 0) as 'PNC Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver Premium Adjustment]), 0) as 'Waiver Premium Adjustment',
						coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Premium Adjustment]), 0) as 'Additional Insured Premium Adjustment',
						ISNULL(rpt.[Broker Agency], '') as 'Broker Agency',
						ISNULL(rpt.Broker_Billing_Address, '') as 'Broker_Billing_Address',
						ISNULL(rpt.[Billing Address], '') as 'Billing Address',
						ISNULL(rpt.[Billing City], '') as 'Billing City',
						ISNULL(rpt.[Billing StateCode], '') as 'Billing StateCode',
						ISNULL(rpt.[Billing Zipcode], '') as 'Billing Zipcode',
						ISNULL(rpt.Broker_Business_Address, '') as 'Broker_Business_Address',
						ISNULL(rpt.[Business Address], '') as 'Business Address',
						ISNULL(rpt.[Business City], '') as 'Business City',
						ISNULL(rpt.[Business StateCode], '') as 'Business StateCode',
						ISNULL(rpt.[Business Zipcode], '') as 'Business Zipcode',
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
								CONVERT(decimal(18,2), (coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) * (2.5/100)))
							ELSE 0
						END AS 'ARS Commission',
						rpt.[Payment Plan],
						rpt.[Credited Office],
						rpt.[Risk Status] as Status,
						rpt.[Latest Transaction Details]
					from
						(
							select 
							--rpd.*,
							rpd.PolicyNumber as 'Policy Number',
							rpd.Status as 'Risk Status',
							rpd.[Endorsement Number],
							rpd.[Transaction Number],
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
							totalpremium as 'Total Premium',
							totalalpremium as 'Auto Liability Premium',
							--totalpdpremium as 'Physical Damage Premium', --TODO: Remove if needed since PD Premium is currently not part of the report
							totalglprem as 'General Liability Premium',
							(coalesce(totalcollpremiumchange.totalamount, 0)) as 'Collision Premium',
							(coalesce(totalcompftpremiumchange.totalamount, 0)) as 'Comp/FT Premium',
							totalaiprem as 'Additional Insured Amount',
							CASE WHEN (rpd.[Transaction Type] = 'Cancellation' or rpd.[Transaction Type] = 'Reinstatement')
								THEN 0
								ELSE (coalesce(totaladdedtiv.totalamount, 0) - coalesce(totalremovedtiv.totalamount, 0))
							END as 'Total Insured Value',
							totalsf as 'Service Fee',
							totalstampingfee as 'Stamping Fees',
							totaltax as 'SL Tax',
							totalwaiver as 'Waiver of Subrogation',
							totalcargoprem as 'Motor Truck Cargo Premium',
							totalhiredpdendstpremium as 'Hired PD Premium',
							totaltrailerinterchangepremium as 'Trailer Premium',
							totalpncpremium as 'Primary and Non-Contributory Premium',
							totalshortratepremium as 'Short Rate Premium',
							totalalshortratepremium as 'Auto Liability Short Rate Premium',
							totalcargoshortratepremium as 'Cargo Short Rate Premium',
							totalalmshortratepremium as 'AL Manuscript Short Rate Premium',
							totalglshortratepremium as 'General Liability Short Rate Premium',
							totalpdshortratepremium as 'Physical Damage Short Rate Premium',
							totalpdmshortratepremium as 'PD Manuscript Short Rate Premium',
							totalalmprem as 'AL Manuscript Premium',
							totalpdmprem as 'PD Manuscript Premium',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalmincancelpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalmincancelpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalminreinstatepremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalminreinstatepremiumadjustment.totalamount, 0))
							END as 'Minimum Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalalmincancelpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalalmincancelpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalalminreinstatepremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalalminreinstatepremiumadjustment.totalamount, 0))
							END as 'Auto Liability Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalcollcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalcollcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalcollreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalcollreinstateminpremiumadjustment.totalamount, 0))
							END as 'Collision Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalcompftcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalcompftcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalcompftreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalcompftreinstateminpremiumadjustment.totalamount, 0))
							END as 'Comp/FT Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalcargocancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalcargocancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalcargoreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalcargoreinstateminpremiumadjustment.totalamount, 0))
							END as 'Cargo Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalalmpcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalalmpcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalalmpreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalalmpreinstateminpremiumadjustment.totalamount, 0))
							END as 'AL Manuscript Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalglcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalglcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalglreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalglreinstateminpremiumadjustment.totalamount, 0))
							END as 'General Liability Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalpdmpcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalpdmpcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalpdmpreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalpdmpreinstateminpremiumadjustment.totalamount, 0))
							END as 'PD Manuscript Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalpdcancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalpdcancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalpdreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalpdreinstateminpremiumadjustment.totalamount, 0))
							END as 'Physical Damage Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalhpdecancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalhpdecancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalhpdereinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalhpdereinstateminpremiumadjustment.totalamount, 0))
							END as 'Hired PD Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totaltiecancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totaltiecancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totaltiereinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totaltiereinstateminpremiumadjustment.totalamount, 0))
							END as 'Trailer Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalpnccancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalpnccancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalpncreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalpncreinstateminpremiumadjustment.totalamount, 0))
							END as 'PNC Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalwaivercancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalwaivercancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalwaiverreinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalwaiverreinstateminpremiumadjustment.totalamount, 0))
							END as 'Waiver Premium Adjustment',
							CASE WHEN rpd.[Transaction Type] = 'Cancellation'
								THEN IIF(SIGN(coalesce(totalaicancelminpremiumadjustment.totalamount, 0)) = -1, 0, coalesce(totalaicancelminpremiumadjustment.totalamount, 0)) 
								ELSE IIF(SIGN(coalesce(totalaireinstateminpremiumadjustment.totalamount, 0)) = 1, 0, coalesce(totalaireinstateminpremiumadjustment.totalamount, 0))
							END as 'Additional Insured Premium Adjustment',
							(coalesce(totalsf, 0) + coalesce(totaltax, 0) + coalesce(totalstampingfee, 0)) as 'Total Tax/Fees',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalpremium, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcompftpremiumchange.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE (coalesce(totalcollpremiumchange.totalamount, 0)) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalcargoprem, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalglprem, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpdpremium, 0) * (
									DATEDIFF
									(day, rpd.[Transaction Effective Date],
										(
											CASE WHEN latestTransactionDetails.PolicyStatus  = 'Cancellation'
											THEN
												CASE WHEN latestTransactionDetails.EffectiveDate <= @toDate
													THEN latestTransactionDetails.EffectiveDate
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
							END as 'PD Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalalmprem, 0)
							END as 'AL Manuscript Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpdmprem, 0)
							END as 'PD Manuscript Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalhiredpdendstpremium, 0)
							END as 'Hired PD Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totaltrailerinterchangepremium, 0)
							END as 'Trailer Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalpncpremium, 0)
							END as 'Primary and Non-Contributory Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalshortratepremium, 0)
							END as 'Short Rate Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalwaiver, 0)
							END as 'Waiver of Subrogation Earned Premium',
							CASE WHEN (latestTransactionDetails.PolicyStatus  = 'Cancellation' AND rpd.[Transaction Effective Date] > latestTransactionDetails.EffectiveDate) OR (rpd.[Transaction Effective Date] > @toDate)
							THEN 0
							ELSE coalesce(totalaiprem, 0)
							END as 'Additional Insured Earned Premium',
							((coalesce(totalalpremium, 0) + 
								coalesce(totalcargoprem, 0) +
								coalesce(totalglprem, 0) +
								coalesce(totalalmprem, 0))
							* (cast(rpd.[Broker Commision AL %] as decimal(18,2)) / 100.0)) as CommisionALAmount,
							((coalesce(totalpdpremium, 0) + 
								coalesce(totaltrailerinterchangepremium, 0) + 
								coalesce(totalhiredpdendstpremium, 0) +
								coalesce(totalpdmprem, 0))
							* (cast(rpd.[Broker Commision PD %] as decimal(18,2)) / 100.0)) as CommisionPDAmount,
							DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate) as 'Total days',
							DATEDIFF(DAY, rpd.[Effective Date], GETDATE()) as 'Total days passed',
							totalpremium / (DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate)) as 'Prem per day',
							rpd.MinimumEarned as 'Minimum Earned',
							latestTransactionDetails.Details as 'Latest Transaction Details'
							from
							(
								select 
									r.Id as 'RiskId',
									ph.Id as 'PolicyHistoryId',
									ph.PreviousRiskDetailId as 'RiskDetailId',
									r.PolicyNumber,
									r.Status,
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
									ph.TransactionNumber as 'Transaction Number',
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
								cross apply 
								(
									select TOP 1 * from EntityAddress 
									where EntityAddress.EntityId = ie.Id
									and EntityAddress.AddressTypeId = 2
									order by CreatedDate desc
								) business
								--inner join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
								inner join [Address] businessaddr on business.AddressId = businessaddr.Id
								left join EntityAddress garage on garage.EntityId = ie.Id and garage.AddressTypeId = 3 and garage.IsMainGaragingAddress = 1
								left join [Address] garageaddr on garage.AddressId = garageaddr.Id
								outer apply 
								(
									select TOP 1 * from EntityAddress 
									where EntityAddress.EntityId = ie.Id
									and EntityAddress.AddressTypeId = 4
									order by CreatedDate desc
								) mailing
								--left join EntityAddress mailing on mailing.EntityId = ie.Id and mailing.AddressTypeId = 4
								left join [Address] mailingaddr on mailing.AddressId = mailingaddr.Id
								left join Agency_Agency agency on agency.Id = bi.AgencyId
								left join Agency_Entity ae on ae.Id = agency.EntityId
								outer apply 
								(
									select TOP 1 * from Agency_EntityAddress 
									where Agency_EntityAddress.EntityId = ae.Id
									and Agency_EntityAddress.AddressTypeId = 1 
									order by CreatedDate desc
								) aea
								--left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
								left join Agency_Address aa on aea.AddressId = aa.Id
								outer apply 
								(
									select TOP 1 * from Agency_EntityAddress 
									where Agency_EntityAddress.EntityId = ae.Id
									and Agency_EntityAddress.AddressTypeId = 2 
									order by CreatedDate desc
								) aea2
								--left join Agency_EntityAddress aea2 on ae.Id = aea2.EntityId and aea2.AddressTypeId = 2 and aea2.RemovedProcessDate is null
								left join Agency_Address aa2 on aea2.AddressId = aa2.Id
								left join BusinessTypes bt on ie.BusinessTypeID = bt.Id
								left join (
									select rdv.RiskDetailId, COUNT(rdv.RiskDetailId) as vcount from RiskDetailVehicle rdv
									group by rdv.RiskDetailId
								) all_vehicle on all_vehicle.RiskDetailId = rd.Id
								where ph.DateCreated IS NOT NULL
									and ph.DateCreated >= @fromDate
									and ph.DateCreated <= @toDate
							) rpd
							left join
							(
								SELECT RiskId, PolicyHistoryId,  
									totalpremium, 
									totalaiprem, 
									totalalpremium, 
									totalpdpremium, 
									totaltrailerinterchangepremium, 
									totalhiredpdendstpremium, 
									totalpncpremium,
									totalalshortratepremium,
									totalcargoshortratepremium,
									totalalmshortratepremium,
									totalglshortratepremium,
									totalpdshortratepremium,
									totalpdmshortratepremium,
									totalsf,
									totalstampingfee,
									totaltax,
									totalwaiver,
									totalalmprem,
									totalpdmprem,
									totalcargoprem,
									totalglprem,
									totalshortratepremium,
									TransactionTypeId FROM   
								(
									SELECT RiskId, PolicyHistoryId, totalamount, TransType, TransactionTypeId
										FROM     dbo.wv_BDXUnionFees
								) t 
							PIVOT(
								SUM(totalamount) 
								FOR TransType IN (
									totalpremium, 
									totalaiprem, 
									totalalpremium, 
									totalpdpremium, 
									totaltrailerinterchangepremium, 
									totalhiredpdendstpremium, 
									totalpncpremium,
									totalalshortratepremium,
									totalcargoshortratepremium,
									totalalmshortratepremium,
									totalglshortratepremium,
									totalpdshortratepremium,
									totalpdmshortratepremium,
									totalsf,
									totalstampingfee,
									totaltax,
									totalwaiver,
									totalalmprem,
									totalpdmprem,
									totalcargoprem,
									totalglprem,
									totalshortratepremium)
							) AS pvtCharge
							) bt  ON bt.RiskId = rpd.RiskId and bt.PolicyHistoryId = rpd.PolicyHistoryId AND bt.TransactionTypeId = rpd.[Transaction Type] 
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId <> 'PDP'	
							) totalmincancelpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId <> 'PDP'
							) totalminreinstatepremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'ALP'			
							) totalalmincancelpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'ALP'			
							) totalalminreinstatepremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'CP'
							) totalcargocancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'CP'
							) totalcargoreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'ALMP'
							) totalalmpcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'ALMP'
							) totalalmpreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'GLP'
							) totalglcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'GLP'
							) totalglreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDP'
							) totalpdcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDP'
							) totalpdreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDM'
							) totalpdmpcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDM'
							) totalpdmpreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDPColl'
							) totalcollcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDPColl'
							) totalcollreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PDPCompFt'
							) totalcompftcancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PDPCompFt'
							) totalcompftreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'HPDE'
							) totalhpdecancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'HPDE'
							) totalhpdereinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'TIE'
							) totaltiecancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'TIE'
							) totaltiereinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'PNC'
							) totalpnccancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'PNC'
							) totalpncreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'WS'
							) totalwaivercancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'WS'
							) totalwaiverreinstateminpremiumadjustment
							cross apply
							(
								select SUM(cb.MinimumPremiumAdjustment) as totalamount from CancellationBreakdown cb
								where cb.RiskDetailId = rpd.RiskDetailId and cb.AmountSubTypeId = 'AIP'
							) totalaicancelminpremiumadjustment
							cross apply
							(
								select SUM(rb.MinimumPremiumAdjustment) as totalamount from ReinstatementBreakdown rb
								where rb.RiskDetailId = rpd.RiskDetailId and rb.AmountSubTypeId = 'AIP'
							) totalaireinstateminpremiumadjustment
							cross apply
							(
								select SUM(rvpt.PremiumChangeColl) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcollpremiumchange
							cross apply
							(
								select SUM(rvpt.PremiumChangeCompFt) as totalamount from RiskVehiclePremiumTransaction rvpt
								where rvpt.RiskDetailId = rpd.RiskDetailId and rvpt.FromReinstatement = 
									CASE
										WHEN rpd.[Transaction Type] = 'Reinstatement'
										THEN 1
										ELSE 0
									END
							) totalcompftpremiumchange
							cross apply
							(
								select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle rdv
								join Vehicle v on rdv.VehicleId = v.Id
								where rpd.RiskDetailId = rdv.RiskDetailId and (rpd.[Endorsement Number] = 0 or rpd.Details LIKE '%Added VIN ''' + v.VIN + '''%')
							) totaladdedtiv
							cross apply
							(
								select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle rdv
								join Vehicle v on rdv.VehicleId = v.Id
								where rpd.RiskDetailId = rdv.RiskDetailId and rpd.Details LIKE '%Removed VIN ''' + v.VIN + '''%'
							) totalremovedtiv
							cross apply
							(
								select top 1 ph.Details, ph.EffectiveDate, ph.DateCreated,
									CASE
										WHEN ph.EndorsementNumber = 0
										THEN 'New'
										WHEN ph.PolicyStatus = 'Reinstated'
										THEN 'Reinstatement'
										WHEN ph.PolicyStatus = 'Canceled'
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and (ph.PolicyStatus = 'Rewrite' and rd_temp.Status = 'Canceled') --Canceled/Rewrite
										THEN 'Cancellation'
										WHEN ph.EndorsementNumber != 0 and rd_temp.Status = 'Rewrite'
										THEN 'New'
										ELSE 'Endorsement'
									END as PolicyStatus
									from PolicyHistory ph
								inner join Risk r_temp on r_temp.PolicyNumber = rpd.PolicyNumber and ph.RiskId = r_temp.Id
								inner join RiskDetail rd_temp on ph.PreviousRiskDetailId = rd_temp.Id
								where ph.DateCreated IS NOT NULL
									and ph.DateCreated >= @fromDate
									and ph.DateCreated <= @toDate
								order by ph.DateCreated desc
							) latestTransactionDetails
						) rpt
					--where rpt.RiskId = '46498d0f-4102-4d70-9bf8-d6256176cfa0'
					where rpt.[Policy Number] IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				) brpt
				order by brpt.[Policy Number], brpt.[Transaction Date], brpt.[Transaction Number]			

			END
        ";


        #endregion
    }
}
