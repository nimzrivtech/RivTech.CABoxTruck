using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class ProratedAmountPeriodTest
    {

        [TestMethod]
        public void ActualPremium_Test()
        {
            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");
            var premiumPeriod1 = new ProratedAmountPeriod(
                It.IsAny<object>(),
                1200,
                new DateRange("1/1/2021", "4/1/2021"),
                policyPeriod
            );

            Assert.AreEqual(295.89m, premiumPeriod1.ActualAmountRounded);


            var premiumPeriod2 = new ProratedAmountPeriod(
                It.IsAny<object>(),
                1400,
                new DateRange("4/1/2021", "1/1/2022"),
                policyPeriod
            );
            Assert.AreEqual(1054.79m, premiumPeriod2.ActualAmountRounded);
        }

    }
}
