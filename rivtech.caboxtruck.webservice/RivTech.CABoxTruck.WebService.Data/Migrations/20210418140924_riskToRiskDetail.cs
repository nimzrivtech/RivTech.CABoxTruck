using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class riskToRiskDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_Risk_RiskId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimsHistory_Risk_RiskId",
                table: "ClaimsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_Risk_RiskId",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverHeader_Risk_RiskId",
                table: "DriverHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_Risk_Insured_InsuredId",
                table: "Risk");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAdditionalInterest_Risk_RiskId",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskCoverage_Risk_RiskId",
                table: "RiskCoverage");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskFiling_Risk_RiskId",
                table: "RiskFiling");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskForm_Risk_RiskId",
                table: "RiskForm");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskHistory_Risk_RiskId",
                table: "RiskHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskNotes_Risk_RiskId",
                table: "RiskNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskResponse_Risk_RiskId",
                table: "RiskResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificCommoditiesHauled_Risk_RiskId",
                table: "RiskSpecificCommoditiesHauled");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDotInfo_Risk_Id",
                table: "RiskSpecificDotInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverHiringCriteria_Risk_Id",
                table: "RiskSpecificDriverHiringCriteria");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverInfo_Risk_Id",
                table: "RiskSpecificDriverInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificRadiusOfOperation_Risk_Id",
                table: "RiskSpecificRadiusOfOperation");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificUnderwritingQuestion_Risk_Id",
                table: "RiskSpecificUnderwritingQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskStatusHistory_Risk_RiskId",
                table: "RiskStatusHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Risk_RiskId",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_RiskId",
                table: "BrokerInfo");

            migrationBuilder.DropColumn(
                name: "AccountCategoryId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "AccountCategoryUWId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "AgentCommission",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "AverageMilesPerVehicleId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "CurrentOperatingAuthorityStates",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DOTNumberLastFiveYearsDetail",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DaysOfService",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DriverMaxHoursDayDriving",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DriverMaxHoursDayOnDuty",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DriverMaxHoursWeekDriving",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "DriverMaxHoursWeekOnDuty",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "FirstIssueDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "FirstYearUnderCurrentManagement",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "HasOperationsOver300MileRadius",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "HasOwnedCompanies",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "HoursOfService",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsARatedCarrierRequired",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsDifferentUSDOTNumberLastFiveYears",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsFilingRequired",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsNewBusiness",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsNewVenture",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsOperatorsWorkOnlyForInsured",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsOthersAllowedToOperateCurrent",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsOthersAllowedToOperatePast",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "IsUWQuestionResponseConfirmed",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "MainUseId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "NewVenturePreviousExperienceId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "NumberOfDrivers",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PastCompanyDetails",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PastOperatingAuthorityStates",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PercentOwnerOperatorVehicles",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PolicySuffix",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PreviousExperienceCompany",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "PrimaryCityZipCodeOfOperationsId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ProgramID",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RadiusOfOperationsPercent1",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RadiusOfOperationsPercent2",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RadiusOfOperationsPercent3",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RenewalCount",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RequestedQuoteDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "RiskStateId",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "ShiftsPerDay",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "YearsInBusiness",
                table: "Risk");

            migrationBuilder.DropColumn(
                name: "YearsUnderCurrentManagement",
                table: "Risk");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "VehiclePremiumRatingFactor",
                newName: "RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "Vehicle",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_RiskId",
                table: "Vehicle",
                newName: "IX_Vehicle_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskStatusHistory",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskStatusHistory_RiskId",
                table: "RiskStatusHistory",
                newName: "IX_RiskStatusHistory_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskSpecificSafetyDevice",
                newName: "RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskSpecificDestination",
                newName: "RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskSpecificCommoditiesHauled",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskSpecificCommoditiesHauled_RiskId",
                table: "RiskSpecificCommoditiesHauled",
                newName: "IX_RiskSpecificCommoditiesHauled_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskResponse",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskResponse_RiskId",
                table: "RiskResponse",
                newName: "IX_RiskResponse_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskNotes",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskNotes_RiskId",
                table: "RiskNotes",
                newName: "IX_RiskNotes_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskHistory",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskHistory_RiskId",
                table: "RiskHistory",
                newName: "IX_RiskHistory_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskForm",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskForm_RiskId",
                table: "RiskForm",
                newName: "IX_RiskForm_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskFiling",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskFiling_RiskId",
                table: "RiskFiling",
                newName: "IX_RiskFiling_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskCoverage",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskCoverage_RiskId",
                table: "RiskCoverage",
                newName: "IX_RiskCoverage_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "RiskAdditionalInterest",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskAdditionalInterest_RiskId",
                table: "RiskAdditionalInterest",
                newName: "IX_RiskAdditionalInterest_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "InsuredId",
                table: "Risk",
                newName: "CurrentRiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Risk_InsuredId",
                table: "Risk",
                newName: "IX_Risk_CurrentRiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "DriverPremiumRatingFactor",
                newName: "RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "DriverHeader",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_DriverHeader_RiskId",
                table: "DriverHeader",
                newName: "IX_DriverHeader_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "Driver",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Driver_RiskId",
                table: "Driver",
                newName: "IX_Driver_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "ClaimsHistory",
                newName: "RiskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimsHistory_RiskId",
                table: "ClaimsHistory",
                newName: "IX_ClaimsHistory_RiskDetailId");

            migrationBuilder.RenameColumn(
                name: "RiskId",
                table: "BrokerInfo",
                newName: "RiskDetailId");

            migrationBuilder.CreateTable(
                name: "RiskDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    QuoteNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PolicyNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PolicySuffix = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    FirstIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedQuoteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ProgramID = table.Column<short>(type: "smallint", nullable: true),
                    InsuredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstYearUnderCurrentManagement = table.Column<short>(type: "smallint", nullable: true),
                    AccountCategoryId = table.Column<short>(type: "smallint", nullable: true),
                    AccountCategoryUWId = table.Column<short>(type: "smallint", nullable: true),
                    MainUseId = table.Column<short>(type: "smallint", nullable: true),
                    IsNewVenture = table.Column<bool>(type: "bit", nullable: true),
                    NewVenturePreviousExperienceId = table.Column<short>(type: "smallint", nullable: true),
                    PreviousExperienceCompany = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PrimaryCityZipCodeOfOperationsId = table.Column<int>(type: "int", nullable: true),
                    HasOwnedCompanies = table.Column<bool>(type: "bit", nullable: true),
                    PastCompanyDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskStateId = table.Column<short>(type: "smallint", nullable: true),
                    AgentCommission = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    IsARatedCarrierRequired = table.Column<bool>(type: "bit", nullable: true),
                    IsFilingRequired = table.Column<bool>(type: "bit", nullable: true),
                    IsDifferentUSDOTNumberLastFiveYears = table.Column<bool>(type: "bit", nullable: true),
                    DOTNumberLastFiveYearsDetail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    IsOthersAllowedToOperateCurrent = table.Column<bool>(type: "bit", nullable: true),
                    IsOthersAllowedToOperatePast = table.Column<bool>(type: "bit", nullable: true),
                    IsOperatorsWorkOnlyForInsured = table.Column<bool>(type: "bit", nullable: true),
                    PastOperatingAuthorityStates = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CurrentOperatingAuthorityStates = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    IsNewBusiness = table.Column<bool>(type: "bit", nullable: true),
                    RenewalCount = table.Column<short>(type: "smallint", nullable: true),
                    YearsInBusiness = table.Column<short>(type: "smallint", nullable: true),
                    YearsUnderCurrentManagement = table.Column<short>(type: "smallint", nullable: true),
                    AverageMilesPerVehicleId = table.Column<short>(type: "smallint", nullable: true),
                    DriverMaxHoursDayDriving = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    DriverMaxHoursDayOnDuty = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    DriverMaxHoursWeekDriving = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    DriverMaxHoursWeekOnDuty = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    HoursOfService = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    DaysOfService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShiftsPerDay = table.Column<int>(type: "int", nullable: true),
                    RadiusOfOperationsPercent1 = table.Column<int>(type: "int", nullable: true),
                    RadiusOfOperationsPercent2 = table.Column<int>(type: "int", nullable: true),
                    RadiusOfOperationsPercent3 = table.Column<int>(type: "int", nullable: true),
                    HasOperationsOver300MileRadius = table.Column<bool>(type: "bit", nullable: true),
                    PercentOwnerOperatorVehicles = table.Column<int>(type: "int", nullable: true),
                    NumberOfDrivers = table.Column<int>(type: "int", nullable: true),
                    IsUWQuestionResponseConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    AssignedToId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskDetail_Insured_InsuredId",
                        column: x => x.InsuredId,
                        principalTable: "Insured",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiskDetail_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_RiskDetailId",
                table: "BrokerInfo",
                column: "RiskDetailId",
                unique: true,
                filter: "[RiskDetailId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RiskDetail_InsuredId",
                table: "RiskDetail",
                column: "InsuredId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskDetail_RiskId",
                table: "RiskDetail",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_RiskDetail_RiskDetailId",
                table: "BrokerInfo",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimsHistory_RiskDetail_RiskDetailId",
                table: "ClaimsHistory",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_RiskDetail_RiskDetailId",
                table: "Driver",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverHeader_RiskDetail_RiskDetailId",
                table: "DriverHeader",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Risk_RiskDetail_CurrentRiskDetailId",
                table: "Risk",
                column: "CurrentRiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAdditionalInterest_RiskDetail_RiskDetailId",
                table: "RiskAdditionalInterest",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskCoverage_RiskDetail_RiskDetailId",
                table: "RiskCoverage",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskFiling_RiskDetail_RiskDetailId",
                table: "RiskFiling",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskForm_RiskDetail_RiskDetailId",
                table: "RiskForm",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskHistory_RiskDetail_RiskDetailId",
                table: "RiskHistory",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskNotes_RiskDetail_RiskDetailId",
                table: "RiskNotes",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskResponse_RiskDetail_RiskDetailId",
                table: "RiskResponse",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificCommoditiesHauled_RiskDetail_RiskDetailId",
                table: "RiskSpecificCommoditiesHauled",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDotInfo_RiskDetail_Id",
                table: "RiskSpecificDotInfo",
                column: "Id",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverHiringCriteria_RiskDetail_Id",
                table: "RiskSpecificDriverHiringCriteria",
                column: "Id",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverInfo_RiskDetail_Id",
                table: "RiskSpecificDriverInfo",
                column: "Id",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificRadiusOfOperation_RiskDetail_Id",
                table: "RiskSpecificRadiusOfOperation",
                column: "Id",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificUnderwritingQuestion_RiskDetail_Id",
                table: "RiskSpecificUnderwritingQuestion",
                column: "Id",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskStatusHistory_RiskDetail_RiskDetailId",
                table: "RiskStatusHistory",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_RiskDetail_RiskDetailId",
                table: "Vehicle",
                column: "RiskDetailId",
                principalTable: "RiskDetail",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerInfo_RiskDetail_RiskDetailId",
                table: "BrokerInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimsHistory_RiskDetail_RiskDetailId",
                table: "ClaimsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_RiskDetail_RiskDetailId",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverHeader_RiskDetail_RiskDetailId",
                table: "DriverHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_Risk_RiskDetail_CurrentRiskDetailId",
                table: "Risk");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAdditionalInterest_RiskDetail_RiskDetailId",
                table: "RiskAdditionalInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskCoverage_RiskDetail_RiskDetailId",
                table: "RiskCoverage");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskFiling_RiskDetail_RiskDetailId",
                table: "RiskFiling");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskForm_RiskDetail_RiskDetailId",
                table: "RiskForm");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskHistory_RiskDetail_RiskDetailId",
                table: "RiskHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskNotes_RiskDetail_RiskDetailId",
                table: "RiskNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskResponse_RiskDetail_RiskDetailId",
                table: "RiskResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificCommoditiesHauled_RiskDetail_RiskDetailId",
                table: "RiskSpecificCommoditiesHauled");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDotInfo_RiskDetail_Id",
                table: "RiskSpecificDotInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverHiringCriteria_RiskDetail_Id",
                table: "RiskSpecificDriverHiringCriteria");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificDriverInfo_RiskDetail_Id",
                table: "RiskSpecificDriverInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificRadiusOfOperation_RiskDetail_Id",
                table: "RiskSpecificRadiusOfOperation");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskSpecificUnderwritingQuestion_RiskDetail_Id",
                table: "RiskSpecificUnderwritingQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskStatusHistory_RiskDetail_RiskDetailId",
                table: "RiskStatusHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_RiskDetail_RiskDetailId",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "RiskDetail");

            migrationBuilder.DropIndex(
                name: "IX_BrokerInfo_RiskDetailId",
                table: "BrokerInfo");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "VehiclePremiumRatingFactor",
                newName: "RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "Vehicle",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_RiskDetailId",
                table: "Vehicle",
                newName: "IX_Vehicle_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskStatusHistory",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskStatusHistory_RiskDetailId",
                table: "RiskStatusHistory",
                newName: "IX_RiskStatusHistory_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskSpecificSafetyDevice",
                newName: "RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskSpecificDestination",
                newName: "RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskSpecificCommoditiesHauled",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskSpecificCommoditiesHauled_RiskDetailId",
                table: "RiskSpecificCommoditiesHauled",
                newName: "IX_RiskSpecificCommoditiesHauled_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskResponse",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskResponse_RiskDetailId",
                table: "RiskResponse",
                newName: "IX_RiskResponse_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskNotes",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskNotes_RiskDetailId",
                table: "RiskNotes",
                newName: "IX_RiskNotes_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskHistory",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskHistory_RiskDetailId",
                table: "RiskHistory",
                newName: "IX_RiskHistory_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskForm",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskForm_RiskDetailId",
                table: "RiskForm",
                newName: "IX_RiskForm_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskFiling",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskFiling_RiskDetailId",
                table: "RiskFiling",
                newName: "IX_RiskFiling_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskCoverage",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskCoverage_RiskDetailId",
                table: "RiskCoverage",
                newName: "IX_RiskCoverage_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "RiskAdditionalInterest",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_RiskAdditionalInterest_RiskDetailId",
                table: "RiskAdditionalInterest",
                newName: "IX_RiskAdditionalInterest_RiskId");

            migrationBuilder.RenameColumn(
                name: "CurrentRiskDetailId",
                table: "Risk",
                newName: "InsuredId");

            migrationBuilder.RenameIndex(
                name: "IX_Risk_CurrentRiskDetailId",
                table: "Risk",
                newName: "IX_Risk_InsuredId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "DriverPremiumRatingFactor",
                newName: "RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "DriverHeader",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_DriverHeader_RiskDetailId",
                table: "DriverHeader",
                newName: "IX_DriverHeader_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "Driver",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_Driver_RiskDetailId",
                table: "Driver",
                newName: "IX_Driver_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "ClaimsHistory",
                newName: "RiskId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimsHistory_RiskDetailId",
                table: "ClaimsHistory",
                newName: "IX_ClaimsHistory_RiskId");

            migrationBuilder.RenameColumn(
                name: "RiskDetailId",
                table: "BrokerInfo",
                newName: "RiskId");

            migrationBuilder.AddColumn<short>(
                name: "AccountCategoryId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AccountCategoryUWId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AgentCommission",
                table: "Risk",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "AverageMilesPerVehicleId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "Risk",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "Risk",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CurrentOperatingAuthorityStates",
                table: "Risk",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DOTNumberLastFiveYearsDetail",
                table: "Risk",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaysOfService",
                table: "Risk",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverMaxHoursDayDriving",
                table: "Risk",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverMaxHoursDayOnDuty",
                table: "Risk",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverMaxHoursWeekDriving",
                table: "Risk",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverMaxHoursWeekOnDuty",
                table: "Risk",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstIssueDate",
                table: "Risk",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "FirstYearUnderCurrentManagement",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasOperationsOver300MileRadius",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasOwnedCompanies",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoursOfService",
                table: "Risk",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsARatedCarrierRequired",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDifferentUSDOTNumberLastFiveYears",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFilingRequired",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNewBusiness",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNewVenture",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOperatorsWorkOnlyForInsured",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOthersAllowedToOperateCurrent",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOthersAllowedToOperatePast",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUWQuestionResponseConfirmed",
                table: "Risk",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "MainUseId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NewVenturePreviousExperienceId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDrivers",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PastCompanyDetails",
                table: "Risk",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PastOperatingAuthorityStates",
                table: "Risk",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PercentOwnerOperatorVehicles",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolicySuffix",
                table: "Risk",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousExperienceCompany",
                table: "Risk",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryCityZipCodeOfOperationsId",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ProgramID",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiusOfOperationsPercent1",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiusOfOperationsPercent2",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadiusOfOperationsPercent3",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RenewalCount",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedQuoteDate",
                table: "Risk",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RiskStateId",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShiftsPerDay",
                table: "Risk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Risk",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Risk",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Risk",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<short>(
                name: "YearsInBusiness",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearsUnderCurrentManagement",
                table: "Risk",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerInfo_RiskId",
                table: "BrokerInfo",
                column: "RiskId",
                unique: true,
                filter: "[RiskId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerInfo_Risk_RiskId",
                table: "BrokerInfo",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimsHistory_Risk_RiskId",
                table: "ClaimsHistory",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_Risk_RiskId",
                table: "Driver",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverHeader_Risk_RiskId",
                table: "DriverHeader",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Risk_Insured_InsuredId",
                table: "Risk",
                column: "InsuredId",
                principalTable: "Insured",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAdditionalInterest_Risk_RiskId",
                table: "RiskAdditionalInterest",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskCoverage_Risk_RiskId",
                table: "RiskCoverage",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskFiling_Risk_RiskId",
                table: "RiskFiling",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskForm_Risk_RiskId",
                table: "RiskForm",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskHistory_Risk_RiskId",
                table: "RiskHistory",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskNotes_Risk_RiskId",
                table: "RiskNotes",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskResponse_Risk_RiskId",
                table: "RiskResponse",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificCommoditiesHauled_Risk_RiskId",
                table: "RiskSpecificCommoditiesHauled",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDotInfo_Risk_Id",
                table: "RiskSpecificDotInfo",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverHiringCriteria_Risk_Id",
                table: "RiskSpecificDriverHiringCriteria",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificDriverInfo_Risk_Id",
                table: "RiskSpecificDriverInfo",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificRadiusOfOperation_Risk_Id",
                table: "RiskSpecificRadiusOfOperation",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskSpecificUnderwritingQuestion_Risk_Id",
                table: "RiskSpecificUnderwritingQuestion",
                column: "Id",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskStatusHistory_Risk_RiskId",
                table: "RiskStatusHistory",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Risk_RiskId",
                table: "Vehicle",
                column: "RiskId",
                principalTable: "Risk",
                principalColumn: "Id");
        }
    }
}
