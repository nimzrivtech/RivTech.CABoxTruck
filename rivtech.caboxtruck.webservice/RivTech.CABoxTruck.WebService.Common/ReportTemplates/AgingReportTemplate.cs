using System;
using Newtonsoft.Json;

namespace RivTech.CABoxTruck.WebService.Common.ReportTemplates
{
    public class AgingReportTemplate
    {

        [JsonProperty("Policy Number")]
        public string PolicyNumber { get; set; }

        [JsonProperty("Policy Inception")]
        public string PolicyInception { get; set; }

        [JsonProperty("Insured")]
        public string Insured { get; set; }

        [JsonProperty("Invoice #")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("InvoiceDate")]
        public string InvoiceDate { get; set; }

        [JsonProperty("Past Due Amount")]
        public decimal PastDueAmount { get; set; }

        [JsonProperty("Due Date")]
        public string DueDate { get; set; }

        [JsonProperty("Total Amount Due")]
        public decimal TotalAmountDue { get; set; }

        [JsonProperty("Current")]
        public string Current { get; set; }

        [JsonProperty("0-30")]
        public string ZeroToThirty { get; set; }

        [JsonProperty("31-60")]
        public string ThirtyOneToSixty { get; set; }

        [JsonProperty("61-90")]
        public string SixtyOneToNinety { get; set; }

        [JsonProperty("91+")]
        public string NinetyOnePlus { get; set; }
    }
}
