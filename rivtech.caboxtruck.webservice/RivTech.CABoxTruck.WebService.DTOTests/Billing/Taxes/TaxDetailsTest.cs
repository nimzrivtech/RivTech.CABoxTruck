using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Billing.Premiums;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTOTests.Taxes
{
    [TestClass]
    public class TaxDetailsTest
    {
        TaxDetails _sut;

        private RiskPremiumFees CreateDefaultOptionAmounts()
        {
            var optionAmounts = new RiskPremiumFees();
            optionAmounts.Add(new RegularPremium(RegularPremiumKind.AutoLiability, 1000));
            optionAmounts.Add(new RegularPremium(RegularPremiumKind.PhysicalDamage, 1000));
            optionAmounts.Add(new RegularPremium(RegularPremiumKind.Cargo, 1000));

            optionAmounts.Add(new Fee(FeeKind.RiskManagementFeeAL, 200));
            optionAmounts.Add(new Fee(FeeKind.RiskManagementFeePD, 200));

            return optionAmounts;
        }

        private readonly StateTax TAX_AZ = new StateTax("AZ", new List<SurplusLineTax>
        {
            new SurplusLineTax(SLTaxKind.Main, 3.00m)
        }, new StampingFeePercentageTax(0.2m));

        private readonly StateTax TAX_NJ = new StateTax("NJ", new List<SurplusLineTax>
        {
            new SurplusLineTax(SLTaxKind.Main, 5.00m)
        });

        private readonly StateTax TAX_OR = new StateTax("OR", new List<SurplusLineTax>
        {
            new SurplusLineTax(SLTaxKind.Main, 2.00m),
            new SurplusLineTax(SLTaxKind.FireMarshal, 0.3m)
        }, new StampingFeeFlatRateTax(10m));

        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void Calculate_MustTaxFee_WhenFeeIsTaxable()
        {
            // Arrage
            _sut = new TaxDetails(CreateDefaultOptionAmounts(), TAX_AZ, null);

            // Act
            _sut.Calculate();

            // Assert
            var slMainTax = _sut.GetAmountByType(TaxType.SurplusLine.Id, SLTaxKind.Main.Id);
            Assert.AreEqual(102m, slMainTax.Amount);

            var stampingFee = _sut.GetAmountByType(TaxType.StampingFee.Id);
            Assert.AreEqual(6.8m, stampingFee.Amount);

            var totalTax = _sut.TotalTax;
            Assert.AreEqual(108.8m, totalTax);
        }

        [TestMethod]
        public void Calculate_MustNotTaxFee_WhenFeeIsNotTaxable()
        {
            // Arrange
            var nonTaxableFees = new List<FeeKind> {
                FeeKind.RiskManagementFeeAL,
                FeeKind.RiskManagementFeePD,
            };
            _sut = new TaxDetails(CreateDefaultOptionAmounts(), TAX_NJ, nonTaxableFees);

            // Act
            _sut.Calculate();

            // Assert
            var slTax = _sut.GetAmountByType(TaxType.SurplusLine.Id, SLTaxKind.Main.Id);
            Assert.AreEqual(150m, slTax.Amount);

            var stampingFee = _sut.GetAmountByType(TaxType.StampingFee.Id);
            Assert.AreEqual(null, stampingFee);

            var totalTax = _sut.TotalTax;
            Assert.AreEqual(150m, totalTax);
        }

        [TestMethod]
        public void Calculate_WithFlatStampingFee()
        {
            // Arrange
            var nonTaxableFees = new List<FeeKind> {
                FeeKind.RiskManagementFeeAL,
                FeeKind.RiskManagementFeePD,
            };
            _sut = new TaxDetails(CreateDefaultOptionAmounts(), TAX_OR, nonTaxableFees);

            // Act
            _sut.Calculate();

            // Assert
            var mainSlTax = _sut.GetAmountByType(TaxType.SurplusLine.Id, SLTaxKind.Main.Id);
            Assert.AreEqual(60m, mainSlTax.Amount);
            var fireMarshalSlTax = _sut.GetAmountByType(TaxType.SurplusLine.Id, SLTaxKind.FireMarshal.Id);
            Assert.AreEqual(9m, fireMarshalSlTax.Amount);

            var stampingFee = _sut.GetAmountByType(TaxType.StampingFee.Id);
            Assert.AreEqual(10m, stampingFee.Amount);

            var totalTax = _sut.TotalTax;
            Assert.AreEqual(79m, totalTax);
        }
    }
}
