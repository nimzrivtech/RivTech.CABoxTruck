using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingFeeDTO
    {
        public Guid Id { get; set; }
        public DateTime AddDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime? VoidDate { get; set; }
    }
}
