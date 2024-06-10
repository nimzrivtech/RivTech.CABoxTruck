using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DomainEntities.CommonValueObjects;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class RiskVehiclePremiumCollectionTest
    {
        [TestMethod]
        public void NewInstanceTest_OrderMustBeCorrect()
        {
            var policyPeriod = new DateRange("1/1/2022", "1/1/2023");
            var premiumCovs = new List<RiskVehiclePremium>
            {
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    PremiumCoverage = new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("3/1/2022", "7/1/2022"), policyPeriod, new PremiumCoverage(1700)),
                },
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    PremiumCoverage =new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("7/1/2022", "1/1/2023"),  policyPeriod,new PremiumCoverage(1900)),
                },
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    PremiumCoverage =new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("1/1/2022", "3/1/2022"),  policyPeriod,new PremiumCoverage(1500)),
                },
            };
            var collection = new RiskVehiclePremiumCollection(premiumCovs, policyPeriod);

            Assert.AreEqual(1900, collection.VehiclePremiums.Pop().PremiumCoverage.Premium.Annual);
            Assert.AreEqual(1700, collection.VehiclePremiums.Pop().PremiumCoverage.Premium.Annual);
            Assert.AreEqual(1500, collection.VehiclePremiums.Pop().PremiumCoverage.Premium.Annual);
        }


        [TestMethod]
        public void AddNew_WhenOutOfSequence_MustBeCorrectlyProrated()
        {
            var policyPeriod = new DateRange("1/1/2022", "1/1/2023");
            var premiumCovs = new List<RiskVehiclePremium>
            {
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    PremiumCoverage =new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("1/1/2022", "3/1/2022"),  policyPeriod,new PremiumCoverage(1500)),
                },
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    PremiumCoverage = new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("3/1/2022", "7/1/2022"), policyPeriod, new PremiumCoverage(1700)),
                },
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    PremiumCoverage =new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("7/1/2022", "1/1/2023"),  policyPeriod,new PremiumCoverage(1900)),
                },
            };
            var collection = new RiskVehiclePremiumCollection(premiumCovs, policyPeriod);

            collection.Add(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = Guid.NewGuid(),
                PremiumCoverage = new VehiclePremiumCoverage(Guid.NewGuid(), new DateRange("2/1/2022", "1/1/2023"), policyPeriod, new PremiumCoverage(2000)),
            });

            Assert.AreEqual(1900, collection.VehiclePremiums.Pop().PremiumCoverage.Premium.Annual);
            Assert.AreEqual(1700, collection.VehiclePremiums.Pop().PremiumCoverage.Premium.Annual);
            var insertedItem = collection.VehiclePremiums.Pop();
            Assert.AreEqual(2000, insertedItem.PremiumCoverage.Premium.Annual);
            Assert.AreEqual(DateTime.Parse("3/1/2022"), insertedItem.PremiumCoverage.CoveragePeriod.End);
            Assert.AreEqual(1500, collection.VehiclePremiums.Pop().PremiumCoverage.Premium.Annual);
        }
    }
}
