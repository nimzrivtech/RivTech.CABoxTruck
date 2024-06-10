using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskStatusHistory : BaseEntity<Guid>
    {
        public Guid RiskDetailId { get; set; }
       
        public int ChangedBy { get; set; }

        public DateTime ChangedDate { get; set; }

        public string ChangedField { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public RiskDetail RiskDetail { get; set; }

    }
}