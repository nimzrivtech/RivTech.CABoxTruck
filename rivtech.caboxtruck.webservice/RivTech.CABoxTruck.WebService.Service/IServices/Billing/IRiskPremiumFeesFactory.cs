using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Billing;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Billing
{
    public interface IRiskPremiumFeesFactory
    {
        RiskPremiumFees CreateFromCoverage(RiskCoverage riskCoverage);
    }
}
