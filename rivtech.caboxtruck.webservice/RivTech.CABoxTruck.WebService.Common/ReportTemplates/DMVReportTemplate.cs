using Newtonsoft.Json;
using System;

namespace RivTech.CABoxTruck.WebService.Common.ReportTemplates
{
    public class InvoiceDetailReportTemplate
    {
        [JsonProperty("Invoice Number")]
        public string InvoiceNumber { get; set; }
        [JsonProperty("Invoice Date")]
        public string InvoiceDate { get; set; }
        [JsonProperty("Due Date")]
        public string DueDate { get; set; }
        [JsonProperty("Amount Sub Type ID")]
        public string AmountSubTypeId { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Invoiced Amount")]
        public string InvoicedAmount { get; set; }
    }
}
