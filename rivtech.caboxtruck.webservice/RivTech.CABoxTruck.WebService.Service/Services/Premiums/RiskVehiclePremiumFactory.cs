using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Premiums
{
    public interface IRiskVehiclePremiumFactory
    {
        Task<RiskVehiclePremiumCollection> CreateEmptyCollectionAsync(Guid riskId);
        Task<RiskVehiclePremiumCollection> CreateFromRatingResult(Guid riskDetailId, Guid vehicleId, int option);
        Task<RiskVehiclePremiumCollection> CreateFromRatingResultCancel(Guid riskDetailId, Guid currentRiskDetailId,
            DateTime? effectiveDate, Guid vehicleId, int option);
    }

    public class RiskVehiclePremiumFactory : IRiskVehiclePremiumFactory
    {
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IVehiclePremiumRatingFactorRepository _vehiclePremiumRatingFactorRepository;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;

        public RiskVehiclePremiumFactory(
            IRiskDetailRepository riskDetailRepository,
            IVehicleRepository vehicleRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IVehiclePremiumRatingFactorRepository vehiclePremiumRatingFactorRepository,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository
        )
        {
            _riskDetailRepository = riskDetailRepository;
            _vehicleRepository = vehicleRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _vehiclePremiumRatingFactorRepository = vehiclePremiumRatingFactorRepository;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
        }

        public async Task<RiskVehiclePremiumCollection> CreateEmptyCollectionAsync(Guid riskId)
        {
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            _brokerInfoRepository.Detach(brokerInfo);
            return new RiskVehiclePremiumCollection(policyPeriod);
        }

        public async Task<RiskVehiclePremiumCollection> CreateFromRatingResult(Guid riskDetailId, Guid vehicleId, int option)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var vehicle = await _vehicleRepository.FindAsync(vehicleId);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var coveragePeriod = new DateRange(riskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);

            var premiumFactors = await _vehiclePremiumRatingFactorRepository.GetByVehicleAsync(vehicleId, riskDetailId, option);
            var previousRiskVehiclePremium = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.SourceRiskDetailId ?? Guid.Empty, vehicle.MainId);

            var compPremiumValue = premiumFactors?.CompPremium ?? 0;
            var fireTheftPremiumValue = premiumFactors?.FireTheftPremium ?? 0;
            var CompFTPremiumValue = compPremiumValue + fireTheftPremiumValue;

            // Create VehicleCoveragePremium
            var coveragePremium = VehicleCoveragePremium.Builder
                .New(coveragePeriod)
                .SetAL(premiumFactors?.TotalALPremium ?? 0)
                .SetPD(premiumFactors?.TotalPDPremium ?? 0)
                .SetCargo(premiumFactors?.CargoPremium ?? 0)
                .SetCompFT(CompFTPremiumValue)
                .SetColl(premiumFactors?.CollisionPremium ?? 0)
                .SetRefrigeration(premiumFactors?.RefrigerationPremium ?? 0)
                .UpdateTotalAnnualPremium()
                .Prorate(policyPeriod)
                .Build();

            // Create RiksVehiclePremium
            var newPremiumCoverage = new RiskVehiclePremium()
            {
                RiskDetailId = riskDetailId,
                VehicleMainId = vehicle.MainId,
                CoveragePremium = coveragePremium
            };

            RiskVehiclePremiumCollection riskVehiclePremiums = previousRiskVehiclePremium?.Clone() ?? new RiskVehiclePremiumCollection(newPremiumCoverage, policyPeriod);
            riskVehiclePremiums.ChangeRiskDetail(riskDetailId);
            riskVehiclePremiums.AddOrUpdate(newPremiumCoverage);

            return riskVehiclePremiums;
        }

        public async Task<RiskVehiclePremiumCollection> CreateFromRatingResultCancel(Guid riskDetailId, Guid currentRiskDetailId, 
            DateTime? effectiveDate, Guid vehicleId, int option)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(currentRiskDetailId);
            var vehicle = await _vehicleRepository.FindAsync(vehicleId);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var coveragePeriod = new DateRange(brokerInfo.EffectiveDate.Value, effectiveDate.Value);

            var premiumFactors = await _vehiclePremiumRatingFactorRepository.GetByVehicleAsync(vehicleId, currentRiskDetailId, option);
            var previousRiskVehiclePremium = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetailId, vehicle.MainId);

            var compPremiumValue = premiumFactors?.CompPremium ?? 0;
            var fireTheftPremiumValue = premiumFactors?.FireTheftPremium ?? 0;
            var CompFTPremiumValue = compPremiumValue + fireTheftPremiumValue;

            // Create VehicleCoveragePremium
            var coveragePremium = VehicleCoveragePremium.Builder
                .New(coveragePeriod)
                .SetAL(premiumFactors?.TotalALPremium ?? 0)
                .SetPD(premiumFactors?.TotalPDPremium ?? 0)
                .SetCargo(premiumFactors?.CargoPremium ?? 0)
                .SetCompFT(CompFTPremiumValue)
                .SetColl(premiumFactors?.CollisionPremium ?? 0)
                .SetRefrigeration(premiumFactors?.RefrigerationPremium ?? 0)
                .UpdateTotalAnnualPremium()
                .Prorate(policyPeriod)
                .Build();

            // Create RiksVehiclePremium
            var newPremiumCoverage = new RiskVehiclePremium()
            {
                RiskDetailId = riskDetailId,
                VehicleMainId = vehicle.MainId,
                CoveragePremium = coveragePremium
            };

            RiskVehiclePremiumCollection riskVehiclePremiums = previousRiskVehiclePremium?.Clone() ?? new RiskVehiclePremiumCollection(newPremiumCoverage, policyPeriod);
            riskVehiclePremiums.ChangeRiskDetail(riskDetailId);
            riskVehiclePremiums.AddOrUpdate(newPremiumCoverage);

            return riskVehiclePremiums;
        }
    }
}
