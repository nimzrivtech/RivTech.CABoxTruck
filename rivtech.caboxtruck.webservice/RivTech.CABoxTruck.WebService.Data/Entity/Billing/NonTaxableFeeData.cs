using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Billing
{
    public class NonTaxableFeeData  
    {
        public string StateCode { get; private set; }
        public string FeeKindId { get; private set; }
        public DateTime EffectiveDate { get; private set; }
    }
}
