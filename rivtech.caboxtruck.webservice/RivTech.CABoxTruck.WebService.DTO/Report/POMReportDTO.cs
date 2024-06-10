using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class POMReportDTO
    {
        public int Count { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuredName { get; set; }
        public string MailingAddress { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CancellationDate { get; set; }
        public string NoticeType { get; set; }
        public decimal? Postage { get; set; }
        public decimal? Fee { get; set; }
        //public int TotalNotices { get; set; }
    }
}
