using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class FullyEarnedAmountTest
    {
        [TestMethod]
        public void CancelledAmount_MustBeSameWithAmount_WhenStillActiveDuringCancellationDate()
        {
            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");
            var effectivity = new DateRange("4/1/2021", "1/1/2022");
            var premium = new FullyEarnedAmount(new AmountInfo(It.IsAny<object>(), AmountKind.ManuscriptAL, It.IsAny<string>()), 100m);

            premium.Cancel(DateTime.Parse("5/1/2021"));
            Console.WriteLine(premium);

            Assert.AreEqual(100m, premium.CancelledAmount);
        }
    }
}
