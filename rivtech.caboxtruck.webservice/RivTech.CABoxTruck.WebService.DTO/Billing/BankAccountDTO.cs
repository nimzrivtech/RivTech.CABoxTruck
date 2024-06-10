using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BankAccountDTO
    {
        public string AccountType { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string NameOnAccount { get; set; }
    }
}
