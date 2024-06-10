using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class InstallmentAndInvoiceDTO
    {
        public Guid? InvoiceId { get; set; }
        public string Status { get; set; }
        public string InstallmentType { get; set; }
        public decimal Premium { get; set; }
        public decimal Tax { get; set; }
        public decimal Fee { get; set; }
        public decimal TotalBilled { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalDue { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal FutureInstallmentFee { get; set; }
        public decimal TotalDueWithoutFutureInstallmentFee { get; set; }
    }
}
