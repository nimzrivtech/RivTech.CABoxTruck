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
    public class TaxDetailsDTO
    {
        public List<TaxDetailsBreakdownDTO> TaxDetailsBreakdowns { get; set; }
        public decimal TotalSLTax { get; set; }
        public decimal TotalStampingFee { get; set; }
        public decimal TotalTax { get; set; }
    }
}
