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
    public class TaxDetails
    {
        private readonly List<TaxDetailsBreakdown> _taxDetailsBreakdowns = new List<TaxDetailsBreakdown>();

        public Guid Id { get; private set; }
        public RiskPremiumFees PremiumFees { get; }
        public StateTax StateTax { get; }
        public IEnumerable<FeeKind> NonTaxableFees { get; }

        public IEnumerable<TaxDetailsBreakdown> TaxDetailsBreakdowns => _taxDetailsBreakdowns.AsReadOnly();

        public decimal TotalSLTax => TaxDetailsBreakdowns
                                    .Where(x => x.TaxAmountTypeId == TaxType.SurplusLine.Id)
                                    .Sum(x => x.Amount);
        public decimal TotalStampingFee => TaxDetailsBreakdowns
                                    .FirstOrDefault(x => x.TaxAmountTypeId == TaxType.StampingFee.Id)
                                    ?.Amount ?? 0;

        public decimal TotalTax => TaxDetailsBreakdowns.Sum(x => x.Amount);

        public TaxDetails(RiskPremiumFees premiumFees, StateTax stateTax, IEnumerable<FeeKind> nonTaxableFees)
        {
            PremiumFees = premiumFees;
            StateTax = stateTax;
            NonTaxableFees = nonTaxableFees;
        }

        public TaxDetailsBreakdown GetAmountByType(string taxTypeId, string taxSubtypeId = null)
        {
            return TaxDetailsBreakdowns.FirstOrDefault(x => x.TaxAmountTypeId == taxTypeId && x.TaxAmountSubtypeId == taxSubtypeId);
        }

        public void Calculate()
        {
            decimal totalTaxableAmount = PremiumFees.Amounts
                // Where amount is Premium or Taxable fee.
                .Where(amount => amount is IPremium
                    || (amount is Fee fee && !NonTaxableFees.Any(ntf => ntf.Equals(fee.Kind))))
                // Total of amount value.
                .Sum(amount => amount.Value);

            foreach (SurplusLineTax slTax in StateTax.SLTaxes)
            {
                decimal amount = slTax.MultiplyRate(totalTaxableAmount);
                _taxDetailsBreakdowns.Add(TaxDetailsBreakdown.FromSLTax(TaxType.SurplusLine.Id, slTax.Kind.Id, amount));
            }

            if (StateTax.StampingFeeTax != null)
            {
                decimal amount = StateTax.StampingFeeTax.CalculateTaxAmount(totalTaxableAmount);
                _taxDetailsBreakdowns.Add(TaxDetailsBreakdown.FromStampingFee(TaxType.StampingFee.Id, amount));
            }
        }
    }
}
