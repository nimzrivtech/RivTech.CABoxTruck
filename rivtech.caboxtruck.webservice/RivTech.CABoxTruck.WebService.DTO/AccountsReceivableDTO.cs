using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AccountsReceivableDTO
    {
        public Guid? PolicyDetailId { get; set; }
        public string PolicyNumber { get; set; }
        public string Insured { get; set; }
        public string InsuredPhone { get; set; }
        public DateTime? Inception { get; set; }
        public DateTime? PendingCancellation { get; set; }
        public DateTime? CancellationDate { get; set; }
        public string Broker { get; set; }
        public string Financed { get; set; }
        public Decimal? PastDueAmount { get; set; }
        public DateTime? PastDueDate { get; set; }
        public DateTime? CurrentDueDate { get; set; }
        public Decimal? CurrentAmountDue { get; set; }
        public Decimal? PremiumDue { get; set; }
        public Decimal? TotalPayment { get; set; }
        public string NoPayments { get; set; }
    }

    public class AccountsReceivableInputDTO
    {
        public string SortNameBy { get; set; }
        public string ReceivableType { get; set; }
        public string Broker { get; set; }
        public bool PastDueOnly { get; set; }
        public string CancellationStatus { get; set; }
        public bool NoPaymentsMade { get; set; }
        public DateTime? ValuationDate { get; set; }

    }
}
