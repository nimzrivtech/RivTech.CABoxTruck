using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class PostPaymentViewDTO
    {
        public Guid RiskId { get; set; }
        public string Instrument { get; set; }
        public string Account { get; set; }
        public string PaymentPlan { get; set; }
        public string CreatedDate { get; set; }
        public PaymentDTO Payment { get; set; }

        public string PaymentReceiptEmailBody { get; set; }
    }
}
