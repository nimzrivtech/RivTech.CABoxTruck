using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DataTests.Entity.Vehicles
{
    [TestClass]
    public class VehiclePremiumItemTest
    {

        DateTime PolicyEffective => DateTime.Parse("11/17/2021");
        DateTime PolicyExpiration => DateTime.Parse("11/17/2022");

        [TestMethod]
        public void Prorate_Initial()
        {
            // Initial premium with 10,000 Annual
            var vehPremItem1 = new VehiclePremiumItem(13837);
            var effectiveDate1 = DateTime.Parse("11/17/2021");
            vehPremItem1 = vehPremItem1.Prorate(effectiveDate1, PolicyExpiration, PolicyEffective, PolicyExpiration);

            Assert.AreEqual(13837.00m, vehPremItem1.Annual.To2Decimals());
            Assert.AreEqual(13837.00m, vehPremItem1.GrossProrated.To2Decimals());
            Assert.AreEqual(13837.00m, vehPremItem1.NetProrated.To2Decimals());
        }

        [TestMethod]
        public void Prorate_WithPrevious()
        {
            // Initial premium with 10,000 Annual
            var vehPremItem1 = new VehiclePremiumItem(13837);
            var effectiveDate1 = DateTime.Parse("11/17/2021");
            vehPremItem1 = vehPremItem1.Prorate(effectiveDate1, PolicyExpiration, PolicyEffective, PolicyExpiration);

            Assert.AreEqual(13837.00m, vehPremItem1.Annual.To2Decimals());
            Assert.AreEqual(13837.00m, vehPremItem1.GrossProrated.To2Decimals());
            Assert.AreEqual(13837.00m, vehPremItem1.NetProrated.To2Decimals());

            // 2nd prorate with Annual of 12,000
            var vehPremItem2 = new VehiclePremiumItem(16098, vehPremItem1);
            var effectiveDate2 = DateTime.Parse("12/1/2021");
            vehPremItem2 = vehPremItem2.Prorate(effectiveDate2, PolicyExpiration, PolicyEffective, PolicyExpiration);

            Assert.AreEqual(16098.00m, vehPremItem2.Annual.To2Decimals());
            Assert.AreEqual(16011.28m, vehPremItem2.GrossProrated.To2Decimals());
            Assert.AreEqual(2174.28m, vehPremItem2.NetProrated.To2Decimals());
        }

    }
}
