using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EndorsementPendingChangeDTO
    {
        public long Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public string AmountSubTypeId { get; set; }
        public decimal PremiumBeforeEndorsement { get; set; }
        public decimal PremiumAfterEndorsement { get; set; }
        public decimal PremiumDifference { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
