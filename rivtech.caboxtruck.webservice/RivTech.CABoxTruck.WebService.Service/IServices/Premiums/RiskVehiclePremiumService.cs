using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Premiums
{
    public class RiskVehiclePremiumService : IRiskVehiclePremiumService
    {
        private readonly IVehiclePremiumRatingFactorRepository _vehiclePremiumRatingFactorRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;

        public RiskVehiclePremiumService(
            IVehiclePremiumRatingFactorRepository vehiclePremiumRatingFactorRepository,
            IVehicleRepository vehicleRepository,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository
        )
        {
            _vehiclePremiumRatingFactorRepository = vehiclePremiumRatingFactorRepository;
            _vehicleRepository = vehicleRepository;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
        }

        public async Task UpdateVehiclePremiums(Guid riskDetailId, DateRange coveragePeriod, DateRange policyPeriod)
        {
            //var riskDetail = await _
            var vehiclePremiumRatingFactors = await _vehiclePremiumRatingFactorRepository.GetByRiskDetailAsync(riskDetailId);
            //var policyPeriod = new DateRange(riskEndorsementData.PolicyEffectiveDate, riskEndorsementData.PolicyExpirationDate);
            //var coveragePeriod = new DateRange(riskEndorsementData.EffectiveDate, riskEndorsementData.PolicyExpirationDate);

            foreach (var premiumFactors in vehiclePremiumRatingFactors)
            {
                var vehicle = _vehicleRepository.GetById(Guid.Parse(premiumFactors.VehicleId));
                var currentRiskVehiclePremium = await _riskVehiclePremiumRepository.GetByVehicleAsync(Guid.Parse(premiumFactors.VehicleId));
                var clonedRiskVehiclePremiums = currentRiskVehiclePremium.Clone();

                clonedRiskVehiclePremiums.ChangeRiskDetail(riskDetailId);

                var premiumCoverage = VehicleCoveragePremium.CreateNew(coveragePeriod, policyPeriod, new Premium(premiumFactors.TotalALPremium ?? 0));
                premiumCoverage.PDPremium = new Premium(premiumFactors.TotalPDPremium ?? 0);
                premiumCoverage.CargoPremium = new Premium(premiumFactors.CargoPremium ?? 0);
                premiumCoverage.CompFTPremium = new Premium((premiumFactors.CompPremium + premiumFactors.FireTheftPremium) ?? 0);
                premiumCoverage.CollisionPremium = new Premium(premiumFactors.CollisionPremium ?? 0);
                premiumCoverage.RefrigerationPremium = new Premium(premiumFactors.RefrigerationPremium ?? 0);

                var newPremiumCoverage = new RiskVehiclePremium()
                {
                    RiskDetailId = riskDetailId,
                    VehicleMainId = vehicle.MainId,
                    CoveragePremium = premiumCoverage
                };
                clonedRiskVehiclePremiums.Add(newPremiumCoverage);

                await _riskVehiclePremiumRepository.CreateAsync(clonedRiskVehiclePremiums);
            }
        }
    }
}
