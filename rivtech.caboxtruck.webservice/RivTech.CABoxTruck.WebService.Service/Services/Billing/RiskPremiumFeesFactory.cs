using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Billing.Premiums;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;

namespace RivTech.CABoxTruck.WebService.Service.Services.Billing
{
    public class RiskPremiumFeesFactory : IRiskPremiumFeesFactory
    {
        public RiskPremiumFees CreateFromCoverage(RiskCoverage riskCoverage)
        {
            var riskCovPremium = riskCoverage.RiskCoveragePremium;
            var riskPremiumFees = new RiskPremiumFees();

            if (riskCovPremium.AutoLiabilityPremium.HasValue)
                riskPremiumFees.Add(new RegularPremium(RegularPremiumKind.AutoLiability, riskCovPremium.AutoLiabilityPremium.Value));

            if (riskCovPremium.PhysicalDamagePremium > 0)
                riskPremiumFees.Add(new RegularPremium(RegularPremiumKind.PhysicalDamage, riskCovPremium.PhysicalDamagePremium.Value));

            if (riskCovPremium.CargoPremium > 0)
                riskPremiumFees.Add(new RegularPremium(RegularPremiumKind.Cargo, riskCovPremium.CargoPremium.Value));

            if (riskCovPremium.GeneralLiabilityPremium.HasValue)
                riskPremiumFees.Add(new AdditionalPremium(AdditionalPremiumKind.GeneralLiability, riskCovPremium.GeneralLiabilityPremium.Value));

            if (riskCovPremium.RiskMgrFeeAL.HasValue)
                riskPremiumFees.Add(new Fee(FeeKind.RiskManagementFeeAL, riskCovPremium.RiskMgrFeeAL.Value));

            if (riskCovPremium.RiskMgrFeePD.HasValue)
                riskPremiumFees.Add(new Fee(FeeKind.RiskManagementFeePD, riskCovPremium.RiskMgrFeePD.Value));

            if ((riskCovPremium.InstallmentFee ?? 0) > 0 && (riskCoverage.NumberOfInstallments ?? 0) > 0)
            {
                decimal totalInstallmentFee = riskCovPremium.InstallmentFee.Value * riskCoverage.NumberOfInstallments.Value;
                riskPremiumFees.Add(new Fee(FeeKind.InstallmentFee, totalInstallmentFee));
            }

            return riskPremiumFees;
        }
    }
}
