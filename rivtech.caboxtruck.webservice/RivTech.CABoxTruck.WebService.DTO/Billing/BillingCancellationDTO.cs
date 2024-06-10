using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingCancellationDTO : InvoiceDTO
    {
        public TransactionDetailDTO[] TransactionDetails { get; set; }
        public bool IsFlatCancel { get; set; }
        public string AppId { get; set; }
        public bool IsReasonNonPayment { get; set; }
        public bool IsManualCancellation { get; set; } = true;
        public DateTime EndorsementEffectiveDate { get; set; }
    }
}
