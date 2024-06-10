using AutoMapper;
using log4net.Core;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class TaxService : ITaxService
    {
        private readonly ILogger<TaxService> _logger;
        private readonly IMapper _mapper;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskPremiumFeesFactory _riskPremiumFeesRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IStateTaxRepository _stateTaxRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly ITaxDetailsRepository _taxDetailsReposiotry;
        private readonly IBillingService _billingService;
        private readonly IRiskRepository _riskRepository;
        private readonly IInsuredRepository _insuredRepository;

        public TaxService(
            ILogger<TaxService> logger,
            IMapper mapper,
            IRiskDetailRepository riskDetailRepository,
            IRiskPremiumFeesFactory riskPremiumFeesFactory,
            IEntityAddressRepository entityAddressRepository,
            IStateTaxRepository stateTaxRepository,
            IBrokerInfoRepository brokerInfoRepository,
            ITaxDetailsRepository taxDetailsReposiotry,
            IBillingService billingService,
            IRiskRepository riskRepository
        )
        {
            _logger = logger;
            _mapper = mapper;
            _riskDetailRepository = riskDetailRepository;   
            _riskPremiumFeesRepository = riskPremiumFeesFactory;
            _entityAddressRepository = entityAddressRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _stateTaxRepository = stateTaxRepository;
            _taxDetailsReposiotry = taxDetailsReposiotry;
            _billingService = billingService;
            _riskRepository = riskRepository;
        }


        public async Task CalculateRiskTaxAsync(Guid riskDetailId, int? optionId = null, bool isEndorsement = false)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var currentRiskDetail = risk.CurrentRiskDetailId.HasValue 
                ? await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value)
                : await Task.FromResult((RiskDetail)null);
            var currentRiskCovPrem = currentRiskDetail?.RiskCoverages?.FirstOrDefault().RiskCoveragePremium;

            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            if (!brokerInfo.EffectiveDate.HasValue)
                throw new InvalidOperationException("Cannot calculate tax without effective date.");

            var businessAddress = await _entityAddressRepository
                .FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business
                                && x.EntityId == riskDetail.Insured.EntityId, i => i.Address);
            StateTax stateTax = await _stateTaxRepository.GetAsync(businessAddress.Address.State, brokerInfo.EffectiveDate.Value);
            if (isEndorsement && stateTax.StampingFeeTax != null) stateTax.StampingFeeTax.IsExcluded = true;

            foreach (var riskCov in riskDetail.RiskCoverages.Where(riskcoverages => riskcoverages.RiskCoveragePremium != null))
            {
                if (optionId.HasValue && riskCov.OptionNumber != optionId) continue;
                try
                {
                    List<TransactionDetailDTO> transactionDetails = new List<TransactionDetailDTO>();

                    if (riskCov.RiskCoveragePremium.AutoLiabilityPremium.HasValue && riskCov.RiskCoveragePremium.AutoLiabilityPremium.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.AutoLiabilityPremium.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleALP } });
                    }
                    if (riskCov.RiskCoveragePremium.PhysicalDamagePremium.HasValue && riskCov.RiskCoveragePremium.PhysicalDamagePremium.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.PhysicalDamagePremium.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehiclePDP } });
                    }
                    if (riskCov.RiskCoveragePremium.CargoPremium.HasValue && riskCov.RiskCoveragePremium.CargoPremium.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.CargoPremium.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleCP } });
                    }
                    if (riskCov.RiskCoveragePremium.GeneralLiabilityPremium.HasValue && riskCov.RiskCoveragePremium.GeneralLiabilityPremium.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.GeneralLiabilityPremium.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GeneralLiabilityPremium } });
                    }
                    if (riskCov.RiskCoveragePremium.TrailerInterchangeAL.HasValue && riskCov.RiskCoveragePremium.TrailerInterchangeAL.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.TrailerInterchangeAL.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangePremium } });
                    }
                    if (riskCov.RiskCoveragePremium.NonOwnedTrailerPremium.HasValue && riskCov.RiskCoveragePremium.NonOwnedTrailerPremium.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.NonOwnedTrailerPremium.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.NonOwnedTrailerPremium } });
                    }
                    if (riskCov.RiskCoveragePremium.RiskMgrFeeAL.HasValue && riskCov.RiskCoveragePremium.RiskMgrFeeAL.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.RiskMgrFeeAL.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeeAL } });
                    }
                    if (riskCov.RiskCoveragePremium.RiskMgrFeePD.HasValue && riskCov.RiskCoveragePremium.RiskMgrFeePD.Value > 0)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.RiskMgrFeePD.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeePD } });
                    }
                    if (riskCov.RiskCoveragePremium.InstallmentFee.HasValue && riskCov.RiskCoveragePremium.InstallmentFee.Value > 0 && !isEndorsement)
                    {
                        transactionDetails.Add(new TransactionDetailDTO { Amount = riskCov.RiskCoveragePremium.InstallmentFee.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.InstallmentFee } });
                    }

                    //Compute risk coverage tax from billing
                    TaxDetailsDTO taxDetails = (await _billingService.ProcessComputeRiskTax(riskDetail.RiskId, brokerInfo.EffectiveDate.Value, stateTax.StateCode, transactionDetails, riskCov.NumberOfInstallments)).TaxDetails;

                    // Save tax details.
                    var taxDetailsData = _mapper.Map<TaxDetailsData>(taxDetails);
                    if(riskCov.RiskCoveragePremium.TaxDetailsId.HasValue)
                    {
                        var existingTaxDetails = await _taxDetailsReposiotry.FindAsync(riskCov.RiskCoveragePremium.TaxDetailsId.Value);
                        if(existingTaxDetails != null && (currentRiskDetail is null || (existingTaxDetails.Id != currentRiskCovPrem.TaxDetailsId)))
                            _taxDetailsReposiotry.Remove(existingTaxDetails);
                    }
                    taxDetailsData  = await _taxDetailsReposiotry.AddAsync(taxDetailsData);

                    // Assign tax details to risk coverage.
                    riskCov.RiskCoveragePremium.TaxDetailsId = taxDetailsData.Id;
                    riskCov.RiskCoveragePremium.TaxesAndStampingFees = taxDetails.TotalTax;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    _logger.LogError(e, e.Message);
                }
            }

            await _taxDetailsReposiotry.SaveChangesAsync();
            _riskDetailRepository.Update(riskDetail);
            await _riskDetailRepository.SaveChangesAsync();
        }

        public async Task<(TaxDetailsDTO, TaxDetailsDTO)> CalculateTaxPremiumChangesAsync(Guid riskDetailId, DateTime effectiveDate, decimal installment, List<EndorsementPendingChange> pendingChanges)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            //var insured = await _insuredRepository.FindAsync(riskDetail.InsuredId.Value);

            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            if (!brokerInfo.EffectiveDate.HasValue)
                throw new InvalidOperationException("Cannot calculate tax without effective date.");

            var businessAddress = await _entityAddressRepository
                .FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business
                                && x.EntityId == riskDetail.Insured.EntityId, i => i.Address);
            StateTax stateTax = await _stateTaxRepository.GetAsync(businessAddress.Address.State, brokerInfo.EffectiveDate.Value);

            try
            {
                List<TransactionDetailDTO> transactionDetailsBefore = new List<TransactionDetailDTO>();
                List<TransactionDetailDTO> transactionDetailsAfter = new List<TransactionDetailDTO>();

                var alPremiumBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP ||
                                                                x.AmountSubTypeId == BillingConstants.ALManuscriptPremium ||
                                                                x.AmountSubTypeId == BillingConstants.AdditionalInsured || 
                                                                x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation ||
                                                                x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory).Sum(x => x.PremiumBeforeEndorsement);
                if (alPremiumBefore.HasValue && alPremiumBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = alPremiumBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleALP } });
                }
                var alPremiumAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP ||
                                                                x.AmountSubTypeId == BillingConstants.ALManuscriptPremium ||
                                                                x.AmountSubTypeId == BillingConstants.AdditionalInsured ||
                                                                x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation ||
                                                                x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory).Sum(x => x.PremiumAfterEndorsement);
                if (alPremiumAfter.HasValue && alPremiumAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = alPremiumAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleALP } });
                }

                var pdPremiumBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehiclePDP ||
                                                                x.AmountSubTypeId == BillingConstants.PDManuscriptPremium ||
                                                                x.AmountSubTypeId == BillingConstants.TrailerInterchangeEndst ||
                                                                x.AmountSubTypeId == BillingConstants.HiredPhysicalDamageEndst).Sum(x => x.PremiumBeforeEndorsement);
                if (pdPremiumBefore.HasValue && pdPremiumBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = pdPremiumBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehiclePDP } });
                }
                var pdPremiumAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehiclePDP ||
                                                                x.AmountSubTypeId == BillingConstants.PDManuscriptPremium ||
                                                                x.AmountSubTypeId == BillingConstants.TrailerInterchangeEndst ||
                                                                x.AmountSubTypeId == BillingConstants.HiredPhysicalDamageEndst).Sum(x => x.PremiumAfterEndorsement);
                if (pdPremiumAfter.HasValue && pdPremiumAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = pdPremiumAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehiclePDP } });
                }

                var cpPremiumBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleCP).Sum(x => x.PremiumBeforeEndorsement);
                if (cpPremiumBefore.HasValue && cpPremiumBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = cpPremiumBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleCP } });
                }
                var cpPremiumAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleCP).Sum(x => x.PremiumAfterEndorsement);
                if (cpPremiumAfter.HasValue && cpPremiumAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = cpPremiumAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleCP } });
                }

                var glPremiumBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.GeneralLiabilityPremium).Sum(x => x.PremiumBeforeEndorsement);
                if (glPremiumBefore.HasValue && glPremiumBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = glPremiumBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GeneralLiabilityPremium } });
                }
                var glPremiumAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.GeneralLiabilityPremium).Sum(x => x.PremiumAfterEndorsement);
                if (glPremiumAfter.HasValue && glPremiumAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = glPremiumAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GeneralLiabilityPremium } });
                }

                var TrailerInterchangePremBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.TrailerInterchangePremium).Sum(x => x.PremiumBeforeEndorsement);
                if (TrailerInterchangePremBefore.HasValue && TrailerInterchangePremBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = TrailerInterchangePremBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangePremium } });
                }
                var TrailerInterchangePremAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.TrailerInterchangePremium).Sum(x => x.PremiumAfterEndorsement);
                if (TrailerInterchangePremAfter.HasValue && TrailerInterchangePremAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = TrailerInterchangePremAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangePremium } });
                }

                var NonOwnedTrailerPremBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.NonOwnedTrailerPremium).Sum(x => x.PremiumBeforeEndorsement);
                if (NonOwnedTrailerPremBefore.HasValue && NonOwnedTrailerPremBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = NonOwnedTrailerPremBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.NonOwnedTrailerPremium } });
                }
                var NonOwnedTrailerPremAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.NonOwnedTrailerPremium).Sum(x => x.PremiumAfterEndorsement);
                if (NonOwnedTrailerPremAfter.HasValue && NonOwnedTrailerPremAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = NonOwnedTrailerPremAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.NonOwnedTrailerPremium } });
                }

                var sfalPremiumBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeeAL).Sum(x => x.PremiumBeforeEndorsement);
                if (sfalPremiumBefore.HasValue && sfalPremiumBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = sfalPremiumBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeeAL } });
                }
                var sfalPremiumAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeeAL).Sum(x => x.PremiumAfterEndorsement);
                if (sfalPremiumAfter.HasValue && sfalPremiumAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = sfalPremiumAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeeAL } });
                }

                var sfpdPremiumBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeePD).Sum(x => x.PremiumBeforeEndorsement);
                if (sfpdPremiumBefore.HasValue && sfpdPremiumBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = sfpdPremiumBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeePD } });
                }
                var sfpdPremiumAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeePD).Sum(x => x.PremiumAfterEndorsement);
                if (sfpdPremiumAfter.HasValue && sfpdPremiumAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = sfpdPremiumAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeePD } });
                }

                var installmentFeeBefore = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.InstallmentFee).Sum(x => x.PremiumBeforeEndorsement);
                if (installmentFeeBefore.HasValue && installmentFeeBefore.Value > 0)
                {
                    transactionDetailsBefore.Add(new TransactionDetailDTO { Amount = installmentFeeBefore.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.InstallmentFee } });
                }
                var installmentFeeAfter = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.InstallmentFee).Sum(x => x.PremiumAfterEndorsement);
                if (installmentFeeAfter.HasValue && installmentFeeAfter.Value > 0)
                {
                    transactionDetailsAfter.Add(new TransactionDetailDTO { Amount = installmentFeeAfter.Value, AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.InstallmentFee } });
                }

                //Compute risk coverage tax from billing
                TaxDetailsDTO taxDetailsBefore = (await _billingService.ProcessComputeRiskTax(riskDetail.RiskId, brokerInfo.EffectiveDate.Value, stateTax.StateCode, transactionDetailsBefore, installment)).TaxDetails;
                TaxDetailsDTO taxDetailsAfter = (await _billingService.ProcessComputeRiskTax(riskDetail.RiskId, brokerInfo.EffectiveDate.Value, stateTax.StateCode, transactionDetailsAfter, installment)).TaxDetails;

                return (taxDetailsBefore, taxDetailsAfter);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return (null, null);
            }
        }

        public async Task<IEnumerable<TaxDetailsData>> GetTaxDetails(List<Guid> ids)
        {
            return await _taxDetailsReposiotry.GetAsync(td => ids.Contains(td.Id));
        }

        public async Task<TaxDetailsData> GetTaxDetail(Guid id)
        {
            return await _taxDetailsReposiotry.FindAsync(id);
        }

        public async Task ResetTaxDetailsAsync(Guid forEndoRiskDetailId)
        {
            var forEndoRiskDetail = await _riskDetailRepository.FindAsync(forEndoRiskDetailId);
            var risk = await _riskRepository.FindAsync(forEndoRiskDetail.RiskId);
            var lastIssuedRiskDetail = await _riskDetailRepository.FindAsync(risk.CurrentRiskDetailId.Value);

            var forEndoRiskCov = forEndoRiskDetail.RiskCoverages.First();
            forEndoRiskCov.RiskCoveragePremium.TaxDetailsId = lastIssuedRiskDetail.RiskCoverages.First().RiskCoveragePremium.TaxDetailsId;
            _riskDetailRepository.Update(forEndoRiskDetail);
            await _riskDetailRepository.SaveChangesAsync();
        }
    }
}
