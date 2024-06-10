using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskStatusDTO
    {
        public Int32 RiskStatus { get; set; }
        public Int64 RiskDetailID { get; set; }
        public Int16 RiskStatusTypeID { get; set; }
        public DateTime ProcessedDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string ChangedBy { get; set; }
    }
}
