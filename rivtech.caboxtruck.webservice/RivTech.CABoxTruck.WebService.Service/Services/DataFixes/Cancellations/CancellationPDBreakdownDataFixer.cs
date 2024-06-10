using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.DataFixes.Cancellations
{
    public interface ICancellationPDBreakdownDataFixer
    {
        Task FixAsync();
        Task FixCollCompByRiskDetailAsync(List<Guid> riskDetailIds);
    }

    public class CancellationPDBreakdownDataFixer : ICancellationPDBreakdownDataFixer
    {
        private readonly ILogger<CancellationPDBreakdownDataFixer> _logger;
        private readonly ICancellationBreakdownRepository _cancellationBreakdownRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRiskVehiclePremiumTransactionRepository _riskVehiclePremiumTransactionRepository;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IBindingRepository _bindingRepository;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;

        public CancellationPDBreakdownDataFixer(
            ILogger<CancellationPDBreakdownDataFixer> logger,
            ICancellationBreakdownRepository cancellationBreakdownRepository,
            IVehicleRepository vehicleRepository,
            IRiskVehiclePremiumTransactionRepository riskVehiclePremiumTransactionRepository,
            IPolicyHistoryRepository policyHistoryRepository,
            IRiskDetailRepository riskDetailRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IBindingRepository bindingRepository,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository
        )
        {
            _logger = logger;
            _cancellationBreakdownRepository = cancellationBreakdownRepository;
            _vehicleRepository = vehicleRepository;
            _riskVehiclePremiumTransactionRepository = riskVehiclePremiumTransactionRepository;
            _policyHistoryRepository = policyHistoryRepository;
            _riskDetailRepository = riskDetailRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _bindingRepository = bindingRepository;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
        }

        public async Task FixAsync()
        {
            _logger.LogInformation("Running PD Cancellation breakdown data fix.");
            var breakdowns = await _cancellationBreakdownRepository.GetAllByAmountTypeAsync(BillingConstants.VehiclePDP);
            breakdowns = breakdowns.Where(x => x != null);

            var riskBreakdowns = breakdowns.GroupBy(x => x.RiskDetailId);

            foreach (var riskBreakdown in riskBreakdowns)
            {
                var riskDetail = (await _riskDetailRepository.GetAsync(x => x.Id == riskBreakdown.Key))?.FirstOrDefault();

                try
                {
                    _logger.LogInformation("Updating PD cancellation breakdown of RiskDetail {0}.", riskDetail.SubmissionNumber);
                    var newBreakdowns = new List<CancellationBreakdown>();

                    var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
                    var binding = await _bindingRepository.FindAsync(b => b.RiskId == riskDetail.RiskId);

                    DateRange policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
                    RiskAmounts riskAmounts = new RiskAmounts(policyPeriod, binding.MinimumEarned);

                    var policyHistories = await _policyHistoryRepository.GetByRiskId(riskDetail.RiskId);

                    var cancelHistory = policyHistories.ToList().FirstOrDefault(x => 
                        x.PreviousRiskDetailId == riskDetail.Id.ToString()
                        && x.PolicyStatus == "Canceled" 
                    );
                    bool isFlatCancel = new[] {
                        "Cancel Flat",
                        "Cancellation Information not found."
                    }.Any(x => cancelHistory.Details.Contains(x));


                    if (isFlatCancel && riskDetail.EndorsementNumber == 1)
                    {
                        _logger.LogInformation("Skipping rewritten {0} for PD cancellation brekadown data fix.", riskDetail.SubmissionNumber);
                        continue;
                    }

                    var previousHistory = await _policyHistoryRepository.GetPreviousActiveHistory(cancelHistory.Id);

                    bool hasShortRate = riskBreakdown.First().ShortRatePremium != 0;
                    var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetail.Id);

                    foreach (var vehicle in vehicles)
                    {
                        if (vehicle is null)
                        {
                            _logger.LogInformation("No vehicle found for riskdetail {0} and VIN {1}.", riskDetail.Id, vehicle.VehicleDescriptionId);
                            continue;
                        }

                        RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(Guid.Parse(previousHistory.PreviousRiskDetailId), vehicle.MainId);
                        if (!(riskVehiclePremiums?.Any() ?? false))
                        {
                            _logger.LogInformation("No vehicle premiums found for riskdetail {0} and VIN {1}.", riskDetail.Id, vehicle.VehicleDescriptionId);
                            continue;
                        }
                        bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
                        RiskVehiclePremium submissionRiskVehiclePremium = null;

                        if (isFromSubmission)
                        {
                            submissionRiskVehiclePremium = riskVehiclePremiums.First(x => x.Order == 1);
                        }

                        // PD Comp FT
                        var comp = new VehicleProratedAmount(
                            riskVehiclePremiums,
                            info: new AmountInfo(vehicle.Id, AmountKind.VehiclePDComprehensive, vehicle.VIN),
                            policyPeriod,
                            submissionRiskVehiclePremium);
                        riskAmounts.Add(comp);

                        // PD Collision
                        var coll = new VehicleProratedAmount(
                            riskVehiclePremiums,
                            info: new AmountInfo(vehicle.Id, AmountKind.VehiclePDCollision, vehicle.VIN),
                            policyPeriod,
                            submissionRiskVehiclePremium);
                        riskAmounts.Add(coll);

                    }

                    riskAmounts.Cancel(riskDetail.CancellationDate.Value, hasShortRate, isFlatCancel);

                    // use to validate canceled amounts that has no reduction but has a cancellation amount, as these should be included in cancellation calculation process
                    var vehiclePremiumTypes = new AmountKind[] {
                        AmountKind.VehiclePDCollision,
                        AmountKind.VehiclePDComprehensive,
                    };

                    // Create and save cancellation breakdowns
                    foreach (var premium in riskAmounts.Amounts.Where(x => x.AmountReduction < 0 || vehiclePremiumTypes.Contains(x.Info.Kind)))
                    {
                        // do not save breakdowns with no amounts.
                        if (premium.Amount == 0 && premium.CancelledAmount == 0)
                            continue;

                        var bd = new CancellationBreakdown
                        {
                            RiskDetailId = riskDetail.Id,
                            AmountSubTypeId = GetAmountSubTypeId(premium.Info.Kind),
                            Description = premium.Info.Description,
                            ActiveAmount = premium.Amount,
                            CancelledAmount = premium.CancelledAmount,
                            AmountReduction = premium.AmountReduction,
                        };

                        if (premium is ProratedAmount proratedPremium)
                        {
                            bd.AnnualMinimumPremium = proratedPremium.AnnualMinimumEarned;
                            bd.MinimumPremium = proratedPremium.MinimumEarned;
                            bd.MinimumPremiumAdjustment = proratedPremium.MinimumEarnedAdjustment;
                            bd.ShortRatePremium = proratedPremium.ShortRatePremium;
                        }

                        newBreakdowns.Add(bd);
                    }


                    if(newBreakdowns.Any())
                        await _cancellationBreakdownRepository.AddRangeAsync(newBreakdowns);

                    if(riskBreakdown.Any())
                        _cancellationBreakdownRepository.RemoveRange(riskBreakdown.ToList());

                    await _cancellationBreakdownRepository.SaveChangesAsync();
                    _logger.LogInformation("Done updating PD cancellation breakdown of RiskDetail {0}.", riskDetail.SubmissionNumber);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while breaking down PD cancellation of riskDetail {0}", riskDetail.SubmissionNumber);
                }
            }
        }


        private string GetAmountSubTypeId(AmountKind premiumKind)
        {
            switch (premiumKind)
            {
                case AmountKind.VehiclePDCollision: return BillingConstants.VehiclePDPColl;
                case AmountKind.VehiclePDComprehensive: return BillingConstants.VehiclePDPCompFt;
                default: return null;
            }
        }

        public async Task FixCollCompByRiskDetailAsync(List<Guid> riskDetailIds)
        {
            _logger.LogInformation("Running PD Cancellation breakdown data fix.");

            foreach (var riskDetailId in riskDetailIds)
            {
                var breakdowns = await _cancellationBreakdownRepository.GetAllByRiskDetailIdAmountTypeAsync(riskDetailId, BillingConstants.VehiclePDP);
                breakdowns = breakdowns.Where(x => x != null);

                var riskBreakdowns = breakdowns.GroupBy(x => x.RiskDetailId);

                foreach (var riskBreakdown in riskBreakdowns)
                {
                    var riskDetail = (await _riskDetailRepository.GetAsync(x => x.Id == riskBreakdown.Key))?.FirstOrDefault();

                    try
                    {
                        _logger.LogInformation("Updating PD cancellation breakdown of RiskDetail {0}.", riskDetail.SubmissionNumber);
                        var newBreakdowns = new List<CancellationBreakdown>();

                        var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
                        var binding = await _bindingRepository.FindAsync(b => b.RiskId == riskDetail.RiskId);

                        DateRange policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
                        RiskAmounts riskAmounts = new RiskAmounts(policyPeriod, binding.MinimumEarned);

                        var policyHistories = await _policyHistoryRepository.GetByRiskId(riskDetail.RiskId);

                        var cancelHistory = policyHistories.ToList().FirstOrDefault(x =>
                            x.PreviousRiskDetailId == riskDetail.Id.ToString()
                            && (x.PolicyStatus == "Canceled" || x.PolicyStatus == "Rewrite")
                        );
                        bool isFlatCancel = new[] {
                        "Cancel Flat",
                        "Cancellation Information not found."
                    }.Any(x => cancelHistory.Details.Contains(x));


                        //if (isFlatCancel && riskDetail.EndorsementNumber == 1)
                        //{
                        //    _logger.LogInformation("Skipping rewritten {0} for PD cancellation brekadown data fix.", riskDetail.SubmissionNumber);
                        //    continue;
                        //}

                        var previousHistory = await _policyHistoryRepository.GetPreviousActiveHistory(cancelHistory.Id);

                        bool hasShortRate = riskBreakdown.First().ShortRatePremium != 0;
                        var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetail.Id);

                        foreach (var vehicle in vehicles)
                        {
                            if (vehicle is null)
                            {
                                _logger.LogInformation("No vehicle found for riskdetail {0} and VIN {1}.", riskDetail.Id, vehicle.VehicleDescriptionId);
                                continue;
                            }

                            RiskVehiclePremiumCollection riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(Guid.Parse(previousHistory.PreviousRiskDetailId), vehicle.MainId);
                            if (!(riskVehiclePremiums?.Any() ?? false))
                            {
                                _logger.LogInformation("No vehicle premiums found for riskdetail {0} and VIN {1}.", riskDetail.Id, vehicle.VehicleDescriptionId);
                                continue;
                            }
                            bool isFromSubmission = await _vehicleRepository.IsFromSubmission(vehicle.Id);
                            RiskVehiclePremium submissionRiskVehiclePremium = null;

                            if (isFromSubmission)
                            {
                                submissionRiskVehiclePremium = riskVehiclePremiums.First(x => x.Order == 1);
                            }

                            // PD Comp FT
                            var comp = new VehicleProratedAmount(
                                riskVehiclePremiums,
                                info: new AmountInfo(vehicle.Id, AmountKind.VehiclePDComprehensive, vehicle.VIN),
                                policyPeriod,
                                submissionRiskVehiclePremium);
                            riskAmounts.Add(comp);

                            // PD Collision
                            var coll = new VehicleProratedAmount(
                                riskVehiclePremiums,
                                info: new AmountInfo(vehicle.Id, AmountKind.VehiclePDCollision, vehicle.VIN),
                                policyPeriod,
                                submissionRiskVehiclePremium);
                            riskAmounts.Add(coll);

                        }

                        riskAmounts.Cancel(riskDetail.CancellationDate.Value, hasShortRate, isFlatCancel);

                        // use to validate canceled amounts that has no reduction but has a cancellation amount, as these should be included in cancellation calculation process
                        var vehiclePremiumTypes = new AmountKind[] {
                        AmountKind.VehiclePDCollision,
                        AmountKind.VehiclePDComprehensive,
                    };

                        // Create and save cancellation breakdowns
                        foreach (var premium in riskAmounts.Amounts.Where(x => x.AmountReduction < 0 || vehiclePremiumTypes.Contains(x.Info.Kind)))
                        {
                            // do not save breakdowns with no amounts.
                            if (premium.Amount == 0 && premium.CancelledAmount == 0)
                                continue;

                            var bd = new CancellationBreakdown
                            {
                                RiskDetailId = riskDetail.Id,
                                AmountSubTypeId = GetAmountSubTypeId(premium.Info.Kind),
                                Description = premium.Info.Description,
                                ActiveAmount = premium.Amount,
                                CancelledAmount = premium.CancelledAmount,
                                AmountReduction = premium.AmountReduction,
                            };

                            if (premium is ProratedAmount proratedPremium)
                            {
                                bd.AnnualMinimumPremium = proratedPremium.AnnualMinimumEarned;
                                bd.MinimumPremium = proratedPremium.MinimumEarned;
                                bd.MinimumPremiumAdjustment = proratedPremium.MinimumEarnedAdjustment;
                                bd.ShortRatePremium = proratedPremium.ShortRatePremium;
                            }

                            newBreakdowns.Add(bd);
                        }


                        if (newBreakdowns.Any())
                            await _cancellationBreakdownRepository.AddRangeAsync(newBreakdowns);

                        if (riskBreakdown.Any())
                            _cancellationBreakdownRepository.RemoveRange(riskBreakdown.ToList());

                        await _cancellationBreakdownRepository.SaveChangesAsync();
                        _logger.LogInformation("Done updating PD cancellation breakdown of RiskDetail {0}.", riskDetail.SubmissionNumber);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error while breaking down PD cancellation of riskDetail {0}", riskDetail.SubmissionNumber);
                    }
                }
            
            }
        }

    }

}
