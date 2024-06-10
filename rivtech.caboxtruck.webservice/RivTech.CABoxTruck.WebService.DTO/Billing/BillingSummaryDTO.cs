using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingSummaryDTO
    {
        public decimal TotalPremium { get; set; }
        public decimal Billed { get; set; }
        public decimal Paid { get; set; }
        public decimal ActiveTotalPaid { get; set; }
        public decimal Balance { get; set; }
        public string PaymentPlan { get; set; }
        public decimal PastDueAmount { get; set; }
        public DateTime? PastDueDate { get; set; }
        public decimal PayoffAmount { get; set; }
        public DateTime? EquityDate { get; set; }
        public decimal MinimumToBindRenewal { get; set; }
        public decimal PayoffRenewal { get; set; }
        public decimal PastDueAmountBeforeCancellation { get; set; }
        public decimal AmountNeededToReinstate { get; set; }
        public bool IsDepositFullyPaid { get; set; }
        public decimal? TotalCommissionWritten { get; set; }
        public decimal? TotalCommissionBilled { get; set; }
        public decimal? TotalCommissionPaid { get; set; }
        public decimal? TotalCommissionBalance { get; set; }
        public List<BillingSummaryDetails> BillingSummaryDetails { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}