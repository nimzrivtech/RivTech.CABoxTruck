using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingFailedResponseDTO
    {
        public string ReferenceNumber { get; set; }
        public string Details { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}
