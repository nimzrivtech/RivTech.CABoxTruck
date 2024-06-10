using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;

namespace RivTech.CABoxTruck.WebService.DomainEntitiesTest.ValueObject
{
    [TestClass]
    public class DateRangeTest
    {
        [TestMethod]
        public void DaysNonNegative_WhenStartIsLaterThanEnd()
        {
            var dates = new DateRange("2/1/2022", "1/31/2022");
            Assert.AreEqual(0, dates.DaysNonNegative);
        }

    }
}
