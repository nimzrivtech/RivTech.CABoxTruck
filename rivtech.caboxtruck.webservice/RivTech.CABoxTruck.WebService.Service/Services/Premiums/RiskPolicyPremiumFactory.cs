using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Premiums
{
    public interface IRiskPolicyPremiumFactory
    {
        Task<RiskPolicyPremiumCollection> CreateEmptyCollectionAsync(Guid riskId);
        Task<RiskPolicyPremiumCollection> CreateFromRatingResult(Guid riskDetailId, string premiumType, int? option = null);
    }

    public class RiskPolicyPremiumFactory : IRiskPolicyPremiumFactory
    {
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;

        public RiskPolicyPremiumFactory(
            IRiskDetailRepository riskDetailRepository,
            IRiskCoverageRepository riskCoverageRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository
        )
        {
            _riskDetailRepository = riskDetailRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
        }

        public async Task<RiskPolicyPremiumCollection> CreateEmptyCollectionAsync(Guid riskId)
        {
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            _brokerInfoRepository.Detach(brokerInfo);
            return new RiskPolicyPremiumCollection(policyPeriod);
        }

        public async Task<RiskPolicyPremiumCollection> CreateFromRatingResult(Guid riskDetailId, string premiumType, int? option = null)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var coveragePeriod = new DateRange(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var previousRiskPolicyPremium = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetail.SourceRiskDetailId ?? Guid.Empty, premiumType);

            // Create VehicleCoveragePremium
            RiskCoverage riskCov = null;
            if(option is null)
                riskCov = await _riskCoverageRepository.GetRiskCoverageBySelectedBindOption(riskDetailId);
            else 
                riskCov = await _riskCoverageRepository.GetRiskCoverageByOption(riskDetailId, option.Value);

            var newPremium = premiumType switch
            {
                "GL" => riskCov.RiskCoveragePremium.GeneralLiabilityPremium ?? 0m,
                "TI" => riskCov.RiskCoveragePremium.TrailerInterchangeAL ?? 0m,
                "NO" => riskCov.RiskCoveragePremium.NonOwnedTrailerPremium ?? 0m,
                _ => 0m
            }; 

            var coveragePremium = PolicyCoveragePremium.CreateNew(coveragePeriod, policyPeriod, new Premium(newPremium));

            // Create RiksVehiclePremium
            var newPremiumCoverage = new RiskPolicyPremium()
            {
                RiskDetailId = riskDetailId,
                PremiumType = premiumType,
                CoveragePremium = coveragePremium
            };

            RiskPolicyPremiumCollection riskPolicyPremiums = previousRiskPolicyPremium?.Clone() ?? new RiskPolicyPremiumCollection(new[] { newPremiumCoverage }, policyPeriod);
            riskPolicyPremiums.ChangeRiskDetail(riskDetailId);
            riskPolicyPremiums.AddOrUpdate(newPremiumCoverage);

            return riskPolicyPremiums;
        }
    }
}
