using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Billing
{
    public interface ITaxDetailsFactory
    {
        TaxDetails Create(RiskPremiumFees premiumFees, StateTax stateTax, DateTime policyEffectiveDate);
    }
}
