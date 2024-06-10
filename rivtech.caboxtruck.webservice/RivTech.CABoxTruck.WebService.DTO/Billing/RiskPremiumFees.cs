using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class RiskPremiumFees
    {
        private List<IAmount> _amounts = new List<IAmount>();

        public IEnumerable<IAmount> Amounts
        {
            get => _amounts.AsReadOnly();
        }

        public void Add(IAmount amount)
        {
            if (amount is null) return;
            _amounts.Add(amount);
        }

    }
}
