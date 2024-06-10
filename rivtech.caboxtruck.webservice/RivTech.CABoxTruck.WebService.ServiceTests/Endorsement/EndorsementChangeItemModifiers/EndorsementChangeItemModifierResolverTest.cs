using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.EndorsementChangeItemModifiers
{
    [TestClass]
    public class EndorsementChangeItemModifierResolverTest
    {
        EndorsementChangeItemModifierResolver _sut; 

        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void Resolve_Test()
        {
            _sut = new EndorsementChangeItemModifierResolver(new List<IEndorsementChangeItemModifier> { 
                new BusinessNameModifier(new Mock<IEntityService>().Object, new Mock<IMapper>().Object, new Mock<IEntityRepository>().Object)
            });

            IEndorsementChangeItem changeItem = new BusinessNameChangeItem();
            var modifier = _sut.Resolve(changeItem);

            Assert.AreEqual(typeof(BusinessNameModifier), modifier.GetType());
        }
    }
}
