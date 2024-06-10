using System;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class DMVReportDTO
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
        public string VIN { get; set; }
        public string Year { get; set; }
        public string TransactionCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime VehicleEffectiveDate { get; set; }
        public DateTime VehicleExpirationDate { get; set; }
    }
}
