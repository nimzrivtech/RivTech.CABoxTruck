using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.Proration
{
    [TestClass]
    public class AmountFactoryTest
    {
        AmountFactory _sut;
        Mock<IVehiclePremiumRepository> _mockVehiclePremiumRepository;
        Mock<IPolicyHistoryRepository> _mockPolicyHistoryRepository;
        Mock<IBindingRepository> _mockBindingRepository;
        Mock<IRiskDetailRepository> _mockRiskDetailRepository;
        Mock<IRiskVehiclePremiumRepository> _mockRiskVehiclePremiumRepository;

        [TestInitialize]
        public void Init()
        {
            _mockVehiclePremiumRepository = new Mock<IVehiclePremiumRepository>();
            _mockPolicyHistoryRepository = new Mock<IPolicyHistoryRepository>();
            _mockBindingRepository = new Mock<IBindingRepository>();
            _mockRiskDetailRepository = new Mock<IRiskDetailRepository>();

            _sut = new AmountFactory(
                null, // vehicle
                _mockVehiclePremiumRepository.Object,
                _mockRiskDetailRepository.Object, // riskdetail
                null, // riskManus 
                _mockPolicyHistoryRepository.Object, // policyhistory
                _mockBindingRepository.Object,
                null,
                null,
                _mockRiskVehiclePremiumRepository.Object,
                null
            );
        }


        [TestMethod]
        public async Task CreateVehicleAL()
        {
            var policyDates = new DateRange("11/30/2021", "11/30/2022");
            var riskDetailId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();

            var submisisonPremium = new VehiclePremium(riskDetailId, It.IsAny<Guid>());
            submisisonPremium.Id = 1;
            submisisonPremium.ChangeAnnualAL(13867);
            submisisonPremium.Prorate(DateTime.Parse("11/30/2021"), policyDates.End, policyDates.Start, policyDates.End);

            var endorsement1Premium = new VehiclePremium(riskDetailId, It.IsAny<Guid>(), submisisonPremium);
            endorsement1Premium.Id = 2;
            endorsement1Premium.ChangeAnnualAL(15678);
            endorsement1Premium.Prorate(DateTime.Parse("12/15/2021"), policyDates.End, policyDates.Start, policyDates.End);

            var vehicliePremiums = new VehiclePremium[] {
                submisisonPremium,
                endorsement1Premium
            };

            _mockVehiclePremiumRepository.Setup(x => x.GetByMainId(It.IsAny<Guid>(), null))
                .Returns(Task.FromResult(vehicliePremiums.Select(x => x)));

            var result = (ProratedAmount)await _sut.CreateVehicleAL(new RiskDetail { Id = riskDetailId }, new Vehicle(), policyDates);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Periods.Count() > 0);
            Assert.IsTrue(result.Periods.ToList()[1].EffectivityPeriod.Start == submisisonPremium.EffectiveDate);
            Assert.IsTrue(result.Periods.ToList()[0].EffectivityPeriod.Start == endorsement1Premium.EffectiveDate);
        }


        [TestMethod]
        public async Task CreateVehicleAL_WhenEndorsementDateSameWithSubmission()
        {
            var policyDates = new DateRange("11/30/2021", "11/30/2022");
            var riskDetailId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();

            var submisisonPremium = new VehiclePremium(riskDetailId, It.IsAny<Guid>());
            submisisonPremium.Id = 1;
            submisisonPremium.ChangeAnnualAL(13867);
            submisisonPremium.Prorate(DateTime.Parse("11/30/2021"), policyDates.End, policyDates.Start, policyDates.End);

            var endorsement1Premium = new VehiclePremium(riskDetailId, It.IsAny<Guid>(), submisisonPremium);
            endorsement1Premium.Id = 2;
            endorsement1Premium.ChangeAnnualAL(15678);
            endorsement1Premium.Prorate(DateTime.Parse("11/30/2021"), policyDates.End, policyDates.Start, policyDates.End);

            var vehicliePremiums = new VehiclePremium[] {
                submisisonPremium,
                endorsement1Premium
            };

            _mockVehiclePremiumRepository.Setup(x => x.GetByMainId(It.IsAny<Guid>(), null))
                .Returns(Task.FromResult(vehicliePremiums.Select(x => x)));

            var result = (ProratedAmount)await _sut.CreateVehicleAL(new RiskDetail { Id = riskDetailId }, new Vehicle(), policyDates);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Periods.Count() > 0);

            var submission = result.Periods.ToList()[1];
            Assert.IsTrue(submission.EffectivityPeriod.Start == submisisonPremium.EffectiveDate);
            Assert.AreEqual(13867, submission.AnnualAmount);

            var endorse1 = result.Periods.ToList()[0];
            Assert.IsTrue(endorse1.EffectivityPeriod.Start == endorsement1Premium.EffectiveDate);
            Assert.AreEqual(15678, endorse1.AnnualAmount);
        }

        [TestMethod]
        public async Task CreateGeneralLiabilityAsync_WithCancelReinstatentAndEndorsement()
        {
            var randomizer = new Random();
            var policyPeriod = new DateRange("2/14/2022", "2/14/2023");

            var binding = new Binding { BindOptionId = 1 };
            _mockBindingRepository.Setup(x => x.GetByRiskAsync(It.IsAny<Guid>())).ReturnsAsync(binding);


            var bindRiskDetail = new RiskDetail
            {
                Id = Guid.NewGuid(),
                EndorsementEffectiveDate = null,
                RiskCoverages = new[]
                {
                    new RiskCoverage
                    {
                        OptionNumber = binding.BindOptionId.Value,
                        RiskCoveragePremium  = new RiskCoveragePremium
                        {
                            GeneralLiabilityPremium = 37928.00m
                        }
                    }
                }.ToList()
            };
            var historyBindRiskDetail = new PolicyHistory
            {
                Id = 1L,
                EffectiveDate = DateTime.Parse("2/14/2022"),
                PreviousRiskDetailId = bindRiskDetail.Id.ToString(),
                EndorsementNumber = "0",
                PolicyStatus = RiskStatus.Active
            };

            var historySep17RiskDetail = new RiskDetail
            {
                Id = Guid.NewGuid(),
                RiskCoverages = new[]
                {
                    new RiskCoverage
                    {
                        OptionNumber = binding.BindOptionId.Value,
                        RiskCoveragePremium  = new RiskCoveragePremium
                        {
                            GeneralLiabilityPremium = 37928.00m
                        }
                    }
                }.ToList()
            };
            var historySep17Canceled = new PolicyHistory
            {
                Id = 2L,
                EffectiveDate = DateTime.Parse("9/17/2022"),
                PreviousRiskDetailId = historySep17RiskDetail.Id.ToString(),
                EndorsementNumber = "1",
                PolicyStatus = RiskStatus.Canceled
            };
            var historySep17Reinstated = new PolicyHistory
            {
                Id = 3L,
                EffectiveDate = DateTime.Parse("9/17/2022"),
                PreviousRiskDetailId = historySep17RiskDetail.Id.ToString(),
                EndorsementNumber = "1",
                PolicyStatus = RiskStatus.Reinstated
            };

            var endorsement1RiskDetail = new RiskDetail
            {
                Id = Guid.NewGuid(),
                RiskCoverages = new[]
                {
                    new RiskCoverage
                    {
                        OptionNumber = binding.BindOptionId.Value,
                        RiskCoveragePremium  = new RiskCoveragePremium
                        {
                            GeneralLiabilityPremium = 37928.00m
                        }
                    }
                }.ToList()
            };
            var historyEndorsement1 = new PolicyHistory()
            {
                Id = 4L,
                PreviousRiskDetailId = endorsement1RiskDetail.Id.ToString(),
                EffectiveDate = DateTime.Parse("3/1/2022"),
                EndorsementNumber = "1",
                PolicyStatus = RiskStatus.Active
            };
            _mockPolicyHistoryRepository.Setup(x => x.GetLatestByRisk(It.IsAny<Guid>())).ReturnsAsync(historyEndorsement1);

            _mockPolicyHistoryRepository.Setup(x => x.GetByRiskDetailAsync(It.IsAny<Guid>())).ReturnsAsync((Guid riskDetailId) =>
            {
                var histories = new List<PolicyHistory>
                {
                    historyBindRiskDetail,
                    historySep17Canceled,
                    historySep17Reinstated,
                    historyEndorsement1
                };
                return histories.Where(x => x.PreviousRiskDetailId == riskDetailId.ToString());
            });

            _mockPolicyHistoryRepository.Setup(x => x.GetPreviousHistory(It.IsAny<long>())).ReturnsAsync((long historyId) =>
            {
                PolicyHistory history;
                if (historyId == historyEndorsement1.Id) history = historySep17Reinstated;
                else if (historyId == historySep17Reinstated.Id) history = historySep17Canceled;
                else if (historyId == historySep17Canceled.Id) history = historyBindRiskDetail;
                else history = null;
                return history;
            });

            _mockRiskDetailRepository.Setup(x => x.GetOneIncludeByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                RiskDetail rd = null;
                if (id.Equals(endorsement1RiskDetail.Id)) rd = endorsement1RiskDetail;
                else if (id.Equals(historySep17RiskDetail.Id)) rd = historySep17RiskDetail;
                else if (id.Equals(bindRiskDetail.Id)) rd = bindRiskDetail;
                else rd = null;
                return rd;
            });

            var currentRiskDetail = new RiskDetail
            {
                Id = Guid.NewGuid()
            };
            var amountGL = (ProratedAmount)await _sut.CreateGeneralLiabilityAsync(currentRiskDetail, null, policyPeriod);

            Console.WriteLine(amountGL);

            var period1 = amountGL.Periods.ToArray()[0];
            Assert.AreEqual(DateTime.Parse("3/1/2022"), period1.EffectivityPeriod.Start);
            Assert.AreEqual(DateTime.Parse("2/14/2023"), period1.EffectivityPeriod.End);

            var period2 = amountGL.Periods.ToArray()[1];
            Assert.AreEqual(DateTime.Parse("2/14/2022"), period2.EffectivityPeriod.Start);
            Assert.AreEqual(DateTime.Parse("3/1/2022"), period2.EffectivityPeriod.End);
        }

    }
}
