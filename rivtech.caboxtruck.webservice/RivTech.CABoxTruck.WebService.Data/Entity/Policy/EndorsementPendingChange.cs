using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Policy
{
    public class EndorsementPendingChange : BaseEntity<long>
    {
        public EndorsementPendingChange()
        {
            DateCreated = DateTime.Now;
        }

        public Guid RiskDetailId { get; set; }
        public string AmountSubTypeId { get; set; }
        public decimal? PremiumBeforeEndorsement { get; set; }
        public decimal? PremiumAfterEndorsement { get; set; }
        public decimal? PremiumDifference { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
