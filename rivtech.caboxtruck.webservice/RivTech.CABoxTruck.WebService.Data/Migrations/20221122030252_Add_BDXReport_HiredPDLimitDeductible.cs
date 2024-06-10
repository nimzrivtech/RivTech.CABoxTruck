using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Add_BDXReport_HiredPDLimitDeductible : Migration
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
					rpt.PolicyNumber,
					rpt.[Endorsement Number] 'EndorsementNumber',
					rpt.[Transaction Number] 'TransactionNumber',
					rpt.[Insured Name] 'InsuredName',
					rpt.[Risk State] 'RiskState',
					rpt.[Transaction Type] 'TransactionType',
					rpt.[Transaction Date] 'TransactionDate',
					rpt.[Transaction Effective Date] 'TransactionEffectiveDate',
					rpt.EffectiveDate,
					rpt.ExpirationDate,
					CASE WHEN rpt.[Garaging Address] IS NULL THEN '' ELSE rpt.[Garaging Address] END as 'GaragingAddress',
					CASE WHEN rpt.[Garaging City] IS NULL THEN '' ELSE rpt.[Garaging City] END as 'GaragingCity',
					CASE WHEN rpt.[Garaging State] IS NULL THEN '' ELSE rpt.[Garaging State] END as 'GaragingState',
					CASE WHEN rpt.[Garaging ZipCode] IS NULL THEN '' ELSE rpt.[Garaging ZipCode] END as 'GaragingZipCode',
					CASE WHEN rpt.[Mailing Address] IS NULL THEN '' ELSE rpt.[Mailing Address] END as 'MailingAddress',
					CASE WHEN rpt.[Mailing City] IS NULL THEN '' ELSE rpt.[Mailing City] END as 'MailingCity',
					CASE WHEN rpt.[Mailing State] IS NULL THEN '' ELSE rpt.[Mailing State] END as 'MailingState',
					CASE WHEN rpt.[Mailing ZipCode] IS NULL THEN '' ELSE rpt.[Mailing ZipCode] END as 'MailingZipCode',
					CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(18,2),rpt.[Vehicle/Unit Count]) END as 'VehicleUnitCount',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) as 'TotalPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium]), 0) as 'AutoLiabilityPremium',
					CASE WHEN rpt.[Auto Liability Limit] IS NULL THEN '' ELSE rpt.[Auto Liability Limit] END as 'AutoLiabilityLimit',
					CASE WHEN rpt.[Auto Liability Deductible] IS NULL THEN '' ELSE rpt.[Auto Liability Deductible] END as 'AutoLiabilityDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium]), 0) as 'ALManuscriptPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium]), 0) as 'GeneralLiabilityPremium',
					CASE WHEN rpt.[PIP Limit] IS NULL THEN '' ELSE rpt.[PIP Limit] END as 'PIPLimit',
					CASE WHEN rpt.[Auto Medical Payment Limit] IS NULL THEN '' ELSE rpt.[Auto Medical Payment Limit] END as 'AutoMedicalPaymentLimit',
					rpt.Symbol,
					--CASE WHEN rpt.[Physical Damage Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Physical Damage Premium] ) END as 'Physical Damage Premium', --TODO: Removed since PD Premium is not required in the report
					coalesce(CONVERT(DECIMAL(18,2),rpt.CollisionPremium), 0) as 'CollisionPremium',
					CASE WHEN rpt.[Collision Deductible] IS NULL THEN '' ELSE rpt.[Collision Deductible] END as 'CollisionDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium]), 0) as 'CompFTPremium',
					CASE WHEN rpt.[Comp/FT Deductible] IS NULL THEN '' ELSE rpt.[Comp/FT Deductible] END as 'CompFTDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium]), 0) as 'PDManuscriptPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Motor Truck Cargo Premium]), 0) as 'CargoPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium]), 0) as 'HiredPDPremium',
					ISNULL(CASE WHEN (rpt.[Hired PD Premium] = 0 AND rpt.[Hired PD Limit] = 0) THEN '' ELSE rpt.[Hired PD Limit] END, '') as 'HiredPDLimit',
					ISNULL(CASE WHEN (rpt.[Hired PD Premium] = 0 AND rpt.[Hired PD Deductible] = 0) THEN '' ELSE rpt.[Hired PD Deductible] END, '') as 'HiredPDDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0) as 'TrailerPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0) as 'PNCPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Premium]), 0) as 'ShortRatePremium',
					CASE WHEN rpt.[Motor Truck Cargo Limit] IS NULL THEN '' ELSE rpt.[Motor Truck Cargo Limit] END as 'CargoLimit',
					CASE WHEN rpt.[Uninsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists BI Limit/Deductible] END as 'UMBILimit',
					CASE WHEN rpt.[Uninsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists PD Limit/Deductible] END as 'UMPDLimit',
					CASE WHEN rpt.[Underinsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists BI Limit/Deductible] END as 'UIMBILimit',
					CASE WHEN rpt.[Underinsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists PD Limit/Deductible] END as 'UIMPDLimit',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0) as 'AdditionalInsuredAmount',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Insured Value]), 0) as 'TotalInsuredValue',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Service Fee]), 0) as 'ServiceFee',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Stamping Fees]), 0) as 'StampingFee',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[SL Tax]), 0) as 'SLTax',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0) as 'WaiverOfSubrogation',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Tax/Fees]), 0) as 'TotalTaxFees',
					--rpt.[Total days],
					--rpt.[Total days passed],
					--rpt.[Prem per day],
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Prem per day] * rpt.[Total days passed]), 0) as 'EarnedPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Minimum Premium Adjustment]), 0) as 'MinimumPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0) as 'AutoLiabilityPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0) as 'CargoPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0) as 'ALManuscriptPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0) as 'GeneralLiabilityPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Premium Adjustment]), 0) as 'PhysicalDamagePremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium Adjustment]), 0) as 'PDManuscriptPremiumAdjustment',
					CASE WHEN rpt.[Broker Agency] IS NULL THEN '' ELSE rpt.[Broker Agency] END as 'BrokerAgency',
					CASE WHEN rpt.Broker_Billing_Address IS NULL THEN '' ELSE rpt.Broker_Billing_Address END as 'Broker_Billing_Address',
					CASE WHEN rpt.[Billing Address] IS NULL THEN '' ELSE rpt.[Billing Address] END as 'BillingAddress',
					CASE WHEN rpt.[Billing City] IS NULL THEN '' ELSE rpt.[Billing City] END as 'BillingCity',
					CASE WHEN rpt.[Billing StateCode] IS NULL THEN '' ELSE rpt.[Billing StateCode] END as 'BillingStateCode',
					CASE WHEN rpt.[Billing Zipcode] IS NULL THEN '' ELSE rpt.[Billing Zipcode] END as 'BillingZipcode',
					CASE WHEN rpt.Broker_Business_Address IS NULL THEN '' ELSE rpt.Broker_Business_Address END as 'Broker_Business_Address',
					CASE WHEN rpt.[Business Address] IS NULL THEN '' ELSE rpt.[Business Address] END as 'BusinessAddress',
					CASE WHEN rpt.[Business City] IS NULL THEN '' ELSE rpt.[Business City] END as 'BusinessCity',
					CASE WHEN rpt.[Business StateCode] IS NULL THEN '' ELSE rpt.[Business StateCode] END as 'BusinessStateCode',
					CASE WHEN rpt.[Business Zipcode] IS NULL THEN '' ELSE rpt.[Business Zipcode] END as 'BusinessZipcode',
					rpt.[Broker Commision AL %] as 'BrokerCommissionALPercent',
					rpt.[Broker Commision PD %] as 'BrokerCommissionPDPercent',
					coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) as 'BrokerCommissionAL',
					coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'BrokerCommissionPD',
					coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'TotalCommission',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN ('2.5%')
						ELSE ('0%') 
					END AS 'ARSCommissionPercent',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN 
							CONVERT(decimal(18,2), ((coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0)) * (2.5/100)))
						ELSE 0
					END AS 'ARSCommission',
					rpt.[Payment Plan] 'PaymentPlan',
					rpt.[Credited Office] 'CreditedOffice'
				from
					(
						select rpd.*,
						totaltransactions.transactionnumber as 'Transaction Number',
						totalpremium.totalamount as 'Total Premium',
						totalalpremium.totalamount as 'Auto Liability Premium',
						--totalcolprorated.totalamount as 'Collision Net',
						--totalpdpremium.totalamount as 'Physical Damage Premium', --TODO: Remove if needed since PD Premium is currently not part of the report
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
						totalhiredpdendstpremium.totalamount as 'Hired PD Premium',
						totaltrailerinterchangepremium.totalamount as 'Trailer Premium',
						totalpncpremium.totalamount as 'Primary and Non-Contributory Premium',
						totalshortratepremium.totalamount as 'Short Rate Premium',
						totalalmprem.totalamount as 'AL Manuscript Premium',
						totalpdmprem.totalamount as 'PD Manuscript Premium',
						totalminpremiumadjustment.totalamount as 'Minimum Premium Adjustment',
						totalalminpremiumadjustment.totalamount as 'Auto Liability Premium Adjustment',
						totalcargominpremiumadjustment.totalamount as 'Cargo Premium Adjustment',
						totalalmpminpremiumadjustment.totalamount as 'AL Manuscript Premium Adjustment',
						totalglminpremiumadjustment.totalamount as 'General Liability Premium Adjustment',
						totalpdminpremiumadjustment.totalamount as 'Physical Damage Premium Adjustment',
						totalpdmpminpremiumadjustment.totalamount as 'PD Manuscript Premium Adjustment',
						(coalesce(totalsf.totalamount, 0) + coalesce(totaltax.totalamount, 0) + coalesce(totalstampingfee.totalamount, 0)) as 'Total Tax/Fees',
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
							left join Risk r on ph.RiskId = r.Id
							left join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1 --TODO: To be confirmed if IsProcessed is to be removed from query so all policies are captured
							left join [Binding] b on b.RiskId = r.Id
							left join BrokerInfo bi on bi.RiskId = r.Id
							left join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
							left join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
							left join RiskForm rf on rf.RiskDetailId = rd.Id and rf.FormId = 'HiredPhysicalDamage'
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
				where rpt.PolicyNumber IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				and rpt.[Transaction Date] >= @fromDate
				and rpt.[Transaction Date] <= @toDate
				order by rpt.PolicyNumber, rpt.[Transaction Number]

			END";

        const string REVERT_BorderauxReport = @"
            CREATE OR ALTER   PROCEDURE[dbo].[GenerateBorderauxReport]
				@fromDate datetime = NULL,
				@toDate datetime = NULL
			AS
			BEGIN

                SET NOCOUNT ON;

				select
					rpt.PolicyNumber,
					rpt.[Endorsement Number] 'EndorsementNumber',
					rpt.[Transaction Number] 'TransactionNumber',
					rpt.[Insured Name] 'InsuredName',
					rpt.[Risk State] 'RiskState',
					rpt.[Transaction Type] 'TransactionType',
					rpt.[Transaction Date] 'TransactionDate',
					rpt.[Transaction Effective Date] 'TransactionEffectiveDate',
					rpt.EffectiveDate,
					rpt.ExpirationDate,
					CASE WHEN rpt.[Garaging Address] IS NULL THEN '' ELSE rpt.[Garaging Address] END as 'GaragingAddress',
					CASE WHEN rpt.[Garaging City] IS NULL THEN '' ELSE rpt.[Garaging City] END as 'GaragingCity',
					CASE WHEN rpt.[Garaging State] IS NULL THEN '' ELSE rpt.[Garaging State] END as 'GaragingState',
					CASE WHEN rpt.[Garaging ZipCode] IS NULL THEN '' ELSE rpt.[Garaging ZipCode] END as 'GaragingZipCode',
					CASE WHEN rpt.[Mailing Address] IS NULL THEN '' ELSE rpt.[Mailing Address] END as 'MailingAddress',
					CASE WHEN rpt.[Mailing City] IS NULL THEN '' ELSE rpt.[Mailing City] END as 'MailingCity',
					CASE WHEN rpt.[Mailing State] IS NULL THEN '' ELSE rpt.[Mailing State] END as 'MailingState',
					CASE WHEN rpt.[Mailing ZipCode] IS NULL THEN '' ELSE rpt.[Mailing ZipCode] END as 'MailingZipCode',
					CASE WHEN rpt.[Vehicle/Unit Count] IS NULL THEN 0 ELSE CONVERT(DECIMAL(18,2),rpt.[Vehicle/Unit Count]) END as 'VehicleUnitCount',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Premium]), 0) as 'TotalPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium]), 0) as 'AutoLiabilityPremium',
					CASE WHEN rpt.[Auto Liability Limit] IS NULL THEN '' ELSE rpt.[Auto Liability Limit] END as 'AutoLiabilityLimit',
					CASE WHEN rpt.[Auto Liability Deductible] IS NULL THEN '' ELSE rpt.[Auto Liability Deductible] END as 'AutoLiabilityDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium]), 0) as 'ALManuscriptPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium]), 0) as 'GeneralLiabilityPremium',
					CASE WHEN rpt.[PIP Limit] IS NULL THEN '' ELSE rpt.[PIP Limit] END as 'PIPLimit',
					CASE WHEN rpt.[Auto Medical Payment Limit] IS NULL THEN '' ELSE rpt.[Auto Medical Payment Limit] END as 'AutoMedicalPaymentLimit',
					rpt.Symbol,
					--CASE WHEN rpt.[Physical Damage Premium] IS NULL THEN 0 ELSE CONVERT(DECIMAL(10,2),rpt.[Physical Damage Premium] ) END as 'Physical Damage Premium', --TODO: Removed since PD Premium is not required in the report
					coalesce(CONVERT(DECIMAL(18,2),rpt.CollisionPremium), 0) as 'CollisionPremium',
					CASE WHEN rpt.[Collision Deductible] IS NULL THEN '' ELSE rpt.[Collision Deductible] END as 'CollisionDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Comp/FT Premium]), 0) as 'CompFTPremium',
					CASE WHEN rpt.[Comp/FT Deductible] IS NULL THEN '' ELSE rpt.[Comp/FT Deductible] END as 'CompFTDeductible',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium]), 0) as 'PDManuscriptPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Motor Truck Cargo Premium]), 0) as 'CargoPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Hired PD Premium]), 0) as 'HiredPDPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Trailer Premium]), 0) as 'TrailerPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Primary and Non-Contributory Premium]), 0) as 'PNCPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Short Rate Premium]), 0) as 'ShortRatePremium',
					CASE WHEN rpt.[Motor Truck Cargo Limit] IS NULL THEN '' ELSE rpt.[Motor Truck Cargo Limit] END as 'CargoLimit',
					CASE WHEN rpt.[Uninsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists BI Limit/Deductible] END as 'UMBILimit',
					CASE WHEN rpt.[Uninsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Uninsured Motorists PD Limit/Deductible] END as 'UMPDLimit',
					CASE WHEN rpt.[Underinsured Motorists BI Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists BI Limit/Deductible] END as 'UIMBILimit',
					CASE WHEN rpt.[Underinsured Motorists PD Limit/Deductible] IS NULL THEN '' ELSE rpt.[Underinsured Motorists PD Limit/Deductible] END as 'UIMPDLimit',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Additional Insured Amount]), 0) as 'AdditionalInsuredAmount',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Insured Value]), 0) as 'TotalInsuredValue',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Service Fee]), 0) as 'ServiceFee',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Stamping Fees]), 0) as 'StampingFee',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[SL Tax]), 0) as 'SLTax',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Waiver of Subrogation]), 0) as 'WaiverOfSubrogation',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Total Tax/Fees]), 0) as 'TotalTaxFees',
					--rpt.[Total days],
					--rpt.[Total days passed],
					--rpt.[Prem per day],
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Prem per day] * rpt.[Total days passed]), 0) as 'EarnedPremium',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Minimum Premium Adjustment]), 0) as 'MinimumPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Auto Liability Premium Adjustment]), 0) as 'AutoLiabilityPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Cargo Premium Adjustment]), 0) as 'CargoPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[AL Manuscript Premium Adjustment]), 0) as 'ALManuscriptPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[General Liability Premium Adjustment]), 0) as 'GeneralLiabilityPremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[Physical Damage Premium Adjustment]), 0) as 'PhysicalDamagePremiumAdjustment',
					coalesce(CONVERT(DECIMAL(18,2),rpt.[PD Manuscript Premium Adjustment]), 0) as 'PDManuscriptPremiumAdjustment',
					CASE WHEN rpt.[Broker Agency] IS NULL THEN '' ELSE rpt.[Broker Agency] END as 'BrokerAgency',
					CASE WHEN rpt.Broker_Billing_Address IS NULL THEN '' ELSE rpt.Broker_Billing_Address END as 'Broker_Billing_Address',
					CASE WHEN rpt.[Billing Address] IS NULL THEN '' ELSE rpt.[Billing Address] END as 'BillingAddress',
					CASE WHEN rpt.[Billing City] IS NULL THEN '' ELSE rpt.[Billing City] END as 'BillingCity',
					CASE WHEN rpt.[Billing StateCode] IS NULL THEN '' ELSE rpt.[Billing StateCode] END as 'BillingStateCode',
					CASE WHEN rpt.[Billing Zipcode] IS NULL THEN '' ELSE rpt.[Billing Zipcode] END as 'BillingZipcode',
					CASE WHEN rpt.Broker_Business_Address IS NULL THEN '' ELSE rpt.Broker_Business_Address END as 'Broker_Business_Address',
					CASE WHEN rpt.[Business Address] IS NULL THEN '' ELSE rpt.[Business Address] END as 'BusinessAddress',
					CASE WHEN rpt.[Business City] IS NULL THEN '' ELSE rpt.[Business City] END as 'BusinessCity',
					CASE WHEN rpt.[Business StateCode] IS NULL THEN '' ELSE rpt.[Business StateCode] END as 'BusinessStateCode',
					CASE WHEN rpt.[Business Zipcode] IS NULL THEN '' ELSE rpt.[Business Zipcode] END as 'BusinessZipcode',
					rpt.[Broker Commision AL %] as 'BrokerCommissionALPercent',
					rpt.[Broker Commision PD %] as 'BrokerCommissionPDPercent',
					coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) as 'BrokerCommissionAL',
					coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'BrokerCommissionPD',
					coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0) as 'TotalCommission',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN ('2.5%')
						ELSE ('0%') 
					END AS 'ARSCommissionPercent',
					CASE WHEN rpt.CreditedOfficeId = 3 
						THEN 
							CONVERT(decimal(18,2), ((coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionALAmount), 0) + coalesce(CONVERT(DECIMAL(18,2),rpt.CommisionPDAmount), 0)) * (2.5/100)))
						ELSE 0
					END AS 'ARSCommission',
					rpt.[Payment Plan] 'PaymentPlan',
					rpt.[Credited Office] 'CreditedOffice'
				from
					(
						select rpd.*,
						totaltransactions.transactionnumber as 'Transaction Number',
						totalpremium.totalamount as 'Total Premium',
						totalalpremium.totalamount as 'Auto Liability Premium',
						--totalcolprorated.totalamount as 'Collision Net',
						--totalpdpremium.totalamount as 'Physical Damage Premium', --TODO: Remove if needed since PD Premium is currently not part of the report
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
						totalhiredpdendstpremium.totalamount as 'Hired PD Premium',
						totaltrailerinterchangepremium.totalamount as 'Trailer Premium',
						totalpncpremium.totalamount as 'Primary and Non-Contributory Premium',
						totalshortratepremium.totalamount as 'Short Rate Premium',
						totalalmprem.totalamount as 'AL Manuscript Premium',
						totalpdmprem.totalamount as 'PD Manuscript Premium',
						totalminpremiumadjustment.totalamount as 'Minimum Premium Adjustment',
						totalalminpremiumadjustment.totalamount as 'Auto Liability Premium Adjustment',
						totalcargominpremiumadjustment.totalamount as 'Cargo Premium Adjustment',
						totalalmpminpremiumadjustment.totalamount as 'AL Manuscript Premium Adjustment',
						totalglminpremiumadjustment.totalamount as 'General Liability Premium Adjustment',
						totalpdminpremiumadjustment.totalamount as 'Physical Damage Premium Adjustment',
						totalpdmpminpremiumadjustment.totalamount as 'PD Manuscript Premium Adjustment',
						(coalesce(totalsf.totalamount, 0) + coalesce(totaltax.totalamount, 0) + coalesce(totalstampingfee.totalamount, 0)) as 'Total Tax/Fees',
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
				where rpt.PolicyNumber IS NOT NULL and rpt.[Insured Name] IS NOT NULL
				and rpt.[Transaction Date] >= @fromDate
				and rpt.[Transaction Date] <= @toDate
				order by rpt.PolicyNumber, rpt.[Transaction Number]

			END";
    }
}
