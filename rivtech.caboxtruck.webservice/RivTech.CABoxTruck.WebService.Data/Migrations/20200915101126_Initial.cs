using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreetAddress1 = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    StreetAddress2 = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    CityZipCodeId = table.Column<int>(nullable: false),
                    ZipCodeExt = table.Column<string>(type: "varchar(10)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11, 8)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 8)", nullable: false),
                    IsGarageIndoor = table.Column<bool>(nullable: false),
                    IsGarageOutdoor = table.Column<bool>(nullable: false),
                    IsGarageFenced = table.Column<bool>(nullable: false),
                    IsGarageLighted = table.Column<bool>(nullable: false),
                    IsGarageWithSecurityGuard = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    StatusCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    JsonMessage = table.Column<string>(nullable: true),
                    Action = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Method = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    KeyID = table.Column<int>(nullable: false),
                    AuditType = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    Action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Method = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsIndividual = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    MiddleName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    LastName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    CompanyName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    FullName = table.Column<string>(type: "varchar(130)", maxLength: 130, nullable: false),
                    HomePhone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    MobilePhone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    WorkPhone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    WorkPhoneExtension = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    WorkFax = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    PersonalEmailAddress = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    WorkEmailAddress = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    DriverLicenseNumber = table.Column<string>(type: "varchar(130)", maxLength: 130, nullable: true),
                    DriverLicenseState = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DriverLicenseExpiration = table.Column<DateTime>(nullable: true),
                    GenderID = table.Column<byte>(nullable: true),
                    MaritalStatusID = table.Column<byte>(nullable: true),
                    BusinessTypeID = table.Column<byte>(nullable: true),
                    DBA = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    YearEstablished = table.Column<int>(nullable: true),
                    FederalIDNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NAICSCodeID = table.Column<byte>(nullable: true),
                    AdditionalNAICSCodeID = table.Column<byte>(nullable: true),
                    ICCMCDocketNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    USDOTNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PUCNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    HasSubsidiaries = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ErrorCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    ErrorMessage = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false),
                    AddressTypeId = table.Column<byte>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<long>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityAddress_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityAddress_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    ContactTypeId = table.Column<byte>(nullable: false),
                    CompanyPositionId = table.Column<byte>(nullable: true),
                    YearsInPosition = table.Column<short>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<long>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityContact_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntitySubsidiary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Relationship = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TypeOfBusiness = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IsIncludedInInsurance = table.Column<bool>(nullable: false),
                    NumberOfVehicles = table.Column<short>(nullable: false),
                    SizeOfCompany = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    IsCurrentlyOwned = table.Column<bool>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<long>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitySubsidiary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntitySubsidiary_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCompany",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceCompany_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insured",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insured", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insured_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SubmissionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    QuoteNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PolicyNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PolicySuffix = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    FirstIssueDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    CancellationDate = table.Column<DateTime>(nullable: true),
                    RequestedQuoteDate = table.Column<DateTime>(nullable: true),
                    Term = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ProgramID = table.Column<byte>(nullable: false),
                    AgencyId = table.Column<Guid>(nullable: false),
                    SubAgencyId = table.Column<Guid>(nullable: false),
                    AgentId = table.Column<Guid>(nullable: false),
                    IsIncumbentAgency = table.Column<bool>(nullable: true),
                    YearsWithAgency = table.Column<short>(nullable: false),
                    IsBrokeredAccount = table.Column<bool>(nullable: true),
                    InsuredId = table.Column<Guid>(nullable: false),
                    FirstYearUnderCurrentManagement = table.Column<short>(nullable: true),
                    AccountCategoryId = table.Column<byte>(nullable: false),
                    MainUseId = table.Column<byte>(nullable: false),
                    IsNewVenture = table.Column<bool>(nullable: true),
                    NewVenturePreviousExperienceId = table.Column<Guid>(nullable: false),
                    PreviousExperienceCompany = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PrimaryCityZipCodeOfOperationsId = table.Column<byte>(nullable: true),
                    HasOwnedCompanies = table.Column<bool>(nullable: true),
                    PastCompanyDetails = table.Column<string>(nullable: true),
                    RiskStateId = table.Column<byte>(nullable: false),
                    AgentCommission = table.Column<decimal>(type: "decimal(6, 2)", nullable: true),
                    IsARatedCarrierRequired = table.Column<bool>(nullable: false),
                    IsFilingRequired = table.Column<bool>(nullable: false),
                    IsDifferentUSDOTNumberLastFiveYears = table.Column<bool>(nullable: false),
                    DOTNumberLastFiveYearsDetail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    IsOthersAllowedToOperateCurrent = table.Column<bool>(nullable: true),
                    IsOthersAllowedToOperatePast = table.Column<bool>(nullable: true),
                    PastOperatingAuthorityStates = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CurrentOperatingAuthorityStates = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    IsNewBusiness = table.Column<bool>(nullable: false),
                    RenewalCount = table.Column<short>(nullable: false),
                    YearsInBusinessId = table.Column<byte>(nullable: false),
                    AverageMilesPerVehicleId = table.Column<byte>(nullable: false),
                    DriverMaxHoursDayDriving = table.Column<decimal>(type: "decimal(6, 2)", nullable: true),
                    DriverMaxHoursDayOnDuty = table.Column<decimal>(type: "decimal(6, 2)", nullable: true),
                    DriverMaxHoursWeekDriving = table.Column<decimal>(type: "decimal(6, 2)", nullable: true),
                    DriverMaxHoursWeekOnDuty = table.Column<decimal>(type: "decimal(6, 2)", nullable: true),
                    HoursOfService = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    DaysOfService = table.Column<string>(nullable: true),
                    ShiftsPerDay = table.Column<int>(nullable: true),
                    RadiusOfOperationsPercent1 = table.Column<int>(nullable: true),
                    RadiusOfOperationsPercent2 = table.Column<int>(nullable: true),
                    RadiusOfOperationsPercent3 = table.Column<int>(nullable: true),
                    HasOperationsOver300MileRadius = table.Column<bool>(nullable: true),
                    PercentOwnerOperatorVehicles = table.Column<int>(nullable: true),
                    NumberOfDrivers = table.Column<int>(nullable: true),
                    IsUWQuestionResponseConfirmed = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risk_Insured_InsuredId",
                        column: x => x.InsuredId,
                        principalTable: "Insured",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RiskAdditionalInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RiskId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    AdditionalInterestTypeId = table.Column<byte>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    AddProcessDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<long>(nullable: false),
                    RemoveProcessDate = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<long>(nullable: true),
                    EndorsementNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAdditionalInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAdditionalInterest_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiskAdditionalInterest_Risk_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risk",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityAddress_AddressId",
                table: "EntityAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAddress_EntityId",
                table: "EntityAddress",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityContact_EntityId",
                table: "EntityContact",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntitySubsidiary_EntityId",
                table: "EntitySubsidiary",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompany_EntityId",
                table: "InsuranceCompany",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insured_EntityId",
                table: "Insured",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risk_InsuredId",
                table: "Risk",
                column: "InsuredId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAdditionalInterest_EntityId",
                table: "RiskAdditionalInterest",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAdditionalInterest_RiskId",
                table: "RiskAdditionalInterest",
                column: "RiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLog");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "EntityAddress");

            migrationBuilder.DropTable(
                name: "EntityContact");

            migrationBuilder.DropTable(
                name: "EntitySubsidiary");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "InsuranceCompany");

            migrationBuilder.DropTable(
                name: "RiskAdditionalInterest");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "Insured");

            migrationBuilder.DropTable(
                name: "Entity");
        }
    }
}
