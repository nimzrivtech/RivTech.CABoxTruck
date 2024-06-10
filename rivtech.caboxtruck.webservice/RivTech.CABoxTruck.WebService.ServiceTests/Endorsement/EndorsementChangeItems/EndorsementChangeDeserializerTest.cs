using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.EndorsementChangeItems
{
    [TestClass]
    public class EndorsementChangeDeserializerTest
    {

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void Deserialize_Test()
        {
            var changeItem = new BusinessNameChangeItem
            {
                BusinessName = "Test1"
            };
            var changeItems = new List<IEndorsementChangeItem>
            {
                changeItem
            };
            var changeText = new EndorsementChanges
            {
                Changes = JsonConvert.SerializeObject(changeItems)
            };
            var deserializer = new EndorsementChangeDeserializer();
            var changes = deserializer.Deserialize(changeText);

            Assert.IsTrue(changes.Any(x => x is BusinessNameChangeItem));
            Assert.AreEqual("Test1", ((BusinessNameChangeItem)changes.First()).BusinessName);
        }
    }
}
