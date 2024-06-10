using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.DALDapperModels
{
    public class DMVReportModel
    {
        public string NaicNumber { get; set; }
        public string GACode { get; set; }
        public string VehicleState { get; set; }
        public string IdType { get; set; }
        public string FederalIDNumber { get; set; }
        public string CompanyName { get; set; }
        public string StreetAddress1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyStatus { get; set; }
        public string EndorsementNumber { get; set; }
        public string PolicyType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime TransactionEffectiveDate { get; set; }
        public DateTime IssuanceDate { get; set; }
        public string VIN { get; set; }
        public string Year { get; set; }
        public string TransactionCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime VehicleEffectiveDate { get; set; }
        public DateTime VehicleExpirationDate { get; set; }
        public string CoverageType1 { get; set; }
        public string IndividualLimit1 { get; set; }
        public string OccurrenceLimit1 { get; set; }
        public int CSLLimit1 { get; set; }
        public string CoverageType2 { get; set; }
        public string IndividualLimit2 { get; set; }
        public string OccurrenceLimit2 { get; set; }
        public string CSLLimit2 { get; set; }
        public string CoverageType3 { get; set; }
        public string IndividualLimit3 { get; set; }
        public string OccurrenceLimit3 { get; set; }
        public string CSLLimit3 { get; set; }
    }
}
