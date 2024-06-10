using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public class RiskSpecificsInputMapper : IRaterInputMapper
    {
        public const string MODULE_NAME = "RiskSpecifics";

        public Task<JObject> Map(RiskDTO getRiskDto, RiskCoverageDTO riskCov, JObject requestBody)
        {
            var drivers = getRiskDto?.Drivers?.OrderBy(driver => driver.CreatedDate).ToList();

            var moduleName = MODULE_NAME;
            if (getRiskDto?.Drivers?.Count > 0)
            {
                int driverCount = drivers.Count(x =>
                    ((x.Options ?? "").Split(',')).Contains(riskCov.OptionNumber.ToString()) == true
                    && x.IsValidated == true
                    && x.IsValidatedKO == true);
                requestBody[moduleName]["DriverInfo"]["TotalDrivers"] = driverCount;
            }

            var driverInfo = getRiskDto?.DriverInfo;

            if (driverInfo != null)
            {
                requestBody[moduleName]["DriverInfo"]["AnyVehiclesOperatedForPersonalUse"] = driverInfo?.DoDriversTakeVehiclesHome.ToYesNo(); // from ui - Do drivers take vehicles home?
                requestBody[moduleName]["DriverInfo"]["AnnualCostOfHire"] = ((driverInfo?.AnnualCostOfHire ?? 0) > 0) ? $"{driverInfo?.AnnualCostOfHire}" : "";
            }

            var genLiability = getRiskDto?.GeneralLiabilityCargo?.GeneralLiability;

            if (genLiability != null)
            {
                if (genLiability?.OperationsAtStorageLotOrImpoundYard != null)
                {
                    requestBody[moduleName]["GeneralLiability"]["IsOperationAtStorageLot"] = genLiability?.OperationsAtStorageLotOrImpoundYard.ToYesNo();
                }

                requestBody[moduleName]["GeneralLiability"]["Payroll"] = ((genLiability?.GlAnnualPayroll ?? 0) > 0) ? $"{genLiability?.GlAnnualPayroll}" : "";
            }

            var safetyDevice = getRiskDto?.MaintenanceSafety?.SafetyDevices?.SafetyDevice;

            if (safetyDevice != null)
            {
                string cameraDiscount = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 2 && x.IsInPlace == true).ToYesNo(); //from UI -   {label: 'Cameras', value: 1},
                string navigationOrGPSDataAvailable = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 3 && x.IsInPlace == true).ToYesNo(); //from UI -   {label: 'Geographic Driving History Data', value: 2},
                string mileageTrackingEquipment = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 4 && x.IsInPlace == true).ToYesNo(); //from UI -   {label: 'Mileage Tracking Device', value: 3},
                string brakeWarningSystem = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 5 && x.IsInPlace == true).ToYesNo(); //from UI -  {label: 'Brake Warning System', value: 4},
                string distractedDrivingEquipmentDiscount = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 6 && x.IsInPlace == true).ToYesNo(); //from UI -         {label: 'Distracted Driving Warning System', value: 5},
                string speedWarningSystem = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 7 && x.IsInPlace == true).ToYesNo(); //from UI -         {label: 'Speed Warning System', value: 6},
                string monitoringViaTelematicsDevice = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 8 && x.IsInPlace == true).ToYesNo(); //from UI -  {label: 'Monitoring of Harsh Braking/Speeding/Accelerating via Telematics Device', value: 7},
                string anyActiveAccidentAvoidanceTechnology = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 9 && x.IsInPlace == true).ToYesNo(); //from UI -    {label: 'Any Active Accident Avoidance Technology', value: 8},
                string anyPassiveAccidentAvoidanceTechnology = safetyDevice.Any(x => x.SafetyDeviceCategoryId == 10 && x.IsInPlace == true).ToYesNo(); //from UI -     {label: 'Any Passive Accident Avoidance Technology', value: 9}

                var safetyDeviceCategory = requestBody[moduleName]["SafetyDevices"]["SafetyDeviceCategory"];
                safetyDeviceCategory["Cameras"]["CameraDiscount"] = cameraDiscount;
                safetyDeviceCategory["NavigationOrGPSDataAvailable"] = navigationOrGPSDataAvailable;
                safetyDeviceCategory["MileageTrackingEquipment"] = mileageTrackingEquipment;
                safetyDeviceCategory["BrakeWarningSystem"] = brakeWarningSystem;
                safetyDeviceCategory["DistractedDrivingEquipmentDiscount"] = distractedDrivingEquipmentDiscount;
                safetyDeviceCategory["SpeedWarningSystem"] = speedWarningSystem;
                safetyDeviceCategory["MonitoringViaTelematicsDevice"] = monitoringViaTelematicsDevice;
                safetyDeviceCategory["AnyActiveAccidentAvoidanceTechnology"] = anyActiveAccidentAvoidanceTechnology;
                safetyDeviceCategory["AnyPassiveAccidentAvoidanceTechnology"] = anyPassiveAccidentAvoidanceTechnology;
            }

            var backgroundCheck = getRiskDto?.DriverHiringCriteria?.BackGroundCheckIncludes;
            var driverHiringCriteriaEnabled = drivers.Count > 2;

            var driverHiringCriteriaReq = requestBody[moduleName]["DriverHiringCriteria"];
            var backgroundCheckIncludedReq = driverHiringCriteriaReq["BackgroundCheckIncluded"];
            if (backgroundCheck != null && driverHiringCriteriaEnabled)
            {
                backgroundCheckIncludedReq["WrittenApplication"] = backgroundCheck.Any(x => x.Id == 2).ToYesNo();
                backgroundCheckIncludedReq["RoadTest"] = backgroundCheck.Any(x => x.Id == 3).ToYesNo();
                backgroundCheckIncludedReq["WrittenTest"] = backgroundCheck.Any(x => x.Id == 4).ToYesNo();
                backgroundCheckIncludedReq["FullMedical"] = backgroundCheck.Any(x => x.Id == 5).ToYesNo();
                backgroundCheckIncludedReq["DrugTesting"] = backgroundCheck.Any(x => x.Id == 6).ToYesNo();
                backgroundCheckIncludedReq["CurrentMVR"] = backgroundCheck.Any(x => x.Id == 7).ToYesNo();
                backgroundCheckIncludedReq["ReferenceChecks"] = backgroundCheck.Any(x => x.Id == 8).ToYesNo();
                backgroundCheckIncludedReq["CriminalBackgroundCheck"] = backgroundCheck.Any(x => x.Id == 9).ToYesNo();
            }
            else
            {
                backgroundCheckIncludedReq["WrittenApplication"] = "";
                backgroundCheckIncludedReq["RoadTest"] = "";
                backgroundCheckIncludedReq["WrittenTest"] = "";
                backgroundCheckIncludedReq["FullMedical"] = "";
                backgroundCheckIncludedReq["DrugTesting"] = "";
                backgroundCheckIncludedReq["CurrentMVR"] = "";
                backgroundCheckIncludedReq["ReferenceChecks"] = "";
                backgroundCheckIncludedReq["CriminalBackgroundCheck"] = "";
            }

            var driverCriteria = getRiskDto?.DriverHiringCriteria;

            if (driverCriteria != null && driverHiringCriteriaEnabled)
            {
                string reportAllDriversToRivington = driverCriteria?.IsAgreedToReportAllDriversToRivington.ToYesNo();
                string isDisciplinaryPlanDocumentedForAllDrivers = (driverCriteria?.IsDisciplinaryPlanDocumented == true && getRiskDto?.Drivers?.Count > 2).ToYesNo();
                string allDriversProperlyLicensedAndDOTCompliant = driverCriteria?.AreDriversProperlyLicensedDotCompliant.ToYesNo();
                string allDriversDrivingSimilarVehicleCommerciallyFor2YrsPlus = (driverCriteria?.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years == true && getRiskDto?.Drivers?.Count > 2).ToYesNo();
                string allDriversHaveAtLeast5YrsUSDrivingExperience = (driverCriteria?.DoDriversHave5YearsDrivingExperience == true && getRiskDto?.Drivers?.Count > 2).ToYesNo();

                driverHiringCriteriaReq["ReportAllDriversToRivington"] = reportAllDriversToRivington;
                driverHiringCriteriaReq["IsDisciplinaryPlanDocumentedForAllDrivers"] = isDisciplinaryPlanDocumentedForAllDrivers;
                driverHiringCriteriaReq["AllDriversProperlyLicensedAndDOTCompliant"] = allDriversProperlyLicensedAndDOTCompliant;
                driverHiringCriteriaReq["AllDriversDrivingSimilarVehicleCommerciallyFor2YrsPlus"] = allDriversDrivingSimilarVehicleCommerciallyFor2YrsPlus;
                driverHiringCriteriaReq["AllDriversHaveAtLeast5YrsUSDrivingExperience"] = allDriversHaveAtLeast5YrsUSDrivingExperience;
            }
            else
            {
                driverHiringCriteriaReq["ReportAllDriversToRivington"] = "";
                driverHiringCriteriaReq["IsDisciplinaryPlanDocumentedForAllDrivers"] = "";
                driverHiringCriteriaReq["AllDriversProperlyLicensedAndDOTCompliant"] = "";
                driverHiringCriteriaReq["AllDriversDrivingSimilarVehicleCommerciallyFor2YrsPlus"] = "";
                driverHiringCriteriaReq["AllDriversHaveAtLeast5YrsUSDrivingExperience"] = "";
            }

            var VehicleMaintenanceProgram = getRiskDto?.MaintenanceSafety?.MaintenanceQuestion?.VehicleMaintenance;
            var maintenanceQuestionsEnabled = getRiskDto?.Vehicles?.Count > 5;
            var maintenanceQuestionsReq = requestBody[moduleName]["MaintenanceQuestions"];


            var maintenanceProgramReq = maintenanceQuestionsReq["VehicleMaintenanceProgram"];
            if (VehicleMaintenanceProgram != null && maintenanceQuestionsEnabled)
            {
                maintenanceProgramReq["ServiceRecordForEachVehicle"] = VehicleMaintenanceProgram.Any(x => x.Value == 1).ToYesNo();
                maintenanceProgramReq["ControlledAndFrequentInspections"] = VehicleMaintenanceProgram.Any(x => x.Value == 2).ToYesNo();
                maintenanceProgramReq["VehicleDailyConditionReports"] = VehicleMaintenanceProgram.Any(x => x.Value == 3).ToYesNo();
            }
            else
            {
                maintenanceProgramReq["ServiceRecordForEachVehicle"] = "";
                maintenanceProgramReq["ControlledAndFrequentInspections"] = "";
                maintenanceProgramReq["VehicleDailyConditionReports"] = "";
            }

            var CompanyPractice = getRiskDto?.MaintenanceSafety?.MaintenanceQuestion?.CompanyPractice;

            if (CompanyPractice != null && maintenanceQuestionsEnabled)
            {
                maintenanceQuestionsReq["WrittenMaintenanceProgram"] = CompanyPractice.Any(x => x.Value == 1).ToYesNo();
                maintenanceQuestionsReq["WrittenDriverTrainingProgram"] = CompanyPractice.Any(x => x.Value == 2).ToYesNo();
                maintenanceQuestionsReq["WrittenSafetyProgram"] = CompanyPractice.Any(x => x.Value == 3).ToYesNo();
                maintenanceQuestionsReq["WrittenAccidentReportingProcedures"] = CompanyPractice.Any(x => x.Value == 4).ToYesNo();
            }
            else
            {
                maintenanceQuestionsReq["WrittenMaintenanceProgram"] = "";
                maintenanceQuestionsReq["WrittenDriverTrainingProgram"] = "";
                maintenanceQuestionsReq["WrittenSafetyProgram"] = "";
                maintenanceQuestionsReq["WrittenAccidentReportingProcedures"] = "";
            }

            var MaintenanceQuestion = getRiskDto?.MaintenanceSafety?.MaintenanceQuestion;

            if (MaintenanceQuestion != null && maintenanceQuestionsEnabled)
            {
                maintenanceQuestionsReq["IsMaintenanceProgramManagedByCompany"] = MaintenanceQuestion?.IsMaintenanceProgramManagedByCompany.ToYesNo();
                maintenanceQuestionsReq["DoYouProvideCompleteMaintenanceOnAllVehicles"] = MaintenanceQuestion?.ProvideCompleteMaintenanceOnVehicles.ToYesNo();
                maintenanceQuestionsReq["DriverFilesAvailableForReview"] = MaintenanceQuestion?.AreDriverFilesAvailableForReview.ToYesNo();
                maintenanceQuestionsReq["AccidentFilesAvailableForReview"] = MaintenanceQuestion?.AreAccidentFilesAvailableForReview.ToYesNo();
            }
            else
            {
                maintenanceQuestionsReq["IsMaintenanceProgramManagedByCompany"] = "";
                maintenanceQuestionsReq["DoYouProvideCompleteMaintenanceOnAllVehicles"] = "";
                maintenanceQuestionsReq["DriverFilesAvailableForReview"] = "";
                maintenanceQuestionsReq["AccidentFilesAvailableForReview"] = "";
            }

            if (getRiskDto?.RadiusOfOperations != null)
            {
                string ownerOperator = (getRiskDto?.RadiusOfOperations?.OwnerOperatorPercentage != null && getRiskDto?.RadiusOfOperations?.OwnerOperatorPercentage > 0) ? $"{getRiskDto?.RadiusOfOperations?.OwnerOperatorPercentage / 100}" : string.Empty;
                string radius0To50Miles = (getRiskDto?.RadiusOfOperations?.ZeroToFiftyMilesPercentage != null && getRiskDto?.RadiusOfOperations?.ZeroToFiftyMilesPercentage > 0) ? $"{getRiskDto?.RadiusOfOperations?.ZeroToFiftyMilesPercentage / 100}" : string.Empty;
                string radius50to200Miles = (getRiskDto?.RadiusOfOperations?.FiftyOneToTwoHundredMilesPercentage != null && getRiskDto?.RadiusOfOperations?.FiftyOneToTwoHundredMilesPercentage > 0) ? $"{getRiskDto?.RadiusOfOperations?.FiftyOneToTwoHundredMilesPercentage / 100}" : string.Empty;
                string radiusAbove200Miles = (getRiskDto?.RadiusOfOperations?.TwoHundredPlusMilesPercentage != null && getRiskDto?.RadiusOfOperations?.TwoHundredPlusMilesPercentage > 0) ? $"{getRiskDto?.RadiusOfOperations?.TwoHundredPlusMilesPercentage / 100}" : string.Empty;
                string avgMilesPerVehicle = getRiskDto?.RadiusOfOperations?.AverageMilesPerVehicle;

                requestBody[moduleName]["DestinationInformation"]["OwnerOperator"] = ownerOperator;
                requestBody[moduleName]["DestinationInformation"]["Radius0To50Miles"] = radius0To50Miles;
                requestBody[moduleName]["DestinationInformation"]["Radius50to200Miles"] = radius50to200Miles;
                requestBody[moduleName]["DestinationInformation"]["RadiusAbove200Miles"] = radiusAbove200Miles;
                requestBody[moduleName]["DestinationInformation"]["AvgMilesPerVehicle"] = avgMilesPerVehicle;
            }

            var destinations = getRiskDto?.RiskSpecificDestinations;
            if (destinations.Count > 0)
            {
                for (int i = 1; i <= destinations.Count; i++)
                {
                    var destination = destinations[i - 1];
                    requestBody[moduleName]["DestinationInformation"]["Destination" + i] = destination?.ZipCode;
                    requestBody[moduleName]["DestinationInformation"]["Destinationpct" + i] = $"{destination.PercentageOfTravelActual / 100m}";

                    if (i == 5) break;
                }
            }

            var DOTinfo = getRiskDto?.DotInfo;

            if (DOTinfo != null)
            {
                requestBody[moduleName]["DOTMiscellaneous"]["PolicyLevelAccicdents"] = DOTinfo?.NumberOfPolicyLevelAccidents;
                requestBody[moduleName]["DOTInformation"]["RiskRequiresDOTNumber"] = DOTinfo?.DoesApplicantRequireDotNumber.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["RiskHasDOTNumber"] = getRiskDto?.FilingsInformation?.USDOTNumber;
                requestBody[moduleName]["DOTInformation"]["RiskHasInterstateAuthority"] = (DOTinfo?.DoesApplicantPlanToStayInterstate == false && DOTinfo?.DoesApplicantHaveInsterstateAuthority == true).ToYesNo();
                requestBody[moduleName]["DOTInformation"]["RiskPlansToStayIntrastate"] = DOTinfo?.DoesApplicantPlanToStayInterstate.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["DOTRegistrationDate"] = DOTinfo?.DotRegistrationDate.ToString("MM/dd/yyyy");
                requestBody[moduleName]["DOTInformation"]["DOTCurrentlyActive"] = DOTinfo?.IsDotCurrentlyActive.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["ChameleonIssues"] = DOTinfo?.ChameleonIssues?.Description;
                requestBody[moduleName]["DOTInformation"]["CurrentSaferRating"] = DOTinfo?.CurrentSaferRating?.Description;
                requestBody[moduleName]["DOTInformation"]["ISSCAB"] = DOTinfo?.IssCabRating?.Description;

                requestBody[moduleName]["DOTInformation"]["BasicAlert"]["UnsafeDrivingBasicAlertOrWorse"] = DOTinfo?.IsUnsafeDrivingChecked.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["BasicAlert"]["HoursOfServiceBasicAlertOrWorse"] = DOTinfo?.IsHoursOfServiceChecked.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["BasicAlert"]["DriverFitnessBasicAlertOrWorse"] = DOTinfo?.IsDriverFitnessChecked.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["BasicAlert"]["ControlledSubstanceBasicAlertOrWorse"] = DOTinfo?.IsControlledSubstanceChecked.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["BasicAlert"]["VehicleMaintenanceBasicAlertOrWorse"] = DOTinfo?.IsVehicleMaintenanceChecked.ToYesNo();
                requestBody[moduleName]["DOTInformation"]["BasicAlert"]["CrashBasicAlertOrWorse"] = DOTinfo?.IsCrashChecked.ToYesNo();
            }

            var cargoCommodityHauled = getRiskDto?.GeneralLiabilityCargo?.CommoditiesHauled;
            if (cargoCommodityHauled.Count > 0)
            {
                for (int i = 1; i <= cargoCommodityHauled.Count; i++)
                {
                    var commodity = cargoCommodityHauled[i - 1];
                    requestBody[moduleName]["Cargo"]["CommoditiesHauled"]["Commodity" + i] = commodity?.Commodity;
                    requestBody[moduleName]["Cargo"]["CommoditiesHauled"]["CarryPercentage" + i] = commodity?.PercentageOfCarry;

                    if (i == 5) break;
                }
            }
            return Task.FromResult(requestBody);
        }
    }
}
