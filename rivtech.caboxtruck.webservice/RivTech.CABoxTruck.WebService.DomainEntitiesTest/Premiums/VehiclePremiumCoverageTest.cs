using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;

namespace RivTech.CABoxTruck.WebService.DomainEntitiesTest.Endorsement.Proration
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
            var premiumCoverage = VehicleCoveragePremium.Builder
                .New(coveragePeriod)
                .SetAL(1000)
                .UpdateTotalAnnualPremium()
                .Prorate(policyPeriod)
                .Build();
            var initialProratedPremium = premiumCoverage.Premium.Prorated;

            // Act
            premiumCoverage.Expire(DateTime.Parse("6/1/2022"), policyPeriod);

            // Assert
            Assert.AreNotEqual(initialProratedPremium, premiumCoverage.Premium.Prorated);
            Assert.AreEqual(413.70m, premiumCoverage.Premium.Prorated);
        }
    }
}
