using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Billing.Premiums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Taxes
{
    /// <summary>
    /// An Entity(Agregate Root) for tax computation and its breakdown.
    /// </summary>
    public class TaxDetailsBreakdownDTO
    {
        public Guid TaxDetailsId { get; set; }
        public string TaxAmountTypeId { get; set; }
        public string TaxAmountSubtypeId { get; set; }
        public decimal Amount { get; set; }
    }
}
