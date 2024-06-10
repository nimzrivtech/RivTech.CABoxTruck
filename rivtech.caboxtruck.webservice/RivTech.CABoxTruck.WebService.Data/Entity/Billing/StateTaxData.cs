using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Billing
{
    public class StateTaxData
    {
        public string StateCode { get; set; }
        public string TaxType { get; set; }
        public string TaxSubtype { get; set; }
        public decimal Rate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
