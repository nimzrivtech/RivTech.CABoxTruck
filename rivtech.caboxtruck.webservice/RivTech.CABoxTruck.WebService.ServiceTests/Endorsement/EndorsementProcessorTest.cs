using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement
{
    [TestClass]
    public class EndorsementProcessorTest
    {
        EndorsementProcessor _endorsementProcessor;


        [TestInitialize]
        public void Init()
        {
         }

        private RiskEndorsementPoints CreateEndorsementPoints()
        {
            var stack = new RiskEndorsementPoints();
            stack.Push(new EndorsementPoint(Guid.NewGuid(), DateTime.Parse("1/1/2022"), new Mock<IEndorsementChangeCollection>().Object, 1));
            stack.Push(new EndorsementPoint(Guid.NewGuid(), DateTime.Parse("2/1/2022"), new Mock<IEndorsementChangeCollection>().Object, 2));
            stack.Push(new EndorsementPoint(Guid.NewGuid(), DateTime.Parse("3/1/2022"), new Mock<IEndorsementChangeCollection>().Object, 3));
            return stack;
        }

        [TestMethod]
        public async Task ProcessEndorsement_CurrentEndorsementPoint_MustBeEqualWithTargetEndorsement_IfInSequence()
        {
            var endorsementPoints = CreateEndorsementPoints();
            await _endorsementProcessor.Process(
                Guid.Empty,
                DateTime.Parse("3/2/2022"),
                new Mock<IEndorsementChangeCollection>().Object,
                endorsementPoints
            );
            Assert.AreEqual(endorsementPoints.Peek(), _endorsementProcessor.EndorsementPointHead);
        }
    }
}
