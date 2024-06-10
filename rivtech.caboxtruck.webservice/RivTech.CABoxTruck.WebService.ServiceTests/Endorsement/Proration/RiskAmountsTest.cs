using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class RiskAmountsTest
    {


        [TestMethod]
        public void Cancellation_Test()
        {
            var minimumPremiumPercentage = 25m;
            var policyPeriod = new DateRange("1/1/2021", "1/1/2022");
            var riskPremiums = new RiskAmounts(policyPeriod, minimumPremiumPercentage);

            // Add VIN 1 AL
            riskPremiums.Add(ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()), 
                new[] {
                    new ProratedAmountPeriod( It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod, fromSubmission: true ),
                    new ProratedAmountPeriod( It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod )
                }, policyPeriod));

            // Add VIN 2 AL
            riskPremiums.Add(ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, It.IsAny<string>()), 
                new[] {
                    new ProratedAmountPeriod( It.IsAny<object>(), 1200, new DateRange("1/1/2021", "4/1/2021"), policyPeriod, fromSubmission: true ),
                    new ProratedAmountPeriod( It.IsAny<object>(), 1400, new DateRange("4/1/2021", "1/1/2022"), policyPeriod )
                }, policyPeriod));


            // Add GL
            riskPremiums.Add(ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.GeneralLiability, It.IsAny<string>()), 
                new[] {
                    new ProratedAmountPeriod( It.IsAny<object>(), 10000, new DateRange("1/1/2021", "7/1/2021"), policyPeriod, fromSubmission: true),
                    new ProratedAmountPeriod( It.IsAny<object>(), 12000, new DateRange("7/1/2021", "1/1/2022"), policyPeriod )
                }, policyPeriod));

            // Add Manuscript(Pro-rated)
            riskPremiums.Add(ProratedAmount.CreateWithSinglePeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.ManuscriptAL, It.IsAny<string>()), 
                new ProratedAmountPeriod( It.IsAny<object>(), 500, new DateRange("1/1/2021", "1/1/2022"), policyPeriod, fromSubmission: true),
                policyPeriod));

            riskPremiums.Add(new FullyEarnedAmount(new AmountInfo(It.IsAny<object>(), AmountKind.ManuscriptAL, It.IsAny<string>()), 700));
            riskPremiums.Add(new FullyEarnedAmount(new AmountInfo(It.IsAny<object>(), AmountKind.AdditionalPremiumAI, It.IsAny<string>()), 250));
            riskPremiums.Add(new FullyEarnedAmount(new AmountInfo(It.IsAny<object>(), AmountKind.AdditionalPremiumWOS, It.IsAny<string>()), 300));
            riskPremiums.Add(new FullyEarnedAmount(new AmountInfo(It.IsAny<object>(), AmountKind.AdditionalPremiumPNC, It.IsAny<string>()), 250));

            riskPremiums.Cancel(DateTime.Parse("2021-04-01"), withShortRate: true);

            Assert.AreEqual(15709.59m, riskPremiums.TotalPremiumRounded);
            Assert.AreEqual(4680.82m, riskPremiums.TotalCancelledAmountRounded);
            Assert.AreEqual(44.18m, riskPremiums.TotalMinimumPremiumRounded);
            Assert.AreEqual(1102.88m, riskPremiums.TotalShortRatePremiumAdjustmentRounded);

        }


        [TestMethod]
        public void Cancellation2_Test()
        {
            var minimumPremiumPercentage = 25m;
            var policyPeriod = new DateRange("11/30/2021", "11/30/2022");
            var riskPremiums = new RiskAmounts(policyPeriod, minimumPremiumPercentage);

            // Add VIN 1 AL
            riskPremiums.Add(ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehicleAL, "111"), 
                new[] {
                    new ProratedAmountPeriod("111", 13867.00m, new DateRange("11/30/2021", "12/15/2021"), policyPeriod, fromSubmission: true ),
                    new ProratedAmountPeriod("111", 15705.00m , new DateRange("12/15/2021", "11/30/2022"), policyPeriod )
                }, policyPeriod));

            // Add VIN 2 PD
            riskPremiums.Add(ProratedAmount.CreateWithManyPeriods(
                new AmountInfo(It.IsAny<Guid>(), AmountKind.VehiclePD, "111"), 
                new[] {
                    new ProratedAmountPeriod( It.IsAny<object>(), 1904, new DateRange("11/30/2021", "12/15/2021"), policyPeriod, fromSubmission: true ),
                    new ProratedAmountPeriod( It.IsAny<object>(), 1904, new DateRange("12/15/2021", "11/30/2022"), policyPeriod ),
                }, policyPeriod));


            riskPremiums.Cancel(DateTime.Parse("2022-01-29"));

            var vehicleAL = (ProratedAmount)riskPremiums.Amounts.First(x => x.Info.Kind == AmountKind.VehicleAL);
            Assert.AreEqual(2506.11m, vehicleAL.CancelledAmountRounded);

            var vehiclePD = (ProratedAmount)riskPremiums.Amounts.First(x => x.Info.Kind == AmountKind.VehiclePD);
            Assert.AreEqual(312.99m, vehiclePD.CancelledAmountRounded);
        }

    }
}
