using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class InvoiceDetailsDTO
    {
        public Guid Id { get; set; }
        public AmountSubTypeDTO AmountSubType { get; set; }
        public string AmountSubTypeId { get; set; }
        public decimal InvoicedAmount { get; set; }
    }
}
