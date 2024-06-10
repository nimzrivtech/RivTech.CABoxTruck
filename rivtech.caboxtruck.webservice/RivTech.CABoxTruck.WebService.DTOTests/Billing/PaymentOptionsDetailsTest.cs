using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DTO.Billing;

namespace RivTech.CABoxTruck.WebService.DTOTests.Billing
{
    [TestClass]
    public class PaymentOptionsDetailsTest
    {
        [TestMethod]
        public void DepositAmount_MustBeCorrect()
        {
            var policyTotals = new PolicyTotals(28921.00m, 400.00m, 1596.05m);
            var _sut = new PaymentOptionDetails(policyTotals, 4338m, 0);
            Assert.AreEqual(6334.05m, _sut.DepositAmount);
        }

        [TestMethod]
        public void TotalDueWithInstallments_MustBeCorrect()
        {
            // Test data 1
            var policyTotals = new PolicyTotals(28921.00m, 400.00m, 1596.05m);
            var _sut = new PaymentOptionDetails(policyTotals, 4338m, 0);
            Assert.AreEqual(31654.55m, _sut.TotalWithInstallments);

            // Test data 2
            policyTotals = new PolicyTotals(19557.00m, 400.00m, 977.85m);
            _sut = new PaymentOptionDetails(policyTotals, 3911.40m, 0, 5);
            Assert.AreEqual(21404.20m, _sut.TotalWithInstallments);
        }
    }
}
