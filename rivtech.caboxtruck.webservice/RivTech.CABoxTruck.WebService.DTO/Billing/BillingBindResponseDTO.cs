using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingBindResponseDTO
    {
        public Guid Id { get; set; }
        public Guid RiskId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal CurrentAmountInvoiced { get; set; }
        public decimal TotalAmountDue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? VoidDate { get; set; }
        public InvoiceDetailsDTO[] InvoiceDetails { get; set; }
        public decimal FutureInstallmentAmount { get; set; }
        public decimal PayOffAmount { get; set; }
        public decimal BrokerCommission { get; set; }
        public bool IsRecurringPayment { get; set; }
        public string Email { get; set; }
        public List<InstallmentAndInvoiceDTO> InstallmentSchedule { get; set; }
        public PostPaymentViewDTO Payment { get; set; }
    }
}
