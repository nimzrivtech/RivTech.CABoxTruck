using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Billing
{
    public class TaxDetailsData : BaseEntity<Guid>
    {
        public List<TaxDetailsBreakdownData> TaxDetailsBreakdowns { get; private set; }
        public decimal TotalSLTax { get; private set; }
        public decimal TotalStampingFee  { get; private set; }
        public decimal TotalTax { get; private set; }
    }
}
