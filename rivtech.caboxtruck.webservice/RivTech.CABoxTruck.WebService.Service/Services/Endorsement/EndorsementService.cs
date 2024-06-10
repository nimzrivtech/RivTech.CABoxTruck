using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.PremiumChanges;
using RivTech.CABoxTruck.WebService.Service.Services.Fees;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EndorsementService : IEndorsementService
    {
        private readonly ILogger<EndorsementService> _logger;
        private readonly AppDbContext _context;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IBindingRepository _bindingRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IEndorsementPendingChangeRepository _endorsementPendingChangeRepository;
        private readonly IBindingService _bindingService;
        private readonly ITaxService _taxService;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IVehiclePremiumRepository _vehiclePremiumRepository;
        private readonly IRiskSequenceService _riskSequence;
        private readonly IMapper _mapper;
        private readonly ILog4NetService _errorLogservice;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IChangeComparerContext _changeComparerContext;
        private readonly IEndorsementProcessor _endorsementProcessor;
        private readonly IEndorsementDataRepository _endorsementDataRepository;
        private readonly IEndorsementChangeDeserializer _endorsementChangeDeserializer;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskVehiclePremiumTransactionRepository _riskVehiclePremiumTransactionRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IBillingService _billingService;
        private readonly IRiskService _riskService;
        private readonly IEndorsementProcessStatusLogService _statusLogService;
        private readonly IRiskVehiclePremiumService _riskVehiclePremiumService;
        private readonly IRiskPremiumTransactionService _riskPremiumTransactionService;
        private readonly IPremiumService _premiumService;
        private readonly INameAndAddressService _nameAndAddressService;
        private readonly IFeeService _feeService;
        private readonly IRaterService _raterService;
        private readonly INotificationService _notificationService;

        public EndorsementService(
            ILogger<EndorsementService> logger,
            AppDbContext context,
            IRiskDetailRepository riskDetailRepository,
            IBindingRepository bindingRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository,
            IRiskFormRepository riskFormRepository,
            IEndorsementPendingChangeRepository endorsementPendingChangeRepository,
            IBindingService bindingService,
            ITaxService taxService,
            IVehicleRepository vehicleRepository,
            IRiskRepository riskRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IPolicyHistoryRepository policyHistoryRepository,
            IRiskCoverageRepository riskCoverageRepository,
            IVehiclePremiumRepository vehiclePremiumRepository,
            IRiskSequenceService riskSequence,
            IMapper mapper,
            ILog4NetService errorLogservice,
            IEndorsementChangesService endorsementChangesService,
            IChangeComparerContext changeComparerContext,
            IEndorsementProcessor endorsementProcessor,
            IEndorsementDataRepository endorsementDataRepository,
            IEndorsementChangeDeserializer endorsementChangeDeserializer,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskVehiclePremiumTransactionRepository riskVehiclePremiumTransactionRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository,
            IVehicleService vehicleService,
            IBillingService billingService,
            IRiskService riskService,
            IEndorsementProcessStatusLogService statusLogService,
            IRiskVehiclePremiumService riskVehiclePremiumService,
            IRiskPremiumTransactionService riskPremiumTransactionService,
            IPremiumService premiumService,
            INameAndAddressService nameAndAddressService,
            IFeeService feeService,
            IRaterService raterService,
            INotificationService notificationService
        )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _errorLogservice = errorLogservice;
            _riskDetailRepository = riskDetailRepository;
            _bindingRepository = bindingRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _riskFormRepository = riskFormRepository;
            _vehicleRepository = vehicleRepository;
            _endorsementPendingChangeRepository = endorsementPendingChangeRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _policyHistoryRepository = policyHistoryRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _vehiclePremiumRepository = vehiclePremiumRepository;
            _riskSequence = riskSequence;
            _bindingService = bindingService;
            _taxService = taxService;
            _riskRepository = riskRepository;
            _endorsementChangesService = endorsementChangesService;
            _changeComparerContext = changeComparerContext;
            _endorsementProcessor = endorsementProcessor;
            _endorsementDataRepository = endorsementDataRepository;
            _endorsementChangeDeserializer = endorsementChangeDeserializer;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskVehiclePremiumTransactionRepository = riskVehiclePremiumTransactionRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
            _vehicleService = vehicleService;
            _billingService = billingService;
            _riskService = riskService;
            _statusLogService = statusLogService;
            _riskVehiclePremiumService = riskVehiclePremiumService;
            _riskPremiumTransactionService = riskPremiumTransactionService;
            _premiumService = premiumService;
            _nameAndAddressService = nameAndAddressService;
            _feeService = feeService;
            _raterService = raterService;
            _notificationService = notificationService;
        }

        public async Task SetServiceFeeChanges(RiskDetail currentRiskDetail)
        {
            var currentRiskDetailId = currentRiskDetail.Id;
            var risk = await _riskRepository.GetRiskByIdAsync(currentRiskDetail.RiskId);
            //var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            //var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value);
            var bindData = await _bindingService.GetBindingByRiskIdAsync(currentRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

            var previousRiskCoverage = previousRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            var currentRiskCoverage = currentRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            var previousSFAL = previousRiskCoverage?.RiskCoveragePremium?.RiskMgrFeeAL ?? 0;
            var previousSFPD = previousRiskCoverage?.RiskCoveragePremium?.RiskMgrFeePD ?? 0;
            var currentSFAL = currentRiskCoverage?.RiskCoveragePremium?.RiskMgrFeeAL ?? 0;
            var currentSFPD = currentRiskCoverage?.RiskCoveragePremium?.RiskMgrFeePD ?? 0;

            //currentSFAL = (currentSFAL < previousSFAL) ? previousSFAL : currentSFAL; // Service Fee should only log for Added Vehicles only
            //currentSFPD = (currentSFPD < previousSFPD) ? previousSFPD : currentSFPD; // Service Fee should only log for Added Vehicles only

            var pendingChangesAL = await _endorsementPendingChangeRepository.FindAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId &&
                    x.AmountSubTypeId == BillingConstants.ServiceFeeAL);
            if (pendingChangesAL != null)
            {
                pendingChangesAL.PremiumBeforeEndorsement = previousSFAL;
                pendingChangesAL.PremiumAfterEndorsement = currentSFAL;
                pendingChangesAL.PremiumDifference = FindDifference(currentSFAL, previousSFAL);
                pendingChangesAL.DateCreated = DateTime.Now;
                pendingChangesAL.Description = "Service Fee - AL";
                _endorsementPendingChangeRepository.Update(pendingChangesAL);
            }
            else
            {
                var newPendingChangesAL = new EndorsementPendingChange();
                newPendingChangesAL.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesAL.AmountSubTypeId = BillingConstants.ServiceFeeAL;
                newPendingChangesAL.PremiumBeforeEndorsement = previousSFAL;
                newPendingChangesAL.PremiumAfterEndorsement = currentSFAL;
                newPendingChangesAL.PremiumDifference = FindDifference(currentSFAL, previousSFAL);
                newPendingChangesAL.DateCreated = DateTime.Now;
                newPendingChangesAL.Description = "Service Fee - AL";
                newPendingChangesAL.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesAL);
            }

            var pendingChangesPD = await _endorsementPendingChangeRepository.FindAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId &&
                    x.AmountSubTypeId == BillingConstants.ServiceFeePD);
            if (pendingChangesPD != null)
            {
                pendingChangesPD.PremiumBeforeEndorsement = previousSFPD;
                pendingChangesPD.PremiumAfterEndorsement = currentSFPD;
                pendingChangesPD.PremiumDifference = FindDifference(currentSFPD, previousSFPD);
                pendingChangesPD.DateCreated = DateTime.Now;
                pendingChangesAL.Description = "Service Fee - PD";
                _endorsementPendingChangeRepository.Update(pendingChangesPD);
            }
            else
            {
                var newPendingChangesPD = new EndorsementPendingChange();
                newPendingChangesPD.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesPD.AmountSubTypeId = BillingConstants.ServiceFeePD;
                newPendingChangesPD.PremiumBeforeEndorsement = previousSFPD;
                newPendingChangesPD.PremiumAfterEndorsement = currentSFPD;
                newPendingChangesPD.PremiumDifference = FindDifference(currentSFPD, previousSFPD);
                newPendingChangesPD.DateCreated = DateTime.Now;
                newPendingChangesPD.Description = "Service Fee - PD";
                newPendingChangesPD.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesPD);
            }

            await _endorsementPendingChangeRepository.SaveChangesAsync();
        }

        public async Task SetTaxesChanges(RiskDetail currentRiskDetail, DateTime effectiveDate)
        {
            var currentRiskDetailId = currentRiskDetail.Id;
            var risk = await _riskRepository.GetRiskByIdAsync(currentRiskDetail.RiskId);
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value);
            var bindData = await _bindingService.GetBindingByRiskIdAsync(currentRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

            var previousRiskCoverage = previousRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            var currentRiskCoverage = currentRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();

            var pendingChanges = await _endorsementPendingChangeRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId.Value);

            var (taxBefore, taxAfter) = await _taxService.CalculateTaxPremiumChangesAsync(currentRiskDetail.Id, effectiveDate, currentRiskCoverage.NumberOfInstallments ?? 0, pendingChanges.ToList());

            var pendingChangesTaxSL = await _endorsementPendingChangeRepository.FindAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId.Value &&
                    x.AmountSubTypeId == BillingConstants.SurplusLinesTax);
            if (pendingChangesTaxSL != null)
            {
                pendingChangesTaxSL.PremiumBeforeEndorsement = taxBefore.TotalSLTax;
                pendingChangesTaxSL.PremiumAfterEndorsement = taxAfter.TotalSLTax;
                pendingChangesTaxSL.PremiumDifference = FindDifference(taxAfter.TotalSLTax, taxBefore.TotalSLTax);
                pendingChangesTaxSL.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesTaxSL);
            }
            else
            {
                var newPendingChangesTaxSL = new EndorsementPendingChange();
                newPendingChangesTaxSL.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesTaxSL.AmountSubTypeId = BillingConstants.SurplusLinesTax;
                newPendingChangesTaxSL.PremiumBeforeEndorsement = taxBefore.TotalSLTax;
                newPendingChangesTaxSL.PremiumAfterEndorsement = taxAfter.TotalSLTax;
                newPendingChangesTaxSL.PremiumDifference = FindDifference(taxAfter.TotalSLTax, taxBefore.TotalSLTax);
                newPendingChangesTaxSL.DateCreated = DateTime.Now;
                newPendingChangesTaxSL.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesTaxSL);
            }

            var pendingChangesTaxSF = await _endorsementPendingChangeRepository.FindAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId.Value &&
                    x.AmountSubTypeId == BillingConstants.StampingFee);
            if (pendingChangesTaxSF != null)
            {
                pendingChangesTaxSF.PremiumBeforeEndorsement = taxBefore.TotalStampingFee;
                pendingChangesTaxSF.PremiumAfterEndorsement = taxAfter.TotalStampingFee;
                pendingChangesTaxSF.PremiumDifference = FindDifference(taxAfter.TotalStampingFee, taxBefore.TotalStampingFee);
                pendingChangesTaxSF.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesTaxSF);
            }
            else
            {
                var newPendingChangesTaxSF = new EndorsementPendingChange();
                newPendingChangesTaxSF.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesTaxSF.AmountSubTypeId = BillingConstants.StampingFee;
                newPendingChangesTaxSF.PremiumBeforeEndorsement = taxBefore.TotalStampingFee;
                newPendingChangesTaxSF.PremiumAfterEndorsement = taxAfter.TotalStampingFee;
                newPendingChangesTaxSF.PremiumDifference = FindDifference(taxAfter.TotalStampingFee, taxBefore.TotalStampingFee);
                newPendingChangesTaxSF.DateCreated = DateTime.Now;
                newPendingChangesTaxSF.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesTaxSF);
            }

            await _endorsementPendingChangeRepository.SaveChangesAsync();
        }

        public async Task SetAdditionalPremiumChanges(Guid forEndorsementRiskDetailId, Guid previousRiskDetailId)
        {
            //var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == currentRiskDetail.RiskId && riskDetail.Id != currentRiskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            //var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(forEndorsementRiskDetailId);
            var risk = await _riskRepository.GetRiskByIdAsync(currentRiskDetail.RiskId);
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
            var bindData = await _bindingService.GetBindingByRiskIdAsync(currentRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

            var previousRiskCoverage = previousRiskDetail?.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            var currentRiskCoverage = currentRiskDetail?.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            var prevAddtl = previousRiskCoverage?.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0;
            var prevWaiver = previousRiskCoverage?.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0;
            var prevPnc = previousRiskCoverage?.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0;
            var prevTrailer = previousRiskCoverage?.RiskCoveragePremium?.TrailerInterchange ?? 0;
            var prevHired = previousRiskCoverage?.RiskCoveragePremium?.HiredPhysicalDamage ?? 0;

            var curAddtl = currentRiskCoverage?.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0;
            var curWaiver = currentRiskCoverage?.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0;
            var curPnc = currentRiskCoverage?.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0;
            var curTrailer = currentRiskCoverage?.RiskCoveragePremium?.TrailerInterchange ?? 0;
            var curHired = currentRiskCoverage?.RiskCoveragePremium?.HiredPhysicalDamage ?? 0;

            IEnumerable<EndorsementPendingChange> endorsementPendingChanges = await _endorsementPendingChangeRepository
                .GetByRiskDetailAsync(risk.ForEndorsementRiskDetailId.Value);

            // Additional Insured
            var pendingChangesAddtl = endorsementPendingChanges.FirstOrDefault(x => x.AmountSubTypeId == BillingConstants.AdditionalInsured);
            if (pendingChangesAddtl != null)
            {
                pendingChangesAddtl.PremiumBeforeEndorsement = prevAddtl;
                pendingChangesAddtl.PremiumAfterEndorsement = curAddtl;
                pendingChangesAddtl.PremiumDifference = FindDifference(curAddtl, prevAddtl);
                pendingChangesAddtl.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesAddtl);
            }
            else
            {
                var newPendingChangesAddtl = new EndorsementPendingChange();
                newPendingChangesAddtl.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesAddtl.AmountSubTypeId = BillingConstants.AdditionalInsured;
                newPendingChangesAddtl.PremiumBeforeEndorsement = prevAddtl;
                newPendingChangesAddtl.PremiumAfterEndorsement = curAddtl;
                newPendingChangesAddtl.PremiumDifference = FindDifference(curAddtl, prevAddtl);
                newPendingChangesAddtl.DateCreated = DateTime.Now;
                newPendingChangesAddtl.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesAddtl);
            }

            // Waiver of Subrogation
            var pendingChangesWaiver = endorsementPendingChanges.FirstOrDefault(x => x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation);
            if (pendingChangesWaiver != null)
            {
                pendingChangesWaiver.PremiumBeforeEndorsement = prevWaiver;
                pendingChangesWaiver.PremiumAfterEndorsement = curWaiver;
                pendingChangesWaiver.PremiumDifference = FindDifference(curWaiver, prevWaiver);
                pendingChangesWaiver.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesWaiver);
            }
            else
            {
                var newPendingChangesWaiver = new EndorsementPendingChange();
                newPendingChangesWaiver.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesWaiver.AmountSubTypeId = BillingConstants.WaiverOfSubrogation;
                newPendingChangesWaiver.PremiumBeforeEndorsement = prevWaiver;
                newPendingChangesWaiver.PremiumAfterEndorsement = curWaiver;
                newPendingChangesWaiver.PremiumDifference = FindDifference(curWaiver, prevWaiver);
                newPendingChangesWaiver.DateCreated = DateTime.Now;
                newPendingChangesWaiver.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesWaiver);
            }

            // Primary Non Contributory
            var pendingChangesPnc = endorsementPendingChanges.FirstOrDefault(x => x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory);
            if (pendingChangesPnc != null)
            {
                pendingChangesPnc.PremiumBeforeEndorsement = prevPnc;
                pendingChangesPnc.PremiumAfterEndorsement = curPnc;
                pendingChangesPnc.PremiumDifference = FindDifference(curPnc, prevPnc);
                pendingChangesPnc.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesPnc);
            }
            else
            {
                var newPendingChangesPnc = new EndorsementPendingChange();
                newPendingChangesPnc.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesPnc.AmountSubTypeId = BillingConstants.PrimaryAndNoncontributory;
                newPendingChangesPnc.PremiumBeforeEndorsement = prevPnc;
                newPendingChangesPnc.PremiumAfterEndorsement = curPnc;
                newPendingChangesPnc.PremiumDifference = FindDifference(curPnc, prevPnc);
                newPendingChangesPnc.DateCreated = DateTime.Now;
                newPendingChangesPnc.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesPnc);
            }

            // Trailer Interchange
            var pendingChangesTrailer = endorsementPendingChanges.FirstOrDefault(x => x.AmountSubTypeId == BillingConstants.TrailerInterchangeEndst);
            if (pendingChangesTrailer != null)
            {
                pendingChangesTrailer.PremiumBeforeEndorsement = prevTrailer;
                pendingChangesTrailer.PremiumAfterEndorsement = curTrailer;
                pendingChangesTrailer.PremiumDifference = FindDifference(curTrailer, prevTrailer);
                pendingChangesTrailer.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesTrailer);
            }
            else
            {
                var newPendingChangesTrailer = new EndorsementPendingChange();
                newPendingChangesTrailer.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesTrailer.AmountSubTypeId = BillingConstants.TrailerInterchangeEndst;
                newPendingChangesTrailer.PremiumBeforeEndorsement = prevTrailer;
                newPendingChangesTrailer.PremiumAfterEndorsement = curTrailer;
                newPendingChangesTrailer.PremiumDifference = FindDifference(curTrailer, prevTrailer);
                newPendingChangesTrailer.DateCreated = DateTime.Now;
                newPendingChangesTrailer.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesTrailer);
            }

            // Hired Physical
            var pendingChangesHired = endorsementPendingChanges.FirstOrDefault(x => x.AmountSubTypeId == BillingConstants.HiredPhysicalDamageEndst);
            if (pendingChangesHired != null)
            {
                pendingChangesHired.PremiumBeforeEndorsement = prevHired;
                pendingChangesHired.PremiumAfterEndorsement = curHired;
                pendingChangesHired.PremiumDifference = FindDifference(curHired, prevHired);
                pendingChangesHired.DateCreated = DateTime.Now;
                _endorsementPendingChangeRepository.Update(pendingChangesHired);
            }
            else
            {
                var newPendingChangesHired = new EndorsementPendingChange();
                newPendingChangesHired.RiskDetailId = risk.ForEndorsementRiskDetailId.Value;
                newPendingChangesHired.AmountSubTypeId = BillingConstants.HiredPhysicalDamageEndst;
                newPendingChangesHired.PremiumBeforeEndorsement = prevHired;
                newPendingChangesHired.PremiumAfterEndorsement = curHired;
                newPendingChangesHired.PremiumDifference = FindDifference(curHired, prevHired);
                newPendingChangesHired.DateCreated = DateTime.Now;
                newPendingChangesHired.CreatedBy = Data.Common.Services.GetCurrentUser().ToString();
                await _endorsementPendingChangeRepository.AddAsync(newPendingChangesHired);
            }

            await _endorsementPendingChangeRepository.SaveChangesAsync();
        }

        public async Task UpdateEffectiveDate(Guid riskDetailId, DateTime effectiveDate)
        {
            Console.WriteLine($"[{DateTime.Now:s}] Updating effective dates of vehicle to {effectiveDate:MM/dd/yyyy}");
            var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);

            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == risk.Id);
            if (effectiveDate >= brokerInfo.ExpirationDate)
                throw new InvalidOperationException("Endorsement effective date must be earlier than policy expiration date.");
            riskDetail.EndorsementEffectiveDate = effectiveDate;

            //Console.WriteLine($"[{DateTime.Now:s}] Retrieving vehicles...");
            //var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, true);
            //Console.WriteLine($"[{DateTime.Now:s}] Retrieving previous vehicles...");
            //var previousVehicles = await _vehicleRepository.GetByRiskDetailAsync(risk.CurrentRiskDetailId.Value, true);
            //foreach (var vehicle in vehicles)
            //{
            //    bool isNewlyAdded = !previousVehicles.Any(x => x.Id == vehicle.Id) && vehicle.PreviousVehicleVersionId is null;
            //    bool isPreviouslyDeleted = previousVehicles.FirstOrDefault(x => x.MainId == vehicle.MainId)?.DeletedDate != null;
            //    bool justReinstated = isPreviouslyDeleted && !vehicle.IsDeleted;
            //    bool justDeleted = !isPreviouslyDeleted && vehicle.IsDeleted;
            //    bool justUpdated = !previousVehicles.Any(x => x.Id == vehicle.Id) && vehicle.PreviousVehicleVersionId != null
            //                       && !(justReinstated || justDeleted);

            //    if (isNewlyAdded || justReinstated || justUpdated)
            //    {
            //        vehicle.EffectiveDate = riskDetail.EndorsementEffectiveDate;
            //    }
            //    else if (justDeleted)
            //    {
            //        vehicle.Delete(riskDetail.EndorsementEffectiveDate);
            //    }

            //    Console.WriteLine($"[{DateTime.Now:s}] VIN {vehicle.VIN} effective date updated.");
            //}


            await _riskCoverageRepository.SaveChangesAsync();
        }

        public async Task UpdateEffectiveDateByButton(Guid riskDetailId, DateTime effectiveDate)
        {
            Console.WriteLine($"[{DateTime.Now:s}] Updating effective dates of vehicle to {effectiveDate:MM/dd/yyyy}");
            var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);

            RiskDTO riskItem = await _riskService.GetByIdIncludeAsync(riskDetailId);
            if (riskItem.SubStatus != RiskDetailStatus.PendingEndorsement)
            {
                throw new Exception("Changing of effective date error. Please try again");
            }
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == risk.Id);
            if (effectiveDate >= brokerInfo.ExpirationDate)
                throw new InvalidOperationException("Endorsement effective date must be earlier than policy expiration date.");
            riskDetail.EndorsementEffectiveDate = effectiveDate;

            //Console.WriteLine($"[{DateTime.Now:s}] Retrieving vehicles...");
            //var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, true);
            //Console.WriteLine($"[{DateTime.Now:s}] Retrieving previous vehicles...");
            //var previousVehicles = await _vehicleRepository.GetByRiskDetailAsync(risk.CurrentRiskDetailId.Value, true);
            //foreach (var vehicle in vehicles)
            //{
            //    bool isNewlyAdded = !previousVehicles.Any(x => x.Id == vehicle.Id) && vehicle.PreviousVehicleVersionId is null;
            //    bool isPreviouslyDeleted = previousVehicles.FirstOrDefault(x => x.MainId == vehicle.MainId)?.DeletedDate != null;
            //    bool justReinstated = isPreviouslyDeleted && !vehicle.IsDeleted;
            //    bool justDeleted = !isPreviouslyDeleted && vehicle.IsDeleted;
            //    bool justUpdated = !previousVehicles.Any(x => x.Id == vehicle.Id) && vehicle.PreviousVehicleVersionId != null
            //                       && !(justReinstated || justDeleted);

            //    if (isNewlyAdded || justReinstated || justUpdated)
            //    {
            //        vehicle.EffectiveDate = riskDetail.EndorsementEffectiveDate;
            //    }
            //    else if (justDeleted)
            //    {
            //        vehicle.Delete(riskDetail.EndorsementEffectiveDate);
            //    }

            //    Console.WriteLine($"[{DateTime.Now:s}] VIN {vehicle.VIN} effective date updated.");
            //}


            await _riskCoverageRepository.SaveChangesAsync();
        }

        public async Task<int> GetNextEndorsementNumber(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var history = await _policyHistoryRepository.GetLatestByRisk(riskDetail.RiskId);
            return history == null ? 0 : (int.Parse(history.EndorsementNumber) + 1);
        }

        public async Task<EndorsementPremiumChangeSummaryDTO> GetPremiumChanges(Guid riskDetailId)
        {
            var result = new EndorsementPremiumChangeSummaryDTO();
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var unprocessedRiskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == risk.Id && x.IsProcessed != true);
            unprocessedRiskDetails = unprocessedRiskDetails.OrderByDescending(x => x.CreatedDate);
            var existingPendingChanges = _endorsementPendingChangeRepository.GetByRiskDetail(riskDetailId);
            if (unprocessedRiskDetails.Count() > 1)
            {
                riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(unprocessedRiskDetails.First().Id);
            }

            var coverage = riskDetail.RiskCoverages.FirstOrDefault(x => x.OptionNumber == binding.BindOptionId);
            riskDetailId = riskDetail.Id;

            var pendingChanges = (await _endorsementPendingChangeRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId)).ToList();
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            var riskManuscript = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);

            // Previous RiskDetail
            var previousRiskDetails = await _riskDetailRepository.GetAsync(rd => rd.RiskId == riskDetail.RiskId && rd.Id != riskDetailId && rd.Status != RiskDetailStatus.Canceled);
            //var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            //var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value);
            var isPreviousRiskDetailSubmission = previousRiskDetails.Last().Id == previousRiskDetail.Id;
            var previousPendingChanges = _endorsementPendingChangeRepository.GetByRiskDetail(previousRiskDetail.Id);
            var previousRiskCoverage = previousRiskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();
            // policy history
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            var lastEndorsementDate = previousRiskDetail.CreatedDate;
            var lastEndorsementEffectiveDate = previousRiskDetail.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate;
            var previousCoverage = previousRiskDetail.RiskCoverages.FirstOrDefault(x => x.OptionNumber == binding.BindOptionId);

            // Vehicle
            var currentVehicles = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(riskDetailId);
            var vehicleFactors = await _riskDetailRepository.GetVehicleRatingFactorsByRiskDetailId(riskDetailId);

            var totalVehicleAL = vehicleFactors.Sum(x => x.TotalALPremium ?? 0).RoundTo2Decimals();
            var totalVehiclePD = vehicleFactors.Sum(x => x.TotalPDPremium ?? 0).RoundTo2Decimals();
            var totalVehicleCP = vehicleFactors.Sum(x => x.CargoPremium ?? 0).RoundTo2Decimals();

            var HasCargo = coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId;
            var totalVehicle = totalVehicleAL +
                                totalVehiclePD +
                                 totalVehicleCP;

            result.VehicleChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = "Vehicle",
                CurrentPremium = totalVehicle,
                EndorsementPendingChange = SetEndorsementPendingChange("Vehicle", pendingChanges)
            });
            result.VehicleChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = Padding(25, "Auto Liability Premium"),
                CurrentPremium = totalVehicleAL,
                EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.VehicleALP, pendingChanges)
            });

            var pdpChanges = SetEndorsementPendingChange(BillingConstants.VehiclePDP, pendingChanges);
            var pdp = totalVehiclePD;
            if (pdp != 0 || pdpChanges.PremiumDifference != 0 ||
                (pdpChanges.PremiumDifference.Equals(0) && pdpChanges.PremiumAfterEndorsement != 0 && pdpChanges.PremiumBeforeEndorsement != 0))
            {
                result.VehicleChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "Physical Damage Premium"),
                    CurrentPremium = pdp,
                    EndorsementPendingChange = pdpChanges
                });
            }

            //if (HasCargo) {}
            var cargoChanges = SetEndorsementPendingChange(BillingConstants.VehicleCP, pendingChanges);
            var motorCargoPremium = totalVehicleCP;
            if (motorCargoPremium != 0 || cargoChanges.PremiumDifference != 0 ||
                        (cargoChanges.PremiumDifference.Equals(0) && cargoChanges.PremiumAfterEndorsement != 0 && cargoChanges.PremiumBeforeEndorsement != 0))
            {
                result.VehicleChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "Motor Truck Cargo Premium"),
                    CurrentPremium = motorCargoPremium,
                    EndorsementPendingChange = cargoChanges
                });
            }

            // Policy
            var endorsementEffectiveDate = (riskDetail.EndorsementEffectiveDate ?? DateTime.Now).Date;
            var hasApd = coverage.CompDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                         || coverage.FireDeductibleId > LimitConstants.ComprehensiveNoCoverageId
                         || coverage.CollDeductibleId > LimitConstants.CollisionNoCoverageId;
            var hasMtc = coverage.CargoLimitId > LimitConstants.CargoNoneCoverageId;
            //var glPremium = ((coverage.RiskCoveragePremium?.GeneralLiabilityPremium ?? 0) == 0
            //                 && previousRiskCoverage.RiskCoveragePremium.GeneralLiabilityPremium > 0)
            //                    ? previousRiskCoverage.RiskCoveragePremium.GeneralLiabilityPremium
            //                    : coverage.RiskCoveragePremium.GeneralLiabilityPremium;
            var additionalPremium = (coverage.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0) +
                                    (coverage.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0) +
                                    (coverage.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0);
            var alManuscriptPremium = riskManuscript.Where(m => (!m.IsDeleted || (m.ExpirationDate >= lastEndorsementEffectiveDate && !m.IsFlatCancelled && m.DeletedDate >= lastEndorsementDate && m.EffectiveDate < endorsementEffectiveDate)) && m.PremiumType == "AL").Sum(m => m.Premium);
            var pdManuscriptPremium = riskManuscript.Where(m => (!m.IsDeleted || (m.ExpirationDate >= lastEndorsementEffectiveDate && !m.IsFlatCancelled && m.DeletedDate >= lastEndorsementDate && m.EffectiveDate < endorsementEffectiveDate)) && m.PremiumType == "PD").Sum(m => m.Premium);
            var hiredPhysdamOtherContent = riskForms.FirstOrDefault(f => f.IsSelected.GetValueOrDefault() && f.FormId == "HiredPhysicalDamage")?.Other;
            var hiredPhysdamPremium = coverage.RiskCoveragePremium?.HiredPhysicalDamage ?? 0;
            var trailerOtherContent = riskForms.FirstOrDefault(f => f.IsSelected.GetValueOrDefault() && f.FormId == "TrailerInterchangeCoverage")?.Other;
            var premiumsSum = coverage.RiskCoveragePremium?.TrailerInterchange ?? 0;
            var glPremium = coverage.RiskCoveragePremium?.GeneralLiabilityPremium ?? 0;
            var tiPremium = coverage.RiskCoveragePremium?.TrailerInterchangeAL ?? 0;
            var noPremium = coverage.RiskCoveragePremium?.NonOwnedTrailerPremium ?? 0;
            var totalPolicy = additionalPremium + glPremium + alManuscriptPremium + tiPremium + noPremium +
                        pdManuscriptPremium + hiredPhysdamPremium + premiumsSum;

            result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = "Policy",
                CurrentPremium = totalPolicy,
                EndorsementPendingChange = SetEndorsementPendingChange("Policy", pendingChanges)
            });

            var attlInsuredChange = SetEndorsementPendingChange(BillingConstants.AdditionalInsured, pendingChanges);
            var waiveChange = SetEndorsementPendingChange(BillingConstants.WaiverOfSubrogation, pendingChanges);
            var primaryChange = SetEndorsementPendingChange(BillingConstants.PrimaryAndNoncontributory, pendingChanges);
            var attpP = coverage.RiskCoveragePremium?.AdditionalInsuredPremium ?? 0;
            var waivPre = coverage.RiskCoveragePremium?.WaiverOfSubrogationPremium ?? 0;
            var primaryCont = coverage.RiskCoveragePremium?.PrimaryNonContributoryPremium ?? 0;

            //if ()
            if (additionalPremium != 0 ||
                (attpP != 0 || attlInsuredChange.PremiumDifference != 0) ||
                (waivPre != 0 || waiveChange.PremiumDifference != 0) ||
                (primaryCont != 0 || primaryChange.PremiumDifference != 0))
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "Additional Premium (Fully Earned)"),
                    CurrentPremium = additionalPremium,
                    EndorsementPendingChange = SetEndorsementPendingChange("Additional Premium (Fully Earned)", pendingChanges)
                });

                if (attpP != 0 || attlInsuredChange.PremiumDifference != 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(50, "Additional Insured"),
                        CurrentPremium = attpP,
                        EndorsementPendingChange = attlInsuredChange
                    });
                }

                if (waivPre != 0 || waiveChange.PremiumDifference != 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(50, "Waiver of Subrogation"),
                        CurrentPremium = waivPre,
                        EndorsementPendingChange = waiveChange
                    });
                }

                if (primaryCont != 0 || primaryChange.PremiumDifference != 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(50, "Primary & Non-Contributory"),
                        CurrentPremium = primaryCont,
                        EndorsementPendingChange = primaryChange
                    });
                }
            }

            //if (hasApd || hasMtc) {}
            var glPremiumChange = SetEndorsementPendingChange(BillingConstants.GeneralLiabilityPremium, pendingChanges);
            if (glPremium != 0 || glPremiumChange.PremiumDifference != 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "General Liability Premium"),
                    CurrentPremium = glPremium,
                    EndorsementPendingChange = glPremiumChange
                });
            }
            else if ((previousRiskCoverage.RiskCoveragePremium.ProratedGeneralLiabilityPremium ?? 0) > 0 && (coverage.RiskCoveragePremium?.GeneralLiabilityPremium ?? 0) == 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "General Liability Premium"),
                    CurrentPremium = 0,
                    EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.GeneralLiabilityPremium, pendingChanges)
                });
            }

            #region Rater V7 Supported Fields

            if (risk.RaterVersionId >= 7)
            {
                var tiPremiumChange = SetEndorsementPendingChange(BillingConstants.TrailerInterchangePremium, pendingChanges);
                if (tiPremium != 0 || tiPremiumChange.PremiumDifference != 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(25, "Trailer Interchange Premium"),
                        CurrentPremium = tiPremium,
                        EndorsementPendingChange = tiPremiumChange
                    });
                }
                else if ((previousRiskCoverage.RiskCoveragePremium.ProratedTrailerInterchangePremiumAL ?? 0) > 0 && (coverage.RiskCoveragePremium?.TrailerInterchangeAL ?? 0) == 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(25, "Trailer Interchange Premium"),
                        CurrentPremium = 0,
                        EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.TrailerInterchangePremium, pendingChanges)
                    });
                }

                var noPremiumChange = SetEndorsementPendingChange(BillingConstants.NonOwnedTrailerPremium, pendingChanges);
                if (noPremium != 0 || noPremiumChange.PremiumDifference != 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(25, "Non-Owned Trailer Premium"),
                        CurrentPremium = noPremium,
                        EndorsementPendingChange = noPremiumChange
                    });
                }
                else if ((previousRiskCoverage.RiskCoveragePremium.ProratedNonOwnedTrailerPremium ?? 0) > 0 && (coverage.RiskCoveragePremium?.NonOwnedTrailerPremium ?? 0) == 0)
                {
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(25, "Non-Owned Trailer Premium"),
                        CurrentPremium = 0,
                        EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.NonOwnedTrailerPremium, pendingChanges)
                    });
                }
            }

            #endregion

            var alManuscriptPremiumChange = SetEndorsementPendingChange(BillingConstants.ALManuscriptPremium, pendingChanges);
            var isAllManuscriptALDeletedButHaveProrateAmt = riskManuscript.Where(x => x.IsActive && x.ProratedPremium != 0 && x.PremiumType == "AL").Count() == 0;
            if (alManuscriptPremium != 0 || alManuscriptPremiumChange.PremiumDifference != 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "AL Manuscript Premium"),
                    CurrentPremium = alManuscriptPremium,
                    EndorsementPendingChange = alManuscriptPremiumChange
                });
            }
            else if (isAllManuscriptALDeletedButHaveProrateAmt && riskManuscript.Count(x => x.PremiumType == "AL") > 0 && alManuscriptPremiumChange.PremiumBeforeEndorsement != 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "AL Manuscript Premium"),
                    CurrentPremium = 0,
                    EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.ALManuscriptPremium, pendingChanges)
                });
            }

            var pdManuscriptPremiumChange = SetEndorsementPendingChange(BillingConstants.PDManuscriptPremium, pendingChanges);
            var isAllManuscriptPDDeletedButHaveProrateAmt = riskManuscript.Where(x => x.IsActive && x.ProratedPremium != 0 && x.PremiumType == "PD").Count() == 0;
            if (pdManuscriptPremium != 0 || pdManuscriptPremiumChange.PremiumDifference != 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "PD Manuscript Premium"),
                    CurrentPremium = pdManuscriptPremium,
                    EndorsementPendingChange = pdManuscriptPremiumChange
                });
            }
            else if (isAllManuscriptPDDeletedButHaveProrateAmt && riskManuscript.Count(x => x.PremiumType == "PD") > 0 && pdManuscriptPremiumChange.PremiumBeforeEndorsement != 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "PD Manuscript Premium"),
                    CurrentPremium = 0,
                    EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.PDManuscriptPremium, pendingChanges)
                });
            }


            //if (hiredPhysdamOtherContent != null) {}
            var hiredPhysdamChange = SetEndorsementPendingChange(BillingConstants.HiredPhysicalDamageEndst, pendingChanges);
            if (hiredPhysdamPremium != 0 || hiredPhysdamChange.PremiumDifference != 0)
            {
                result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                {
                    Description = Padding(25, "PD Hired Physical Damage"),
                    CurrentPremium = hiredPhysdamPremium,
                    EndorsementPendingChange = hiredPhysdamChange
                });
            }

            if (risk.RaterVersionId < 7)
            {
                var trailerOtherChange = SetEndorsementPendingChange(BillingConstants.TrailerInterchangeEndst, pendingChanges);
                if (premiumsSum != 0 || trailerOtherChange.PremiumDifference != 0)
                {
                    //if (premiumsSum != 0) {}
                    result.PolicyChangeList.Add(new EndorsementPremiumChangeDTO()
                    {
                        Description = Padding(25, "PD Trailer Interchange"),
                        CurrentPremium = premiumsSum,
                        EndorsementPendingChange = trailerOtherChange
                    });
                }
            }

            // Total Premium (!m.IsDeleted || (m.ExpirationDate >= lastEndorsementEffectiveDate && !m.IsFlatCancelled ))
            var deletedProratedManuscriptPremium = riskManuscript.Where(x => x.IsDeleted && x.IsProrate && x.ProratedPremium != 0
                                                                            && x.ExpirationDate >= lastEndorsementEffectiveDate
                                                                            && x.DeletedDate >= lastEndorsementDate).Sum(x => x.Premium); // include annual premium of deleted prorated manuscripts
            result.TotalPremiumChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = "Total Premium",
                CurrentPremium = coverage.RiskCoveragePremium?.Premium + deletedProratedManuscriptPremium,
                EndorsementPendingChange = SetEndorsementPendingChange("Total Premium", pendingChanges)
            });

            var taxDetail = await _taxService.GetTaxDetail(coverage.RiskCoveragePremium.TaxDetailsId.Value);
            result.TotalPremiumChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = Padding(25, "Surplus Lines Tax"),
                CurrentPremium = taxDetail?.TotalSLTax ?? 0,
                EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.SurplusLinesTax, pendingChanges)
            });
            result.TotalPremiumChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = Padding(25, "Stamping Fee"),
                CurrentPremium = taxDetail?.TotalStampingFee ?? 0,
                EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.StampingFee, pendingChanges)
            });

            // Fees
            decimal? totalFees = (coverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0) + (coverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0);
            result.FeesChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = "Fees (Fully Earned)",
                CurrentPremium = totalFees ?? 0,
                EndorsementPendingChange = SetEndorsementPendingChange("Fees (Fully Earned)", pendingChanges)
            });
            result.FeesChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = Padding(25, "Service Fee - AL"),
                CurrentPremium = ((coverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0) < (previousRiskCoverage?.RiskCoveragePremium?.RiskMgrFeeAL ?? 0)) ? previousRiskCoverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0 : coverage.RiskCoveragePremium?.RiskMgrFeeAL ?? 0,
                EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.ServiceFeeAL, pendingChanges)
            });
            result.FeesChangeList.Add(new EndorsementPremiumChangeDTO()
            {
                Description = Padding(25, "Service Fee - PD"),
                CurrentPremium = ((coverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0) < (previousRiskCoverage?.RiskCoveragePremium?.RiskMgrFeePD ?? 0)) ? previousRiskCoverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0 : coverage.RiskCoveragePremium?.RiskMgrFeePD ?? 0,
                EndorsementPendingChange = SetEndorsementPendingChange(BillingConstants.ServiceFeePD, pendingChanges)
            });

            // Get all previous riskdetail endorsement Premium After changes

            return result;
        }

        public static decimal FindDifference(decimal nr1, decimal nr2)
        {
            //return Math.Abs(nr1 - nr2);
            return nr1 - nr2;
        }

        private static string Padding(int size, dynamic value)
        {
            if (size == 25)
                return $"<div class='pad-25'>{value}</div>";
            return $"<div class='pad-50'>{value}</div>";
        }

        private static EndorsementPendingChangeDTO SetEndorsementPendingChange(string amountSubTypeId, List<EndorsementPendingChange> pendingChanges)
        {
            var result = new EndorsementPendingChangeDTO();
            if (pendingChanges.Count() > 0)
            {
                switch (amountSubTypeId)
                {
                    case BillingConstants.VehicleALP:
                        var vehicleAL = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP).ToList();
                        result.PremiumBeforeEndorsement = vehicleAL.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = vehicleAL.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = vehicleAL.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case BillingConstants.VehiclePDP:
                        var vehiclePD = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehiclePDP).ToList();
                        result.PremiumBeforeEndorsement = vehiclePD.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = vehiclePD.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = vehiclePD.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case BillingConstants.VehicleCP:
                        var vehicleCP = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleCP).ToList();
                        result.PremiumBeforeEndorsement = vehicleCP.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = vehicleCP.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = vehicleCP.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case "Vehicle":
                        var vehicles = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP ||
                                    x.AmountSubTypeId == BillingConstants.VehiclePDP ||
                                    x.AmountSubTypeId == BillingConstants.VehicleCP).ToList();
                        result.PremiumBeforeEndorsement = vehicles.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = vehicles.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = vehicles.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case "Policy":
                        var policy = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.AdditionalInsured || x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation ||
                                    x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory || x.AmountSubTypeId == BillingConstants.GeneralLiabilityPremium ||
                                    x.AmountSubTypeId == BillingConstants.TrailerInterchangePremium || x.AmountSubTypeId == BillingConstants.NonOwnedTrailerPremium ||
                                    x.AmountSubTypeId == BillingConstants.ALManuscriptPremium || x.AmountSubTypeId == BillingConstants.PDManuscriptPremium ||
                                    x.AmountSubTypeId == BillingConstants.HiredPhysicalDamageEndst || x.AmountSubTypeId == BillingConstants.TrailerInterchangeEndst).ToList();
                        result.PremiumBeforeEndorsement = policy.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = policy.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = policy.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case "Additional Premium (Fully Earned)":
                        var addtlPrem = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.AdditionalInsured || x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation ||
                                    x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory).ToList();
                        result.PremiumBeforeEndorsement = addtlPrem.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = addtlPrem.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = addtlPrem.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case "Total Premium":
                        var totalPrem = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP ||
                                    x.AmountSubTypeId == BillingConstants.VehiclePDP || x.AmountSubTypeId == BillingConstants.VehicleCP ||
                                    x.AmountSubTypeId == BillingConstants.AdditionalInsured || x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation ||
                                    x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory || x.AmountSubTypeId == BillingConstants.GeneralLiabilityPremium ||
                                    x.AmountSubTypeId == BillingConstants.TrailerInterchangePremium || x.AmountSubTypeId == BillingConstants.NonOwnedTrailerPremium ||
                                    x.AmountSubTypeId == BillingConstants.ALManuscriptPremium || x.AmountSubTypeId == BillingConstants.PDManuscriptPremium ||
                                    x.AmountSubTypeId == BillingConstants.HiredPhysicalDamageEndst || x.AmountSubTypeId == BillingConstants.TrailerInterchangeEndst).ToList();
                        result.PremiumBeforeEndorsement = totalPrem.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = totalPrem.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = totalPrem.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    case "Fees (Fully Earned)":
                        var feesData = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeeAL || x.AmountSubTypeId == BillingConstants.ServiceFeePD).ToList();
                        result.PremiumBeforeEndorsement = feesData.Sum(x => (x.PremiumBeforeEndorsement ?? 0));
                        result.PremiumAfterEndorsement = feesData.Sum(x => (x.PremiumAfterEndorsement ?? 0));
                        result.PremiumDifference = feesData.Sum(x => (x.PremiumDifference ?? 0));
                        break;
                    default:
                        // Additional Insured
                        // Waiver of Subrogation
                        // Primary & Non-Contributory
                        // General Liability Premium
                        // AL Manuscript Premium
                        // PD Manuscript Premium
                        // PD Hired Physical Damage
                        // PD Trailer Interchange
                        // Surplus Lines Tax
                        // Stamping Fee
                        // Service Fee - AL
                        // Service Fee - PD
                        var defaultData = pendingChanges.Where(x => x.AmountSubTypeId == amountSubTypeId).FirstOrDefault();
                        result.PremiumBeforeEndorsement = defaultData?.PremiumBeforeEndorsement ?? 0;
                        result.PremiumAfterEndorsement = defaultData?.PremiumAfterEndorsement ?? 0;
                        result.PremiumDifference = defaultData?.PremiumDifference ?? 0;
                        break;
                }
            }

            return result;
        }

        public async Task CalculatePremiumChanges(Guid riskDetailId, DateTime effectiveDate, int optionId)
        {
            _context.ChangeTracker.Clear();
            // Current RiskDetail
            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(currentRiskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(risk.Id);

            var unprocessedRiskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == risk.Id && x.IsProcessed != true);
            unprocessedRiskDetails = unprocessedRiskDetails.OrderByDescending(x => x.CreatedDate);
            var existingPendingChanges = await _endorsementPendingChangeRepository.GetByRiskDetailAsync(riskDetailId);
            if (unprocessedRiskDetails.Count() > 1)
            {
                currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(unprocessedRiskDetails.First().Id);
            }

            var policyHistory = await _policyHistoryRepository.GetAsync(x => x.RiskDetailId == currentRiskDetail.Id);
            var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == risk.Id.ToString());
            var currentVehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailId, true);

            var previousRiskDetailId = risk.CurrentRiskDetailId.Value;
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);
            var previousVehicles = await _vehicleRepository.GetByRiskDetailAsync(previousRiskDetailId, true);
            previousVehicles = previousVehicles.Where(x => IsIncludedInOption(x.Options, optionId) == true).ToList();

            var previousPendingChanges = await _endorsementPendingChangeRepository.GetByRiskDetailAsync(previousRiskDetailId);
            var proratedPremiumCalculator = new ProratedPremiumCalculator();

            var annualDays = (brokerInfo.ExpirationDate - brokerInfo.EffectiveDate)?.TotalDays ?? 0;
            var pendingChanges = new List<EndorsementPendingChange>();

            foreach (var change in existingPendingChanges)
            {
                await _endorsementPendingChangeRepository.DeleteAsync(change);
            }

            pendingChanges.AddRange(await GetVehiclePremiumChanges(previousRiskDetail, currentRiskDetail));

            #region Policy Premium changes 

            var policyLevelChanges = new List<EndorsementPendingChange>();


            var currentRiskCoveragePremium = currentRiskDetail.RiskCoverages.First(x => x.OptionNumber == optionId).RiskCoveragePremium;
            var previousRiskCoveragePremium = previousRiskDetail.RiskCoverages.First(x => x.OptionNumber == optionId).RiskCoveragePremium;
            bool IsPreviousGLPremiumExpired = await IsPreviousGeneralLiabilityExpired(risk.Id, effectiveDate);

            // General Liability
            var glPremiumsBefore = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(previousRiskDetailId, Premium.TYPE_POLICY_GL);
            var glPremiumsAfter = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(currentRiskDetail.Id, Premium.TYPE_POLICY_GL);

            var generalLiabilityChanges = new EndorsementPendingChange();
            generalLiabilityChanges.PremiumBeforeEndorsement = glPremiumsBefore.TotalProratedPremium;
            generalLiabilityChanges.PremiumAfterEndorsement = glPremiumsAfter.TotalProratedPremium;
            generalLiabilityChanges.AmountSubTypeId = BillingConstants.GeneralLiabilityPremium;
            generalLiabilityChanges.PremiumDifference = glPremiumsAfter.TotalProratedPremium - glPremiumsBefore.TotalProratedPremium;
            generalLiabilityChanges.DateCreated = DateTime.Now;
            policyLevelChanges.Add(generalLiabilityChanges);

            if (risk.RaterVersionId >= 7)
            {
                //Trailer Interchange AL
                var tiPremiumsBefore = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(previousRiskDetailId, Premium.TYPE_POLICY_TI);
                var tiPremiumsAfter = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(currentRiskDetail.Id, Premium.TYPE_POLICY_TI);

                if (tiPremiumsAfter != null)
                {
                    var trailerInterchangeALChanges = new EndorsementPendingChange();
                    trailerInterchangeALChanges.PremiumBeforeEndorsement = tiPremiumsBefore.TotalProratedPremium;
                    trailerInterchangeALChanges.PremiumAfterEndorsement = tiPremiumsAfter.TotalProratedPremium;
                    trailerInterchangeALChanges.AmountSubTypeId = BillingConstants.TrailerInterchangePremium;
                    trailerInterchangeALChanges.PremiumDifference = tiPremiumsAfter.TotalProratedPremium - tiPremiumsBefore.TotalProratedPremium;
                    trailerInterchangeALChanges.DateCreated = DateTime.Now;
                    policyLevelChanges.Add(trailerInterchangeALChanges);
                }

                //Non-Owned Trailer
                var noPremiumsBefore = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(previousRiskDetailId, Premium.TYPE_POLICY_NO);
                var noPremiumsAfter = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(currentRiskDetail.Id, Premium.TYPE_POLICY_NO);

                if (noPremiumsAfter != null)
                {
                    var NonOwnedTrailerChanges = new EndorsementPendingChange();
                    NonOwnedTrailerChanges.PremiumBeforeEndorsement = noPremiumsBefore.TotalProratedPremium;
                    NonOwnedTrailerChanges.PremiumAfterEndorsement = noPremiumsAfter.TotalProratedPremium;
                    NonOwnedTrailerChanges.AmountSubTypeId = BillingConstants.NonOwnedTrailerPremium;
                    NonOwnedTrailerChanges.PremiumDifference = noPremiumsAfter.TotalProratedPremium - noPremiumsBefore.TotalProratedPremium;
                    NonOwnedTrailerChanges.DateCreated = DateTime.Now;
                    policyLevelChanges.Add(NonOwnedTrailerChanges);
                }
            }

            // Manuscript
            var currentManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            var previousManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);
            var manuscriptPremiumChanges = new List<EndorsementPendingChange>();
            if (currentManuscripts.Any())
            {
                var addedManuscripts = currentManuscripts.Where(newObj => previousManuscripts.All(oldObj => oldObj.CreatedDate != newObj.CreatedDate) && newObj.DeletedDate == null);

                // get all added manuscripts
                foreach (var addedManuscript in addedManuscripts)
                {
                    var addedManuscriptEndorsmentChange = new EndorsementPendingChange();
                    if (addedManuscript.IsSelected ?? false)
                    {
                        addedManuscriptEndorsmentChange.AmountSubTypeId = (addedManuscript.PremiumType == "AL") ? BillingConstants.ALManuscriptPremium : BillingConstants.PDManuscriptPremium;
                        addedManuscriptEndorsmentChange.PremiumBeforeEndorsement = 0;
                        if (addedManuscript.IsProrate)
                        {
                            addedManuscriptEndorsmentChange.PremiumAfterEndorsement = proratedPremiumCalculator.CalculateAdded(addedManuscript.Premium, brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value, effectiveDate);
                            // update manuscript prorated premium
                            await UpdateManuscriptProratedPremium(addedManuscript, addedManuscriptEndorsmentChange.PremiumAfterEndorsement ?? 0);
                        }
                        else
                        {
                            addedManuscriptEndorsmentChange.PremiumAfterEndorsement = addedManuscript.Premium;
                        }
                        addedManuscriptEndorsmentChange.PremiumDifference = (addedManuscriptEndorsmentChange.PremiumAfterEndorsement - addedManuscriptEndorsmentChange.PremiumBeforeEndorsement);

                        manuscriptPremiumChanges.Add(addedManuscriptEndorsmentChange);
                    }
                }

                // check all existing manuscripts
                foreach (var previousManuscript in previousManuscripts)
                {
                    var currentManuscript = currentManuscripts.FirstOrDefault(x => x.CreatedDate == previousManuscript.CreatedDate);

                    if (currentManuscript != null)
                    {
                        // if manuscript is deleted
                        if (previousManuscript.DeletedDate == null && currentManuscript.DeletedDate != null)
                        {
                            var removedManuscriptEndorsementChange = new EndorsementPendingChange
                            {
                                AmountSubTypeId = (previousManuscript.PremiumType == "AL") ? BillingConstants.ALManuscriptPremium : BillingConstants.PDManuscriptPremium
                            };
                            if (previousManuscript.IsProrate)
                            {
                                removedManuscriptEndorsementChange.PremiumBeforeEndorsement = previousManuscript.ProratedPremium;
                                removedManuscriptEndorsementChange.PremiumAfterEndorsement = proratedPremiumCalculator.CalculateRemoved(previousManuscript.ProratedPremium, currentManuscript.EffectiveDate, brokerInfo.ExpirationDate.Value, effectiveDate);
                                // update manuscript prorated premium
                                await UpdateManuscriptProratedPremium(currentManuscript, removedManuscriptEndorsementChange.PremiumAfterEndorsement ?? 0);
                            }
                            else
                            {
                                removedManuscriptEndorsementChange.PremiumBeforeEndorsement = previousManuscript.Premium;
                                removedManuscriptEndorsementChange.PremiumAfterEndorsement = 0;
                            }
                            removedManuscriptEndorsementChange.PremiumDifference = (removedManuscriptEndorsementChange.PremiumAfterEndorsement - removedManuscriptEndorsementChange.PremiumBeforeEndorsement);

                            manuscriptPremiumChanges.Add(removedManuscriptEndorsementChange);
                        }

                        // if there are no changes on the manuscript return annual premium
                        if (((previousManuscript.DeletedDate == null && currentManuscript.DeletedDate == null)
                                && (previousManuscript.Premium == currentManuscript.Premium))
                             || (previousManuscript.DeletedDate != null && currentManuscript.DeletedDate != null &&
                                previousManuscript.IsProrate && currentManuscript.IsProrate &&
                                previousManuscript.ProratedPremium != 0 && currentManuscript.ProratedPremium != 0))
                        {
                            var currentManuscriptEndorsementChange = new EndorsementPendingChange();
                            currentManuscriptEndorsementChange.AmountSubTypeId = (previousManuscript.PremiumType == "AL") ? BillingConstants.ALManuscriptPremium : BillingConstants.PDManuscriptPremium;
                            currentManuscriptEndorsementChange.PremiumBeforeEndorsement = currentManuscript.ProratedPremium;
                            currentManuscriptEndorsementChange.PremiumAfterEndorsement = currentManuscript.ProratedPremium;
                            currentManuscriptEndorsementChange.PremiumDifference = (currentManuscriptEndorsementChange.PremiumAfterEndorsement - currentManuscriptEndorsementChange.PremiumBeforeEndorsement);
                            manuscriptPremiumChanges.Add(currentManuscriptEndorsementChange);
                        }
                    }
                }

                // after collecting all the manuscript AL and PD, need to separate records
                // by amount sub type and save only single record in DB
                var collectedALManuscriptPremiumChanges = new EndorsementPendingChange();
                var collectedPDManuscriptPremiumChanges = new EndorsementPendingChange();

                // collect all AL Manuscripts
                if (manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.ALManuscriptPremium).Count() > 0)
                {
                    collectedALManuscriptPremiumChanges.AmountSubTypeId = BillingConstants.ALManuscriptPremium;
                    collectedALManuscriptPremiumChanges.PremiumBeforeEndorsement = manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.ALManuscriptPremium).Sum(x => x.PremiumBeforeEndorsement);
                    collectedALManuscriptPremiumChanges.PremiumAfterEndorsement = manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.ALManuscriptPremium).Sum(x => x.PremiumAfterEndorsement);
                    collectedALManuscriptPremiumChanges.PremiumDifference = manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.ALManuscriptPremium).Sum(x => x.PremiumDifference);
                    collectedALManuscriptPremiumChanges.DateCreated = DateTime.Now;
                    policyLevelChanges.Add(collectedALManuscriptPremiumChanges);
                }

                // collect all PD Manuscripts
                if (manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.PDManuscriptPremium).Count() > 0)
                {
                    collectedPDManuscriptPremiumChanges.AmountSubTypeId = BillingConstants.PDManuscriptPremium;
                    collectedPDManuscriptPremiumChanges.PremiumBeforeEndorsement = manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.PDManuscriptPremium).Sum(x => x.PremiumBeforeEndorsement);
                    collectedPDManuscriptPremiumChanges.PremiumAfterEndorsement = manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.PDManuscriptPremium).Sum(x => x.PremiumAfterEndorsement);
                    collectedPDManuscriptPremiumChanges.PremiumDifference = manuscriptPremiumChanges.Where(x => x.AmountSubTypeId == BillingConstants.PDManuscriptPremium).Sum(x => x.PremiumDifference);
                    collectedPDManuscriptPremiumChanges.DateCreated = DateTime.Now;
                    policyLevelChanges.Add(collectedPDManuscriptPremiumChanges);
                }
            }


            if (policyLevelChanges.Count > 0)
            {
                pendingChanges.AddRange(policyLevelChanges);
            }

            #endregion

            pendingChanges = pendingChanges.Where(x => !(x.PremiumBeforeEndorsement == 0 && x.PremiumAfterEndorsement == 0)).ToList();

            pendingChanges.ForEach(x => x.RiskDetailId = riskDetailId);
            // update risk coverage premium
            await UpdateCurrentRiskCoveragePremium(riskDetailId, pendingChanges, currentVehicles, optionId, currentRiskDetail, previousRiskDetail);

            await _endorsementPendingChangeRepository.AddRangeAsync(pendingChanges);
            await _endorsementPendingChangeRepository.SaveChangesAsync();

            #region Additional Premiums
            await SetAdditionalPremiumChanges(riskDetailId, previousRiskDetail.Id);
            #endregion

            #region Fees change
            await SetServiceFeeChanges(currentRiskDetail);
            #endregion

            #region Taxes
            await SetTaxesChanges(currentRiskDetail, brokerInfo.EffectiveDate.Value);
            #endregion
        }

        public async Task UpdateCurrentRiskCoveragePremium(Guid riskDetailId, List<EndorsementPendingChange> pendingChanges, IEnumerable<Vehicle> currentVehicles, int optionNumber, RiskDetail currentRiskDetail, RiskDetail previousRiskDetail)
        {
            var riskCoverages = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            bool hasHiredPhysicalDamage = riskForms.Any(x => x.FormId == "HiredPhysicalDamage" && (x.IsSelected ?? false));
            bool hasTrailerInterchange = riskForms.Any(x => x.FormId == "TrailerInterchangeCoverage" && (x.IsSelected ?? false));
            var riskCoverage = riskCoverages.Where(x => x.OptionNumber == optionNumber).FirstOrDefault();
            var riskCoveragePremium = riskCoverage.RiskCoveragePremium;

            var generalLiabilityPremiumChange = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.GeneralLiabilityPremium).Sum(x => x.PremiumAfterEndorsement ?? 0);
            var trailerInterchangeALPremiumChange = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.TrailerInterchangePremium).Sum(x => x.PremiumAfterEndorsement ?? 0);
            var nonOwnedTrailerPremiumChange = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.NonOwnedTrailerPremium).Sum(x => x.PremiumAfterEndorsement ?? 0);
            var alManuscriptPremiumChange = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.ALManuscriptPremium).Sum(x => x.PremiumAfterEndorsement ?? 0);
            var pdManuscriptPremiumChange = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.PDManuscriptPremium).Sum(x => x.PremiumAfterEndorsement ?? 0);

            riskCoveragePremium.ProratedALManuscriptPremium = (alManuscriptPremiumChange == 0) ? riskCoveragePremium.ALManuscriptPremium : alManuscriptPremiumChange;
            riskCoveragePremium.ProratedPDManuscriptPremium = (pdManuscriptPremiumChange == 0) ? riskCoveragePremium.PDManuscriptPremium : pdManuscriptPremiumChange;
            riskCoveragePremium.ProratedGeneralLiabilityPremium = (generalLiabilityPremiumChange == 0) ? riskCoveragePremium.GeneralLiabilityPremium : generalLiabilityPremiumChange;
            riskCoveragePremium.ProratedTrailerInterchangePremiumAL = (trailerInterchangeALPremiumChange == 0) ? riskCoveragePremium.TrailerInterchangeAL : trailerInterchangeALPremiumChange;
            riskCoveragePremium.ProratedNonOwnedTrailerPremium = (nonOwnedTrailerPremiumChange == 0) ? riskCoveragePremium.NonOwnedTrailerPremium : nonOwnedTrailerPremiumChange;

            string[] NonPremiumAmountSubTypeId = { "PDSF", "ALSF", "SF", "SLT" };
            var totalPolicyPremium = pendingChanges.Where(x => !NonPremiumAmountSubTypeId.Contains(x.AmountSubTypeId)).Sum(x => (x.PremiumAfterEndorsement ?? 0).RoundTo2Decimals());

            var totalProratedALPremium = pendingChanges.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP).Sum(x => (x.PremiumAfterEndorsement ?? 0).RoundTo2Decimals());

            if (hasHiredPhysicalDamage)
            {
                totalPolicyPremium += riskCoveragePremium.HiredPhysicalDamage ?? 0;
            }

            if (hasTrailerInterchange)
            {
                totalPolicyPremium += riskCoveragePremium.TrailerInterchange ?? 0;
            }

            riskCoveragePremium.ProratedPremium = totalPolicyPremium +
                                                    (riskCoveragePremium.AdditionalInsuredPremium ?? 0) +
                                                    (riskCoveragePremium.WaiverOfSubrogationPremium ?? 0) +
                                                    (riskCoveragePremium.PrimaryNonContributoryPremium ?? 0);

            riskCoveragePremium.ARSCommissionAmount = (totalProratedALPremium +
                                                    (riskCoveragePremium.AdditionalInsuredPremium ?? 0) +
                                                    (riskCoveragePremium.WaiverOfSubrogationPremium ?? 0) +
                                                    (riskCoveragePremium.PrimaryNonContributoryPremium ?? 0)) * (Convert.ToDecimal(riskCoverage?.ARSCommission) / 100m);

            var currentAutoLiabilityUnits = 0;
            var currentPhysicalDamageUnits = 0;
            var previousAutoLiabilityUnits = 0;
            var previousPhysicalDamageUnits = 0;

            RiskCoverage previousRiskCoverage = currentRiskDetail.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == optionNumber);

            var allPreviousVehicles = (await _vehicleService.GetByRiskDetailIdIncludeAsync(previousRiskDetail.Id, true))?
                .OrderBy(vehicle => vehicle.VIN).ToList();
            allPreviousVehicles = allPreviousVehicles.Where(x => ((x.Options ?? "").Split(',')).Contains(previousRiskCoverage.OptionNumber.ToString()) == true && x.IsValidated == true).ToList();

            previousAutoLiabilityUnits = allPreviousVehicles.Count;

            foreach (var allPreviousVehicle in allPreviousVehicles)
            {
                if (allPreviousVehicle.HasCompFtLimit() || allPreviousVehicle.HasCollisionLimit())
                {
                    previousPhysicalDamageUnits++;
                }
            }

            RiskCoverage currentRiskCoverage = currentRiskDetail.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == optionNumber);

            var allCurrentVehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(currentRiskDetail.Id, withDeleted: true);

            allCurrentVehicles = allCurrentVehicles.Where(x => ((x.Options ?? "").Split(',')).Contains(currentRiskCoverage.OptionNumber.ToString()) == true && x.IsValidated == true).ToList();

            currentAutoLiabilityUnits = allCurrentVehicles.Count;

            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == currentRiskDetail.RiskId && x.IsProcessed == true);
            riskDetails = riskDetails.OrderByDescending(x => x.CreatedDate);
            var riskDetailIds = riskDetails.Select(x => x.Id);

            foreach (var currentVehicle in allCurrentVehicles)
            {
                var hasPreviousCompFTLimitOrCollisionLimit = false;

                foreach (var id in riskDetailIds)
                {
                    var riskDetailVehicle = await _vehicleRepository.GetByRiskDetailAndMainIdAsync(id, currentVehicle.MainId);
                    if (riskDetailVehicle is null)
                        break;

                    hasPreviousCompFTLimitOrCollisionLimit = riskDetailVehicle.HasCompFtLimit() || riskDetailVehicle.HasCollisionLimit();

                    if (hasPreviousCompFTLimitOrCollisionLimit)
                        break;
                }

                if ((currentVehicle.HasCompFtLimit() || currentVehicle.HasCollisionLimit()) && !hasPreviousCompFTLimitOrCollisionLimit)
                {
                    currentPhysicalDamageUnits++;
                }
            }

            var nameAddress = await _nameAndAddressService.GetNameAndAddress(currentRiskDetail.Id);
            var feeDetails = _billingService.GetFeeDetailsByState(nameAddress?.State);
            decimal RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultiplePDServiceFee)?.Amount ?? 0;

            decimal RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultiplePDServiceFee)?.Amount ?? 0;

            decimal RISK_MANAGEMENT_FEE_FLAT_AL_MID_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMidtermALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_FLAT_PD_MID_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMidtermPDServiceFee)?.Amount ?? 0;

            var billingFees = await _billingService.GetBillingFeesAsync(currentRiskDetail.RiskId);
            decimal? alServiceFees = 0;
            decimal? pdServiceFees = 0;

            var hasSingleALPDServiceFee = feeDetails?.Any(feeDetail => feeDetail.Description == FeeDetails.SingleALServiceFee &&
                feeDetail.Description == FeeDetails.SinglePDServiceFee) ?? false;

            if (billingFees.Any() || !hasSingleALPDServiceFee)
            {
                alServiceFees = billingFees.Where(a => a.Description == "Service Fee - AL")?.Sum(a => a.Amount) ?? 0;
                pdServiceFees = billingFees.Where(a => a.Description == "Service Fee - PD")?.Sum(a => a.Amount) ?? 0;

                if (currentAutoLiabilityUnits > previousAutoLiabilityUnits)
                {
                    if (RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT > 0)
                        alServiceFees += RISK_MANAGEMENT_FEE_FLAT_AL_MID_UNIT;
                    else if (RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT > 0)
                        alServiceFees += (RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT * (currentAutoLiabilityUnits - previousAutoLiabilityUnits));
                }

                if (currentPhysicalDamageUnits > 0)
                {
                    if (RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT > 0)
                        pdServiceFees += RISK_MANAGEMENT_FEE_FLAT_PD_MID_UNIT;
                    else if (RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT > 0)
                        pdServiceFees += (RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT * currentPhysicalDamageUnits);
                }

                riskCoveragePremium.RiskMgrFeeAL = alServiceFees;
                riskCoveragePremium.RiskMgrFeePD = pdServiceFees;
                riskCoveragePremium.Fees = alServiceFees + pdServiceFees;
            }

            riskCoverage.RiskCoveragePremium = riskCoveragePremium;
            _riskCoverageRepository.Update(riskCoverage);
            await _riskCoverageRepository.SaveChangesAsync();

            var saveCurrentRiskCoverages = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(currentRiskDetail.Id);
            var saveRiskCoverage = saveCurrentRiskCoverages.Where(x => x.OptionNumber == optionNumber).FirstOrDefault();
            var saveRiskCoveragePremium = saveRiskCoverage.RiskCoveragePremium;

            if (billingFees.Any() || !hasSingleALPDServiceFee)
            {
                saveRiskCoveragePremium.RiskMgrFeeAL = alServiceFees;
                saveRiskCoveragePremium.RiskMgrFeePD = pdServiceFees;
                saveRiskCoveragePremium.Fees = alServiceFees + pdServiceFees;
            }

            saveRiskCoverage.RiskCoveragePremium = saveRiskCoveragePremium;
            _riskCoverageRepository.Update(saveRiskCoverage);
            await _riskCoverageRepository.SaveChangesAsync();
        }

        private async Task UpdateManuscriptProratedPremium(RiskManuscripts manuscripts, decimal proratedPremium)
        {
            manuscripts.ProratedPremium = proratedPremium;
            _riskManuscriptsRepository.Update(manuscripts);
            await _riskManuscriptsRepository.SaveChangesAsync();
        }

        private async Task<bool> IsPreviousGeneralLiabilityExpired(Guid riskId, DateTime effectiveDate)
        {
            var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == riskId.ToString());
            bool isGLPremiumExpired = false;

            foreach (var policyHistory in policyHistories.OrderByDescending(x => x.DateCreated))
            {
                if (policyHistory.EffectiveDate.Value > effectiveDate) continue; // skip all currently in effect endorsements
                if (int.Parse(policyHistory.EndorsementNumber) < 1) continue; // skips NEW transaction type
                if (policyHistory.Details is null) continue; // skips history with empty details i.e. NEW and REINSTATED

                string[] policyChanges = JsonConvert.DeserializeObject<string[]>(policyHistory.Details);

                foreach (var detail in policyChanges)
                {
                    if (detail.StartsWith("Changed General Liability - Bodily Injury") && detail.EndsWith("To: 'NONE'"))
                    {
                        isGLPremiumExpired = true;
                    }
                }

                if (isGLPremiumExpired) break;
            }

            return isGLPremiumExpired;
        }

        private async Task<IEnumerable<EndorsementPendingChange>> GetVehiclePremiumChanges(RiskDetail previousRiskDetail, RiskDetail riskDetail)
        {
            var risk = await _riskRepository.GetRiskByIdAsync(riskDetail.RiskId);
            var previousVehiclePremiums = await _riskVehiclePremiumTransactionRepository.GetAsync(previousRiskDetail.Id);
            var vehiclePremiums = await _riskVehiclePremiumTransactionRepository.GetAsync(riskDetail.Id);

            var vehicleEndorsementChange = new List<EndorsementPendingChange>();

            var autoLiabilityEndorsementChanges = new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehicleALP,
                PremiumAfterEndorsement = vehiclePremiums.Sum(x => x.CurrentALPremium.RoundTo2Decimals()),
                PremiumBeforeEndorsement = previousVehiclePremiums.Sum(x => x.CurrentALPremium.RoundTo2Decimals()),
                Description= "Auto Liability Premium"

            };
            autoLiabilityEndorsementChanges.PremiumDifference = autoLiabilityEndorsementChanges.PremiumAfterEndorsement - autoLiabilityEndorsementChanges.PremiumBeforeEndorsement;

            vehicleEndorsementChange.Add(autoLiabilityEndorsementChanges);

            var physicalDamageEndorsementChanges = new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehiclePDP,
                PremiumAfterEndorsement = vehiclePremiums.Sum(x => x.CurrentPDPremium.RoundTo2Decimals()),
                PremiumBeforeEndorsement = previousVehiclePremiums.Sum(x => x.CurrentPDPremium.RoundTo2Decimals()),
                Description = "Physical Damage Premium"
            };
            physicalDamageEndorsementChanges.PremiumDifference = physicalDamageEndorsementChanges.PremiumAfterEndorsement - physicalDamageEndorsementChanges.PremiumBeforeEndorsement;
            vehicleEndorsementChange.Add(physicalDamageEndorsementChanges);

            var cargoEndorsementChanges = new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehicleCP,
                PremiumAfterEndorsement = vehiclePremiums.Sum(x => x.CurrentCargoPremium.RoundTo2Decimals()),
                PremiumBeforeEndorsement = previousVehiclePremiums.Sum(x => x.CurrentCargoPremium.RoundTo2Decimals()),
                Description = "Cargo Premium"
            };
            cargoEndorsementChanges.PremiumDifference = cargoEndorsementChanges.PremiumAfterEndorsement - cargoEndorsementChanges.PremiumBeforeEndorsement;
            vehicleEndorsementChange.Add(cargoEndorsementChanges);

            return vehicleEndorsementChange;
        }

        public async Task<string> GeneratePolicyNumber(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.GetInsuredAddressAsync(riskDetailId);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var state = riskDetail.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == (short)AddressTypesEnum.Business).Address.State;
            if (risk.PolicyNumber == null)
            {
                if (risk.IsRenewal == false || 
                    (risk.IsRenewal && _riskSequence.validatePolicyNumber(risk.PreviousPolicyNumber) == false))
                {
                    risk.PolicyNumber = _riskSequence.GeneratePolicyNumber(state, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value);
                    if (risk.IsRenewal) risk.IsFromSN = true;
                }
                else
                {
                    var updatedPolicyNumber = _riskSequence.UpdatePolicyNumber(
                        risk.PreviousPolicyNumber, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value, state);
                    risk.PolicyNumber = _riskSequence.GenerateRenewalPolicyNumber(updatedPolicyNumber, state,
                                    brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value);
                }

                await _riskRepository.SaveChangesAsync();
            }

            return risk.PolicyNumber;
        }

        public async Task<bool> ProcessEndorsement(Guid riskId)
        {
            try
            {
                await _statusLogService.DeleteAsync(riskId);
                await _statusLogService.LogAsync(riskId, "Initiated endorsement processing.");

                await _statusLogService.LogAsync(riskId, "Retrieving risk data for processing.");
                var risk = await _riskRepository.GetRiskByIdAsync(riskId);
                RiskDetail forEndorsementRiskDetail = await _riskDetailRepository.FindAsync(risk.ForEndorsementRiskDetailId.Value);
                Binding binding = await _bindingRepository.FindAsync(x => x.RiskId == risk.Id);
                bool hasPendingEndorsement = forEndorsementRiskDetail.Status == RiskDetailStatus.PendingEndorsement;
                DateTime endorsementEffectiveDate = forEndorsementRiskDetail.EndorsementEffectiveDate.Value;

                if (hasPendingEndorsement)
                {
                    await _statusLogService.LogAsync(riskId, $"Endorsement is effective on {endorsementEffectiveDate:MM/dd/yyyy}.");

                    await _statusLogService.LogAsync(riskId, "Retrieving past endorsements.");
                    var riskEndorsementPoints = await _endorsementDataRepository.GetRiskEndorsementPoints(riskId);

                    await _statusLogService.LogAsync(riskId, "Gathering changes.");
                    await _changeComparerContext.CompareChanges(riskId);
                    var endorsementChanges = await _endorsementChangesService.GetEndorsementChangesByRiskId(riskId, risk.NextEndorsementNumber());
                    var changeCollection = _endorsementChangeDeserializer.Deserialize(endorsementChanges);

                    await _endorsementProcessor.Process(riskId, endorsementEffectiveDate, changeCollection, riskEndorsementPoints);
                }
                else
                {
                    if (risk.IsFromReset)
                    {
                        await UpdateEndorsementEffectiveDatePromptedAsync(riskId, false);
                        await _statusLogService.LogAsync(riskId, "No pending changes.");
                        await _statusLogService.LogAsync(riskId, "Resetting premiums.");
                        await _premiumService.ResetPremiumsAsync(risk);
                        await _taxService.ResetTaxDetailsAsync(forEndorsementRiskDetail.Id);
                        await _feeService.ResetServiceFeesAsync(riskId);
                        await _taxService.CalculateRiskTaxAsync(forEndorsementRiskDetail.Id, isEndorsement: true); // Added Mac
                        await _riskPremiumTransactionService.CalculateAsync(forEndorsementRiskDetail.Id, risk.CurrentRiskDetailId.Value);

                        var saveRisk = await _riskRepository.FindAsync(riskId);
                        saveRisk.IsFromReset = false;
                        await _riskRepository.SaveChangesAsync();
                    }
                }

                await _statusLogService.LogAsync(riskId, "Generating premium changes.");
                await CalculatePremiumChanges(risk.ForEndorsementRiskDetailId.Value, endorsementEffectiveDate, binding.BindOptionId ?? 1);
                await _taxService.CalculateRiskTaxAsync(forEndorsementRiskDetail.Id, isEndorsement: true);

                await _statusLogService.LogAsync(riskId, "Finished endorsement processing.");

                return true;
            }
            catch (RaterException e)
            {
                foreach (var err in e.Errors)
                {
                    await _statusLogService.LogAsync(riskId, $"ERROR: {err}");
                }
                throw;
            }
            catch (OutOfSequenceChangeConflictException e)
            {
                foreach (var error in e.Errors)
                {
                    await _statusLogService.LogAsync(riskId, $"ERROR: {error}");
                }
                throw;
            }
            catch (Exception e)
            {
                await _statusLogService.LogAsync(riskId, $"{e.Message}");
                await _statusLogService.LogAsync(riskId, "An error occured while processing endorsement changes.");
                _logger.LogError(e, "Error processing endorsement.");
                throw;
            }
        }

        public async Task<bool> ProcessEndorsement(Guid riskId, NotificationFilterDTO notificationFilterDTO)
        {
            try
            {
                //await _statusLogService.DeleteAsync(riskId);
                //await _statusLogService.LogAsync(riskId, "Initiated endorsement processing.");

                notificationFilterDTO.CompletionPercentage = 10;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                //await _statusLogService.LogAsync(riskId, "Retrieving risk data for processing.");
                var risk = await _riskRepository.GetRiskByIdAsync(riskId);
                RiskDetail forEndorsementRiskDetail = await _riskDetailRepository.FindAsync(risk.ForEndorsementRiskDetailId.Value);
                Binding binding = await _bindingRepository.FindAsync(x => x.RiskId == risk.Id);
                bool hasPendingEndorsement = forEndorsementRiskDetail.Status == RiskDetailStatus.PendingEndorsement;
                DateTime endorsementEffectiveDate = forEndorsementRiskDetail.EndorsementEffectiveDate.Value;

                if (hasPendingEndorsement)
                {
                    //await _statusLogService.LogAsync(riskId, $"Endorsement is effective on {endorsementEffectiveDate:MM/dd/yyyy}.");

                    //await _statusLogService.LogAsync(riskId, "Retrieving past endorsements.");
                    var riskEndorsementPoints = await _endorsementDataRepository.GetRiskEndorsementPoints(riskId);

                    //await _statusLogService.LogAsync(riskId, "Gathering changes.");
                    await _changeComparerContext.CompareChanges(riskId);
                    var endorsementChanges = await _endorsementChangesService.GetEndorsementChangesByRiskId(riskId, risk.NextEndorsementNumber());
                    var changeCollection = _endorsementChangeDeserializer.Deserialize(endorsementChanges);


                    notificationFilterDTO.CompletionPercentage = 20;

                    await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                    await _endorsementProcessor.Process(riskId, endorsementEffectiveDate, changeCollection, riskEndorsementPoints, notificationFilterDTO);
                }

                //await _statusLogService.LogAsync(riskId, "Generating premium changes.");
                await CalculatePremiumChanges(risk.ForEndorsementRiskDetailId.Value, endorsementEffectiveDate, binding.BindOptionId ?? 1);
                await _taxService.CalculateRiskTaxAsync(forEndorsementRiskDetail.Id, isEndorsement: true);

                //await _statusLogService.LogAsync(riskId, "Finished endorsement processing.");

                notificationFilterDTO.CompletionPercentage = 100;
                notificationFilterDTO.Title = "Premium calculation was completed successfully";
                notificationFilterDTO.NotificationStatus = "Success";
                notificationFilterDTO.DateUpdated = DateTime.Now;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                return true;
            }
            catch (RaterException e)
            {
                foreach (var err in e.Errors)
                {
                    await _statusLogService.LogAsync(riskId, $"ERROR: {err}");
                }
                throw;
            }
            catch (OutOfSequenceChangeConflictException e)
            {
                foreach (var error in e.Errors)
                {
                    await _statusLogService.LogAsync(riskId, $"ERROR: {error}");
                }
                throw;
            }
            catch (Exception e)
            {
                await _statusLogService.LogAsync(riskId, $"{e.Message}");
                await _statusLogService.LogAsync(riskId, "An error occured while processing endorsement changes.");
                _logger.LogError(e, "Error processing endorsement.");
                throw;
            }
        }

        public async Task<bool> ProcessEndorsementDefault(Guid riskId, NotificationFilterDTO notificationFilterDTO)
        {
            try
            {
                notificationFilterDTO.CompletionPercentage = 10;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                var risk = await _riskRepository.GetRiskByIdAsync(riskId);
                RiskDetail forEndorsementRiskDetail = await _riskDetailRepository.FindAsync(risk.ForEndorsementRiskDetailId.Value);
                Binding binding = await _bindingRepository.FindAsync(x => x.RiskId == risk.Id);
                bool hasPendingEndorsement = forEndorsementRiskDetail.Status == RiskDetailStatus.PendingEndorsement;
                DateTime endorsementEffectiveDate = forEndorsementRiskDetail.EndorsementEffectiveDate.Value;

                notificationFilterDTO.CompletionPercentage = 40;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                if (risk.IsFromReset)
                {
                    await UpdateEndorsementEffectiveDatePromptedAsync(riskId, false);
                    await _statusLogService.LogAsync(riskId, "No pending changes.");
                    await _statusLogService.LogAsync(riskId, "Resetting premiums.");
                    await _premiumService.ResetPremiumsAsync(risk);
                    await _taxService.ResetTaxDetailsAsync(forEndorsementRiskDetail.Id);
                    await _feeService.ResetServiceFeesAsync(riskId);
                    await _taxService.CalculateRiskTaxAsync(forEndorsementRiskDetail.Id, isEndorsement: true); // Added Mac
                    await _riskPremiumTransactionService.CalculateAsync(forEndorsementRiskDetail.Id, risk.CurrentRiskDetailId.Value);

                    var saveRisk = await _riskRepository.FindAsync(riskId);
                    saveRisk.IsFromReset = false;
                    await _riskRepository.SaveChangesAsync();
                }

                notificationFilterDTO.CompletionPercentage = 60;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                await _statusLogService.LogAsync(riskId, "Generating premium changes.");
                await CalculatePremiumChanges(risk.ForEndorsementRiskDetailId.Value, endorsementEffectiveDate, binding.BindOptionId ?? 1);
                await _taxService.CalculateRiskTaxAsync(forEndorsementRiskDetail.Id, isEndorsement: true);

                notificationFilterDTO.CompletionPercentage = 100;
                notificationFilterDTO.Title = "Premium calculation was completed successfully";
                notificationFilterDTO.NotificationStatus = "Success";
                notificationFilterDTO.DateUpdated = DateTime.Now;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                return true;
            }
            catch (RaterException e)
            {
                foreach (var err in e.Errors)
                {
                    await _statusLogService.LogAsync(riskId, $"ERROR: {err}");
                }
                throw;
            }
            catch (OutOfSequenceChangeConflictException e)
            {
                foreach (var error in e.Errors)
                {
                    await _statusLogService.LogAsync(riskId, $"ERROR: {error}");
                }
                throw;
            }
            catch (Exception e)
            {
                await _statusLogService.LogAsync(riskId, $"{e.Message}");
                await _statusLogService.LogAsync(riskId, "An error occured while processing endorsement changes.");
                _logger.LogError(e, "Error processing endorsement.");
                throw;
            }
        }

        private async Task UpdateEndorsementEffectiveDatePromptedAsync(Guid riskId, bool prompted)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            risk.EndorsementEffectiveDatePrompted = prompted;
            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();
        }

        public async Task GetEndorsementChanges(Guid riskId)
        {
            await _changeComparerContext.CompareChanges(riskId);
        }

        public async Task<bool> HasEffectiveDatePromptedAsync(Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            bool prompted = risk.EndorsementEffectiveDatePrompted;
            return prompted;
        }

        public async Task UpdateEffectiveDatePromptedAsync(Guid riskId, bool prompted)
        {
            prompted = true; // TODO: remove if BA approved.
            var risk = await _riskRepository.FindAsync(riskId);
            risk.EndorsementEffectiveDatePrompted = prompted;
            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();
        }

        private bool IsIncludedInOption(string options, int option)
        {
            return !string.IsNullOrEmpty(options) && options.Contains(option.ToString());
        }
    }
}
