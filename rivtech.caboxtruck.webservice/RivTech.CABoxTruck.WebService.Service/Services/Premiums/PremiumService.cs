using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Migrations;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Policy;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RiskEntity = RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk;

namespace RivTech.CABoxTruck.WebService.Service.Services.Premiums
{
    public interface IPremiumService
    {
        Task UpdateRaterPremiumsAsync(Guid riskDetailId, bool? fromPolicy = null, int? option = null);
        Task UpdateRaterPremiumsCopySubmissionAsync(Guid riskDetailId, bool? fromPolicy = null, int? option = null);
        Task UpdatePolicyPremiums(Guid riskDetailId, int? option = null);
        Task UpdateVehiclePremiums(Guid riskDetailId, int option, bool fromPolicy);
        Task UpdatePolicyPremiums(RiskPolicyPremiumCollection riskPolicyPremiumCollection);
        Task UpdateVehiclePremiums(RiskVehiclePremiumCollection newRiskVehiclePremiums);
        Task UpdateRiskCoverageProratedPremium(Guid riskDetailId, int? option = null);
        /// <summary>
        /// Generates RiskPremiums for reinstatement
        /// </summary>
        Task GenerateReinstatementPremiums(RiskDetail lastIssuedRiskDetail, RiskDetail canceledRiskDetail);
        Task<PolicyPremiumDetailsDTO> GetPolicyPremiumDetailsAsync(Guid riskId);
        Task UpdateVehiclePremiumsForDataFix(Guid riskDetailId, int option, bool fromPolicy);
        Task ResetPremiumsAsync(RiskEntity risk);
        Task UpdateVehiclePremiumMetrics(Guid riskDetailId, int option, bool fromPolicy, bool? isFlatCancel = false);
        Task UpdateVehiclePremiumMetricsCancel(Guid riskDetailId, Guid currentRiskDetailId, DateTime? effectiveDate, int option, bool fromPolicy, bool? isFlatCancel = false);
    }

    public class PremiumService : IPremiumService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRiskVehiclePremiumFactory _riskVehiclePremiumFactory;
        private readonly IRiskPolicyPremiumFactory _riskPolicyPremiumFactory;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBindingRepository _bindingRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IVehiclePremiumRatingFactorRepository _vehiclePremiumRatingFactorRepository;
        private readonly IRiskCoveragePremiumAvgRepository _riskCoveragePremiumAvgRepository;

        public PremiumService(
            IVehicleRepository vehicleRepository,
            IRiskVehiclePremiumFactory riskVehiclePremiumFactory,
            IRiskPolicyPremiumFactory riskPolicyPremiumFactory,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository,
            IRiskDetailRepository riskDetailRepository,
            IBindingRepository bindingRepository,
            IRiskCoverageRepository riskCoverageRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository,
            IRiskRepository riskRepository,
            IVehiclePremiumRatingFactorRepository vehiclePremiumRatingFactorRepository,
            IRiskCoveragePremiumAvgRepository riskCoveragePremiumAvgRepository
        )
        {
            _vehicleRepository = vehicleRepository;
            _riskVehiclePremiumFactory = riskVehiclePremiumFactory;
            _riskPolicyPremiumFactory = riskPolicyPremiumFactory;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
            _riskDetailRepository = riskDetailRepository;
            _bindingRepository = bindingRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _riskRepository = riskRepository;
            _vehiclePremiumRatingFactorRepository = vehiclePremiumRatingFactorRepository;
            _riskCoveragePremiumAvgRepository = riskCoveragePremiumAvgRepository;
        }

        public async Task UpdateRaterPremiumsAsync(Guid riskDetailId, bool? fromPolicy, int? option = null)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var binding = await _bindingRepository.GetByRiskAsync(riskDetail.RiskId);
            option ??= binding.BindOptionId;

            await UpdateVehiclePremiums(riskDetailId, fromPolicy != true ? option.Value : (int)binding.BindOptionId, fromPolicy ?? false);
            await UpdatePolicyPremiums(riskDetailId, fromPolicy != true ? option.Value : (int)binding.BindOptionId);

            await UpdateRiskCoverageProratedPremium(riskDetailId, option.Value);
            await UpdateVehiclePremiumMetrics(riskDetailId, fromPolicy != true ? option.Value : (int)binding.BindOptionId, fromPolicy ?? false);
        }

        public async Task UpdatePolicyPremiums(Guid riskDetailId, int? option = null)
        {
            var riskId = await _riskDetailRepository.GetRiskIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(riskId);

            RiskPolicyPremiumCollection riskPolicyGLPremiums = await _riskPolicyPremiumFactory.CreateFromRatingResult(riskDetailId, Premium.TYPE_POLICY_GL, option);
            await UpdatePolicyPremiums(riskPolicyGLPremiums);

            if (risk.RaterVersionId >= 7)
            {
                RiskPolicyPremiumCollection riskPolicyTIPremiums = await _riskPolicyPremiumFactory.CreateFromRatingResult(riskDetailId, Premium.TYPE_POLICY_TI, option);
                await UpdatePolicyPremiums(riskPolicyTIPremiums);

                RiskPolicyPremiumCollection riskPolicyNOPremiums = await _riskPolicyPremiumFactory.CreateFromRatingResult(riskDetailId, Premium.TYPE_POLICY_NO, option);
                await UpdatePolicyPremiums(riskPolicyNOPremiums);
            }
        }

        public async Task UpdateVehiclePremiums(Guid riskDetailId, int option, bool fromPolicy)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, riskDetail.EndorsementNumber > 0);
            foreach (var vehicle in vehicles)
            {
                if (vehicle.IsIncludedInOption(option) && ((fromPolicy == true) || (!fromPolicy && option == 1)))
                {
                    var riskVehiclePremiums = await _riskVehiclePremiumFactory.CreateFromRatingResult(riskDetailId, vehicle.Id, !fromPolicy ? option : 0);
                    await UpdateVehiclePremiumsByRiskId(riskDetail.RiskId, riskVehiclePremiums);
                }
            }
        }

        public async Task UpdateRaterPremiumsCopySubmissionAsync(Guid riskDetailId, bool? fromPolicy, int? option = null)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var binding = await _bindingRepository.GetByRiskAsync(riskDetail.RiskId);
            option ??= binding.BindOptionId;

            await UpdateVehiclePremiumsCopySubmission(riskDetailId, fromPolicy != true ? option.Value : (int)binding.BindOptionId, fromPolicy ?? false);
            await UpdatePolicyPremiums(riskDetailId, fromPolicy != true ? option.Value : (int)binding.BindOptionId);

            await UpdateRiskCoverageProratedPremium(riskDetailId, option.Value);
        }

        public async Task UpdateVehiclePremiumsCopySubmission(Guid riskDetailId, int option, bool fromPolicy)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, riskDetail.EndorsementNumber > 0);
            foreach (var vehicle in vehicles)
            {
                if (!fromPolicy && option == 1)
                {
                    var riskVehiclePremiums = await _riskVehiclePremiumFactory.CreateFromRatingResult(riskDetailId, vehicle.Id, !fromPolicy ? option : 0);
                    await UpdateVehiclePremiumsByRiskId(riskDetail.RiskId, riskVehiclePremiums);
                }
            }
        }


        public async Task UpdatePolicyPremiums(RiskPolicyPremiumCollection newRiskPolicyPremiums)
        {
            RiskPolicyPremiumCollection savedRiskPolicyPremiums = await _riskPolicyPremiumRepository
                .GetByRiskDetailAsync(newRiskPolicyPremiums.CurrentRiskDetailId, newRiskPolicyPremiums.PremiumType);

            if (savedRiskPolicyPremiums != null)
                await _riskPolicyPremiumRepository.DeleteAsync(savedRiskPolicyPremiums);

            await _riskPolicyPremiumRepository.SaveAsync(newRiskPolicyPremiums);
        }

        public async Task UpdateVehiclePremiumsByRiskId(Guid riskId, RiskVehiclePremiumCollection newRiskVehiclePremiums)
        {
            RiskVehiclePremiumCollection savedRiskVehiclePremiums = await _riskVehiclePremiumRepository
                            .GetByVehicleByRiskAsync(riskId, newRiskVehiclePremiums.RiskDetailId, newRiskVehiclePremiums.VehicleMainId);

            if (savedRiskVehiclePremiums != null)
                await _riskVehiclePremiumRepository.DeleteAsync(savedRiskVehiclePremiums);

            await _riskVehiclePremiumRepository.SaveAsync(newRiskVehiclePremiums);
        }

        public async Task UpdateVehiclePremiums(RiskVehiclePremiumCollection newRiskVehiclePremiums)
        {
            RiskVehiclePremiumCollection savedRiskVehiclePremiums = await _riskVehiclePremiumRepository
                            .GetByVehicleAsync(newRiskVehiclePremiums.RiskDetailId, newRiskVehiclePremiums.VehicleMainId);

            if (savedRiskVehiclePremiums != null)
                await _riskVehiclePremiumRepository.DeleteAsync(savedRiskVehiclePremiums);

            await _riskVehiclePremiumRepository.SaveAsync(newRiskVehiclePremiums);
        }

        public async Task UpdateRiskCoverageProratedPremium(Guid riskDetailId, int? option = null)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var binding = await _bindingRepository.GetByRiskAsync(riskDetail.RiskId);
            option ??= binding.BindOptionId;

            var riskCov = await _riskCoverageRepository.GetRiskCoverageByOption(riskDetailId, option.Value);

            var totalPremium = 0m;
            // Add vehicle premiums
            var vehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(riskDetailId);
            totalPremium += vehiclePremiumsList.Sum(x => x.TotalProratedPremium);

            // Add GL premiums
            var glPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_GL);
            riskCov.RiskCoveragePremium.ProratedGeneralLiabilityPremium = glPremiums.TotalProratedPremium;
            totalPremium += riskCov.RiskCoveragePremium.ProratedGeneralLiabilityPremium ?? 0;

            // Add TI premiums
            var tiPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_TI);
            riskCov.RiskCoveragePremium.ProratedTrailerInterchangePremiumAL = tiPremiums.TotalProratedPremium;
            totalPremium += riskCov.RiskCoveragePremium.ProratedTrailerInterchangePremiumAL ?? 0;

            // Add NO premiums
            var noPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_NO);
            riskCov.RiskCoveragePremium.ProratedNonOwnedTrailerPremium = noPremiums.TotalProratedPremium;
            totalPremium += riskCov.RiskCoveragePremium.ProratedNonOwnedTrailerPremium ?? 0;

            // Add manuscripts
            //var currentManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            //totalPremium += currentManuscripts.Sum(x => x.ProratedPremium);
            totalPremium += riskCov.RiskCoveragePremium.ProratedALManuscriptPremium ?? 0;
            totalPremium += riskCov.RiskCoveragePremium.ProratedPDManuscriptPremium ?? 0;

            // Add fully-earned premiums
            totalPremium += (riskCov.RiskCoveragePremium.AdditionalInsuredPremium ?? 0) +
                    (riskCov.RiskCoveragePremium.WaiverOfSubrogationPremium ?? 0) +
                    (riskCov.RiskCoveragePremium.PrimaryNonContributoryPremium ?? 0);

            // Add forms premiums
            totalPremium += riskCov.RiskCoveragePremium.HiredPhysicalDamage ?? 0;
            totalPremium += riskCov.RiskCoveragePremium.TrailerInterchange ?? 0;

            // Implement ARS Commission
            var arsCommissionAmount = 0m;
            var totalProratedALPremium = vehiclePremiumsList.Sum(x => x.TotalProratedALPremium);
            arsCommissionAmount = (totalProratedALPremium +
                                    (riskCov.RiskCoveragePremium.AdditionalInsuredPremium ?? 0) +
                                    (riskCov.RiskCoveragePremium.WaiverOfSubrogationPremium ?? 0) +
                                    (riskCov.RiskCoveragePremium.PrimaryNonContributoryPremium ?? 0)) * (Convert.ToDecimal(riskCov?.ARSCommission) / 100m);

            riskCov.RiskCoveragePremium.ProratedPremium = totalPremium;
            riskCov.RiskCoveragePremium.ARSCommissionAmount = arsCommissionAmount;

            _riskCoverageRepository.Update(riskCov);
            await _riskCoverageRepository.SaveChangesAsync();
        }

        public async Task GenerateReinstatementPremiums(RiskDetail lastIssuedRiskDetail, RiskDetail canceledRiskDetail)
        {
            var currentRiskVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(lastIssuedRiskDetail.Id);
            foreach (var riskPremiums in currentRiskVehiclePremiumsList)
            {
                var existingPremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(canceledRiskDetail.Id, riskPremiums.VehicleMainId, fromReinstatement: true);
                if(existingPremiums != null)
                {
                    await _riskVehiclePremiumRepository.DeleteAsync(existingPremiums);
                }

                // Clone each vehicle risk premiums
                // and assign the previously canceled RiskDetail
                // and tag as from reinstatement.
                var clonedRiskPremiums = riskPremiums.Clone();
                clonedRiskPremiums.ChangeRiskDetail(canceledRiskDetail.Id);
                clonedRiskPremiums.TagAsFromReinstatement();
                await _riskVehiclePremiumRepository.SaveAsync(clonedRiskPremiums);
            }

            // Clone GL Premiums from last active RiskDetail
            // and assign the previously canceled RiskDetail
            // and tag as from reinstatement.
            var currentGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(lastIssuedRiskDetail.Id, Premium.TYPE_POLICY_GL);
            var exisitngGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_GL, fromReinstatement: true);
            if(exisitngGLPremiums != null)
            {
                await _riskPolicyPremiumRepository.DeleteAsync(exisitngGLPremiums);
            }
            var clonedGLPremiums = currentGLPremiums.Clone();
            clonedGLPremiums.TagAsFromReinstatement();
            clonedGLPremiums.ChangeRiskDetail(canceledRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(clonedGLPremiums);

            // Clone TI Premiums from last active RiskDetail
            // and assign the previously canceled RiskDetail
            // and tag as from reinstatement.
            var currentTIPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(lastIssuedRiskDetail.Id, Premium.TYPE_POLICY_TI);
            var exisitngTIPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_TI, fromReinstatement: true);
            if (exisitngTIPremiums != null)
            {
                await _riskPolicyPremiumRepository.DeleteAsync(exisitngTIPremiums);
            }
            var clonedTIPremiums = currentTIPremiums.Clone();
            clonedTIPremiums.TagAsFromReinstatement();
            clonedTIPremiums.ChangeRiskDetail(canceledRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(clonedTIPremiums);

            // Clone NO Premiums from last active RiskDetail
            // and assign the previously canceled RiskDetail
            // and tag as from reinstatement.
            var currentNOPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(lastIssuedRiskDetail.Id, Premium.TYPE_POLICY_TI);
            var exisitngNOPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(canceledRiskDetail.Id, Premium.TYPE_POLICY_TI, fromReinstatement: true);
            if (exisitngNOPremiums != null)
            {
                await _riskPolicyPremiumRepository.DeleteAsync(exisitngNOPremiums);
            }
            var clonedNOPremiums = currentNOPremiums.Clone();
            clonedNOPremiums.TagAsFromReinstatement();
            clonedNOPremiums.ChangeRiskDetail(canceledRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(clonedNOPremiums);
        }

        public async Task<PolicyPremiumDetailsDTO> GetPolicyPremiumDetailsAsync(Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(x => x.Id == riskId);
            Guid riskDetailId = riskDetailId = risk.ForEndorsementRiskDetailId.Value; // If For Endorsement Risk Detail

            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(riskDetailId);
            var alAnnualPremiumTotal = riskVehiclePremiums.Sum(rvp =>
                rvp.RiskPremiums.OrderBy(x => x.Order).Last()
                    .CoveragePremium.ALPremium.Annual);

            var pdAnnualPremiumTotal = riskVehiclePremiums.Sum(rvp =>
                rvp.RiskPremiums.OrderBy(x => x.Order).Last()
                    .CoveragePremium.PDPremium.Annual);

            var cargoAnnualPremiumTotal = riskVehiclePremiums.Sum(rvp =>
                rvp.RiskPremiums.OrderBy(x => x.Order).Last()
                    .CoveragePremium.CargoPremium.Annual);

            var riskPolicyPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_GL);
            var glAnnualPremium = riskPolicyPremiums.RiskPremiums.OrderBy(x => x.Order).Last().CoveragePremium.Premium.Annual;

            return new PolicyPremiumDetailsDTO
            {
                VehicleALAnnualPremiumTotal = alAnnualPremiumTotal,
                VehiclePDAnnualPremiumTotal = pdAnnualPremiumTotal,
                VehicleCargoAnnualPremiumTotal = cargoAnnualPremiumTotal,
                GLAnnualPremium = glAnnualPremium
            };
        }

        public async Task UpdateVehiclePremiumsForDataFix(Guid riskDetailId, int option, bool fromPolicy)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, riskDetail.EndorsementNumber > 0);
            foreach (var vehicle in vehicles)
            {
                var riskVehiclePremiums = await _riskVehiclePremiumFactory.CreateFromRatingResult(riskDetailId, vehicle.Id, fromPolicy ? 0 : option);
                await UpdateVehiclePremiums(riskVehiclePremiums);
            }
        }

        public async Task ResetPremiumsAsync(RiskEntity risk)
        {
            var forEndoRiskDetail = await _riskDetailRepository.FindAsync(risk.ForEndorsementRiskDetailId.Value);
            var forEndoVehicles = await _vehicleRepository.GetByRiskDetailAsync(risk.ForEndorsementRiskDetailId.Value);

            var forEndoRatingFactors = new List<VehiclePremiumRatingFactor>();
            foreach(var forEndoVeh in forEndoVehicles)
            {
                // Copy rating factors
                await _vehiclePremiumRatingFactorRepository.DeleteAsync(forEndoVeh.Id, risk.ForEndorsementRiskDetailId.Value);
                   
                var lastEndoVehRatingFactor = await _vehiclePremiumRatingFactorRepository.GetByVehicleAsync(forEndoVeh.Id, risk.CurrentRiskDetailId.Value);
                var forEndoVehRatingFactor = lastEndoVehRatingFactor.Clone();
                forEndoVehRatingFactor.Id = 0;
                forEndoVehRatingFactor.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                forEndoRatingFactors.Add(forEndoVehRatingFactor);

                // Copy premiums
                var forEndoVehPrem = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(risk.Id, risk.ForEndorsementRiskDetailId.Value, forEndoVeh.MainId);

                if (forEndoVehPrem != null)
                    await _riskVehiclePremiumRepository.DeleteAsync(forEndoVehPrem);
                var lastEndoVehPrem = await _riskVehiclePremiumRepository.GetByVehicleByRiskAsync(risk.Id, risk.CurrentRiskDetailId.Value, forEndoVeh.MainId);
                forEndoVehPrem = lastEndoVehPrem.Clone();
                forEndoVehPrem.ChangeRiskDetail(risk.ForEndorsementRiskDetailId.Value);
                await _riskVehiclePremiumRepository.SaveAsync(forEndoVehPrem);

            }
            await _vehiclePremiumRatingFactorRepository.CreateAsync(forEndoRatingFactors.ToArray());

            // Copy GL premiums.
            var glPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(risk.CurrentRiskDetailId.Value, Premium.TYPE_POLICY_GL);
            if(glPremiums.Any())
            {
                var forEndoGLPremiums = await _riskPolicyPremiumRepository
                    .GetByRiskDetailAsync(risk.ForEndorsementRiskDetailId.Value, Premium.TYPE_POLICY_GL);

                if (forEndoGLPremiums.Any())
                {
                    await _riskPolicyPremiumRepository.DeleteAsync(forEndoGLPremiums);
                }
                forEndoGLPremiums = glPremiums.Clone();
                forEndoGLPremiums.ChangeRiskDetail(risk.ForEndorsementRiskDetailId.Value);
                await _riskPolicyPremiumRepository.SaveAsync(forEndoGLPremiums);
            }

            var lastIssuedRiskCov = (await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(risk.CurrentRiskDetailId.Value)).First();    
            var forEndoRiskCov = (await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(risk.ForEndorsementRiskDetailId.Value)).First();
            forEndoRiskCov.RiskCoveragePremium.GeneralLiabilityPremium = lastIssuedRiskCov.RiskCoveragePremium.GeneralLiabilityPremium;
            _riskCoverageRepository.Update(forEndoRiskCov);
            await _riskCoverageRepository.SaveChangesAsync();
        }

        public async Task UpdateVehiclePremiumMetrics(Guid riskDetailId, int option, bool fromPolicy, bool? isFlatCancel = false)
        {
            var riskVehiclePremiumAvg = new RiskCoveragePremiumAvg();

            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, riskDetail.EndorsementNumber > 0);
            var riskCov = await _riskCoverageRepository.GetRiskCoverageByOption(riskDetailId, option);
            foreach (var vehicle in vehicles)
            {
                if (vehicle.IsIncludedInOption(option))
                {
                    var riskVehiclePremiums = await _riskVehiclePremiumFactory.CreateFromRatingResult(riskDetailId, vehicle.Id, !fromPolicy ? option : 0);

                    // Populate Premium Metrics
                    if (vehicle.VehicleType == "Power Unit")
                    {
                        riskVehiclePremiumAvg.NumberOfPowerUnits++;
                        riskVehiclePremiumAvg.AlVehiclePowerPremium += riskVehiclePremiums?.TotalProratedALPremium ?? 0;
                        riskVehiclePremiumAvg.ApdVehiclePowerPremium += riskVehiclePremiums?.TotalProratedPDPremium ?? 0;
                        riskVehiclePremiumAvg.MtcRefVehiclePowerPremium += riskVehiclePremiums?.TotalProratedCargoPremium ?? 0;
                        //riskVehiclePremiumAvg.MtcRefVehiclePowerPremium += riskVehiclePremiums?.TotalProratedRefPremium ?? 0;
                        riskVehiclePremiumAvg.TotalVehiclePowerPremium = (riskVehiclePremiumAvg.AlVehiclePowerPremium +
                                                                            riskVehiclePremiumAvg.ApdVehiclePowerPremium +
                                                                            riskVehiclePremiumAvg.MtcRefVehiclePowerPremium);
                    }
                    else
                    {
                        riskVehiclePremiumAvg.NumberOfTrailerOthers++;
                        riskVehiclePremiumAvg.AlVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedALPremium ?? 0;
                        riskVehiclePremiumAvg.ApdVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedPDPremium ?? 0;
                        riskVehiclePremiumAvg.MtcRefVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedCargoPremium ?? 0;
                        //riskVehiclePremiumAvg.MtcRefVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedRefPremium ?? 0;
                        riskVehiclePremiumAvg.TotalVehicleTrailerOtherPremium = (riskVehiclePremiumAvg.AlVehicleTrailerOtherPremium +
                                                                            riskVehiclePremiumAvg.ApdVehicleTrailerOtherPremium +
                                                                            riskVehiclePremiumAvg.MtcRefVehicleTrailerOtherPremium);
                    }

                    riskVehiclePremiumAvg.NumberOfVehicles++;
                    riskVehiclePremiumAvg.AlVehicleTotalPremium += riskVehiclePremiums?.TotalProratedALPremium ?? 0;
                    riskVehiclePremiumAvg.ApdVehicleTotalPremium += riskVehiclePremiums?.TotalProratedPDPremium ?? 0;
                    riskVehiclePremiumAvg.MtcRefVehicleTotalPremium += riskVehiclePremiums?.TotalProratedCargoPremium ?? 0;
                    //riskVehiclePremiumAvg.MtcRefVehicleTotalPremium += riskVehiclePremiums?.TotalProratedRefPremium ?? 0;
                    riskVehiclePremiumAvg.TotalVehiclePremium = (riskVehiclePremiumAvg.AlVehicleTotalPremium +
                                                                        riskVehiclePremiumAvg.ApdVehicleTotalPremium +
                                                                        riskVehiclePremiumAvg.MtcRefVehicleTotalPremium);
                }
            }

            riskVehiclePremiumAvg.RiskCoveragePremiumId = riskCov.RiskCoveragePremium.Id;

            // Manuscript
            riskVehiclePremiumAvg.TotalManuPremium += riskCov.RiskCoveragePremium.ManuscriptALPremium ?? 0;
            riskVehiclePremiumAvg.TotalManuPremium += riskCov.RiskCoveragePremium.ManuscriptPDPremium ?? 0;

            // Additional Insured
            riskVehiclePremiumAvg.TotalAIPremium += riskCov.RiskCoveragePremium.AdditionalInsuredPremium ?? 0;
            riskVehiclePremiumAvg.TotalAIPremium += riskCov.RiskCoveragePremium.WaiverOfSubrogationPremium ?? 0;
            riskVehiclePremiumAvg.TotalAIPremium += riskCov.RiskCoveragePremium.PrimaryNonContributoryPremium ?? 0;

            riskVehiclePremiumAvg.TotalVehiclePremium += (riskVehiclePremiumAvg.TotalAIPremium + riskVehiclePremiumAvg.TotalManuPremium);
            riskVehiclePremiumAvg.TotalVehiclePowerPremium += (riskVehiclePremiumAvg.TotalAIPremium + riskVehiclePremiumAvg.TotalManuPremium);
            riskVehiclePremiumAvg.TotalVehicleTrailerOtherPremium += (riskVehiclePremiumAvg.TotalAIPremium + riskVehiclePremiumAvg.TotalManuPremium);

            riskVehiclePremiumAvg.CalculateAveragePremiums();
            await UpdateVehiclePremiumsAvg(riskVehiclePremiumAvg);
        }

        public async Task UpdateVehiclePremiumMetricsCancel(Guid riskDetailId, Guid currentRiskDetailId, DateTime? effectiveDate, int option, bool fromPolicy, bool? isFlatCancel = false)
        {
            var riskVehiclePremiumAvg = new RiskCoveragePremiumAvg();

            var riskDetail = await _riskDetailRepository.FindAsync(currentRiskDetailId);
            var vehicles = await _vehicleRepository.GetByRiskDetailAsync(currentRiskDetailId, riskDetail.EndorsementNumber > 0);
            var riskCov = await _riskCoverageRepository.GetRiskCoverageByOption(riskDetailId, option);
            foreach (var vehicle in vehicles)
            {
                if (vehicle.IsIncludedInOption(option))
                {
                    var riskVehiclePremiums = await _riskVehiclePremiumFactory.CreateFromRatingResultCancel(riskDetailId, currentRiskDetailId, effectiveDate,
                        vehicle.Id, 1);

                    // Populate Premium Metrics
                    if (vehicle.VehicleType == "Power Unit")
                    {
                        riskVehiclePremiumAvg.NumberOfPowerUnits++;
                        riskVehiclePremiumAvg.AlVehiclePowerPremium += riskVehiclePremiums?.TotalProratedALPremium ?? 0;
                        riskVehiclePremiumAvg.ApdVehiclePowerPremium += riskVehiclePremiums?.TotalProratedPDPremium ?? 0;
                        riskVehiclePremiumAvg.MtcRefVehiclePowerPremium += riskVehiclePremiums?.TotalProratedCargoPremium ?? 0;
                        //riskVehiclePremiumAvg.MtcRefVehiclePowerPremium += riskVehiclePremiums?.TotalProratedRefPremium ?? 0;
                        riskVehiclePremiumAvg.TotalVehiclePowerPremium = (riskVehiclePremiumAvg.AlVehiclePowerPremium +
                                                                            riskVehiclePremiumAvg.ApdVehiclePowerPremium +
                                                                            riskVehiclePremiumAvg.MtcRefVehiclePowerPremium);
                    }
                    else
                    {
                        riskVehiclePremiumAvg.NumberOfTrailerOthers++;
                        riskVehiclePremiumAvg.AlVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedALPremium ?? 0;
                        riskVehiclePremiumAvg.ApdVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedPDPremium ?? 0;
                        riskVehiclePremiumAvg.MtcRefVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedCargoPremium ?? 0;
                        //riskVehiclePremiumAvg.MtcRefVehicleTrailerOtherPremium += riskVehiclePremiums?.TotalProratedRefPremium ?? 0;
                        riskVehiclePremiumAvg.TotalVehicleTrailerOtherPremium = (riskVehiclePremiumAvg.AlVehicleTrailerOtherPremium +
                                                                            riskVehiclePremiumAvg.ApdVehicleTrailerOtherPremium +
                                                                            riskVehiclePremiumAvg.MtcRefVehicleTrailerOtherPremium);
                    }

                    riskVehiclePremiumAvg.NumberOfVehicles++;
                    riskVehiclePremiumAvg.AlVehicleTotalPremium += riskVehiclePremiums?.TotalProratedALPremium ?? 0;
                    riskVehiclePremiumAvg.ApdVehicleTotalPremium += riskVehiclePremiums?.TotalProratedPDPremium ?? 0;
                    riskVehiclePremiumAvg.MtcRefVehicleTotalPremium += riskVehiclePremiums?.TotalProratedCargoPremium ?? 0;
                    //riskVehiclePremiumAvg.MtcRefVehicleTotalPremium += riskVehiclePremiums?.TotalProratedRefPremium ?? 0;
                    riskVehiclePremiumAvg.TotalVehiclePremium = (riskVehiclePremiumAvg.AlVehicleTotalPremium +
                                                                        riskVehiclePremiumAvg.ApdVehicleTotalPremium +
                                                                        riskVehiclePremiumAvg.MtcRefVehicleTotalPremium);
                }
            }

            if (isFlatCancel.Value == true)
            {
                riskVehiclePremiumAvg.NumberOfVehicles = 0;
                riskVehiclePremiumAvg.NumberOfPowerUnits = 0;
                riskVehiclePremiumAvg.NumberOfTrailerOthers = 0;

                riskVehiclePremiumAvg.AlVehiclePowerPremium = 0;
                riskVehiclePremiumAvg.ApdVehiclePowerPremium = 0;
                riskVehiclePremiumAvg.MtcRefVehiclePowerPremium = 0;
                riskVehiclePremiumAvg.AlVehiclePowerPremium = 0;

                riskVehiclePremiumAvg.TotalVehiclePowerPremium = 0;
                riskVehiclePremiumAvg.TotalVehicleTrailerOtherPremium = 0;
                riskVehiclePremiumAvg.TotalVehiclePremium = 0;
            }

            riskVehiclePremiumAvg.RiskCoveragePremiumId = riskCov.RiskCoveragePremium.Id;

            // Manuscript
            riskVehiclePremiumAvg.TotalManuPremium += riskCov.RiskCoveragePremium.ManuscriptALPremium ?? 0;
            riskVehiclePremiumAvg.TotalManuPremium += riskCov.RiskCoveragePremium.ManuscriptPDPremium ?? 0;

            // Additional Insured
            riskVehiclePremiumAvg.TotalAIPremium += riskCov.RiskCoveragePremium.AdditionalInsuredPremium ?? 0;
            riskVehiclePremiumAvg.TotalAIPremium += riskCov.RiskCoveragePremium.WaiverOfSubrogationPremium ?? 0;
            riskVehiclePremiumAvg.TotalAIPremium += riskCov.RiskCoveragePremium.PrimaryNonContributoryPremium ?? 0;

            riskVehiclePremiumAvg.TotalVehiclePremium += (riskVehiclePremiumAvg.TotalAIPremium + riskVehiclePremiumAvg.TotalManuPremium);
            riskVehiclePremiumAvg.TotalVehiclePowerPremium += (riskVehiclePremiumAvg.TotalAIPremium + riskVehiclePremiumAvg.TotalManuPremium);
            riskVehiclePremiumAvg.TotalVehicleTrailerOtherPremium += (riskVehiclePremiumAvg.TotalAIPremium + riskVehiclePremiumAvg.TotalManuPremium);

            riskVehiclePremiumAvg.CalculateAveragePremiums();
            await UpdateVehiclePremiumsAvg(riskVehiclePremiumAvg);
        }

        public async Task UpdateVehiclePremiumsAvg(RiskCoveragePremiumAvg riskVehiclePremiumAvg)
        {
            RiskCoveragePremiumAvg savedRiskVehiclePremiumsAvg = await _riskCoveragePremiumAvgRepository
                .FindAsync(x => x.RiskCoveragePremiumId == riskVehiclePremiumAvg.RiskCoveragePremiumId);

            if (savedRiskVehiclePremiumsAvg != null)
                _riskCoveragePremiumAvgRepository.Remove(savedRiskVehiclePremiumsAvg);

            await _riskCoveragePremiumAvgRepository.AddAsync(riskVehiclePremiumAvg);
            await _riskCoveragePremiumAvgRepository.SaveChangesAsync();
        }
    }
}
