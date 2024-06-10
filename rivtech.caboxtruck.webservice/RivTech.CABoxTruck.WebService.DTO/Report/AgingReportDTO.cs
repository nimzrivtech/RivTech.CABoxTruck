using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class AgingReportDTO
    {
        public Guid RiskId { get; set; }
        public string? PolicyNumber { get; set; }
        public string PolicyInception { get; set; }
        public string? Insured { get; set; }
        public string? InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public decimal PastDueAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string DueDate { get; set; }
        public decimal TotalAmountDue { get; set; }
        public string? Current { get; set; }
        public string? ZeroToThirty { get; set; }
        public string? ThirtyOneToSixty { get; set; }
        public string? SixtyOneToNinety { get; set; }
        public string? NinetyOnePlus { get; set; }
    }
}
