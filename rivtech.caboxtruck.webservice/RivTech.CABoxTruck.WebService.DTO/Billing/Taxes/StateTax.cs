using RivTech.CABoxTruck.WebService.DTO.Billing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Taxes
{
    public class StateTax
    {
        public StateTax(string stateCode, IEnumerable<SurplusLineTax> slTaxes, StampingFeeTax stampingFee = null, string stampingFeeTaxType = null)
        {
            StateCode = stateCode;
            SLTaxes = slTaxes;
            StampingFeeTax = stampingFee;
            StampingFeeTaxSubType = stampingFeeTaxType ?? StampingFeeTaxType.Percentage.Id;
        }

        public string StateCode { get; }
        public IEnumerable<SurplusLineTax> SLTaxes { get; }
        public StampingFeeTax StampingFeeTax { get; }
        public string StampingFeeTaxSubType { get; set; } 
        //public decimal TotalRate => SLTaxes.Sum(x => x.Rate) + (StampingFeeTax?.Rate ?? 0);
        public decimal TotalRate
        {
            set { }
            get => SLTaxes.Sum(x => x.Rate) + 
                (StampingFeeTaxSubType == StampingFeeTaxType.Percentage.Id ? StampingFeeTax?.Rate ?? 0 : 0);
        }

    }
}
