using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingBindDTO
    {
        public DateTime EffectiveDate { get; set; }
        public string InstrumentId { get; set; }
        public string paymentPlan { get; set; }
        public decimal alpCommission { get; set; }
        public decimal pdpCommission { get; set; }
        public BillingDetailDTO BillingDetail { get; set; }
        public TransactionDetailDTO[] transactionDetails { get; set; }
        public string AppId { get; set; }
        public decimal TotalAmountCharged { get; set; }
        public string TransactionId { get; set; }
        public bool WillPayLater { get; set; }
        public bool IsRecurringPayment { get; set; }
        public string Email { get; set; }
        public decimal PercentOfOutstanding { get; set; }
        public string StateCode { get; set; }
        public string PolicyNumber { get; set; }
        public decimal ARSCommission { get; set; }
    }
}
