using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Billing
{
    public class TaxDetailsBreakdownData : BaseEntity<Guid>
    {
        public Guid TaxDetailsId { get; private set; }
        public string TaxAmountTypeId { get; private set; }
        public string TaxAmountSubtypeId { get; private set; }
        public decimal Amount { get; private set; }
    }
}
