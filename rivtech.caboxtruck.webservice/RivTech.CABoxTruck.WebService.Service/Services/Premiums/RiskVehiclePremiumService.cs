using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiskData = RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk;

namespace RivTech.CABoxTruck.WebService.Service.Services.Premiums
{
    public interface IRiskVehiclePremiumService
    {
        Task UpdateVehiclePremiums(Vehicle vehicle, Guid riskDetailId, DateRange coveragePeriod, DateRange policyPeriod, int? option = null);
    }

    public class RiskVehiclePremiumService : IRiskVehiclePremiumService
    {
        private readonly IVehiclePremiumRatingFactorRepository _vehiclePremiumRatingFactorRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;

        public RiskVehiclePremiumService(
            IVehiclePremiumRatingFactorRepository vehiclePremiumRatingFactorRepository,
            IVehicleRepository vehicleRepository,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskDetailRepository riskDetailRepository
        )
        {
            _vehiclePremiumRatingFactorRepository = vehiclePremiumRatingFactorRepository;
            _vehicleRepository = vehicleRepository;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskDetailRepository = riskDetailRepository;
        }



        public async Task UpdateVehiclePremiums(Vehicle vehicle, Guid riskDetailId, DateRange coveragePeriod, DateRange policyPeriod, int? option = null)
        {
            var premiumFactors = await _vehiclePremiumRatingFactorRepository.GetByVehicleAsync(vehicle.Id, riskDetailId, option);
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);

            var compPremiumValue = premiumFactors?.CompPremium ?? 0;
            var fireTheftPremiumValue = premiumFactors?.FireTheftPremium ?? 0;
            var CompFTPremiumValue = compPremiumValue + fireTheftPremiumValue;

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

            var newPremiumCoverage = new RiskVehiclePremium()
            {
                RiskDetailId = riskDetailId,
                VehicleMainId = vehicle.MainId,
                CoveragePremium = coveragePremium
            };

            RiskVehiclePremiumCollection currentRiskVehiclePremium = await _riskVehiclePremiumRepository
                .GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetailId, vehicle.MainId);
            if (currentRiskVehiclePremium != null)
                await _riskVehiclePremiumRepository.DeleteAsync(currentRiskVehiclePremium);

            RiskVehiclePremiumCollection previousRiskVehiclePremium = await _riskVehiclePremiumRepository
                .GetByVehicleByRiskAsync(riskDetail.RiskId, riskDetail.SourceRiskDetailId ?? Guid.Empty, vehicle.MainId);

            currentRiskVehiclePremium = previousRiskVehiclePremium?.Clone() ?? new RiskVehiclePremiumCollection(newPremiumCoverage, policyPeriod);
            currentRiskVehiclePremium.ChangeRiskDetail(riskDetailId);
            currentRiskVehiclePremium.AddOrUpdate(newPremiumCoverage);

            await _riskVehiclePremiumRepository.SaveAsync(currentRiskVehiclePremium);
        }
    }
}
