using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RiskStatus = RivTech.CABoxTruck.WebService.DTO.Constants.RiskStatus;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class QuoteOptionsService : IQuoteOptionsService
    {
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskAdditionalInterestRepository _riskAdditionalInterestRepository;
        private readonly IBillingService _billingService;
        private readonly ITaxService _taxService;
        private readonly IMapper _mapper;
        private Int16 optionNumber = 1;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;

        public QuoteOptionsService(IRiskCoverageRepository riskCoverageRepositoryy,
            IRiskDetailRepository riskDetailRepository,
            IRiskAdditionalInterestRepository riskAdditionalInterestRepository,
            IMapper mapper,
            IBillingService billingService,
            ITaxService taxService,
            IPolicyHistoryRepository policyHistoryRepository)
        {
            _riskCoverageRepository = riskCoverageRepositoryy;
            _riskDetailRepository = riskDetailRepository;
            _riskAdditionalInterestRepository = riskAdditionalInterestRepository;
            _billingService = billingService;
            _mapper = mapper;
            _taxService = taxService;
            _policyHistoryRepository = policyHistoryRepository;
        }

        public async Task<List<RiskCoverageDTO>> GetByIdAsync(Guid id)
        {
            var result = await _riskCoverageRepository.GetAsync(x => x.RiskDetailId == id);
            return _mapper.Map<List<RiskCoverageDTO>>(result);
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _riskCoverageRepository.AnyAsync(x => x.RiskDetailId == id);
        }

        public async Task<RiskCoverageListDTO> InsertAsync(RiskCoverageListDTO model)
        {
            Int16? option1CargoLimitId = 0;
            Int16? option1RefCargoLimitId = 0;

            foreach (var item in model.RiskCoverages)
            {
                if (item.OptionNumber == 1)
                {
                    option1CargoLimitId = item.CargoLimitId;
                    option1RefCargoLimitId = item.RefCargoLimitId;
                }

                item.RiskDetailId = model.RiskDetailId;
                item.CargoLimitId = option1CargoLimitId == LimitConstants.CargoNoneCoverageId ? LimitConstants.CargoNoneCoverageId : item.CargoLimitId;
                item.RefCargoLimitId = option1CargoLimitId == LimitConstants.CargoNoneCoverageId || option1RefCargoLimitId == LimitConstants.RefNoneCoverageId ? LimitConstants.RefNoneCoverageId : item.RefCargoLimitId;

                await _riskCoverageRepository.AddAsync(_mapper.Map<RiskCoverage>(item));
            }

            await _riskCoverageRepository.SaveChangesAsync();
            return model;
        }

        public async Task<RiskCoverageListDTO> UpdateAsync(RiskCoverageListDTO model)
        {
            List<short> optionList = new List<short>();
            var objCoverage = new List<RiskCoverage>();

            Int16? option1CargoLimitId = 0;
            Int16? option1RefCargoLimitId = 0;

            foreach (var item in model.RiskCoverages)
            {
                var data = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == model.RiskDetailId && x.OptionNumber == item.OptionNumber);
                if (data == null)
                {
                    data = new RiskCoverage();
                }

                if (item.OptionNumber == 1)
                {
                    option1CargoLimitId = item.CargoLimitId;
                    option1RefCargoLimitId = item.RefCargoLimitId;
                }

                data.RiskDetailId = model.RiskDetailId;
                data.ALSymbolId = item.ALSymbolId;
                data.PDSymbolId = item.PDSymbolId;
                data.AutoBILimitId = item.AutoBILimitId;
                data.AutoPDLimitId = item.AutoPDLimitId;
                data.LiabilityDeductibleId = item.LiabilityDeductibleId;
                data.MedPayLimitId = item.MedPayLimitId;
                data.PIPLimitId = item.PIPLimitId;
                data.UMBILimitId = item.UMBILimitId;
                data.UMPDLimitId = item.UMPDLimitId;
                data.UIMBILimitId = item.UIMBILimitId;
                data.UIMPDLimitId = item.UIMPDLimitId;
                data.CoveredSymbolsId = item.CoveredSymbolsId;
                data.SpecifiedPerils = item.SpecifiedPerils;
                data.CompDeductibleId = item.CompDeductibleId;
                data.FireDeductibleId = item.FireDeductibleId;
                data.CollDeductibleId = item.CollDeductibleId;
                data.GLBILimitId = item.GLBILimitId;
                data.CargoLimitId = option1CargoLimitId == LimitConstants.CargoNoneCoverageId ? LimitConstants.CargoNoneCoverageId : item.CargoLimitId;
                data.RefCargoLimitId = option1CargoLimitId == LimitConstants.CargoNoneCoverageId || option1RefCargoLimitId == LimitConstants.RefNoneCoverageId ? LimitConstants.RefNoneCoverageId : item.RefCargoLimitId;
                data.TrailerInterchangeLimitId = item.TrailerInterchangeLimitId;
                data.NonOwnedTrailerLimitId = item.NonOwnedTrailerLimitId;
                data.TrailerTypeLimitId = item.TrailerTypeLimitId;
                data.PollutionCovLimitId = item.PollutionCovLimitId;
                data.EndorsementNumber = item.EndorsementNumber;
                data.OptionNumber = item.OptionNumber;
                data.NumAddtlInsured = item.NumAddtlInsured;
                data.WaiverSubrogation = item.WaiverSubrogation;
                data.PrimeNonContribEndorsement = item.PrimeNonContribEndorsement;
                data.BrokerCommisionAL = string.IsNullOrEmpty(item.BrokerCommisionAL) ? "0" : $"{Convert.ToDecimal(item.BrokerCommisionAL):F1}";
                data.BrokerCommisionPD = string.IsNullOrEmpty(item.BrokerCommisionPD) ? "0" : $"{Convert.ToDecimal(item.BrokerCommisionPD):F1}";
                data.GlScheduleRF = item.GlScheduleRF;
                data.GlExperienceRF = item.GlExperienceRF;
                data.CargoScheduleRF = item.CargoScheduleRF;
                data.CargoExperienceRF = item.CargoExperienceRF;
                data.ApdScheduleRF = item.ApdScheduleRF;
                data.ApdExperienceRF = item.ApdExperienceRF;
                data.DepositPct = item.DepositPct;
                data.DepositPremium = item.DepositPremium;
                data.NumberOfInstallments = item.NumberOfInstallments;
                data.LiabilityExperienceRatingFactor = item.LiabilityExperienceRatingFactor;
                data.LiabilityScheduleRatingFactor = item.LiabilityScheduleRatingFactor;
                data.SubjectivitiesId = item.SubjectivitiesId;
                data.ARSCommission = item.ARSCommission;
                data.CargoLimitSymbolId = item.CargoLimitSymbolId;
                data.RefCargoLimitSymbolId = item.RefCargoLimitSymbolId;
                data.TrailerInterchangeLimitSymbolId = item.TrailerInterchangeLimitSymbolId;
                data.NonOwnedTrailerLimitSymbolId = item.NonOwnedTrailerLimitSymbolId;

                var result = _riskCoverageRepository.Update(data);
                optionList.Add(item.OptionNumber.Value);
                objCoverage.Add(result);
            }

            await _riskCoverageRepository.SaveChangesAsync();
            var deleteOptions = await _riskCoverageRepository.GetAsync(x => x.RiskDetailId == model.RiskDetailId && !optionList.Contains(x.OptionNumber.Value));
            foreach (var item in deleteOptions)
            {
                _riskCoverageRepository.Remove(item);
            }
            await _riskCoverageRepository.SaveChangesAsync();

            var riskCoverages = await GetRiskCoverageAndPremiumByIdAsync(model.RiskDetailId);

            // update id use in premium rating
            //i shoould use mapper.map but not sure if something needs to retain from the model Ui
            objCoverage.ForEach(obj =>
            {
                model.RiskCoverages.ForEach(x =>
                {
                    if (x.OptionNumber == obj.OptionNumber)
                    {
                        x.Id = obj.Id;
                        x.RiskDetailId = obj.RiskDetailId;
                        x.LiabilityExperienceRatingFactor = obj.LiabilityExperienceRatingFactor;
                        x.LiabilityScheduleRatingFactor = obj.LiabilityScheduleRatingFactor;
                        x.RiskCoveragePremium = riskCoverages.FirstOrDefault(riskCoverage => riskCoverage.Id == x.Id)?.RiskCoveragePremium ?? null;
                    }
                });
            });

            return model;
        }

        public async Task<List<RiskCoverageDTO>> GetRiskCoverageAndPremiumByIdAsync(Guid id)
        {
            var result = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(id);
            return _mapper.Map<List<RiskCoverageDTO>>(result);
        }

        public async Task<List<RiskCoverageDTO>> GetRiskCoverageAndPremiumMetricsByRiskIdAsync(Guid riskId)
        {

            var result = await _riskCoverageRepository.GetRiskDetailsAsync(riskId);
            var history = await _policyHistoryRepository.GetByRiskId(riskId);

            var prevRiskDetails = history.Where(policyHistory => policyHistory.PolicyStatus != RiskStatus.Canceled && policyHistory.PolicyStatus != RiskStatus.Reinstated)
                                         .Select(policyHistory => policyHistory.PreviousRiskDetailId).ToList();

            var riskCoverages = result.Where(riskDetails => (bool)riskDetails.IsProcessed && prevRiskDetails.Contains(riskDetails.Id.ToString()))
                                      .Select(riskDetails => riskDetails.RiskCoverages.Where(riskCoverage => riskCoverage.OptionNumber == 1).FirstOrDefault()).ToList();

            foreach (var item in history)
            {
                //use OptionNumber as Endorsement Number
                foreach (var riskCoverage in riskCoverages)
                {
                    if (riskCoverage.RiskDetailId.ToString() == item.PreviousRiskDetailId)
                    {
                        riskCoverage.EndorsementNumber = short.Parse(item.EndorsementNumber);
                        riskCoverage.OptionNumber = short.Parse(item.EndorsementNumber);
                    }
                }
                //riskCoverages.Find(riskCoverage => riskCoverage.RiskDetailId == item.RiskDetailId).OptionNumber = short.Parse(item.EndorsementNumber);
            }
            riskCoverages.OrderByDescending(riskCoverages => riskCoverages.OptionNumber).ToList();
            return _mapper.Map<List<RiskCoverageDTO>>(riskCoverages);
        }
        public async Task<RiskCoverageDTO> CopyOptionAsync(Guid riskDetailId, int optionIdToCopy)
        {
            var options = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            RiskCoverage optionToCopy = options.FirstOrDefault(opt => opt.OptionNumber == optionIdToCopy);
            short optionNumber = (short)(options.OrderBy(x => x.OptionNumber).Last().OptionNumber.Value + 1);
            RiskCoverage optionCopy = optionToCopy.Copy(optionNumber);
            await _riskCoverageRepository.AddAsync(optionCopy);
            await _riskCoverageRepository.SaveChangesAsync();
            return _mapper.Map<RiskCoverageDTO>(optionCopy);
        }

        public async Task DeleteOptionAsync(Guid riskDetailId, Guid optionId)
        {
            var options = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            RiskCoverage optionToDelete = options.First(opt => opt.Id == optionId);
            _riskCoverageRepository.Remove(optionToDelete);
            foreach (var opt in options)
            {
                if (opt.OptionNumber > optionToDelete.OptionNumber)
                    opt.OptionNumber = (short?)(opt.OptionNumber - 1);
            }
            await _riskCoverageRepository.SaveChangesAsync();
        }

        public async Task UpdateHiredPhysicalDamage(RiskFormOtherDTO riskFormOther)
        {
            var riskCovs = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskFormOther.RiskDetailId);
            var hiredPhysicalDamage = _mapper.Map<HiredPhysdamDTO>(riskFormOther);

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                riskCov.RiskCoveragePremium.HiredPhysicalDamage = hiredPhysicalDamage.EstimatedAdditionalPremium;
                riskCov.RiskCoveragePremium.PhysicalDamagePremium += hiredPhysicalDamage.EstimatedAdditionalPremium;
                riskCov.RiskCoveragePremium.Premium += hiredPhysicalDamage.EstimatedAdditionalPremium;
                _riskCoverageRepository.Update(riskCov);
            }

            await _riskCoverageRepository.SaveChangesAsync();

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                await _billingService.UpdatePaymentOptions(riskCov.RiskDetailId, riskCov.OptionNumber);
                await _taxService.CalculateRiskTaxAsync(riskCov.RiskDetailId, riskCov.OptionNumber);
            }
        }

        public async Task UpdateManuscript(RiskManuscriptsDTO manuscript)
        {
            var riskCovs = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(manuscript.RiskDetailId);

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                if (manuscript.PremiumType == "PD")
                {
                    if (manuscript.IsProrate)
                    {
                        riskCov.RiskCoveragePremium.ProratedPDManuscriptPremium = manuscript.IsSelected.GetValueOrDefault()
                            ? riskCov.RiskCoveragePremium.ProratedPDManuscriptPremium + manuscript.ProratedPremium
                            : riskCov.RiskCoveragePremium.ProratedPDManuscriptPremium - manuscript.ProratedPremium;

                        riskCov.RiskCoveragePremium.ProratedPremium = manuscript.IsSelected.GetValueOrDefault()
                            ? riskCov.RiskCoveragePremium.ProratedPremium + manuscript.ProratedPremium
                            : riskCov.RiskCoveragePremium.ProratedPremium - manuscript.ProratedPremium;
                    }

                    riskCov.RiskCoveragePremium.PDManuscriptPremium = manuscript.IsSelected.GetValueOrDefault()
                        ? riskCov.RiskCoveragePremium.PDManuscriptPremium + manuscript.Premium
                        : riskCov.RiskCoveragePremium.PDManuscriptPremium - manuscript.Premium;

                    riskCov.RiskCoveragePremium.PhysicalDamagePremium = manuscript.IsSelected.GetValueOrDefault()
                        ? riskCov.RiskCoveragePremium.PhysicalDamagePremium + manuscript.Premium
                        : riskCov.RiskCoveragePremium.PhysicalDamagePremium - manuscript.Premium;
                }
                else
                {
                    if (manuscript.IsProrate)
                    {
                        riskCov.RiskCoveragePremium.ProratedALManuscriptPremium = manuscript.IsSelected.GetValueOrDefault()
                            ? riskCov.RiskCoveragePremium.ProratedALManuscriptPremium + manuscript.ProratedPremium
                            : riskCov.RiskCoveragePremium.ProratedALManuscriptPremium - manuscript.ProratedPremium;

                        riskCov.RiskCoveragePremium.ProratedPremium = manuscript.IsSelected.GetValueOrDefault()
                            ? riskCov.RiskCoveragePremium.ProratedPremium + manuscript.ProratedPremium
                            : riskCov.RiskCoveragePremium.ProratedPremium - manuscript.ProratedPremium;
                    }

                    riskCov.RiskCoveragePremium.ALManuscriptPremium = manuscript.IsSelected.GetValueOrDefault()
                        ? riskCov.RiskCoveragePremium.ALManuscriptPremium + manuscript.Premium
                        : riskCov.RiskCoveragePremium.ALManuscriptPremium - manuscript.Premium;

                    riskCov.RiskCoveragePremium.AutoLiabilityPremium = manuscript.IsSelected.GetValueOrDefault()
                        ? riskCov.RiskCoveragePremium.AutoLiabilityPremium + manuscript.Premium
                        : riskCov.RiskCoveragePremium.AutoLiabilityPremium - manuscript.Premium;
                }

                riskCov.RiskCoveragePremium.Premium = manuscript.IsSelected.GetValueOrDefault()
                    ? riskCov.RiskCoveragePremium.Premium + manuscript.Premium
                    : riskCov.RiskCoveragePremium.Premium - manuscript.Premium;

                _riskCoverageRepository.Update(riskCov);

            }

            await _riskCoverageRepository.SaveChangesAsync();

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                await _billingService.UpdatePaymentOptions(riskCov.RiskDetailId, riskCov.OptionNumber);
                await _taxService.CalculateRiskTaxAsync(riskCov.RiskDetailId, riskCov.OptionNumber);
            }
        }

        public async Task UpdateTrailerInterchange(RiskFormOtherDTO riskFormOther)
        {
            var riskCovs = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskFormOther.RiskDetailId);
            var trailerInterchange = _mapper.Map<TrailerInterchangeDTO>(riskFormOther);
            var trailerInterchangePremium = trailerInterchange.ComprePremium
                                          + trailerInterchange.LossPremium
                                          + trailerInterchange.CollisionPremium
                                          + trailerInterchange.FirePremium
                                          + trailerInterchange.FireTheftPremium;

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                riskCov.RiskCoveragePremium.TrailerInterchange = trailerInterchangePremium;
                riskCov.RiskCoveragePremium.PhysicalDamagePremium += trailerInterchangePremium;
                riskCov.RiskCoveragePremium.Premium += trailerInterchangePremium;
                _riskCoverageRepository.Update(riskCov);
            }

            await _riskCoverageRepository.SaveChangesAsync();

            // recalculate taxes and payment option
            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                await _billingService.UpdatePaymentOptions(riskCov.RiskDetailId, riskCov.OptionNumber);
                await _taxService.CalculateRiskTaxAsync(riskCov.RiskDetailId, riskCov.OptionNumber);
            }
        }

        public async Task RemoveHiredPhysicalDamage(Guid riskDetailId)
        {
            var riskCovs = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                var hiredPhysicalDamage = riskCov.RiskCoveragePremium.HiredPhysicalDamage ?? 0;
                riskCov.RiskCoveragePremium.PhysicalDamagePremium -= hiredPhysicalDamage;
                riskCov.RiskCoveragePremium.Premium -= hiredPhysicalDamage;
                _riskCoverageRepository.Update(riskCov);
            }

            await _riskCoverageRepository.SaveChangesAsync();

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                await _billingService.UpdatePaymentOptions(riskCov.RiskDetailId, riskCov.OptionNumber);
                await _taxService.CalculateRiskTaxAsync(riskCov.RiskDetailId, riskCov.OptionNumber);
            }
        }

        public async Task RemoveTrailerInterchange(Guid riskDetailId)
        {
            var riskCovs = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                var trailerInterchange = riskCov.RiskCoveragePremium.TrailerInterchange ?? 0;
                riskCov.RiskCoveragePremium.PhysicalDamagePremium -= trailerInterchange;
                riskCov.RiskCoveragePremium.Premium -= trailerInterchange;
                _riskCoverageRepository.Update(riskCov);
            }

            await _riskCoverageRepository.SaveChangesAsync();

            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                await _billingService.UpdatePaymentOptions(riskCov.RiskDetailId, riskCov.OptionNumber);
                await _taxService.CalculateRiskTaxAsync(riskCov.RiskDetailId, riskCov.OptionNumber);
            }
        }

        public async Task CalculateAdditionalInterests(Guid riskDetailId)
        {
            // initialize
            var additionalInterests = await _riskAdditionalInterestRepository.GetAsync(ai => ai.RiskDetailId == riskDetailId && ai.RemoveProcessDate == null, ai => ai.Entity);
            var risk = await _riskDetailRepository.FindAsync(r => r.Id == riskDetailId);
            var riskCovs = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            // get count per additional interest type
            int additionalInsuredCount = additionalInterests.Count(x => x.AdditionalInterestTypeId == AdditionalInterestType.AdditionalInsured.Id);
            int primaryAndNonContributoryCount = additionalInterests.Count(x => x.IsPrimaryNonContributory);
            int waiverOfSubrogationCount = additionalInterests.Count(x => x.IsWaiverOfSubrogation);
            // compute premium per additional premium type
            decimal additionInsuredPremium = risk.IsAdditionalInsured.GetValueOrDefault() ? AdditionalInterestRates.BLANKET_ADDITIONAL_INSURED_PREMIUM : (additionalInsuredCount * AdditionalInterestRates.ADDITIONAL_INSURED_PREMIUM);
            decimal primaryAndNonContributoryPremium = risk.IsPrimaryNonContributory.GetValueOrDefault() ? AdditionalInterestRates.BLANKET_PRIMARY_AND_NONCONTRIBUTORY_PREMIUM : (primaryAndNonContributoryCount * AdditionalInterestRates.PRIMARY_AND_NONCONTRIBUTORY_PREMIUM);
            decimal waiverOfSubrogationPremium = risk.IsWaiverOfSubrogation.GetValueOrDefault() ? AdditionalInterestRates.BLANKET_WAIVER_OF_SUBROGATION_PREMIUM : (waiverOfSubrogationCount * AdditionalInterestRates.WAIVER_OF_SUBROGATION_PREMIUM);
            // loop through each risk coverage
            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null && (riskCovs.RiskCoveragePremium.Premium ?? 0) != 0))
            {
                var riskCoveragePremium = riskCov.RiskCoveragePremium; // temp hold riskCoveragePremium
                var currentAdditionalPremiums = riskCoveragePremium?.AdditionalInsuredPremium + riskCoveragePremium?.PrimaryNonContributoryPremium + riskCoveragePremium?.WaiverOfSubrogationPremium; // sum up current values
                var newAdditionalPremiums = additionInsuredPremium + primaryAndNonContributoryPremium + waiverOfSubrogationPremium; // sum up new values
                riskCov.RiskCoveragePremium.Premium -= currentAdditionalPremiums; // remove current additional interest amount from the premium
                riskCov.RiskCoveragePremium.Premium += newAdditionalPremiums; // add new calculations
                riskCov.RiskCoveragePremium.AdditionalInsuredPremium = additionInsuredPremium; // update field
                riskCov.RiskCoveragePremium.PrimaryNonContributoryPremium = primaryAndNonContributoryPremium; // update field
                riskCov.RiskCoveragePremium.WaiverOfSubrogationPremium = waiverOfSubrogationPremium; // update field

                _riskCoverageRepository.Update(riskCov);
            }

            await _riskCoverageRepository.SaveChangesAsync(); // save changes

            // recalculate tax and payment options
            foreach (var riskCov in riskCovs.Where(riskCovs => riskCovs.RiskCoveragePremium != null))
            {
                await _billingService.UpdatePaymentOptions(riskCov.RiskDetailId, riskCov.OptionNumber);
                await _taxService.CalculateRiskTaxAsync(riskCov.RiskDetailId, riskCov.OptionNumber);
            }
        }
    }
}
