using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DomainEntities.CommonValueObjects;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class VehiclePremiumCoverageTest
    {

        [TestMethod]
        public void Expire_MustChangeProratedPremium()
        {
            // Arrange
            var coveragePeriod = new DateRange(DateTime.Parse("1/1/2022"), DateTime.Parse("1/1/2023"));
            var policyPeriod = new DateRange(DateTime.Parse("1/1/2022"), DateTime.Parse("1/1/2023"));
            var premiumCoverage = VehiclePremiumCoverage.CreateNew(coveragePeriod, policyPeriod, new PremiumCoverage(1000));
            var initialProratedPremium = premiumCoverage.Premium.Prorated;

            // Act
            premiumCoverage.Expire(DateTime.Parse("6/1/2022"), policyPeriod);

            // Assert
            Assert.AreNotEqual(initialProratedPremium, premiumCoverage.Premium.Prorated);
            Assert.AreEqual(413.70m, premiumCoverage.Premium.Prorated);
        }
    }
}
