using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class PaymentDetailDTO
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string AmountTypeId { get; set; }
        public EnumerationDTO AmountType { get; set; }
    }
}
