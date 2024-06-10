using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddGenerateSubmissionQuoteReportSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CREATE_SubmissionQuoteReport);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_SubmissionQuoteReport);
        }

        const string CREATE_SubmissionQuoteReport = @"
            CREATE OR ALTER   PROCEDURE[dbo].[GenerateSubmissionQuoteReport]
				@fromDate datetime = NULL,
				@toDate datetime = NULL
			AS
			BEGIN

				SET NOCOUNT ON;

				select 
					rpt.Program,
					COALESCE(rpt.Insured, '') as 'Insured',
					rpt.SubmissionNumber as 'SubmissionNumber',
					COALESCE(rpt.[Broker], '') as 'Broker',
					COALESCE(rpt.[Broker Contact], '') as 'BrokerContact',
					--rpt.[Submission Type] as 'Submission Type', --TODO: Applicable only when Renewal is implemented
					COALESCE(rpt.[Submission Status], '') as 'SubmissionStatus',
					--rpt.[Expiring Policy] as 'Expiring Policy', --TODO: Applicable only when Renewal is implemented
					COALESCE(rpt.[Date Submitted], '') as 'DateSubmitted',
					COALESCE(rpt.[Inception Date], '') as 'InceptionDate',
					--rpt.[Inception TBD] as 'Inception TBD', --TODO: Currently has no mapping in the system
					COALESCE(rpt.[AU Representative],'') as 'AURepresentative',
					COALESCE(rpt.[AU PSR],'') as 'AUPSR',
					COALESCE(rpt.[Last Note Added],'') as 'LastNoteAdded',
					COALESCE(rpt.[Auto Liability Limit],'') as 'AutoLiabilityLimit',
					COALESCE(rpt.[Auto Liability Deductible],'') as 'AutoLiabilityDeductible',
					COALESCE(rpt.[PIP Limit],'') as 'PIPLimit',
					COALESCE(rpt.[Auto Medical Payment Limit],'') as 'AutoMedicalPaymentLimit',
					COALESCE(rpt.[Collision Deductible],'') as 'CollisionDeductible',
					COALESCE(rpt.[Comp/FT Deductible],'') as 'CompFTDeductible',
					COALESCE(rpt.[Motor Truck Cargo Limit],'') as 'MotorTruckCargoLimit',
					COALESCE(rpt.[Uninsured Motorists BI Limit/Deductible],'') as 'UMBILimit',
					COALESCE(rpt.[Uninsured Motorists PD Limit/Deductible],'') as 'UMPDLimit',
					COALESCE(rpt.[Underinsured Motorists BI Limit/Deductible],'') as 'UIMBILimit',
					COALESCE(rpt.[Underinsured Motorists PD Limit/Deductible],'') as 'UIMPDLimit',
					COALESCE(rpt.Underwriter,'') as 'Underwriter',
					COALESCE(rpt.[# of Units], 0) as 'VehicleUnits',
					--rpt.LossRatio as 'LossRatio', --TODO: Currently has no mapping in the system
					COALESCE(rpt.[Quoted Premium], 0) as 'QuotedPremium',
					--rpt.[Quoted Paper] as 'Quoted Paper', --TODO: Currently has no mapping in the system
					COALESCE(rpt.[Broker City],'') as 'BrokerCity',
					COALESCE(rpt.[Broker State],'') as 'BrokerState',
					COALESCE(rpt.[Broker Contact Phone],'') as 'BrokerContactPhone',
					COALESCE(rpt.[Broker Contact Email],'') as 'BrokerContactEmail'
				from
				(
					select
						'RC BoxTruck' as 'Program',
						rpd.SubmissionNumber,
						rpd.Insured,
						rpd.[Broker],
						rpd.[Broker Contact],
						'' as 'Submission Type',
						rpd.[Submission Status],
						'' as 'Expiring Policy',
						rpd.[Date Submitted],
						rpd.[Inception Date],
						'' as 'Inception TBD',
						rpd.[AU Representative],
						rpd.[AU PSR],
						CONVERT(varchar(10), rpd.[Last Note Added], 101) as 'Last Note Added',
						rpd.[Auto Liability Limit],
						rpd.[Auto Liability Deductible],
						rpd.[PIP Limit],
						rpd.[Auto Medical Payment Limit],
						rpd.[Collision Deductible],
						rpd.[Comp/FT Deductible],
						rpd.[Motor Truck Cargo Limit],
						rpd.[Uninsured Motorists BI Limit/Deductible],
						rpd.[Uninsured Motorists PD Limit/Deductible],
						rpd.[Underinsured Motorists BI Limit/Deductible],
						rpd.[Underinsured Motorists PD Limit/Deductible],
						rpd.Underwriter,
						rpd.[# of Units],
						'' as 'LossRatio',
						rpd.[Quoted Premium],
						'' as 'Quoted Paper',
						rpd.[Broker City],
						rpd.[Broker State],
						rpd.[Broker Contact Phone],
						rpd.[Broker Contact Email]
					from
					(
						select 
							r.SubmissionNumber,
							r.Id as 'RiskId',
							ie.CompanyName as 'Insured',
							ae.CompanyName as 'Broker',
							ae2.FullName as 'Broker Contact',
							rd.[Status] as 'Submission Status',
							rd.CreatedDate as 'Date Submitted',
							bi.EffectiveDate as 'Inception Date',
							(select [Name] from PolicyContacts where Id = bi.TeamLeadUserId) as 'AU Representative',
							(select [Name] from PolicyContacts where Id = bi.PsrUserId) as 'AU PSR',
							(select top 1 CreatedDate from RiskNotes where RiskDetailId = rd.Id order by CreatedDate desc) as 'Last Note Added',
							(select LimitDisplay from LvLimits where Id = rc.AutoBILimitId) as 'Auto Liability Limit',
							(select LimitDisplay from LvLimits where Id = rc.LiabilityDeductibleId) as 'Auto Liability Deductible',
							(select LimitDisplay from LvLimits where Id = rc.PIPLimitId) as 'PIP Limit',
							(select LimitDisplay from LvLimits where Id = rc.MedPayLimitId) as 'Auto Medical Payment Limit',
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
							(select [Name] from PolicyContacts where Id = bi.ProductUWUserId) as 'Underwriter',
							coalesce(rcp.Premium, 0) as 'Quoted Premium',
							all_vehicle.vcount as '# of Units',
							aa.City as 'Broker City',
							aa.StateCode as 'Broker State',
							ae.WorkPhone as 'Broker Contact Phone',
							ae.WorkEmailAddress as 'Broker Contact Email'
						from Risk r
						left join BrokerInfo bi on bi.RiskId = r.Id
						left join RiskDetail rd on r.SubmissionNumber = rd.SubmissionNumber
						left join [Binding] b on b.RiskId = r.Id
						left join RiskCoverage rc on rd.Id = rc.RiskDetailId and b.BindOptionId = rc.OptionNumber
						left join RiskCoveragePremium rcp on rc.Id = rcp.RiskCoverageId
						left join Insured i on rd.InsuredId = i.Id
						left join Entity ie on i.EntityId = ie.Id
						left join Agency_Agency agency on agency.Id = bi.AgencyId
						left join Agency_Entity ae on ae.Id = agency.EntityId
						left join Agency_EntityAddress aea on ae.Id = aea.EntityId and aea.AddressTypeId = 1 and aea.RemovedProcessDate is null
						left join Agency_Address aa on aea.AddressId = aa.Id
						left join (
							select rdv.RiskDetailId, COUNT(rdv.RiskDetailId) as vcount from RiskDetailVehicle rdv
							group by rdv.RiskDetailId
						) all_vehicle on all_vehicle.RiskDetailId = rd.Id
						outer apply (
							select top 1 FullName, WorkEmailAddress from Agency_Entity 
							where CompanyName IS NULL
							and WorkEmailAddress = ae.WorkEmailAddress
						) ae2
						where r.PolicyNumber IS NULL
						and rd.CreatedDate >= @fromDate
						and rd.CreatedDate <= @toDate
					) rpd
				) rpt
				order by rpt.[Date Submitted]

			END";
        const string DELETE_SubmissionQuoteReport = @"DROP PROCEDURE GenerateSubmissionQuoteReport";
    }
}
