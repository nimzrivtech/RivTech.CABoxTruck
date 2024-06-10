using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class InstallmentDetails
    {
        private const decimal INSTALLMENT_CHARGE = 0.03m;

        public InstallmentDetails(decimal totalPremium, decimal depositPremium, decimal installmentCount, decimal taxRate)
        {
            TotalPremiumDue = totalPremium.RoundTo2Decimals();
            RemainingPremium = totalPremium.RoundTo2Decimals() - depositPremium.RoundTo2Decimals();
            TaxRate = taxRate;
            InstallmentCount = installmentCount;
        }

        public decimal TaxRate { get; }
        public decimal InstallmentCount { get; }
        public decimal Fee => (TotalInstallmentFee / InstallmentCount).RoundTo2Decimals();
        public decimal RemainingPremium { get; private set; }
        public decimal Premium => (RemainingPremium / InstallmentCount).RoundTo2Decimals();
        public decimal Tax => ((TaxRate / 100) * Fee).RoundTo2Decimals();
        public decimal PerInstallmentDue => Premium + Fee + Tax;
        public decimal TotalFee => Fee * InstallmentCount;
        public decimal TotalDue => PerInstallmentDue * InstallmentCount;
        public decimal TotalPremiumDue { get; }
        public decimal TotalInstallmentFee => TotalPremiumDue switch // => (Premium * INSTALLMENT_CHARGE).RoundTo2Decimals(); // No value yet.
        {
            decimal totalPremiumDue when totalPremiumDue < 0 => 0,
            decimal totalPremiumDue when totalPremiumDue >= 0 && totalPremiumDue < 5000 => 77,
            decimal totalPremiumDue when totalPremiumDue >= 5000 && totalPremiumDue < 10000 => 232,
            decimal totalPremiumDue when totalPremiumDue >= 10000 && totalPremiumDue < 20000 => 465,
            decimal totalPremiumDue when totalPremiumDue >= 20000 && totalPremiumDue < 35000 => 852,
            decimal totalPremiumDue when totalPremiumDue >= 35000 && totalPremiumDue < 50000 => 1317,
            decimal totalPremiumDue when totalPremiumDue >= 50000 && totalPremiumDue < 100000 => 2325,
            decimal totalPremiumDue when totalPremiumDue >= 100000 && totalPremiumDue < 200000 => 4650,
            decimal totalPremiumDue when totalPremiumDue >= 200000 && totalPremiumDue < 300000 => 7750,
            decimal totalPremiumDue when totalPremiumDue >= 300000 && totalPremiumDue < 400000 => 10850,
            decimal totalPremiumDue when totalPremiumDue >= 400000 && totalPremiumDue < 500000 => 13950,
            decimal totalPremiumDue when totalPremiumDue >= 500000 && totalPremiumDue < 750000 => 19375,
            decimal totalPremiumDue when totalPremiumDue >= 750000 && totalPremiumDue < 1000000 => 27125,
            decimal totalPremiumDue when totalPremiumDue >= 1000000 => 30000,
            _ => 0
        };
    }
}
