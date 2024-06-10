using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Payments
{
    public class PaymentReceiptDTO
    {
        public string UnderwriterCompany { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyPeriod { get; set; }
        public string Insured { get; set; }
        public string BillingAddress { get; set; }
        public string Broker { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
