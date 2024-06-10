using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    /// <summary>
    /// An entity that contains all the information required for calculating taxes such as State Tax, Policy Effective Date, and Per Option Premium and Fees.
    /// </summary>
    public class RiskDetailsPremiumFees 
    {
        public RiskDetailsPremiumFees(
            StateTax stateTax, 
            DateTime effectiveDate, 
            IEnumerable<RiskPremiumFees> riskOptionAmounts
        ) {
            StateTax = stateTax;
            EffectiveDate = effectiveDate;
            RiskOptionAmounts = riskOptionAmounts;
        }

        public StateTax StateTax { get; }
        public DateTime EffectiveDate { get; }
        public IEnumerable<RiskPremiumFees> RiskOptionAmounts { get; }
    }
}
