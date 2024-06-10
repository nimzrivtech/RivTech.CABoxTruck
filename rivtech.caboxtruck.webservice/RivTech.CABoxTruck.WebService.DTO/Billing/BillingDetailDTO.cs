using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingDetailDTO
    {
        public CreditCardDTO CreditCard { get; set; }
        public BillingAddressDTO BillingAddress { get; set; }
        public BankAccountDTO BankAccount { get; set; }
    }
}
