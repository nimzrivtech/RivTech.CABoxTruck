using System;

namespace RivTech.CABoxTruck.WebService.DTO.DocumentGeneration.Reports
{
    public class PolicyStatementReportDTO
    {
        public string UnderwriterCompany { get; set; }
        public string Insured { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyPeriod { get; set; }
        public decimal PastDueAmount { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal InvoiceTotal { get; set; }
        public PolicyStatementInvoiceDTO[] Invoices { get; set; }
        public decimal PaymentTotal { get; set; }
        public PolicyStatementPaymentDTO[] Payments { get; set; }

        public DateTime DateTime { get; set; }

        public class PolicyStatementInvoiceDTO
        {
            public string Number { get; set; }
            public DateTime DueDate { get; set; }
            public decimal Amount { get; set; }
            public bool IsVoided { get; set; }
        }

        public class PolicyStatementPaymentDTO
        {
            public string Number { get; set; }
            public DateTime PostDate { get; set; }
            public decimal Amount { get; set; }
            public bool IsVoided { get; set; }
        }
    }
}
