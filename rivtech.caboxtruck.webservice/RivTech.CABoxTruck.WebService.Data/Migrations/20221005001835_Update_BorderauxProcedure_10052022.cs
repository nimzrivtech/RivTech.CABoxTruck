using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_BorderauxProcedure_10052022 : Migration
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
				@date datetime = NULL
			AS
			BEGIN

                SET NOCOUNT ON;

				select
					rpt.PolicyNumber,
					rpt.[Endorsement Number],
					rpt.[Transaction Number],
					rpt.[Insured Name],
					rpt.[Risk State],
					rpt.[Transaction Type],
					rpt.[Transaction Date],
					rpt.[Transaction Effective Date],
					rpt.EffectiveDate,
					rpt.ExpirationDate,
					CASE WHEN rpt.[Garaging Address] IS NULL THEN '' ELSE rpt.[Garaging Address] END as 'Garaging Address',
					CASE WHEN rpt.[Garaging City] IS NULL THEN '' ELSE rpt.[Garaging City] END as 'Garaging City',
					CASE WHEN rpt.[Garaging State] IS NULL THEN '' ELSE rpt.[Garaging State] END as 'Garaging State',
					CASE WHEN rpt.[Garaging ZipCode] IS NULL THEN '' ELSE rpt.[Garaging ZipCode] END as 'Garaging ZipCode',
					CASE WHEN rpt.[Mailing Address] IS NULL THEN '' ELSE rpt.[Mailing Address] END as 'Mailing Address',
					CASE WHEN rpt.[Mailing City] IS NULL THEN '' ELSE rpt.[Mailing City] END as 'Mailing City',
					CASE WHEN rpt.[Mailing State] IS NULL THEN '' ELSE rpt.[Mailing State] END as 'Mailing State',
					CASE WHEN rpt.[Mailing ZipCode] IS NULL THEN '' ELSE rpt.[Mailing ZipCode] END as 'Mailing ZipCode',
					CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Vehicle/Unit Count]) END as 'Vehicle/Unit Count',
					CASE WHEN rpt.[Total Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Total Premium]) END as 'Total Premium',
					CASE WHEN rpt.[Auto Liability Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Auto Liability Premium]) END as 'Auto Liability Premium',
					CASE WHEN rpt.[Auto Liability Limit] IS NULL THEN '' ELSE rpt.[Auto Liability Limit] END as 'Auto Liability Limit',
					CASE WHEN rpt.[Auto Liability Deductible] IS NULL THEN '' ELSE rpt.[Auto Liability Deductible] END as 'Auto Liability Deductible',
					CASE WHEN rpt.[AL Manuscript Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[AL Manuscript Premium]) END as 'AL Manuscript Premium',
					CASE WHEN rpt.[General Liability Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[General Liability Premium]) END as 'General Liability Premium',
					CASE WHEN rpt.[PIP Limit] IS NULL THEN '' ELSE rpt.[PIP Limit] END as 'PIP Limit',
					CASE WHEN rpt.[Auto Medical Payment Limit] IS NULL THEN '' ELSE rpt.[Auto Medical Payment Limit] END as 'Auto Medical Payment Limit',
					rpt.Symbol,
					CASE WHEN rpt.CollisionPremium IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.CollisionPremium) END as 'CollisionPremium',
					CASE WHEN rpt.[Collision Deductible] IS NULL THEN '' ELSE rpt.[Collision Deductible] END as 'Collision Deductible',
					CASE WHEN rpt.[Comp/FT Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Comp/FT Premium]) END as 'Comp/FT Premium',
					CASE WHEN rpt.[Comp/FT Deductible] IS NULL THEN '' ELSE rpt.[Comp/FT Deductible] END as 'Comp/FT Deductible',
					CASE WHEN rpt.[PD Manuscript Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[PD Manuscript Premium]) END as 'PD Manuscript Premium',
					CASE WHEN rpt.[Motor Truck Cargo Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Motor Truck Cargo Premium]) END as 'Motor Truck Cargo Premium',
					CASE WHEN rpt.[Motor Truck Cargo Limit] IS NULL THEN '' ELSE rpt.[Motor Truck Cargo Limit] END as 'Motor Truck Cargo Limit',
					CASE WHEN rpt.[Uninsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists BI Limit/Deductible] END as 'Uninsured Motorists BI Limit/Deductible',
					CASE WHEN rpt.[Uninsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists PD Limit/Deductible] END as 'Uninsured Motorists PD Limit/Deductible',
					CASE WHEN rpt.[Underinsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists BI Limit/Deductible] END as 'Underinsured Motorists BI Limit/Deductible',
					CASE WHEN rpt.[Underinsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists PD Limit/Deductible] END as 'Underinsured Motorists PD Limit/Deductible',
					CASE WHEN rpt.[Additional Insured Amount] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Additional Insured Amount]) END as 'Additional Insured Amount',
					CASE WHEN rpt.[Total Insured Value] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Total Insured Value]) END as 'Total Insured Value',
					CASE WHEN rpt.[Service Fee] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Service Fee]) END as 'Service Fee',
					CASE WHEN rpt.[Stamping Fees] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Stamping Fees]) END as 'Stamping Fees',
					CASE WHEN rpt.[SL Tax] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[SL Tax]) END as 'SL Tax',
					CASE WHEN rpt.[Waiver of Subrogation] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Waiver of Subrogation]) END as 'Waiver of Subrogation',
					CASE WHEN rpt.[Total Tax/Fees] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Total Tax/Fees]) END as 'Total Tax/Fees',
					--rpt.[Total days],
					--rpt.[Total days passed],
					--rpt.[Prem per day],
					coalesce(CONVERT(DECIMAL(10,2),rpt.[Prem per day] * rpt.[Total days passed]), 0) as 'Earned Premium',
					CASE WHEN rpt.[Minimum Premium Adjustment] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Minimum Premium Adjustment]) END as 'Minimum Premium Adjustment',
					rpt.[Broker Agency],
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
					rpt.[Broker Commision AL %],
					rpt.[Broker Commision PD %],
					coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionALAmount), 0) as 'Broker Commission AL',
					coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionPDAmount), 0) as 'Broker Commission PD',
					coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionPDAmount), 0) as 'Total Commission',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN ('2.5%')
						ELSE ('0%') 
					END AS 'ARS Commission %',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN 
							CONVERT(decimal(10,2), ((coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionPDAmount), 0)) * (2.5/100)))
						ELSE 0
					END AS 'ARS Commission',
					rpt.[Credited Office]
				from
					(
						select rpd.*,
						totaltransactions.transactionnumber as 'Transaction Number',
						totalpremium.totalamount as 'Total Premium',
						totalalpremium.totalamount as 'Auto Liability Premium',
						totalglprem.totalamount as 'General Liability Premium',
						(coalesce(totalcol.totalamount, 0) - coalesce(previoustotalcol.totalamount, 0)) as 'CollisionPremium',
						(coalesce(totalcompft.totalamount, 0) - coalesce(previoustotalcompft.totalamount, 0)) as 'Comp/FT Premium',
						totalaiprem.totalamount as 'Additional Insured Amount',
						totaltiv.totalamount as 'Total Insured Value',
						totalsf.totalamount as 'Service Fee',
						totalstampingfee.totalamount as 'Stamping Fees',
						totaltax.totalamount as 'SL Tax',
						totalwaiver.totalamount as 'Waiver of Subrogation',
						totalcargoprem.totalamount as 'Motor Truck Cargo Premium',
						totalalmprem.totalamount as 'AL Manuscript Premium',
						totalpdmprem.totalamount as 'PD Manuscript Premium',
						totalminpremiumadjustment.totalamount as 'Minimum Premium Adjustment',
						(coalesce(totalsf.totalamount, 0) + coalesce(totaltax.totalamount, 0) + coalesce(totalstampingfee.totalamount, 0)) as 'Total Tax/Fees',
						(totalalpremium.totalamount * (cast(rpd.[Broker Commision AL %] as decimal(18,2)) / 100.0)) as CommisionALAmount,
						(totalpdpremium.totalamount * (cast(rpd.[Broker Commision PD %] as decimal(18,2)) / 100.0)) as CommisionPDAmount,
						DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate) as 'Total days',
						DATEDIFF(DAY, rpd.[Effective Date], GETDATE()) as 'Total days passed',
						totalpremium.totalamount / (DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate)) as 'Prem per day'
						--CONCAT(rpd.[Broker Commision AL %],'/',rpd.[Broker Commision PD %],'/', CASE WHEN rpd.CreditedOfficeId = 3 THEN ('2.5') ELSE ('0') END) as 'Total Commission %'
						--CASE WHEN rpd.CreditedOfficeId = 3 
						--	THEN ((rep.CommisionALAmount + rep.CommisionPDAmount) * (2.5/100)) 
						--	ELSE ((rep.CommisionALAmount + rep.CommisionPDAmount) * 0) 
						--END AS 'ARS Commission',
						--CASE WHEN rpd.CreditedOfficeId = 3 
						--	THEN ('2.5%')
						--	ELSE ('0%') 
						--END AS 'ARS Commission %'
						from
						(
							select 
								r.Id as 'RiskId',
								ph.PreviousRiskDetailId as 'RiskDetailId',
								r.PolicyNumber,
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
									ELSE (LEN(ph.Details) - LEN(REPLACE(ph.Details, 'Added VIN', ''))) / LEN('Added VIN')
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
								ph.Details
							from PolicyHistory ph
							left join Risk r on ph.RiskId = r.Id
							left join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1 --TODO: To be confirmed if IsProcessed is to be removed from query so all policies are captured
							left join [Binding] b on b.RiskId = r.Id
							left join BrokerInfo bi on bi.RiskId = r.Id
							left join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
							left join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
							left join Insured i on rd.InsuredId = i.Id
							left join Entity ie on i.EntityId = ie.Id
							left join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
							left join [Address] businessaddr on business.AddressId = businessaddr.Id
							left join EntityAddress garage on garage.EntityId = ie.Id and garage.AddressTypeId = 3 and garage.IsMainGaragingAddress = 1
							left join [Address] garageaddr on garage.AddressId = garageaddr.Id
							left join EntityAddress mailing on mailing.EntityId = ie.Id and mailing.AddressTypeId = 4
							left join [Address] mailingaddr on mailing.AddressId = mailingaddr.Id
							left join Agency_Agency agency on agency.Id = bi.AgencyId
							left join Agency_Entity ae on ae.Id = agency.EntityId
							left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1
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
								bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and bt.RiskId = rpd.RiskId
						) totalpremium
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction bt
							join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
							where btd.AmountSubTypeId = 'AIP' and btd.AmountTypeId = 'P' and 
								bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and bt.RiskId = rpd.RiskId
						) totalaiprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'ALP' or 
								   btd.AmountSubTypeId = 'AIP' or 
								   btd.AmountSubTypeId = 'WS' or 
								   btd.AmountSubTypeId = 'PNC') and btd.AmountTypeId = 'P' and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalalpremium
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'PDP') and btd.AmountTypeId = 'P' and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalpdpremium
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'ALSF' or btd.AmountSubTypeId = 'PDSF') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalsf
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'SF') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalstampingfee
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'SLT') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totaltax
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'WS') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalwaiver
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'ALMP') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalalmprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'PDM') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalpdmprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'CP') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalcargoprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'GLP') and 
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
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
								btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalminpremiumadjustment
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
				where rpt.PolicyNumber IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				order by rpt.PolicyNumber, rpt.[Transaction Number]

			END";

        const string REVERT_BorderauxReport = @"
            CREATE OR ALTER   PROCEDURE [dbo].[GenerateBorderauxReport] 
                @date datetime = NULL
            AS
            BEGIN

				SET NOCOUNT ON;
            
				
				select
					rpt.PolicyNumber,
					rpt.[Endorsement Number],
					rpt.[Transaction Number],
					rpt.[Insured Name],
					rpt.[Risk State],
					rpt.[Transaction Type],
					rpt.[Transaction Date],
					rpt.[Transaction Effective Date],
					rpt.EffectiveDate,
					rpt.ExpirationDate,
					CASE WHEN rpt.[Garaging Address] IS NULL THEN '' ELSE rpt.[Garaging Address] END as 'Garaging Address',
					CASE WHEN rpt.[Garaging City] IS NULL THEN '' ELSE rpt.[Garaging City] END as 'Garaging City',
					CASE WHEN rpt.[Garaging State] IS NULL THEN '' ELSE rpt.[Garaging State] END as 'Garaging State',
					CASE WHEN rpt.[Garaging ZipCode] IS NULL THEN '' ELSE rpt.[Garaging ZipCode] END as 'Garaging ZipCode',
					CASE WHEN rpt.[Mailing Address] IS NULL THEN '' ELSE rpt.[Mailing Address] END as 'Mailing Address',
					CASE WHEN rpt.[Mailing City] IS NULL THEN '' ELSE rpt.[Mailing City] END as 'Mailing City',
					CASE WHEN rpt.[Mailing State] IS NULL THEN '' ELSE rpt.[Mailing State] END as 'Mailing State',
					CASE WHEN rpt.[Mailing ZipCode] IS NULL THEN '' ELSE rpt.[Mailing ZipCode] END as 'Mailing ZipCode',
					CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Vehicle/Unit Count]) END as 'Vehicle/Unit Count',
					CASE WHEN rpt.[Total Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Total Premium]) END as 'Total Premium',
					CASE WHEN rpt.[Auto Liability Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Auto Liability Premium]) END as 'Auto Liability Premium',
					CASE WHEN rpt.[Auto Liability Limit] IS NULL THEN '' ELSE rpt.[Auto Liability Limit] END as 'Auto Liability Limit',
					CASE WHEN rpt.[Auto Liability Deductible] IS NULL THEN '' ELSE rpt.[Auto Liability Deductible] END as 'Auto Liability Deductible',
					CASE WHEN rpt.[AL Manuscript Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[AL Manuscript Premium]) END as 'AL Manuscript Premium',
					CASE WHEN rpt.[General Liability Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[General Liability Premium]) END as 'General Liability Premium',
					CASE WHEN rpt.[PIP Limit] IS NULL THEN '' ELSE rpt.[PIP Limit] END as 'PIP Limit',
					CASE WHEN rpt.[Auto Medical Payment Limit] IS NULL THEN '' ELSE rpt.[Auto Medical Payment Limit] END as 'Auto Medical Payment Limit',
					rpt.Symbol,
					CASE WHEN rpt.CollisionPremium IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.CollisionPremium) END as 'CollisionPremium',
					CASE WHEN rpt.[Collision Deductible] IS NULL THEN '' ELSE rpt.[Collision Deductible] END as 'Collision Deductible',
					CASE WHEN rpt.[Comp/FT Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Comp/FT Premium]) END as 'Comp/FT Premium',
					CASE WHEN rpt.[Comp/FT Deductible] IS NULL THEN '' ELSE rpt.[Comp/FT Deductible] END as 'Comp/FT Deductible',
					CASE WHEN rpt.[PD Manuscript Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[PD Manuscript Premium]) END as 'PD Manuscript Premium',
					CASE WHEN rpt.[Motor Truck Cargo Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Motor Truck Cargo Premium]) END as 'Motor Truck Cargo Premium',
					CASE WHEN rpt.[Motor Truck Cargo Limit] IS NULL THEN '' ELSE rpt.[Motor Truck Cargo Limit] END as 'Motor Truck Cargo Limit',
					CASE WHEN rpt.[Uninsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists BI Limit/Deductible] END as 'Uninsured Motorists BI Limit/Deductible',
					CASE WHEN rpt.[Uninsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists PD Limit/Deductible] END as 'Uninsured Motorists PD Limit/Deductible',
					CASE WHEN rpt.[Underinsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists BI Limit/Deductible] END as 'Underinsured Motorists BI Limit/Deductible',
					CASE WHEN rpt.[Underinsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists PD Limit/Deductible] END as 'Underinsured Motorists PD Limit/Deductible',
					CASE WHEN rpt.[Additional Insured Amount] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Additional Insured Amount]) END as 'Additional Insured Amount',
					CASE WHEN rpt.[Total Insured Value] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Total Insured Value]) END as 'Total Insured Value',
					CASE WHEN rpt.[Service Fee] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Service Fee]) END as 'Service Fee',
					CASE WHEN rpt.[Stamping Fees] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Stamping Fees]) END as 'Stamping Fees',
					CASE WHEN rpt.[SL Tax] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[SL Tax]) END as 'SL Tax',
					CASE WHEN rpt.[Waiver of Subrogation] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Waiver of Subrogation]) END as 'Waiver of Subrogation',
					CASE WHEN rpt.[Total Tax/Fees] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Total Tax/Fees]) END as 'Total Tax/Fees',
					--rpt.[Total days],
					--rpt.[Total days passed],
					--rpt.[Prem per day],
					coalesce(CONVERT(DECIMAL(10,2),rpt.[Prem per day] * rpt.[Total days passed]), 0) as 'Earned Premium',
					rpt.[Broker Agency],
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
					rpt.[Broker Commision AL %],
					rpt.[Broker Commision PD %],
					coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionALAmount), 0) as 'Broker Commission AL',
					coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionPDAmount), 0) as 'Broker Commission PD',
					coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionPDAmount), 0) as 'Total Commission',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN ('2.5%')
						ELSE ('0%') 
					END AS 'ARS Commission %',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN 
							CONVERT(decimal(10,2), ((coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(10,2),rpt.CommisionPDAmount), 0)) * (2.5/100)))
						ELSE 0
					END AS 'ARS Commission',
					rpt.[Credited Office]
				from
					(
						select rpd.*,
						totaltransactions.transactionnumber as 'Transaction Number',
						totalpremium.totalamount as 'Total Premium',
						totalalpremium.totalamount as 'Auto Liability Premium',
						--CASE
						--	WHEN rpd.EndorsementNumber = 0
						--	THEN rpd.CollisionPremium
						--	ELSE totalcol.totalamount
						--END as 'Collision Premium',
						--totalcolprorated.totalamount as 'Collision Net',
						totalglprem.totalamount as 'General Liability Premium',
						totalcol.totalamount as 'CollisionPremium',
						totalcompft.totalamount as 'Comp/FT Premium',
						totalaiprem.totalamount as 'Additional Insured Amount',
						totaltiv.totalamount as 'Total Insured Value',
						totalsf.totalamount as 'Service Fee',
						totalstampingfee.totalamount as 'Stamping Fees',
						totaltax.totalamount as 'SL Tax',
						totalwaiver.totalamount as 'Waiver of Subrogation',
						totalcargoprem.totalamount as 'Motor Truck Cargo Premium',
						totalalmprem.totalamount as 'AL Manuscript Premium',
						totalpdmprem.totalamount as 'PD Manuscript Premium',
						(totalsf.totalamount + totaltax.totalamount + totalstampingfee.totalamount) as 'Total Tax/Fees',
						(totalalpremium.totalamount * (cast(rpd.[Broker Commision AL %] as decimal(18,2)) / 100.0)) as CommisionALAmount,
						(totalpdpremium.totalamount * (cast(rpd.[Broker Commision PD %] as decimal(18,2)) / 100.0)) as CommisionPDAmount,
						--((totalcol.totalamount - rpd.[Collision Premium]) + (totalpdpremium.totalamount - rpd.ComprehensivePremium)) as 'CollisionPremium', 
						--(totalpdpremium.totalamount - rpd.[Collision Premium]) as 'Comp/FT Premium', 
						DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate) as 'Total days',
						DATEDIFF(DAY, rpd.[Effective Date], GETDATE()) as 'Total days passed',
						totalpremium.totalamount / (DATEDIFF(DAY, rpd.[Effective Date], rpd.ExpirationDate)) as 'Prem per day'
						--CONCAT(rpd.[Broker Commision AL %],'/',rpd.[Broker Commision PD %],'/', CASE WHEN rpd.CreditedOfficeId = 3 THEN ('2.5') ELSE ('0') END) as 'Total Commission %'
						--CASE WHEN rpd.CreditedOfficeId = 3 
						--	THEN ((rep.CommisionALAmount + rep.CommisionPDAmount) * (2.5/100)) 
						--	ELSE ((rep.CommisionALAmount + rep.CommisionPDAmount) * 0) 
						--END AS 'ARS Commission',
						--CASE WHEN rpd.CreditedOfficeId = 3 
						--	THEN ('2.5%')
						--	ELSE ('0%') 
						--END AS 'ARS Commission %'
						from
						(
							select 
								r.Id as 'RiskId',
								ph.PreviousRiskDetailId as 'RiskDetailId',
								r.PolicyNumber,
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
									ELSE (LEN(ph.Details) - LEN(REPLACE(ph.Details, 'Added VIN', ''))) / LEN('Added VIN')
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
								--rcp.CollisionPremium as 'CollisionPremium',
								ph.Details
							from PolicyHistory ph
							left join Risk r on ph.RiskId = r.Id
							left join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1 --TODO: To be confirmed if IsProcessed is to be removed so all policies are captured
							left join [Binding] b on b.RiskId = r.Id
							left join BrokerInfo bi on bi.RiskId = r.Id
							left join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
							left join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
							left join Insured i on rd.InsuredId = i.Id
							left join Entity ie on i.EntityId = ie.Id
							left join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
							left join [Address] businessaddr on business.AddressId = businessaddr.Id
							left join EntityAddress garage on garage.EntityId = ie.Id and garage.AddressTypeId = 3 and garage.IsMainGaragingAddress = 1
							left join [Address] garageaddr on garage.AddressId = garageaddr.Id
							left join EntityAddress mailing on mailing.EntityId = ie.Id and mailing.AddressTypeId = 4
							left join [Address] mailingaddr on mailing.AddressId = mailingaddr.Id
							left join Agency_Agency agency on agency.Id = bi.AgencyId
							left join Agency_Entity ae on ae.Id = agency.EntityId
							left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1
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
							where btd.AmountTypeId = 'P' and bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and bt.RiskId = rpd.RiskId
						) totalpremium
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction bt
							join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
							where btd.AmountSubTypeId = 'AIP' and btd.AmountTypeId = 'P' and bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and bt.RiskId = rpd.RiskId
						) totalaiprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'ALP' or 
								   btd.AmountSubTypeId = 'AIP' or 
								   btd.AmountSubTypeId = 'WS' or 
								   btd.AmountSubTypeId = 'PNC') and btd.AmountTypeId = 'P'
							and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalalpremium
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'PDP') and btd.AmountTypeId = 'P'
							and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalpdpremium
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'ALSF' or btd.AmountSubTypeId = 'PDSF') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalsf
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'SF') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalstampingfee
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'SLT') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totaltax
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'WS') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalwaiver
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'ALMP') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalalmprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'PDM') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalpdmprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'CP') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalcargoprem
						cross apply
						(
							select SUM(btd.Amount) as totalamount from Billing_Transaction btt
							join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
							where (btd.AmountSubTypeId = 'GLP') and btt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Transaction Date]) and btt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Transaction Date]) and btt.RiskId = rpd.RiskId
						) totalglprem
						cross apply
						(
							select SUM(r.CurrentCollPremium) as totalamount from RiskVehiclePremiumTransaction r
							where r.RiskDetailId = rpd.RiskDetailId
						) totalcol
						cross apply
						(
							select SUM(r.CurrentCompPremium) as totalamount from RiskVehiclePremiumTransaction r
							where r.RiskDetailId = rpd.RiskDetailId 
						) totalcompft
						cross apply
						(
							select SUM(v.StatedAmount) as totalamount from RiskDetailVehicle r
							join Vehicle v on r.VehicleId = v.Id
							where rpd.RiskDetailId = r.RiskDetailId and (rpd.[Endorsement Number] = 0 or rpd.Details LIKE '%Added VIN ''' + v.VIN + '''%')
						) totaltiv
					) rpt
				--where rpt.RiskId = '46498d0f-4102-4d70-9bf8-d6256176cfa0'
				where rpt.PolicyNumber IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				order by rpt.PolicyNumber, rpt.[Transaction Number]

            END";
    }
}
