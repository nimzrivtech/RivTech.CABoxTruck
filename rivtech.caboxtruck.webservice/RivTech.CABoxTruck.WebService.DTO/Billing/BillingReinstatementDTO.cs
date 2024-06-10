using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingReinstatementDTO
    {
        public Guid RiskId { get; set; }

        public TransactionDetailDTO[] TransactionDetails { get; set; }
        public string AppId { get; set; }
        public DateTime? PolicyEffectiveDate { get; set; }
        public bool IsFlatCancel { get; set; }
    }
}
