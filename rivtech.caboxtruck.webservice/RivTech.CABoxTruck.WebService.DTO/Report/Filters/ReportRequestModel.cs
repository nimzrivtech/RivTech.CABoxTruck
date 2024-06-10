using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO.Report.Filters
{
    public class ReportRequestModel : FilterBase
    {
        public ReportManagementFilter PaymentRequest { get; set; }
        public bool? IsRaterV7 { get; set; }
    }
}
