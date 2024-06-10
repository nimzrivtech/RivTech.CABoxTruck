using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class InstallmentInvoiceBeforeBindRequestDTO
    {
        public DateTime EffectiveDate { get; set; }
        public string PaymentPlan { get; set; }
        public List<TransactionDetailDTO> TransactionDetails { get; set; }
        public string AppId { get; set; }
        public decimal PercentOfOutstanding { get; set; }
        public string StateCode { get; set; }
    }
}