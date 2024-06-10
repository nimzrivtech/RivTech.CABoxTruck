using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Policy.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class PolicyHistoryService : IPolicyHistoryService
    {
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILog4NetService _errorLogservice;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly ICancellationReasonsRepository _cancellationReasonsRepository;
        private readonly ICancellationBreakdownRepository _cancellationBreakdownRepository;
        private readonly IReinstatementBreakdownRepository _reinstatementBreakdownRepository;
        private readonly IBillingService _billingService;

        public PolicyHistoryService(IPolicyHistoryRepository policyHistoryRepository,
                                    IRiskDetailRepository riskDetailRepository,
                                    IRiskManuscriptsRepository riskManuscriptsRepository,
                                    ICancellationReasonsRepository cancellationReasonsRepository,
                                    IMapper mapper,
                                    ILog4NetService errorLogservice,
                                    ICancellationBreakdownRepository cancellationBreakdownRepository,
                                    IReinstatementBreakdownRepository reinstatementBreakdownRepository,
                                    IBillingService billingService)
        {
            _policyHistoryRepository = policyHistoryRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _cancellationReasonsRepository = cancellationReasonsRepository;
            _mapper = mapper;
            _errorLogservice = errorLogservice;
            _cancellationBreakdownRepository = cancellationBreakdownRepository;
            _reinstatementBreakdownRepository = reinstatementBreakdownRepository;
            _billingService = billingService;
        }
        public async Task<PolicyHistoryDTO> GetFirstIssuance(Guid riskId)
        {
            var history = await _policyHistoryRepository.GetFirstIssuance(riskId);
            return _mapper.Map<PolicyHistoryDTO>(history);
        }
        public async Task<PolicyHistoryDTO> GetLastActiveByRiskId(Guid riskId)
        {
            var history = await _policyHistoryRepository.GetLastActive(riskId);
            return _mapper.Map<PolicyHistoryDTO>(history);
        }
        public async Task<PolicyHistoryDTO> GetLastCancelledByRiskId(Guid riskId)
        {
            var history = await _policyHistoryRepository.GetLastCancelled(riskId);
            return _mapper.Map<PolicyHistoryDTO>(history);
        }

        public async Task<List<PolicyHistoryDTO>> GetEndorsementHistoryByRiskId(Guid riskId)
        {
            var result = await _policyHistoryRepository.GetAsync(x => x.RiskId == riskId.ToString());
            return _mapper.Map<List<PolicyHistoryDTO>>(result);
        }

        public async Task<int> CreatePolicyHistory(Guid riskId, RiskDetail forEndorsementRiskDetail, Guid previousRiskDetailId, RiskCoveragePremium riskCovPrem, DateTime effectiveDate, string policyStatus, string details = null)
        {
            var transactionNumber = 0;
            var endorsementNumber = 0;
            try
            {
                if (forEndorsementRiskDetail == null) return endorsementNumber;
                if (riskCovPrem == null) return endorsementNumber;

                endorsementNumber = forEndorsementRiskDetail.EndorsementNumber - 1;

                decimal currentPremium = 0m;
                decimal newPremium = 0m;
                decimal proratedPremium = 0m;
                decimal premiumChange = 0m;
                decimal totalMinimumAdjustment = 0m;
                decimal totalShortRatePremium = 0m;
                var currentEndorsements = await GetEndorsementHistoryByRiskId(forEndorsementRiskDetail.RiskId);

                if (currentEndorsements.Count < 1)
                {
                    newPremium = (riskCovPrem.Premium ?? 0);
                    currentPremium = 0;
                    proratedPremium = newPremium;
                }
                else
                {
                    var deletedProratedManusciptsAnnualPremium = GetDeletedProratedManuscriptPremium(currentEndorsements, forEndorsementRiskDetail.Id);
                    if (policyStatus == RiskStatus.Canceled)
                    {
                        var policyChanges = new List<string>();
                        var cancellationType = details; // if cancelled, cancellation type is passed on this parameter
                        var cancellationReason = _cancellationReasonsRepository.GetAsync(x => x.Id == Convert.ToInt32(cancellationType)).Result.FirstOrDefault();
                        policyChanges.Add($"Cancellation Reason: {cancellationReason.Description}");

                        var cancellationBreakdown = _cancellationBreakdownRepository.GetByRiskDetailId(previousRiskDetailId);
                        totalMinimumAdjustment = cancellationBreakdown.Sum(x => x.MinimumPremiumAdjustment.RoundTo2Decimals());
                        if (totalMinimumAdjustment > 0)
                        {
                            policyChanges.Add($"Added Minimum Premium Adjustment {totalMinimumAdjustment:C}");
                        } 
                        else
                        {
                            totalMinimumAdjustment = 0; // set to 0 if value is not greater than 1
                        }
                        totalShortRatePremium = cancellationBreakdown.Sum(x => x.ShortRatePremium.RoundTo2Decimals());
                        if (totalShortRatePremium > 0)
                        {
                            policyChanges.Add($"Added Short Rate Premium {totalShortRatePremium:C}");
                        }
                        else
                        {
                            totalShortRatePremium = 0; // set to 0 if value is not greater than 1
                        }
                        details = JsonConvert.SerializeObject(policyChanges.ToArray());

                        // set to 0, manuscripts are already included
                        deletedProratedManusciptsAnnualPremium = 0;
                    }

                    if (policyStatus == RiskStatus.Reinstated)
                    {
                        var policyChanges = new List<string>();
                        var reinstatementBreakdown = _reinstatementBreakdownRepository.GetByRiskDetailId(previousRiskDetailId);
                        var reinstatementFee = reinstatementBreakdown.FirstOrDefault(x => x.AmountSubTypeId == FeeKind.ReinstatementFee.Id);
                        if ((reinstatementFee?.Amount ?? 0) > 0)
                        {
                            policyChanges.Add($"Added Reinstatement Fee {reinstatementFee.Amount:C}");
                        }
                        details = JsonConvert.SerializeObject(policyChanges.ToArray());

                        // set effective date to last cancellation date
                        DateTime lastCancellationEffectiveDate = currentEndorsements.Where(x => x.PolicyStatus == RiskDetailStatus.Canceled).OrderByDescending(x => x.DateCreated)
                                                                                    .Select(x => x.EffectiveDate.Value).FirstOrDefault();
                        effectiveDate = lastCancellationEffectiveDate;
                    }

                    newPremium = (riskCovPrem.Premium ?? 0) + deletedProratedManusciptsAnnualPremium;
                    currentPremium = currentEndorsements.OrderByDescending(x => x.DateCreated).Select(x => x.ProratedPremium ?? 0).FirstOrDefault();
                    proratedPremium = (policyStatus == "Rewrite") ? 0 : (riskCovPrem.TotalPremium ?? 0) + (totalMinimumAdjustment + totalShortRatePremium);
                }

                endorsementNumber = currentEndorsements.Count(x => x.PolicyStatus == RiskDetailStatus.Active);
                transactionNumber = currentEndorsements.Count() + 1; 

                var billingSummary = await _billingService.GetBillingSummaryAsync(riskId);
                proratedPremium = billingSummary.PremiumAmount;

                premiumChange = (proratedPremium - currentPremium);

                var policyHistory = await _policyHistoryRepository.AddAsync(new PolicyHistory()
                {
                    RiskDetailId = forEndorsementRiskDetail.Id,
                    DateCreated = DateTime.Now,
                    TransactionNumber = $"{transactionNumber}",
                    EndorsementNumber = $"{endorsementNumber}", // start frm 0
                    Details = details,
                    CurrentPremium = currentPremium,
                    NewPremium = newPremium,
                    ProratedPremium = proratedPremium,
                    PremiumChange = premiumChange,
                    EffectiveDate = effectiveDate,
                    PolicyStatus = policyStatus,
                    CreatedBy = $"{Data.Common.Services.GetCurrentUser()}",
                    PreviousRiskDetailId = $"{previousRiskDetailId}",
                    RiskId = $"{forEndorsementRiskDetail.RiskId}"
                });

                await _policyHistoryRepository.SaveChangesAsync();

                if (policyHistory != null)
                {
                    await _billingService.UpdateTransactionPolicyHistoryId(riskId, policyHistory.Id);
                }
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
            }

            return endorsementNumber;
        }

        private decimal GetDeletedProratedManuscriptPremium(List<PolicyHistoryDTO> currentEndorsements, Guid policyDetailId)
        {
            var lastEndorsementEffectiveDate = currentEndorsements.OrderByDescending(x => x.DateCreated).Select(x => x.EffectiveDate).FirstOrDefault();
            var lastEndorsementCreatedDate = currentEndorsements.OrderByDescending(x => x.DateCreated).Select(x => x.DateCreated).FirstOrDefault();
            var deletedProratedManuscipts = _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == policyDetailId).Result;
            var validDeletedProratedManusciptsAnnualPremium = deletedProratedManuscipts.Where(x => x.IsDeleted && x.IsProrate && x.ProratedPremium != 0
                                                                                                && x.ExpirationDate >= lastEndorsementEffectiveDate
                                                                                                && x.DeletedDate >= lastEndorsementCreatedDate);

            return (validDeletedProratedManusciptsAnnualPremium.Count() > 0) ? validDeletedProratedManusciptsAnnualPremium.Sum(x => x.Premium) : 0;
        }
    }
}
