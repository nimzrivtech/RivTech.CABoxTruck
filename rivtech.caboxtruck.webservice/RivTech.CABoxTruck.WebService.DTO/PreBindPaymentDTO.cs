using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class PreBindPaymentDTO
    {
        public Guid Id { get; set; }
        public string InsuredName { get; set; }
        public string AccessCode { get; set; }
        public string PayeeName { get; set; }
        public string PayeeEmail { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string ConfirmationCode { get; set; }
    }

    public class PreBindPaymentInputDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
