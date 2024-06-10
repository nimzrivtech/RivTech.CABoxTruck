using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_PremiumEarned_AddTransactionNumber_11092922 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_UpdateGeneratePremiumEarnedTaxReport);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(REVERT_UpdateGeneratePremiumEarnedTaxReport);
        }

        const string UPDATE_UpdateGeneratePremiumEarnedTaxReport = @"
            CREATE OR ALTER     PROCEDURE [dbo].[GeneratePremiumEarnedTaxReport] 
                @fromDate datetime = NULL,
                @toDate datetime = NULL
            AS
            BEGIN

                SET NOCOUNT ON;

                select 
                    --rpt.Id,
                    rpt.PolicyNumber,
                    rpt.[Transaction Type] 'TransactionType',
                    rpt.[Endorsement Number] 'EndorsementNumber',
	                rpt.[Transaction Number] 'TransactionNumber',
                    rpt.[Coverage Type] 'CoverageType',
                    rpt.[Insured Name] 'InsuredName',
                    rpt.[Risk State] 'RiskState',
                    coalesce(CONVERT(DECIMAL(18,2),rpt.[Premium of Transaction]), 0) as 'PremiumOfTransaction',
                    coalesce(CONVERT(DECIMAL(18,2),rpt.[Fees for Transaction]), 0) as 'FeesForTransaction',
                    coalesce(CONVERT(DECIMAL(18,2),rpt.[Premium of Transaction]), 0) as 'Taxable', -- TODO tax for TX
                    coalesce(CONVERT(DECIMAL(18,2),rpt.[Tax Surplus]), 0) as 'TaxSurplus',
                    coalesce(CONVERT(DECIMAL(18,2),rpt.[Stamping Fee]), 0) as 'StampingFee',
                    CONVERT(DECIMAL(18,2),coalesce(rpt.[Tax Surplus],0) + coalesce(rpt.[Stamping Fee],0)) as 'TotalTax',
                    coalesce(CONVERT(DECIMAL(18,2),rpt.[TIV (Stated Amount)]), 0) as 'TIVStatedAmount',
                    rpt.[Transaction Effective Date] 'TransactionEffectiveDate', 
                    rpt.[Policy Inception Date] 'PolicyInceptionDate',
                    rpt.[Policy Expiration Date] 'PolicyExpirationDate',
                    rpt.[Process Date] 'ProcessDate',
                    CASE WHEN rpt.[Business Description] IS NULL THEN '' ELSE rpt.[Business Description] END as 'BusinessDescription',
                    CASE WHEN rpt.[Insured Address] IS NULL THEN '' ELSE rpt.[Insured Address] END as 'InsuredAddress',
                    CASE WHEN rpt.[Insured City] IS NULL THEN '' ELSE rpt.[Insured City] END as 'InsuredCity',
                    CASE WHEN rpt.[Insured State] IS NULL THEN '' ELSE rpt.[Insured State] END as 'InsuredState',
                    CASE WHEN rpt.[Insured ZipCode] IS NULL THEN '' ELSE rpt.[Insured ZipCode] END as 'InsuredZipCode',
                    CASE WHEN rpt.[Insured County] IS NULL THEN '' ELSE rpt.[Insured County] END as 'InsuredCounty',
                    rpt.[Liability Claim Limit] 'LiabilityClaimLimit',
                    rpt.[Property Damage Limit] 'PropertyDamageLimit',
                    CASE WHEN rpt.[Broker Name] IS NULL THEN '' ELSE rpt.[Broker Name] END as 'BrokerName',
                    CASE WHEN rpt.Broker_Billing_Address IS NULL THEN '' ELSE rpt.Broker_Billing_Address END as 'Broker_Billing_Address',
                    CASE WHEN rpt.[Billing Address] IS NULL THEN '' ELSE rpt.[Billing Address] END as 'BillingAddress',
                    CASE WHEN rpt.[Billing City] IS NULL THEN '' ELSE rpt.[Billing City] END as 'BillingCity',
                    CASE WHEN rpt.[Billing StateCode] IS NULL THEN '' ELSE rpt.[Billing StateCode] END as 'BillingStateCode',
                    CASE WHEN rpt.[Billing Zipcode] IS NULL THEN '' ELSE rpt.[Billing Zipcode] END as 'BillingZipcode',
                    CASE WHEN rpt.Broker_Business_Address IS NULL THEN '' ELSE rpt.Broker_Business_Address END as 'Broker_Business_Address',
                    CASE WHEN rpt.[Business Address] IS NULL THEN '' ELSE rpt.[Business Address] END as 'BusinessAddress',
                    CASE WHEN rpt.[Business City] IS NULL THEN '' ELSE rpt.[Business City] END as 'BusinessCity',
                    CASE WHEN rpt.[Business StateCode] IS NULL THEN '' ELSE rpt.[Business StateCode] END as 'BusinessStateCode',
                    CASE WHEN rpt.[Business Zipcode] IS NULL THEN '' ELSE rpt.[Business Zipcode] END as 'BusinessZipcode'
                from
                (
                    select 
                        'Auto Liability' as 'Coverage Type',
		                totaltransactions.transactionnumber as 'Transaction Number',
                        totalpremium.totalamount as 'Premium of Transaction',
                        totalsf.totalamount as 'Fees for Transaction',
                        totaltax.totalamount as 'Tax Surplus',
                        totalstampingfee.totalamount as 'Stamping Fee',
                        rpd.*
                    from
                    (
                        select 
                            r.Id as 'RiskId',
                            ph.PreviousRiskDetailId as 'RiskDetailId',
                            r.PolicyNumber,
                            ie.CompanyName as 'Insured Name',
                            businessaddr.State as 'Risk State',
                            vehicletiv.totalamount as 'TIV (Stated Amount)',
                            ph.EffectiveDate as 'Transaction Effective Date',
                            bi.EffectiveDate as 'Policy Inception Date',
                            bi.ExpirationDate as 'Policy Expiration Date',
                            ph.DateCreated as 'Process Date',
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
				                WHEN ph.EndorsementNumber = 0 or 
					                    ph.PolicyStatus = 'Canceled' or 
						                ph.PolicyStatus = 'Reinstated' or 
						                rd.Status = 'Rewrite' or
						                (ph.PolicyStatus = 'Rewrite' and rd.Status = 'Canceled') --Canceled/Rewrite
				                THEN ''
				                ELSE
				                ph.EndorsementNumber
			                END as 'Endorsement Number',
                            ie.DescOperations as 'Business Description',
                            businessaddr.StreetAddress1 as 'Insured Address',
                            businessaddr.City as 'Insured City',
                            businessaddr.State as 'Insured State',
                            businessaddr.ZipCode as 'Insured ZipCode',
                            businessaddr.County as 'Insured County',
                            (select LimitDisplay from LvLimits where Id = rc.AutoBILimitId) as 'Liability Claim Limit',
                            (select LimitDisplay from LvLimits where Id = rc.AutoPDLimitId) as 'Property Damage Limit',
                            ae.CompanyName as 'Broker Name',
                            aa.StreetAddress1 as 'Broker_Billing_Address',
                            aa.StreetAddress2 as 'Billing Address',
                            aa.City as 'Billing City',
                            aa.StateCode as 'Billing StateCode',
                            aa.ZipCode as 'Billing Zipcode',
                            aa2.StreetAddress1 as 'Broker_Business_Address',
                            aa2.StreetAddress2 as 'Business Address',
                            aa2.City as 'Business City',
                            aa2.StateCode as 'Business StateCode',
                            aa2.ZipCode as 'Business Zipcode'
                            --trandet.AmountSubTypeId,
                            --trandet.totalamount
                        from PolicyHistory ph
                        left join Risk r on ph.RiskId = r.Id
                        left join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1
                        left join [Binding] b on b.RiskId = r.Id
                        left join BrokerInfo bi on bi.RiskId = r.Id
                        left join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
                        left join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
                        left join Insured i on rd.InsuredId = i.Id
                        left join Entity ie on i.EntityId = ie.Id
                        left join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
                        left join [Address] businessaddr on business.AddressId = businessaddr.Id
                        left join Agency_Agency agency on agency.Id = bi.AgencyId
                        left join Agency_Entity ae on ae.Id = agency.EntityId
                        left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
                        left join Agency_Address aa on aea.AddressId = aa.Id  
                        left join Agency_EntityAddress aea2 on ae.Id = aea2.EntityId and aea2.AddressTypeId = 2
                        left join Agency_Address aa2 on aea2.AddressId = aa2.Id
                        left join BusinessTypes bt on ie.BusinessTypeID = bt.Id
                        left join
                        (
                            select r.RiskDetailId, SUM(v.StatedAmount) as totalamount from RiskDetailVehicle r
                            join Vehicle v on r.VehicleId = v.Id
                            group by r.RiskDetailId
                        ) vehicletiv on vehicletiv.RiskDetailId = ph.PreviousRiskDetailId
                        --left join
                        --(
                        --	select btt.RiskId, btd.AmountSubTypeId, btt.CreatedDate, SUM(btd.Amount) as totalamount from Billing_Transaction btt
                        --	join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
                        --	group by btt.RiskId, btd.AmountSubTypeId, btt.CreatedDate
                        --) trandet on trandet.CreatedDate <= DATEADD(MINUTE, 1, ph.DateCreated) and trandet.CreatedDate >= DATEADD(MINUTE, -1, ph.DateCreated) and trandet.RiskId = r.Id
                    ) rpd
	                cross apply
		                (
			                select COUNT(*) as transactionnumber from PolicyHistory ph
			                where ph.RiskId = rpd.RiskId and ph.DateCreated <= rpd.[Process Date]
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
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totalpremium
                    cross apply
                    (
                        select SUM(btd.Amount) as totalamount from Billing_Transaction bt
                        join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
                        where (btd.AmountSubTypeId = 'ALSF' or btd.AmountSubTypeId = 'PDSF') and 
			                bt.RiskId = rpd.RiskId and
			                bt.TransactionTypeId = 
			                CASE	
				                WHEN rpd.[Transaction Type] = 'New' THEN 'N'
				                WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
				                WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
				                ELSE 'E'
			                END
			                and
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totalsf
                    cross apply
                    (
                        select SUM(btd.Amount) as totalamount from Billing_Transaction bt
                        join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
                        where (btd.AmountSubTypeId = 'SF') and bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and 
			                bt.RiskId = rpd.RiskId and
			                bt.TransactionTypeId = 
			                CASE	
				                WHEN rpd.[Transaction Type] = 'New' THEN 'N'
				                WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
				                WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
				                ELSE 'E'
			                END
			                and
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totalstampingfee
                    cross apply
                    (
                        select SUM(btd.Amount) as totalamount from Billing_Transaction bt
                        join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
                        where (btd.AmountSubTypeId = 'SLT') and bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and 
			                bt.RiskId = rpd.RiskId and
			                bt.TransactionTypeId = 
			                CASE	
				                WHEN rpd.[Transaction Type] = 'New' THEN 'N'
				                WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
				                WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
				                ELSE 'E'
			                END
			                and
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totaltax
                ) rpt
                where rpt.PolicyNumber IS NOT NULL and rpt.[Insured Name] IS NOT NULL -- and rpt.AmountSubTypeId IS NOT NULL
                and rpt.[Process Date] >= @fromDate
                and rpt.[Process Date] <= @toDate
                order by rpt.PolicyNumber, rpt.[Endorsement Number], rpt.[Coverage Type]
            END
        ";


        const string REVERT_UpdateGeneratePremiumEarnedTaxReport = @"
            CREATE OR ALTER     PROCEDURE [dbo].[GeneratePremiumEarnedTaxReport] 
                @fromDate datetime = NULL,
                @toDate datetime = NULL
            AS
            BEGIN

                SET NOCOUNT ON;

                select 
                    --rpt.Id,
                    rpt.PolicyNumber,
                    rpt.[Transaction Type] 'TransactionType',
                    rpt.[Endorsement Number] 'EndorsementNumber',
                    rpt.[Coverage Type] 'CoverageType',
                    rpt.[Insured Name] 'InsuredName',
                    rpt.[Risk State] 'RiskState',
                    coalesce(CONVERT(DECIMAL(10,2),rpt.[Premium of Transaction]), 0) as 'PremiumOfTransaction',
                    coalesce(CONVERT(DECIMAL(10,2),rpt.[Fees for Transaction]), 0) as 'FeesForTransaction',
                    coalesce(CONVERT(DECIMAL(10,2),rpt.[Premium of Transaction]), 0) as 'Taxable', -- TODO tax for TX
                    coalesce(CONVERT(DECIMAL(10,2),rpt.[Tax Surplus]), 0) as 'TaxSurplus',
                    coalesce(CONVERT(DECIMAL(10,2),rpt.[Stamping Fee]), 0) as 'StampingFee',
                    CONVERT(DECIMAL(10,2),coalesce(rpt.[Tax Surplus],0) + coalesce(rpt.[Stamping Fee],0)) as 'TotalTax',
                    coalesce(CONVERT(DECIMAL(10,2),rpt.[TIV (Stated Amount)]), 0) as 'TIVStatedAmount',
                    rpt.[Transaction Effective Date] 'TransactionEffectiveDate', 
                    rpt.[Policy Inception Date] 'PolicyInceptionDate',
                    rpt.[Policy Expiration Date] 'PolicyExpirationDate',
                    rpt.[Process Date] 'ProcessDate',
                    CASE WHEN rpt.[Business Description] IS NULL THEN '' ELSE rpt.[Business Description] END as 'BusinessDescription',
                    CASE WHEN rpt.[Insured Address] IS NULL THEN '' ELSE rpt.[Insured Address] END as 'InsuredAddress',
                    CASE WHEN rpt.[Insured City] IS NULL THEN '' ELSE rpt.[Insured City] END as 'InsuredCity',
                    CASE WHEN rpt.[Insured State] IS NULL THEN '' ELSE rpt.[Insured State] END as 'InsuredState',
                    CASE WHEN rpt.[Insured ZipCode] IS NULL THEN '' ELSE rpt.[Insured ZipCode] END as 'InsuredZipCode',
                    CASE WHEN rpt.[Insured County] IS NULL THEN '' ELSE rpt.[Insured County] END as 'InsuredCounty',
                    rpt.[Liability Claim Limit] 'LiabilityClaimLimit',
                    rpt.[Property Damage Limit] 'PropertyDamageLimit',
                    rpt.[Broker Name] 'BrokerName',
                    CASE WHEN rpt.Broker_Billing_Address IS NULL THEN '' ELSE rpt.Broker_Billing_Address END as 'Broker_Billing_Address',
                    CASE WHEN rpt.[Billing Address] IS NULL THEN '' ELSE rpt.[Billing Address] END as 'BillingAddress',
                    CASE WHEN rpt.[Billing City] IS NULL THEN '' ELSE rpt.[Billing City] END as 'BillingCity',
                    CASE WHEN rpt.[Billing StateCode] IS NULL THEN '' ELSE rpt.[Billing StateCode] END as 'BillingStateCode',
                    CASE WHEN rpt.[Billing Zipcode] IS NULL THEN '' ELSE rpt.[Billing Zipcode] END as 'BillingZipcode',
                    CASE WHEN rpt.Broker_Business_Address IS NULL THEN '' ELSE rpt.Broker_Business_Address END as 'Broker_Business_Address',
                    CASE WHEN rpt.[Business Address] IS NULL THEN '' ELSE rpt.[Business Address] END as 'BusinessAddress',
                    CASE WHEN rpt.[Business City] IS NULL THEN '' ELSE rpt.[Business City] END as 'BusinessCity',
                    CASE WHEN rpt.[Business StateCode] IS NULL THEN '' ELSE rpt.[Business StateCode] END as 'BusinessStateCode',
                    CASE WHEN rpt.[Business Zipcode] IS NULL THEN '' ELSE rpt.[Business Zipcode] END as 'BusinessZipcode'
                from
                (
                    select 
                        'Auto Liability' as 'Coverage Type',
                        totalpremium.totalamount as 'Premium of Transaction',
                        totalsf.totalamount as 'Fees for Transaction',
                        totaltax.totalamount as 'Tax Surplus',
                        totalstampingfee.totalamount as 'Stamping Fee',
                        rpd.*
                    from
                    (
                        select 
                            r.Id as 'RiskId',
                            ph.PreviousRiskDetailId as 'RiskDetailId',
                            r.PolicyNumber,
                            ie.CompanyName as 'Insured Name',
                            businessaddr.State as 'Risk State',
                            vehicletiv.totalamount as 'TIV (Stated Amount)',
                            ph.EffectiveDate as 'Transaction Effective Date',
                            bi.EffectiveDate as 'Policy Inception Date',
                            bi.ExpirationDate as 'Policy Expiration Date',
                            ph.DateCreated as 'Process Date',
                            CASE
                                WHEN ph.EndorsementNumber = 0
                                THEN 'New'
                                WHEN ph.PolicyStatus = 'Reinstated'
                                THEN 'Reinstatement'
                                WHEN ph.PolicyStatus = 'Canceled'
                                THEN 'Cancellation'
                                WHEN ph.EndorsementNumber != 0 and rd.Status = 'Rewrite'
                                THEN 'Renew'
                                ELSE 'Endorsement'
                            END as 'Transaction Type',
                            ph.EndorsementNumber as 'Endorsement Number',
                            ie.DescOperations as 'Business Description',
                            businessaddr.StreetAddress1 as 'Insured Address',
                            businessaddr.City as 'Insured City',
                            businessaddr.State as 'Insured State',
                            businessaddr.ZipCode as 'Insured ZipCode',
                            businessaddr.County as 'Insured County',
                            (select LimitDisplay from LvLimits where Id = rc.AutoBILimitId) as 'Liability Claim Limit',
                            (select LimitDisplay from LvLimits where Id = rc.AutoPDLimitId) as 'Property Damage Limit',
                            ae.CompanyName as 'Broker Name',
                            aa.StreetAddress1 as 'Broker_Billing_Address',
                            aa.StreetAddress2 as 'Billing Address',
                            aa.City as 'Billing City',
                            aa.StateCode as 'Billing StateCode',
                            aa.ZipCode as 'Billing Zipcode',
                            aa2.StreetAddress1 as 'Broker_Business_Address',
                            aa2.StreetAddress2 as 'Business Address',
                            aa2.City as 'Business City',
                            aa2.StateCode as 'Business StateCode',
                            aa2.ZipCode as 'Business Zipcode'
                            --trandet.AmountSubTypeId,
                            --trandet.totalamount
                        from PolicyHistory ph
                        left join Risk r on ph.RiskId = r.Id
                        left join RiskDetail rd on rd.Id = ph.PreviousRiskDetailId and rd.IsProcessed = 1
                        left join [Binding] b on b.RiskId = r.Id
                        left join BrokerInfo bi on bi.RiskId = r.Id
                        left join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
                        left join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
                        left join Insured i on rd.InsuredId = i.Id
                        left join Entity ie on i.EntityId = ie.Id
                        left join EntityAddress business on business.EntityId = ie.Id and business.AddressTypeId = 2
                        left join [Address] businessaddr on business.AddressId = businessaddr.Id
                        left join Agency_Agency agency on agency.Id = bi.AgencyId
                        left join Agency_Entity ae on ae.Id = agency.EntityId
                        left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
                        left join Agency_Address aa on aea.AddressId = aa.Id  
                        left join Agency_EntityAddress aea2 on ae.Id = aea2.EntityId and aea2.AddressTypeId = 2
                        left join Agency_Address aa2 on aea2.AddressId = aa2.Id
                        left join BusinessTypes bt on ie.BusinessTypeID = bt.Id
                        left join
                        (
                            select r.RiskDetailId, SUM(v.StatedAmount) as totalamount from RiskDetailVehicle r
                            join Vehicle v on r.VehicleId = v.Id
                            group by r.RiskDetailId
                        ) vehicletiv on vehicletiv.RiskDetailId = ph.PreviousRiskDetailId
                        --left join
                        --(
                        --	select btt.RiskId, btd.AmountSubTypeId, btt.CreatedDate, SUM(btd.Amount) as totalamount from Billing_Transaction btt
                        --	join Billing_TransactionDetail btd on btt.Id = btd.TransactionId
                        --	group by btt.RiskId, btd.AmountSubTypeId, btt.CreatedDate
                        --) trandet on trandet.CreatedDate <= DATEADD(MINUTE, 1, ph.DateCreated) and trandet.CreatedDate >= DATEADD(MINUTE, -1, ph.DateCreated) and trandet.RiskId = r.Id
                    ) rpd
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
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totalpremium
                    cross apply
                    (
                        select SUM(btd.Amount) as totalamount from Billing_Transaction bt
                        join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
                        where (btd.AmountSubTypeId = 'ALSF' or btd.AmountSubTypeId = 'PDSF') and 
			                bt.RiskId = rpd.RiskId and
			                bt.TransactionTypeId = 
			                CASE	
				                WHEN rpd.[Transaction Type] = 'New' THEN 'N'
				                WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
				                WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
				                ELSE 'E'
			                END
			                and
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totalsf
                    cross apply
                    (
                        select SUM(btd.Amount) as totalamount from Billing_Transaction bt
                        join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
                        where (btd.AmountSubTypeId = 'SF') and bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and 
			                bt.RiskId = rpd.RiskId and
			                bt.TransactionTypeId = 
			                CASE	
				                WHEN rpd.[Transaction Type] = 'New' THEN 'N'
				                WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
				                WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
				                ELSE 'E'
			                END
			                and
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totalstampingfee
                    cross apply
                    (
                        select SUM(btd.Amount) as totalamount from Billing_Transaction bt
                        join Billing_TransactionDetail btd on bt.Id = btd.TransactionId
                        where (btd.AmountSubTypeId = 'SLT') and bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and 
			                bt.RiskId = rpd.RiskId and
			                bt.TransactionTypeId = 
			                CASE	
				                WHEN rpd.[Transaction Type] = 'New' THEN 'N'
				                WHEN rpd.[Transaction Type] = 'Cancellation' THEN 'C'
				                WHEN rpd.[Transaction Type] = 'Reinstatement' THEN 'R'
				                ELSE 'E'
			                END
			                and
			                bt.CreatedDate <= DATEADD(MINUTE, 1, rpd.[Process Date]) and bt.CreatedDate >= DATEADD(MINUTE, -1, rpd.[Process Date])
                    ) totaltax
                ) rpt
                where rpt.PolicyNumber IS NOT NULL and rpt.[Insured Name] IS NOT NULL -- and rpt.AmountSubTypeId IS NOT NULL
                and rpt.[Process Date] >= @fromDate
                and rpt.[Process Date] <= @toDate
                order by rpt.PolicyNumber, rpt.[Endorsement Number], rpt.[Coverage Type]
            END
        ";

    }
}
