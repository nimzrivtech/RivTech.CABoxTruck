using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class PolicyHistoryDetails : BaseEntity<Guid>
    {
        public Guid RiskDetailId { get; set; }
        public string Section { get; set; }
        public string ValueBefore { get; set; } // json DTO
        public string ValueAfter { get; set; } // json DTO
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; } // issued / drafted
        public int IsDiscarded { get; set; } // 1 - discarded or 0 - default

        // initial value will be from submission
    }
}
