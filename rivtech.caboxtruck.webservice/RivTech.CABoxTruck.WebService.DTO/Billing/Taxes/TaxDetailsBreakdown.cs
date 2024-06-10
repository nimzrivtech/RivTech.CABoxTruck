using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Taxes
{
    public class TaxDetailsBreakdown
    {
        public static TaxDetailsBreakdown FromSLTax(string taxType, string slTaxKind, decimal amount)
        {
            return new TaxDetailsBreakdown
            {
                TaxAmountTypeId = taxType,
                TaxAmountSubtypeId = slTaxKind,
                Amount = amount
            };
        }

        public static TaxDetailsBreakdown FromStampingFee(string id, decimal amount)
        {
            return new TaxDetailsBreakdown
            {
                TaxAmountTypeId = id,
                Amount = amount
            };
        }

        private TaxDetailsBreakdown() { }

        public string TaxAmountTypeId { get; private set; }
        public string TaxAmountSubtypeId { get; private set; }
        public decimal Amount { get; private set; }
    }
}
