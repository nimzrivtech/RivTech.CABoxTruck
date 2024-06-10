using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class CreditCardDTO
    {
        public string CardCode { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
    }
}
