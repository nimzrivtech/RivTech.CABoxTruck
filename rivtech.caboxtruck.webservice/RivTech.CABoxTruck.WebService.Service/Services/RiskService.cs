using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RiskData = RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using Microsoft.Extensions.Configuration;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskService : IRiskService
    {
        private readonly IMapper _mapper;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IRiskSpecificMaintenanceSafetyService _maintenanceSafetyService;
        private readonly IRiskSpecificGeneralLiabilityCargoService _generalLiabilityCargoService;
        private readonly IBusinessDetailsService _businessDetailsService;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IClaimsHistoryService _claimsHistoryService;
        private readonly IRiskStatusHistoryService _riskStatusHistoryService;
        private readonly IRiskNotesService _riskNotesService;
        private readonly IFormsService _formsService;
        private readonly IBrokerInfoService _brokerInfoService;
        private readonly IBindingService _bindingService;
        private readonly IFileUploadDocumentService _documentService;
        private readonly IApplicantAdditionalInterestService _additionalInterestService;
        private readonly IRiskManuscriptsService _manuscriptsService;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IRiskPolicyContactService _riskPolicyContactService;
        private readonly IRiskFactory _riskFactory;
        private readonly IEndorsementDataCleaner _endorsementDataCleaner;
        private readonly IFileUploadDocumentRepository _fileUploadDocumentRepository;
        private readonly IRiskPolicyPremiumTransactionRepository _riskPolicyPremiumTransactionRepository;
        private readonly IPremiumService _premiumService;
        private readonly IRiskPremiumTransactionService _riskPremiumTransactionService;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IRiskSpecificDestinationService _riskSpecificDestinationService;
        private readonly IRiskStatusHistoryRepository _riskStatusHistoryRepository;
        private readonly string _excludedPolicyNumbers;
        private readonly IConfiguration _configuration;

        public RiskService(
            IMapper mapper,
            IRiskDetailRepository riskDetailRepository,
            IRiskRepository riskRepository,
            IEntityAddressRepository entityAddressRepository,
            IRiskSpecificMaintenanceSafetyService maintenanceSafetyService,
            IRiskSpecificGeneralLiabilityCargoService generalLiabilityCargoService,
            IBusinessDetailsService businessDetailsService,
            ICoverageHistoryService coverageHistoryService,
            IClaimsHistoryService claimsHistoryService,
            IRiskStatusHistoryService riskStatusHistoryService,
            IRiskNotesService riskNotesService,
            IFormsService formsService,
            IBrokerInfoService brokerInfoService,
            IBindingService bindingService,
            IFileUploadDocumentService documentService,
            IApplicantAdditionalInterestService additionalInterestService,
            IRiskManuscriptsService manuscriptsService,
            IPolicyHistoryRepository policyHistoryRepository,
            IRiskPolicyContactService riskPolicyContactService,
            IRiskFactory riskFactory,
            IEndorsementDataCleaner endorsementDataCleaner,
            IFileUploadDocumentRepository fileUploadDocumentRepository,
            IRiskPolicyPremiumTransactionRepository riskPolicyPremiumTransactionRepository,
            IPremiumService premiumService,
            IRiskPremiumTransactionService riskPremiumTransactionService,
            IEndorsementChangesService endorsementChangesService,
            IRiskSpecificDestinationService riskSpecificDestinationService,
            IRiskStatusHistoryRepository riskStatusHistoryRepository,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _entityAddressRepository = entityAddressRepository;
            _maintenanceSafetyService = maintenanceSafetyService;
            _generalLiabilityCargoService = generalLiabilityCargoService;
            _businessDetailsService = businessDetailsService;
            _coverageHistoryService = coverageHistoryService;
            _claimsHistoryService = claimsHistoryService;
            _riskStatusHistoryService = riskStatusHistoryService;
            _riskNotesService = riskNotesService;
            _formsService = formsService;
            _brokerInfoService = brokerInfoService;
            _bindingService = bindingService;
            _documentService = documentService;
            _additionalInterestService = additionalInterestService;
            _manuscriptsService = manuscriptsService;
            _policyHistoryRepository = policyHistoryRepository;
            _riskPolicyContactService = riskPolicyContactService;
            _riskFactory = riskFactory;
            _endorsementDataCleaner = endorsementDataCleaner;
            _fileUploadDocumentRepository = fileUploadDocumentRepository;
            _riskPolicyPremiumTransactionRepository = riskPolicyPremiumTransactionRepository;
            _premiumService = premiumService;
            _riskPremiumTransactionService = riskPremiumTransactionService;
            _endorsementChangesService = endorsementChangesService;
            _riskSpecificDestinationService = riskSpecificDestinationService;
            _riskStatusHistoryRepository = riskStatusHistoryRepository;
            _configuration = configuration;
            _excludedPolicyNumbers = _configuration["ExcludedPoliciesFromExpiry"] ?? string.Empty;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _riskDetailRepository.AnyAsync(x => x.Id == id);
        }

        public async Task<List<RiskDTO>> GetAllAsync()
        {
            var result = await _riskDetailRepository.GetAllAsync();
            return _mapper.Map<List<RiskDTO>>(result);
        }

        public async Task<RiskDTO> GetRiskAsync(Guid id)
        {
            var result = await _riskDetailRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<RiskDTO>(result);
        }

        public async Task<RiskDTO> InsertRiskAsync(RiskDTO model)
        {
            var result = await _riskDetailRepository.AddAsync(_mapper.Map<Data.Entity.RiskDetail>(model));
            await _riskDetailRepository.SaveChangesAsync();
            return _mapper.Map<RiskDTO>(result);
        }

        public async Task<string> RemoveRiskAsync(Guid id)
        {
            var entity = await _riskDetailRepository.FindAsync(x => x.Id == id);
            //entity.IsActive = false;
            //entity.DeletedDate = DateTime.Now;
            //await _riskDetailRepository.SaveChangesAsync();acc
            //return entity.Id.ToString();
            return id.ToString();
        }

        public async Task<string> UpdateRiskAsync(RiskDTO model)
        {
            var result = _riskDetailRepository.Update(_mapper.Map<Data.Entity.RiskDetail>(model));
            await _riskDetailRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<bool> UpdateRiskStatusAsync(RiskDTO model)
        {
            var risk = await _riskDetailRepository.FindAsync(r => r.Id == model.Id);

            if (risk.Status != model.Status)
            {
                var statusHistory = CreateStatusHistoryModel(risk.Id, "Status", risk.Status, model.Status);
                await _riskStatusHistoryService.InsertAsync(statusHistory);
            }
            if (risk.AssignedToId != model.AssignedToId)
            {
                var assignedToHistory = CreateStatusHistoryModel(risk.Id, "AssignedToId", risk.AssignedToId?.ToString(), model.AssignedToId?.ToString());
                await _riskStatusHistoryService.InsertAsync(assignedToHistory);
            }

            risk.Status = model.Status;
            risk.AssignedToId = model.AssignedToId;

            await _riskDetailRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRiskStatusOnlyAsync(Guid riskDetailId, string status)
        {
            var risk = await _riskDetailRepository.FindAsync(r => r.Id == riskDetailId);

            if (risk.Status != status)
            {
                var statusHistory = CreateStatusHistoryModel(risk.Id, "Status", risk.Status, status);
                await _riskStatusHistoryService.InsertAsync(statusHistory);
            }

            risk.Status = status;

            await _riskDetailRepository.SaveChangesAsync();
            return true;
        }

        public async Task<string> UpdatePendingEndorsementAsync(Guid riskDetailId)
        {
            return await UpdateRiskStatusPendingEndorsement(riskDetailId);
        }

        private async Task<string> UpdateRiskStatusPendingEndorsement(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(r => r.Id == riskDetailId);
            var risk = await _riskRepository.FindAsync(r => r.Id == riskDetail.RiskId);
            var changes = await _riskFactory.GetPolicyChanges(riskDetailId, riskDetail.RiskId, true);
            var endorsementChanges = await _endorsementChangesService.GetChangesByRiskAndEndorsement(risk.Id, risk.NextEndorsementNumber());
            var hasRatingFactorsChangeItem = endorsementChanges.Any(x => x is RatingFactorsChangeItem);

            if (changes.Count() > 0 || hasRatingFactorsChangeItem)
            {
                var hasActivePendingStatusHistory = await _riskStatusHistoryRepository.GetAsync(r => r.RiskDetailId == riskDetailId && r.OldValue == RiskStatus.Active && r.NewValue == RiskDetailStatus.PendingEndorsement);

                if (!hasActivePendingStatusHistory.Any())
                {
                    var newStatus = RiskDetailStatus.PendingEndorsement;
                    var oldStatus = riskDetail.Status;

                    if (riskDetail.Status == RiskDetailStatus.PendingEndorsement)
                    {
                        oldStatus = RiskStatus.Active;
                    }
                    else if (riskDetail.Status == RiskStatus.Canceled)
                    {
                        newStatus = RiskStatus.Canceled;
                        oldStatus = RiskStatus.Active;
                    }

                    var statusHistory = CreateStatusHistoryModel(riskDetail.Id, "Status", oldStatus, newStatus);
                    await _riskStatusHistoryService.InsertAsync(statusHistory);
                    if (risk.Status == RiskStatus.Active)
                        riskDetail.Status = RiskDetailStatus.PendingEndorsement;
                }
            }
            else
            {
                if (risk.Status == RiskStatus.Active && !(risk.PendingCancellationNoticeDate.HasValue && risk.PendingCancellationNoticeDate.Value <= DateTime.Now))
                    riskDetail.Status = RiskDetailStatus.Active;
            }

            await _riskDetailRepository.SaveChangesAsync();

            return riskDetail.Status;
        }

        private static RiskStatusHistory CreateStatusHistoryModel(Guid riskId, string field, string oldValue, string newValue)
        {
            var riskStatusHistory = new RiskStatusHistory
            {
                RiskDetailId = riskId,
                ChangedBy = (int)Data.Common.Services.GetCurrentUser(),
                ChangedDate = DateTime.Now,
                ChangedField = field,
                OldValue = oldValue,
                NewValue = newValue
            };
            return riskStatusHistory;
        }

        public async Task<List<RiskDTO>> GetAllIncludeAsync()
        {
            var result = await _riskDetailRepository.GetAllIncludeAsync();
            return _mapper.Map<List<RiskDTO>>(result).ToList();
        }

        public async Task<Guid> GetRiskDetailIdByStatusAsync(Guid riskId, string status)
        {
            var result = await _riskDetailRepository.GetAsync(rd => rd.RiskId == riskId && rd.Status == status);
            return result.OrderByDescending(r => r.UpdatedDate).FirstOrDefault()?.Id ?? Guid.Empty;
        }

        public async Task<RiskDetail> GetSubmissionRiskDetailAsync(Guid riskId)
        {
            return await _riskDetailRepository.GetSubmissionRiskDetailAsync(riskId);
        }

        public async Task<RiskDTO> GetByIdIncludeAsync(Guid riskDetailId, bool withAgency = true)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);

            RiskDTO result = new RiskDTO();

            if (riskDetail != null)
            {
                var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
                var vehicleFactors = await _riskDetailRepository.GetVehicleRatingFactorsByRiskDetailId(riskDetailId);

                if (riskDetail.SourceRiskDetailId != null)
                {
                    var riskPolicyGLPremiumTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(riskDetail.SourceRiskDetailId.Value, "GL");

                    if (riskPolicyGLPremiumTransaction != null)
                    {
                        if (riskDetail.RiskCoverages[0].RiskCoveragePremium != null)
                        {
                            riskDetail.RiskCoverages[0].RiskCoveragePremium.ProratedGeneralLiabilityPremium = 
                                (riskDetail.Status == "Canceled") ? riskDetail.RiskCoverages[0].RiskCoveragePremium.ProratedGeneralLiabilityPremium : riskPolicyGLPremiumTransaction.CurrentPremium;
                        }
                    }

                    if (risk.RaterVersionId >= 7)
                    {
                        var riskPolicyTIPremiumTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(riskDetail.SourceRiskDetailId.Value, "TI");

                        if (riskPolicyTIPremiumTransaction != null)
                        {
                            if (riskDetail.RiskCoverages[0].RiskCoveragePremium != null)
                            {
                                riskDetail.RiskCoverages[0].RiskCoveragePremium.ProratedTrailerInterchangePremiumAL =
                                    (riskDetail.Status == "Canceled") ? riskDetail.RiskCoverages[0].RiskCoveragePremium.ProratedTrailerInterchangePremiumAL : riskPolicyTIPremiumTransaction.CurrentPremium;
                            }
                        }

                        var riskPolicyNOPremiumTransaction = await _riskPolicyPremiumTransactionRepository.GetAsync(riskDetail.SourceRiskDetailId.Value, "NO");

                        if (riskPolicyNOPremiumTransaction != null)
                        {
                            if (riskDetail.RiskCoverages[0].RiskCoveragePremium != null)
                            {
                                riskDetail.RiskCoverages[0].RiskCoveragePremium.ProratedNonOwnedTrailerPremium =
                                    (riskDetail.Status == "Canceled") ? riskDetail.RiskCoverages[0].RiskCoveragePremium.ProratedNonOwnedTrailerPremium : riskPolicyNOPremiumTransaction.CurrentPremium;
                            }
                        }
                    }
                }

                result = _mapper.Map<RiskDTO>(riskDetail);

                var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == riskDetail.RiskId);
                RiskDetail previousRiskDetail = null;
                if (riskDetail.SourceRiskDetailId.HasValue)
                    previousRiskDetail = await _riskDetailRepository.FindAsync(riskDetail.SourceRiskDetailId.Value);

                await SetNameAndAddressAsync(riskDetail, result);
                SetFilingsInformation(riskDetail, result);
                if (riskDetails.Count() > 1)
                    result.IsPolicy = true;
                result.Status = risk.Status;
                result.SubStatus = riskDetail.Status;
                if (risk.Status == RiskStatus.Canceled)
                {
                    result.SubStatus = CancelationReason(riskDetail) ?? riskDetail.Status;
                }
                result.RaterVersionId = risk.RaterVersionId;
                result.IsRaterVersionLocked = risk.IsRaterVersionLocked;
                result.UpdateXPRaterVersion = risk.UpdateXPRaterVersion;
                result.PendingCancellationDate = risk.PendingCancellationDate;
                result.PendingCancellationNoticeDate = risk.PendingCancellationNoticeDate;
                result.PolicyNumber = risk?.PolicyNumber;
                result.QuoteNumber = risk?.QuoteNumber;
                result.EndorsementEffectiveDate = riskDetail.EndorsementEffectiveDate;
                result.PrevEndorsementEffectiveDate = riskDetail.EndorsementEffectiveDate;
                if (previousRiskDetail != null)
                    result.PrevEndorsementEffectiveDate = previousRiskDetail.EndorsementEffectiveDate;
                //result.Vehicles = result.Vehicles?.Where(x =>(x.DeletedDate is null && x.IsActive != false))?.ToList();
                result.VehicleFactors = _mapper.Map<List<PremiumRaterVehicleFactorsDTO>>(vehicleFactors);
                result.Drivers = result.Drivers?.Where(x => x.IsActive != false).OrderBy(d => d.CreatedDate)?.ToList();

                if(withAgency)
                    result.BrokerInfo = await _brokerInfoService.GetByRiskDetailIdAllIncludeAsync(riskDetail.RiskId);
                else 
                    result.BrokerInfo = await _brokerInfoService.GetByRiskDetailIdAsync(riskDetail.RiskId);

                result.RiskHistory = await _coverageHistoryService.GetByIdAsync(riskDetailId);
                result.BusinessDetails = await _businessDetailsService.GetBusinessDetails(riskDetailId);
                result.MaintenanceSafety = await _maintenanceSafetyService.GetMaintenanceSafety(riskDetailId);
                result.GeneralLiabilityCargo = await _generalLiabilityCargoService.GetGeneralLiabilityCargo(riskDetailId);
                result.GeneralLiabilityCargo.CommoditiesHauled = _mapper.Map<List<RiskSpecificCommoditiesHauledDto>>(riskDetail.CommoditiesHauled);
                result.ClaimsHistory = await _claimsHistoryService.GetByRiskDetailIdAsync(riskDetailId);
                result.RiskNotes = await _riskNotesService.GetByRiskIdAsync(riskDetailId);
                result.RiskForms = await _formsService.GetByRiskIdAsync(riskDetailId);

                result.Binding = await _bindingService.GetBindingByRiskIdAsync(riskDetail.RiskId);
                result.BindingRequirements = await _bindingService.GetByRiskDetailIdAsync(riskDetailId);

                result.QuoteConditions = await _bindingService.GetQuoteByRiskDetailIdAsync(riskDetailId);

                result.BlanketCoverage = _mapper.Map<BlanketCoverageDTO>(riskDetail);
                result.AdditionalInterest = (await _additionalInterestService.GetByRiskDetailIdActiveAsync(riskDetailId)).ToList();
                result.RiskManuscripts = await _manuscriptsService.GetByRiskIdAsync(riskDetailId);
                result.RiskDocuments = await _documentService.GetByRefIdAsync(riskDetailId);
                result.RiskPolicyContacts = await _riskPolicyContactService.GetByRiskDetailIdAsync(riskDetailId);
                result.HasInitialDriverExclusion = await _formsService.HasInitialDriverExclusionFormAsync(riskDetail.RiskId);
                result.RiskSpecificDestinations = await _riskSpecificDestinationService.GetAllAsync(riskDetailId);

                // Renewal details
                result.IsRenewal = risk.IsRenewal;
                result.IsFromRenewal = risk.IsFromRenewal;
                result.RenewalDate = risk.RenewalDate;
                result.PreviousPolicyNumber = risk.PreviousPolicyNumber;
                result.RiskRelated = GetRelatedRiskIds(risk.Id);

                //Power units,Trailer and other count
                result.CountPowerUnit = riskDetail.CountPowerUnit;
                result.CountOthers = riskDetail.CountOthers;
                result.CountTrailer = riskDetail.CountTrailer;

                //Policy Contacts
                result.InternalUWUserId = risk.InternalUWUserId;
                result.ProductUWUserId = risk.ProductUWUserId;
                result.PsrUserId = risk.PsrUserId;
                result.AssistantUnderwriterId = risk.AssistantUnderwriterId;
                result.TeamLeadUserId = risk.TeamLeadUserId;

                //Field Versions
                var fieldVersions = await _riskRepository.GetFieldVersions();
                result.FieldVersions = _mapper.Map<List<FieldVersionDTO>>(fieldVersions);
            }
            return result;
        }

        private void SetFilingsInformation(RiskDetail risk, RiskDTO riskDto)
        {
            var filingsInfoDto = _mapper.Map<FilingsInformationDTO>(risk);

            if (risk?.Insured?.Entity != null)
            {
                filingsInfoDto.USDOTNumber = risk.Insured.Entity.USDOTNumber;
                filingsInfoDto.McNumber = risk.Insured.Entity.McNumber;
            }
            if (filingsInfoDto.IsFilingRequired.GetValueOrDefault(false) && risk?.RiskFilings != null)
            {
                filingsInfoDto.RequiredFilingIds = risk.RiskFilings.Select(rf => rf.FilingTypeId).ToList();
            }
            riskDto.FilingsInformation = filingsInfoDto;
        }

        private async Task SetNameAndAddressAsync(RiskDetail risk, RiskDTO riskDto)
        {
            riskDto.NameAndAddress = new NameAndAddressDTO();
            riskDto.NameAndAddress.BusinessName = risk.Insured?.Entity?.CompanyName;
            riskDto.NameAndAddress.InsuredDBA = risk.Insured?.Entity?.DBA;
            riskDto.NameAndAddress.BusinessPrincipal = risk.Insured?.Entity?.BusinessPrincipal;
            //riskDto.NameAndAddress.PrimaryCityZipCodeOfOperationsId = risk.PrimaryCityZipCodeOfOperationsId;
            riskDto.NameAndAddress.Email = risk.Insured?.Entity?.WorkEmailAddress;
            riskDto.NameAndAddress.PhoneExt = risk.Insured?.Entity?.WorkPhoneExtension;

            var businessAddress = await _entityAddressRepository
                .FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business && x.EntityId == risk.Insured.EntityId, i => i.Address);
            riskDto.NameAndAddress.BusinessAddress = businessAddress?.Address?.StreetAddress1;
            riskDto.NameAndAddress.City = businessAddress?.Address?.City;
            riskDto.NameAndAddress.State = businessAddress?.Address?.State;
            riskDto.NameAndAddress.ZipCode = businessAddress?.Address?.ZipCode;

            var primaryCityOfOperations = await _entityAddressRepository
                .FindAsync(x => x.AddressTypeId == null && x.EntityId == risk.Insured.EntityId, i => i.Address);

            riskDto.NameAndAddress.CityOperations = primaryCityOfOperations?.Address?.City;
            riskDto.NameAndAddress.StateOperations = primaryCityOfOperations?.Address?.State;
            riskDto.NameAndAddress.ZipCodeOperations = primaryCityOfOperations?.Address?.ZipCode;

            var entityAddresses = _mapper.Map<List<EntityAddressDTO>>(risk?.Insured?.Entity?.EntityAddresses);
            riskDto.NameAndAddress.Addresses = entityAddresses;
        }

        private string CancelationReason(RiskDetail riskDetail)
        {
            var history = _policyHistoryRepository.FindAsync(x => x.RiskDetailId == riskDetail.Id && x.PolicyStatus == RiskDetailStatus.Rewrite).Result;
            if (history != null)
            {
                return RiskDetailStatus.Rewrite;
            }

            return null;
        }

        public async Task<PaymentPortalSearchResponseDTO> SearchRiskByPolicyAndZipCodeAsync(string policyNumber, string zipCode)
        {
            var risk = await _riskRepository.GetRiskByPolicyNumberAsync(policyNumber);
            var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(risk?.Id);
            var result = new PaymentPortalSearchResponseDTO();

            var businessAddress = _entityAddressRepository.FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Billing && x.EntityId == riskDetail.Insured.EntityId,
                i => i.Address).Result;

            if (riskDetail != null && businessAddress?.Address?.ZipCode == zipCode)
            {
                result = _mapper.Map<PaymentPortalSearchResponseDTO>(riskDetail);
                result.NameAndAddress = new NameAndAddressDTO();
                result.NameAndAddress.BusinessName = riskDetail?.Insured?.Entity?.CompanyName;
            }
            return result;
        }

        public async Task ProcessRiskDetailsForIssuance(Guid riskId, RiskDetail riskDetailToIssue, BrokerInfo brokerInfo)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var forEndorseRiskDetailIdBeforeIssueId = risk.ForEndorsementRiskDetailId;
            RiskDetail forEndorseRiskDetailIdBeforeIssue = null;
            if (forEndorseRiskDetailIdBeforeIssueId != null)
            {
                forEndorseRiskDetailIdBeforeIssue = await _riskDetailRepository.FindAsync(forEndorseRiskDetailIdBeforeIssueId.Value);
            }
            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == riskId);
            riskDetails = riskDetails.OrderByDescending(x => x.CreatedDate);
            var riskDetailIds = riskDetails.Where(x => x.IsProcessed != true).Select(x => x.Id);
            var lastCreatedRiskDetailId = riskDetailIds.First();
            var boundRiskDetailId = risk.CurrentRiskDetailId;

            List<FileUploadDocument> fileUploadDocuments = new List<FileUploadDocument>();

            if (risk.ForEndorsementRiskDetailId.HasValue && risk.ForEndorsementRiskDetailId != lastCreatedRiskDetailId)
            {
                fileUploadDocuments = (await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId)).ToList();

                await _endorsementDataCleaner.DeleteForEndorsementDataAsync(riskId);

                riskDetailIds = riskDetailIds.Where(x => !x.Equals(risk.ForEndorsementRiskDetailId)).ToList();
                risk.ForEndorsementRiskDetailId = null;
            }

            risk.CurrentRiskDetailId = lastCreatedRiskDetailId;

            foreach (var riskDetailId in riskDetailIds)
            {
                var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
                riskDetail.IsProcessed = true;
                riskDetail.Status = RiskStatus.Active;
            }

            await _riskRepository.SaveChangesAsync();
            await _riskDetailRepository.SaveChangesAsync();

            risk = await _riskRepository.FindAsync(risk.Id);

            bool isForEndorseFromOutOfSequence = forEndorseRiskDetailIdBeforeIssueId != null && forEndorseRiskDetailIdBeforeIssueId != risk.ForEndorsementRiskDetailId;
            if (isForEndorseFromOutOfSequence)
            {
                riskDetailToIssue = await _riskDetailRepository.GetOneIncludeByIdAsync(lastCreatedRiskDetailId);
            }

            RiskDetail policyDetailForEndorsement = null;
            if (riskDetailToIssue.EndorsementNumber == 0)
            {
                policyDetailForEndorsement = await _riskFactory.GeneratePolicy(risk.CurrentRiskDetailId.Value, risk.PolicyNumber);
            }
            else
            {
                policyDetailForEndorsement = await _riskFactory.GeneratePolicyDetailForEndorsement(risk.CurrentRiskDetailId.Value, riskDetailToIssue.EndorsementEffectiveDate ?? brokerInfo.EffectiveDate.Value, risk, boundRiskDetailId.Value);
            }
            policyDetailForEndorsement = await _riskDetailRepository.FindAsync(policyDetailForEndorsement.Id);

            risk.IssuanceUpdate(riskDetailToIssue, policyDetailForEndorsement);
            risk.EndorsementEffectiveDatePrompted = false;
            policyDetailForEndorsement.EndorsementNumber = risk.CurrentEndorsementNumber + 1;
            policyDetailForEndorsement.VersionNumber = 0;
            policyDetailForEndorsement.SourceRiskDetailId = riskDetailToIssue.Id;
            policyDetailForEndorsement.IsProcessed = false;
            policyDetailForEndorsement.EndorsementEffectiveDate ??= brokerInfo.EffectiveDate.Value;
            policyDetailForEndorsement.FromReinstatement = false;
            await _riskDetailRepository.SaveChangesAsync();

            if (fileUploadDocuments.Any())
            {
                foreach (var item in fileUploadDocuments)
                {
                    //TODO:Need to change value to endorsementRiskdetailId
                    item.RiskDetailId = risk.CurrentRiskDetailId;
                }
                await _fileUploadDocumentRepository.AddRangeAsync(fileUploadDocuments);
                await _fileUploadDocumentRepository.SaveChangesAsync();
            }

            risk.RiskDetails = null;
            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();
        }
        public async Task ProcessRiskDetailsForReinstatement(Guid riskId, RiskDetail canceledRiskDetail, BrokerInfo brokerInfo)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var boundRiskDetail = await _riskDetailRepository.FindAsync(risk.CurrentRiskDetailId.Value);

            RiskDetail policyDetailForEndorsement = await _riskFactory.GeneratePolicyDetailForReinstatement(risk.CurrentRiskDetailId.Value, risk.Id);
            policyDetailForEndorsement = await _riskDetailRepository.FindAsync(policyDetailForEndorsement.Id);

            //risk.CurrentRiskDetailId = riskDetailToIssue.Id;
            risk.ForEndorsementRiskDetailId = policyDetailForEndorsement.Id;
            risk.LastReinstatedRiskDetailId = canceledRiskDetail.Id;

            policyDetailForEndorsement.EndorsementNumber = risk.CurrentEndorsementNumber + 1;
            policyDetailForEndorsement.VersionNumber = 0;
            policyDetailForEndorsement.SourceRiskDetailId = risk.CurrentRiskDetailId;
            policyDetailForEndorsement.IsProcessed = false;
            policyDetailForEndorsement.EndorsementEffectiveDate ??= brokerInfo.EffectiveDate.Value;
            policyDetailForEndorsement.FromReinstatement = true;
            await _riskDetailRepository.SaveChangesAsync();

            List<FileUploadDocument> fileUploadDocuments = new List<FileUploadDocument>();

            fileUploadDocuments = (await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId)).ToList();

            if (fileUploadDocuments.Any())
            {
                foreach (var item in fileUploadDocuments)
                {
                    //TODO:Need to change value to endorsementRiskdetailId
                    item.RiskDetailId = risk.CurrentRiskDetailId;
                }
                await _fileUploadDocumentRepository.AddRangeAsync(fileUploadDocuments);
                await _fileUploadDocumentRepository.SaveChangesAsync();
            }

            await _premiumService.GenerateReinstatementPremiums(boundRiskDetail, canceledRiskDetail);
            // Generate transactions for reinstatement premiums.
            await _riskPremiumTransactionService.CalculateAfterReinstatementAsync(canceledRiskDetail);
            // Generate transactions for new ForEndorsement RiskDetail.
            await _riskPremiumTransactionService.CalculateAsync(risk.ForEndorsementRiskDetailId.Value, risk.CurrentRiskDetailId);

            risk.RiskDetails = null;
            risk.Status = RiskStatus.Active;
            risk.PendingCancellationDate = null;
            risk.PendingCancellationNoticeDate = null;
            risk.CancellationReasonType = null;
            risk.IsPendingCancellation = false;
            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();
        }

        public async Task UpdateForEndorsementRiskDetailId(RiskData risk)
        {
            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == risk.Id);
            riskDetails = riskDetails.OrderByDescending(x => x.CreatedDate);
            var riskDetailIds = riskDetails.Where(x => x.IsProcessed != true).Select(x => x.Id);
            var lastCreatedRiskDetailId = riskDetailIds.First();
            risk.ForEndorsementRiskDetailId = lastCreatedRiskDetailId;

            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();
        }

        public async Task UpdatePolicyNumberAsync(Guid riskId, string policyNumber)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            risk.PolicyNumber = policyNumber;
            await _riskRepository.SaveChangesAsync();
        }

        public async Task<Guid> GetLatestRiskDetailVersionAsync(Guid riskId)
        {
            return await _riskDetailRepository.GetLatestVersionAsync(riskId);
        }

        public async Task AddRiskStatusHistoryIsPendingCancellationAsync(Guid riskDetailId, bool oldStatus, bool newStatus)
        {
            var statusHistory = CreateStatusHistoryModel(riskDetailId, "IsPendingCancellation", oldStatus.ToString(), newStatus.ToString());
            await _riskStatusHistoryService.InsertAsync(statusHistory);
        }
        public async Task AddRiskStatusHistoryPNOCNoticeDateAsync(Guid riskDetailId, DateTime? oldStatus, DateTime newStatus)
        {
            var statusHistory = CreateStatusHistoryModel(riskDetailId, "PendingCancellationNoticeDate", oldStatus.HasValue ? oldStatus.Value.ToString() : "null", newStatus.ToString());
            await _riskStatusHistoryService.InsertAsync(statusHistory);
        }

        public async Task<string[]> GetPoliciesForRenewalAsync(DateTime date)
        {
            return await _riskRepository.GetForRenewalsAsync(date);
        }

        public async Task<string[]> GetExpiredPoliciesAsync(DateTime date)
        {
            string[] excludedPolicyNumbers = Array.Empty<string>();
            var expiredPolicies = await _riskRepository.GetExpiredPoliciesAsync(date);
            
            if (!string.IsNullOrEmpty(_excludedPolicyNumbers))
            {
                excludedPolicyNumbers = _excludedPolicyNumbers.ToString().Split(",");
                expiredPolicies = expiredPolicies.Where(e => !excludedPolicyNumbers.Contains(e)).ToArray();
            }

            return expiredPolicies;
        }

        public List<RiskRelatedDTO> GetRelatedRiskIds(Guid riskId)
        {
            var relatedRiskIds = new List<RiskRelatedDTO>();
            Guid? initialRiskId = null;
            var riskIdCheck = riskId;
            // Get initial RiskId
            do
            {
                var risk = _riskRepository.FindAsync(x => x.Id == riskIdCheck).Result;
                if (risk.RenewalFromRiskId.HasValue)
                {
                    riskIdCheck = risk.RenewalFromRiskId.Value;
                }
                else
                {
                    initialRiskId = riskIdCheck;
                    break;
                }
            }
            while (true);

            do
            {
                var risk = _riskRepository.FindAsync(x => x.Id == riskIdCheck).Result;
                if (risk.CurrentRiskDetailId.HasValue)
                {
                    relatedRiskIds.Add(new RiskRelatedDTO()
                    {
                        Id = riskIdCheck,
                        Current = risk.CurrentRiskDetailId,
                        PolicyNumber = risk.PolicyNumber,
                        IsPolicy = true
                    });
                }
                else
                {
                    var riskDetail = _riskDetailRepository.FindAsync(x => x.RiskId == riskIdCheck).Result;
                    relatedRiskIds.Add(new RiskRelatedDTO()
                    {
                        Id = riskIdCheck,
                        Current = riskDetail.Id,
                        PolicyNumber = risk.PolicyNumber,
                        IsPolicy = false
                    });
                }
                
                var riskRenewal = _riskRepository.FindAsync(x => x.RenewalFromRiskId == riskIdCheck && x.Status != RiskStatus.Canceled).Result;
                if (riskRenewal != null)
                    riskIdCheck = riskRenewal.Id;
                else
                    break;
            }
            while (true);

            return relatedRiskIds;
        }
    }
}
