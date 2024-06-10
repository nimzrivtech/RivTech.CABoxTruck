using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Common.ReportTemplates
{
    public class POMReportTemplate
    {
        [JsonProperty("Count")]
        public int Count { get; set; }
        [JsonProperty("Policy #")]
        public string PolicyNumber { get; set; }
        [JsonProperty("Name")]
        public string InsuredName { get; set; }
        [JsonProperty("Address")]
        public string MailingAddress { get; set; }
        [JsonProperty("Trans Date")]
        public string TransactionDate { get; set; }
        [JsonProperty("Cancel Date")]
        public string CancellationDate { get; set; }
        [JsonProperty("Notice Type")]
        public string NoticeType { get; set; }
        [JsonProperty("Postage")]
        public string Postage { get; set; }
        [JsonProperty("Fee")]
        public string Fee { get; set; }
        //[JsonProperty("Total number of pieces listed by sender")]
        //public int TotalNotices { get; set; }
    }
}
