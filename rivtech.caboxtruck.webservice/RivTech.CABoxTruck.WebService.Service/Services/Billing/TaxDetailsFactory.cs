using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.Billing
{
    public class TaxDetailsFactory : ITaxDetailsFactory
    {
        private readonly IFeeRepository _feeRepository;

        public TaxDetailsFactory(
            IFeeRepository feeRepository
        ) {
            _feeRepository = feeRepository;
        }

        public TaxDetails Create(RiskPremiumFees premiumFees, StateTax stateTax, DateTime policyEffectiveDate)
        {
            IEnumerable<FeeKind> nonTaxableFees = _feeRepository
                .GetNonTaxableTypesByState(stateTax.StateCode, policyEffectiveDate);

            return new TaxDetails(premiumFees, stateTax, nonTaxableFees);
        }
    }
}
