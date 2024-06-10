using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class ProratedAmountTest
    {

        [TestMethod]
        public void Amount_Test()
        {
            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod);
            var premiumPeriod2 = new ProratedAmountPeriod(It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<object>(), AmountKind.VehicleAL, It.IsAny<string>()), periods, policyPeriod);
            Console.WriteLine(proratedPremium);
            Assert.AreEqual(1350.68m, proratedPremium.AmountRounded);
        }

        [TestMethod]
        public void CancelledAmount_Test()
        {
            #region Test 1

            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod);
            var premiumPeriod2 = new ProratedAmountPeriod(It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()),
                periods, policyPeriod);
            proratedPremium.Cancel(DateTime.Parse("5/1/2021"));

            Console.WriteLine(proratedPremium);

            Assert.AreEqual(410.96m, proratedPremium.CancelledAmountRounded);

            #endregion


            #region Test 2

            premiumPeriod2 = new ProratedAmountPeriod(
                It.IsAny<object>(),
                1400,
                new DateRange("4/1/2021", "7/1/2021"),
                policyPeriod
            );
            var premiumPeriod3 = new ProratedAmountPeriod(
                It.IsAny<object>(),
                1400,
                new DateRange("7/1/2021", "1/1/2022"),
                policyPeriod
            );

            periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2,
                premiumPeriod3,
            };
            proratedPremium = ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()), periods, policyPeriod);
            proratedPremium.Cancel(DateTime.Parse("8/1/2021"));

            Console.WriteLine(proratedPremium);

            Assert.AreEqual(763.84m, proratedPremium.CancelledAmountRounded);

            #endregion 
        }


        [TestMethod]
        public void CancelledAmount_FromEndorsement_Test()
        {
            var policyPeriod = new DateRange("11/30/2021", "11/30/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1000, new DateRange("2021-12-07", "2022-11-30"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()),
                periods, policyPeriod);
            proratedPremium.Cancel(DateTime.Parse("2021-12-30"));

            Console.WriteLine(proratedPremium);

            Assert.AreEqual(63.01m, proratedPremium.CancelledAmountRounded);
        }

        [TestMethod]
        public void WhenNotCancelled_CancelledAmount_Is0()
        {
            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod);
            var premiumPeriod2 = new ProratedAmountPeriod(It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<object>(), AmountKind.VehicleAL, It.IsAny<string>()), periods, policyPeriod);

            Console.WriteLine(proratedPremium);
            Assert.AreEqual(0.00m, proratedPremium.CancelledAmountRounded);
        }

        [TestMethod]
        public void MinimumPremiumAdjustment_MustBeZero_WhenCancelledAmountIsHigherThanMinPrem()
        {

            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod, fromSubmission: true);
            var premiumPeriod2 = new ProratedAmountPeriod(It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()), periods, policyPeriod);
            proratedPremium.MinimumPercentage = 35;
            proratedPremium.Cancel(DateTime.Parse("7/1/2021"));

            Console.WriteLine(proratedPremium);

            Assert.AreEqual(420.00m, proratedPremium.MinimumEarned);
            Assert.AreEqual(0m, proratedPremium.MinimumEarnedAdjustmentRounded);
        }

        [TestMethod]
        public void MinimumPremiumAdjustment_MustBeCorrect_WhenCancelledAmountIsLowerThanMinPrem()
        {

            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod, fromSubmission: true);
            var premiumPeriod2 = new ProratedAmountPeriod(It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                 new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()), periods, policyPeriod);
            proratedPremium.MinimumPercentage = 35;
            proratedPremium.Cancel(DateTime.Parse("5/1/2021"));

            Console.WriteLine(proratedPremium);

            Assert.AreEqual(420.00m, proratedPremium.MinimumEarned);
            Assert.AreEqual(9.04m, proratedPremium.MinimumEarnedAdjustmentRounded);
        }

        [TestMethod]
        public void ShortRatePremium_Test()
        {

            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");

            var premiumPeriod1 = new ProratedAmountPeriod(It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod, fromSubmission: true);
            var premiumPeriod2 = new ProratedAmountPeriod(It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod);
            var periods = new[]
            {
                premiumPeriod1,
                premiumPeriod2
            };

            var proratedPremium = ProratedAmount.CreateWithManyPeriods(
                 new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()), periods, policyPeriod);
            proratedPremium.MinimumPercentage = 35;
            proratedPremium.WithShortRate = true;
            proratedPremium.Cancel(DateTime.Parse("7/1/2021"));

            Console.WriteLine(proratedPremium);

            Assert.AreEqual(70.58m, proratedPremium.ShortRatePremiumRounded);
        }
    }
}
