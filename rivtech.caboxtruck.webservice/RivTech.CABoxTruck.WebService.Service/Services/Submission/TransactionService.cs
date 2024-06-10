using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NPOI.SS.Formula.Functions;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Enums;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.Emails;
using RivTech.CABoxTruck.WebService.Service.IServices.Policy;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission
{
    public class TransactionService : ITransactionService
    {
        private readonly IErrorLogRepository _errorLogRepository;
        private readonly IMapper _mapper;
        private readonly IRiskFactory _riskFactory;
        private readonly IPacketReportService _packetReportService;
        private readonly IQuoteReportService _quoteReportService;
        private readonly IInvoiceReportService _invoiceReportService;
        private readonly IBillingService _billingService;
        private readonly IBindingRepository _bindingRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskSequenceService _riskSequenceService;
        private readonly IPolicyHistoryService _policyHistoryService;
        private readonly IEndorsementPendingChangeRepository _endorsementPendingChangeRepository;
        private readonly IEndorsementService _endorsementService;
        private readonly ICancellationService _cancellationService;
        private readonly IReinstatementService _reinstatementService;
        private readonly ICancellationBreakdownRepository _cancellationBreakdownRepository;
        private readonly IFormsService _formsService;
        private readonly IRaterService _raterService;
        private readonly IRiskService _riskService;
        private readonly IFileUploadDocumentService _fileUploadDocumentService;
        private readonly IVehicleService _vehicleService;
        private readonly ILog4NetService _errorLogger;
        private readonly IEmailQueueService _emailQueueService;
        private readonly IInvoiceEmailService _invoiceEmailService;
        private readonly IRiskPremiumTransactionService _riskPremiumTransactionService;
        private readonly IEndorsementDataCleaner _endorsementDataCleaner;
        private readonly IPremiumService _premiumService;
        private readonly IDriverService _driverService;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IApplogService _applogService;
        private readonly ISubmissionService _submissionService;
        private readonly IEndorsementProcessStatusLogService _statusLogService;
        private readonly IGenericsRepository _genericsRepository;
        private readonly INotificationService _notificationService;

        public TransactionService(
            IErrorLogRepository errorLogRepository,
            IMapper mapper,
            IRiskFactory riskFactory,
            IPacketReportService packetReportService,
            IQuoteReportService quoteReportService,
            IInvoiceReportService invoiceReportService,
            IBillingService billingService,
            IBindingRepository bindingRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskSequenceService riskSequenceService,
            IRiskRepository riskRepository,
            IPolicyHistoryService policyHistoryService,
            IEndorsementPendingChangeRepository endorsementPendingChangeRepository,
            IEndorsementService endorsementService,
            ICancellationService cancellationService,
            ICancellationBreakdownRepository cancellationBreakdownRepository,
            IFormsService formsService,
            IRaterService raterService,
            IRiskService riskService,
            IFileUploadDocumentService fileUploadDocumentService,
            IVehicleService vehicleService,
            ILog4NetService errorLogger,
            IReinstatementService reinstatementService,
            IInvoiceEmailService invoiceEmailService,
            IRiskPremiumTransactionService riskPremiumTransactionService,
            IEndorsementDataCleaner endorsementDataCleaner,
            IPremiumService premiumService,
            IDriverService driverService,
            IRiskCoverageRepository riskCoverageRepository,
            IApplogService applogService,
            IEndorsementProcessStatusLogService statusLogService,
            ISubmissionService submissionService,
            IGenericsRepository genericsRepository,
            INotificationService notificationService
        )
        {
            _errorLogRepository = errorLogRepository;
            _mapper = mapper;
            _riskFactory = riskFactory;
            _packetReportService = packetReportService;
            _quoteReportService = quoteReportService;
            _invoiceReportService = invoiceReportService;
            _billingService = billingService;
            _bindingRepository = bindingRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskSequenceService = riskSequenceService;
            _policyHistoryService = policyHistoryService;
            _endorsementPendingChangeRepository = endorsementPendingChangeRepository;
            _endorsementService = endorsementService;
            _cancellationService = cancellationService;
            _cancellationBreakdownRepository = cancellationBreakdownRepository;
            _reinstatementService = reinstatementService;
            _formsService = formsService;
            _raterService = raterService;
            _cancellationBreakdownRepository = cancellationBreakdownRepository;
            _riskService = riskService;
            _fileUploadDocumentService = fileUploadDocumentService;
            _vehicleService = vehicleService;
            _errorLogger = errorLogger;
            _invoiceEmailService = invoiceEmailService;
            _riskPremiumTransactionService = riskPremiumTransactionService;
            _endorsementDataCleaner = endorsementDataCleaner;
            _premiumService = premiumService;
            _driverService = driverService;
            _riskCoverageRepository = riskCoverageRepository;
            _applogService = applogService;
            _submissionService = submissionService;
            _statusLogService = statusLogService;
            _genericsRepository = genericsRepository;
            _notificationService = notificationService;
        }

        public async Task<RiskDTO> IssueSubmission(Guid riskDetailId)
        {
            // billing api bind process
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var vehicles = riskDetail.RiskDetailVehicles.Select(rdv => rdv.Vehicle).Where(x => x.Options != null && x.Options.Contains(binding.BindOptionId.Value.ToString()) && x.IsActive != false).ToList();
            var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            if (riskCoverage.RiskCoveragePremium.Premium < 1)
            {
                throw new Exception("Estimated Premium is invalid.");
            }

            var optionId = (binding.BindOptionId ?? 1).ToString();
            //remove vehicles if bind option is null
            var vehiclesToRemove = riskDetail.RiskDetailVehicles.Select(rdv => rdv.Vehicle).Where(d => d.IsActive != false && (d.Options == null || !d.Options.Contains(optionId))).ToList();

            foreach (var vehicle in vehiclesToRemove)
            {
                await _vehicleService.RemoveAsync(vehicle.Id);
            }
            riskDetail.RiskDetailVehicles = riskDetail.RiskDetailVehicles.Where(rdv => !vehiclesToRemove.Contains(rdv.Vehicle)).ToList();
            if (binding.BindOptionId != 1)
            {
                foreach (var vehicle in riskDetail.RiskDetailVehicles.Select(rdv => rdv.Vehicle))
                {
                    vehicle.UpdateCoveragesFromOption(riskCoverage);
                }
            }

            //remove drivers if bind option is null
            var driversToRemove = riskDetail.RiskDetailDrivers.Select(rdd => rdd.Driver).Where(d => d.IsActive != false && (d.Options == null || !d.Options.Contains(optionId))).ToList();
            foreach (var driver in driversToRemove)
            {
                await _driverService.RemoveAsync(driver.Id);
            }
            riskDetail.RiskDetailDrivers = riskDetail.RiskDetailDrivers.Where(rdd => !driversToRemove.Contains(rdd.Driver)).ToList();

            await _vehicleService.UpdatePremiums(riskDetailId, binding.BindOptionId.Value);
            await _riskPremiumTransactionService.CalculateAsync(riskDetailId);

            // Generate Policy Number if Issued without one
            var state = riskDetail.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == (short)AddressTypesEnum.Business).Address.State;
            if (risk.PolicyNumber == null)
            {
                if (risk.IsRenewal == false ||
                    (risk.IsRenewal && _riskSequenceService.validatePolicyNumber(risk.PreviousPolicyNumber) == false))
                {
                    risk.PolicyNumber = _riskSequenceService.GeneratePolicyNumber(state, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value);
                    if (risk.IsRenewal) risk.IsFromSN = true;
                }
                else
                {
                    var updatedPolicyNumber = _riskSequenceService.UpdatePolicyNumber(
                        risk.PreviousPolicyNumber, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value, state);
                    risk.PolicyNumber = _riskSequenceService.GenerateRenewalPolicyNumber(updatedPolicyNumber, state,
                                    brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value);
                }

                await _riskService.UpdatePolicyNumberAsync(risk.Id, risk.PolicyNumber);
            }

            // Billing Bind
            var billingBind = _billingService.ProcessBillingBind(risk.PolicyNumber, brokerInfo.EffectiveDate.Value, riskDetail, binding, riskCoverage, vehicles);

            // record data generation
            await _riskService.ProcessRiskDetailsForIssuance(risk.Id, riskDetail, brokerInfo);

            List<DriverDTO> excludedDrivers = new List<DriverDTO>();
            if (driversToRemove.Count() > 0)
            {
                excludedDrivers = driversToRemove.Select(x => new DriverDTO()
                {
                    LicenseNumber = x.LicenseNumber,
                    FirstName = x.Entity.FirstName,
                    LastName = x.Entity.LastName
                }).ToList();
            }
            var policyForEndorsement = await _riskRepository.FindAsync(risk.Id);
            await _formsService.SaveFormsContentAsync(riskDetail, policyForEndorsement.ForEndorsementRiskDetailId.Value, initialExcludedDrivers: excludedDrivers);

            var policyDetail = await _riskDetailRepository.FindAsync(policyForEndorsement.ForEndorsementRiskDetailId.Value);

            await _riskPremiumTransactionService.CalculateAsync(policyForEndorsement.ForEndorsementRiskDetailId.Value);

            // create policy History
            await _policyHistoryService.CreatePolicyHistory(risk.Id, policyDetail, riskDetailId, riskCoverage.RiskCoveragePremium, brokerInfo.EffectiveDate.Value, policyDetail.Status);

            // policy packet data generation
            var endorsementPlaceholder = new EndorsementDetailsDTO { Risk = _mapper.Map<RiskDTO>(policyDetail) };
            await _packetReportService.GenerateSavePolicyPacketReport(riskDetailId, endorsementPlaceholder);

            //Quote Proposal Save
            await _quoteReportService.GenerateSaveQuoteReport(riskDetailId); // Generate quote proposal using quoted RiskDetailId

            //Invoice Save
            var reportResponse = await _invoiceReportService.GenerateInvoiceReport<string>(_mapper.Map<InvoiceDTO>(billingBind), returnUrl: true, fromIssuance: true);
            //await _invoiceEmailService.SendEmailInvoiceToBrokerAgent(riskDetail.RiskId, reportResponse.Output);
            return _mapper.Map<RiskDTO>(policyDetail);
        }

        public async Task<RiskDTO> IssueEndorsement(Guid riskDetailId, DateTime effectiveDate, string endorsementText, string policyChanges)
        {
            var riskDetailToIssue = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetailToIssue.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetailToIssue.RiskId);

            // get premium changes for endorsement documents
            var endorsementPremiumChange = await _endorsementService.GetPremiumChanges(riskDetailToIssue.Id);
            var aiChanges = await _riskFactory.GetAdditionalInterestChanges(riskDetailId, riskDetailToIssue.RiskId);

            await _statusLogService.DeleteAsync(riskDetailToIssue.RiskId);
            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Processing endorsement for issuance.");
            // record data generation
            await _riskService.ProcessRiskDetailsForIssuance(risk.Id, riskDetailToIssue, brokerInfo);

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Processing premium transactions.");
            var policy = await _riskRepository.FindAsync(risk.Id);
            var policyDetailForEndorsement = await _riskDetailRepository.GetOneIncludeByIdAsync(policy.ForEndorsementRiskDetailId.Value);
            await _riskPremiumTransactionService.CalculateAsync(risk.ForEndorsementRiskDetailId.Value, risk.CurrentRiskDetailId.Value);

            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetailToIssue.RiskId);
            var pendingChanges = _endorsementPendingChangeRepository.GetAsync(x => x.RiskDetailId == riskDetailId).Result.ToList();

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Processing billing transactions.");
            // Endorsement Billing Bind
            var billingBind = await _billingService.ProcessBillingBindEndorsement(riskDetailToIssue.RiskId, effectiveDate, pendingChanges, brokerInfo.EffectiveDate);

            var hasBillingTransaction = billingBind != null ? true: false;

            var newCurrentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(policy.CurrentRiskDetailId.Value);
            var riskCoverage = newCurrentRiskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Creating policy history.");
            // create policy History
            var endorsementNumber = await _policyHistoryService.CreatePolicyHistory(policy.Id, policyDetailForEndorsement, policy.CurrentRiskDetailId.Value, riskCoverage.RiskCoveragePremium, effectiveDate, policyDetailForEndorsement.Status, policyChanges);

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Saving policy forms.");
            await _formsService.SaveFormsContentAsync(newCurrentRiskDetail, policy.ForEndorsementRiskDetailId.Value, effectiveDate);

            //Invoice Save
            if (billingBind != null)
            {
                try
                {
                    await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Generating Invoice.");
                    var invoice = _mapper.Map<InvoiceDTO>(billingBind);
                    invoice.InvoiceType = InvoiceType.Endorsement;
                    invoice.RiskDetailId = policy.CurrentRiskDetailId.Value;
                    invoice.PolicyChanges = policyChanges;
                    await _invoiceReportService.GenerateInvoiceReport<string>(invoice, returnUrl: true, fromIssuance: true);
                }
                catch { }
            }

            // endorsement document
            var endorsementDetail = new EndorsementDetailsDTO
            {
                EndorsementNumber = endorsementNumber,
                Description = endorsementText,
                PolicyChanges = policyChanges,
                EffectiveDate = effectiveDate,
                PremiumChange = endorsementPremiumChange,
                AdditionalInterestChanges = aiChanges,
                Risk = _mapper.Map<RiskDTO>(newCurrentRiskDetail)
            };

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Generating policy packet.");
            // policy packet data generation
            var fileName = $"Policy Packet_{risk.PolicyNumber}_{DateTime.Now:MMddyyyy_HHmmss}";
            await _packetReportService.GenerateSavePolicyPacketReport(policy.CurrentRiskDetailId.Value, endorsementDetail, fileName);

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Generating endorsement documents.");
            var endorsementForms = GetEndorsementForms(endorsementNumber, risk.PolicyNumber, policyChanges, endorsementPremiumChange, policy.CurrentRiskDetailId.Value, aiChanges);
            await _packetReportService.GenerateEndorsementDocuments(endorsementForms, _mapper.Map<InvoiceDTO>(billingBind), endorsementDetail, true, true);

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Endorsement issue completed.");
            return _mapper.Map<RiskDTO>(policyDetailForEndorsement);
        }

        public async Task<RiskDTO> IssueEndorsement(Guid riskDetailId, DateTime effectiveDate, string endorsementText, string policyChanges, NotificationFilterDTO notificationFilterDTO)
        {
            notificationFilterDTO.CompletionPercentage = 10;
            notificationFilterDTO.OldRiskDetailId = riskDetailId;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            var riskDetailToIssue = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetailToIssue.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetailToIssue.RiskId);

            // get premium changes for endorsement documents
            var endorsementPremiumChange = await _endorsementService.GetPremiumChanges(riskDetailToIssue.Id);
            var aiChanges = await _riskFactory.GetAdditionalInterestChanges(riskDetailId, riskDetailToIssue.RiskId);

            await _statusLogService.DeleteAsync(riskDetailToIssue.RiskId);
            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Processing endorsement for issuance.");
            // record data generation
            await _riskService.ProcessRiskDetailsForIssuance(risk.Id, riskDetailToIssue, brokerInfo);

            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Processing premium transactions.");
            var policy = await _riskRepository.FindAsync(risk.Id);
            var policyDetailForEndorsement = await _riskDetailRepository.GetOneIncludeByIdAsync(policy.ForEndorsementRiskDetailId.Value);
            await _riskPremiumTransactionService.CalculateAsync(risk.ForEndorsementRiskDetailId.Value, risk.CurrentRiskDetailId.Value);

            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetailToIssue.RiskId);
            var pendingChanges = _endorsementPendingChangeRepository.GetAsync(x => x.RiskDetailId == riskDetailId).Result.ToList();

            notificationFilterDTO.CompletionPercentage = 50;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Processing billing transactions.");
            // Endorsement Billing Bind
            var billingBind = await _billingService.ProcessBillingBindEndorsement(riskDetailToIssue.RiskId, effectiveDate, pendingChanges, brokerInfo.EffectiveDate);

            var hasBillingTransaction = billingBind != null ? true : false;

            var newCurrentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(policy.CurrentRiskDetailId.Value);
            var riskCoverage = newCurrentRiskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Creating policy history.");
            // create policy History
            var endorsementNumber = await _policyHistoryService.CreatePolicyHistory(policy.Id, policyDetailForEndorsement, policy.CurrentRiskDetailId.Value, riskCoverage.RiskCoveragePremium, effectiveDate, policyDetailForEndorsement.Status, policyChanges);

            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Saving policy forms.");
            await _formsService.SaveFormsContentAsync(newCurrentRiskDetail, policy.ForEndorsementRiskDetailId.Value, effectiveDate);

            //Invoice Save
            if (billingBind != null)
            {
                try
                {
                    await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Generating Invoice.");
                    var invoice = _mapper.Map<InvoiceDTO>(billingBind);
                    invoice.InvoiceType = InvoiceType.Endorsement;
                    invoice.RiskDetailId = policy.CurrentRiskDetailId.Value;
                    invoice.PolicyChanges = policyChanges;
                    await _invoiceReportService.GenerateInvoiceReport<string>(invoice, returnUrl: true, fromIssuance: true);
                }
                catch { }
            }

            // endorsement document
            var endorsementDetail = new EndorsementDetailsDTO
            {
                EndorsementNumber = endorsementNumber,
                Description = endorsementText,
                PolicyChanges = policyChanges,
                EffectiveDate = effectiveDate,
                PremiumChange = endorsementPremiumChange,
                AdditionalInterestChanges = aiChanges,
                Risk = _mapper.Map<RiskDTO>(newCurrentRiskDetail)
            };


            notificationFilterDTO.CompletionPercentage = 90;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Generating policy packet.");
            // policy packet data generation
            var fileName = $"Policy Packet_{risk.PolicyNumber}_{DateTime.Now:MMddyyyy_HHmmss}";
            await _packetReportService.GenerateSavePolicyPacketReport(policy.CurrentRiskDetailId.Value, endorsementDetail, fileName);

            await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Generating endorsement documents.");
            var endorsementForms = GetEndorsementForms(endorsementNumber, risk.PolicyNumber, policyChanges, endorsementPremiumChange, policy.CurrentRiskDetailId.Value, aiChanges);
            await _packetReportService.GenerateEndorsementDocuments(endorsementForms, _mapper.Map<InvoiceDTO>(billingBind), endorsementDetail, true, true);

            //await _statusLogService.LogAsync(riskDetailToIssue.RiskId, "Endorsement issue completed.");

            notificationFilterDTO.CompletionPercentage = 100;
            notificationFilterDTO.Title = "Endorsement issuance completed successfully";
            notificationFilterDTO.NotificationStatus = "Success";
            notificationFilterDTO.NewRiskDetailId = policy.ForEndorsementRiskDetailId.Value;
            notificationFilterDTO.DateUpdated = DateTime.Now;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            return _mapper.Map<RiskDTO>(policyDetailForEndorsement);
        }

        public async Task<RiskDTO> ResetPolicyChanges(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            risk.IsFromReset = true;
            await _riskRepository.SaveChangesAsync();

            await _statusLogService.DeleteAsync(riskDetail.RiskId);
            await _statusLogService.LogAsync(riskDetail.RiskId, "Resetting Policy.");
            
            if (riskDetail is null) throw new ArgumentNullException(nameof(Data.Entity.RiskDetail));

            await _statusLogService.LogAsync(riskDetail.RiskId, "Processing policy cleanup.");
            await _endorsementDataCleaner.CleanUpAsync(risk.Id);
            
            await _statusLogService.LogAsync(riskDetail.RiskId, "Restoring policy details.");
            var forEndorseRiskDetail = await _riskFactory.GetPreviousPolicyDetail(risk.ForEndorsementRiskDetailId.Value, riskDetail.RiskId);
            forEndorseRiskDetail = await _riskDetailRepository.FindAsync(forEndorseRiskDetail.Id);
            forEndorseRiskDetail.IsProcessed = false;
            forEndorseRiskDetail.EndorsementNumber = risk.CurrentEndorsementNumber + 1;
            forEndorseRiskDetail.SourceRiskDetailId = risk.CurrentRiskDetailId.Value;
            forEndorseRiskDetail.VersionNumber = await _riskDetailRepository.GetNextVersionNumberOfEndorsementNumAsync(risk.Id, forEndorseRiskDetail.EndorsementNumber);
            await _riskDetailRepository.SaveChangesAsync();
            
            await _statusLogService.LogAsync(riskDetail.RiskId, "Restoring policy history.");
            var policyHistory = await _policyHistoryService.GetLastActiveByRiskId(forEndorseRiskDetail.RiskId);
            var nextEndorsement = Convert.ToInt32(policyHistory.EndorsementNumber) + 1;
            
            await _statusLogService.LogAsync(riskDetail.RiskId, "Restoring rater.");
            await _raterService.DeleteArchive(forEndorseRiskDetail.SubmissionNumber, nextEndorsement);

            return _mapper.Map<RiskDTO>(forEndorseRiskDetail);
        }
        public async Task<RiskDTO> ResetPolicyChanges(Guid riskDetailId, NotificationFilterDTO notificationFilterDTO)
        {
            notificationFilterDTO.CompletionPercentage = 10;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            risk.IsFromReset = true;
            await _riskRepository.SaveChangesAsync();

            await _statusLogService.DeleteAsync(riskDetail.RiskId);

            notificationFilterDTO.CompletionPercentage = 30;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            if (riskDetail is null) throw new ArgumentNullException(nameof(Data.Entity.RiskDetail));

            await _endorsementDataCleaner.CleanUpAsync(risk.Id);

            notificationFilterDTO.CompletionPercentage = 50;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);
            var forEndorseRiskDetail = await _riskFactory.GetPreviousPolicyDetail(risk.ForEndorsementRiskDetailId.Value, riskDetail.RiskId);
            forEndorseRiskDetail = await _riskDetailRepository.FindAsync(forEndorseRiskDetail.Id);
            forEndorseRiskDetail.IsProcessed = false;
            forEndorseRiskDetail.EndorsementNumber = risk.CurrentEndorsementNumber + 1;
            forEndorseRiskDetail.SourceRiskDetailId = risk.CurrentRiskDetailId.Value;
            forEndorseRiskDetail.VersionNumber = await _riskDetailRepository.GetNextVersionNumberOfEndorsementNumAsync(risk.Id, forEndorseRiskDetail.EndorsementNumber);
            await _riskDetailRepository.SaveChangesAsync();


            notificationFilterDTO.CompletionPercentage = 70;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);
            var policyHistory = await _policyHistoryService.GetLastActiveByRiskId(forEndorseRiskDetail.RiskId);
            var nextEndorsement = Convert.ToInt32(policyHistory.EndorsementNumber) + 1;


            notificationFilterDTO.CompletionPercentage = 90;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            await _raterService.DeleteArchive(forEndorseRiskDetail.SubmissionNumber, nextEndorsement); 
            
            notificationFilterDTO.CompletionPercentage = 100;
            notificationFilterDTO.Title = "Policy reset completed successfully";
            notificationFilterDTO.NotificationStatus = "Success";
            notificationFilterDTO.DateUpdated = DateTime.Now;

            await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

            return _mapper.Map<RiskDTO>(forEndorseRiskDetail);
        }

        public async Task<List<string>> GetPolicyChanges(Guid riskDetailId)
        {
            try
            {
                var riskDetail = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);

                return await _riskFactory.GetPolicyChanges(riskDetailId, riskDetail.RiskId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<EndorsementFormDTO> GetEndorsementForms(int endorsementNumber, string policyNumber, string policyChanges,
            EndorsementPremiumChangeSummaryDTO endorsementPremiumChange, Guid? riskDetailId, List<AdditionalInterestChange> aiChanges)
        {
            bool hasPremiumChanges = endorsementPremiumChange.TotalPremiumChangeList.Where(x => x.Description == "Total Premium").Select(x => x.EndorsementPendingChange.PremiumDifference).FirstOrDefault() != 0;
            var decPageTriggers = new[] { "Added Form", "Removed Form" };
            var namedInsuredTriggers = new[] { "Changed Business Name" };
            var lossPayeeTriggers = new[] { "Added Additional Interest 'Loss Payee'" };
            var endorsementForm = new EndorsementFormDTO
            {
                Description = $"Endorsement {endorsementNumber}",
                FileName = $"Endorsement_{endorsementNumber}_{policyNumber}_{DateTime.Now:MMddyyyy_HHmmss}",
                RiskForms = new List<RiskFormDTO>()
            };
            endorsementForm.RiskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = "PotentialChangeEndorsement" } });

            if (namedInsuredTriggers.Any(policyChanges.Contains))
            {
                endorsementForm.RiskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = "NamedInsuredEndorsement" } });
            }
            if (lossPayeeTriggers.Any(policyChanges.Contains))
            {
                var riskForms = _formsService.GetByRiskIdAsync(riskDetailId.Value).Result;
                var lossPayeeOther = riskForms.FirstOrDefault(rf => rf.FormId == "LossPayeeEndorsement")?.Other;
                endorsementForm.RiskForms.AddRange(aiChanges.Select(di => new RiskFormDTO { Id = di.Id, IsSelected = true, Other = lossPayeeOther, Form = new FormDTO { Id = "LossPayeeEndorsement" } }));
            }
            if (decPageTriggers.Any(policyChanges.Contains) || hasPremiumChanges)
            {
                endorsementForm.RiskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = "BusinessAutoPolicyDeclarations" } });
            }

            var endorsementForms = new List<EndorsementFormDTO> { endorsementForm };
            return endorsementForms;
        }

        public async Task<RiskDTO> CancelPolicy(Guid riskDetailId, DateTime effectiveDate, string cancellationReasonType, bool isShortRatePremium, bool? isFlatCancel = null, bool isAutoCancel = false)
        {
            try
            {
                var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);

                if (riskDetail.Status == RiskDetailStatus.Canceled)
                {
                    throw new ArgumentException("Policy is already canceled.");
                }

                var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
                var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
                var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
                var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

                if (cancellationReasonType == Data.Entity.DataSets.CancellationReasons.CancelFlat.Id.ToString())
                {
                    isFlatCancel = true;
                }

                // reset and re-clone riskdetail
                var policyDetailForCancellation = await _riskFactory.GeneratePolicyDetailForCancellation(riskDetailId, riskDetail.RiskId, effectiveDate);

                // pro-rate/short rate
                // Cancel premium/fees.
                // Generate transactions from riskAmounts.
                await _cancellationService.CancelPremiumFees(policyDetailForCancellation.Id, effectiveDate, isShortRatePremium, isFlatCancel ?? false);
                await _riskPremiumTransactionService.CalculateAsync(policyDetailForCancellation.Id, risk.CurrentRiskDetailId);

                await _premiumService.UpdateVehiclePremiumMetricsCancel(policyDetailForCancellation.Id, risk.CurrentRiskDetailId.Value, effectiveDate,
                    binding.BindOptionId ?? 1, true, isFlatCancel ?? false);

                policyDetailForCancellation = await _riskDetailRepository.GetOneIncludeByIdAsync(policyDetailForCancellation.Id); // re-fetch policyDetailForCancellation for any updated data.

                // pass premium changes to billing
                var cancellationBreakdowns = _cancellationBreakdownRepository.GetByRiskDetailId(riskDetailId);
                var billingCancellation = await _billingService.ProcessBillingCancellation(risk.Id, effectiveDate, cancellationBreakdowns, isFlatCancel ?? false);

                // policy history
                var canceledRiskCoverage = policyDetailForCancellation.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();
                var endorsementNumber = await _policyHistoryService.CreatePolicyHistory(risk.Id, riskDetail, riskDetail.Id, canceledRiskCoverage.RiskCoveragePremium, effectiveDate, DTO.Constants.RiskStatus.Canceled, cancellationReasonType);

                // generate cancellation notice/final audit invoice
                await GenerateCancellationDocuments(policyDetailForCancellation, billingCancellation, effectiveDate, cancellationReasonType, isAutoCancel);

                return _mapper.Map<RiskDTO>(policyDetailForCancellation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RiskDTO> NoticeCancelPolicy(Guid riskDetailId, DateTime effectiveDate, string cancellationReasonType)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            var billing = await _billingService.GetBillingSummaryAsync(riskDetail.RiskId);

            // generate cancellation notice/final audit invoice
            await GenerateCancellationDocumentsOnly(riskDetail, effectiveDate, cancellationReasonType, billing.PremiumAmount > 100000);

            return _mapper.Map<RiskDTO>(riskDetail);
        }

        public async Task<RiskDTO> ReinstatePolicy(Guid riskDetailId)
        {
            try
            {
                var canceledRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);

                if (canceledRiskDetail.Status != RiskDetailStatus.Canceled)
                {
                    throw new ArgumentException("Policy is already active.");
                }

                var risk = await _riskRepository.FindAsync(x => x.Id == canceledRiskDetail.RiskId);
                var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == canceledRiskDetail.RiskId);
                var binding = await _bindingRepository.FindAsync(x => x.RiskId == canceledRiskDetail.RiskId);
                var vehicles = canceledRiskDetail.RiskDetailVehicles.Select(rdv => rdv.Vehicle).Where(x => x.Options != null && x.Options.Contains(binding.BindOptionId.Value.ToString()) && x.IsActive != false).ToList();
                var riskCoverage = canceledRiskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

                // record data generation
                await _riskService.ProcessRiskDetailsForReinstatement(risk.Id, canceledRiskDetail, brokerInfo);

                risk = await _riskRepository.FindAsync(risk.Id);
                var reinstatedPolicyDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.ForEndorsementRiskDetailId.Value);
                await _premiumService.UpdateRiskCoverageProratedPremium(reinstatedPolicyDetail.Id);
                await _premiumService.UpdateVehiclePremiumMetrics(riskDetailId, (int)binding.BindOptionId, true);

                // process billing
                await _reinstatementService.ReverseCancellationAmounts(risk.Id, riskDetailId);
                var resinstatementBreakdowns = await _reinstatementService.GetLatestAmountBreakdownsByRisk(risk.Id);

                var latestCancelledPolicyHistory = await _policyHistoryService.GetLastCancelledByRiskId(risk.Id);

                bool isFlatCancel = new[] {
                        "Cancel Flat",
                        "Cancellation Information not found."
                    }.Any(x => latestCancelledPolicyHistory.Details.Contains(x));

                await _billingService.ProcessBillingReinstatement(risk.Id, resinstatementBreakdowns, brokerInfo.EffectiveDate, isFlatCancel);

                // generate policy history
                var reinstatedRiskCoverage = reinstatedPolicyDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();
                await _policyHistoryService.CreatePolicyHistory(risk.Id, reinstatedPolicyDetail, canceledRiskDetail.Id, reinstatedRiskCoverage.RiskCoveragePremium, DateTime.Now, RiskDetailStatus.Reinstated);

                // generate reinstatement notice
                await GenerateReinstatementDocuments(reinstatedPolicyDetail);

                await _riskService.UpdateRiskStatusOnlyAsync(riskDetailId, "Reinstate");

                return _mapper.Map<RiskDTO>(reinstatedPolicyDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RiskDTO> ReinstatePendingCancellationPolicy(Guid riskDetailId, bool isAutoRescind = false)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var vehicles = riskDetail.RiskDetailVehicles.Select(rdv => rdv.Vehicle).Where(x => x.Options != null && x.Options.Contains(binding.BindOptionId.Value.ToString()) && x.IsActive != false).ToList();
            var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            // generate reinstatement notice
            await GenerateReinstatementDocuments(riskDetail, isAutoRescind);

            return _mapper.Map<RiskDTO>(riskDetail);
        }

        private async Task GenerateCancellationDocuments(RiskDetail policyDetail, InvoiceDTO billing, DateTime effectiveDate, string cancellationReasonId, bool isAutoCancel = false)
        {
            billing.InvoiceType = InvoiceType.Cancellation;
            billing.InvoiceNumber = null;
            await _invoiceReportService.GenerateInvoiceReport<string>(_mapper.Map<InvoiceDTO>(billing));

            var cancellationForm = new EndorsementFormDTO
            {
                Description = isAutoCancel ? "Final Cancellation" : "Cancellation Notice",
                FileName = $"Cancellation_Notice_{DateTime.Now:MMddyyyy_HHmmss}",
                RiskForms = new List<RiskFormDTO> { new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = "CancellationNotice" } } }
            };
            var forms = new List<EndorsementFormDTO> { cancellationForm };
            var endorsement = new EndorsementDetailsDTO { Risk = _mapper.Map<RiskDTO>(policyDetail), EffectiveDate = effectiveDate, CancellationReasonId = cancellationReasonId };

            await _packetReportService.GenerateEndorsementDocuments(forms, billing, endorsement);
        }

        private async Task GenerateCancellationDocumentsOnly(RiskDetail policyDetail, DateTime effectiveDate, string cancellationReasonId, bool isPremiumAbove100k = false)
        {
            var cancellationForm = new EndorsementFormDTO
            {
                Description = "Notice of Pending Cancellation",
                FileName = $"Notice_of_Pending_Cancellation_{DateTime.Now:MMddyyyy_HHmmss}",
                RiskForms = new List<RiskFormDTO> { new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = "CancellationNotice" } } }
            };
            var forms = new List<EndorsementFormDTO> { cancellationForm };
            var endorsement = new EndorsementDetailsDTO { Risk = _mapper.Map<RiskDTO>(policyDetail), EffectiveDate = effectiveDate, CancellationReasonId = cancellationReasonId };

            await _packetReportService.GenerateNOPCDocuments(forms, new InvoiceDTO(), endorsement, isPremiumAbove100k);
        }

        private async Task GenerateReinstatementDocuments(RiskDetail policyDetail, bool isAutoRescind = false)
        {
            var reinstatementForm = new EndorsementFormDTO
            {
                Description = "Reinstatement Notice",
                FileName = $"Reinstatement_Notice_{DateTime.Now:MMddyyyy_HHmmss}",
                RiskForms = new List<RiskFormDTO> { new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = "ReinstatementNotice" } } }
            };

            var forms = new List<EndorsementFormDTO> { reinstatementForm };
            var endorsement = new EndorsementDetailsDTO { Risk = _mapper.Map<RiskDTO>(policyDetail) };
            await _packetReportService.GenerateEndorsementDocuments(forms, null, endorsement, isAutoRescind:isAutoRescind);
        }


        public async Task<RiskDTO> RewritePolicyToSubmission(Guid riskDetailId, DateTime effectiveDate, bool? isFlatCancel = true)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            // rewrite: re-clone quoted risk detail
            string newSubmissionNumber = _riskSequenceService.GenerateSubmissionNumber();
            string newQuoteNumber = _riskSequenceService.GenerateQuoteNumber();
            var newSubmissionDetail = await _riskFactory.GenerateSubmissionDetailFromPolicy(riskDetailId, risk.Id, newSubmissionNumber, newQuoteNumber);

            await _raterService.CopyRaterAsync(riskDetail.SubmissionNumber, newSubmissionNumber);

            try
            {
                var result = await _quoteReportService.GenerateQuoteReport(newSubmissionDetail.Id);
                await _riskService.UpdateRiskStatusOnlyAsync(newSubmissionDetail.Id, RiskDetailStatus.Quoted);
                var quoteUploadData = new FileUploadDocumentDTO()
                {
                    RiskDetailId = newSubmissionDetail.Id,
                    tableRefId = newSubmissionDetail.Id,
                    FileName = "Pre-Bind Quote",
                    FilePath = result,
                    IsConfidential = true,
                    CreatedBy = 0,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                await _fileUploadDocumentService.AddUpdateAsync(quoteUploadData);
            }
            catch (Exception e)
            {

            }

            await _premiumService.UpdateRaterPremiumsAsync(newSubmissionDetail.Id, fromPolicy: false, option: 1);
            await _riskPremiumTransactionService.CalculateAsync(newSubmissionDetail.Id);

            return _mapper.Map<RiskDTO>(newSubmissionDetail);
        }

        public async Task<RiskDTO> CopySubmission(Guid riskDetailId, DateTime effectiveDate, bool? isFlatCancel = true)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            // rewrite: re-clone quoted risk detail
            string newSubmissionNumber = _riskSequenceService.GenerateSubmissionNumber();
            string newQuoteNumber = _riskSequenceService.GenerateQuoteNumber();
            var newSubmissionDetail = await _riskFactory.GenerateSubmissionDetail(riskDetailId, risk.Id, newSubmissionNumber, newQuoteNumber);

            if (string.Equals(riskDetail.Status, RiskDetailStatus.Quoted, StringComparison.OrdinalIgnoreCase))
            {
                await _raterService.CopyRaterAsync(riskDetail.SubmissionNumber, newSubmissionNumber);

                try
                {
                    var result = await _quoteReportService.GenerateQuoteReport(newSubmissionDetail.Id);
                    await _riskService.UpdateRiskStatusOnlyAsync(newSubmissionDetail.Id, RiskDetailStatus.Quoted);
                    var quoteUploadData = new FileUploadDocumentDTO()
                    {
                        RiskDetailId = newSubmissionDetail.Id,
                        tableRefId = newSubmissionDetail.Id,
                        FileName = "Pre-Bind Quote",
                        FilePath = result,
                        IsConfidential = true,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                    };
                    await _fileUploadDocumentService.AddUpdateAsync(quoteUploadData);
                }
                catch (Exception e)
                {

                }
            }

            if (riskCoverage != null && riskCoverage.RiskCoveragePremium != null)
            {
                await _premiumService.UpdateRaterPremiumsCopySubmissionAsync(newSubmissionDetail.Id, fromPolicy: false, option: 1);
                await _riskPremiumTransactionService.CalculateAsync(newSubmissionDetail.Id);
            }

            return _mapper.Map<RiskDTO>(newSubmissionDetail);
        }

        public async Task<RiskDTO> RenewPolicy(string policyNumber)
        {
            RiskDetail newSubmissionDetail = null;
            try
            {
                var risk = await _riskRepository.FindAsync(x => x.PolicyNumber == policyNumber);
                var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value);
                var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
                var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
                var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

                // rewrite: re-clone quoted risk detail
                string newSubmissionNumber = _riskSequenceService.GenerateSubmissionNumber();
                string newQuoteNumber = _riskSequenceService.GenerateQuoteNumber();
                newSubmissionDetail = await _riskFactory.GenerateRenewalDetailFromPolicy(riskDetail.Id, risk.Id, newSubmissionNumber, newQuoteNumber);
                await _raterService.UpdateToLatestRaterVersionAsync(newSubmissionDetail.RiskId, newSubmissionDetail.Id);

                risk = await _riskRepository.FindAsync(risk.Id);
                risk.IsRenewed = true;
                await _riskRepository.SaveChangesAsync();

                await _applogService.Save($"Policy {policyNumber} renewal successful with submission {newSubmissionDetail.SubmissionNumber}.", null);

                return _mapper.Map<RiskDTO>(newSubmissionDetail);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                if (newSubmissionDetail != null)
                    await _submissionService.DeleteSubmissionAsync(newSubmissionDetail.SubmissionNumber);
                await _applogService.Save($"Policy {policyNumber} renewal failed.", null);
                throw;
            }
        }

        public async Task<bool> ExpirePolicy(string policyNumber)
        {
            var risk = await _riskRepository.FindAsync(x => x.PolicyNumber == policyNumber
                            && x.Status == DTO.Constants.RiskStatus.Active);
            if (risk != null)
            {
                risk = await _riskRepository.FindAsync(risk.Id);
                risk.Status = "Expired";

                await _riskRepository.SaveChangesAsync();

                await _applogService.Save($"Policy {policyNumber} expire successful.", null);

                return true;
            }

            return false;
        }

        public async Task CancelPolicyForRewrite(Guid riskDetailId, DateTime effectiveDate, bool? isFlatCancel = true)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var hasPendingEndorsement = _policyHistoryService.GetEndorsementHistoryByRiskId(risk.Id).Result.FindAll(result => Convert.ToInt32(result.EndorsementNumber) > 0 && result.PolicyStatus == RiskDetailStatus.Active);
            if (hasPendingEndorsement.Count > 0 || riskDetail.Status == RiskDetailStatus.Canceled)
            {
                throw new Exception("Unable to rewrite policy. Please try again");
            }
            var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);
            var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == binding.BindOptionId).FirstOrDefault();

            // still proceed if policy effective date is in future and need to rewrite policy
            if (effectiveDate < brokerInfo.EffectiveDate.Value)
            {
                effectiveDate = brokerInfo.EffectiveDate.Value;
            }

            // cancellation: reset and re-clone riskdetail
            var policyDetailForCancellation = await _riskFactory.GeneratePolicyDetailForCancellation(riskDetailId, riskDetail.RiskId, effectiveDate);

            // pro-rate/short rate
            // Cancel premium/fees.
            // Generate transactions from riskAmounts.
            await _cancellationService.CancelPremiumFees(riskDetailId, effectiveDate, false, isFlatCancel ?? false);

            // pass premium changes to billing
            var cancellationBreakdowns = _cancellationBreakdownRepository.GetByRiskDetailId(riskDetailId);
            await _billingService.ProcessBillingCancellation(risk.Id, effectiveDate, cancellationBreakdowns, isFlatCancel ?? false);

            // policy history
            var cancellationForRewriteDetails = new List<string>() { "Cancellation Information not found." };
            var endorsementNumber = await _policyHistoryService.CreatePolicyHistory(risk.Id, policyDetailForCancellation, riskDetail.Id, riskCoverage.RiskCoveragePremium, effectiveDate, RiskDetailStatus.Rewrite, JsonConvert.SerializeObject(cancellationForRewriteDetails));
        }

        public async Task AutoPolicyCancellation(DateTime effectiveDate)
        {
            var riskids = await _riskRepository.GetAllRiskIdForCancellationAsync(effectiveDate);
            if (riskids != null && riskids.Count() > 0)
            {
                foreach (Guid riskId in riskids)
                {
                    try
                    {
                        var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(riskId);
                        var risk = await _riskRepository.FindAsync(riskId);

                        if (riskDetail != null)
                        {
                            Guid riskDetailId = riskDetail.Id;
                            string cancellationReasonType = risk.CancellationReasonType;
                            bool isShortRate = false;

                            await CancelPolicy(riskDetailId, risk.PendingCancellationDate.Value, cancellationReasonType, isShortRate, null, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        await _logError(string.Concat("Job: AutoPolicyCancellation,", DateTime.Now, " ", riskId), ex.ToString());
                    }
                }
            }
        }

        public async Task AutoNoticeOfCancellation(DateTime effectiveDate)
        {
            var pnocList = await _billingService.GetRisksForNoticeOfCancellation(effectiveDate);
            var riskids = pnocList.Select(x => Guid.Parse(x.RiskId)).ToList();

            var risksFromManualCancellationNotice = await _riskRepository.GetManualNoticeCancellationRisks(effectiveDate);

            if (risksFromManualCancellationNotice.Count > 0)
            {
                riskids.AddRange(risksFromManualCancellationNotice.Select(x => x.Id).ToList());
            }

            if (riskids != null && riskids.Count() > 0)
            {
                foreach (Guid riskId in riskids.Distinct())
                {
                    try
                    {
                        var risk = await _riskRepository.FindAsync(x => x.Id == riskId && (x.PendingCancellationDate == null || x.PendingCancellationNoticeDate.HasValue) && x.Status == DTO.Constants.RiskStatus.Active && x.IsPendingCancellation == false);

                        if (risk != null)
                        {
                            var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(riskId);

                            if (risk.IsPendingCancellation == false)
                            {
                                DateTime currentNoticeDate = effectiveDate;

                                // Check if riskid is from Manual PNOC
                                if (!risksFromManualCancellationNotice.Any(x => x.Id == riskId))
                                {
                                    var pnocData = pnocList.FirstOrDefault(x => x.RiskId == riskId.ToString());
                                    var pnocDays = Environment.GetEnvironmentVariable("PnocDays") ?? "1";

                                    // Get PastDueDate
                                    var riskDueDate = pnocData?.DueDate;
                                    // Skip PNOC for past due less than 30days
                                    var riskPNOCDate = DateTime.Parse(riskDueDate).AddDays(Int32.Parse(pnocDays)).Date;
                                    if (effectiveDate.Date < riskPNOCDate)
                                        continue;

                                    //currentNoticeDate = riskPNOCDate;
                                }

                                int cancelDate = 30;
                                var riskForms = _formsService.GetByRiskIdAsync(riskDetail.Id).Result;
                                var mcs90Selected = riskForms.FirstOrDefault(rf => rf.FormId == "MCS90")?.IsSelected ?? false;
                                if (mcs90Selected) cancelDate = 35;

                                var cancellationEffectiveDate = (currentNoticeDate).AddDays(cancelDate);

                                cancellationEffectiveDate = await ValidateCancellationDate(cancellationEffectiveDate);

                                string cancellationReasonType = risk.CancellationReasonType != null ? risk.CancellationReasonType : "7";
                                cancellationEffectiveDate = risk.PendingCancellationDate.HasValue ? risk.PendingCancellationDate.Value : cancellationEffectiveDate;
                                currentNoticeDate = risk.PendingCancellationNoticeDate.HasValue ? risk.PendingCancellationNoticeDate.Value : currentNoticeDate;

                                // Set PendingCancellationDate
                                risk.PendingCancellationDate = cancellationEffectiveDate;
                                risk.PendingCancellationNoticeDate = currentNoticeDate;
                                risk.CancellationReasonType = cancellationReasonType;
                                risk.IsPendingCancellation = true;

                                _riskRepository.Update(risk);

                                await _riskRepository.SaveChangesAsync();

                                Guid riskDetailId = riskDetail.Id;

                                await _riskService.AddRiskStatusHistoryIsPendingCancellationAsync(riskDetailId, false, true);

                                // Generate cancellation notice documents
                                await NoticeCancelPolicy(riskDetailId, cancellationEffectiveDate, cancellationReasonType);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await _logError(string.Concat("Job: AutoNoticeOfCancellation,", DateTime.Now, " ", riskId), ex.ToString());
                    }
                }
            }
        }

        private async Task<DateTime> ValidateCancellationDate(DateTime nocDate)
        {
            var cancellationDate = nocDate;
            var holidayList = await _genericsRepository.GetHolidaySchedule();
            bool isDateValid = false;

            while (!isDateValid)
            {
                var holiday = holidayList.FirstOrDefault(val =>
                val.Day == cancellationDate.Day && val.Month == cancellationDate.Month);

                if (holiday != null)
                {
                    cancellationDate = cancellationDate.AddDays(1);
                    continue;
                }

                if (cancellationDate.DayOfWeek == DayOfWeek.Saturday || cancellationDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    cancellationDate = cancellationDate.AddDays(1);
                    continue;
                }

                isDateValid = true;
            }

            return cancellationDate;
        }

        public async Task AutoRescindNoticeOfCancellation(DateTime effectiveDate)
        {
            var riskKVpair = await _billingService.GetRisksForNoticeOfCancellation(effectiveDate);
            var riskids = riskKVpair.Select(x => Guid.Parse(x.RiskId)).ToList();

            var riskWithPendingCancellation = await _riskRepository.GetAllRiskIdForPendingCancellationAsync();
            if (riskWithPendingCancellation != null && riskWithPendingCancellation.Count() > 0)
            {
                foreach (var risk in riskWithPendingCancellation)
                {
                    try
                    {
                        if (riskids.Contains(risk.Id) == false)
                        {
                            if (risk.PendingCancellationNoticeDate.HasValue && risk.PendingCancellationNoticeDate > DateTime.Now) continue;

                            var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(risk.Id);

                            if (riskDetail != null && risk.IsPendingCancellation == true
                                && (
                                    !string.Equals(risk.CancellationReasonType, CancellationReasons.FinanceCompanyNonPay.Id.ToString()) &&
                                    !string.Equals(risk.CancellationReasonType, CancellationReasons.NonPaymentOfDeductible.Id.ToString())
                                ))
                            {
                                var billing = await _billingService.GetBillingSummaryAsync(risk.Id);
                                
                                bool exCovenir = false;
                                exCovenir = billing.PremiumAmount > 100000? true : false;
                               
                                    // Clear PendingCancellationDate
                                    risk.PendingCancellationDate = null;
                                    risk.PendingCancellationNoticeDate = null;
                                    risk.CancellationReasonType = null;
                                    risk.IsPendingCancellation = false;

                                    _riskRepository.Update(risk);

                                    await _riskRepository.SaveChangesAsync();

                                    await _riskService.AddRiskStatusHistoryIsPendingCancellationAsync(riskDetail.Id, true, false);

                                    // Generate Reinstatement Document
                                    await ReinstatePendingCancellationPolicy(riskDetail.Id, exCovenir);
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await _logError(string.Concat("Job: AutoRescindNoticeOfCancellation,", DateTime.Now, " ", risk.Id), ex.ToString());
                    }
                }
            }
        }

        public async Task AutoReinstateCancelledPolicy(DateTime effectiveDate)
        {
            var riskids = await _billingService.GetRisksForReinstate(effectiveDate);
            if (riskids != null && riskids.Count() > 0)
            {
                foreach (Guid riskId in riskids)
                {
                    try
                    {
                        var risk = await _riskRepository.FindAsync(x => x.Id == riskId && x.Status == DTO.Constants.RiskStatus.Canceled);
                        if (risk != null)
                        {
                            var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(risk.Id);
                            var policyHistory = await _policyHistoryService.GetLastCancelledByRiskId(risk.Id);
                            // Only Reinstate Cancellation with Non-Payment Reason
                            if (policyHistory.Details.Contains("Cancellation Reason: Non-Payment of Premium") || policyHistory.Details.Contains("Cancellation Reason: Non-Payment of Deductible"))
                            {
                                // Reinstatement Policy
                                await ReinstatePolicy(policyHistory.RiskDetailId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await _logError(string.Concat("Job: AutoReinstateCancelledPolicy,", DateTime.Now, " ", riskId), ex.ToString());
                    }
                }
            }
        }

        private async Task _logError(string error, string jsonmsg)
        {
            ErrorLogDTO errLog = new ErrorLogDTO
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                ErrorMessage = error,
                JsonMessage = jsonmsg,
                CreatedDate = DateTime.Now
            };
            await _errorLogger.ErrorAsync(errLog);
        }

        public async Task<RiskDTO> ManualRescindPolicy(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);

            riskDetail.Status = RiskDetailStatus.Active;

            _riskDetailRepository.Update(riskDetail);

            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            var billing = await _billingService.GetBillingSummaryAsync(risk.Id);

            bool exCovenir = false;
            exCovenir = billing.PremiumAmount > 100000 ? true : false;

            // Clear PendingCancellationDate
            var noticeDate = risk.PendingCancellationNoticeDate;
            risk.PendingCancellationDate = null;
            risk.PendingCancellationNoticeDate = null;
            risk.CancellationReasonType = null;
            risk.IsPendingCancellation = false;

            _riskRepository.Update(risk);

            await _riskService.AddRiskStatusHistoryIsPendingCancellationAsync(riskDetailId, true, false);

            if (noticeDate <= DateTime.Now)
            {
                // Generate Reinstatement Document
                await ReinstatePendingCancellationPolicy(riskDetail.Id, exCovenir);
            }

            await _riskRepository.SaveChangesAsync();

            return _mapper.Map<RiskDTO>(riskDetail);
        }

        public async Task ManualCancellationNotice(Guid riskId, Guid riskDetailId, DateTime noticeDate, DateTime effectiveDate, string cancellationReasonType)
        {
            var risk = await _riskRepository.FindAsync(riskId);

            await _riskService.AddRiskStatusHistoryPNOCNoticeDateAsync(riskDetailId, risk.PendingCancellationNoticeDate, noticeDate);

            risk.PendingCancellationNoticeDate = noticeDate;
            risk.PendingCancellationDate = effectiveDate;
            risk.CancellationReasonType = cancellationReasonType;

            if (noticeDate <= DateTime.Now)
            {
                risk.IsPendingCancellation = true;
            }

            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();

            if (noticeDate <= DateTime.Now)
            {
                await _riskService.AddRiskStatusHistoryIsPendingCancellationAsync(riskDetailId, false, true);

                // Generate cancellation notice documents
                await NoticeCancelPolicy(riskDetailId, effectiveDate, cancellationReasonType);
            }
        }

        public async Task ManualRescindPNOCByRiskIdDataFix(Guid riskId)
        {
            try
            {
                var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(riskId);

                if (riskDetail != null)
                {
                    var risk = await _riskRepository.FindAsync(x => x.Id == riskId);

                    // Clear PendingCancellationDate
                    risk.PendingCancellationDate = null;
                    _riskRepository.Update(risk);

                    // Generate Reinstatement Document
                    await ReinstatePendingCancellationPolicy(riskDetail.Id);

                    await _riskRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logError(string.Concat("Job: AutoRescindNoticeOfCancellation,", DateTime.Now, " ", riskId), ex.ToString());
            }
        }
    }
}
