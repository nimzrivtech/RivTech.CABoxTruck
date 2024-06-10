using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Insert_Question_MaintenanceCargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(INSERT_QUESTION_DATA);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_QUESTION_DATA);
        }

        const string INSERT_QUESTION_DATA = @"
            delete dbo.LvQuestionSection;

            insert into dbo.LvQuestionSection ([Description], IsActive)
            values ('Maintenance Question', 1),
            ('General Liability', 1),
            ('Cargo', 1);

            declare @effectiveDate datetime = '2020-01-01',
	            @expirationDate datetime = '2030-01-01',
	            @currentDate datetime = getdate(),
	            --category id
	            @riskSpecificCatId smallint = (select s.Id from dbo.LvQuestionCategory s where s.[Description] = 'Risk specific'),
	            --section id
	            @maintenanceQuestionSecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'Maintenance Question'),
	            @genLiabilitySecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'General Liability'),
	            @cargoSecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'Cargo');

            insert into dbo.Question (EffectiveDate, ExpirationDate, Categoryid, SectionId, CreatedAt, LastUpdatedAt, [description])
            values(@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'safetyMeetings'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'personInCharge'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'garagingType'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'vehicleMaintenance'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'isMaintenanceProgramManagedByCompany'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'provideCompleteMaintenanceOnVehicles'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'areDriverFilesAvailableForReview'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'areAccidentFilesAvailableForReview'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @maintenanceQuestionSecId, @currentDate, @currentDate, 'companyPractice'),
            -- general liability--
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'contractuallyRequiredToCarryGeneralLiabilityInsurance'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'haveOperationsOtherThanTrucking'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'haveOperationsOtherThanTruckingExplain'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'operationsAtStorageLotOrImpoundYard'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'anyStorageOfGoods'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'anyWarehousing'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'anyStorageOfVehiclesForOthers'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'anyLeasingSpaceToOthers'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'anyFreightForwarding'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'anyStorageOfFuelsAndOrChemicals'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'hasApplicantHadGeneralLiabilityLoss'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'hasApplicantHadGeneralLiabilityLossExplain'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @genLiabilitySecId, @currentDate, @currentDate, 'glAnnualPayroll'),
            --cargo--
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'areRequiredToCarryCargoInsurance'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'areCommoditiesStoredInTruckOvernight'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'areCommoditiesStoredInTruckOvernightExplain'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'haulHazardousMaterials'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'haveRefrigeratedUnits'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'requireRefrigerationBreakdownCoverage'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'hadCargoLoss'),
            (@effectiveDate, @expirationDate, @riskSpecificCatId, @cargoSecId, @currentDate, @currentDate, 'hadCargoLossExplain');
            ";

        const string DELETE_QUESTION_DATA = @"
            declare @maintenanceQuestionSecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'Maintenance Question'),
	        @genLiabilitySecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'General Liability'),
	        @cargoSecId smallint = (select s.Id from dbo.LvQuestionSection s where s.[Description] = 'Cargo');

            delete q from dbo.Question q
            where q.SectionId in (@maintenanceQuestionSecId, @genLiabilitySecId, @cargoSecId);

            delete qs from dbo.LvQuestionSection qs
            where qs.[Description] in ('Maintenance Question', 'General Liability', 'Cargo');\
        ";
    }
}
