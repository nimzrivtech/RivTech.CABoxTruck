using System;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class PremiumEarnedTaxReportDTO
    {
        public string PolicyNumber { get; set; }
        public string TransactionType { get; set; }
        public string EndorsementNumber { get; set; }
        public int TransactionNumber { get; set; }
        public string CoverageType { get; set; }
        public string InsuredName { get; set; }
        public string RiskState { get; set; }
        public decimal PremiumOfTransaction { get; set; }
        public decimal FeesForTransaction { get; set; }
        public decimal Taxable { get; set; }
        public decimal TaxSurplus { get; set; }
        public decimal StampingFee { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TIVStatedAmount { get; set; }
        public DateTime TransactionEffectiveDate { get; set; }
        public DateTime PolicyInceptionDate { get; set; }
        public DateTime PolicyExpirationDate { get; set; }
        public DateTime ProcessDate { get; set; }
        public string BusinessDescription { get; set; }
        public string InsuredAddress { get; set; }
        public string InsuredCity { get; set; }
        public string InsuredState { get; set; }
        public string InsuredZipCode { get; set; }
        public string InsuredCounty { get; set; }
        public string LiabilityClaimLimit { get; set; }
        public string PropertyDamageLimit { get; set; }
        public string BrokerName { get; set; }
        public string Broker_Billing_Address { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingStateCode { get; set; }
        public string BillingZipcode { get; set; }
        public string Broker_Business_Address { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessCity { get; set; }
        public string BusinessStateCode { get; set; }
        public string BusinessZipcode { get; set; }
    }
}
