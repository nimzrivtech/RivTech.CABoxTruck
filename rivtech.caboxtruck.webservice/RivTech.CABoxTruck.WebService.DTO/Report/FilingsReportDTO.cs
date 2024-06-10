using System;

namespace RivTech.CABoxTruck.WebService.DTO.Report
{
    public class FilingsReportDTO
    {
        public string PolicyNumber { get; set; }
        public string InsuredName { get; set; }
        public string Filings { get; set; }
        public string MailingAddress { get; set; }
        public DateTime? PolicyEffectiveDate { get; set; }
        public DateTime? PolicyExpirationDate { get; set; }
        public Decimal? PastDueAmount { get; set; }
        public string PastDueDate { get; set; }
        public string PaymentPlan { get; set; }
        public string AgentName { get; set; }
        public string AgentPhoneNumber { get; set; }
        public string AgentEmailAddress { get; set; }
        public string AgencyAddress { get; set; }
        public string SubAgencyName { get; set; }
        public string AgencyName { get; set; }
        public string UnderwriterName { get; set; }
        public string Carrier { get; set; }
    }
}
