using DocumentFormat.OpenXml.Drawing.Charts;
using log4net.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater
{
    public interface IRaterInputValuesFactory
    {
        Task<dynamic> CreateAsync(RiskDTO risk, int? optionNumber = 0);
    }

    public class RaterInputValuesFactory : IRaterInputValuesFactory
    {
        private readonly ILogger<RaterInputValuesFactory> _logger;
        private readonly IListValueTextParser _listValueTextParser;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IVehicleService _vehicleService;
        private readonly IBillingService _billingService;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IDriverHeaderRepository _driverHeaderRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IApplicantAdditionalInterestService _applicantAdditionalInterestService;

        public RaterInputValuesFactory(
            ILogger<RaterInputValuesFactory> logger,
            IListValueTextParser listValueTextParser,
            ICoverageHistoryService coverageHistoryService,
            IApplicantAdditionalInterestService riskAdditionalInterestService,
            IVehicleService vehicleService,
            IBillingService billingService,
            IRiskDetailRepository riskDetailRepository,
            IDriverHeaderRepository driverHeaderRepository
        )
        {
            _logger = logger;
            _listValueTextParser = listValueTextParser;
            _coverageHistoryService = coverageHistoryService;
            _vehicleService = vehicleService;
            _billingService = billingService;
            _riskDetailRepository = riskDetailRepository;
            _driverHeaderRepository = driverHeaderRepository;
            _applicantAdditionalInterestService = riskAdditionalInterestService;
        }


        public async Task<dynamic> CreateAsync(RiskDTO risk, int? optionNumber = 0)
        {
            var sw = new Stopwatch();
            sw.Start();
            dynamic values = new ExpandoObject();
            string state = risk.NameAndAddress.State;
            RiskCoverageDTO selectedOption = risk.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == optionNumber);
            selectedOption ??= risk.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == risk.Binding.BindOptionId);
            values.OptionId = optionNumber;
            values.riskDetailId = risk.Id;
            values.RiskCoverageId = selectedOption.Id.ToString();

            values.Policy = new ExpandoObject();
            values.Policy.InceptionDate = risk?.BrokerInfo?.EffectiveDate?.ToString("MM/dd/yyyy");
            values.Policy.CreditedOffice = CreditedOffice.GetEnumerationById<CreditedOffice>(risk?.Binding?.CreditedOfficeId).Description;
            values.Policy.Midterm = risk.EndorsementNumber > 0 ? "TRUE" : "FALSE";

            if (risk.RaterVersionId >= 3 && values.Policy.Midterm == "TRUE")
            {
                values.Policy.DriverVehicleRatio = decimal.Parse(risk.DriverHeader.driverVehicleRatio);
                values.Policy.AccountDriverFactor = decimal.Parse(risk.DriverHeader.accDriverFactor);
            }

            var bussnsDet = risk.BusinessDetails;
            string newVenture = await _listValueTextParser.ParseAsync(bussnsDet?.NewVenturePreviousExperienceId, ListParserType.NewVenture);
            values.Applicant = new
            {
                BusinessDetails = new
                {
                    BusinessType = await _listValueTextParser.ParseAsync(risk.BusinessDetails.BusinessTypeId, ListParserType.BusinessType),
                    ZipCode = risk.NameAndAddress.ZipCode,
                    OperatingZipCode = risk.NameAndAddress.ZipCodeOperations,
                    AccountCategory = await _listValueTextParser.ParseAsync(risk.BusinessDetails.AccountCategoryUWId, ListParserType.AccountCategory),
                    AccountCategoryMain = await _listValueTextParser.ParseAsync(risk.BusinessDetails.AccountCategoryId, ListParserType.AccountCategory),
                    UseClass = await _listValueTextParser.ParseAsync(risk.BusinessDetails.MainUseId, ListParserType.MainUse),
                    YearsInBusiness = bussnsDet?.YearsInBusiness <= 0 ? newVenture : bussnsDet?.YearsInBusiness.ToString(), //autogenerated from ui, // TODO: fix year in business
                    MainGVW = risk.RaterVersionId > 3 ? await _listValueTextParser.ParseAsync(risk.BusinessDetails.MainGVWId, ListParserType.MainGVW, risk.RaterVersionId) : string.Empty,
                    SecondaryBusinessGroup = risk.RaterVersionId >= 7 ? await _listValueTextParser.ParseAsync(risk.BusinessDetails.SecondaryBusinessGroupId, ListParserType.SecondaryBusinessGroup) : string.Empty
                }
            };

            values.Broker = new
            {
                CommissionAL = Convert.ToDecimal(risk.RiskCoverages.First().BrokerCommisionAL) / 100,
                CommissionPD = Convert.ToDecimal(risk.RiskCoverages.First().BrokerCommisionPD) / 100,
                EffectiveDate = risk.BrokerInfo.EffectiveDate?.ToString("MM/dd/yyyy"),
                BrokerName = risk.BrokerInfo?.Agent?.Entity?.FullName,
                BrokerAgency = risk.BrokerInfo?.Agency?.Entity?.CompanyName
            };

            values.RiskSpecifics = new ExpandoObject();
            values.RiskSpecifics.DriverInfo = new ExpandoObject();

            string noCovText = "No Coverage";
            values.RequestedCoverages = new ExpandoObject();
            values.RequestedCoverages.AutoLiability = new
            {
                AutoBodilyInjury = await _listValueTextParser.ParseLimitAsync(selectedOption.AutoBILimitId, LimitType.AutoBI, state),
                AutoPropertyDamage = await _listValueTextParser.ParseLimitAsync(selectedOption.AutoPDLimitId, LimitType.AutoPD, state),
                AutoLiabilityDeductible = await _listValueTextParser.ParseLimitAsync(selectedOption.LiabilityDeductibleId, LimitType.LiabilityDeductible, state, risk.RaterVersionId),
                MedicalPaymentsLimit = (await _listValueTextParser.ParseLimitAsync(selectedOption.MedPayLimitId, LimitType.MedPay, state))?.SanitizeStringVal(),
                PIPLimit = await _listValueTextParser.ParseLimitAsync(selectedOption.PIPLimitId, LimitType.PIP, state),
                UMSCLLimit = await _listValueTextParser.ParseLimitAsync(selectedOption.UMBILimitId, LimitType.UMBI, state),
                UMPDLimit = await _listValueTextParser.ParseLimitAsync(selectedOption.UMPDLimitId, LimitType.UMPD, state),
                UIMSCLLimit = await _listValueTextParser.ParseLimitAsync(selectedOption.UIMBILimitId, LimitType.UIMBI, state),
                UIMPDLimit = await _listValueTextParser.ParseLimitAsync(selectedOption.UIMPDLimitId, LimitType.UIMPD, state),
                UMStacking = "No",
                SymbolsRequested = await _listValueTextParser.ParseAsync(selectedOption.ALSymbolId, ListParserType.Symbols),
            };

            short? pdSymbol = 10; // default to Id = 10, Desc = "7) Autos Specified on  Schedule"
            if (risk.RaterVersionId >= 7)
            {
                if (selectedOption.CompDeductibleSymbolId.HasValue)
                    pdSymbol = selectedOption.CompDeductibleSymbolId;
                else if (selectedOption.FireDeductibleSymbolId.HasValue)
                    pdSymbol = selectedOption.FireDeductibleSymbolId;
                else if (selectedOption.CollDeductibleSymbolId.HasValue)
                    pdSymbol = selectedOption.CollDeductibleSymbolId;
            }

            values.RequestedCoverages.PhysicalDamage = new
            {
                FireTheftSpecPerils = (await _listValueTextParser.ParseLimitAsync(selectedOption.FireDeductibleId, LimitType.Comprehensive, state)) ?? noCovText,
                COMPDeductible = (await _listValueTextParser.ParseLimitAsync(selectedOption.CompDeductibleId, LimitType.Comprehensive, state)) ?? noCovText,
                COLLDeductible = (await _listValueTextParser.ParseLimitAsync(selectedOption.CollDeductibleId, LimitType.Collision, state)) ?? noCovText,
                PdSymbol = risk.RaterVersionId >= 7 ? await _listValueTextParser.ParseAsync(pdSymbol, ListParserType.Symbols) : string.Empty
            };

            values.RequestedCoverages.OtherCoverages = new
            {
                GLBodilyInjuryLimits = await _listValueTextParser.ParseLimitAsync(selectedOption.GLBILimitId, LimitType.GL, state, risk.RaterVersionId),
                CargoCollLimit = await _listValueTextParser.ParseLimitAsync(selectedOption.CargoLimitId, LimitType.Cargo, state),
                RefrigeratedCargoLimits = await _listValueTextParser.ParseLimitAsync(selectedOption.RefCargoLimitId, LimitType.RefBreakdown, state),
                TrailerInterchangeLimit = risk.RaterVersionId >= 7 ? await _listValueTextParser.ParseLimitAsync(selectedOption.TrailerInterchangeLimitId, LimitType.TrailerInterchange, state) : string.Empty,
                NonOwnedTrailerLimit = risk.RaterVersionId >= 7 ? await _listValueTextParser.ParseLimitAsync(selectedOption.NonOwnedTrailerLimitId, LimitType.NonOwnedTrailer, state) : string.Empty,
                TrailerType = risk.RaterVersionId >= 7 ? await _listValueTextParser.ParseLimitAsync(selectedOption.TrailerTypeLimitId, LimitType.TrailerType, state) : string.Empty,
                PollutionCoverageLimit = risk.RaterVersionId >= 7 ? await _listValueTextParser.ParseLimitAsync(selectedOption.PollutionCovLimitId, LimitType.PollutionCov, state) : string.Empty
            };

            values.RiskSpecifics.GeneralLiability = new
            {
                IsOperationAtStorageLot = risk.GeneralLiabilityCargo.GeneralLiability.OperationsAtStorageLotOrImpoundYard.ToYesNo(), // TODO: fix
                Payroll = risk.GeneralLiabilityCargo.GeneralLiability.GlAnnualPayroll
            };

            // Commodities Hauled
            values.RiskSpecifics.Cargo = new ExpandoObject();
            values.RiskSpecifics.Cargo.CommoditiesHauled = new ExpandoObject();
            var valuesCommoditiesHauled = values.RiskSpecifics.Cargo.CommoditiesHauled as IDictionary<string, object>;
            var commodities = risk.GeneralLiabilityCargo?.CommoditiesHauled;
            var commiditiesMaxCount = commodities.Count > 5 ? 5 : commodities.Count;
            for (int i = 0; i < commiditiesMaxCount; i++)
            {
                valuesCommoditiesHauled.Add($"Commodity{i + 1}", commodities[i].Commodity);
                valuesCommoditiesHauled.Add($"CarryPercentage{i + 1}", commodities[i].PercentageOfCarry);
            }

            // History Coverage
            values.HistoricalCoverage = new ExpandoObject();
            values.HistoricalCoverage.HistoricalInsuranceCoverage = new ExpandoObject();
            var valuesHistoricalInsuranceCov = values.HistoricalCoverage.HistoricalInsuranceCoverage as IDictionary<string, object>;
            var covHistory = await _coverageHistoryService.GetByIdAsync(risk.RiskHistory.RiskDetailId);
            var riskHistories = covHistory?.RiskHistories.OrderBy(x => x.HistoryYear).ToList();
            for (int i = 0; i < (riskHistories?.Count ?? 0); i++)
            {
                var yearName = i == 0 ? "Current" : $"Y{i}";
                valuesHistoricalInsuranceCov.Add($"PolicyInceptionStartDate{yearName}", riskHistories[i]?.PolicyTermStartDate?.ToString("MM/dd/yyyy"));
                valuesHistoricalInsuranceCov.Add($"PolicyEndDate{yearName}", riskHistories[i]?.PolicyTermEndDate?.ToString("MM/dd/yyyy"));
                valuesHistoricalInsuranceCov.Add($"LiabilityLimits{yearName}", riskHistories[i]?.LiabilityLimits.ToNumberOrEmpty());
                valuesHistoricalInsuranceCov.Add($"LiabilityDeductible{yearName}", riskHistories[i]?.LiabilityDeductible.ToNumberOrEmpty());
                valuesHistoricalInsuranceCov.Add($"PowerUnits{yearName}", riskHistories[i]?.NumberOfPowerUnits.ToNumberOrEmpty());

                if (i == 0)
                    valuesHistoricalInsuranceCov.Add($"NoOfVehiclesCurrent", riskHistories[i]?.NumberOfVehicles.ToNumberOrEmpty());
            }
            if (riskHistories?.Any() ?? false)
            {
                values.HistoricalCoverage.ProjectionsAndHistoricalFigures = new
                {
                    GrossFleetMileage = riskHistories[0].TotalFleetMileage
                };
            }

            // Claim History
            values.ClaimsHistory = new ExpandoObject();
            values.ClaimsHistory.LiabilityClaimsInfo = new ExpandoObject();
            var valuesLiabilityClaimsInfo = values.ClaimsHistory.LiabilityClaimsInfo as IDictionary<string, object>;
            var claimHistories = risk.ClaimsHistory.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < (claimHistories?.Count ?? 0); i++)
            {
                var yearName = i == 0 ? "Current" : $"Y{i}";
                valuesLiabilityClaimsInfo.Add($"LossRunDate{yearName}", claimHistories[i].LossRunDate?.ToString("MM/dd/yyyy"));
                valuesLiabilityClaimsInfo.Add($"LossesGroundUpOrNet{yearName}", claimHistories[i].GroundUpNet);
                valuesLiabilityClaimsInfo.Add($"BIPIPMPIncLoss{yearName}", claimHistories[i].BiPiPmpIncLossTotal.ToNumberOrEmpty());
                valuesLiabilityClaimsInfo.Add($"PDIncLoss{yearName}", claimHistories[i].PropertyDamageIncLoss.ToNumberOrEmpty());
                valuesLiabilityClaimsInfo.Add($"BIPIPClaimCount{yearName}", claimHistories[i].ClaimCountBiPip.ToNumberOrEmpty());
                valuesLiabilityClaimsInfo.Add($"PDClaimCount{yearName}", claimHistories[i].ClaimCountPropertyDamage.ToNumberOrEmpty());
                valuesLiabilityClaimsInfo.Add($"OpenClaimsUnder400{yearName}", claimHistories[i].OpenClaimsUnder500.ToNumberOrEmpty());

                //Physical Damage Claims Info
                valuesLiabilityClaimsInfo.Add($"BIPIPMPIncLossAt100k{yearName}", claimHistories[i].BiPiPmpIncLoss100KCap.ToNumberOrEmpty());
                valuesLiabilityClaimsInfo.Add($"PDNetLoss{yearName}", claimHistories[i].PhysicalDamageNetLoss.ToNumberOrEmpty());
                valuesLiabilityClaimsInfo.Add($"PDTotalClaimCount{yearName}", claimHistories[i].PhysicalDamageClaimCount.ToNumberOrEmpty());
            }

            // Safety Device Category
            var safetyDevice = risk?.MaintenanceSafety?.SafetyDevices?.SafetyDevice;
            if (safetyDevice != null)
            {
                values.RiskSpecifics.SafetyDevices = new ExpandoObject();
                values.RiskSpecifics.SafetyDevices.SafetyDeviceCategory = new
                {
                    Cameras = new
                    {
                        CameraDiscount = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 2 && x.IsInPlace == true).ToYesNo(), //from UI -   {label: 'Cameras', value: 1}
                    },
                    NavigationOrGPSDataAvailable = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 3 && x.IsInPlace == true).ToYesNo(), //from UI -   {label: 'Geographic Driving History Data', value: 2}
                    MileageTrackingEquipment = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 4 && x.IsInPlace == true).ToYesNo(), //from UI -   {label: 'Mileage Tracking Device', value: 3},
                    BrakeWarningSystem = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 5 && x.IsInPlace == true).ToYesNo(), //from UI -  {label: 'Brake Warning System', value: 4},
                    DistractedDrivingEquipmentDiscount = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 6 && x.IsInPlace == true).ToYesNo(), //from UI -         {label: 'Distracted Driving Warning System', value: 5},
                    SpeedWarningSystem = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 7 && x.IsInPlace == true).ToYesNo(), //from UI -         {label: 'Speed Warning System', value: 6},
                    MonitoringViaTelematicsDevice = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 8 && x.IsInPlace == true).ToYesNo(), //from UI -  {label: 'Monitoring of Harsh Braking/Speeding/Accelerating via Telematics Device', value: 7},
                    AnyActiveAccidentAvoidanceTechnology = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 9 && x.IsInPlace == true).ToYesNo(), //from UI -    {label: 'Any Active Accident Avoidance Technology', value: 8},
                    AnyPassiveAccidentAvoidanceTechnology = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 10 && x.IsInPlace == true).ToYesNo(), //from UI -     {label: 'Any Passive Accident Avoidance Technology', value: 9}
                };
            }

            // Driver Hiring Criteria - Background Check
            values.RiskSpecifics.DriverHiringCriteria = new ExpandoObject();
            var valuesDriverHiringBgCheck = values.RiskSpecifics.DriverHiringCriteria.BackgroundCheckIncluded = new ExpandoObject();

            var backgroundCheck = risk?.DriverHiringCriteria?.BackGroundCheckIncludes;
            var driverCriteria = risk?.DriverHiringCriteria;

            var areDriversMoreThan2 = GetDriverCount(risk.Drivers, optionNumber) > 2;

            if (!areDriversMoreThan2)
            {
                backgroundCheck = null;
                driverCriteria = null;
            }

            values.RiskSpecifics.DriverHiringCriteria = new
            {
                ReportAllDriversToRivington = driverCriteria?.IsAgreedToReportAllDriversToRivington.ToYesNo() ?? "",
                IsDisciplinaryPlanDocumentedForAllDrivers = driverCriteria != null ? (driverCriteria.IsDisciplinaryPlanDocumented && areDriversMoreThan2).ToYesNo() : "",
                AllDriversProperlyLicensedAndDOTCompliant = driverCriteria != null ? driverCriteria.AreDriversProperlyLicensedDotCompliant.ToYesNo() : "",
                AllDriversDrivingSimilarVehicleCommerciallyFor2YrsPlus = driverCriteria != null ? (driverCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years && areDriversMoreThan2).ToYesNo() : "",
                AllDriversHaveAtLeast5YrsUSDrivingExperience = driverCriteria != null ? (driverCriteria.DoDriversHave5YearsDrivingExperience && areDriversMoreThan2).ToYesNo() : "",
                BackgroundCheckIncluded = new
                {
                    WrittenApplication = backgroundCheck?.Any(x => x.Id == 2).ToYesNo() ?? "",
                    RoadTest = backgroundCheck?.Any(x => x.Id == 3).ToYesNo() ?? "",
                    WrittenTest = backgroundCheck?.Any(x => x.Id == 4).ToYesNo() ?? "",
                    FullMedical = backgroundCheck?.Any(x => x.Id == 5).ToYesNo() ?? "",
                    DrugTesting = backgroundCheck?.Any(x => x.Id == 6).ToYesNo() ?? "",
                    CurrentMVR = backgroundCheck?.Any(x => x.Id == 7).ToYesNo() ?? "",
                    ReferenceChecks = backgroundCheck?.Any(x => x.Id == 8).ToYesNo() ?? "",
                    CriminalBackgroundCheck = backgroundCheck?.Any(x => x.Id == 9).ToYesNo() ?? ""
                }
            };

            // Maintenance Question
            var maintenanceQuestionValues = risk.MaintenanceSafety?.MaintenanceQuestion;
            var vehicleMaintenanceProgramValues = risk?.MaintenanceSafety?.MaintenanceQuestion?.VehicleMaintenance;
            var companyPracticeValues = risk?.MaintenanceSafety?.MaintenanceQuestion?.CompanyPractice;

            bool areVehiclesMoreThan5 = GetVehicleCount(risk.Vehicles, optionNumber) > 5;
            var maintenanceQuestionsEnabled = areVehiclesMoreThan5;
            if (!areVehiclesMoreThan5)
                maintenanceQuestionValues = null;

            values.RiskSpecifics.MaintenanceQuestions = new ExpandoObject();
            var maintenanceQuestionsValues = values.RiskSpecifics.MaintenanceQuestions;

            maintenanceQuestionsValues.IsMaintenanceProgramManagedByCompany = maintenanceQuestionValues?.IsMaintenanceProgramManagedByCompany.ToYesNo() ?? "";
            maintenanceQuestionsValues.DoYouProvideCompleteMaintenanceOnAllVehicles = maintenanceQuestionValues?.ProvideCompleteMaintenanceOnVehicles.ToYesNo();
            maintenanceQuestionsValues.DriverFilesAvailableForReview = maintenanceQuestionValues?.AreDriverFilesAvailableForReview.ToYesNo();
            maintenanceQuestionsValues.AccidentFilesAvailableForReview = maintenanceQuestionValues?.AreAccidentFilesAvailableForReview.ToYesNo();
            maintenanceQuestionsValues.VehicleMaintenanceProgram = new ExpandoObject();

            if (risk.DriverInfo != null)
            {
                var cost = risk.DriverInfo?.AnnualCostOfHire.ToNumberOrEmpty();
                values.RiskSpecifics.DriverInfo.AnyVehiclesOperatedForPersonalUse = risk.DriverInfo?.DoDriversTakeVehiclesHome.ToYesNo(); // from ui - Do drivers take vehicles home?
                values.RiskSpecifics.DriverInfo.AnnualCostOfHire = !string.IsNullOrEmpty(cost) ? $"${cost}" : "";
            }

            var maintenanceProgramReq = values.RiskSpecifics.MaintenanceQuestions.VehicleMaintenanceProgram;
            var VehicleMaintenanceProgram = risk?.MaintenanceSafety?.MaintenanceQuestion?.VehicleMaintenance;
            if (VehicleMaintenanceProgram != null && maintenanceQuestionsEnabled)
            {
                maintenanceProgramReq.ServiceRecordForEachVehicle = VehicleMaintenanceProgram.Any(x => x.Value == 1).ToYesNo();
                maintenanceProgramReq.ControlledAndFrequentInspections = VehicleMaintenanceProgram.Any(x => x.Value == 2).ToYesNo();
                maintenanceProgramReq.VehicleDailyConditionReports = VehicleMaintenanceProgram.Any(x => x.Value == 3).ToYesNo();
            }
            else
            {
                maintenanceProgramReq.ServiceRecordForEachVehicle = "";
                maintenanceProgramReq.ControlledAndFrequentInspections = "";
                maintenanceProgramReq.VehicleDailyConditionReports = "";
            }

            var CompanyPractice = risk?.MaintenanceSafety?.MaintenanceQuestion?.CompanyPractice;
            if (CompanyPractice != null && maintenanceQuestionsEnabled)
            {
                maintenanceQuestionsValues.WrittenMaintenanceProgram = CompanyPractice.Any(x => x.Value == 1).ToYesNo();
                maintenanceQuestionsValues.WrittenDriverTrainingProgram = CompanyPractice.Any(x => x.Value == 2).ToYesNo();
                maintenanceQuestionsValues.WrittenSafetyProgram = CompanyPractice.Any(x => x.Value == 3).ToYesNo();
                maintenanceQuestionsValues.WrittenAccidentReportingProcedures = CompanyPractice.Any(x => x.Value == 4).ToYesNo();
            }
            else
            {
                maintenanceQuestionsValues.WrittenMaintenanceProgram = "";
                maintenanceQuestionsValues.WrittenDriverTrainingProgram = "";
                maintenanceQuestionsValues.WrittenSafetyProgram = "";
                maintenanceQuestionsValues.WrittenAccidentReportingProcedures = "";
            }


            var valuesDestinationInfo = values.RiskSpecifics.DestinationInformation = new ExpandoObject();
            var destinationInformation = values.RiskSpecifics.DestinationInformation as IDictionary<string, object>;
            var operationsRadius = risk?.RadiusOfOperations;
            destinationInformation.Add($"OwnerOperator", operationsRadius?.OwnerOperatorPercentage.ToPercentageOrEmpty() ?? "");
            destinationInformation.Add($"Radius0To50Miles", operationsRadius?.ZeroToFiftyMilesPercentage.ToPercentageOrEmpty() ?? "");
            destinationInformation.Add($"Radius50to200Miles", operationsRadius?.FiftyOneToTwoHundredMilesPercentage.ToPercentageOrEmpty() ?? "");
            destinationInformation.Add($"RadiusAbove200Miles", operationsRadius?.TwoHundredPlusMilesPercentage.ToPercentageOrEmpty() ?? "");
            destinationInformation.Add($"AvgMilesPerVehicle", operationsRadius?.AverageMilesPerVehicle);

            if (risk.RaterVersionId > 3)
            {
                destinationInformation.Add($"RatingType", await _listValueTextParser.ParseAsync(operationsRadius?.RatingTypeId, ListParserType.RatingType));
            }

            //Destination Information
            var destinations = risk?.RiskSpecificDestinations;
            var destinationMaxCount = destinations.Count > 5 ? 5 : destinations.Count;
            for (int i = 0; i < destinationMaxCount; i++)
            {
                destinationInformation.Add($"Destination{i + 1}", destinations[i].ZipCode);
                destinationInformation.Add($"Destinationpct{i + 1}", $"{destinations[i].PercentageOfTravelActual / 100m}");
            }

            var valuesAdditionalInterest = values.AdditionalInterest = new ExpandoObject();

            var additionalInterest = await _applicantAdditionalInterestService.GetByRiskDetailIdActiveAsync(risk.Id.Value);

            bool IsBlanketCoverageAdditionalInsured = risk?.BlanketCoverage?.IsAdditionalInsured ?? false;
            bool IsBlanketCoveragePrimaryAndNonContributory = risk?.BlanketCoverage?.IsPrimaryNonContributory ?? false;
            bool IsBlanketCoverageWaiverOfSubrogation = risk?.BlanketCoverage?.IsWaiverOfSubrogation ?? false;

            int ADDITIONAL_INSURED_PREMIUM = 50;
            int WAIVER_OF_SUBROGATION_PREMIUM = 250;
            int PRIMARY_AND_NONCONTRIBUTORY_PREMIUM = 500;

            if (additionalInterest.Count() > 0)
            {
                valuesAdditionalInterest.NoOfAdditionalInsured = additionalInterest.Count(x => x.AdditionalInterestTypeId == AdditionalInterestType.AdditionalInsured.Id);
                valuesAdditionalInterest.WaiverOfSubrogation = additionalInterest.Any(x => x.IsWaiverOfSubrogation == true).ToYesNo();
                valuesAdditionalInterest.PrimaryNonContributoryCoverage = additionalInterest.Any(x => x.IsPrimaryNonContributory == true).ToYesNo();
                // additional interest premiums & blanket coverage
                valuesAdditionalInterest.AdditionalInsuredPremium = (IsBlanketCoverageAdditionalInsured) ? 500 : (((int)valuesAdditionalInterest.NoOfAdditionalInsured) * ADDITIONAL_INSURED_PREMIUM);
                valuesAdditionalInterest.PrimaryNonContributoryPremium = (IsBlanketCoveragePrimaryAndNonContributory) ? 1500 : (additionalInterest.Where(x => x.IsPrimaryNonContributory == true).Count() * PRIMARY_AND_NONCONTRIBUTORY_PREMIUM);
                valuesAdditionalInterest.WaiverOfSubrogationPremium = (IsBlanketCoverageWaiverOfSubrogation) ? 1000 : (additionalInterest.Where(x => x.IsWaiverOfSubrogation == true).Count() * WAIVER_OF_SUBROGATION_PREMIUM);
            }
            else
            {
                valuesAdditionalInterest.AdditionalInsuredPremium = (IsBlanketCoverageAdditionalInsured) ? 500 : 0;
                valuesAdditionalInterest.PrimaryNonContributoryPremium = (IsBlanketCoveragePrimaryAndNonContributory) ? 1500 : 0;
                valuesAdditionalInterest.WaiverOfSubrogationPremium = (IsBlanketCoverageWaiverOfSubrogation) ? 1000 : 0;
            }

            AddDOTInfo(values, risk);
            AddDriverValues(values, risk, selectedOption);
            AddRatingFactors(values, selectedOption);
            await AddVehicleValuesAsync(values, risk, selectedOption);

            AddEndorsementPremiums(values, risk);

            sw.Stop();
            _logger.LogDebug($"Created rater input request body. [{sw.Elapsed.TotalSeconds}]");
            return values;
        }

        private int GetVehicleCount(List<VehicleDTO> vehicles, int? option)
        {
            if (vehicles == null) return 0;
            if (option == 0) return vehicles.Count();
            return vehicles.Where(x => (!string.IsNullOrEmpty(x.Options) ? x.Options.Contains(option.ToString()) : false) == true).Count();
        }

        private int GetDriverCount(List<DriverDTO> driverDTOs, int? option)
        {
            if (driverDTOs == null) return 0;
            if (option == 0) return driverDTOs.Count();
            return driverDTOs.Where(x => (!string.IsNullOrEmpty(x.Options) ? x.Options.Contains(option.ToString()) : false) == true).Count();
        }

        private void AddDriverValues(dynamic values, RiskDTO risk, RiskCoverageDTO selectedOption)
        {
            var drivers = risk.Drivers
                .Where(x => ((x.Options ?? "").Split(',')).Contains(selectedOption.OptionNumber.ToString()) == true && x.IsValidated == true && x.IsValidatedKO == true)
                .ToList();
            var driverListValues = new List<dynamic>();
            if (drivers.Count() == 0)
                return;

            foreach (var dvr in drivers)
            {
                var jObject = new
                {
                    DriverID = dvr.Id,
                    Name = $"{dvr.FirstName} {dvr.LastName}",
                    DateOfBirth = dvr.BirthDate?.ToString("MM/dd/yyyy"),
                    Age = dvr.Age,
                    OutOfState = dvr.IsOutOfState.ToYesNo(),
                    YrsDrivingExperience = dvr.YrsDrivingExp ?? 0,
                    YrsCommercialDriverExperience = dvr.YrsCommDrivingExp,
                    Majors = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "1")),
                    Drug = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "2")),
                    Minors = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "3")),
                    AtFault = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "4")),
                    MajorLicenseIssue = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "5")),
                    NAF = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "6")),
                    Equipment = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "7")),
                    Speed = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "8")),
                    Other = CountIncident(dvr.DriverIncidents.Count(x => x.IncidentType == "9")),
                };
                driverListValues.Add(jObject);
            }

            values.Driver = new
            {
                UseDriverRatingTab = risk.DriverHeader?.driverRatingTab.ToYesNo(),
                UseAccidentsOrViolationInfo = risk.DriverHeader?.accidentInfo.ToYesNo(),
                KOUnder25 = risk.DriverHeader?.applyFilter?.Split(',').Any(x => x == "1").ToYesNo(),
                KOAFOrMovingViolations = risk.DriverHeader?.applyFilter?.Split(',').Any(x => x == "2").ToYesNo(),
                DriverList = driverListValues,
            };

            values.RiskSpecifics.DriverInfo.TotalDrivers = driverListValues.Count(); // TODO
        }

        private string CountIncident(int inccount)
        {
            string count = "0";

            switch (inccount)
            {
                case 0:
                    count = "0";
                    break;
                case 1:
                    count = "1";
                    break;
                case 2:
                    count = "2";
                    break;
                default:
                    count = "3";
                    break;

            }

            return count;

        }

        private async Task AddVehicleValuesAsync(dynamic values, RiskDTO risk, RiskCoverageDTO selectedOption)
        {
            values.Vehicle = new ExpandoObject();
            var state = risk.NameAndAddress.State;

            // can we call the vehicle from getRiskDto similar to driver?
            var feeDetails = _billingService.GetFeeDetailsByState(risk?.NameAndAddress?.State);
            decimal RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.SingleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.SinglePDServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultiplePDServiceFee)?.Amount ?? 0;

            decimal RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultiplePDServiceFee)?.Amount ?? 0;

            var veh = (await _vehicleService.GetByRiskDetailIdIncludeAsync(risk.RiskCoverages.First().RiskDetailId))?
                    .OrderBy(vehicle => vehicle.VIN).ToList();
            veh = veh.Where(x => ((x.Options ?? "").Split(',')).Contains(selectedOption.OptionNumber.ToString()) == true && x.IsValidated == true).ToList();
            if (veh.Count == 0)
                throw new RaterException($"No vehicles available for rating.");
            var idMapping = new Dictionary<int, Guid>();
            var valuesVehicleList = new List<dynamic>();
            if (veh.Count == 0)
                return;

            int autoLiabilityUnits = veh.Count; // all vehicles are autoliability units
            int physicalDamageUnits = 0;

            bool isPDLimitsEnabled = (selectedOption.CollDeductibleId ?? 65) != 65 || ((selectedOption.CompDeductibleId ?? 76) != 76 || (selectedOption.FireDeductibleId ?? 76) != 76);

            var row = 1;
            foreach (var _veh in veh)
            {

                short? compId = _veh.CoverageFireTheftDeductibleId ?? 76;
                short? collId = _veh.CoverageCollisionDeductibleId ?? 65;

                string hasCargo = _veh.HasCoverageCargo.ToYesNo();
                string hasRef = _veh.HasCoverageRefrigeration.ToYesNo();

                if (selectedOption.OptionNumber > 1)
                {
                    if (selectedOption.CompDeductibleId != null)
                    {
                        compId = selectedOption.CompDeductibleId;
                    }
                    else if (selectedOption.FireDeductibleId != null)
                    {
                        compId = selectedOption.FireDeductibleId;
                    }

                    hasCargo = "Yes";
                    hasRef = "Yes";
                    if (selectedOption.CargoLimitId == 53)
                        hasCargo = "No";
                    if (selectedOption.RefCargoLimitId == 157)
                        hasRef = "No";

                    collId = selectedOption.CollDeductibleId;
                }

                if (compId != 76 || collId != 65)
                {
                    physicalDamageUnits++;
                }

                idMapping[row] = _veh.Id.Value;
                var jObject = new
                {
                    Number = row,
                    VIN = _veh.VIN,
                    Year = _veh.Year,
                    VehicleType = _veh.VehicleType, // { "VehicleType", parseListToValue(_veh.VehicleType, requestBody[moduleName]["VehicleDropdowns"]["VehicleType"])  },
                    Description = await _listValueTextParser.ParseAsync(_veh.VehicleDescriptionId, ListParserType.VehicleDescription,risk.RaterVersionId),
                    CompDeductible = await _listValueTextParser.ParseLimitAsync(compId, LimitType.Comprehensive, state),
                    CollDeductible = await _listValueTextParser.ParseLimitAsync(collId, LimitType.Collision, state),
                    CollStateAmount = _veh.StatedAmount,
                    IncCargoLimit = hasCargo,
                    IncRefCargoLimit = hasRef,
                    UseClass = await _listValueTextParser.ParseAsync(_veh.UseClassId, ListParserType.MainUse),
                    BusinessClass = await _listValueTextParser.ParseAsync(_veh.VehicleBusinessClassId, ListParserType.AccountCategory),
                    Radius = _veh.Radius,
                };
                valuesVehicleList.Add(jObject);
                row++;
            }

            var vehicleIdMappingKey = Guid.NewGuid();
            PremiumRaterVehicleFactorsDTO.VehicleIdMappings.Add(vehicleIdMappingKey, idMapping);

            decimal riskManagementFeeAL = (autoLiabilityUnits > 1)
                ? (autoLiabilityUnits * RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT)
                : (autoLiabilityUnits == 1) ? RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT : 0;

            decimal riskManagementFeePD = (autoLiabilityUnits > 1 && physicalDamageUnits > 0)
                ? (physicalDamageUnits * RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT)
                : (autoLiabilityUnits == 1 && physicalDamageUnits == 1) ? RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT : 0;

            if ((autoLiabilityUnits >= 1) && RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT > 0)
                riskManagementFeeAL = RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT;

            if ((autoLiabilityUnits >= 1 && physicalDamageUnits > 0) && RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT > 0)
                riskManagementFeePD = RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT;

            values.Vehicle = new
            {
                RiskManagementFeeAL = riskManagementFeeAL,
                RiskManagementFeePD = riskManagementFeePD,
                VehicleList = valuesVehicleList,
                IdMappingKey = vehicleIdMappingKey.ToString(),
                TotalVehicles = veh.Count
            };
        }

        private void AddDOTInfo(dynamic values, RiskDTO risk)
        {
            var DOTinfo = risk?.DotInfo;
            if (DOTinfo != null)
            {
                values.RiskSpecifics.DOTMiscellaneous = new
                {
                    PolicyLevelAccicdents = DOTinfo?.NumberOfPolicyLevelAccidents ?? 0
                };

                values.RiskSpecifics.DOTInformation = new
                {
                    RiskRequiresDOTNumber = DOTinfo?.DoesApplicantRequireDotNumber.ToYesNo() ?? "",
                    RiskHasDOTNumber = risk.FilingsInformation?.USDOTNumber ?? "",
                    RiskHasInterstateAuthority = (DOTinfo?.DoesApplicantPlanToStayInterstate == false && DOTinfo?.DoesApplicantHaveInsterstateAuthority == true).ToYesNo(),
                    RiskPlansToStayIntrastate = DOTinfo?.DoesApplicantPlanToStayInterstate.ToYesNo(),
                    DOTRegistrationDate = DOTinfo?.DotRegistrationDate.ToString("MM/dd/yyyy"),
                    DOTCurrentlyActive = DOTinfo?.IsDotCurrentlyActive.ToYesNo(),
                    ChameleonIssues = DOTinfo?.ChameleonIssues?.Description,
                    CurrentSaferRating = DOTinfo?.CurrentSaferRating?.Description,
                    ISSCAB = DOTinfo?.IssCabRating?.Description,
                    BasicAlert = new
                    {
                        UnsafeDrivingBasicAlertOrWorse = DOTinfo?.IsUnsafeDrivingChecked.ToYesNo(),
                        HoursOfServiceBasicAlertOrWorse = DOTinfo?.IsHoursOfServiceChecked.ToYesNo(),
                        DriverFitnessBasicAlertOrWorse = DOTinfo?.IsDriverFitnessChecked.ToYesNo(),
                        ControlledSubstanceBasicAlertOrWorse = DOTinfo?.IsControlledSubstanceChecked.ToYesNo(),
                        VehicleMaintenanceBasicAlertOrWorse = DOTinfo?.IsVehicleMaintenanceChecked.ToYesNo(),
                        CrashBasicAlertOrWorse = DOTinfo?.IsCrashChecked.ToYesNo()
                    }
                };
            }

        }

        private void AddRatingFactors(dynamic values, RiskCoverageDTO selectedOption)
        {
            values.RatingFactors = new
            {
                LiabilityScheduleRatingFactor = selectedOption.LiabilityScheduleRatingFactor,
                LiabilityExperieceRatingFactor = selectedOption.LiabilityExperienceRatingFactor,
                GLScheduleRatingFactor = selectedOption.GlScheduleRF,
                GLExperienceRatingFactor = selectedOption.GlExperienceRF,
                CargoScheduleRatingFactor = selectedOption.CargoScheduleRF,
                CargoExperienceRatingFactor = selectedOption.CargoExperienceRF,
                APDScheduleRatingFactor = selectedOption.ApdScheduleRF,
                APDExperienceRatingFactor = selectedOption.ApdExperienceRF,
            };
        }

        private void AddEndorsementPremiums(dynamic values, RiskDTO risk)
        {
            var riskForms = risk.RiskForms;
            var hiredPhysicalDamage = riskForms?.FirstOrDefault(form => form.Form.Id == "HiredPhysicalDamage" && form.IsSelected == true);
            var hiredPhysicalDamageFormData = JsonConvert.DeserializeObject<dynamic>(hiredPhysicalDamage?.Other ?? "");
            var trailerInterchange = riskForms?.FirstOrDefault(form => form.Form.Id == "TrailerInterchangeCoverage" && form.IsSelected == true);
            var trailerInterchangeFormData = JsonConvert.DeserializeObject<dynamic>(trailerInterchange?.Other ?? "");
            var trailerIntPremium = Convert.ToDecimal(trailerInterchangeFormData?.comprePremium)
                + Convert.ToDecimal(trailerInterchangeFormData?.lossPremium)
                + Convert.ToDecimal(trailerInterchangeFormData?.collisionPremium)
                + Convert.ToDecimal(trailerInterchangeFormData?.firePremium)
                + Convert.ToDecimal(trailerInterchangeFormData?.fireTheftPremium);

            var manuscripts = risk.RiskManuscripts;
            var alManuscriptPremium = manuscripts?.Where(m => m.IsSelected == true && m.PremiumType == "AL")
                ?.Aggregate(0m, (sum, current) => sum + current.Premium);
            var pdManuscriptPremium = manuscripts?.Where(m => m.IsSelected == true && m.PremiumType == "PD")
                ?.Aggregate(0m, (sum, current) => sum + current.Premium);

            values.EndorsementPremiums = new
            {
                HiredPhysicalDamage = Convert.ToDecimal(hiredPhysicalDamageFormData?.estimatedAdditionalPremium.ToString()),
                TrailerInterchange = trailerIntPremium,
                ALManuscriptPremium = alManuscriptPremium,
                PDManuscriptPremium = pdManuscriptPremium
            };
        }
    }

    static class RaterInputExtensions
    {
        /// <summary>
        /// Returns empty if value is zero else returns the number as is.
        /// </summary>

        public static string ToNumberOrEmpty(this decimal number)
        {
            return number > 0 ? number.ToString() : "";
        }

        /// <summary>
        /// Returns empty if value is null/zero else returns the number as is.
        /// </summary>
        public static string ToNumberOrEmpty(this decimal? number)
        {
            return ToNumberOrEmpty(number ?? 0m);
        }

        /// <summary>
        /// Returns empty if value is zero else returns the number as is.
        /// </summary>
        public static string ToNumberOrEmpty(this int number)
        {
            return number > 0 ? number.ToString() : "";
        }

        /// <summary>
        /// Returns empty if value is null/zero else returns the number as is.
        /// </summary>
        public static string ToNumberOrEmpty(this int? number)
        {
            return ToNumberOrEmpty(number ?? 0);
        }

        /// <summary>
        /// Returns empty if value is zero else returns the number as is.
        /// </summary>
        public static string ToPercentageOrEmpty(this decimal number)
        {
            return number > 0 ? (number / 100).ToString() : "";
        }

    }
}
