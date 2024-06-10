using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Report.Filters
{
    public class ReportManagementFilter : FilterBase
    {
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public string ReportType { get; set; }
        public string ReportFileName { get; set; }
        public Guid RiskDetailId { get; set; }

        public bool? isRaterV7 { get; set; }
    }
}