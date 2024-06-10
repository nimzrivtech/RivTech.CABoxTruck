using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class TransactionDetailDTO
    {
        public decimal Amount { get; set; }

        public AmountSubTypeDTO AmountSubType { get; set; }
        public string Description { get; set; }
    }
}
