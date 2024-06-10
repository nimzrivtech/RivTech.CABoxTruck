using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntitiesTest.Premiums
{
    [TestClass]
    public class RiskPolicyPremiumCollectionTest
    {
        [TestMethod]
        public void InitTest_MustHaveCorrectOrder()
        {
            var policyPeriod = new DateRange("6/1/2022", "6/1/2023");
            var riskDetailId = Guid.NewGuid();
            var items = new RiskPolicyPremium[]
            {
                new RiskPolicyPremium()
                {
                    Id = Guid.NewGuid(),
                    CoveragePremium = PolicyCoveragePremium.CreateNew(new DateRange("6/30/2022", "6/1/2023"), policyPeriod, new Premium(1000)),
                    PremiumType = Premium.TYPE_POLICY_GL,
                    RiskDetailId =riskDetailId
                },
                new RiskPolicyPremium()
                {
                    Id = Guid.NewGuid(),
                    CoveragePremium = PolicyCoveragePremium.CreateNew(new DateRange("6/1/2022", "6/30/2022"), policyPeriod, new Premium(2000)),
                    PremiumType = Premium.TYPE_POLICY_GL,
                    RiskDetailId =riskDetailId
                },
            };

            var collection = new RiskPolicyPremiumCollection(items, policyPeriod);
            Assert.AreEqual(DateTime.Parse("6/30/2022"), collection.RiskPremiums.Peek().EffectiveDate);
        }
    }
}
