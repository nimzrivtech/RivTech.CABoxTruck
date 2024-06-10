using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Constants
{
    public class RiskStatus
    {
        public const string Active = "Active";
        public const string Pending = "Pending";
        public const string Canceled = "Canceled";
        public const string Reinstated = "Reinstated";
        public const string Expired = "Expired";
    }

    public class AuditType
    {
        public const string Login = "LOGIN";
        public const string Logout = "LOGOUT";
        public const string Lock = "LOCK";
        public const string Locked = "LOCKED";
        public const string Failed = "LOGINFAIL";
    }

    public class RiskDetailStatus
    {
        public const string Received = "Received";
        public const string InResearch = "In Research";
        public const string Complete = "Complete";
        public const string Incomplete = "Incomplete";
        public const string Withdrawn = "Withdrawn";
        public const string Quoted = "Quoted";
        public const string Declined = "Declined";
        public const string Canceled = "Canceled";
        public const string Reinstated = "Reinstated";
        public const string Expired = "Expired";
        public const string DataOnly = "Data Only";
        public const string TestClass = "Test Class";

        public const string Active = "Active";
        public const string PendingEndorsement = "Pending Endorsement";
        public const string PendingCancellation = "Pending Cancellation";
        public const string PendingNonRenewal = "Pending Non Renewal";
        public const string Draft = "Draft";
        public const string Rewrite = "Rewrite";
    }

    public class PayPlanOption
    {
        public const string FullPay = "PPO0";
        public const string TwoPay = "PPO1";
        public const string FourPay = "PPO2";
        public const string EightPay = "PPO3";
        public const string Mortgagee = "PPO4";
    }

    public class InstallmentOption
    {
        public const string Full = "Full";
        public const string Two = "Two";
        public const string Three = "Three";
        public const string Four = "Four";
        public const string Five = "Five";
        public const string Six = "Six";
        public const string Seven = "Seven";
        public const string Eight = "Eight";
        public const string Nine = "Nine";
        public const string Ten = "Ten";
        public const string Eleven = "Eleven";
    }

    public class PaymentMethod
    {
        public const string CashCheck = "PM0";
        public const string CreditCard = "PM1";
        public const string ECheck = "PM2";
        public const string RecurringCreditCard = "PM3";
        public const string RecurringECheck = "PM4";

        public static bool IsPaymentCreditCard(string paymentMethod)
        {
            return
                paymentMethod == CreditCard ||
                paymentMethod == RecurringCreditCard;
        }

        public static bool IsPaymentCashOrCheck(string paymentMethod)
        {
            return
                paymentMethod == CashCheck ||
                paymentMethod == ECheck ||
                paymentMethod == RecurringECheck;
        }

        public static bool IsPaymentMethodRecurring(string paymentMethod)
        {
            return
                paymentMethod == RecurringCreditCard ||
                paymentMethod == RecurringECheck;
        }
    }

    public class FieldLabels
    {
        public static readonly Dictionary<string, string> Labels = new Dictionary<string, string>
        {
            // applicant
            { "Entity.CompanyName", "Business Name" },
            { "Address.StreetAddress1", "Street Address" },
            { "Address.City", "City" },
            { "Address.State", "State" },
            { "Address.ZipCode", "Zip Code" },
            //{ "Address.IsMainGarage", "Main Garaging Address" },
            { "EntityAddress.IsMainGaragingAddress", "Main Garaging Address" },
            // limits
            { "RiskCoverage.AutoBILimitId", "Auto Liability" },
            { "RiskCoverage.UMBILimitId", "Uninsured Motorist - BI" },
            { "RiskCoverage.UMPDLimitId", "Uninsured Motorist - PD" },
            { "RiskCoverage.UIMBILimitId", "Underinsured Motorist - BI" },
            { "RiskCoverage.UIMPDLimitId", "Uninsured Motorist - PD" },
            { "RiskCoverage.MedPayLimitId", "Medical Payments" },
            { "RiskCoverage.PIPLimitId", "Personal Injury Protection (PIP)" },
            { "RiskCoverage.LiabilityDeductibleId", "Liability Deductible" },
            { "RiskCoverage.CompDeductibleId", "Comprehensive" },
            { "RiskCoverage.CollDeductibleId", "Collision" },
            { "RiskCoverage.FireDeductibleId", "Fire / Theft" },
            { "RiskCoverage.GLBILimitId", "General Liability - Bodily Injury" },
            { "RiskCoverage.CargoLimitId", "Cargo" },
            { "RiskCoverage.RefCargoLimitId", "Refrigeration Breakdown" },
            { "RiskCoverage.TrailerInterchangeLimitId", "Trailer Interchange" },
            { "RiskCoverage.NonOwnedTrailerLimitId", "Non-Owned Trailer" },
            { "RiskCoverage.TrailerTypeLimitId", "Trailer Type" },
            { "RiskCoverage.PollutionCovLimitId", "Pollution Coverage" },
            //{ "RiskCoverage.LiabilityScheduleRatingFactor", "Auto Liability Schedule RF" },
            //{ "RiskCoverage.ApdScheduleRF", "Physical Damage Schedule RF" },
            //{ "RiskCoverage.CargoScheduleRF", "Cargo Schedule RF" },
            //{ "RiskCoverage.GlScheduleRF", "General Liability Schedule RF" },
            // symbols
            { "RiskCoverage.ALSymbolId", "Auto Liability Symbol" },
            { "RiskCoverage.UMBILimitSymbolId", "Uninsured Motorist - BI Symbol" },
            { "RiskCoverage.UMPDLimitSymbolId", "Uninsured Motorist - PD Symbol" },
            { "RiskCoverage.UIMBILimitSymbolId", "Underinsured Motorist - BI Symbol" },
            { "RiskCoverage.UIMPDLimitSymbolId", "Underinsured Motorist - PD Symbol" },
            { "RiskCoverage.MedPayLimitSymbolId", "Medical Payments Symbol" },
            { "RiskCoverage.PIPLimitSymbolId", "Personal Injury Protection (PIP) Symbol" },
            { "RiskCoverage.CompDeductibleSymbolId", "Comprehensive Symbol" },
            { "RiskCoverage.CollDeductibleSymbolId", "Collision Symbol" },
            { "RiskCoverage.FireDeductibleSymbolId", "Fire / Theft Symbol" },
            { "RiskCoverage.CargoLimitSymbolId", "Cargo Symbol" },
            { "RiskCoverage.RefCargoLimitSymbolId", "Refrigeration Breakdown Symbol" },
            { "RiskCoverage.TrailerInterchangeLimitSymbolId", "Trailer Interchange Symbol" },
            { "RiskCoverage.NonOwnedTrailerLimitSymbolId", "Non-Owned Trailer Symbol" },
            //risk coverage premium -- N/A
            {"RiskCoveragePremium.AutoLiabilityPremium", "Auto Liability Premium"},
            {"RiskCoveragePremium.CargoPremium", "Cargo Premium"},
            {"RiskCoveragePremium.PhysicalDamagePremium", "Physical Damage Premium"},
            {"RiskCoveragePremium.GeneralLiabilityPremium", "General Liability Premium"},
            {"RiskCoveragePremium.TrailerInterchangeAL", "Trailer Interchange Premium"},
            {"RiskCoveragePremium.NonOwnedTrailerPremium", "Non-Owned Trailer Premium"},
            {"RiskCoveragePremium.Premium", "Premium"},
            {"RiskCoveragePremium.RiskMgrFeeAL", "Service Fee AL"},
            {"RiskCoveragePremium.RiskMgrFeePD", "Service Fee PD"},
            {"RiskCoveragePremium.AdditionalInsuredPremium", "Additional Insured Premium"},
            {"RiskCoveragePremium.PrimaryNonContributoryPremium", "Primary & Non-Contributory Premium"},
            {"RiskCoveragePremium.WaiverOfSubrogationPremium", "Waiver of Subrogation Premium"},
            {"RiskCoveragePremium.CollisionPremium", "Collision Premium"},
            {"RiskCoveragePremium.ComprehensivePremium", "Comprehensive / Fire & Theft Premium"},
            {"RiskCoveragePremium.HiredPhysicalDamage", "Hired Physical Damage"},
            {"RiskCoveragePremium.TrailerInterchange", "Trailer Interchange Coverage"},
            {"RiskCoveragePremium.ALManuscriptPremium", "Manuscript (AL) Premium"},
            {"RiskCoveragePremium.PDManuscriptPremium", "Manuscript (PD) Premium"},
            // risk history -- N/A
            { "RiskHistory.PolicyTermStartDate", "Policy Term Start Date" },
            { "RiskHistory.PolicyTermEndDate", "Policy Term End Date" },
            { "RiskHistory.InsuranceCarrierOrBroker", "Insurance Carrier Or Broker" },
            { "RiskHistory.LiabilityLimits", "Liability Limits" },
            { "RiskHistory.LiabilityDeductible", "Liability Deductible" },
            { "RiskHistory.AutoLiabilityPremiumPerVehicle", "Auto Liability Premium Per Vehicle" },
            { "RiskHistory.PhysicalDamagePremiumPerVehicle", "Physical Damage Premium Per Vehicle" },
            { "RiskHistory.TotalPremiumPerVehicle", "Total Premium Per Vehicle" },
            { "RiskHistory.NumberOfVehicles", "Number Of Vehicles" },
            { "RiskHistory.LossRunDateForEachTerm", "Loss Run Date For Each Term" },
            { "RiskHistory.NumberOfPowerUnits", "Number Of Power Units" },
            { "RiskHistory.NumberOfPowerUnitsAL", "Number Of Power Units (AL)" },
            { "RiskHistory.NumberOfPowerUnitsAPD", "Number Of Power Units (APD)" },
            { "RiskHistory.NumberOfSpareVehicles", "Number Of Spare Vehicles" },
            { "RiskHistory.NumberOfDrivers", "Number Of Drivers" },
            { "RiskHistory.CollisionDeductible", "Collision Deductible" },
            { "RiskHistory.ComprehensiveDeductible", "Comprehensive Deductible" },
            { "RiskHistory.CargoLimits", "Cargo Limits" },
            { "RiskHistory.RefrigeratedCargoLimits", "Refrigerated Cargo Limits" },
            { "RiskHistory.GrossRevenues", "Gross Revenues" },
            { "RiskHistory.TotalFleetMileage", "Total Fleet Mileage" },
            { "RiskHistory.NumberOfOneWayTrips", "Number Of One Way Trips" },
            // claims -- N/A
            { "ClaimsHistory.LossRunDate", "Loss Run Date" },
            { "ClaimsHistory.GroundUpNet", "Gross or Net Losses?" },
            { "ClaimsHistory.BiPiPmpIncLossTotal", "BI/PIP/MP Inc Loss - Total" },
            { "ClaimsHistory.BiPiPmpIncLoss100KCap", "BI/PIP/MP Inc Loss - $100k Cap" },
            { "ClaimsHistory.PropertyDamageIncLoss", "Property Damage Inc Loss" },
            { "ClaimsHistory.ClaimCountBiPip", "Claim Count with BI/PIP" },
            { "ClaimsHistory.ClaimCountPropertyDamage", "Claim Count (Property Damage Only)" },
            { "ClaimsHistory.OpenClaimsUnder500", "Open Claims Under $400" },
            { "ClaimsHistory.NetLoss", "Net Loss" },
            { "ClaimsHistory.TotalClaimCount", "Total Claim Count" },
            { "ClaimsHistory.PhysicalDamageClaimCount", "Physical Damage Net Total Claim Count" },
            { "ClaimsHistory.PhysicalDamageNetLoss", "Physical Damage Net Loss" },
            { "ClaimsHistory.CargoClaimCount", "Cargo Total Claim Count" },
            { "ClaimsHistory.CargoNetLoss", "Cargo Net Loss" },
            { "ClaimsHistory.GeneralLiabilityClaimCount", "General Liability Total Claim Count" },
            { "ClaimsHistory.GeneralLiabilityNetLoss", "General Liability Net Loss" },
            // general liab and cargo -- N/A
            { "contractuallyRequiredToCarryGeneralLiabilityInsurance", "Are you contractually required to carry General Liability insurance?" },
            { "haveOperationsOtherThanTrucking", "Does the applicant have any operations other than trucking?" },
            { "operationsAtStorageLotOrImpoundYard", "Are any operations at a storage lot or impound yard?" },
            { "anyStorageOfGoods", "Any storage of goods?" },
            { "anyWarehousing", "Any warehousing?" },
            { "anyStorageOfVehiclesForOthers", "Any storage of vehicles for others?" },
            { "anyLeasingSpaceToOthers", "Any leasing space to others?" },
            { "anyFreightForwarding", "Any freight forwarding?" },
            { "anyStorageOfFuelsAndOrChemicals", "Any storage of fuels and/or chemicals?" },
            { "hasApplicantHadGeneralLiabilityLoss", "Has applicant ever had a general liability loss?" },
            { "glAnnualPayroll", "GL Annual Payroll" },
            { "areRequiredToCarryCargoInsurance", "Are you contractually required to carry cargo insurance?" },
            { "requireRefrigerationBreakdownCoverage", "Do you require refrigeration breakdown coverage?" },
            { "haulHazardousMaterials", "Do you haul any hazardous materials?" },
            { "haveRefrigeratedUnits", "Do you have any refrigerated units?" },
            { "areCommoditiesStoredInTruckOvernight", "Are commodities ever stored in truck overnight?" },
            { "hadCargoLoss", "Has applicant ever had a cargo loss?" },
            // vehicles
            { "Vehicle.VIN", "VIN"},
            { "Vehicle.Year", "Year"},
            { "Vehicle.Make", "Make"},
            { "Vehicle.Model", "Model"},
            { "Vehicle.Style", "Style"},
            { "Vehicle.VehicleType", "Vehicle Type"},
            { "Vehicle.VehicleDescriptionId", "Vehicle Description"},
            { "Vehicle.State", "State" },
            { "Vehicle.RegisteredState", "Registered State" },
            //{ "Vehicle.IsCoverageFireTheft", "Comprehensive / Fire & Theft" },
            //{ "Vehicle.IsCoverageCollision", "Collision Coverage" },
            { "Vehicle.HasCoverageCargo", "Cargo Coverage" },
            { "Vehicle.HasCoverageRefrigeration", "Refrigeration Coverage" },
            { "Vehicle.GaragingAddressId", "Garaging Address" },
            { "Vehicle.PrimaryOperatingZipCode", "Primary Operating Zip Code" },
            { "Vehicle.CoverageCollisionDeductibleId", "Collision Deductible"},
            { "Vehicle.CoverageFireTheftDeductibleId", "Comprehensive / Fire & Theft Deductible"},
            { "Vehicle.GrossVehicleWeightId", "Gross Vehicle Weight"},
            { "Vehicle.GrossVehicleWeightCategory", "GVW Category"},
            { "Vehicle.StatedAmount", "Stated Amount"},
            { "Vehicle.UseClassId", "Use Class"},
            { "Vehicle.VehicleBusinessClassId", "Vehicle Business Class"},
            //{ "Vehicle.AutoLiabilityPremium", "Auto Liability Premium"},
            //{ "Vehicle.CargoPremium", "Cargo Premium"},
            //{ "Vehicle.PhysicalDamagePremium", "Physical Damage Premium"},
            { "Vehicle.Radius", "Radius"},
            { "Vehicle.RadiusTerritory", "Radius Territory"},
            //{ "Vehicle.TotalPremium", "Total Premium"},
            //{ "Vehicle.CollisionPremium", "Collision Premium"},
            //{ "Vehicle.ComprehensivePremium", "Comprehensive / Fire & Theft Premium"},
            //{ "Vehicle.RefrigerationPremium", "Refrigeration Premium"},
            // driver header
            { "DriverHeader.mvrHeaderDate", "MVR Value Date" },
            // drivers
            { "Entity.FirstName", "First Name" },
            { "Entity.LastName", "Last Name" },
            { "Entity.MiddleName", "Middle Name" },
            { "Entity.BirthDate", "Date of Birth" },
            { "Driver.LicenseNumber", "License Number" },
            { "Driver.YrsDrivingExp", "Yrs. Driving Experience" },
            { "Driver.YrsCommDrivingExp", "Years Commerical Driving Experience" },
            //{ "Driver.IsMvr", "Is MVR?" },
            { "Driver.MvrDate", "MVR Value Date" },
            { "Driver.HireDate", "Hire Date" },
            { "Driver.State", "State" },
            //{ "Driver.IsOutOfState", "Is Out of State Driver?" },
            //{ "Driver.isExcludeKO", "Exclude from KO Rules?" },
            { "Driver.koReason", "Exclude from KO Rules Reasoning" },
            { "Driver.TotalFactor", "Total Factor" },
            { "Driver.YearPoint", "5 Year Point" },
             // { "Driver.Options", "Pass Rating?" },
            { "DriverIncidents.IncidentType", "Incident Type" },
            { "DriverIncidents.IncidentDate", "Incident Date" },
            { "DriverIncidents.ConvictionDate", "Conviction Date" },
            // manuscripts
            { "RiskManuscript.Title", "Title" },
            { "RiskManuscript.Description", "Description" },
            { "RiskManuscript.Premium", "Premium" },
            { "RiskManuscript.IsProrate", "IsProrated" },
            { "RiskManuscript.PremiumType", "Premium Type" },
            // exclusion radius endorsement
            { "ExclusionRadiusDTO.Radius", "Radius" },
            // territoryExclusion
            { "TerritoryExclusionDTO.States", "States" },
            // driver requirements
            { "DriverRequirementDTO.Requirement", "Requirement" },
            // hired physical damage
            { "HiredPhysdamDTO.EstimatedAdditionalPremium", "Estimated Additional Premium" },
            { "HiredPhysdamDTO.VehicleDays", "Vehicle Days" },
            { "HiredPhysdamDTO.RatePerDay", "Rate Per Day" },
            { "HiredPhysdamDTO.GrossCombinedWeight", "Gross Combined Weight" },
            { "HiredPhysdamDTO.AccidentLimitPerVehicle", "Accident Limit Per Vehicle" },
            { "HiredPhysdamDTO.Deductible", "Deductible" },
            { "HiredPhysdamDTO.PhysdamLimit", "Physdam Limit" },
            // trailer interchange coverage
            { "TrailerInterchangeDTO.CompreLimitOfInsurance", "Comprehensive Limit of Insurance" },
            { "TrailerInterchangeDTO.CompreDeductible", "Comprehensive Deductible" },
            { "TrailerInterchangeDTO.ComprePremium", "Comprehensive Premium" },
            { "TrailerInterchangeDTO.LossLimitOfInsurance", "Loss Limit of Insurance" },
            { "TrailerInterchangeDTO.LossDeductible", "Loss Deductible" },
            { "TrailerInterchangeDTO.LossPremium", "Loss Premium" },
            { "TrailerInterchangeDTO.CollisionLimitOfInsurance", "Collision Limit of Insurance" },
            { "TrailerInterchangeDTO.CollisionDeductible", "Collision Deductible" },
            { "TrailerInterchangeDTO.CollisionPremium", "Collision Premium" },
            { "TrailerInterchangeDTO.FireLimitOfInsurance", "Fire Limit of Insurance" },
            { "TrailerInterchangeDTO.FireDeductible", "Fire Deductible" },
            { "TrailerInterchangeDTO.FirePremium", "Fire Premium" },
            { "TrailerInterchangeDTO.FireTheftLimitOfInsurance", "Fire Theft Limit of Insurance" },
            { "TrailerInterchangeDTO.FireTheftDeductible", "Fire Theft Deductible" },
            { "TrailerInterchangeDTO.FireTheftPremium", "Fire Theft Premium" },
            //MCS90
            { "Mcs90DTO.UsDotNumber", "USDOT Number" },
            { "Mcs90DTO.DateReceived", "Date Received" },
            { "Mcs90DTO.IssuedTo", "Issued To" },
            { "Mcs90DTO.State", "State" },
            { "Mcs90DTO.DatedAt", "Date at" },
            { "Mcs90DTO.PolicyDateDay", "Day" },
            { "Mcs90DTO.PolicyDateMonth", "Month" },
            { "Mcs90DTO.PolicyDateYear", "Year" },
            { "Mcs90DTO.PolicyNumber", "Policy Number" },
            { "Mcs90DTO.EffectiveDate", "Effective Date" },
            { "Mcs90DTO.InsuranceCompany", "Insurance Company" },
            { "Mcs90DTO.CounterSignedBy", "Counter Signed By" },
            { "Mcs90DTO.AccidentLimit", "Accident Limit" },
            // commodities -- N/A
            { "RiskSpecificCommoditiesHauled.Commodity", "Commodity" },
            { "RiskSpecificCommoditiesHauled.PercentageOfCarry", "Percentage of Carry" },
            // additional interest
            { "Entity.FullName", "Name" },
            { "RiskAdditionalInterest.AdditionalInterestTypeId", "Entity Type" },
            { "RiskAdditionalInterest.EffectiveDate", "Effective Date" },
            { "RiskAdditionalInterest.ExpirationDate", "Expiration Date" },
            // Loss Payee
            { "FormLossPayeeDTO.DescriptionOfAutos", "Designation / Description of Autos" },
            // State PA
            { "FormPAaddcombFPBDTO.AddedFirstPartyBenefits", "Added First Party Benefits" },
            { "FormPAaddcombFPBDTO.MedicalExpenseBenefits", "Medical Expense Benefits" },
            { "FormPAaddcombFPBDTO.WorkLossBenefits", "Work Loss Benefits" },
            { "FormPAaddcombFPBDTO.FuneralExpenseBenefits", "Funeral Expense Benefits" },
            { "FormPAaddcombFPBDTO.AccidentalDeathBenefits", "Accidental Death Benefits" },
            { "FormPAaddcombFPBDTO.CombinationFirstPartyBenefits", "Combination First Party Benefits" },
            { "FormPAaddcombFPBDTO.MaximumTotalLimitAllBenefits", "Maximum Total Limit; All Benefits" },
            { "FormPAaddcombFPBDTO.CombFuneralExpenseBenefits", "Funeral Expense Benefits" },
            { "FormPAaddcombFPBDTO.CombAccidentalDeathBenefits", "Accidental Death Benefits" },
            //Covered Auto Designation Symbol
            { "CoveredAutoDesigSymbolDTO.CoveredAutoText", "Covered Auto Text" },
            { "CoveredAutoDesigSymbolDTO.TextForEndorsement", "Text For Endorsement" }
        };

        public static readonly Dictionary<string, string> ExcludedLabels = new Dictionary<string, string>
        {
            // vehicles
            { "Vehicle.IsCoverageFireTheft", "Comprehensive / Fire & Theft" },
            { "Vehicle.IsCoverageCollision", "Collision Coverage" },
            { "Vehicle.HasCoverageCargo", "Cargo Coverage" },
            { "Vehicle.HasCoverageRefrigeration", "Refrigeration Coverage" },
            { "Vehicle.AutoLiabilityPremium", "Auto Liability Premium"},
            { "Vehicle.CargoPremium", "Cargo Premium"},
            { "Vehicle.PhysicalDamagePremium", "Physical Damage Premium"},
            { "Vehicle.TotalPremium", "Total Premium"},
            { "Vehicle.CollisionPremium", "Collision Premium"},
            { "Vehicle.ComprehensivePremium", "Comprehensive / Fire & Theft Premium"},
            { "Vehicle.RefrigerationPremium", "Refrigeration Premium"},
            
            // drivers
            //{ "Driver.IsMvr", "Is MVR?" },
            { "Driver.IsOutOfState", "Is Out of State Driver?" },
            { "Driver.isExcludeKO", "Exclude from KO Rules?" },
        };
    }

    public class AdditionalInterestRates // Per Record
    {
        public const int ADDITIONAL_INSURED_PREMIUM = 50;
        public const int WAIVER_OF_SUBROGATION_PREMIUM = 250;
        public const int PRIMARY_AND_NONCONTRIBUTORY_PREMIUM = 500;
        public const int BLANKET_ADDITIONAL_INSURED_PREMIUM = 500;
        public const int BLANKET_WAIVER_OF_SUBROGATION_PREMIUM = 1000;
        public const int BLANKET_PRIMARY_AND_NONCONTRIBUTORY_PREMIUM = 1500;
    }
    public class GenerateDocumentSource
    {
        public const string InstallmentInvoice = "InstallmentInvoice";
        public const string DepositInvoice = "DepositInvoice";
        public const string NonPayNoticePendingCancellation = "NonPayNOPCCert";
        public const string RescindNonPayNoticePendingCancellation = "RescindNonPayNoPCReg";
        public const string Cancellation = "Cancellation";
        public const string FinalCancellation = "FinalCancellation";
        public const string Reinstatement = "ReinstatementNotice";
        public const string QuoteProposal = "QuoteProposal";
    }

    public class GenerateDocumentDescription
    {
        public const string InstallmentInvoice = "Deposit Invoice";
        public const string NonPayNoticePendingCancellation = "Notice of Pending Cancellation";
        public const string RescindNonPayNoticePendingCancellation = "RescindNonPayNoPCReg";
        public const string Cancellation = "Cancellation Notice";
        public const string FinalCancellation = "Final Cancellation";
        public const string Reinstatement = "Reinstatement Notice";
        public const string QuoteProposal = "QuoteProposal";
    }

    public class CompFtLimits
    {
        public const int NoCov = 76;
    }


    public class CollisionLimits
    {
        public const int NoCov = 65;
    }

    public class QuoteConditionOptionsConstants
    {
        public const int MileageLimitation = 1;
        public const int UnlistedDriverExclusion = 2;
        public const int OccupantHazardExclusion = 3;
        public const int DriverUnder23 = 4;
        public const int DriverCleanMVR = 5;
        public const int DriverOnlyHasOneViolation = 6;
        public const int SubjectToChange = 7;
        public const int BasedOnMVRProvided = 8;
        public const int DriverMustHaveTwoYearsExp = 9;
        public const int Other = 10;
        public const int ProducerNoAuthority = 11;
        public const int AttachedProposalValidity = 12;
        public const int PhysDamLimitOfInsurance = 13;
        public const int MinimumEarned = 14;
        public const int QuoteCoverage = 15;
        public const int SymbolTen = 23;
    }

    public class LvDatasetType 
    {
        public const string MainGVW = "MainGVW";
        public const string VehicleGVW = "VehicleGVW";
        public const string VehicleDescription = "VehicleDesc";
        public const string SecondaryBusinessGroup = "SecondaryBusGroup";
    }
}
