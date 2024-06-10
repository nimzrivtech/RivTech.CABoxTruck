using System;

namespace RivTech.CABoxTruck.Billing.Application.DTO.Views
{
    public class PaymentDetailsViewDto
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PostDate { get; set; }
        public string PostedBy { get; set; }
        public decimal Premium { get; set; }
        public decimal Tax { get; set; }
        public decimal Fee { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
        public string ReversalType { get; set; }
        public DateTime? ReversalDate { get; set; }
        public string ReversalProcessedBy { get; set; }
        public string Comments { get; set; }
        public DateTime? ClearDate { get; set; }
        public DateTime? EscheatDate { get; set; }
    }
}
