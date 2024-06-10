using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DTO.Billing;

namespace RivTech.CABoxTruck.WebService.DTOTests.Billing
{
    [TestClass]
    public class InstallmentDetailsTest
    {
        InstallmentDetails _sut;   

        [TestInitialize]
        public void Init()
        {
            _sut = new InstallmentDetails(28921.00m, 4338m, 10, 0);
        }

        [TestMethod]
        public void Premium_MustBeCorrect()
        {
            Assert.AreEqual(2458.3m, _sut.Premium);
        }

        [TestMethod]
        public void Fee_MustBeCorrect()
        {
            Assert.AreEqual(73.75m, _sut.Fee);
        }

        [TestMethod]
        public void Tax_MustBeCorrect()
        {
            Assert.AreEqual(0m, _sut.Tax);
        }


        [TestMethod]
        public void PerInstallmentDue_MustBeCorrect()
        {
            Assert.AreEqual(2532.05m, _sut.PerInstallmentDue);
        }

        [TestMethod]
        public void TotalDue_MustBeCorrect()
        {
            Assert.AreEqual(25320.5m, _sut.TotalDue);
        }
    }
}
