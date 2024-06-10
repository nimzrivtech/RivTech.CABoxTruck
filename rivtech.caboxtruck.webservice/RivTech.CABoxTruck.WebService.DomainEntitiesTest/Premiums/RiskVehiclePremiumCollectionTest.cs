using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DomainEntitiesTest.Premiums
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
                    CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("3/1/2022", "7/1/2022"), policyPeriod, new Premium(1700)),
                    Order = 2
                },
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    CoveragePremium =new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("7/1/2022", "1/1/2023"),  policyPeriod,new Premium(1900)),
                    Order = 3
                },
                new RiskVehiclePremium{
                    RiskDetailId = Guid.NewGuid(),
                    VehicleMainId = Guid.NewGuid(),
                    CoveragePremium =new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("1/1/2022", "3/1/2022"),  policyPeriod,new Premium(1500)),
                    Order = 1
                },
            };
            var collection = new RiskVehiclePremiumCollection(premiumCovs, policyPeriod);

            Assert.AreEqual(1900, collection.RiskPremiums.Pop().CoveragePremium.Premium.Annual);
            Assert.AreEqual(1700, collection.RiskPremiums.Pop().CoveragePremium.Premium.Annual);
            Assert.AreEqual(1500, collection.RiskPremiums.Pop().CoveragePremium.Premium.Annual);
        }

        [TestMethod]
        public void AddOrUpdate_ShouldCreateNewCoverage_WhenEndorsementHasPremiumChanges()
        {
            var policyPeriod = new DateRange("1/1/2022", "1/1/2023");
            var vehicleMainId = Guid.NewGuid();
            var riskDetailId = Guid.NewGuid();
            var premiumCovs = new List<RiskVehiclePremium>
            {
                new RiskVehiclePremium{
                    RiskDetailId = riskDetailId,
                    VehicleMainId = vehicleMainId,
                    CoveragePremium =new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("1/1/2022", "1/1/2023"),  policyPeriod,new Premium(1500)),
                },
            };
            var collection = new RiskVehiclePremiumCollection(premiumCovs, policyPeriod);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = riskDetailId,
                VehicleMainId = vehicleMainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("2/1/2022", "1/1/2023"), policyPeriod, new Premium(2000)),
            });

            Assert.AreEqual(2, collection.RiskPremiums.Count);
            Assert.AreEqual(new DateRange("1/1/2022", "2/1/2022"), collection.RiskPremiums.Last().CoveragePremium.CoveragePeriod);
            Assert.AreEqual(new DateRange("2/1/2022", "1/1/2023"), collection.RiskPremiums.First().CoveragePremium.CoveragePeriod);
            Assert.AreEqual("2/1/2022", collection.RiskPremiums.Last().CoveragePremium.CoveragePeriod.End.ToString("M/d/yyyy"));
        }

        [TestMethod]
        public void AddOrUpdate_ShouldCreateNewCoverage_WhenEndorsementHasPremiumChangesWith2CoveragesAlready()
        {
            var policyPeriod = new DateRange("1/1/2022", "1/1/2023");
            var vehicleMainId = Guid.NewGuid();
            var riskDetailId = Guid.NewGuid();
            var premiumCovs = new List<RiskVehiclePremium>
            {
                new RiskVehiclePremium{
                    RiskDetailId = riskDetailId,
                    VehicleMainId = vehicleMainId,
                    CoveragePremium =new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("1/1/2022", "2/1/2022"),  policyPeriod,new Premium(1500)),
                },
                new RiskVehiclePremium{
                    RiskDetailId = riskDetailId,
                    VehicleMainId = vehicleMainId,
                    CoveragePremium =new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("2/1/2022", "3/1/2022"),  policyPeriod,new Premium(1500)),
                },
            };
            var collection = new RiskVehiclePremiumCollection(premiumCovs, policyPeriod);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = riskDetailId,
                VehicleMainId = vehicleMainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("3/1/2022", "1/1/2023"), policyPeriod, new Premium(2000)),
            });

            Assert.AreEqual(3, collection.RiskPremiums.Count);
            Assert.AreEqual(new DateRange("3/1/2022", "1/1/2023"), collection.RiskPremiums.ToArray()[0].CoveragePremium.CoveragePeriod);
            Assert.AreEqual(new DateRange("2/1/2022", "3/1/2022"), collection.RiskPremiums.ToArray()[1].CoveragePremium.CoveragePeriod);
            Assert.AreEqual(new DateRange("1/1/2022", "2/1/2022"), collection.RiskPremiums.ToArray()[2].CoveragePremium.CoveragePeriod);
        }

     
        [TestMethod]
        public void AddOrUpdate_ShouldNotCreateCoverage_WhenNoPremiumChangeOn2CoveragePeriods()
        {
            var policyPeriod = new DateRange("1/1/2022", "1/1/2023");
            var vehicleMainId = Guid.NewGuid();
            var riskDetailId = Guid.NewGuid();

            var premiumCovs = new List<RiskVehiclePremium>
            {
                new RiskVehiclePremium{
                    RiskDetailId = riskDetailId,
                    VehicleMainId = vehicleMainId,
                    CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("1/1/2022", "1/1/2023"), policyPeriod, new Premium(1500)),
                },
                new RiskVehiclePremium
                {
                    RiskDetailId = riskDetailId,
                    VehicleMainId = vehicleMainId,
                    CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("2/1/2022", "1/1/2023"), policyPeriod, new Premium(2000)),
                }
            };
            var collection = new RiskVehiclePremiumCollection(premiumCovs, policyPeriod);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = riskDetailId,
                VehicleMainId = vehicleMainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("2/1/2022", "1/1/2023"), policyPeriod, new Premium(2000)),
            });

            Assert.AreEqual(2, collection.RiskPremiums.Count);
        }



        [TestMethod]
        public void AddOrUpdate_WhenHasSameEffectiveAndPremium_MustBeAddedCorrectly()
        {
            var policyPeriod = new DateRange("5/11/2022", "5/11/2023");
            var collection = new RiskVehiclePremiumCollection(policyPeriod);
            var mainId = Guid.NewGuid();

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("5/11/2022", "6/15/2022"), policyPeriod, new Premium(10137)),
            });
            Assert.AreEqual(1, collection.RiskPremiums.Count);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("6/15/2022", "6/15/2022"), policyPeriod, new Premium(0)),
            });
            Assert.AreEqual(2, collection.RiskPremiums.Count);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("6/15/2022", "7/31/2022"), policyPeriod, new Premium(10137)),
            });
            Assert.AreEqual(3, collection.RiskPremiums.Count);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("7/31/2022", "5/11/2023"), policyPeriod, new Premium(11846)),
            });
            Assert.AreEqual(4, collection.RiskPremiums.Count);
        }

        [TestMethod]
        public void AddOrUpdate_WhenNewIsInSequence_MustBeAddedCorrectly()
        {
            var policyPeriod = new DateRange("6/1/2022", "6/1/2023");
            var collection = new RiskVehiclePremiumCollection(policyPeriod);
            var mainId = Guid.NewGuid();

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("6/1/2022", "6/1/2023"), policyPeriod, new Premium(10137)),
            });
            Assert.AreEqual(1, collection.RiskPremiums.Count);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("6/30/2022", "6/1/2023"), policyPeriod, new Premium(0)),
            });
            Assert.AreEqual(2, collection.RiskPremiums.Count);

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("7/31/2022", "6/1/2023"), policyPeriod, new Premium(10137)),
            });
            Assert.AreEqual(3, collection.RiskPremiums.Count);
            Assert.AreEqual(DateTime.Parse("7/31/2022"), collection.RiskPremiums.Peek().EffectiveDate);
            Assert.AreEqual(3, collection.RiskPremiums.Peek().Order);
        }

        [TestMethod]
        public void Cancel_WhenVehicleEffectiveIsLaterThanCancellation()
        {
            var policyPeriod = new DateRange("1/1/2022", "1/1/2023");
            var collection = new RiskVehiclePremiumCollection(policyPeriod);
            var mainId = Guid.NewGuid();

            collection.AddOrUpdate(new RiskVehiclePremium
            {
                RiskDetailId = Guid.NewGuid(),
                VehicleMainId = mainId,
                CoveragePremium = new VehicleCoveragePremium(It.IsAny<int>(), new DateRange("2/1/2022", "1/1/2023"), policyPeriod, new Premium(10137)),
            });
            Assert.AreEqual(1, collection.RiskPremiums.Count);

            collection.Cancel(DateTime.Parse("1/15/2022"));
            Assert.AreEqual(1, collection.RiskPremiums.Count);
            Assert.AreEqual(DateTime.Parse("1/15/2022"), collection.RiskPremiums.Peek().ExpirationDate);
        }
    }
}
