using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingEndorsementDTO
    {
        public DateTime EndorsementEffectiveDate { get; set; }
        public Guid RiskId { get; set; }
        public TransactionDetailDTO[] TransactionDetails { get; set; }
        public string AppId { get; set; }
        public DateTime? PolicyEffectiveDate { get; set; }
    }
}
