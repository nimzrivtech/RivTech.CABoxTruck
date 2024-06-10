using Newtonsoft.Json;
using System;

namespace RivTech.CABoxTruck.WebService.Common.ReportTemplates
{
    public class DMVReportTemplate
    {
        [JsonProperty("*Error Correction 'E' if Yes else blank")]
        public string ErrorCorrection { get; set; }
        [JsonProperty("*Primary NAIC number 5/A")]
        public string NaicNumber { get; set; }
        [JsonProperty("*MGU/MGA Code 10/A See BU List")]
        public string GACode { get; set; }
        [JsonProperty("*Report TO State 2/A")]
        public string VehicleState { get; set; }
        [JsonProperty("Agent 10/A See BR List")]
        public string Agent { get; set; }
        [JsonProperty("ID Type F = FEIN S = SSN D = DL Insured Id Number 1/A")]
        public string IdType { get; set; }
        [JsonProperty("Insured ID Number 20/A")]
        public string FederalIDNumber { get; set; }
        [JsonProperty("ID State 2/A")]
        public string IDState { get; set; }
        [JsonProperty("Insured Date of Birth  CCYY-MM-DD  10/N")]
        public string InsuredDateOfBirth { get; set; }
        [JsonProperty("Insured First Name 30/A")]
        public string InsuredFirstName { get; set; }
        [JsonProperty("Insured Middle Name 25/A")]
        public string InsuredMiddleName { get; set; }
        [JsonProperty("*Last name or Org Name 35A")]
        public string CompanyName { get; set; }
        [JsonProperty("*Insured Gender 1/A")]
        public string InsuredGender { get; set; }
        [JsonProperty("*Insured  Address 40/A")]
        public string StreetAddress1 { get; set; }
        [JsonProperty("*Ins City 30/A")]
        public string City { get; set; }
        [JsonProperty("*Ins State  2/A")]
        public string State { get; set; }
        [JsonProperty("* Insured Zip Code 5/N")]
        public string ZipCode { get; set; }
        [JsonProperty("Insured Zip Code 4/N")]
        public string ZipCode4N { get; set; }
        [JsonProperty("Garage Address 40/A")]
        public string GarageAddress { get; set; }
        [JsonProperty("Garage City 30/A")]
        public string GarageCity { get; set; }
        [JsonProperty("Garage State 2/A")]
        public string GarageState { get; set; }
        [JsonProperty("Gar Zip 5/N")]
        public string GarageZip5N { get; set; }
        [JsonProperty("Gar Zip 4/N")]
        public string GarageZip4N { get; set; }
        [JsonProperty("*Policy No 20/A")]
        public string PolicyNumber { get; set; }
        [JsonProperty("*Policy Type: 1 = Personal 2 = Commercial  1/N")]
        public string PolicyType { get; set; }
        [JsonProperty("*Policy Effective date CCYY-MM-DD 10/N")]
        public string EffectiveDate { get; set; }
        [JsonProperty("* Policy Expiration date CCYY-MM-DD 10/N")]
        public string ExpirationDate { get; set; }
        [JsonProperty("*Transaction Effective Date CCYY-MM-DD 10/N")]
        public string TransactionEffectiveDate { get; set; }
        [JsonProperty("* Issuance Date")]
        public string IssuanceDate { get; set; }
        [JsonProperty("*Vehicle ID (VIN) 20/A")]
        public string VIN { get; set; }
        [JsonProperty("*Veh Year CCYY 4/N")]
        public string Year { get; set; }
        [JsonProperty("Veh License Plate 20/A")]
        public string LicensePlate { get; set; }
        [JsonProperty("Veh Tran Code* 3/A See List")]
        public string TransactionCode { get; set; }
        [JsonProperty("* Vehicle Make: 15/A")]
        public string Make { get; set; }
        [JsonProperty("Vehicle Model 15/A")]
        public string Model { get; set; }
        [JsonProperty("*Vehicle Eff Date CCYY-MM-DD 10/N")]
        public string VehicleEffectiveDate { get; set; }
        [JsonProperty("*Vehicle Expiration Date CCYY-MM-DD 10/N")]
        public string VehicleExpirationDate { get; set; }
        [JsonProperty("*For Hire Indicator: Y = Yes N=No")]
        public string HireIndicator { get; set; }
        [JsonProperty("For Hire Cancellation Reasons: 5=Tags Surrendered 6=Superseding Coverage 7=Replacement Vehicle")]
        public string HireCancellationReasons { get; set; }
        [JsonProperty("Deductible1(Collision)")]
        public string? DeductibleCollision { get; set; }
        [JsonProperty("Deductible2(Comprehensive)")]
        public string DeductibleComprehensive { get; set; }
        [JsonProperty("CoverageType_1")]
        public string CoverageType1 { get; set; }
        [JsonProperty("IndividualLimit_1")]
        public string IndividualLimit1 { get; set; }
        [JsonProperty("OccurrenceLimit_1")]
        public string OccurrenceLimit1 { get; set; }
        [JsonProperty("CSLLimit_1")]
        public string CSLLimit1 { get; set; }
        [JsonProperty("CoverageType_2")]
        public string CoverageType2 { get; set; }
        [JsonProperty("IndividualLimit_2")]
        public string IndividualLimit2 { get; set; }
        [JsonProperty("OccurrenceLimit_2")]
        public string OccurrenceLimit2 { get; set; }
        [JsonProperty("CSLLimit_2")]
        public string CSLLimit2 { get; set; }
        [JsonProperty("CoverageType_3")]
        public string CoverageType3 { get; set; }
        [JsonProperty("IndividualLimit_3")]
        public string IndividualLimit3 { get; set; }
        [JsonProperty("OccurrenceLimit_3")]
        public string OccurrenceLimit3 { get; set; }
        [JsonProperty("CSLLimit_3")]
        public string CSLLimit3 { get; set; }
        [JsonProperty("PriorPolicyNumber")]
        public string PriorPolicyNumber { get; set; }
    }
}
