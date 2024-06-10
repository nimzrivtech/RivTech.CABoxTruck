using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Policy;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAmount = RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration.IAmount;

namespace RivTech.CABoxTruck.WebService.Service.Services.Policy
{
    public class CancellationService : ICancellationService
    {
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IBindingRepository _bindingRepository;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly IRiskAdditionalInterestRepository _riskAdditionalInterestRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IAmountFactory _amountFactory;
        private readonly ICancellationBreakdownRepository _premiumCancellationBreakdownRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehiclePremiumRepository _vehiclePremiumRepository;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IPremiumService _premiumService;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;

        public CancellationService(
            IBrokerInfoRepository brokerInfoRepository,
            IBindingRepository bindingRepository,
            IPolicyHistoryRepository policyHistoryRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository,
            IRiskAdditionalInterestRepository riskAdditionalInterestRepository,
            IVehicleService vehicleService,
            IAmountFactory premiumFactory,
            ICancellationBreakdownRepository premiumCancellationBreakdownRepository,
            IRiskCoverageRepository riskCoverageRepository,
            IVehicleRepository vehicleRepository,
            IVehiclePremiumRepository vehiclePremiumRepository,
            IRiskFormRepository riskFormRepository,
            IRiskRepository riskRepository,
            IPremiumService premiumService,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository
        )
        {
            _brokerInfoRepository = brokerInfoRepository;
            _bindingRepository = bindingRepository;
            _policyHistoryRepository = policyHistoryRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _riskAdditionalInterestRepository = riskAdditionalInterestRepository;
            _vehicleService = vehicleService;
            _amountFactory = premiumFactory;
            _premiumCancellationBreakdownRepository = premiumCancellationBreakdownRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _vehicleRepository = vehicleRepository;
            _vehiclePremiumRepository = vehiclePremiumRepository;
            _riskFormRepository = riskFormRepository;
            _riskRepository = riskRepository;
            _premiumService = premiumService;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
        }

        public async Task<RiskAmounts> CancelPremiumFees(Guid riskDetailId, DateTime cancellationDate, bool? withShortRate = null, bool? isFlatCancel = null)
        {
            withShortRate ??= false;
            isFlatCancel ??= false;

            RiskDetail forEndorsementRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var policyHistory = await _policyHistoryRepository.GetLatestByRisk(forEndorsementRiskDetail.RiskId);
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(Guid.Parse(policyHistory.PreviousRiskDetailId));

            BrokerInfo brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            Binding binding = await _bindingRepository.FindAsync(b => b.RiskId == riskDetail.RiskId);

            DateRange policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            RiskAmounts riskAmounts = new RiskAmounts(policyPeriod, binding.MinimumEarned);
            IEnumerable<RiskVehiclePremiumCollection> riskVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(riskDetailId);
            RiskPolicyPremiumCollection policyPremiumsGL = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_GL);
            RiskPolicyPremiumCollection policyPremiumsTI = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_TI);
            RiskPolicyPremiumCollection policyPremiumsNO = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_NO);

            await AddVehiclePremiumsFees(riskAmounts, riskDetail);

            if (policyPremiumsGL != null && policyPremiumsGL.RiskPremiums.Count > 0)
            {
                var amountGLPremium = await _amountFactory.CreatePremiumAmountAsync(riskDetail, policyPremiumsGL, policyPeriod);
                riskAmounts.Add(amountGLPremium);
            }

            if (policyPremiumsTI != null && policyPremiumsTI.RiskPremiums.Count > 0)
            {
                var amountTIPremium = await _amountFactory.CreatePremiumAmountAsync(riskDetail, policyPremiumsTI, policyPeriod);
                riskAmounts.Add(amountTIPremium);
            }

            if (policyPremiumsNO != null && policyPremiumsNO.RiskPremiums.Count > 0)
            {
                var amountNOPremium = await _amountFactory.CreatePremiumAmountAsync(riskDetail, policyPremiumsNO, policyPeriod);
                riskAmounts.Add(amountNOPremium);
            }

            await AddManuscriptsAsync(riskAmounts, riskDetail);
            await AddAdditionalPremiumsAsync(riskAmounts, riskDetail);
            await AddPDPremiums(riskAmounts, riskDetail);

            riskAmounts.Cancel(cancellationDate, withShortRate, isFlatCancel);
            await SaveCancellationBreakdowns(riskDetailId, riskAmounts);


            if (policyPremiumsGL != null && policyPremiumsGL.RiskPremiums.Count > 0)
            {
                policyPremiumsGL.Cancel(cancellationDate);

                await SavePolicyPremiums(forEndorsementRiskDetail, policyPremiumsGL);
            }

            if (policyPremiumsTI != null && policyPremiumsTI.RiskPremiums.Count > 0)
            {
                policyPremiumsTI.Cancel(cancellationDate);
                await SavePolicyPremiums(forEndorsementRiskDetail, policyPremiumsTI);
            }

            if (policyPremiumsNO != null && policyPremiumsNO.RiskPremiums.Count > 0)
            {
                policyPremiumsNO.Cancel(cancellationDate);
                await SavePolicyPremiums(forEndorsementRiskDetail, policyPremiumsNO);
            }

            foreach (var vehiclePremiums in riskVehiclePremiumsList)
            {
                vehiclePremiums.Cancel(cancellationDate);
                await SaveVehiclePremiums(forEndorsementRiskDetail, vehiclePremiums);
            }

            await UpdateCurrentRiskCoveragePremium(riskDetailId, riskAmounts, binding.BindOptionId ?? 1, isFlatCancel ?? false,
                riskAmounts.TotalMinimumPremiumRounded,
                riskAmounts.TotalShortRatePremiumAdjustmentRounded);
            // await UpdateVehicleProratedPremiums(riskAmounts, forEndorsementRiskDetail, cancellationDate);
            await UpdateManuscriptProratedPremiums(riskAmounts, forEndorsementRiskDetail);

            return riskAmounts;
        }

        private async Task SaveVehiclePremiums(RiskDetail forEndorsementRiskDetail, RiskVehiclePremiumCollection riskVehiclePremiums)
        {
            var newRiskVehiclePremiums = riskVehiclePremiums.Clone();
            newRiskVehiclePremiums.ChangeRiskDetail(forEndorsementRiskDetail.Id);
            await _premiumService.UpdateVehiclePremiums(newRiskVehiclePremiums);
        }

        private async Task SavePolicyPremiums(RiskDetail forEndorsementRiskDetail, RiskPolicyPremiumCollection policyPremiums)
        {
            var newRiskPolicyPremiums = policyPremiums.Clone();
            newRiskPolicyPremiums.ChangeRiskDetail(forEndorsementRiskDetail.Id);
            await _premiumService.UpdatePolicyPremiums(newRiskPolicyPremiums);
        }

        private async Task AddPDPremiums(RiskAmounts riskAmounts, RiskDetail riskDetail)
        {
            var premiums = await _amountFactory.CreatePDPremiumsAsync(riskDetail);
            foreach (var premium in premiums)
                riskAmounts.Add(premium);
        }

        private async Task SaveCancellationBreakdowns(Guid riskDetailId, RiskAmounts riskAmounts)
        {
            // use to validate canceled amounts that has no reduction but has a cancellation amount, as these should be included in cancellation calculation process
            var vehiclePremiumTypes = new AmountKind[] { 
                AmountKind.VehicleAL, 
                AmountKind.VehiclePDCollision, 
                AmountKind.VehiclePDComprehensive, 
                AmountKind.VehicleCargo 
            };

            // Include Fully earned
            var fulleEarnedTypes = new AmountKind[] {
                AmountKind.ManuscriptAL,
                AmountKind.ManuscriptPD,
                AmountKind.PDHired,
                AmountKind.PDTrailer,
                AmountKind.PDTrailer,
                AmountKind.AdditionalPremiumAI,
                AmountKind.AdditionalPremiumPNC,
                AmountKind.AdditionalPremiumWOS,
            };

            var amountsForCancellation = riskAmounts.Amounts.Where(x => x.AmountReduction < 0 || vehiclePremiumTypes.Contains(x.Info.Kind)
                                                || fulleEarnedTypes.Contains(x.Info.Kind));

            // Calculation for New MPA with Fully earned
            decimal totalCancelledAmount = amountsForCancellation.Sum(x => x.CancelledAmount);
            IEnumerable<ProratedAmount> proratedPremiums = amountsForCancellation.Where(p => p is ProratedAmount)?.Select(p => p as ProratedAmount);
            IEnumerable<FullyEarnedAmount> fullyEarnedPremiums = amountsForCancellation.Where(p => p is FullyEarnedAmount)?.Select(p => p as FullyEarnedAmount);
            decimal totalMinimumPremium = proratedPremiums.Sum(p => p.MinimumEarned) + fullyEarnedPremiums.Sum(p => p.MinimumEarned);
            decimal totalMPA = totalMinimumPremium - totalCancelledAmount;

            foreach (var premium in amountsForCancellation)
            {
                // do not save breakdowns with no amounts.
                if (premium.Amount == 0 && premium.CancelledAmount == 0)
                    continue; 

                var breakdown = new CancellationBreakdown
                {
                    RiskDetailId = riskDetailId,
                    AmountSubTypeId = GetAmountSubTypeId(premium.Info.Kind),
                    Description = premium.Info.Description,
                    ActiveAmount = premium.Amount,
                    CancelledAmount = premium.CancelledAmount,
                    AmountReduction = premium.AmountReduction,
                };

                if (premium is ProratedAmount proratedPremium)
                {
                    proratedPremium.CalculateEntityMPA(totalCancelledAmount, totalMPA);
                    breakdown.AnnualMinimumPremium = proratedPremium.AnnualMinimumEarned;
                    breakdown.MinimumPremium = proratedPremium.MinimumEarned;
                    breakdown.MinimumPremiumAdjustment = proratedPremium.MinimumEarnedAdjustment;
                    breakdown.ShortRatePremium = proratedPremium.ShortRatePremium;
                }

                if (premium is FullyEarnedAmount fullyEarnedAmount)
                {
                    fullyEarnedAmount.CalculateEntityMPA(totalCancelledAmount, totalMPA);
                    breakdown.AnnualMinimumPremium = fullyEarnedAmount.AnnualMinimumEarned;
                    breakdown.MinimumPremium = fullyEarnedAmount.MinimumEarned;
                    breakdown.MinimumPremiumAdjustment = fullyEarnedAmount.MinimumEarnedAdjustment;
                }

                await _premiumCancellationBreakdownRepository.AddAsync(breakdown);
            }

            await _premiumCancellationBreakdownRepository.SaveChangesAsync();
        }

        private async Task UpdateCurrentRiskCoveragePremium(Guid riskDetailId, RiskAmounts riskAmounts, short optionNumber, bool isFlatCancel,
            decimal? minimumPremiumEarned = 0,
            decimal? shortRatePremium = 0)
        {
            var riskCoverages = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            bool hasHiredPhysicalDamage = riskForms.Any(x => x.FormId == "HiredPhysicalDamage" && (x.IsSelected ?? false));
            bool hasTrailerInterchange = riskForms.Any(x => x.FormId == "TrailerInterchangeCoverage" && (x.IsSelected ?? false));
            var riskCoverage = riskCoverages.Where(x => x.OptionNumber == optionNumber).FirstOrDefault();
            var riskCoveragePremium = riskCoverage.RiskCoveragePremium;

            // use to validate canceled amounts that has no reduction but has a cancellation amount, as these should be included in cancellation calculation process
            var vehiclePremiumTypes = new AmountKind[] { AmountKind.VehicleAL, AmountKind.VehiclePD, AmountKind.VehicleCargo };
            var cancellationBrkdown = riskAmounts.Amounts.Where(x => x.AmountReduction < 0 || vehiclePremiumTypes.Contains(x.Info.Kind)).ToList();

            var arsCommissionPremiumTypes = new AmountKind[] { AmountKind.VehicleAL, AmountKind.AdditionalPremiumAI, AmountKind.AdditionalPremiumPNC, AmountKind.AdditionalPremiumWOS };
            var arsCommissionCancellationBrkdown = riskAmounts.Amounts.Where(x => arsCommissionPremiumTypes.Contains(x.Info.Kind)).ToList();

            decimal canceledProratedGeneralLiabilityPremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.GeneralLiabilityPremium).Sum(x => x.CancelledAmount.RoundTo2Decimals());
            decimal canceledProratedTrailerInterchangePremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.TrailerInterchangePremium).Sum(x => x.CancelledAmount.RoundTo2Decimals());
            decimal canceledProratedNonOwnedTrailerPremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.NonOwnedTrailerPremium).Sum(x => x.CancelledAmount.RoundTo2Decimals());
            decimal canceledProratedManuscriptALPremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.ALManuscriptPremium).Sum(x => x.CancelledAmount.RoundTo2Decimals());
            decimal canceledProratedManuscriptPDPremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.PDManuscriptPremium).Sum(x => x.CancelledAmount.RoundTo2Decimals());
            decimal reductionAmountManuscriptALPremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.ALManuscriptPremium).Sum(x => x.AmountReduction.RoundTo2Decimals());
            decimal reductionAmountManuscriptPDPremium = cancellationBrkdown.Where(x => GetAmountSubTypeId(x.Info.Kind) == BillingConstants.PDManuscriptPremium).Sum(x => x.AmountReduction.RoundTo2Decimals());
            decimal canceledProratedTotalPremium = cancellationBrkdown.Where(x => x.Info.Kind != AmountKind.VehiclePDCollision
                                                                                && x.Info.Kind != AmountKind.VehiclePDComprehensive
                                                                                && x.Info.Kind != AmountKind.PDHired
                                                                                && x.Info.Kind != AmountKind.PDTrailer
                                                                                && x.Info.Kind != AmountKind.ManuscriptPD
                                                                                && x.Info.Kind != AmountKind.ManuscriptAL)
                                                                        .Sum(x => x.CancelledAmount.RoundTo2Decimals()).RoundTo2Decimals();

            riskCoveragePremium.ProratedGeneralLiabilityPremium = canceledProratedGeneralLiabilityPremium;
            riskCoveragePremium.ProratedTrailerInterchangePremiumAL = canceledProratedTrailerInterchangePremium;
            riskCoveragePremium.ProratedNonOwnedTrailerPremium = canceledProratedNonOwnedTrailerPremium;
            riskCoveragePremium.ProratedALManuscriptPremium = (canceledProratedManuscriptALPremium == 0) ? riskCoveragePremium.ALManuscriptPremium : (riskCoveragePremium.ALManuscriptPremium + reductionAmountManuscriptALPremium); // reduction amount is negative already
            riskCoveragePremium.ProratedPDManuscriptPremium = (canceledProratedManuscriptPDPremium == 0) ? riskCoveragePremium.PDManuscriptPremium : (riskCoveragePremium.PDManuscriptPremium + reductionAmountManuscriptPDPremium); // reduction amount is negative already

            if ((minimumPremiumEarned ?? 0) < 0) minimumPremiumEarned = null;
            riskCoveragePremium.MinimumPremiumEarned = minimumPremiumEarned ?? 0;

            if ((shortRatePremium ?? 0) < 0) shortRatePremium = null;
            riskCoveragePremium.ShortRatePremium = shortRatePremium ?? 0;

            if (isFlatCancel)
            {
                riskCoveragePremium.ProratedGeneralLiabilityPremium = 0;
                riskCoveragePremium.GeneralLiabilityPremium = 0;
                riskCoveragePremium.ProratedTrailerInterchangePremiumAL = 0;
                riskCoveragePremium.TrailerInterchangeAL = 0;
                riskCoveragePremium.ProratedNonOwnedTrailerPremium = 0;
                riskCoveragePremium.NonOwnedTrailerPremium = 0;
                riskCoveragePremium.ProratedALManuscriptPremium = 0;
                riskCoveragePremium.ProratedPDManuscriptPremium = 0;
                riskCoveragePremium.AdditionalInsuredPremium = 0;
                riskCoveragePremium.WaiverOfSubrogationPremium = 0;
                riskCoveragePremium.PrimaryNonContributoryPremium = 0;
                riskCoveragePremium.ALManuscriptPremium = 0;
                riskCoveragePremium.PDManuscriptPremium = 0;
                riskCoveragePremium.HiredPhysicalDamage = 0;
                riskCoveragePremium.TrailerInterchange = 0;
                riskCoveragePremium.Premium = 0;
                riskCoveragePremium.ProratedPremium = 0;
                riskCoveragePremium.ARSCommissionAmount = 0;

                // update riskforms
                if (hasHiredPhysicalDamage)
                {
                    var hiredPDForm = riskForms.FirstOrDefault(x => x.FormId == "HiredPhysicalDamage");
                    if (!string.IsNullOrEmpty(hiredPDForm.Other))
                    {
                        var hiredPhysicalDmg = JsonConvert.DeserializeObject<HiredPhysdamDTO>(hiredPDForm.Other);
                        hiredPhysicalDmg.EstimatedAdditionalPremium = 0;
                        hiredPDForm.Other = JsonConvert.SerializeObject(hiredPhysicalDmg);
                        await _riskFormRepository.UpdateRiskForm(hiredPDForm);
                    }
                }

                if (hasTrailerInterchange)
                {
                    var trailerPDForm = riskForms.FirstOrDefault(x => x.FormId == "TrailerInterchangeCoverage");
                    if (!string.IsNullOrEmpty(trailerPDForm.Other))
                    {
                        var trailerInterchangeCoverage = JsonConvert.DeserializeObject<TrailerInterchangeDTO>(trailerPDForm.Other);
                        trailerInterchangeCoverage.CollisionPremium = 0;
                        trailerInterchangeCoverage.ComprePremium = 0;
                        trailerInterchangeCoverage.FirePremium = 0;
                        trailerInterchangeCoverage.FireTheftPremium = 0;
                        trailerInterchangeCoverage.LossPremium = 0;
                        trailerPDForm.Other = JsonConvert.SerializeObject(trailerInterchangeCoverage);
                        await _riskFormRepository.UpdateRiskForm(trailerPDForm);
                    }
                }
            }

            // include fully earned premiums + prorated manuscripts
            canceledProratedTotalPremium = canceledProratedTotalPremium +
                                            (riskCoveragePremium.ProratedALManuscriptPremium ?? 0) +
                                            (riskCoveragePremium.ProratedPDManuscriptPremium ?? 0) +
                                            (riskCoveragePremium.AdditionalInsuredPremium ?? 0) +
                                            (riskCoveragePremium.WaiverOfSubrogationPremium ?? 0) +
                                            (riskCoveragePremium.PrimaryNonContributoryPremium ?? 0);
                                            //+ (riskCoveragePremium.MinimumPremiumEarned) 
                                            //+ (riskCoveragePremium.ShortRatePremium);

            if (hasHiredPhysicalDamage && !isFlatCancel)
            {
                canceledProratedTotalPremium += riskCoveragePremium.HiredPhysicalDamage ?? 0;
            }

            if (hasTrailerInterchange && !isFlatCancel)
            {
                canceledProratedTotalPremium += riskCoveragePremium.TrailerInterchange ?? 0;
            }

            if (!isFlatCancel)
            {
                var targetALForARSCommission = arsCommissionCancellationBrkdown.Sum(x => x.CancelledAmount.RoundTo2Decimals());
                riskCoveragePremium.ARSCommissionAmount = targetALForARSCommission * (Convert.ToDecimal(riskCoverage.ARSCommission) / 100m);
            }

            riskCoveragePremium.ProratedPremium = canceledProratedTotalPremium;

            riskCoverage.RiskCoveragePremium = riskCoveragePremium;
            _riskCoverageRepository.Update(riskCoverage);
            await _riskCoverageRepository.SaveChangesAsync();
        }

        private async Task UpdateVehicleProratedPremiums(RiskAmounts riskAmounts, RiskDetail newRiskDetail, DateTime cancellationDate)
        {
            foreach (var riskAmount in riskAmounts.Amounts.Where(x => x.AmountReduction < 0))
            {
                var vehiclePremiumTypes = new AmountKind[] { AmountKind.VehicleAL, AmountKind.VehiclePD, AmountKind.VehicleCargo, AmountKind.VehiclePDCollision, AmountKind.VehiclePDComprehensive };

                if (!vehiclePremiumTypes.Contains(riskAmount.Info.Kind)) continue; // if manuscript or GL, should not proceed

                Vehicle vehicle = newRiskDetail.Vehicles.Find(vehicle => vehicle.PreviousVehicleVersionId == Guid.Parse(riskAmount.Info.Id.ToString()));

                if (vehicle is null) continue;

                var vehiclePremium = await _vehiclePremiumRepository.GetByRiskDetailVehicleIdAsync(newRiskDetail.Id, vehicle.Id);
                if (vehiclePremium is null)
                {
                    var latestVehiclePremium = await _vehiclePremiumRepository.GetLatestByMainId(vehicle.MainId);
                    vehiclePremium = latestVehiclePremium.Clone();
                    vehiclePremium.RiskDetailId = newRiskDetail.Id;
                    vehiclePremium.VehicleId = vehicle.Id;
                    await _vehiclePremiumRepository.AddAsync(vehiclePremium);
                    await _vehicleRepository.SaveChangesAsync();
                }

                vehiclePremium.TransactionDate = DateTime.Now.Date;
                vehiclePremium.EffectiveDate = cancellationDate;

                switch (riskAmount.Info.Kind)
                {
                    case AmountKind.VehicleAL:
                        vehiclePremium.SetCancelAL(riskAmount.CancelledAmount.RoundTo2Decimals());
                        break;
                    case AmountKind.VehiclePD:
                        vehiclePremium.SetCancelPD(riskAmount.CancelledAmount.RoundTo2Decimals());
                        break;
                    case AmountKind.VehicleCargo:
                        vehiclePremium.SetCancelCargo(riskAmount.CancelledAmount.RoundTo2Decimals());
                        break;
                    case AmountKind.VehiclePDCollision:
                        vehiclePremium.SetCancelCollision(riskAmount.CancelledAmount.RoundTo2Decimals());
                        break;
                    case AmountKind.VehiclePDComprehensive:
                        vehiclePremium.SetCancelCompFT(riskAmount.CancelledAmount.RoundTo2Decimals());
                        break;
                }

                //await _vehiclePremiumRepository.UpdateAsync(vehiclePremium);
                _vehiclePremiumRepository.Update(vehiclePremium);
                await _vehicleRepository.SaveChangesAsync();
            }
        }

        private async Task UpdateManuscriptProratedPremiums(RiskAmounts riskAmounts, RiskDetail newRiskDetail)
        {
            foreach (var riskAmount in riskAmounts.Amounts.Where(x => x.AmountReduction < 0))
            {
                if (riskAmount.Info.Kind != AmountKind.ManuscriptAL &&
                    riskAmount.Info.Kind != AmountKind.ManuscriptPD) continue;

                await _riskManuscriptsRepository.CancelManuscript(newRiskDetail.Id, (long)riskAmount.Info.Id, riskAmount.CancellationDate.Value, riskAmount.CancelledAmount);
            }
        }

        private string GetAmountSubTypeId(AmountKind premiumKind)
        {
            switch (premiumKind)
            {
                case AmountKind.VehicleAL: return BillingConstants.VehicleALP;
                case AmountKind.VehiclePDCollision: return BillingConstants.VehiclePDPColl;
                case AmountKind.VehiclePDComprehensive: return BillingConstants.VehiclePDPCompFt;
                case AmountKind.VehicleCargo: return BillingConstants.VehicleCP;
                case AmountKind.GeneralLiability: return BillingConstants.GeneralLiabilityPremium;
                case AmountKind.TrailerInterchange: return BillingConstants.TrailerInterchangePremium;
                case AmountKind.NonOwnedTrailer: return BillingConstants.NonOwnedTrailerPremium;
                case AmountKind.ManuscriptAL: return BillingConstants.ALManuscriptPremium;
                case AmountKind.ManuscriptPD: return BillingConstants.PDManuscriptPremium;
                case AmountKind.AdditionalPremiumAI: return BillingConstants.AdditionalInsured;
                case AmountKind.AdditionalPremiumPNC: return BillingConstants.PrimaryAndNoncontributory;
                case AmountKind.AdditionalPremiumWOS: return BillingConstants.WaiverOfSubrogation;
                case AmountKind.PDHired: return BillingConstants.HiredPhysicalDamageEndst;
                case AmountKind.PDTrailer: return BillingConstants.TrailerInterchangeEndst;
                case AmountKind.ServiceFeeAL: return BillingConstants.ServiceFeeAL;
                case AmountKind.ServiceFeePD: return BillingConstants.ServiceFeePD;
                default: return null;
            }
        }

        private async Task AddVehiclePremiumsFees(RiskAmounts riskAmounts, RiskDetail riskDetail)
        {
            foreach (var vehicle in riskDetail.Vehicles)
            {
                var vehiclePremiums = await _amountFactory.CreateVehiclePremiums(riskDetail, vehicle, riskAmounts.PolicyPeriod);
                riskAmounts.Add(vehiclePremiums);

                var vehicleFees = await _amountFactory.CreateVehicleFees(vehicle, riskDetail);
                riskAmounts.Add(vehicleFees);
            }
        }

        private async Task AddManuscriptsAsync(RiskAmounts riskAmounts, RiskDetail riskDetail)
        {
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetail.Id);

            foreach (var manuscript in manuscripts)
            {
                IAmount manuscriptPremium = await _amountFactory.CreateManuscriptAsync(manuscript, riskAmounts.PolicyPeriod);
                riskAmounts.Add(manuscriptPremium);
            }
        }

        private async Task AddAdditionalPremiumsAsync(RiskAmounts riskAmounts, RiskDetail riskDetail)
        {
            var additionalInterests = (await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetail.Id))?.ToList()
                    ?? new List<RiskAdditionalInterest>();

            // if there are no records, and yet blankets are selected. Create a placeholder.
            if (additionalInterests.Count < 1)
            {
                var tmpAdditionalInterest = new RiskAdditionalInterest()
                {
                    Id = riskDetail.Id,
                    IsPrimaryNonContributory = riskDetail.IsPrimaryNonContributory ?? false,
                    AdditionalInterestTypeId = (riskDetail.IsAdditionalInsured ?? false) ? AdditionalInterestType.AdditionalInsured.Id : ((short)-1),
                    IsWaiverOfSubrogation = riskDetail.IsWaiverOfSubrogation ?? false
                };

                additionalInterests.Add(tmpAdditionalInterest);
            }

            var submission = await _riskDetailRepository.GetSubmissionRiskDetailAsync(riskDetail.RiskId);
            var additionalInterestsSubmission = (await _riskAdditionalInterestRepository.GetAllIncludeAsync(submission.Id));

            // Additional insured
            bool isAIBlanket = riskDetail.IsAdditionalInsured == true;
            var aiList = additionalInterests.Where(i => i.AdditionalInterestTypeId == AdditionalInterestType.AdditionalInsured.Id).ToList();
            var aiListAvailable = aiList.Where(x => x.RemoveProcessDate == null).ToList(); // Check if all wos are deleted to allow blanket removal
            var aiCount = aiList.Count();
            if (isAIBlanket && aiCount > 0)
            {
                aiCount = 1;
                if (aiListAvailable.Count() > 0) aiList = aiListAvailable; // If Blanket yes and ai available
            }
            for (int i = 0; i < aiCount; i++)
            {
                bool fromSubmission = additionalInterestsSubmission.Any(x => x.AddProcessDate.Ticks == aiList[i].AddProcessDate.Ticks);
                IAmount premium = _amountFactory.CreateAdditionalPremium(aiList[i], AdditionalPremiumType.AdditionalInsured, isAIBlanket, isAIBlanket ? true : fromSubmission);
                riskAmounts.Add(premium);
            }

            // Waiver of Subrogation
            bool isWoSBlanket = riskDetail.IsWaiverOfSubrogation == true;
            var wosList = additionalInterests.Where(i => i.IsWaiverOfSubrogation).ToList();
            var wosListAvailable = wosList.Where(x => x.RemoveProcessDate == null).ToList(); // Check if all wos are deleted to allow blanket removal
            var wosCount = wosList.Count();
            if (isWoSBlanket && wosCount > 0)
            {
                wosCount = 1;
                if (wosListAvailable.Count() > 0) wosList = wosListAvailable; // If Blanket yes and wos available
            }
            for (int i = 0; i < wosCount; i++)
            {
                bool fromSubmission = additionalInterestsSubmission.Any(x => x.AddProcessDate.Ticks == wosList[i].AddProcessDate.Ticks);
                IAmount premium = _amountFactory.CreateAdditionalPremium(wosList[i], AdditionalPremiumType.WaiverOfSubrogation, isWoSBlanket, isWoSBlanket ? true : fromSubmission);
                riskAmounts.Add(premium);
            }

            // Primary Non-Contributory
            bool isPnCBlanket = riskDetail.IsPrimaryNonContributory == true;
            var pncList = additionalInterests.Where(i => i.IsPrimaryNonContributory).ToList();
            var pncListtAvailable = pncList.Where(x => x.RemoveProcessDate == null).ToList(); // Check if all wos are deleted to allow blanket removal
            var pncCount = pncList.Count();
            if (isPnCBlanket && pncCount > 0)
            {
                pncCount = 1;
                if (pncListtAvailable.Count() > 0) pncList = pncListtAvailable; // If Blanket yes and pnc available
            }
            for (int i = 0; i < pncCount; i++)
            {
                bool fromSubmission = additionalInterestsSubmission.Any(x => x.AddProcessDate.Ticks == pncList[i].AddProcessDate.Ticks);
                IAmount premium = _amountFactory.CreateAdditionalPremium(pncList[i], AdditionalPremiumType.PrimaryNonContributory, isPnCBlanket, isPnCBlanket ? true : fromSubmission);
                riskAmounts.Add(premium);
            }
        }
    }
}
