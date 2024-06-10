using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class PaymentOptionDetails
    {
        public Guid Id { get; }

        public PaymentOptionDetails(PolicyTotals policyTotals, decimal depositPremium, decimal taxRate, int installmentCount = 10, decimal taxFlatRate = 0m)
        {
            PolicyTotals = policyTotals;
            DepositPremium = depositPremium;
            TaxFlatRate = taxFlatRate;

            if (depositPremium > 0 && installmentCount > 0)
                InstallmentDetails = new InstallmentDetails(policyTotals.Premium, DepositPremium, installmentCount, taxRate);
        }

        public decimal DepositAmount => DepositPremium + PolicyTotals.Fee + PolicyTotals.Tax + TaxFlatRate;

        public decimal DepositPremium { get; }
        public InstallmentDetails InstallmentDetails { get; }
        public PolicyTotals PolicyTotals { get; }
        public decimal TotalWithInstallments => DepositAmount + InstallmentDetails?.TotalDue ?? 0;
        public decimal TaxFlatRate { get; set; } 
    }
}
