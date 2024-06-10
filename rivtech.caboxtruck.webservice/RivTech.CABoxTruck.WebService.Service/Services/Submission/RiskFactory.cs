using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mono.TextTemplating;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Drivers;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using static System.Guid;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission
{
    public class RiskFactory : IRiskFactory
    {
        private readonly AppDbContext _context;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskResponseRepository _riskResponseRepository;
        private readonly IRiskAdditionalInterestRepository _riskAdditionalInterestRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskSequenceService _riskSequence;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IRiskFormRepository _riskFormRepository;
        private readonly IRiskHistoryRepository _riskHistoryRepository;
        private readonly IRiskSpecificSafetyDeviceRepository _riskSpecificSafetyDeviceRepository;
        private readonly IRiskSpecificDriverInfoRepository _riskSpecificDriverInfoRepository;
        private readonly IRiskSpecificDotInfoRepository _riskSpecificDotInfoRepository;
        private readonly IRiskSpecificDriverHiringCriteriaRepository _riskSpecificDriverHiringCriteriaRepository;
        private readonly IRiskNotesRepository _riskNotesRepository;
        private readonly IMapper _mapper;
        private readonly IFormsService _formsService;
        private readonly IFileUploadDocumentRepository _fileUploadDocumentRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly IBindingRequirementsRepository _bindingRequirementsRepository;
        private readonly IQuoteConditionsRepository _quoteConditionsRepository;
        private readonly IBindingService _bindingService;
        private readonly IClaimsHistoryRepository _claimsHistoryRepository;
        private readonly IRiskSpecificDestinationRepository _riskSpecificDestinationRepository;
        private readonly IEntitySubsidiaryRepository _entitySubsidiaryRepository;
        private readonly IBackgroundCheckOptionRepository _backgroundCheckOptionRepository;
        private readonly IDriverTrainingOptionRepository _driverTrainingOptionRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILimitsService _limitsService;
        private readonly IPolicyHistoryRepository _policyHistoryRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IFormRepository _formRepository;
        private readonly IDriverIncidentsRepository _driverIncidentsRepository;
        private readonly IRiskPolicyContactRepository _riskPolicyContactRepository;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskVehiclePremiumFactory _riskVehiclePremiumFactory;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IRiskPolicyPremiumFactory _riskPolicyPremiumFactory;
        private readonly IRiskFilingRepository _riskFilingRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IRiskStatusHistoryRepository _riskStatusHistoryRepository;
        private readonly string[] ValidSurplusLinesState = { "AL", "AZ", "CA", "CO", "DE", "GA", "IL", "KY", "MD", "NC", "NJ", "NV", "OH", "OR", "PA", "SC", "TN", "TX", "VA", "WA", "MO" };
        private List<string> _paths = new List<string>();
        private LimitListDTO _limitList = null;
        private static string currentCargoCoverageValue = string.Empty; // to temporarily hold value from risk coverage for later use on vehicle level
        private static string currentRefrigerationCoverageValue = string.Empty; // to temporarily hold value from risk coverage for later use on vehicle level
        private readonly IConfiguration _configuration;
        private readonly ISubmissionService _submissionService;
        private readonly IConfigurationSection _genericServiceSettings;
        private readonly string _genericBaseUrl;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IVehicleService _vehicleService;
        private readonly ILvLimitsRepository _lvLimitsRepository;
        private readonly IPolicyContactsService _policyContactsService;
        private readonly IListValueTextParser _listValueTextParser;
        public RiskFactory(
            AppDbContext context,
            IRiskDetailRepository riskDetailRepository,
            IRiskResponseRepository riskResponseRepository,
            IRiskAdditionalInterestRepository riskAdditionalInterestRepository,
            IEntityAddressRepository entityAddressRepository,
            IRiskRepository riskRepository,
            IRiskSequenceService riskSequence,
            IBrokerInfoRepository brokerInfoRepository,
            IRiskFormRepository riskFormRepository,
            IRiskHistoryRepository riskHistoryRepository,
            IRiskSpecificSafetyDeviceRepository riskSpecificSafetyDeviceRepository,
            IRiskSpecificDriverInfoRepository riskSpecificDriverInfoRepository,
            IRiskSpecificDotInfoRepository riskSpecificDotInfoRepository,
            IRiskSpecificDriverHiringCriteriaRepository riskSpecificDriverHiringCriteriaRepository,
            IRiskNotesRepository riskNotesRepository,
            IFormsService formsService,
            IFileUploadDocumentRepository fileUploadDocumentRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository,
            IBindingRequirementsRepository bindingRequirementsRepository,
            IQuoteConditionsRepository quoteConditionsRepository,
            IBindingService bindingService,
            IClaimsHistoryRepository claimsHistoryRepository,
            IRiskSpecificDestinationRepository riskSpecificDestinationRepository,
            IEntitySubsidiaryRepository entitySubsidiaryRepository,
            IBackgroundCheckOptionRepository backgroundCheckOptionRepository,
            IDriverTrainingOptionRepository driverTrainingOptionRepository,
            IVehicleRepository vehicleRepository,
            ILimitsService limitsService,
            IPolicyHistoryRepository policyHistoryRepository,
            IDriverRepository driverRepository,
            IFormRepository formRepository,
            IDriverIncidentsRepository driverIncidentsRepository,
            IRiskPolicyContactRepository riskPolicyContactRepository,
            IEndorsementChangesService endorsementChangesService,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskVehiclePremiumFactory riskVehiclePremiumFactory,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository,
            IRiskPolicyPremiumFactory riskPolicyPremiumFactory,
            IMapper mapper,
            IRiskFilingRepository riskFilingRepository,
            IAddressRepository addressRepository,
            IRiskCoverageRepository riskCoverageRepository,
            IRiskStatusHistoryRepository riskStatusHistoryRepository,
            IConfiguration configuration,
            ISubmissionService submissionService,
            IAgencyRepository agencyRepository,
            IVehicleService vehicleService,
            ILvLimitsRepository lvLimitsRepository,
            IPolicyContactsService policyContactsService,
            IListValueTextParser listValueTextParser
            )
        {
            _context = context;
            _riskDetailRepository = riskDetailRepository;
            _riskResponseRepository = riskResponseRepository;
            _riskAdditionalInterestRepository = riskAdditionalInterestRepository;
            _entityAddressRepository = entityAddressRepository;
            _riskRepository = riskRepository;
            _riskSequence = riskSequence;
            _brokerInfoRepository = brokerInfoRepository;
            _riskFormRepository = riskFormRepository;
            _riskHistoryRepository = riskHistoryRepository;
            _riskSpecificSafetyDeviceRepository = riskSpecificSafetyDeviceRepository;
            _riskSpecificDriverInfoRepository = riskSpecificDriverInfoRepository;
            _riskSpecificDotInfoRepository = riskSpecificDotInfoRepository;
            _riskSpecificDriverHiringCriteriaRepository = riskSpecificDriverHiringCriteriaRepository;
            _riskNotesRepository = riskNotesRepository;
            _formsService = formsService;
            _fileUploadDocumentRepository = fileUploadDocumentRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _bindingRequirementsRepository = bindingRequirementsRepository;
            _quoteConditionsRepository = quoteConditionsRepository;
            _bindingService = bindingService;
            _claimsHistoryRepository = claimsHistoryRepository;
            _riskSpecificDestinationRepository = riskSpecificDestinationRepository;
            _entitySubsidiaryRepository = entitySubsidiaryRepository;
            _backgroundCheckOptionRepository = backgroundCheckOptionRepository;
            _driverTrainingOptionRepository = driverTrainingOptionRepository;
            _vehicleRepository = vehicleRepository;
            _limitsService = limitsService;
            _policyHistoryRepository = policyHistoryRepository;
            _driverRepository = driverRepository;
            _formRepository = formRepository;
            _driverIncidentsRepository = driverIncidentsRepository;
            _riskPolicyContactRepository = riskPolicyContactRepository;
            _endorsementChangesService = endorsementChangesService;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskVehiclePremiumFactory = riskVehiclePremiumFactory;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
            _riskPolicyPremiumFactory = riskPolicyPremiumFactory;
            _mapper = mapper;
            _riskFilingRepository = riskFilingRepository;
            _addressRepository = addressRepository;
            _riskCoverageRepository = riskCoverageRepository;
            _riskStatusHistoryRepository = riskStatusHistoryRepository;
            _configuration = configuration;
            _submissionService = submissionService;
            _genericServiceSettings = _configuration.GetSection("GenericService");
            _genericBaseUrl = _genericServiceSettings.GetValue<string>("BaseUrl");
            _agencyRepository = agencyRepository;
            _vehicleService = vehicleService;
            _lvLimitsRepository = lvLimitsRepository;
            _policyContactsService = policyContactsService;
            _listValueTextParser = listValueTextParser;
        }

        public async Task<RiskDetail> GeneratePolicy(Guid riskDetailId, string policyNumber)
        {
            var newRiskDetailId = NewGuid();

            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;
            RiskCoverage boundRiskCoverage = toCloneRiskDetail.RiskCoverages.Single(rc => rc.OptionNumber == bindOption);

            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == riskDetailId);

            #region Risk
            var risk = await _riskRepository.FindAsync(x => x.Id == toCloneRiskDetail.RiskId);
            risk.PolicyNumber = policyNumber;
            risk.Status = DTO.Constants.RiskStatus.Active;
            #endregion

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = toCloneRiskDetail.Clone();
            newRiskDetail.Id = newRiskDetailId;
            newRiskDetail.Status = RiskDetailStatus.Active;
            newRiskDetail.CreatedDate = DateTime.Now;
            newRiskDetail.SourceRiskDetailId = riskDetailId;
            //newRiskDetail.FirstIssueDate = DateTime.Now;
            #endregion

            #region RiskResponse
            var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskResponses != null)
                newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.RiskDetailId = newRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            #region RiskFilings
            if (newRiskDetail.RiskFilings != null)
                newRiskDetail.RiskFilings.Select(x => (RiskFiling)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                var submissionRiskCoverage = await _riskCoverageRepository.FindAsync(boundRiskCoverage.Id);

                // Clone only the bound risk coverage.
                newRiskDetail.RiskCoverages = newRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).Select(x => (RiskCoverage)SetId(x, newRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = newRiskDetailId;
            #endregion

            #region CommoditiesHauled
            if (newRiskDetail.CommoditiesHauled != null)
                newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = newRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = newRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDotInfo != null)
            {
                newDotInfo.Id = newRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = newRiskDetailId;
            #endregion

            #region Insured Subsidiary
            //var addressIdMapping = new Dictionary<Guid, Guid>();
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;

                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }

                var newEntityAddresses = newRiskDetail.Insured.Entity.EntityAddresses.ToList();
                var newGaragingAddresses = newEntityAddresses.Where(x => x.AddressTypeId == (short?)AddressTypesEnum.Garaging);

                //foreach (var newGaragingAddress in newGaragingAddresses)
                //{
                //    newEntityAddresses = newEntityAddresses.Where(x => x != newGaragingAddress).ToList();
                //    var toCloneRiskDetailGaragagingAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newGaragingAddress.AddressId);
                //    var cloneGaragingAddress = toCloneRiskDetailGaragagingAddress?.Clone();

                //    cloneGaragingAddress.Id = Guid.Empty;
                //    cloneGaragingAddress.EntityId = newEntityId;
                //    cloneGaragingAddress.AddressId = toCloneRiskDetailGaragagingAddress.AddressId;
                //    cloneGaragingAddress.Address = null;
                //    newEntityAddresses.Add(cloneGaragingAddress);
                //}
                var newEntityAddressesToIssue = newEntityAddresses;
                newEntityAddresses = new List<EntityAddress>();

                foreach (var newEntityAddress in newEntityAddressesToIssue)
                {
                    var toCloneRiskDetailEntityAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newEntityAddress.AddressId);
                    var cloneEntityAddress = toCloneRiskDetailEntityAddress?.Clone();

                    cloneEntityAddress.Id = Guid.Empty;
                    cloneEntityAddress.EntityId = newEntityId;
                    cloneEntityAddress.AddressId = toCloneRiskDetailEntityAddress.AddressId;
                    cloneEntityAddress.Address = null;
                    newEntityAddresses.Add(cloneEntityAddress);
                }

                newRiskDetail.Insured.Entity.EntityAddresses = newEntityAddresses;
            }
            #endregion

            #region RiskHistory
            var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskHistories != null)
            {
                var riskDetailDrivers = await _context.RiskDetailDriver
                                        .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                                        .Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();
                var boundCoverages = newRiskDetail.RiskCoverages?.Where(riskCov => riskCov?.OptionNumber == bindOption)?.FirstOrDefault();
                var calculatedVehicles = await _context.VehiclePremiumRatingFactor.Where(vrf => vrf.OptionId == bindOption && vrf.RiskDetailId == riskDetailId).ToListAsync();
                foreach (var x in riskHistories)
                {
                    x.NumberOfDrivers = riskDetailDrivers?.Where(driver => ((driver.Driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count() > 10 ? x.NumberOfDrivers : null;
                    x.NumberOfSpareVehicles = calculatedVehicles?.Count() > 10 ? x.NumberOfSpareVehicles : null;
                    x.ComprehensiveDeductible = boundCoverages?.CompDeductibleId != 76 ? x.ComprehensiveDeductible : null;
                    x.CollisionDeductible = boundCoverages?.CollDeductibleId != 65 ? x.CollisionDeductible : null;
                    x.CargoLimits = boundCoverages?.CargoLimitId != 53 ? x.CargoLimits : null;
                    x.RefrigeratedCargoLimits = boundCoverages?.RefCargoLimitId != 157 ? x.RefrigeratedCargoLimits : null;

                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = newRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                    _riskHistoryRepository.Update(x);
                }
            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = newRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = newRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = newRiskDetailId;
                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.RiskDetailId = newRiskDetailId;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);
                }
            }
            #endregion

            #region BindingRequirements
            var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (bindingRequirements != null)
            {
                foreach (var x in bindingRequirements)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = fileUploadDocuments.Where(relatedDoc => relatedDoc.FileCategoryId == 4 && relatedDoc.tableRefId == x.Id).FirstOrDefault();
                    if (relatedDocuments != null)
                    {
                        relatedDocuments.tableRefId = newGuid;
                        _fileUploadDocumentRepository.Update(relatedDocuments);
                    }

                    var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
                    newBindingRequirements.Id = newGuid;
                    newBindingRequirements.RiskDetailId = newRiskDetailId;
                    await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
                }
            }
            #endregion

            #region QuoteConditions
            var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (quoteCond != null)
            {
                foreach (var x in quoteCond)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = fileUploadDocuments.Where(relatedDoc => relatedDoc.FileCategoryId == 5 && relatedDoc.tableRefId == x.Id).FirstOrDefault();
                    if (relatedDocuments != null)
                    {
                        relatedDocuments.tableRefId = newGuid;
                        _fileUploadDocumentRepository.Update(relatedDocuments);
                    }

                    var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
                    newQuoteCond.Id = newGuid;
                    newQuoteCond.RiskDetailId = newRiskDetailId;
                    await _quoteConditionsRepository.AddAsync(newQuoteCond);
                }
            }
            #endregion

            #region Claims
            var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = newRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = newRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            #region RiskPolicyContacts
            var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
            if (riskPolicyContacts != null)
            {
                foreach (var item in riskPolicyContacts)
                {
                    var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
                    newRiskPolicyContact.Id = Guid.NewGuid();
                    newRiskPolicyContact.RiskDetailId = newRiskDetailId;
                    newRiskPolicyContact.Entity.Id = Empty;
                    foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }
                    await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
                }
            }
            #endregion

            newRiskDetail.DriverInfo = null;

            // Set all documents to IsSystemGenerated = true. Disable editing and deletion
            if (fileUploadDocuments != null)
            {
                foreach (var item in fileUploadDocuments)
                {
                    item.IsSystemGenerated = true; // Set to true to disable editing and deletion after issuance.
                }
                _fileUploadDocumentRepository.UpdateRange(fileUploadDocuments);
            }

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            newRiskDetail.RiskDetailVehicles = toCloneRiskDetail.RiskDetailVehicles.Where(rdv => rdv.Vehicle.IsIncludedInOption(bindOption)).ToList();
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles
                // Include only those selected in bind option.
                .Where(rdv => rdv.Vehicle.Options != null && rdv.Vehicle.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())) && !rdv.Vehicle.IsDeleted)
                .ToList();

            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            // The 'newRiskDetail.RiskDetailDrivers' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailDrivers();

            await _riskDetailRepository.AddAsync(newRiskDetail);

            #region Vehicles
            var brokerInfo = risk.BrokerInfo;
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = _vehicleRepository.GetById(riskDetailVehicle.VehicleId);  // Get vehicle.
                var rdv = riskDetailVehicle.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                vehicle.RiskDetailVehicles.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                //vehicle.GaragingAddressId = addressIdMapping.GetValueOrDefault(vehicle.GaragingAddressId.Value, garagingIdDefault);

                _vehicleRepository.Update(vehicle);                                     // Update vehicle in db context.
            }
            await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region Drivers
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailDriver in oldRiskDetailDrivers)
            {
                var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.
                if (driver.Options != null && driver.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())) && !driver.IsDeleted)
                {
                    var rdv = riskDetailDriver.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                    driver.RiskDetailDrivers.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                    _driverRepository.Update(driver);                                     // Update drvr in db context.
                }
            }
            await _driverRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            await _riskDetailRepository.SaveChangesAsync();

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = newRiskDetailId;
                newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            #endregion


            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();

            await _riskFormRepository.SaveChangesAsync();
            await _riskManuscriptsRepository.SaveChangesAsync();
            await _riskHistoryRepository.SaveChangesAsync();
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _fileUploadDocumentRepository.SaveChangesAsync();
            await _bindingRequirementsRepository.SaveChangesAsync();
            await _quoteConditionsRepository.SaveChangesAsync();
            await _riskCoverageRepository.SaveChangesAsync();

            _context.SaveChanges();
            _context.ChangeTracker.Clear();


            #region RiskVehiclePremium
            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            foreach (var rvpColl in riskVehiclePremiums)
            {
                var cloneRvpColl = rvpColl.Clone();
                cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
                await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            }
            #endregion

            #region RiskPolicyPremium
            //General Liability Premium
            var riskPolicyGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            var cloneGLRpp = riskPolicyGLPremiums.Clone();
            cloneGLRpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneGLRpp);

            //Trailer Interchange Premium
            var riskPolicyTIPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_TI);
            var cloneTIRpp = riskPolicyTIPremiums.Clone();
            cloneTIRpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneTIRpp);

            //Non-Owned Trailer Premium
            var riskPolicyNOPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_NO);
            var cloneNORpp = riskPolicyNOPremiums.Clone();
            cloneNORpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneNORpp);
            #endregion

            return newRiskDetail;
        }

        public async Task<RiskDetail> GeneratePolicyDetailForEndorsement(Guid? previousRiskDetailId, DateTime effectiveDate, RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk risk, Guid boundRiskDetailId)
        {
            _context.ChangeTracker.Clear();

            var newRiskDetailId = NewGuid();

            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId.Value);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

            var riskDetailIds = (await _riskDetailRepository.GetAsync(x => x.RiskId == risk.Id))?.Select(r => r.Id);
            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => riskDetailIds.Contains((Guid)x.RiskDetailId));

            var previousManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == boundRiskDetailId);
            var previousAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == boundRiskDetailId);

            #region Risk
            var toCloneRisk = await _riskRepository.FindAsync(x => x.Id == toCloneRiskDetail.RiskId);
            risk.Status = DTO.Constants.RiskStatus.Active;
            #endregion

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = toCloneRiskDetail.Clone();
            newRiskDetail.Id = newRiskDetailId;
            newRiskDetail.Status = RiskDetailStatus.Active;
            newRiskDetail.CreatedDate = DateTime.Now;
            //newRiskDetail.FirstIssueDate = DateTime.Now;
            #endregion

            #region Policy History
            // update only existing histories to new riskdetail
            var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == risk.Id.ToString());
            if (policyHistories != null)
            {
                foreach (var policyHistory in policyHistories)
                {
                    policyHistory.RiskDetailId = newRiskDetailId;
                }
                _policyHistoryRepository.UpdateRange(policyHistories);
            }
            #endregion PolicyHistory

            #region RiskResponse
            var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (riskResponses != null)
                newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(previousRiskDetailId.Value);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var isNewlyAdded = previousAdditionalInterest?.All(oldObj => oldObj.AddProcessDate != item.AddProcessDate) ?? true;
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.EffectiveDate = (item.RemoveProcessDate == null) && isNewlyAdded ? effectiveDate : item.EffectiveDate;
                    newAdditionalInterest.ExpirationDate = (item.RemoveProcessDate >= policyHistories.LastOrDefault().DateCreated.Value) ? effectiveDate : item.ExpirationDate;
                    newAdditionalInterest.RiskDetailId = newRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    // update current additional interests to prevent on showing in policy changes after issuance
                    item.EffectiveDate = newAdditionalInterest.EffectiveDate;
                    item.ExpirationDate = newAdditionalInterest.ExpirationDate;
                    _riskAdditionalInterestRepository.Update(item);
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            //#region RiskDetailDriver
            //if (newRiskDetail.RiskDetailDrivers != null)
            //{
            //    var riskDetailDrivers = await _context.RiskDetailDriver
            //        .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
            //        .Where(x => x.RiskDetailId == riskDetailId).ToListAsync();
            //        //.Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();

            //    foreach (var x in riskDetailDrivers)
            //    {
            //        var newRiskDetailDriver = SystemExtension.Clone<RiskDetailDriver>(x);
            //        newRiskDetailDriver.RiskDetailId = newRiskDetailId;
            //        await _context.RiskDetailDriver.AddAsync(newRiskDetailDriver);
            //    }
            //}
            //newRiskDetail.RiskDetailDrivers = null;
            //#endregion

            #region RiskFilings
            if (newRiskDetail.RiskFilings != null)
                newRiskDetail.RiskFilings.Select(x => (RiskFiling)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                newRiskDetail.RiskCoverages.Select(x => (RiskCoverage)SetId(x, newRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        item.RiskCoveragePremium.RiskCoverageId = item.Id;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = newRiskDetailId;
            #endregion

            #region CommoditiesHauled
            if (newRiskDetail.CommoditiesHauled != null)
                newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = newRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(previousRiskDetailId.Value);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = newRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == previousRiskDetailId.Value);
            if (newDotInfo != null)
            {
                newDotInfo.Id = newRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = newRiskDetailId;
            #endregion

            #region Insured Subsidiary
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;
                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }

                var newEntityAddresses = newRiskDetail.Insured.Entity.EntityAddresses.ToList();
                var newGaragingAddresses = newEntityAddresses.Where(x => x.AddressTypeId == (short?)AddressTypesEnum.Garaging);

                //foreach (var newGaragingAddress in newGaragingAddresses)
                //{
                //    newEntityAddresses = newEntityAddresses.Where(x => x != newGaragingAddress).ToList();
                //    var toCloneRiskDetailGaragagingAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newGaragingAddress.AddressId);
                //    var cloneGaragingAddress = toCloneRiskDetailGaragagingAddress?.Clone();

                //    cloneGaragingAddress.Id = Guid.Empty;
                //    cloneGaragingAddress.EntityId = newEntityId;
                //    cloneGaragingAddress.AddressId = toCloneRiskDetailGaragagingAddress.AddressId;
                //    cloneGaragingAddress.Address = null;
                //    newEntityAddresses.Add(cloneGaragingAddress);
                //}
                var newEntityAddressesToIssue = newEntityAddresses;
                newEntityAddresses = new List<EntityAddress>();

                foreach (var newEntityAddress in newEntityAddressesToIssue)
                {
                    var toCloneRiskDetailEntityAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newEntityAddress.AddressId);
                    var cloneEntityAddress = toCloneRiskDetailEntityAddress?.Clone();

                    cloneEntityAddress.Id = Guid.Empty;
                    cloneEntityAddress.EntityId = newEntityId;
                    cloneEntityAddress.AddressId = toCloneRiskDetailEntityAddress.AddressId;
                    cloneEntityAddress.Address = null;
                    newEntityAddresses.Add(cloneEntityAddress);
                }

                newRiskDetail.Insured.Entity.EntityAddresses = newEntityAddresses;
            }
            #endregion

            #region RiskHistory
            var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (riskHistories != null)
            {
                var riskDetailDrivers = await _context.RiskDetailDriver
                                        .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                                        .Where(x => x.RiskDetailId == previousRiskDetailId.Value && !x.Driver.DeletedDate.HasValue).ToListAsync();
                var boundCoverages = newRiskDetail.RiskCoverages?.Where(riskCov => riskCov?.OptionNumber == bindOption)?.FirstOrDefault();
                var calculatedVehicles = await _context.VehiclePremiumRatingFactor.Where(vrf => vrf.OptionId == bindOption && vrf.RiskDetailId == previousRiskDetailId.Value).ToListAsync();
                foreach (var x in riskHistories)
                {
                    x.NumberOfDrivers = riskDetailDrivers?.Where(driver => ((driver.Driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count() > 10 ? x.NumberOfDrivers : null;
                    x.NumberOfSpareVehicles = calculatedVehicles?.Count() > 10 ? x.NumberOfSpareVehicles : null;
                    x.ComprehensiveDeductible = boundCoverages?.CompDeductibleId != 76 ? x.ComprehensiveDeductible : null;
                    x.CollisionDeductible = boundCoverages?.CollDeductibleId != 65 ? x.CollisionDeductible : null;
                    x.CargoLimits = boundCoverages?.CargoLimitId != 53 ? x.CargoLimits : null;
                    x.RefrigeratedCargoLimits = boundCoverages?.RefCargoLimitId != 157 ? x.RefrigeratedCargoLimits : null;

                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = newRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                    _riskHistoryRepository.Update(x);
                }
            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = newRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = newRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = newRiskDetailId;
                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var isNewlyAdded = previousManuscripts?.All(oldObj => oldObj.CreatedDate != x.CreatedDate) ?? true;
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.EffectiveDate = (x.IsProrate && x.DeletedDate == null) && isNewlyAdded ? effectiveDate : x.EffectiveDate;
                    newManuscripts.ExpirationDate = (x.IsProrate && x.Premium != x.ProratedPremium && x.DeletedDate >= policyHistories.LastOrDefault().DateCreated.Value) ? effectiveDate : x.ExpirationDate;
                    newManuscripts.RiskDetailId = newRiskDetailId;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);

                    if (isNewlyAdded)
                    {
                        x.EffectiveDate = (x.IsProrate && x.DeletedDate == null) ? effectiveDate : x.EffectiveDate;
                        _riskManuscriptsRepository.Update(x);
                    }
                }
            }
            #endregion

            #region BindingRequirements
            var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (bindingRequirements != null)
            {
                foreach (var x in bindingRequirements)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = fileUploadDocuments.Where(relatedDoc => relatedDoc.FileCategoryId == 4 && relatedDoc.tableRefId == x.Id).FirstOrDefault();
                    if (relatedDocuments != null)
                    {
                        relatedDocuments.tableRefId = newGuid;
                        _fileUploadDocumentRepository.Update(relatedDocuments);
                    }

                    var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
                    newBindingRequirements.Id = newGuid;
                    newBindingRequirements.RiskDetailId = newRiskDetailId;
                    await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
                }
            }
            #endregion

            #region QuoteConditions
            var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (quoteCond != null)
            {
                foreach (var x in quoteCond)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = fileUploadDocuments.Where(relatedDoc => relatedDoc.FileCategoryId == 5 && relatedDoc.tableRefId == x.Id).FirstOrDefault();
                    if (relatedDocuments != null)
                    {
                        relatedDocuments.tableRefId = newGuid;
                        _fileUploadDocumentRepository.Update(relatedDocuments);
                    }

                    var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
                    newQuoteCond.Id = newGuid;
                    newQuoteCond.RiskDetailId = newRiskDetailId;
                    await _quoteConditionsRepository.AddAsync(newQuoteCond);
                }
            }
            #endregion

            #region Claims
            var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = newRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId.Value);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = newRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            #region FileUploadDocuments
            // Set all documents to IsSystemGenerated = true. Disable editing and deletion
            if (fileUploadDocuments != null)
            {
                foreach (var item in fileUploadDocuments)
                {
                    item.IsSystemGenerated = true; // Set to true to disable editing and deletion after issuance.
                }
                _fileUploadDocumentRepository.UpdateRange(fileUploadDocuments);
            }
            #endregion

            #region RiskPolicyContacts
            var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(previousRiskDetailId.Value);
            if (riskPolicyContacts != null)
            {
                foreach (var item in riskPolicyContacts)
                {
                    var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
                    newRiskPolicyContact.Id = Guid.NewGuid();
                    newRiskPolicyContact.RiskDetailId = newRiskDetailId;
                    newRiskPolicyContact.Entity.Id = Empty;
                    foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }
                    await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
                }
            }
            #endregion

            newRiskDetail.DriverInfo = null;

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers.ToList();
            newRiskDetail.ClearRiskDetailDrivers();

            await _riskDetailRepository.AddAsync(newRiskDetail);

            #region Vehicles
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = _vehicleRepository.GetById(riskDetailVehicle.VehicleId);  // Get vehicle.
                var rdv = riskDetailVehicle.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                vehicle.RiskDetailVehicles.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _vehicleRepository.Update(vehicle);                                     // Update vehicle in db context.
            }
            await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion Vehicles

            #region Drivers
            foreach (var riskDetailDriver in oldRiskDetailDrivers)
            {
                var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.
                var rdv = riskDetailDriver.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                driver.RiskDetailDrivers.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _driverRepository.Update(driver);                                     // Update drvr in db context.
            }
            await _driverRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == previousRiskDetailId.Value);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = newRiskDetailId;
                newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            newRiskDetail.DriverInfo = null;
            #endregion

            await _riskDetailRepository.SaveChangesAsync();

            await _riskFormRepository.SaveChangesAsync();
            await _riskManuscriptsRepository.SaveChangesAsync();
            await _riskHistoryRepository.SaveChangesAsync();
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _fileUploadDocumentRepository.SaveChangesAsync();
            await _bindingRequirementsRepository.SaveChangesAsync();
            await _quoteConditionsRepository.SaveChangesAsync();
            await _policyHistoryRepository.SaveChangesAsync();

            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            #region RiskVehiclePremium
            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            foreach (var rvpColl in riskVehiclePremiums)
            {
                var cloneRvpColl = rvpColl.Clone();
                cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
                await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            }
            #endregion

            #region RiskPolicyPremium
            //General Liability Premium
            var riskPolicyGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            var cloneGLRpp = riskPolicyGLPremiums.Clone();
            cloneGLRpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneGLRpp);

            //Trailer Interchange Premium
            var riskPolicyTIPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_TI);
            var cloneTIRpp = riskPolicyTIPremiums.Clone();
            cloneTIRpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneTIRpp);

            //Non-Owned Trailer Premium
            var riskPolicyNOPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_NO);
            var cloneNORpp = riskPolicyNOPremiums.Clone();
            cloneNORpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneNORpp);
            #endregion

            return newRiskDetail;
        }

        public async Task<RiskDetail> GeneratePolicyDetailForCancellation(Guid currentRiskDetailId, Guid riskId, DateTime effectiveDate)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == riskId && riskDetail.Id != currentRiskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            //var riskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var riskDetailId = risk.CurrentRiskDetailId.Value;
            //var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

            // save file upload documents data first
            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId);

            var insuredAddress = await _riskDetailRepository.GetInsuredAddressAsync(currentRiskDetailId);
            var currentAddresses = insuredAddress.Insured.Entity.EntityAddresses;

            var currentRiskStatusHistory = await _riskStatusHistoryRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId);

            // delete current policy details
            await DeleteCurrentPolicyDetail(currentRiskDetailId, riskDetailId);

            if (currentRiskStatusHistory != null && currentRiskStatusHistory.Count() > 0)
            {
                await _riskStatusHistoryRepository.AddRangeAsync(currentRiskStatusHistory);
            }

            #region Risk
            risk.Status = DTO.Constants.RiskStatus.Canceled;
            #endregion

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            //newRiskDetail = SystemExtension.Clone<RiskDetail>(toCloneRiskDetail);
            newRiskDetail = toCloneRiskDetail.DeepClone();
            newRiskDetail.Id = currentRiskDetailId;
            newRiskDetail.Status = RiskDetailStatus.Canceled;
            newRiskDetail.CreatedDate = DateTime.Now;
            newRiskDetail.CancellationDate = effectiveDate;
            //newRiskDetail.FirstIssueDate = DateTime.Now;
            #endregion

            #region RiskResponse
            var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskResponses != null)
                newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, currentRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.RiskDetailId = currentRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    newAdditionalInterest.ExpirationDate = effectiveDate;
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            #region RiskFilings
            var riskFilings = await _riskFilingRepository.GetAsync(x => x.RiskDetailId == risk.CurrentRiskDetailId.Value);
            if (riskFilings != null)
            {
                newRiskDetail.RiskFilings = new List<RiskFiling>();
                foreach (var riskFiling in riskFilings)
                {
                    riskFiling.Id = Guid.Empty;
                    riskFiling.RiskDetailId = currentRiskDetailId;
                    newRiskDetail.RiskFilings.Add(riskFiling);
                }
            }
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                newRiskDetail.RiskCoverages = newRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).Select(x => (RiskCoverage)SetId(x, currentRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = currentRiskDetailId;
            #endregion

            #region CommoditiesHauled
            if (newRiskDetail.CommoditiesHauled != null)
                newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, currentRiskDetailId)).ToList();
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = currentRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = currentRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDotInfo != null)
            {
                newDotInfo.Id = currentRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = currentRiskDetailId;
            #endregion

            #region Insured Subsidiary
            var addressIdMapping = new Dictionary<Guid, Guid>();
            newRiskDetail.Insured = SystemExtension.Clone<Insured>(toCloneRiskDetail.Insured);
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;
                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }

                var newEntityAddresses = newRiskDetail.Insured.Entity.EntityAddresses.ToList();
                var newGaragingAddresses = newEntityAddresses.Where(x => x.AddressTypeId == (short?)AddressTypesEnum.Garaging);

                //foreach (var newGaragingAddress in newGaragingAddresses)
                //{
                //    newEntityAddresses = newEntityAddresses.Where(x => x != newGaragingAddress).ToList();
                //    var toCloneRiskDetailGaragagingAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newGaragingAddress.AddressId);
                //    var cloneGaragingAddress = toCloneRiskDetailGaragagingAddress?.Clone();

                //    cloneGaragingAddress.Id = Guid.Empty;
                //    cloneGaragingAddress.EntityId = newEntityId;
                //    cloneGaragingAddress.AddressId = toCloneRiskDetailGaragagingAddress.AddressId;
                //    cloneGaragingAddress.Address = null;
                //    newEntityAddresses.Add(cloneGaragingAddress);
                //}
                var newEntityAddressesToIssue = newEntityAddresses;
                newEntityAddresses = new List<EntityAddress>();

                foreach (var newEntityAddress in newEntityAddressesToIssue)
                {
                    var toCloneRiskDetailEntityAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newEntityAddress.AddressId);
                    var cloneEntityAddress = toCloneRiskDetailEntityAddress?.Clone();

                    cloneEntityAddress.Id = Guid.Empty;
                    cloneEntityAddress.EntityId = newEntityId;
                    cloneEntityAddress.AddressId = toCloneRiskDetailEntityAddress.AddressId;
                    cloneEntityAddress.Address = null;
                    newEntityAddresses.Add(cloneEntityAddress);
                }

                newRiskDetail.Insured.Entity.EntityAddresses = newEntityAddresses;
            }
            #endregion

            #region RiskHistory
            var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskHistories != null)
            {
                foreach (var x in riskHistories)
                {
                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = currentRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                }

            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = currentRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = currentRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = currentRiskDetailId;
                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.RiskDetailId = currentRiskDetailId;
                    newManuscripts.ExpirationDate = (x.IsProrate && !x.IsDeleted) ? effectiveDate : x.ExpirationDate;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);
                }
            }
            #endregion

            #region BindingRequirements
            // should not include bind requirements and QC
            //var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            //if (bindingRequirements != null)
            //{
            //    foreach (var x in bindingRequirements)
            //    {
            //        var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
            //        newBindingRequirements.Id = Empty;
            //        newBindingRequirements.RiskDetailId = currentRiskDetailId;
            //        await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
            //    }
            //}
            #endregion

            #region QuoteConditions
            //var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            //if (quoteCond != null)
            //{
            //    foreach (var x in quoteCond)
            //    {
            //        var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
            //        newQuoteCond.Id = Empty;
            //        newQuoteCond.RiskDetailId = currentRiskDetailId;
            //        await _quoteConditionsRepository.AddAsync(newQuoteCond);
            //    }
            //}
            #endregion

            #region Claims
            var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = currentRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = currentRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            #region RiskPolicyContacts
            // Disabled by Krist Iann Tablan to fix NEWCABT-2089.
            // Reason: Cancellation reuses the current risk detail id. With this enabled only duplicating the record.
            //var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
            //if (riskPolicyContacts != null)
            //{
            //    foreach (var item in riskPolicyContacts)
            //    {
            //        var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
            //        newRiskPolicyContact.Id = Guid.NewGuid();
            //        newRiskPolicyContact.RiskDetailId = currentRiskDetailId;
            //        newRiskPolicyContact.Entity.Id = Empty;
            //        foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
            //        {
            //            x.Id = Empty;
            //            x.Address.Id = Empty;
            //        }
            //        await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
            //    }
            //}
            #endregion

            newRiskDetail.DriverInfo = null;

            if (fileUploadDocuments != null)
            {
                foreach (var item in fileUploadDocuments)
                {
                    item.RiskDetailId = riskDetailId;
                }
                await _fileUploadDocumentRepository.AddRangeAsync(fileUploadDocuments);
            }

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailDrivers();

            await _riskDetailRepository.AddAsync(newRiskDetail);

            #region Vehicles
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = _vehicleRepository.GetById(riskDetailVehicle.VehicleId);  // Get vehicle.
                var clonedVehicle = vehicle.CloneForEndorsement();
                clonedVehicle.ClearRiskDetailVehicles();
                await _vehicleRepository.AddAsync(clonedVehicle);                                     // Update vehicle in db context.
                if (clonedVehicle.PreviousVehicleVersionId != null)
                {
                    var previousVersion = _vehicleRepository.GetById(clonedVehicle.PreviousVehicleVersionId.Value); // get previous version
                    clonedVehicle.PreviousVehicleVersionId = (previousVersion is null) ? null : clonedVehicle.PreviousVehicleVersionId; // check previous version if deleted already
                }
                var vehicleRatingFactors = _context.VehiclePremiumRatingFactor.Where(x => x.VehicleId == clonedVehicle.Id.ToString()).ToList(); // get vehicle rating factors
                foreach (var vrf in vehicleRatingFactors)
                {
                    var newVehicleRatingFactor = SystemExtension.Clone<VehiclePremiumRatingFactor>(vrf);
                    newVehicleRatingFactor.Id = 0;
                    newVehicleRatingFactor.VehicleId = clonedVehicle.Id.ToString();
                    newVehicleRatingFactor.RiskDetailId = newRiskDetail.Id;
                    _context.VehiclePremiumRatingFactor.Add(newVehicleRatingFactor);
                }
                var rdv = new RiskDetailVehicle(newRiskDetail, clonedVehicle);           // Create new RiskDetailVehicle with updated RiskDetail.
                clonedVehicle.RiskDetailVehicles.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                if (vehicle.ExpirationDate > effectiveDate)
                {
                    clonedVehicle.ExpirationDate = effectiveDate;                                 // set expiration to cancellation date
                }
            }
            await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region Drivers
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailDriver in oldRiskDetailDrivers)
            {
                var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.

                if (driver.IsDeleted)
                {
                    var previousVersions = await _driverRepository.GetPreviousRiskDetailDrivers(newRiskDetail.RiskId); // get previous versions
                    var isDriverAPreviousVersion = previousVersions.Any(x => x.Id == driver.Id);
                    if (isDriverAPreviousVersion == false)
                    {
                        driver.IsActive = null; // re-enable record
                        driver.DeletedDate = null; // re-enable record
                    }
                }

                bool hasBindOptionSelected = driver.Options != null && driver.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())) && !driver.IsDeleted;
                if (!hasBindOptionSelected && toCloneRiskDetail.Status == RiskDetailStatus.Quoted) continue; // any driver not bound from submision should not be carried over to policy

                var rdv = riskDetailDriver.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                driver.RiskDetailDrivers.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _driverRepository.Update(driver);                                     // Update drvr in db context.
            }
            await _driverRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = currentRiskDetailId;
                newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            #endregion

            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();

            await _riskDetailRepository.SaveChangesAsync();

            await _riskFormRepository.SaveChangesAsync();
            await _riskManuscriptsRepository.SaveChangesAsync();
            await _riskHistoryRepository.SaveChangesAsync();
            await _claimsHistoryRepository.SaveChangesAsync();
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _fileUploadDocumentRepository.SaveChangesAsync();
            await _quoteConditionsRepository.SaveChangesAsync();
            await _riskStatusHistoryRepository.SaveChangesAsync();
            _context.SaveChanges();


            #region RiskVehiclePremium
            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            foreach (var rvpColl in riskVehiclePremiums)
            {
                var cloneRvpColl = rvpColl.Clone();
                cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
                await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            }
            #endregion

            #region RiskPolicyPremium (GL)
            var riskPolicyPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            var cloneRpp = riskPolicyPremiums.Clone();
            cloneRpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneRpp);
            #endregion

            #region RiskPolicyPremium (TI)
            var riskPolicyPremiumsTI = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_TI);
            var cloneRppTI = riskPolicyPremiumsTI.Clone();
            cloneRppTI.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneRppTI);
            #endregion

            #region RiskPolicyPremium (NO)
            var riskPolicyPremiumsNO = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_NO);
            var cloneRppNO = riskPolicyPremiumsNO.Clone();
            cloneRppNO.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneRppNO);
            #endregion

            return newRiskDetail;
        }

        public async Task<RiskDetail> GeneratePolicyDetailForReinstatement(Guid currentRiskDetailId, Guid riskId)
        {
            _context.ChangeTracker.Clear();
            var risk = await _riskRepository.FindAsync(riskId);

            var newRiskDetailId = NewGuid();

            var riskDetailId = currentRiskDetailId;
            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;
            
            var riskDetailIds = (await _riskDetailRepository.GetAsync(x => x.RiskId == riskId))?.Select(r => r.Id);
            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => riskDetailIds.Contains((Guid)x.RiskDetailId));

            var insuredAddress = await _riskDetailRepository.GetInsuredAddressAsync(riskDetailId);
            var currentAddresses = insuredAddress.Insured.Entity.EntityAddresses;

            #region Risk
            risk.Status = DTO.Constants.RiskStatus.Canceled;

            // Clear PendingCancellationDate
            risk.PendingCancellationDate = null;
            risk.PendingCancellationNoticeDate = null;
            risk.CancellationReasonType = null;

            _riskRepository.Update(risk);
            #endregion

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = SystemExtension.Clone<RiskDetail>(toCloneRiskDetail);
            newRiskDetail.Id = newRiskDetailId;
            newRiskDetail.Status = RiskDetailStatus.Active;
            newRiskDetail.CreatedDate = DateTime.Now;
            //newRiskDetail.FirstIssueDate = DateTime.Now;
            #endregion

            #region Policy History
            // update only existing histories to new riskdetail
            var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == riskId.ToString()); // should use risk id
            if (policyHistories != null)
            {
                foreach (var policyHistory in policyHistories)
                {
                    policyHistory.RiskDetailId = newRiskDetailId;
                }
                _policyHistoryRepository.UpdateRange(policyHistories);
            }
            #endregion PolicyHistory

            #region RiskResponse
            var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskResponses != null)
                newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.RiskDetailId = newRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            #region RiskDetailDriver
            if (newRiskDetail.RiskDetailDrivers != null)
            {
                var riskDetailDrivers = await _context.RiskDetailDriver
                    .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                    .Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();

                foreach (var x in riskDetailDrivers)
                {
                    var newRiskDetailDriver = SystemExtension.Clone<RiskDetailDriver>(x);
                    newRiskDetailDriver.RiskDetailId = newRiskDetailId;
                    await _context.RiskDetailDriver.AddAsync(newRiskDetailDriver);
                }
            }
            newRiskDetail.RiskDetailDrivers = null;
            #endregion

            #region RiskFilings
            if (newRiskDetail.RiskFilings != null)
                newRiskDetail.RiskFilings.Select(x => (RiskFiling)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                newRiskDetail.RiskCoverages = newRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).Select(x => (RiskCoverage)SetId(x, newRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = newRiskDetailId;
            #endregion

            #region CommoditiesHauled
            if (newRiskDetail.CommoditiesHauled != null)
                newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = newRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = newRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = newRiskDetailId;
                newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            newRiskDetail.DriverInfo = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDotInfo != null)
            {
                newDotInfo.Id = newRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = newRiskDetailId;
            #endregion

            #region Insured Subsidiary
            var addressIdMapping = new Dictionary<Guid, Guid>();
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;
                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }

                var newEntityAddresses = newRiskDetail.Insured.Entity.EntityAddresses.ToList();
                var newGaragingAddresses = newEntityAddresses.Where(x => x.AddressTypeId == (short?)AddressTypesEnum.Garaging);

                //foreach (var newGaragingAddress in newGaragingAddresses)
                //{
                //    newEntityAddresses = newEntityAddresses.Where(x => x != newGaragingAddress).ToList();
                //    var toCloneRiskDetailGaragagingAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newGaragingAddress.AddressId);
                //    var cloneGaragingAddress = toCloneRiskDetailGaragagingAddress?.Clone();

                //    cloneGaragingAddress.Id = Guid.Empty;
                //    cloneGaragingAddress.EntityId = newEntityId;
                //    cloneGaragingAddress.AddressId = toCloneRiskDetailGaragagingAddress.AddressId;
                //    cloneGaragingAddress.Address = null;
                //    newEntityAddresses.Add(cloneGaragingAddress);
                //}
                var newEntityAddressesToIssue = newEntityAddresses;
                newEntityAddresses = new List<EntityAddress>();

                foreach (var newEntityAddress in newEntityAddressesToIssue)
                {
                    var toCloneRiskDetailEntityAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newEntityAddress.AddressId);
                    var cloneEntityAddress = toCloneRiskDetailEntityAddress?.Clone();

                    cloneEntityAddress.Id = Guid.Empty;
                    cloneEntityAddress.EntityId = newEntityId;
                    cloneEntityAddress.AddressId = toCloneRiskDetailEntityAddress.AddressId;
                    cloneEntityAddress.Address = null;
                    newEntityAddresses.Add(cloneEntityAddress);
                }

                newRiskDetail.Insured.Entity.EntityAddresses = newEntityAddresses;
            }
            #endregion

            #region RiskHistory
            var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskHistories != null)
            {
                var riskDetailDrivers = await _context.RiskDetailDriver
                                        .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                                        .Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();
                var boundCoverages = newRiskDetail.RiskCoverages?.Where(riskCov => riskCov?.OptionNumber == bindOption)?.FirstOrDefault();
                var calculatedVehicles = await _context.VehiclePremiumRatingFactor.Where(vrf => (vrf.OptionId == bindOption || vrf.OptionId == 0) && vrf.RiskDetailId == riskDetailId).ToListAsync();
                foreach (var x in riskHistories)
                {
                    x.NumberOfDrivers = riskDetailDrivers?.Where(driver => ((driver.Driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count() > 10 ? x.NumberOfDrivers : null;
                    x.NumberOfSpareVehicles = calculatedVehicles?.Count() > 10 ? x.NumberOfSpareVehicles : null;
                    x.ComprehensiveDeductible = boundCoverages?.CompDeductibleId != 76 ? x.ComprehensiveDeductible : null;
                    x.CollisionDeductible = boundCoverages?.CollDeductibleId != 65 ? x.CollisionDeductible : null;
                    x.CargoLimits = boundCoverages?.CargoLimitId != 53 ? x.CargoLimits : null;
                    x.RefrigeratedCargoLimits = boundCoverages?.RefCargoLimitId != 157 ? x.RefrigeratedCargoLimits : null;

                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = newRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                    _riskHistoryRepository.Update(x);
                }
            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = newRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = newRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = newRiskDetailId;
                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.EffectiveDate = x.EffectiveDate;
                    newManuscripts.ExpirationDate = x.ExpirationDate;
                    newManuscripts.RiskDetailId = newRiskDetailId;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);
                }
            }
            #endregion

            #region BindingRequirements
            var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId);
            if (bindingRequirements != null)
            {
                foreach (var x in bindingRequirements)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = fileUploadDocuments.Where(relatedDoc => relatedDoc.FileCategoryId == 4 && relatedDoc.tableRefId == x.Id).FirstOrDefault();
                    if (relatedDocuments != null)
                    {
                        relatedDocuments.tableRefId = newGuid;
                        _fileUploadDocumentRepository.Update(relatedDocuments);
                    }

                    var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
                    newBindingRequirements.Id = newGuid;
                    newBindingRequirements.RiskDetailId = newRiskDetailId;
                    await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
                }
            }
            #endregion

            #region QuoteConditions
            var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId);
            if (quoteCond != null)
            {
                foreach (var x in quoteCond)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = fileUploadDocuments.Where(relatedDoc => relatedDoc.FileCategoryId == 5 && relatedDoc.tableRefId == x.Id).FirstOrDefault();
                    if (relatedDocuments != null)
                    {
                        relatedDocuments.tableRefId = newGuid;
                        _fileUploadDocumentRepository.Update(relatedDocuments);
                    }

                    var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
                    newQuoteCond.Id = newGuid;
                    newQuoteCond.RiskDetailId = newRiskDetailId;
                    await _quoteConditionsRepository.AddAsync(newQuoteCond);
                }
            }
            #endregion

            #region Claims
            var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = newRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = newRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            #region RiskPolicyContacts
            var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
            if (riskPolicyContacts != null)
            {
                foreach (var item in riskPolicyContacts)
                {
                    var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
                    newRiskPolicyContact.Id = Guid.NewGuid();
                    newRiskPolicyContact.RiskDetailId = newRiskDetailId;
                    newRiskPolicyContact.Entity.Id = Empty;
                    foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }
                    await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
                }
            }
            #endregion

            // Set all documents to IsSystemGenerated = true. Disable editing and deletion
            if (fileUploadDocuments != null)
            {
                foreach (var item in fileUploadDocuments)
                {
                    item.IsSystemGenerated = true; // Set to true to disable editing and deletion after issuance.
                }
                _fileUploadDocumentRepository.UpdateRange(fileUploadDocuments);
            }

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            await _riskDetailRepository.AddAsync(newRiskDetail);

            #region Vehicles
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = _vehicleRepository.GetById(riskDetailVehicle.VehicleId);  // Get vehicle.
                var vehicleRatingFactors = _context.VehiclePremiumRatingFactor.Where(x => x.VehicleId == vehicle.Id.ToString()).ToList(); // get vehicle rating factors
                foreach (var vrf in vehicleRatingFactors)
                {
                    var newVehicleRatingFactor = SystemExtension.Clone<VehiclePremiumRatingFactor>(vrf);
                    newVehicleRatingFactor.Id = 0;
                    newVehicleRatingFactor.VehicleId = vehicle.Id.ToString();
                    newVehicleRatingFactor.RiskDetailId = newRiskDetail.Id;
                    _context.VehiclePremiumRatingFactor.Add(newVehicleRatingFactor);
                }
                var rdv = riskDetailVehicle.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                vehicle.RiskDetailVehicles.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                // update rating factors
                //var boundRatingFactors = vehicleRatingFactors.Find(vrf => vrf.OptionId == bindOption || vrf.OptionId == 0); // 0 id are for policies.
                //vehicle.AutoLiabilityPremium = boundRatingFactors?.TotalALPremium ?? 0;
                //vehicle.CargoPremium = boundRatingFactors?.CargoPremium ?? 0;
                //vehicle.PhysicalDamagePremium = boundRatingFactors?.TotalPDPremium ?? 0;
                //vehicle.ComprehensivePremium = (boundRatingFactors?.CompPremium ?? 0) + (boundRatingFactors?.FireTheftPremium ?? 0);
                //vehicle.CollisionPremium = boundRatingFactors?.CollisionPremium ?? 0;
                //vehicle.TotalPremium = boundRatingFactors?.PerVehiclePremium ?? 0;
                //vehicle.Radius = boundRatingFactors?.Radius ?? 0;
                //vehicle.RadiusTerritory = boundRatingFactors?.RadiusTerritory ?? 0;
                //vehicle.GaragingAddressId = addressIdMapping.GetValueOrDefault(vehicle.GaragingAddressId.Value);
                _vehicleRepository.Update(vehicle);                                     // Update vehicle in db context.
            }
            await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion Vehicles

            await _riskRepository.SaveChangesAsync();

            await _riskDetailRepository.SaveChangesAsync();

            await _riskFormRepository.SaveChangesAsync();
            await _riskManuscriptsRepository.SaveChangesAsync();
            await _riskHistoryRepository.SaveChangesAsync();
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _fileUploadDocumentRepository.SaveChangesAsync();
            await _bindingRequirementsRepository.SaveChangesAsync();
            await _quoteConditionsRepository.SaveChangesAsync();
            await _policyHistoryRepository.SaveChangesAsync();
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            #region RiskVehiclePremium
            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            foreach (var rvpColl in riskVehiclePremiums)
            {
                var cloneRvpColl = rvpColl.Clone();
                cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
                await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            }
            #endregion

            #region RiskPolicyPremium (GL)
            var riskPolicyPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            var cloneRpp = riskPolicyPremiums.Clone();
            cloneRpp.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneRpp);
            #endregion

            #region RiskPolicyPremium (TI)
            var riskPolicyPremiumsTI = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_TI);
            var cloneRppTI = riskPolicyPremiumsTI.Clone();
            cloneRppTI.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneRppTI);
            #endregion

            #region RiskPolicyPremium (NO)
            var riskPolicyPremiumsNO = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_NO);
            var cloneRppNO = riskPolicyPremiumsNO.Clone();
            cloneRppNO.ChangeRiskDetail(newRiskDetail.Id);
            await _riskPolicyPremiumRepository.SaveAsync(cloneRppNO);
            #endregion

            return newRiskDetail;
        }

        public async Task<RiskDetail> GenerateSubmissionDetailFromPolicy(Guid currentRiskDetailId, Guid riskId, string newSubmissionNumber, string newQuoteNumber)
        {
            _context.ChangeTracker.Clear();

            var newRiskDetailId = Guid.NewGuid();
            var newRiskId = Guid.NewGuid();

            var risk = await _riskRepository.GetRiskByIdAsync(riskId);
            var riskDetailId = risk.CurrentRiskDetailId.Value;
            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;
            RiskCoverage boundRiskCoverage = toCloneRiskDetail.RiskCoverages.Single(rc => rc.OptionNumber == bindOption);

            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == riskDetailId);

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = toCloneRiskDetail.Clone();
            newRiskDetail.Id = newRiskDetailId;
            newRiskDetail.RiskId = newRiskId;
            newRiskDetail.SubmissionNumber = newSubmissionNumber;
            newRiskDetail.Status = RiskDetailStatus.Quoted;
            newRiskDetail.CreatedDate = DateTime.Now;
            newRiskDetail.IsProcessed = false;
            newRiskDetail.EndorsementNumber = 0;
            #endregion

            #region Risk
            var toCloneRisk = await _riskRepository.FindAsync(x => x.Id == toCloneRiskDetail.RiskId, i => i.BrokerInfo);
            _riskRepository.Detach(toCloneRisk);

            var newRisk = new Data.Entity.Submission.Risk();
            newRisk = toCloneRisk.Clone();
            newRisk.Id = newRiskId;
            newRisk.Status = DTO.Constants.RiskStatus.Pending;
            newRisk.SubmissionNumber = newSubmissionNumber;
            newRisk.QuoteNumber = newQuoteNumber;
            newRisk.PolicyNumber = null;
            newRisk.RiskDetails = new List<RiskDetail>();
            newRisk.Binding.Id = Guid.Empty;
            newRisk.BrokerInfo.Id = Guid.Empty;
            newRisk.RiskDetails.Add(newRiskDetail);
            newRisk.CurrentRiskDetailId = null;
            newRisk.ForEndorsementRiskDetailId = null;
            newRisk.CancellationReasonType = null;
            newRisk.PendingCancellationDate = null;
            newRisk.PendingCancellationNoticeDate = null;
            #endregion

            #region RiskResponse
            var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskResponses != null)
                newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.RiskDetailId = newRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            #region RiskFilings
            if (newRiskDetail.RiskFilings != null)
                newRiskDetail.RiskFilings.Select(x => (RiskFiling)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                // Clone only the bound risk coverage.
                newRiskDetail.RiskCoverages.Select(x => (RiskCoverage)SetId(x, newRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = newRiskDetailId;
            #endregion

            #region CommoditiesHauled
            if (newRiskDetail.CommoditiesHauled != null)
                newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = newRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = newRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDotInfo != null)
            {
                newDotInfo.Id = newRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = newRiskDetailId;
            #endregion

            #region Insured Subsidiary
            var addressIdMapping = new Dictionary<Guid, Guid>();
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;
                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    if (x.AddressTypeId == (short)AddressTypesEnum.Garaging)
                    {
                        addressIdMapping.Add(x.Address.Id, newAddressId);
                    }
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }
            }
            #endregion

            #region RiskHistory
            var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskHistories != null)
            {
                var riskDetailDrivers = await _context.RiskDetailDriver
                                        .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                                        .Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();
                var boundCoverages = newRiskDetail.RiskCoverages?.Where(riskCov => riskCov?.OptionNumber == bindOption)?.FirstOrDefault();
                var calculatedVehicles = await _context.VehiclePremiumRatingFactor.Where(vrf => vrf.OptionId == bindOption && vrf.RiskDetailId == riskDetailId).ToListAsync();
                foreach (var x in riskHistories)
                {
                    x.NumberOfDrivers = riskDetailDrivers?.Where(driver => ((driver.Driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count() > 10 ? x.NumberOfDrivers : null;
                    x.NumberOfSpareVehicles = calculatedVehicles?.Count() > 10 ? x.NumberOfSpareVehicles : null;
                    x.ComprehensiveDeductible = boundCoverages?.CompDeductibleId != 76 ? x.ComprehensiveDeductible : null;
                    x.CollisionDeductible = boundCoverages?.CollDeductibleId != 65 ? x.CollisionDeductible : null;
                    x.CargoLimits = boundCoverages?.CargoLimitId != 53 ? x.CargoLimits : null;
                    x.RefrigeratedCargoLimits = boundCoverages?.RefCargoLimitId != 157 ? x.RefrigeratedCargoLimits : null;

                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = newRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                    _riskHistoryRepository.Update(x);
                }
            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = newRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = newRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = newRiskDetailId;

                    //Clear Driver Exclusions
                    if (newRiskForms.FormId == "DriverExclusion")
                    {
                        newRiskForms.IsSelected = null;
                        newRiskForms.Other = null;
                    }

                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.RiskDetailId = newRiskDetailId;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);
                }
            }
            #endregion

            #region BindingRequirements
            var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId);
            if (bindingRequirements != null)
            {
                foreach (var x in bindingRequirements)
                {
                    var newGuid = Guid.NewGuid();

                    var relatedDocuments = await _fileUploadDocumentRepository.GetAsync(relatedDoc => relatedDoc.FileCategoryId == 4 && relatedDoc.tableRefId == x.Id);
                    if (relatedDocuments != null)
                    {
                        foreach (var fileDocument in relatedDocuments)
                        {
                            var newFileUploadDocument = SystemExtension.Clone(fileDocument);
                            newFileUploadDocument.Id = Guid.NewGuid();
                            newFileUploadDocument.RiskDetailId = newRiskDetailId;
                            newFileUploadDocument.tableRefId = newGuid;
                            await _fileUploadDocumentRepository.AddAsync(newFileUploadDocument);
                        }
                    }

                    var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
                    newBindingRequirements.Id = newGuid;
                    newBindingRequirements.RiskDetailId = newRiskDetailId;
                    await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
                }
            }
            #endregion

            #region QuoteConditions
            var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == risk.ForEndorsementRiskDetailId);
            if (quoteCond != null)
            {
                foreach (var x in quoteCond)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = await _fileUploadDocumentRepository.GetAsync(relatedDoc => relatedDoc.FileCategoryId == 5 && relatedDoc.tableRefId == x.Id);
                    if (relatedDocuments != null)
                    {
                        foreach (var fileDocument in relatedDocuments)
                        {
                            var newFileUploadDocument = SystemExtension.Clone(fileDocument);
                            newFileUploadDocument.Id = Guid.NewGuid();
                            newFileUploadDocument.RiskDetailId = newRiskDetailId;
                            newFileUploadDocument.tableRefId = newGuid;
                            await _fileUploadDocumentRepository.AddAsync(newFileUploadDocument);
                        }
                    }

                    var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
                    newQuoteCond.Id = newGuid;
                    newQuoteCond.RiskDetailId = newRiskDetailId;
                    await _quoteConditionsRepository.AddAsync(newQuoteCond);
                }
            }
            #endregion

            #region Claims
            var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = newRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = newRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            #region RiskPolicyContacts
            var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
            if (riskPolicyContacts != null)
            {
                foreach (var item in riskPolicyContacts)
                {
                    var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
                    newRiskPolicyContact.Id = Guid.NewGuid();
                    newRiskPolicyContact.RiskDetailId = newRiskDetailId;
                    newRiskPolicyContact.Entity.Id = Empty;
                    foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }
                    await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
                }
            }
            #endregion

            newRiskDetail.DriverInfo = null;

            var userFileUploadDocuments = fileUploadDocuments.Where(x => x.RiskDetailId == riskDetailId
                                            && x.IsUserUploaded.HasValue && x.IsUserUploaded.Value
                                            && x.FileCategoryId != 4 && x.FileCategoryId != 5).ToList();

            if (userFileUploadDocuments != null)
            {
                foreach (var item in userFileUploadDocuments)
                {
                    var newFileUploadDocument = SystemExtension.Clone<FileUploadDocument>(item);
                    newFileUploadDocument.Id = Empty;
                    newFileUploadDocument.tableRefId = newRiskDetailId;
                    newFileUploadDocument.RiskDetailId = newRiskDetailId;
                    await _fileUploadDocumentRepository.AddAsync(newFileUploadDocument);
                }
            }

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            newRiskDetail.RiskDetailVehicles = toCloneRiskDetail.RiskDetailVehicles;
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles
                // Include only those selected in bind option.
                .Where(rdv => rdv.Vehicle.Options != null && rdv.Vehicle.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())) && !rdv.Vehicle.IsDeleted)
                .ToList();

            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            // The 'newRiskDetail.RiskDetailDrivers' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailDrivers();

            await _riskRepository.AddAsync(newRisk);
            await _riskRepository.SaveChangesAsync();

            #region Vehicles
            var brokerInfo = newRisk.BrokerInfo;
            Guid garagingIdDefault = newRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(addr => addr.Address.IsMainGarage).Address.Id;

            // Copy each vehicle
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(riskDetailVehicle.VehicleId);  // Get vehicle.
                var clonedVehicle = vehicle.Clone();
                clonedVehicle.ClearRiskDetailVehicles();
                //clonedVehicle.VehicleProratedPremium = vehicle.VehicleProratedPremium.Clone();
                clonedVehicle.Id = Guid.Empty;
                clonedVehicle.MainId = Guid.NewGuid();
                clonedVehicle.PreviousVehicleVersionId = null;
                clonedVehicle.PreviousVehicleVersion = null;
                await _vehicleRepository.AddAsync(clonedVehicle);

                var vehicleRatingFactors = await _context.VehiclePremiumRatingFactor.Where(x => x.VehicleId == vehicle.Id.ToString()).ToListAsync(); // get vehicle rating factors
                foreach (var vrf in vehicleRatingFactors)
                {
                    var newVehicleRatingFactor = SystemExtension.Clone<VehiclePremiumRatingFactor>(vrf);
                    newVehicleRatingFactor.Id = 0;
                    newVehicleRatingFactor.VehicleId = clonedVehicle.Id.ToString();
                    newVehicleRatingFactor.RiskDetailId = newRiskDetail.Id;
                    _context.VehiclePremiumRatingFactor.Add(newVehicleRatingFactor);
                }
                clonedVehicle.LinkRiskDetail(newRiskDetail);                                    // Add the new RiskDetailVehicle to vehicle.
                // update rating factors
                //var boundRatingFactors = vehicleRatingFactors.Find(vrf => vrf.OptionId == bindOption);
                //vehicle.AutoLiabilityPremium = boundRatingFactors?.TotalALPremium ?? 0;
                //vehicle.CargoPremium = boundRatingFactors?.CargoPremium ?? 0;
                //vehicle.PhysicalDamagePremium = boundRatingFactors?.TotalPDPremium ?? 0;
                //vehicle.ComprehensivePremium = (boundRatingFactors?.CompPremium ?? 0) + (boundRatingFactors?.FireTheftPremium ?? 0);
                //vehicle.CollisionPremium = boundRatingFactors?.CollisionPremium ?? 0;
                //vehicle.TotalPremium = boundRatingFactors?.PerVehiclePremium ?? 0;
                //vehicle.Radius = boundRatingFactors?.Radius ?? 0;
                //vehicle.RadiusTerritory = boundRatingFactors?.RadiusTerritory ?? 0;
                //vehicle.EffectiveDate = brokerInfo.EffectiveDate;
                //vehicle.ExpirationDate = brokerInfo.ExpirationDate;
                //vehicle.ProratedPremium = vehicle.AutoLiabilityPremium + vehicle.PhysicalDamagePremium + vehicle.CargoPremium;

                clonedVehicle.GaragingAddressId = addressIdMapping.GetValueOrDefault(vehicle.GaragingAddressId.Value, garagingIdDefault);

                clonedVehicle.UpdateLimits(boundRiskCoverage);

                _vehicleRepository.Detach(vehicle);
                //_vehicleRepository.Update(vehicle);                                     // Update vehicle in db context.
            }

            await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region Drivers
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailDriver in oldRiskDetailDrivers)
            {
                var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.
                var rdv = riskDetailDriver.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                driver.RiskDetailDrivers.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _driverRepository.Update(driver);                                     // Update drvr in db context.
            }
            await _driverRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = newRiskDetailId;
                newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            #endregion

            await _riskFormRepository.SaveChangesAsync();
            await _riskManuscriptsRepository.SaveChangesAsync();
            await _riskHistoryRepository.SaveChangesAsync();
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _fileUploadDocumentRepository.SaveChangesAsync();
            await _bindingRequirementsRepository.SaveChangesAsync();
            await _quoteConditionsRepository.SaveChangesAsync();
            await _formsService.SaveFormsContentAsync(newRiskDetail, isEffectiveDateChanged: true);
            _context.SaveChanges();


            //#region RiskVehiclePremium
            //var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            //foreach (var rvpColl in riskVehiclePremiums)
            //{
            //    var cloneRvpColl = rvpColl.Clone();
            //    cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
            //    cloneRvpColl.ChangeVehicle()
            //    await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            //}
            //#endregion

            //#region RiskPolicyPremium (GL)
            //var riskPolicyPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            //var cloneRpp = riskPolicyPremiums.Clone();
            //cloneRpp.ChangeRiskDetail(newRiskDetail.Id);
            //await _riskPolicyPremiumRepository.SaveAsync(cloneRpp);
            //#endregion

            return newRiskDetail;
        }

        public async Task<RiskDetail> GenerateSubmissionDetail(Guid currentRiskDetailId, Guid riskId, string newSubmissionNumber, string newQuoteNumber)
        {
            _context.ChangeTracker.Clear();

            var newRiskDetailId = Guid.NewGuid();
            var newRiskId = Guid.NewGuid();

            var risk = await _riskRepository.GetRiskByIdAsync(riskId);
            var riskDetailId = currentRiskDetailId;
            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;
            RiskCoverage boundRiskCoverage = (toCloneRiskDetail != null && toCloneRiskDetail.RiskCoverages.Any()) ? toCloneRiskDetail.RiskCoverages.Single(rc => rc.OptionNumber == bindOption) : new();

            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == riskDetailId);

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = toCloneRiskDetail.Clone();
            newRiskDetail.Id = newRiskDetailId;
            newRiskDetail.RiskId = newRiskId;
            newRiskDetail.SubmissionNumber = newSubmissionNumber;
            newRiskDetail.CreatedDate = DateTime.Now;
            newRiskDetail.IsProcessed = false;
            newRiskDetail.EndorsementNumber = 0;
            #endregion

            #region Risk
            var toCloneRisk = await _riskRepository.FindAsync(x => x.Id == toCloneRiskDetail.RiskId, i => i.BrokerInfo);
            _riskRepository.Detach(toCloneRisk);

            var newRisk = new Data.Entity.Submission.Risk();
            newRisk = toCloneRisk.Clone();
            newRisk.Id = newRiskId;
            newRisk.Status = DTO.Constants.RiskStatus.Pending;
            newRisk.SubmissionNumber = newSubmissionNumber;
            newRisk.QuoteNumber = newQuoteNumber;
            newRisk.PolicyNumber = null;
            newRisk.RiskDetails = new List<RiskDetail>();
            newRisk.Binding.Id = Guid.Empty;

            if (newRisk.BrokerInfo != null)
            {
                newRisk.BrokerInfo.Id = Guid.Empty;
            }
            newRisk.RiskDetails.Add(newRiskDetail);
            newRisk.CurrentRiskDetailId = null;
            newRisk.ForEndorsementRiskDetailId = null;
            newRisk.CancellationReasonType = null;
            newRisk.PendingCancellationDate = null;
            newRisk.PendingCancellationNoticeDate = null;
            #endregion

            #region RiskResponse
            var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskResponses != null)
                newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.RiskDetailId = newRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            #region RiskFilings
            if (newRiskDetail.RiskFilings != null)
                newRiskDetail.RiskFilings.Select(x => (RiskFiling)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                // Clone only the bound risk coverage.
                newRiskDetail.RiskCoverages.Select(x => (RiskCoverage)SetId(x, newRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = newRiskDetailId;
            #endregion

            #region CommoditiesHauled
            if (newRiskDetail.CommoditiesHauled != null)
                newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, newRiskDetailId)).ToList();
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = newRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = newRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDotInfo != null)
            {
                newDotInfo.Id = newRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = newRiskDetailId;
            #endregion

            #region Insured Subsidiary
            var addressIdMapping = new Dictionary<Guid, Guid>();
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;
                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    if (x.AddressTypeId == (short)AddressTypesEnum.Garaging)
                    {
                        addressIdMapping.Add(x.Address.Id, newAddressId);
                    }
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }
            }
            #endregion

            #region RiskHistory
            var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskHistories != null)
            {
                var riskDetailDrivers = await _context.RiskDetailDriver
                                        .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                                        .Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();
                var boundCoverages = newRiskDetail.RiskCoverages?.Where(riskCov => riskCov?.OptionNumber == bindOption)?.FirstOrDefault();
                var calculatedVehicles = await _context.VehiclePremiumRatingFactor.Where(vrf => vrf.OptionId == bindOption && vrf.RiskDetailId == riskDetailId).ToListAsync();
                foreach (var x in riskHistories)
                {
                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = newRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                    _riskHistoryRepository.Update(x);
                }
            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = newRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = newRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = newRiskDetailId;

                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.RiskDetailId = newRiskDetailId;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);
                }
            }
            #endregion

            #region BindingRequirements
            var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (bindingRequirements != null)
            {
                foreach (var x in bindingRequirements)
                {
                    var newGuid = Guid.NewGuid();

                    var relatedDocuments = await _fileUploadDocumentRepository.GetAsync(relatedDoc => relatedDoc.FileCategoryId == 4 && relatedDoc.tableRefId == x.Id);
                    if (relatedDocuments != null)
                    {
                        foreach (var fileDocument in relatedDocuments)
                        {
                            var newFileUploadDocument = SystemExtension.Clone(fileDocument);
                            newFileUploadDocument.Id = Guid.NewGuid();
                            newFileUploadDocument.RiskDetailId = newRiskDetailId;
                            newFileUploadDocument.tableRefId = newGuid;
                            await _fileUploadDocumentRepository.AddAsync(newFileUploadDocument);
                        }
                    }

                    var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
                    newBindingRequirements.Id = newGuid;
                    newBindingRequirements.RiskDetailId = newRiskDetailId;
                    await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
                }
            }
            #endregion

            #region QuoteConditions
            var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (quoteCond != null)
            {
                foreach (var x in quoteCond)
                {
                    var newGuid = Guid.NewGuid();
                    var relatedDocuments = await _fileUploadDocumentRepository.GetAsync(relatedDoc => relatedDoc.FileCategoryId == 5 && relatedDoc.tableRefId == x.Id);
                    if (relatedDocuments != null)
                    {
                        foreach (var fileDocument in relatedDocuments)
                        {
                            var newFileUploadDocument = SystemExtension.Clone(fileDocument);
                            newFileUploadDocument.Id = Guid.NewGuid();
                            newFileUploadDocument.RiskDetailId = newRiskDetailId;
                            newFileUploadDocument.tableRefId = newGuid;
                            await _fileUploadDocumentRepository.AddAsync(newFileUploadDocument);
                        }
                    }

                    var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
                    newQuoteCond.Id = newGuid;
                    newQuoteCond.RiskDetailId = newRiskDetailId;
                    await _quoteConditionsRepository.AddAsync(newQuoteCond);
                }
            }
            #endregion

            #region Claims
            var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = newRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = newRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            #region RiskPolicyContacts
            var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
            if (riskPolicyContacts != null)
            {
                foreach (var item in riskPolicyContacts)
                {
                    var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
                    newRiskPolicyContact.Id = Guid.NewGuid();
                    newRiskPolicyContact.RiskDetailId = newRiskDetailId;
                    newRiskPolicyContact.Entity.Id = Empty;
                    foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }
                    await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
                }
            }
            #endregion

            newRiskDetail.DriverInfo = null;

            var userFileUploadDocuments = fileUploadDocuments.Where(x => x.RiskDetailId == riskDetailId
                                            && ((x.IsUserUploaded.HasValue && x.IsUserUploaded.Value && x.FileCategoryId != 4 && x.FileCategoryId != 5)
                                            || (x.IsSystemGenerated == false && (x.FileCategoryId == 4 || x.FileCategoryId == 5)))).ToList();

            if (userFileUploadDocuments != null)
            {
                foreach (var item in userFileUploadDocuments)
                {
                    var newFileUploadDocument = SystemExtension.Clone<FileUploadDocument>(item);
                    newFileUploadDocument.Id = Empty;
                    newFileUploadDocument.tableRefId = newRiskDetailId;
                    newFileUploadDocument.RiskDetailId = newRiskDetailId;
                    await _fileUploadDocumentRepository.AddAsync(newFileUploadDocument);
                }
            }

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            newRiskDetail.RiskDetailVehicles = toCloneRiskDetail.RiskDetailVehicles;
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles.ToList();

            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            // The 'newRiskDetail.RiskDetailDrivers' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailDrivers();

            await _riskRepository.AddAsync(newRisk);
            await _riskRepository.SaveChangesAsync(updateCreatedBy: false);

            #region Vehicles
            var brokerInfo = newRisk.BrokerInfo;
            Guid garagingIdDefault = newRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(addr => addr.Address.IsMainGarage).Address.Id;

            // Copy each vehicle
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(riskDetailVehicle.VehicleId);  // Get vehicle.
                var clonedVehicle = vehicle.Clone();
                clonedVehicle.ClearRiskDetailVehicles();
                //clonedVehicle.VehicleProratedPremium = vehicle.VehicleProratedPremium.Clone();
                clonedVehicle.Id = Guid.Empty;
                clonedVehicle.MainId = Guid.NewGuid();
                clonedVehicle.PreviousVehicleVersionId = null;
                clonedVehicle.PreviousVehicleVersion = null;
                await _vehicleRepository.AddAsync(clonedVehicle);

                var vehicleRatingFactors = await _context.VehiclePremiumRatingFactor.Where(x => x.VehicleId == vehicle.Id.ToString()).ToListAsync(); // get vehicle rating factors
                foreach (var vrf in vehicleRatingFactors)
                {
                    var newVehicleRatingFactor = SystemExtension.Clone<VehiclePremiumRatingFactor>(vrf);
                    newVehicleRatingFactor.Id = 0;
                    newVehicleRatingFactor.VehicleId = clonedVehicle.Id.ToString();
                    newVehicleRatingFactor.RiskDetailId = newRiskDetail.Id;
                    _context.VehiclePremiumRatingFactor.Add(newVehicleRatingFactor);
                }
                clonedVehicle.LinkRiskDetail(newRiskDetail);

                clonedVehicle.GaragingAddressId = addressIdMapping.GetValueOrDefault(vehicle.GaragingAddressId.Value, garagingIdDefault);

                clonedVehicle.UpdateLimits(boundRiskCoverage);

                _vehicleRepository.Detach(vehicle);
                //_vehicleRepository.Update(vehicle);                                     // Update vehicle in db context.
            }

            await _vehicleRepository.SaveChangesAsync(updateCreatedBy: false);                                // Save all vehicle changes.
            #endregion

            #region Drivers
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailDriver in oldRiskDetailDrivers)
            {
                var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.
                var rdv = riskDetailDriver.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                driver.RiskDetailDrivers.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _driverRepository.Update(driver);                                     // Update drvr in db context.
            }
            await _driverRepository.SaveChangesAsync(updateCreatedBy: false);                                // Save all vehicle changes.
            #endregion

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = newRiskDetailId;
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            #endregion

            await _riskFormRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskManuscriptsRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskHistoryRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskSpecificDriverInfoRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskSpecificDotInfoRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync(updateCreatedBy: false);
            await _riskPolicyContactRepository.SaveChangesAsync(updateCreatedBy: false);
            await _fileUploadDocumentRepository.SaveChangesAsync(updateCreatedBy: false);
            await _bindingRequirementsRepository.SaveChangesAsync(updateCreatedBy: false);
            await _quoteConditionsRepository.SaveChangesAsync(updateCreatedBy: false);
            await _formsService.SaveFormsContentAsync(newRiskDetail, isEffectiveDateChanged: true);
            _context.SaveChanges();

            //#region RiskVehiclePremium
            //var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            //foreach (var rvpColl in riskVehiclePremiums)
            //{
            //    var cloneRvpColl = rvpColl.Clone();
            //    cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
            //    await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            //}
            //#endregion

            //#region RiskPolicyPremium
            ////General Liability Premium
            //var riskPolicyGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            //var cloneGLRpp = riskPolicyGLPremiums.Clone();
            //cloneGLRpp.ChangeRiskDetail(newRiskDetail.Id);
            //await _riskPolicyPremiumRepository.SaveAsync(cloneGLRpp);

            ////Trailer Interchange Premium
            //var riskPolicyTIPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_TI);
            //var cloneTIRpp = riskPolicyTIPremiums.Clone();
            //cloneTIRpp.ChangeRiskDetail(newRiskDetail.Id);
            //await _riskPolicyPremiumRepository.SaveAsync(cloneTIRpp);

            ////Non-Owned Trailer Premium
            //var riskPolicyNOPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_NO);
            //var cloneNORpp = riskPolicyNOPremiums.Clone();
            //cloneNORpp.ChangeRiskDetail(newRiskDetail.Id);
            //await _riskPolicyPremiumRepository.SaveAsync(cloneNORpp);
            //#endregion

            return newRiskDetail;
        }

        public async Task<RiskDetail> GenerateRenewalDetailFromPolicy(Guid currentRiskDetailId, Guid riskId, string newSubmissionNumber, string newQuoteNumber)
        {
            var newRiskDetail = new RiskDetail();
            try
            {
                _context.ChangeTracker.Clear();

                var newRiskDetailId = Guid.NewGuid();
                var newRiskId = Guid.NewGuid();

                var risk = await _riskRepository.GetRiskByIdAsync(riskId);
                var riskDetailId = risk.CurrentRiskDetailId.Value;
                var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
                _riskDetailRepository.Detach(toCloneRiskDetail);

                var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
                int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

                var policyContacts = await _policyContactsService.GetAllAssignedToPolicyContacts();
                RiskCoverage boundRiskCoverage = toCloneRiskDetail.RiskCoverages.Single(rc => rc.OptionNumber == bindOption);

                #region RiskDetail
                newRiskDetail = toCloneRiskDetail.Clone();
                newRiskDetail.Id = newRiskDetailId;
                newRiskDetail.RiskId = newRiskId;
                newRiskDetail.SubmissionNumber = newSubmissionNumber;
                newRiskDetail.Status = RiskDetailStatus.Received;
                newRiskDetail.CreatedDate = DateTime.Now;
                newRiskDetail.IsProcessed = false;
                newRiskDetail.EndorsementNumber = 0;
                newRiskDetail.AssignedToId = Convert.ToInt32(policyContacts?.Find(contact => contact.Name == "Sara Kenkel")?.Id);
                newRiskDetail.IsNewVenture = false;
                newRiskDetail.NewVenturePreviousExperienceId = null;
                newRiskDetail.EndorsementEffectiveDate = null;
                newRiskDetail.SourceRiskDetailId = null;
                #endregion

                #region Risk
                var toCloneRisk = await _riskRepository.FindAsync(x => x.Id == toCloneRiskDetail.RiskId, i => i.BrokerInfo);
                _riskRepository.Detach(toCloneRisk);

                var newRisk = new Data.Entity.Submission.Risk();
                newRisk = toCloneRisk.Clone();
                newRisk.Id = newRiskId;
                newRisk.Status = DTO.Constants.RiskStatus.Pending;
                newRisk.SubmissionNumber = newSubmissionNumber;
                newRisk.QuoteNumber = newQuoteNumber;
                newRisk.PolicyNumber = null;
                newRisk.RiskDetails = new List<RiskDetail>();
                newRisk.Binding.Id = Guid.Empty;
                newRisk.RiskDetails.Add(newRiskDetail);
                newRisk.CurrentEndorsementNumber = 0;
                newRisk.CurrentRiskDetailId = null;
                newRisk.ForEndorsementRiskDetailId = null;
                newRisk.CancellationReasonType = null;
                newRisk.PendingCancellationDate = null;
                newRisk.PendingCancellationNoticeDate = null;
                newRisk.RaterVersionId = 0;
                newRisk.IsRaterVersionLocked = false;
                newRisk.PreviousPolicyNumber = toCloneRisk.PolicyNumber;
                newRisk.IsRenewed = false;
                newRisk.IsFromRenewal = true;
                newRisk.IsPendingCancellation = false;
                newRisk.IsRenewal = true;
                newRisk.RenewalDate = DateTime.Now;
                newRisk.RenewalFromRiskId = toCloneRisk.Id;
                #endregion

                #region BrokerInfo 
                if (newRisk.BrokerInfo != null)
                {
                    var brokerInfo = newRisk.BrokerInfo;
                    brokerInfo.Id = Guid.Empty;

                    var toClonebrokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == toCloneRiskDetail.RiskId);
                    brokerInfo.RenewEffectivityDates(toClonebrokerInfo);

                    int effectiveYear = brokerInfo.EffectiveDate?.Year ?? DateTime.Now.Year; 
                    int yearEstablished = newRiskDetail.Insured.Entity.YearEstablished ?? 0;
                    int year1UnderCurrentMgmt = newRiskDetail.FirstYearUnderCurrentManagement ?? 0;
                    newRiskDetail.YearsInBusiness = (short)(effectiveYear - yearEstablished);
                    newRiskDetail.YearsUnderCurrentManagement = (short)(effectiveYear - year1UnderCurrentMgmt);

                    // Update policy number
                    var state = toCloneRiskDetail.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == (short)AddressTypesEnum.Business).Address.State;
                    var updatedPolicyNumber = _riskSequence.UpdatePolicyNumber(
                        toCloneRisk.PolicyNumber, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value, state);
                    newRisk.PolicyNumber = _riskSequence.GenerateRenewalPolicyNumber(updatedPolicyNumber, state, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value);
                }
                #endregion

                #region RiskResponse
                var riskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (riskResponses != null)
                    newRiskDetail.RiskResponses = riskResponses.Select(x => (RiskResponse)SetId(x, newRiskDetailId)).ToList();
                #endregion

                #region RiskAdditionalInterest
                var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
                if (riskAdditionalInterest != null)
                {
                    newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                    foreach (var item in riskAdditionalInterest)
                    {
                        if (!(item.ExpirationDate.Value <= DateTime.Now) && !item.RemoveProcessDate.HasValue)
                        {
                            var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                            newAdditionalInterest.Id = Guid.NewGuid();
                            newAdditionalInterest.RiskDetailId = newRiskDetailId;
                            newAdditionalInterest.Entity.Id = Empty;
                            newAdditionalInterest.EffectiveDate = newRisk.BrokerInfo?.EffectiveDate.Value;
                            newAdditionalInterest.ExpirationDate = newRisk.BrokerInfo?.ExpirationDate.Value;
                            foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                            {
                                x.Id = Empty;
                                x.Address.Id = Empty;
                            }

                            newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                        }
                    }
                }
                #endregion

                #region RiskFilings
                if (newRiskDetail.RiskFilings != null)
                    newRiskDetail.RiskFilings.Select(x => (RiskFiling)SetId(x, newRiskDetailId)).ToList();
                #endregion

                #region RiskCoverages
                if (newRiskDetail.RiskCoverages != null)
                {
                    // Clone only the bound risk coverage.
                    newRiskDetail.RiskCoverages = newRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption)
                        .Select(x => (RiskCoverage)SetId(x, newRiskDetailId)).ToList();
                    var riskCov = newRiskDetail.RiskCoverages.First();
                    if (riskCov.RiskCoveragePremium != null)
                        riskCov.RiskCoveragePremium.Id = Empty;
                    if (riskCov.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        riskCov.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                    riskCov.ClearPremiumFactors();
                }
                #endregion

                #region RadiusOfOperation
                if (newRiskDetail.RadiusOfOperations != null)
                    newRiskDetail.RadiusOfOperations.Id = newRiskDetailId;
                #endregion

                #region CommoditiesHauled
                if (newRiskDetail.CommoditiesHauled != null)
                    newRiskDetail.CommoditiesHauled.Select(x => (RiskSpecificCommoditiesHauled)SetId(x, newRiskDetailId)).ToList();
                #endregion

                #region DriverHeader
                if (newRiskDetail.DriverHeader != null)
                {
                    var driverHeader = newRiskDetail.DriverHeader;
                    driverHeader.Id = Empty;
                    driverHeader.RiskDetailId = newRiskDetailId;
                    driverHeader.accDriverFactor = null;
                    driverHeader.driverVehicleRatio = null;
                    driverHeader.mvrHeaderDate = newRisk.BrokerInfo.EffectiveDate;
                }
                #endregion

                #region DriverHiringCriteria
                var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
                if (driverHiringCriteria != null)
                {
                    var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                    newDriverHiringCriteria.Id = newRiskDetailId;
                    newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                    newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                    newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                    newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                    newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                    newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                    newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                    newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                    if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                    {
                        foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                        {
                            var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                            if (optionFromDb != null)
                            {
                                newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                            }
                        }
                    }

                    newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                    if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                    {
                        foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                        {
                            var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                            if (optionFromDb != null)
                            {
                                newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                            }
                        }
                    }

                    await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
                }

                newRiskDetail.DriverHiringCriteria = null;
                #endregion

                #region DotInfo
                var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
                if (newDotInfo != null)
                {
                    newDotInfo.Id = newRiskDetailId;
                    await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
                }
                newRiskDetail.DotInfo = null;
                #endregion

                #region UnderwritingQuestions
                if (newRiskDetail.UnderwritingQuestions != null)
                {
                    newRiskDetail.UnderwritingQuestions.Id = newRiskDetailId;
                    newRiskDetail.UnderwritingQuestions.AreAnswersConfirmed = false;
                }
                #endregion

                #region Insured Subsidiary, Addresses
                var addressIdMapping = new Dictionary<Guid, Guid>();
                if (newRiskDetail.Insured != null)
                {
                    var newEntityId = Guid.NewGuid();

                    newRiskDetail.Insured.Id = Empty;
                    newRiskDetail.Insured.Entity.Id = newEntityId;
                    foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                    {
                        var newAddressId = Guid.NewGuid();
                        if (x.AddressTypeId == (short)AddressTypesEnum.Garaging)
                        {
                            addressIdMapping.Add(x.Address.Id, newAddressId);
                        }
                        x.Id = Empty;
                        x.Address.Id = newAddressId;
                    }
                }
                #endregion

                

                #region RiskSpecificSafetyDevice
                var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (riskSpecificSafetyDevice != null)
                {
                    foreach (var x in riskSpecificSafetyDevice)
                    {
                        x.Id = 0;
                        x.RiskDetailId = newRiskDetailId;
                        await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                    }
                }
                #endregion

                #region RiskForms
                var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (riskForms != null)
                {
                    foreach (var x in riskForms)
                    {
                        Guid guidOutput = Guid.Empty;
                        var isFormIdGuid = Guid.TryParse(x.FormId, out guidOutput);

                        if (!isFormIdGuid)
                        {
                            var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                            
                            newRiskForms.Id = Empty;
                            newRiskForms.RiskDetailId = newRiskDetailId;

                            //Clear Driver Exclusions
                            if (newRiskForms.FormId == "DriverExclusion")
                            {
                                newRiskForms.IsSelected = null;
                                newRiskForms.Other = null;
                            }

                            await _riskFormRepository.AddAsync(newRiskForms);
                        }
                    }
                }
                #endregion

                #region Manuscripts
                var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (manuscripts != null)
                {
                    foreach (var x in manuscripts)
                    {
                        if (!(x.ExpirationDate <= DateTime.Now) && x.IsActive == true && !x.DeletedDate.HasValue)
                        {
                            var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                            newManuscripts.Id = Empty;
                            newManuscripts.RiskDetailId = newRiskDetailId;
                            newManuscripts.EffectiveDate = newRisk.BrokerInfo.EffectiveDate.Value;
                            newManuscripts.ExpirationDate = newRisk.BrokerInfo.ExpirationDate.Value;
                            await _riskManuscriptsRepository.AddAsync(newManuscripts);
                        }
                    }
                }
                #endregion

                #region Claims
                var claims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (claims != null)
                {
                    foreach (var claim in claims.OrderBy(o => o.Order))
                    {
                        claim.Order += 1;

                        var newClaim = claim.Order > 4 ? new ClaimsHistory { Order = 0 } : SystemExtension.Clone<ClaimsHistory>(claim);
                        newClaim.Id = Empty;
                        newClaim.RiskDetailId = newRiskDetailId;
                        await _claimsHistoryRepository.AddAsync(newClaim);
                    }
                }
                #endregion

                #region Destinations
                var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (destinations != null)
                {
                    foreach (var destination in destinations)
                    {
                        var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                        newDestination.Id = Empty;
                        newDestination.RiskDetailId = newRiskDetailId;
                        await _riskSpecificDestinationRepository.AddAsync(newDestination);
                    }
                }
                #endregion

                #region RiskPolicyContacts
                var riskPolicyContacts = await _riskPolicyContactRepository.GetAllIncludeAsync(riskDetailId);
                if (riskPolicyContacts != null)
                {
                    foreach (var item in riskPolicyContacts)
                    {
                        var newRiskPolicyContact = SystemExtension.Clone<RiskPolicyContact>(item);
                        newRiskPolicyContact.Id = Guid.NewGuid();
                        newRiskPolicyContact.RiskDetailId = newRiskDetailId;
                        newRiskPolicyContact.Entity.Id = Empty;
                        foreach (var x in newRiskPolicyContact.Entity.EntityAddresses)
                        {
                            x.Id = Empty;
                            x.Address.Id = Empty;
                        }
                        await _riskPolicyContactRepository.AddAsync(newRiskPolicyContact);
                    }
                }
                #endregion

                #region Binding 
                newRisk.Binding.PaymentTypesId = PaymentTypes.DirectBillInstallments.Id;
                newRisk.Binding.MinimumEarned = 25;
                newRisk.Binding.CreditedOfficeId = CreditedOffice.Applied.Id;
                newRisk.Binding.SurplusLIneNum = null;
                newRisk.Binding.ProducerSLNumber = null;
                #endregion

                // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
                // must be updated so we back up its reference.
                newRiskDetail.RiskDetailVehicles = toCloneRiskDetail.RiskDetailVehicles;
                var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles
                    // Include only those selected in bind option.
                    .Where(rdv => rdv.Vehicle.Options != null && rdv.Vehicle.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())))
                    .Where(rdv => rdv.Vehicle.DeletedDate == null) // Exclude deleted
                    .OrderBy(rdv => rdv.Vehicle.VIN)
                    .ToList();

                // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
                newRiskDetail.ClearRiskDetailVehicles();

                // The 'newRiskDetail.RiskDetailDrivers' after cloning from old RiskDetail
                // must be updated so we back up its reference.

                newRiskDetail.RiskDetailDrivers = toCloneRiskDetail.RiskDetailDrivers;
                var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers
                    .Where(rdd => rdd.Driver.Options != null && rdd.Driver.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())))
                    .Where(rdd => rdd.Driver.DeletedDate == null) // Exclude deleted
                    .ToList();
                // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
                newRiskDetail.ClearRiskDetailDrivers();

                newRiskDetail.DriverInfo = null;

                await _riskRepository.AddAsync(newRisk);
                await _riskRepository.SaveChangesAsync();

                #region RiskHistory

                var riskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
                if (riskHistories != null)
                {

                    var riskDetailDrivers = await _context.RiskDetailDriver
                                            .Include(x => x.Driver).ThenInclude(x => x.DriverIncidents)
                                            .Where(x => x.RiskDetailId == riskDetailId && !x.Driver.DeletedDate.HasValue).ToListAsync();
                    var boundCoverages = newRiskDetail.RiskCoverages?.Where(riskCov => riskCov?.OptionNumber == bindOption)?.FirstOrDefault();
                    var calculatedVehicles = await _context.VehiclePremiumRatingFactor.Where(vrf => vrf.OptionId == bindOption && vrf.RiskDetailId == riskDetailId).ToListAsync();

                    var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(risk.CurrentRiskDetailId.Value);
                    var riskInsured = await _riskDetailRepository.GetInsuredAddressAsync(riskDetailId);
                    var brokerInfoCovHist = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDetail.RiskId);

                    List<Guid> riskdetaillst = new List<Guid> { brokerInfoCovHist.AgencyId.GetValueOrDefault() };

                    var agencies = await _agencyRepository.GetAllAgenciesByIds(riskdetaillst);
                    var agency = agencies.FirstOrDefault();

                    var stateCovHist = riskInsured.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == null).Address.State;
                    var getLimitsList = await _limitsService.GetPerStateAsync(stateCovHist, risk.RaterVersionId);
                    var newCovHistory = new RiskHistory { HistoryYear = 0 };
                    var liablimits = getLimitsList?.AutoBILimits.Where(x => x.Id == boundCoverages.AutoBILimitId).Select(x => x.LimitValue).FirstOrDefault();
                    var autoLiabilityDeductible = getLimitsList?.LiabilityDeductibleLimits.Where(x => x.Id == boundCoverages?.LiabilityDeductibleId).Select(x => x.LimitValue).FirstOrDefault();
                    var cOLLDeductible = getLimitsList?.CollisionLimits.Where(x => x.Id == boundCoverages?.CollDeductibleId).Select(x => x.LimitValue).FirstOrDefault();
                    var cOMPDeductible = getLimitsList?.ComprehensiveLimits.Where(x => x.Id == boundCoverages?.CompDeductibleId).Select(x => x.LimitValue).FirstOrDefault();
                    var cargolimits = getLimitsList?.CargoLimits.Where(x => x.Id == boundCoverages?.CargoLimitId).Select(x => x.LimitValue).FirstOrDefault();
                    var refLimits = getLimitsList?.RefLimits.Where(x => x.Id == boundCoverages?.RefCargoLimitId).Select(x => x.LimitValue).FirstOrDefault();
                    var fireDeductible = (await _listValueTextParser.ParseLimitAsync(boundCoverages?.FireDeductibleId, LimitType.Comprehensive, stateCovHist)) ?? null;
                    
                    var cOLLDeductibleVal = cOLLDeductible != null && cOLLDeductible.Length > 0 ? cOLLDeductible.IndexOf("DED") > 0 ? cOLLDeductible.Replace("DED", "") : null : null;
                    var cOMPDeductibleVal = cOMPDeductible != null && cOMPDeductible.Length > 0 ? cOMPDeductible.IndexOf("DED") > 0 ? cOMPDeductible.Replace("DED", "") : null : null;
                    var fireDeductibleVal = fireDeductible != null && fireDeductible.Length > 0 ? fireDeductible.IndexOf("DED") > 0 ? fireDeductible.Replace("DED", "") : null : null;
                    var cargolimitsVal = cargolimits != null && cargolimits.Length > 0 ? cargolimits.IndexOf("K") > 0 ? cargolimits.Substring(0, cargolimits.IndexOf("K")) + "000" : null : null;
                    var refLimitsVal = refLimits != null && refLimits.Length > 0 ? refLimits.IndexOf("K") > 0 ? refLimits.Substring(0, refLimits.IndexOf("K")) + "000" : null : null;
                    var veh = (await _vehicleService.GetByRiskDetailIdIncludeAsync(riskDetailId))?
                    .OrderBy(vehicle => vehicle.VIN).ToList();
                    var vehcount = veh.Where(x => ((x.Options ?? "").Split(',')).Contains(boundCoverages.OptionNumber.ToString()) == true && x.IsValidated == true).ToList();
                    var povecount = veh.Where(x => ((x.Options ?? "").Split(',')).Contains(boundCoverages.OptionNumber.ToString()) == true && x.VehicleType == "Power Unit").ToList();
                    var drivers = await _driverRepository.GetByRiskDetailAsync(riskDetailId);
                    drivers.Where(x => ((x.Options ?? "").Split(',')).Contains(boundCoverages.OptionNumber.ToString()) == true && x.IsValidated == true && x.IsValidatedKO == true)
                    .ToList();


                    foreach (var x in riskHistories.OrderBy(o => o.HistoryYear))
                    {
                        x.PolicyTermStartDate = x.PolicyTermStartDate;
                        x.PolicyTermEndDate = x.PolicyTermEndDate;
                        x.InsuranceCarrierOrBroker = x.InsuranceCarrierOrBroker;
                        x.LiabilityLimits = x.LiabilityLimits;
                        x.LiabilityDeductible = x.LiabilityDeductible;
                        x.NumberOfPowerUnits = x.NumberOfPowerUnits;
                        x.NumberOfDrivers = riskDetailDrivers?.Where(driver => ((driver.Driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count() > 10 ? x.NumberOfDrivers : null;
                        x.NumberOfSpareVehicles = calculatedVehicles?.Count() > 10 ? x.NumberOfSpareVehicles : null;
                        x.ComprehensiveDeductible = boundCoverages?.CompDeductibleId != 76 ? x.ComprehensiveDeductible  : null;
                        x.CollisionDeductible = boundCoverages?.CollDeductibleId != 65 ? x.CollisionDeductible : null;
                        x.CargoLimits = boundCoverages?.CargoLimitId != 53 ? x.CargoLimits : null;
                        x.RefrigeratedCargoLimits = boundCoverages?.RefCargoLimitId != 157 ? x.RefrigeratedCargoLimits : null;

                        x.HistoryYear += 1;

                        var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                        newRiskHistory.Id = Empty;
                        newRiskHistory.RiskDetailId = newRiskDetailId;
                        await _riskHistoryRepository.AddAsync(newRiskHistory);
                    }


                    var newRiskHistory0 = new RiskHistory { HistoryYear = 0 };
                    newRiskHistory0.HistoryYear = 0;
                    newRiskHistory0.PolicyTermStartDate = brokerInfoCovHist.EffectiveDate;
                    newRiskHistory0.PolicyTermEndDate = brokerInfoCovHist.ExpirationDate;
                    newRiskHistory0.InsuranceCarrierOrBroker = agency.Entity.CompanyName;
                    newRiskHistory0.LiabilityLimits = liablimits == null ? null : Convert.ToDecimal(liablimits);
                    newRiskHistory0.LiabilityDeductible = autoLiabilityDeductible == null ? null : Convert.ToDecimal(autoLiabilityDeductible); 
                    newRiskHistory0.CollisionDeductible = cOLLDeductibleVal == null ? null : Convert.ToDecimal(cOLLDeductibleVal);
                    newRiskHistory0.ComprehensiveDeductible = cOMPDeductibleVal == null ? fireDeductibleVal ==null ? null : Convert.ToDecimal(fireDeductibleVal) : Convert.ToDecimal(cOMPDeductibleVal);
                    newRiskHistory0.CargoLimits = cargolimitsVal == null ? null : Convert.ToDecimal(cargolimitsVal);
                    newRiskHistory0.RefrigeratedCargoLimits = refLimitsVal == null ? null : Convert.ToDecimal(refLimitsVal); 
                    newRiskHistory0.NumberOfVehicles = vehcount.Count();
                    newRiskHistory0.NumberOfPowerUnits = povecount.Count();
                    newRiskHistory0.NumberOfDrivers = drivers.Count() > 10 ? drivers.Count() : null;
                    newRiskHistory0.Id = Empty;
                    newRiskHistory0.RiskDetailId = newRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory0);


                }


                #endregion

                #region Subsidiaries  
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == toCloneRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSub = SystemExtension.Clone(sub);
                    newSub.Id = Empty;
                    newSub.EntityId = newRiskDetail.Insured.EntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSub);
                }
                #endregion

                #region Vehicles
                Guid garagingIdDefault = newRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(addr => addr.Address.IsMainGarage).Address.Id;

                // Copy each vehicle
                foreach (var riskDetailVehicle in oldRiskDetailVehicles)
                {
                    var vehicle = await _vehicleRepository.GetByRiskDetailAndMainIdAsync(toCloneRiskDetail.Id, riskDetailVehicle.Vehicle.MainId);  // Get vehicle.
                    var clonedVehicle = vehicle.CloneForRenewal();
                    clonedVehicle.EffectiveDate = newRisk.BrokerInfo.EffectiveDate;
                    clonedVehicle.ExpirationDate = newRisk.BrokerInfo.ExpirationDate;
                    await _vehicleRepository.AddAsync(clonedVehicle);

                    clonedVehicle.LinkRiskDetail(newRiskDetail);                                    // Add the new RiskDetailVehicle to vehicle.
                    clonedVehicle.GaragingAddressId = addressIdMapping.GetValueOrDefault(vehicle.GaragingAddressId.Value, garagingIdDefault);
                    clonedVehicle.UpdateLimits(boundRiskCoverage);

                    _vehicleRepository.Detach(vehicle);
                }

                await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
                #endregion

                #region Drivers
                // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
                // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
                // with its RiskDetail set to 'newRiskDetail'
                foreach (var riskDetailDriver in oldRiskDetailDrivers)
                {
                    var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.
                    var clonedDriver = driver.CloneForRenewal();
                    clonedDriver.EffectiveDate = newRisk.BrokerInfo.EffectiveDate;
                    clonedDriver.ExpirationDate = newRisk.BrokerInfo.ExpirationDate;
                    clonedDriver.LinkRiskDetail(newRiskDetail);
                    await _driverRepository.AddAsync(clonedDriver);                                     // Update drvr in db context.
                }
                await _driverRepository.SaveChangesAsync();                                // Save all vehicle changes.
                #endregion

                #region DriverInfo
                var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
                if (newDriverInfo != null)
                {
                    newDriverInfo.Id = newRiskDetailId;
                    newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                    await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
                    newRiskDetail.DriverInfo = newDriverInfo;
                }
                #endregion

                await _entitySubsidiaryRepository.SaveChangesAsync();
                await _riskFormRepository.SaveChangesAsync();
                await _riskManuscriptsRepository.SaveChangesAsync();
                await _riskHistoryRepository.SaveChangesAsync();
                await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
                await _riskSpecificDriverInfoRepository.SaveChangesAsync();
                await _riskSpecificDotInfoRepository.SaveChangesAsync();
                await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
                await _riskPolicyContactRepository.SaveChangesAsync();
                await _bindingRequirementsRepository.SaveChangesAsync();
                await _quoteConditionsRepository.SaveChangesAsync();
                await _formsService.SaveFormsContentAsync(newRiskDetail, isEffectiveDateChanged:true);
                _context.SaveChanges();

                return newRiskDetail;
            }
            catch (Exception e)
            {
                if (newRiskDetail != null)
                    await _submissionService.DeleteSubmissionAsync(newRiskDetail.SubmissionNumber);
                throw;
            }
        }

        private dynamic SetId(dynamic entity, Guid riskDetailId)
        {
            entity.Id = Empty;
            entity.RiskDetailId = riskDetailId;
            return entity;
        }

        public async Task<List<RiskFormDTO>> GetRiskForms(Guid riskDetailId)
        {
            var riskDetail = await _riskDetailRepository.GetInsuredAddressAsync(riskDetailId);
            var state = riskDetail.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == null).Address.State;
            var forms = await _formsService.GetSelectedByRiskIdAsync(riskDetailId);
            if (!ValidSurplusLinesState.Any(x => x == state))
            {
                var surplusLines = forms.Where(x => x.FormId == "SurplusLinesNotice").FirstOrDefault();
                if (surplusLines != null)
                    forms.Remove(surplusLines);
            }
            return forms;
        }

        public async Task<List<RiskFormDTO>> GetRiskFormsWithManuscripts(Guid riskDetailId)
        {
            var forms = await GetRiskForms(riskDetailId);
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                var ofacSortOrder = forms.Where(x => x.FormId == "OFACNotice").FirstOrDefault()?.Form?.SortOrder ?? 0;
                foreach (var manu in manuscripts)
                {
                    if (manu.IsProrate == false && manu.DeletedDate != null) continue;
                    forms.Add(new RiskFormDTO()
                    {
                        Id = manu.Id,
                        IsSelected = manu.IsSelected,
                        RiskDetailId = manu.RiskDetailId,
                        Form = new FormDTO()
                        {
                            FormNumber = "RPCAMANU",
                            Id = "ManuscriptEndorsement",
                            Name = manu.Title,
                            SortOrder = ofacSortOrder - 1
                        }
                    });
                }
            }

            // Excldue forms
            forms = forms.Where(x => x.IsSelected.Value || x.Form.FormNumber == "RPCAMANU").ToList();

            return forms;
        }

        public async Task<List<RiskFormDTO>> GetRiskFormsWithConditionals(RiskDTO riskDetail, bool isEndorsement = false, List<RiskFormDTO> riskForms = null)
        {
            var risk = await _riskRepository.FindAsync(riskDetail.RiskId);
            if (riskForms != null && riskForms.Count == 1 && riskForms.First().Form.IsSupplementalDocument) return riskForms;
            var conditionalFormIds = new[] { "PrimaryAndNoncontributory", "DesignatedInsured", "LossPayeeEndorsement", "WaiverOfTransferOfRights", "GeneralLiabilityEndorsement", "AdditionalInsuredBlanket" };
            var conditionalForms = _mapper.Map<List<FormDTO>>(await _formRepository.GetAsync(f => conditionalFormIds.Contains(f.Id), entity => entity.FormVersions));
            conditionalForms.ForEach(cf => cf.LatestFormVersion(riskDetail.RequestedQuoteDate, risk.IsRaterVersionLocked));

            riskForms ??= await GetRiskFormsWithManuscripts(riskDetail.Id.GetValueOrDefault());

            var lossPayeeOther = riskForms.FirstOrDefault(rf => rf.FormId == "LossPayeeEndorsement")?.Other;
            var glOther = riskForms.FirstOrDefault(rf => rf.FormId == "GeneralLiabilityEndorsement")?.Other;
            var isGlSelected = riskForms.FirstOrDefault(rf => rf.FormId == "GeneralLiabilityEndorsement")?.IsSelected ?? false;

            riskForms.RemoveAll(rf => conditionalFormIds.Contains(rf.Form.Id));
            if (isEndorsement) riskForms.RemoveAll(rf => rf.Form.FormNumber == "RPCA002");

            var bindOption = riskDetail.Binding.BindOptionId.Value - 1;
            if (isEndorsement)
                bindOption = 0;
            if (riskDetail.RiskCoverages[bindOption].GLBILimitId.HasValue 
                && riskDetail.RiskCoverages[bindOption].GLBILimitId != LimitConstants.GLNoneCoverageId
                && riskDetail.RiskCoverages[bindOption].GLBILimitId != LimitConstants.GLNoneCoverageIdv7
                || isGlSelected)
            {
                var glForm = conditionalForms.FirstOrDefault(cf => cf.Id == "GeneralLiabilityEndorsement");
                riskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), IsSelected = true, Form = glForm, FormId = "GeneralLiabilityEndorsement", Other = isGlSelected ? glOther : null });
            }

            var pncs = riskDetail.AdditionalInterest.Where(ai => ai.IsPrimaryNonContributory);
            if (!pncs.Any() && !isEndorsement && riskDetail.BlanketCoverage.IsPrimaryNonContributory.GetValueOrDefault())
                riskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), IsSelected = true, FormId = "PrimaryAndNoncontributory", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "PrimaryAndNoncontributory") });
            else if (pncs.Any())
            {
                int fetchCount = riskDetail.BlanketCoverage.IsPrimaryNonContributory.GetValueOrDefault() ? 1 : pncs.Count();
                riskForms.AddRange(pncs.Take(fetchCount).Select(pnc => new RiskFormDTO { Id = pnc.Id, IsSelected = true, FormId = "PrimaryAndNoncontributory", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "PrimaryAndNoncontributory") }));
            }

            var waivers = riskDetail.AdditionalInterest.Where(ai => ai.IsWaiverOfSubrogation);
            if (!waivers.Any() && !isEndorsement && riskDetail.BlanketCoverage.IsWaiverOfSubrogation.GetValueOrDefault())
                riskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), IsSelected = true, FormId = "WaiverOfTransferOfRights", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "WaiverOfTransferOfRights") });
            else if (waivers.Any())
            {
                int fetchCount = riskDetail.BlanketCoverage.IsWaiverOfSubrogation.GetValueOrDefault() ? 1 : waivers.Count();
                riskForms.AddRange(waivers.Take(fetchCount).Select(waiver => new RiskFormDTO { Id = waiver.Id, IsSelected = true, FormId = "WaiverOfTransferOfRights", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "WaiverOfTransferOfRights") }));
            }

            var additionalInsureds = riskDetail.AdditionalInterest.Where(ai => ai.AdditionalInterestTypeId == 1);
            if (!additionalInsureds.Any() && !isEndorsement && riskDetail.BlanketCoverage.IsAdditionalInsured.GetValueOrDefault())
                riskForms.Add(new RiskFormDTO { Id = Guid.NewGuid(), IsSelected = true, FormId = "AdditionalInsuredBlanket", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "AdditionalInsuredBlanket") });
            else if (additionalInsureds.Any())
            {
                int fetchCount = riskDetail.BlanketCoverage.IsAdditionalInsured.GetValueOrDefault() ? 1 : additionalInsureds.Count();
                riskForms.AddRange(additionalInsureds.Take(fetchCount).Select(ai => new RiskFormDTO { Id = ai.Id, IsSelected = true, FormId = "AdditionalInsuredBlanket", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "AdditionalInsuredBlanket") }));
            }

            var designatedInsureds = riskDetail.AdditionalInterest.Where(ai => ai.AdditionalInterestTypeId == 12);
            riskForms.AddRange(designatedInsureds.Select(di => new RiskFormDTO { Id = di.Id, IsSelected = true, FormId = "DesignatedInsured", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "DesignatedInsured") }));

            var lossPayees = riskDetail.AdditionalInterest.Where(ai => ai.AdditionalInterestTypeId == 2);
            riskForms.AddRange(lossPayees.Select(di => new RiskFormDTO { Id = di.Id, IsSelected = true, Other = lossPayeeOther, FormId = "LossPayeeEndorsement", Form = conditionalForms.FirstOrDefault(cf => cf.Id == "LossPayeeEndorsement") }));

            riskForms.ForEach(rf => rf.Form.LatestFormVersion(riskDetail.RequestedQuoteDate, risk.IsRaterVersionLocked));
            return riskForms;
        }

        public async Task<RiskDetail> GetPreviousPolicyDetail(Guid forEndorsementRiskDetailId, Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            //var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == riskId && riskDetail.Id != currentRiskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            //var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == riskId && riskDetail.Id != currentRiskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            //var riskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var riskDetailId = risk.CurrentRiskDetailId.Value;
            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;
            //RiskCoverage boundRiskCoverage = toCloneRiskDetail.RiskCoverages.Single(rc => rc.OptionNumber == bindOption);

            // save file upload documents data first
            var fileUploadDocuments = (await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == forEndorsementRiskDetailId)).ToList();
            // save current addresses
            var insuredAddress = await _riskDetailRepository.GetInsuredAddressAsync(riskDetailId);
            var currentAddresses = insuredAddress.Insured.Entity.EntityAddresses;

            var backupRiskResponse = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            var backupRiskHistories = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            var backupClaims = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == riskDetailId);

            // delete current policy details
            await DeleteCurrentPolicyDetail(forEndorsementRiskDetailId, riskDetailId);

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = toCloneRiskDetail.DeepClone();
            newRiskDetail.Id = forEndorsementRiskDetailId;
            newRiskDetail.Status = RiskDetailStatus.Active;
            newRiskDetail.CreatedDate = DateTime.Now;
            newRiskDetail.VersionNumber = -1;
            newRiskDetail.FromReinstatement = toCloneRiskDetail.FromReinstatement;

            //newRiskDetail.FirstIssueDate = DateTime.Now;
            #endregion

            #region RiskResponse
            if (backupRiskResponse != null)
                newRiskDetail.RiskResponses = backupRiskResponse.Select(x => (RiskResponse)SetId(x, forEndorsementRiskDetailId)).ToList();
            #endregion

            #region RiskAdditionalInterest
            var riskAdditionalInterest = await _riskAdditionalInterestRepository.GetAllIncludeAsync(riskDetailId);
            if (riskAdditionalInterest != null)
            {
                newRiskDetail.AdditionalInterests = new List<RiskAdditionalInterest>();
                foreach (var item in riskAdditionalInterest)
                {
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.RiskDetailId = forEndorsementRiskDetailId;
                    newAdditionalInterest.Entity.Id = Empty;
                    foreach (var x in newAdditionalInterest.Entity.EntityAddresses)
                    {
                        x.Id = Empty;
                        x.Address.Id = Empty;
                    }

                    newRiskDetail.AdditionalInterests.Add(newAdditionalInterest);
                }
            }
            #endregion

            #region RiskFilings

            // save RiskFilings data first
            var riskFilings = await _riskFilingRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskFilings != null)
            {
                newRiskDetail.RiskFilings = new List<RiskFiling>();
                foreach (var riskFiling in riskFilings)
                {
                    riskFiling.Id = Guid.Empty;
                    riskFiling.RiskDetailId = forEndorsementRiskDetailId;
                    newRiskDetail.RiskFilings.Add(riskFiling);
                }
            }
            #endregion

            #region RiskCoverages
            if (newRiskDetail.RiskCoverages != null)
            {
                newRiskDetail.RiskCoverages = newRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).Select(x => (RiskCoverage)SetId(x, forEndorsementRiskDetailId)).ToList();
                foreach (var item in newRiskDetail.RiskCoverages)
                {
                    if (item.RiskCoveragePremium != null)
                    {
                        item.RiskCoveragePremium.Id = Empty;
                        if (item.RiskCoveragePremium.RiskCoveragePremiumAvg != null)
                        {
                            item.RiskCoveragePremium.RiskCoveragePremiumAvg.Id = Empty;
                        }
                    }
                }
            }
            #endregion

            #region RadiusOfOperation
            if (newRiskDetail.RadiusOfOperations != null)
                newRiskDetail.RadiusOfOperations.Id = forEndorsementRiskDetailId;
            #endregion

            #region CommoditiesHauled            
            var commoditiesHauleds = newRiskDetail.CommoditiesHauled;
            if (commoditiesHauleds != null)
            {
                newRiskDetail.CommoditiesHauled = new List<RiskSpecificCommoditiesHauled>();
                foreach (var item in commoditiesHauleds)
                {
                    var newCommoditiesHauled = SystemExtension.Clone<RiskSpecificCommoditiesHauled>(item);
                    newCommoditiesHauled.Id = Guid.NewGuid();
                    newCommoditiesHauled.RiskDetailId = forEndorsementRiskDetailId;

                    newRiskDetail.CommoditiesHauled.Add(newCommoditiesHauled);
                }
            }
            #endregion

            #region DriverHeader
            if (newRiskDetail.DriverHeader != null)
            {
                newRiskDetail.DriverHeader.Id = Empty;
                newRiskDetail.DriverHeader.RiskDetailId = forEndorsementRiskDetailId;
            }
            #endregion

            #region DriverHiringCriteria
            var driverHiringCriteria = await _riskSpecificDriverHiringCriteriaRepository.GetWithIncludesNoTracking(riskDetailId);
            if (driverHiringCriteria != null)
            {
                var newDriverHiringCriteria = new RiskSpecificDriverHiringCriteria();
                newDriverHiringCriteria.Id = forEndorsementRiskDetailId;
                newDriverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years = driverHiringCriteria.AreDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years;
                newDriverHiringCriteria.DoDriversHave5YearsDrivingExperience = driverHiringCriteria.DoDriversHave5YearsDrivingExperience;
                newDriverHiringCriteria.IsAgreedToReportAllDriversToRivington = driverHiringCriteria.IsAgreedToReportAllDriversToRivington;
                newDriverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto = driverHiringCriteria.AreFamilyMembersUnder21PrimaryDriversOfCompanyAuto;
                newDriverHiringCriteria.AreDriversProperlyLicensedDotCompliant = driverHiringCriteria.AreDriversProperlyLicensedDotCompliant;
                newDriverHiringCriteria.IsDisciplinaryPlanDocumented = driverHiringCriteria.IsDisciplinaryPlanDocumented;
                newDriverHiringCriteria.IsThereDriverIncentiveProgram = driverHiringCriteria.IsThereDriverIncentiveProgram;

                newDriverHiringCriteria.BackGroundCheckIncludes = new List<Data.Entity.DataSets.BackgroundCheckOption>();
                if (driverHiringCriteria.BackGroundCheckIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.BackGroundCheckIncludes)
                    {
                        var optionFromDb = await _backgroundCheckOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.BackGroundCheckIncludes.Add(optionFromDb);
                        }
                    }
                }

                newDriverHiringCriteria.DriverTrainingIncludes = new List<Data.Entity.DataSets.DriverTrainingOption>();
                if (driverHiringCriteria.DriverTrainingIncludes.Count > 0)
                {
                    foreach (var option in driverHiringCriteria.DriverTrainingIncludes)
                    {
                        var optionFromDb = await _driverTrainingOptionRepository.FindAsync(option.Id);
                        if (optionFromDb != null)
                        {
                            newDriverHiringCriteria.DriverTrainingIncludes.Add(optionFromDb);
                        }
                    }
                }

                await _riskSpecificDriverHiringCriteriaRepository.AddAsync(newDriverHiringCriteria);
            }

            newRiskDetail.DriverHiringCriteria = null;
            #endregion

            #region DotInfo
            var newDotInfo = await _riskSpecificDotInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDotInfo != null)
            {
                newDotInfo.Id = forEndorsementRiskDetailId;
                await _riskSpecificDotInfoRepository.AddAsync(newDotInfo);
            }
            newRiskDetail.DotInfo = null;
            #endregion

            #region UnderwritingQuestions
            if (newRiskDetail.UnderwritingQuestions != null)
                newRiskDetail.UnderwritingQuestions.Id = forEndorsementRiskDetailId;
            #endregion

            #region Insured Subsidiary
            newRiskDetail.Insured = SystemExtension.Clone<Insured>(toCloneRiskDetail.Insured);
            if (newRiskDetail.Insured != null)
            {
                var newEntityId = Guid.NewGuid();
                var subsidiaries = await _entitySubsidiaryRepository.GetAsync(x => x.EntityId == newRiskDetail.Insured.EntityId);
                foreach (var sub in subsidiaries)
                {
                    var newSubsidiaries = SystemExtension.Clone<EntitySubsidiary>(sub);
                    newSubsidiaries.Id = Empty;
                    newSubsidiaries.EntityId = newEntityId;
                    await _entitySubsidiaryRepository.AddAsync(newSubsidiaries);
                }

                newRiskDetail.Insured.Id = Empty;
                newRiskDetail.Insured.Entity.Id = newEntityId;
                foreach (var x in newRiskDetail.Insured.Entity.EntityAddresses)
                {
                    var newAddressId = Guid.NewGuid();
                    x.Id = Empty;
                    x.Address.Id = newAddressId;
                }

                var newEntityAddresses = newRiskDetail.Insured.Entity.EntityAddresses.ToList();
                var newGaragingAddresses = newEntityAddresses.Where(x => x.AddressTypeId == (short?)AddressTypesEnum.Garaging);

                //foreach (var newGaragingAddress in newGaragingAddresses)
                //{
                //    newEntityAddresses = newEntityAddresses.Where(x => x != newGaragingAddress).ToList();
                //    var toCloneRiskDetailGaragagingAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newGaragingAddress.AddressId);
                //    var cloneGaragingAddress = toCloneRiskDetailGaragagingAddress?.Clone();

                //    cloneGaragingAddress.Id = Guid.Empty;
                //    cloneGaragingAddress.EntityId = newEntityId;
                //    cloneGaragingAddress.AddressId = toCloneRiskDetailGaragagingAddress.AddressId;
                //    cloneGaragingAddress.Address = null;
                //    newEntityAddresses.Add(cloneGaragingAddress);
                //}
                var newEntityAddressesToIssue = newEntityAddresses;
                newEntityAddresses = new List<EntityAddress>();

                foreach (var newEntityAddress in newEntityAddressesToIssue)
                {
                    var toCloneRiskDetailEntityAddress = toCloneRiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == newEntityAddress.AddressId);
                    var cloneEntityAddress = toCloneRiskDetailEntityAddress?.Clone();

                    cloneEntityAddress.Id = Guid.Empty;
                    cloneEntityAddress.EntityId = newEntityId;
                    cloneEntityAddress.AddressId = toCloneRiskDetailEntityAddress.AddressId;
                    cloneEntityAddress.Address = null;
                    newEntityAddresses.Add(cloneEntityAddress);
                }

                newRiskDetail.Insured.Entity.EntityAddresses = newEntityAddresses;
            }
            #endregion

            #region RiskHistory
            if (backupRiskHistories != null)
            {
                foreach (var x in backupRiskHistories)
                {
                    var newRiskHistory = SystemExtension.Clone<RiskHistory>(x);
                    newRiskHistory.Id = Empty;
                    newRiskHistory.RiskDetailId = forEndorsementRiskDetailId;
                    await _riskHistoryRepository.AddAsync(newRiskHistory);
                }
            }
            #endregion

            #region RiskSpecificSafetyDevice
            var riskSpecificSafetyDevice = await _riskSpecificSafetyDeviceRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskSpecificSafetyDevice != null)
            {
                foreach (var x in riskSpecificSafetyDevice)
                {
                    x.Id = 0;
                    x.RiskDetailId = forEndorsementRiskDetailId;
                    await _riskSpecificSafetyDeviceRepository.AddAsync(x);
                }
            }
            #endregion

            #region RiskNotes
            var riskNotes = await _riskNotesRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskNotes != null)
            {
                foreach (var riskNote in riskNotes)
                {
                    riskNote.Id = Empty;
                    riskNote.RiskDetailId = forEndorsementRiskDetailId;
                    await _riskNotesRepository.AddAsync(riskNote);
                }
            }
            #endregion

            #region RiskForms
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (riskForms != null)
            {
                foreach (var x in riskForms)
                {
                    var newRiskForms = SystemExtension.Clone<RiskForm>(x);
                    newRiskForms.Id = Empty;
                    newRiskForms.RiskDetailId = forEndorsementRiskDetailId;
                    await _riskFormRepository.AddAsync(newRiskForms);
                }
            }
            #endregion

            #region Manuscripts
            var manuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (manuscripts != null)
            {
                foreach (var x in manuscripts)
                {
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.RiskDetailId = forEndorsementRiskDetailId;
                    await _riskManuscriptsRepository.AddAsync(newManuscripts);
                }
            }
            #endregion

            #region BindingRequirements
            // should not include bind requirements and QC
            //var bindingRequirements = await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            //if (bindingRequirements != null)
            //{
            //    foreach (var x in bindingRequirements)
            //    {
            //        var newBindingRequirements = SystemExtension.Clone<BindingRequirements>(x);
            //        newBindingRequirements.Id = Empty;
            //        newBindingRequirements.RiskDetailId = currentRiskDetailId;
            //        await _bindingRequirementsRepository.AddAsync(newBindingRequirements);
            //    }
            //}
            #endregion

            #region QuoteConditions
            //var quoteCond = await _quoteConditionsRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            //if (quoteCond != null)
            //{
            //    foreach (var x in quoteCond)
            //    {
            //        var newQuoteCond = SystemExtension.Clone<QuoteConditions>(x);
            //        newQuoteCond.Id = Empty;
            //        newQuoteCond.RiskDetailId = currentRiskDetailId;
            //        await _quoteConditionsRepository.AddAsync(newQuoteCond);
            //    }
            //}
            #endregion

            #region Claims
            if (backupClaims != null)
            {
                foreach (var claim in backupClaims)
                {
                    var newClaim = SystemExtension.Clone<ClaimsHistory>(claim);
                    newClaim.Id = Empty;
                    newClaim.RiskDetailId = forEndorsementRiskDetailId;
                    await _claimsHistoryRepository.AddAsync(newClaim);
                }
            }
            #endregion

            #region Destinations
            var destinations = await _riskSpecificDestinationRepository.GetAsync(x => x.RiskDetailId == riskDetailId);
            if (destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var newDestination = SystemExtension.Clone<RiskSpecificDestination>(destination);
                    newDestination.Id = Empty;
                    newDestination.RiskDetailId = forEndorsementRiskDetailId;
                    await _riskSpecificDestinationRepository.AddAsync(newDestination);
                }
            }
            #endregion

            newRiskDetail.DriverInfo = null;

            if (fileUploadDocuments != null)
            {
                foreach (var item in fileUploadDocuments)
                {
                    item.Id = Guid.Empty;
                    item.RiskDetailId = forEndorsementRiskDetailId;
                }
                await _fileUploadDocumentRepository.AddRangeAsync(fileUploadDocuments);
            }

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles.Where(rdv => rdv.Vehicle.IsIncludedInOption(bindOption)).ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailVehicles();

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailDrivers = newRiskDetail.RiskDetailDrivers.ToList();
            // Then clear it to avoid error while adding 'newRiskDetail' in db context. 
            newRiskDetail.ClearRiskDetailDrivers();

            await _riskDetailRepository.AddAsync(newRiskDetail);

            #region Vehicles
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailVehicle in oldRiskDetailVehicles)
            {
                var vehicle = _vehicleRepository.GetById(riskDetailVehicle.VehicleId);  // Get vehicle.
                if (vehicle.PreviousVehicleVersionId != null)
                {
                    var previousVersion = _vehicleRepository.GetById(vehicle.PreviousVehicleVersionId.Value); // get previous version
                    vehicle.PreviousVehicleVersionId = (previousVersion is null) ? null : vehicle.PreviousVehicleVersionId; // check previous version if deleted already
                }
                var rdv = riskDetailVehicle.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                vehicle.RiskDetailVehicles.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _vehicleRepository.Update(vehicle);                                     // Update vehicle in db context.
            }
            await _vehicleRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region Drivers
            // After 'newRiskDetail' is added to db context, we now fetch the vehicles 
            // from 'oldRiskDetailVehicles' and add new RiskDetailVehicle reference 
            // with its RiskDetail set to 'newRiskDetail'
            foreach (var riskDetailDriver in oldRiskDetailDrivers)
            {
                var driver = _driverRepository.GetById(riskDetailDriver.DriverId);  // Get vehicle.
                if (driver.PreviousDriverVersionId != null)
                {
                    var previousVersion = _driverRepository.GetById(driver.PreviousDriverVersionId.Value); // get previous version
                    driver.PreviousDriverVersionId = (previousVersion is null) ? null : driver.PreviousDriverVersionId; // check previous version if deleted already
                }
                //if (driver.IsDeleted)
                //{
                //    var previousVersions = await _driverRepository.GetPreviousRiskDetailDrivers(newRiskDetail.RiskId); // get previous versions
                //    var isDriverAPreviousVersion = previousVersions.Any(x => x.Id == driver.Id);
                //    if (isDriverAPreviousVersion == false)
                //    {
                //        driver.IsActive = null; // re-enable record
                //        driver.DeletedDate = null; // re-enable record
                //    }
                //}

                bool hasBindOptionSelected = driver.Options != null && driver.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())) && !driver.IsDeleted;
                if (!hasBindOptionSelected && toCloneRiskDetail.Status == RiskDetailStatus.Quoted) continue; // any driver not bound from submision should not be carried over to policy

                var rdv = riskDetailDriver.WithNewRiskDetail(newRiskDetail);           // Create new RiskDetailVehicle with updated RiskDetail.
                driver.RiskDetailDrivers.Add(rdv);                                    // Add the new RiskDetailVehicle to vehicle.
                _driverRepository.Update(driver);                                     // Update drvr in db context.
            }
            await _driverRepository.SaveChangesAsync();                                // Save all vehicle changes.
            #endregion

            #region DriverInfo
            var newDriverInfo = await _riskSpecificDriverInfoRepository.FindAsync(x => x.Id == riskDetailId);
            if (newDriverInfo != null)
            {
                newDriverInfo.Id = forEndorsementRiskDetailId;
                newDriverInfo.TotalNumberOfDrivers = newRiskDetail.Drivers.Where(driver => ((driver.Options ?? "")?.Split(',')).Contains($"{bindOption}")).Count(); // should update always depending on bound drivers
                await _riskSpecificDriverInfoRepository.AddAsync(newDriverInfo);
            }
            #endregion

            await _riskDetailRepository.SaveChangesAsync();

            await _riskFormRepository.SaveChangesAsync();
            await _riskManuscriptsRepository.SaveChangesAsync();
            await _riskHistoryRepository.SaveChangesAsync();
            await _claimsHistoryRepository.SaveChangesAsync();
            await _riskSpecificSafetyDeviceRepository.SaveChangesAsync();
            await _riskSpecificDriverInfoRepository.SaveChangesAsync();
            await _riskSpecificDotInfoRepository.SaveChangesAsync();
            await _riskSpecificDriverHiringCriteriaRepository.SaveChangesAsync();
            await _fileUploadDocumentRepository.SaveChangesAsync();
            await _quoteConditionsRepository.SaveChangesAsync();
            _context.SaveChanges();


            #region RiskVehiclePremium
            var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id);
            foreach (var rvpColl in riskVehiclePremiums)
            {
                var cloneRvpColl = rvpColl.Clone();
                cloneRvpColl.ChangeRiskDetail(newRiskDetail.Id);
                await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            }
            #endregion

            #region RiskPolicyPremium (GL)
            //var riskPolicyPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_GL);
            //if(riskPolicyPremiums != null)
            //{
            //    var cloneRpp = riskPolicyPremiums.Clone();
            //    cloneRpp.ChangeRiskDetail(newRiskDetail.Id);
            //    await _riskPolicyPremiumRepository.SaveAsync(cloneRpp);
            //}
            #endregion

            #region RiskPolicyPremium (TI)
            var riskPolicyPremiumsTI = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_TI);
            if (riskPolicyPremiumsTI != null)
            {
                var cloneRppTI = riskPolicyPremiumsTI.Clone();
                cloneRppTI.ChangeRiskDetail(newRiskDetail.Id);
                await _riskPolicyPremiumRepository.SaveAsync(cloneRppTI);
            }
            #endregion

            #region RiskPolicyPremium (NO)
            var riskPolicyPremiumsNO = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(toCloneRiskDetail.Id, Premium.TYPE_POLICY_NO);
            if (riskPolicyPremiumsNO != null)
            {
                var cloneRppNO = riskPolicyPremiumsNO.Clone();
                cloneRppNO.ChangeRiskDetail(newRiskDetail.Id);
                await _riskPolicyPremiumRepository.SaveAsync(cloneRppNO);
            }
            #endregion

            return newRiskDetail;
        }

        private async Task DeleteCurrentPolicyDetail(Guid riskDetailId, Guid previousRiskDetailId)
        {
            var toDeleteRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);

            if (toDeleteRiskDetail == null) { return; }

            _riskDetailRepository.Detach(toDeleteRiskDetail);

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            executionStrategy.Execute(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
                // total clean up on data upon reset, to avoid data duplication
                try
                {
                    string deletePolicyDetail = string.Format(@"
                        DELETE FROM [dbo].[EntityAddress] WHERE [EntityId] = '{1}'
                        DELETE FROM [dbo].[EntitySubsidiary] WHERE [EntityId] = '{1}'
                        DELETE FROM [dbo].[RiskSpecificCommoditiesHauled] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDriverInfo] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[DriverHeader] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDriverHiringCriteria] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDotInfo] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDestination] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[ClaimsHistory] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskManuscripts] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificUnderwritingQuestion] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskCoveragePremiumAvg] WHERE [RiskCoveragePremiumId] IN 
                            (SELECT [Id] FROM [RiskCoveragePremium] WHERE [RiskCoverageId] IN (SELECT [Id] FROM [RiskCoverage] WHERE [RiskDetailId] = '{0}'))
                        DELETE FROM [dbo].[RiskCoveragePremium] WHERE [RiskCoverageId] IN (SELECT [Id] FROM [RiskCoverage] WHERE [RiskDetailId] = '{0}')
                        DELETE FROM [dbo].[RiskCoverage] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskHistory] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificSafetyDevice] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificRadiusOfOperation] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskFiling] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskForm] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[FileUploadDocument] WHERE [RiskDetailId] = '{0}'

                        UPDATE  [dbo].[Driver]
                        SET     [PreviousDriverVersionId] = NULL
                        WHERE   [PreviousDriverVersionId] IS NOT NULL 
                                AND [PreviousDriverVersionId] NOT IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{3}') 
                                AND [PreviousDriverVersionId] IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[Driver] 
                        WHERE   [Id] NOT IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{3}') 
                                AND [Id] IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{0}')

                        UPDATE  [dbo].[Vehicle]
                        SET     [PreviousVehicleVersionId] = NULL
                        WHERE   [PreviousVehicleVersionId] IS NOT NULL 
                                AND [PreviousVehicleVersionId] NOT IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{3}') 
                                AND [PreviousVehicleVersionId] IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[Vehicle]
                        WHERE   [Id] NOT IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{3}') 
                                AND [Id] IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[RIskDetailVehicle] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskDetailDriver] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[VehiclePremiumRatingFactor] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[VehiclePremium] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskResponse] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskAdditionalInterest] WHERE [RiskDetailId] = '{0}'                     
                        DELETE FROM [dbo].[RiskDetail] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[Insured] WHERE [Id] = '{2}'
                        DELETE FROM [dbo].[Entity] WHERE [Id] = '{1}'

                        DELETE FROM [dbo].[RiskVehiclePremiumCoverage] WHERE [Id] not in (SELECT CoveragePremiumId FROM RiskVehiclePremium) 
                        DELETE FROM [dbo].[RiskVehiclePremium] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskVehiclePremiumTransaction] WHERE [RiskDetailId] = '{0}'

                        DELETE FROM [dbo].[RiskPolicyPremiumCoverage] WHERE [Id] not in (SELECT CoveragePremiumId FROM RiskPolicyPremium) 
                        DELETE FROM [dbo].[RiskPolicyPremium] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskPolicyPremiumTransaction] WHERE [RiskDetailId] = '{0}'

                    ", riskDetailId, toDeleteRiskDetail.Insured.EntityId, toDeleteRiskDetail.InsuredId, previousRiskDetailId);

                    _context.Database.ExecuteSqlRaw(deletePolicyDetail);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
                transaction.Commit();
            });
        }

        public async Task<List<string>> GetPolicyChanges(Guid currentRiskDetailId, Guid riskId, bool includeAll = false)
        {
            // reset temp variables
            currentCargoCoverageValue = string.Empty;
            currentRefrigerationCoverageValue = string.Empty;

            var risk = await _riskRepository.FindAsync(riskId);
            var previousRiskDetailId = risk.CurrentRiskDetailId.Value;

            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(currentRiskDetailId);
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(riskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;
            var businessAddress = _entityAddressRepository.FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business && x.EntityId == currentRiskDetail.Insured.EntityId, i => i.Address).Result;
            _limitList = _limitsService.GetPerStateAsync(businessAddress.Address?.State, risk.RaterVersionId).Result;

            var changes = new List<string>();

            // Applicant - Business Name
            changes.AddRange(Compare(previousRiskDetail.Insured.Entity, currentRiskDetail.Insured.Entity, nameof(Entity)));
            // Applicant - Address
            changes.AddRange(CompareList(previousRiskDetail.Insured.Entity.EntityAddresses.AsQueryable(), currentRiskDetail.Insured.Entity.EntityAddresses.AsQueryable()));

            // Limits
            var previousRiskCoverage = previousRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            var currentRiskCoverage = currentRiskDetail.RiskCoverages.Where(x => x.OptionNumber == bindOption).FirstOrDefault();
            changes.AddRange(Compare(previousRiskCoverage, currentRiskCoverage, nameof(RiskCoverage)));

            // Additional Interest
            var previousAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId, i => i.Entity);
            var currentAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId, i => i.Entity);
            changes.AddRange(CompareList(previousAdditionalInterest.AsQueryable(), currentAdditionalInterest.AsQueryable()));

            // Manuscript
            var previousManuscript = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId);
            var currentManuscript = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId);
            changes.AddRange(CompareList(previousManuscript.AsQueryable(), currentManuscript.AsQueryable()));

            // Vehicle
            changes.AddRange(CompareList(previousRiskDetail.Vehicles.AsQueryable(), currentRiskDetail.Vehicles.AsQueryable(), bindOption, includeAll,risk.RaterVersionId));

            // driver header
            changes.AddRange(Compare(previousRiskDetail.DriverHeader, currentRiskDetail.DriverHeader, nameof(DriverHeader)));

            // Drivers
            changes.AddRange(CompareList(previousRiskDetail.Drivers.AsQueryable(), currentRiskDetail.Drivers.AsQueryable(), bindOption, includeAll));

            // Risk History
            // var previousRiskHistory = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId);
            // var currentRiskHistory = await _riskHistoryRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId);
            // changes.AddRange(CompareList(previousRiskHistory.AsQueryable(), currentRiskHistory.AsQueryable()));

            // Claims History
            // var previousClaimsHistory = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId);
            // var currentClaimsHistory = await _claimsHistoryRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId);
            // changes.AddRange(CompareList(previousClaimsHistory.AsQueryable(), currentClaimsHistory.AsQueryable()));

            // Responses
            // var previousRiskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId, x => x.Question);
            // var currentRiskResponses = await _riskResponseRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId, x => x.Question);
            // changes.AddRange(CompareList(previousRiskResponses.AsQueryable(), currentRiskResponses.AsQueryable()));

            // Commodities
            // var previousCommodities = previousRiskDetail.CommoditiesHauled;
            // var currentCommodities = currentRiskDetail.CommoditiesHauled;
            // changes.AddRange(CompareList(previousCommodities.AsQueryable(), currentCommodities.AsQueryable()));

            // Risk Coverage Premium
            // changes.AddRange(Compare(ValidateEndorsementPremiums(previousRiskCoverage), ValidateEndorsementPremiums(currentRiskCoverage), nameof(RiskCoveragePremium)));

            // Risk Forms
            var previousRiskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId, i => i.Form);
            var currentRiskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId, i => i.Form);
            changes.AddRange(CompareList(previousRiskForms.AsQueryable(), currentRiskForms.AsQueryable()));

            // Update RiskDetail Status to Pending Endorsement if changes > 0
            if (changes.Count() > 0 && currentRiskDetail.Status == RiskDetailStatus.Active)
            {
                currentRiskDetail.Status = RiskDetailStatus.PendingEndorsement;
                _riskDetailRepository.Update(currentRiskDetail);
                await _riskDetailRepository.SaveChangesAsync();
            }

            return changes;
        }

        #region Compare Policy Details

        public bool IsDriverChanged(Driver oldDriver, Driver newDriver)
        {
            var compareResult = new List<string>();
            compareResult.AddRange(Compare(oldDriver.Entity, newDriver.Entity, nameof(Entity), $"Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} "));
            compareResult.AddRange(Compare(oldDriver, newDriver, nameof(Driver), $"Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} "));
            if (compareResult.Count() > 0)
                return true;

            compareResult.AddRange(CompareList(oldDriver.DriverIncidents.AsQueryable(), newDriver.DriverIncidents.AsQueryable(), $"Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} "));
            if (compareResult.Count() > 0)
                return true;

            return false;
        }

        private List<string> Compare(object oldProp, object newProp, string path, string label = "", bool includeAll = false ,int? raterVersion = null)
        {
            var result = new List<string>();
            if (oldProp == null && newProp == null)
                return result;

            if ((oldProp ?? newProp) is IEnumerable<object>)
            {
                return result;
            }

            var propType = (oldProp ?? newProp).GetType();
            var propertyInfos = propType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propInfo in propertyInfos)
            {
                if (_paths.Any(x => x == $"{propType.Name}.{propInfo.Name}")
                    // && !propType.Name.Equals("ClaimsHistory")
                    // && !propType.Name.Equals("RiskHistory")
                    && !propType.Name.Equals("Vehicle")
                    && !propType.Name.Equals("Driver")
                    && !propType.Name.Equals("Entity")
                    && !propType.Name.Equals("EntityAddress")
                    && !propType.Name.Equals("Address")
                    && !propType.Name.Equals("DriverHeader")
                    && !propType.Name.Equals("DriverIncidents")
                    && !propType.Name.Equals("RiskAdditionalInterest")
                 // && !propType.Name.Equals("RiskCoveragePremium")
                 )
                    continue;
                _paths.Add($"{propType.Name}.{propInfo.Name}");

                var fullName = $"{path}.{propInfo.Name}";

                // Skip excluded fields.
                if (!FieldLabels.Labels.ContainsKey(fullName) && !(includeAll && FieldLabels.ExcludedLabels.ContainsKey(fullName))) continue;

                object oldValue = null;
                object newValue = null;

                if (oldProp != null) oldValue = propInfo.GetValue(oldProp, null);
                if (newProp != null) newValue = propInfo.GetValue(newProp, null);

                var nullableType = Nullable.GetUnderlyingType(propInfo.PropertyType);

                var isNullablePrimitive = (nullableType != null && (nullableType.IsPrimitive
                    || nullableType == typeof(string)
                    || nullableType == typeof(decimal)
                    || propInfo == typeof(double)
                    || propInfo == typeof(short)
                    || propInfo == typeof(int)
                    || propInfo == typeof(long)
                    || propInfo == typeof(bool)
                ));

                if (propInfo.PropertyType.IsPrimitive
                    || propInfo.PropertyType == typeof(string)
                    || propInfo.PropertyType == typeof(decimal)
                    || propInfo.PropertyType == typeof(double)
                    || propInfo.PropertyType == typeof(short)
                    || propInfo.PropertyType == typeof(int)
                    || propInfo.PropertyType == typeof(long)
                    || propInfo.PropertyType == typeof(bool)
                    || propInfo.PropertyType == typeof(DateTime?)
                    || propInfo.PropertyType == typeof(Guid?)
                    || isNullablePrimitive)
                {
                    if (propInfo.PropertyType == typeof(bool) || propInfo.PropertyType == typeof(bool?))
                    {
                        oldValue = Convert.ToBoolean(oldValue ?? false).ToYesNo();
                        newValue = Convert.ToBoolean(newValue ?? false).ToYesNo();
                    }

                    if (propInfo.PropertyType == typeof(decimal?))
                    {
                        oldValue = Convert.ToDecimal(oldValue ?? 0);
                        newValue = Convert.ToDecimal(newValue ?? 0);
                    }

                    if (((newValue == null && oldValue != null) || (newValue == "No" && oldValue == "Yes")))
                    {
                        if (FormatPropertyName(fullName) == "Main Garaging Address" || FormatPropertyName(fullName) == "Is MVR?" || FormatPropertyName(fullName) == "Is Out of State Driver?"
                            || FormatPropertyName(fullName) == "Added First Party Benefits"
                            || FormatPropertyName(fullName) == "Combination First Party Benefits")
                        {
                            result.Add($"Changed {label} {FormatPropertyName(fullName)} From: '{oldValue}' To: '{newValue}'");
                        }
                        else
                        {
                            if (path == nameof(Vehicle))
                            {
                                if (fullName == "Vehicle.HasCoverageCargo") result.Add($"Removed {label}{FormatPropertyName(fullName)} '{currentCargoCoverageValue}'");
                                if (fullName == "Vehicle.HasCoverageRefrigeration") result.Add($"Removed {label}{FormatPropertyName(fullName)} '{currentRefrigerationCoverageValue}'");
                                if (fullName == "Vehicle.CoverageCollisionDeductibleId") result.Add($"Removed {label}{FormatPropertyName(fullName)} '{FormatValues(fullName, oldValue)}'");
                                if (fullName == "Vehicle.CoverageFireTheftDeductibleId") result.Add($"Removed {label}{FormatPropertyName(fullName)} '{FormatValues(fullName, oldValue)}'");
                            }
                            else
                            {
                                result.Add($"Removed {label}{FormatPropertyName(fullName)}");
                            }
                        }
                    }
                    else if (propInfo.PropertyType == typeof(string) && Convert.ToString(newValue) == String.Empty && oldValue == null)
                    {
                        continue;
                    }
                    else if (propInfo.PropertyType == typeof(decimal?) && Convert.ToDecimal(newValue ?? 0) == 0 && Convert.ToDecimal(oldValue ?? 0) == 0)
                    {
                        continue;
                    }
                    else if ((path == nameof(RiskCoveragePremium) || path == nameof(Vehicle) || path == nameof(RiskHistory) || path == nameof(ClaimsHistory))
                                && propInfo.PropertyType == typeof(decimal?) && Convert.ToDecimal(newValue ?? 0) != 0 && Convert.ToDecimal(oldValue ?? 0) == 0)
                    {
                        result.Add($"Added {label}{FormatPropertyName(fullName)}: '{FormatValues(fullName, newValue)}'");
                    }
                    else if ((newValue != null && oldValue == null))
                    {
                        if (propInfo.PropertyType == typeof(DateTime?))
                        {
                            result.Add($"Added {label}{FormatPropertyName(fullName)}: '{newValue:MM/dd/yyyy}'");
                        }
                        else if (propInfo.PropertyType == typeof(int?) || propInfo.PropertyType == typeof(short?))
                        {
                            if (path == nameof(Vehicle) || path == nameof(RiskCoverage))
                            {
                                result.Add($"Added {label}{FormatPropertyName(fullName)}: '{FormatValues(fullName, newValue)}'");
                            }
                            else
                            {
                                result.Add($"Added {label}{FormatPropertyName(fullName)}: '{newValue:N0}'");
                            }
                        }
                        else
                        {
                            result.Add($"Added {label}{FormatPropertyName(fullName)}: '{FormatValues(fullName, newValue)}'");
                        }
                    }
                    else if (propInfo.PropertyType == typeof(DateTime?) && $"{oldValue:MM/dd/yyyy}" != $"{newValue:MM/dd/yyyy}")
                    {
                        // result.Add($"Changed {label}{FormatPropertyName(fullName)} From: {oldValue:MM/dd/yyyy} To: {newValue:MM/dd/yyyy}");
                        result.Add($"Changed {label}{FormatPropertyName(fullName)} To: {newValue:MM/dd/yyyy}");
                    }
                    else if (newValue == "Yes" && oldValue == "No" && path == nameof(Vehicle))
                    {
                        if (fullName == "Vehicle.HasCoverageCargo") result.Add($"Added {label}{FormatPropertyName(fullName)} To: '{currentCargoCoverageValue}'");
                        if (fullName == "Vehicle.HasCoverageRefrigeration") result.Add($"Added {label}{FormatPropertyName(fullName)} To: '{currentRefrigerationCoverageValue}'");
                    }
                    else if (newValue != null && oldValue != null && !oldValue.Equals(newValue))
                    {
                        switch (fullName) // ui computed values in Vehicle Forms
                        {
                            case "Vehicle.Year":
                                int currentYear = DateTime.Now.Year;
                                var newYear = Convert.ToInt32(newValue);
                                var oldYear = Convert.ToInt32(oldValue);
                                int newVehicleAge = (currentYear) - (newYear > currentYear ? currentYear : newYear);
                                int oldVehicleAge = (currentYear) - (oldYear > currentYear ? currentYear : oldYear);
                                // result.Add($"Changed {label} Age From: '{FormatValues(fullName, oldVehicleAge)}' To: '{FormatValues(fullName, newVehicleAge)}'");
                                if (newVehicleAge != oldVehicleAge)
                                    result.Add($"Changed {label} Age To: '{FormatValues(fullName, newVehicleAge)}'");
                                break;
                            case "Vehicle.GrossVehicleWeightId":
                                // result.Add($"Changed {label}{FormatPropertyName("Vehicle.GrossVehicleWeightCategory")} From: '{FormatValues("Vehicle.GrossVehicleWeightCategory", oldValue)}' To: '{FormatValues("Vehicle.GrossVehicleWeightCategory", newValue)}'");
                                result.Add($"Changed {label}{FormatPropertyName("Vehicle.GrossVehicleWeightCategory")} To: '{FormatValues("Vehicle.GrossVehicleWeightCategory", newValue , raterVersion)}'");
                                break;
                            default:
                                break;
                        }

                        // result.Add($"Changed {label}{FormatPropertyName(fullName)} From: '{FormatValues(fullName, oldValue)}' To: '{FormatValues(fullName, newValue)}'");
                        result.Add($"Changed {label}{FormatPropertyName(fullName)} To: '{FormatValues(fullName, newValue)}'");
                    }

                    if (fullName == "RiskCoverage.GLBILimitId")
                    {
                        if (FormatValues(fullName, oldValue) == "NONE" && FormatValues(fullName, newValue) != "NONE")
                        {
                            result.Add($"Added Form 'General Liability Endorsement'");
                        }

                        if (FormatValues(fullName, oldValue) != "NONE" && FormatValues(fullName, newValue) == "NONE")
                        {
                            result.Add($"Removed Form 'General Liability Endorsement'");
                        }
                    }

                    if (fullName == "RiskCoverage.CargoLimitId")
                    {
                        currentCargoCoverageValue = (FormatValues(fullName, newValue) == "NONE") ? FormatValues(fullName, oldValue) : FormatValues(fullName, newValue);
                    }

                    if (fullName == "RiskCoverage.RefCargoLimitId")
                    {
                        currentRefrigerationCoverageValue = (FormatValues(fullName, newValue) == "NONE") ? FormatValues(fullName, oldValue) : FormatValues(fullName, newValue);
                    }
                }
                else if (propInfo.PropertyType.IsArray)
                {
                    return result;
                }
                else
                {
                    if (newValue != null || oldValue != null)
                    {
                        result.AddRange(Compare(oldValue, newValue, fullName));
                    }
                }
            }

            return result;
        }

        private List<string> CompareList(IQueryable<RiskAdditionalInterest> oldList, IQueryable<RiskAdditionalInterest> newList)
        {
            var result = new List<string>();
            var formChanges = new List<string>();

            var addedInterests = newList.Where(newObj => oldList.All(oldObj => oldObj.AddProcessDate != newObj.AddProcessDate));

            foreach (var interest in addedInterests)
            {
                var interestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == interest.EntityId, e => e.Address).Result;
                string interestAddress = $"{interestEntityAddress.Address.StreetAddress1}, {interestEntityAddress.Address.City}, {interestEntityAddress.Address.State}, {interestEntityAddress.Address.ZipCode}";
                result.Add($"Added Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", interest.AdditionalInterestTypeId)}': '{interest.Entity.FullName}', '{interestAddress}'");
            }

            foreach (var oldInterest in oldList.OrderBy(x => x.AddProcessDate))
            {
                var interest = newList.FirstOrDefault(newObj => newObj.AddProcessDate.Ticks == oldInterest.AddProcessDate.Ticks);

                if (interest is null) continue;

                var interestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == interest.EntityId, e => e.Address).Result;

                string interestAddress = $"{interestEntityAddress.Address.StreetAddress1}, {interestEntityAddress.Address.City}, {interestEntityAddress.Address.State}, {interestEntityAddress.Address.ZipCode}";

                if (interest.RemoveProcessDate != null && oldInterest.RemoveProcessDate == null)
                {
                    result.Add($"Removed Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", interest.AdditionalInterestTypeId)}': '{interest.Entity.FullName}', '{interestAddress}'");
                }
                else
                {
                    var oldInterestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == oldInterest.EntityId, e => e.Address).Result;
                    var currentInterestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == interest.EntityId, e => e.Address).Result;
                    string oldInterestAddressStr = $"{oldInterestEntityAddress.Address.StreetAddress1}, {oldInterestEntityAddress.Address.City}, {oldInterestEntityAddress.Address.State}, {oldInterestEntityAddress.Address.ZipCode}";
                    result.AddRange(Compare(oldInterest.Entity, interest.Entity, nameof(Entity), $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': "));
                    result.AddRange(Compare(oldInterestEntityAddress.Address, currentInterestEntityAddress.Address, nameof(Address), $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': "));
                    result.AddRange(Compare(oldInterest, interest, nameof(RiskAdditionalInterest), $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': "));
                }
            }

            if (oldList.Count(x => x.AdditionalInterestTypeId == 1 && x.RemoveProcessDate == null) == 0 && newList.Count(x => x.AdditionalInterestTypeId == 1 && x.RemoveProcessDate == null) > 0) // Additional Insured
            {
                formChanges.Add("Added Form 'Additional Insured Endorsement'");
            }

            if (oldList.Count(x => x.IsPrimaryNonContributory && x.RemoveProcessDate == null) == 0 && newList.Count(x => x.IsPrimaryNonContributory && x.RemoveProcessDate == null) > 0)
            {
                formChanges.Add("Added Form 'Primary and Noncontributory'");
            }

            if (oldList.Count(x => x.IsWaiverOfSubrogation && x.RemoveProcessDate == null) == 0 && newList.Count(x => x.IsWaiverOfSubrogation && x.RemoveProcessDate == null) > 0)
            {
                formChanges.Add("Added Form 'Waiver of Transfer of Right'");
            }

            if (oldList.Count(x => x.AdditionalInterestTypeId == 2 && x.RemoveProcessDate == null) == 0 && newList.Count(x => x.AdditionalInterestTypeId == 2 && x.RemoveProcessDate == null) > 0) // Loss Payee
            {
                formChanges.Add("Added Form 'Loss Payee Endorsement'");
            }

            if (oldList.Count(x => x.AdditionalInterestTypeId == 12 && x.RemoveProcessDate == null) == 0 && newList.Count(x => x.AdditionalInterestTypeId == 12 && x.RemoveProcessDate == null) > 0) // Designated Insured
            {
                formChanges.Add("Added Form 'Designated Insured'");
            }

            if (oldList.Count(x => x.AdditionalInterestTypeId == 1 && x.RemoveProcessDate == null) > 0 && newList.Count(x => x.AdditionalInterestTypeId == 1 && x.RemoveProcessDate == null) == 0) // Additional Insured
            {
                formChanges.Add("Removed Form 'Additional Insured Endorsement'");
            }

            if (oldList.Count(x => x.IsPrimaryNonContributory && x.RemoveProcessDate == null) > 0 && newList.Count(x => x.IsPrimaryNonContributory && x.RemoveProcessDate == null) == 0)
            {
                formChanges.Add("Removed Form 'Primary and Noncontributory'");
            }

            if (oldList.Count(x => x.IsWaiverOfSubrogation && x.RemoveProcessDate == null) > 0 && newList.Count(x => x.IsWaiverOfSubrogation && x.RemoveProcessDate == null) == 0)
            {
                formChanges.Add("Removed Form 'Waiver of Transfer of Right'");
            }

            if (oldList.Count(x => x.AdditionalInterestTypeId == 2 && x.RemoveProcessDate == null) > 0 && newList.Count(x => x.AdditionalInterestTypeId == 2 && x.RemoveProcessDate == null) == 0) // Loss Payee
            {
                formChanges.Add("Removed Form 'Loss Payee Endorsement'");
            }

            if (oldList.Count(x => x.AdditionalInterestTypeId == 12 && x.RemoveProcessDate == null) > 0 && newList.Count(x => x.AdditionalInterestTypeId == 12 && x.RemoveProcessDate == null) == 0) // Designated Insured
            {
                formChanges.Add("Removed Form 'Designated Insured'");
            }

            if (formChanges.Count > 0) result.AddRange(formChanges);

            return result;
        }

        private List<string> CompareList(IQueryable<RiskSpecificCommoditiesHauled> oldList, IQueryable<RiskSpecificCommoditiesHauled> newList)
        {
            var result = new List<string>();

            var addedCommodity = newList.Where(newObj => oldList.All(oldObj => oldObj.CreatedDate != newObj.CreatedDate));
            var removedCommodity = oldList.Where(oldObj => newList.All(newObj => newObj.CreatedDate != oldObj.CreatedDate));

            foreach (var commodity in addedCommodity)
            {
                result.Add($"Added Commodity '{commodity.Commodity}'; '{commodity.PercentageOfCarry:N1}%' of Carry");
            }

            foreach (var commodity in removedCommodity)
            {
                result.Add($"Removed Commodity '{commodity.Commodity}'; '{commodity.PercentageOfCarry:N1}%' of Carry");
            }

            foreach (var oldCommodity in oldList)
            {
                var currentCommodity = newList.FirstOrDefault(x => x.CreatedDate.Ticks == oldCommodity.CreatedDate.Ticks);

                if (currentCommodity is null) continue;

                result.AddRange(Compare(oldCommodity, currentCommodity, nameof(RiskSpecificCommoditiesHauled), $"{oldCommodity.Commodity}: "));
            }

            return result;
        }

        private List<string> CompareList(IQueryable<RiskManuscripts> oldList, IQueryable<RiskManuscripts> newList)
        {
            var result = new List<string>();
            var formChanges = new List<string>();

            var addedManuscripts = newList.Where(newObj => oldList.All(oldObj => oldObj.CreatedDate != newObj.CreatedDate) && newObj.DeletedDate == null);

            foreach (var manuscript in addedManuscripts)
            {
                result.Add($"Added Manuscript '{manuscript.Title}'");
                formChanges.Add($"Added Form 'Manuscript {manuscript.Title}'");
            }

            foreach (var oldManuscript in oldList.OrderBy(x => x.Title))
            {
                var newManuscript = newList.FirstOrDefault(x => x.CreatedDate == oldManuscript.CreatedDate);

                if (newManuscript is null) continue;

                if ((newManuscript.DeletedDate != null && oldManuscript.DeletedDate == null))
                {
                    result.Add($"Removed Manuscript '{oldManuscript.Title}'");
                    formChanges.Add($"Removed Form 'Manuscript {oldManuscript.Title}'");
                    continue;
                }
                else
                {
                    var compareResult = Compare(oldManuscript, newManuscript, nameof(RiskManuscripts), $"{oldManuscript.Title} ");
                    result.AddRange(compareResult);
                }
            }

            if (formChanges.Count > 0) result.AddRange(formChanges);

            return result;
        }

        private List<string> CompareList(IQueryable<RiskHistory> oldList, IQueryable<RiskHistory> newList)
        {
            var result = new List<string>();

            foreach (var oldRiskHistory in oldList)
            {
                var newRiskHistory = newList.FirstOrDefault(x => x.HistoryYear == oldRiskHistory.HistoryYear);

                if (newRiskHistory != null)
                {
                    var compareResult = Compare(oldRiskHistory, newRiskHistory, nameof(RiskHistory), $"{GetHistoryYear(newRiskHistory.HistoryYear.Value)} ");
                    result.AddRange(compareResult);
                }
            }

            return result;
        }

        private List<string> CompareList(IQueryable<ClaimsHistory> oldList, IQueryable<ClaimsHistory> newList)
        {
            var result = new List<string>();

            foreach (var oldClaimsHistory in oldList)
            {
                var newClaimsHistory = newList.FirstOrDefault(x => x.Order == oldClaimsHistory.Order);

                if (newClaimsHistory != null)
                {
                    var compareResult = Compare(oldClaimsHistory, newClaimsHistory, nameof(ClaimsHistory), $"{GetHistoryYear(newClaimsHistory.Order.Value)} ");
                    result.AddRange(compareResult);
                }
            }

            return result;
        }

        private List<string> CompareList(IQueryable<RiskResponse> oldList, IQueryable<RiskResponse> newList)
        {
            var result = new List<string>();

            var addedResponses = newList.Where(newObj => oldList.All(oldObj => oldObj.QuestionId != newObj.QuestionId)).GroupBy(x => x.QuestionId).Select(x => x.First()).Distinct();
            var removedResponses = oldList.Where(oldObj => newList.All(newObj => newObj.QuestionId != oldObj.QuestionId));

            var skippedResponses = new string[] { "haveOperationsOtherThanTruckingExplain", "hasApplicantHadGeneralLiabilityLossExplain", "areCommoditiesStoredInTruckOvernightExplain", "hadCargoLossExplain" };

            foreach (var response in addedResponses)
            {
                if (skippedResponses.Contains(response.Question.Description)) continue;
                if (response.ResponseValue is null) continue;
                //result.Add($"Added '{FormatPropertyName(response.Question.Description)}': '{(response.ResponseValue == "True" ? "Yes" : "No")}'");
                result.Add($"Added '{FormatPropertyName(response.Question.Description)}'");
            }

            foreach (var response in removedResponses)
            {
                if (skippedResponses.Contains(response.Question.Description)) continue;
                result.Add($"Removed '{FormatPropertyName(response.Question.Description)}'");
            }

            foreach (var oldResponses in oldList.GroupBy(x => x.QuestionId).Select(x => x.First()).Distinct())
            {
                var newResponses = newList.FirstOrDefault(x => x.QuestionId == oldResponses.QuestionId);

                if (newResponses != null)
                {
                    if (!FieldLabels.Labels.ContainsKey(newResponses.Question.Description)) continue;
                    if (oldResponses.ResponseValue != null && newResponses.ResponseValue != null && !newResponses.ResponseValue.Equals(oldResponses.ResponseValue))
                    {
                        if (newResponses.Question.Description != "glAnnualPayroll")
                        {
                            string oldResponse = oldResponses.ResponseValue == "True" ? "Yes" : "No";
                            string newResponse = newResponses.ResponseValue == "True" ? "Yes" : "No";
                            result.Add($"Changed '{FormatPropertyName(newResponses.Question.Description)}' From: '{oldResponse}' To: '{newResponse}'");
                        }
                        else
                        {
                            result.Add($"Changed '{FormatPropertyName(newResponses.Question.Description)}' From: '${Convert.ToDecimal(oldResponses.ResponseValue ?? "0"):N0}' To: '${Convert.ToDecimal(newResponses.ResponseValue ?? "0"):N0}'");
                        }
                    }
                }
            }

            return result;
        }

        private List<string> CompareList(IQueryable<Vehicle> oldList, IQueryable<Vehicle> newList, int bindOption, bool includeAll = false,int? raterVersion = null)
        {
            var result = new List<string>();

            var addedVehicles = newList.Where(newObj => oldList.All(oldObj => oldObj.VIN != newObj.VIN) && newObj.DeletedDate == null);

            foreach (var vehicle in addedVehicles)
            {
                result.Add($"Added VIN '{vehicle.VIN}'");
            }

            foreach (var oldVehicle in oldList.OrderBy(x => x.VIN))
            {
                var newVehicle = newList.FirstOrDefault(vehicle => vehicle.VIN == oldVehicle.VIN && (vehicle.PreviousVehicleVersionId ?? Empty) == oldVehicle.Id);

                if (newVehicle is null) continue;

                if (newVehicle.IsDeleted && newList.Any(x => (x.PreviousVehicleVersionId ?? Empty) == oldVehicle.Id))
                {
                    newVehicle = newList.FirstOrDefault(vehicle => vehicle.VIN == oldVehicle.VIN && (vehicle.PreviousVehicleVersionId ?? Empty) == oldVehicle.Id);
                }

                if (newVehicle.IsDeleted && !oldVehicle.IsDeleted)
                {
                    result.Add($"Removed VIN '{oldVehicle.VIN}'");
                    continue;
                }

                if (!newVehicle.IsDeleted && !oldVehicle.IsDeleted || (newVehicle.PreviousVehicleVersionId ?? Empty) == oldVehicle.Id)
                {
                    var compareResult = Compare(oldVehicle, newVehicle, nameof(Vehicle), $"VIN {oldVehicle.VIN} ", includeAll, raterVersion);
                    result.AddRange(compareResult);
                }

                if ((newVehicle.ExpirationDate.Value >= oldVehicle.ExpirationDate.Value) && (!newVehicle.IsDeleted && oldVehicle.IsDeleted)) // reinstated vehicle
                {
                    result.Add($"Reinstated VIN '{newVehicle.VIN}'");
                }
            }

            return result;
        }

        private List<string> CompareList(IQueryable<Driver> oldList, IQueryable<Driver> newList, int bindOption, bool includeAll = false)
        {
            var result = new List<string>();

            var addedDrivers = newList.Where(newObj => oldList.All(oldObj => !oldObj.IsSameWith(newObj)) && newObj.DeletedDate == null);

            foreach (var driver in addedDrivers)
            {
                result.Add($"Added Driver '{driver.Entity.FirstName} {driver.Entity.LastName}'");
            }

            foreach (var oldDriver in oldList.OrderBy(x => x.Entity.FirstName))
            {
                var newDriver = newList.FirstOrDefault(driver => driver.LicenseNumber == oldDriver.LicenseNumber && driver.Id == oldDriver.Id);

                if (newDriver is null)
                {
                    var isDeletedDriver = newList.FirstOrDefault(driver => driver.LicenseNumber == oldDriver.LicenseNumber && (driver.PreviousDriverVersionId ?? driver.Id) == oldDriver.Id);

                    if (isDeletedDriver is null) continue;

                    newDriver = isDeletedDriver;
                }

                if (newDriver.IsDeleted && newList.Any(x => (x.PreviousDriverVersionId ?? Empty) == oldDriver.Id))
                {
                    newDriver = newList.FirstOrDefault(driver => driver.LicenseNumber == oldDriver.LicenseNumber && (driver.PreviousDriverVersionId ?? Empty) == oldDriver.Id);
                }

                if (newDriver.IsDeleted && !oldDriver.IsDeleted) // all previous records are deleted
                {
                    result.Add($"Removed Driver '{oldDriver.Entity.FirstName} {oldDriver.Entity.LastName}'");
                    continue;
                }

                if (!newDriver.IsDeleted && !oldDriver.IsDeleted || ((newDriver.PreviousDriverVersionId ?? Empty) == oldDriver.Id && !newDriver.IsDeleted))
                {
                    var compareResult = new List<string>();
                    compareResult.AddRange(Compare(oldDriver.Entity, newDriver.Entity, nameof(Entity), $"Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} "));
                    compareResult.AddRange(Compare(oldDriver, newDriver, nameof(Driver), $"Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} ", includeAll));
                    if (includeAll)
                    {
                        if (!(newDriver.Options ?? "").Split(',').Any(x => x == bindOption.ToString()) && (oldDriver.Options ?? "").Split(',').Any(x => x == bindOption.ToString()))
                        {
                            compareResult.Add($"Changed Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} Pass Rating? From: 'Yes' To: 'No'");
                        }
                        else if ((newDriver.Options ?? "").Split(',').Any(x => x == bindOption.ToString()) && !(oldDriver.Options ?? "").Split(',').Any(x => x == bindOption.ToString()))
                        {
                            compareResult.Add($"Changed Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} Pass Rating? From: 'No' To: 'Yes'");
                        }
                    }
                    result.AddRange(compareResult);

                    // driver incidents
                    var changedDriverIncidentsData = _driverIncidentsRepository.GetAsync(x => x.DriverId == newDriver.Id).Result;
                    var oldDriverIncidentsData = _driverIncidentsRepository.GetAsync(x => x.DriverId == oldDriver.Id).Result;
                    result.AddRange(CompareList(oldDriverIncidentsData.AsQueryable(), changedDriverIncidentsData.AsQueryable(), $"Driver {oldDriver.Entity.FirstName} {oldDriver.Entity.LastName} "));
                }

                if (!newDriver.IsDeleted && oldDriver.IsDeleted) // reinstated driver
                {
                    result.Add($"Reinstated Driver '{newDriver.Entity.FirstName} {newDriver.Entity.LastName}'");
                }
            }

            return result;
        }

        private List<string> CompareList(IQueryable<DriverIncidents> oldList, IQueryable<DriverIncidents> newList, string label)
        {
            var result = new List<string>();
            label = $"Incident for {label}";

            var newDriverIncidents = newList.Where(newObj => oldList.All(oldObj => oldObj.UniqueId != newObj.UniqueId));
            var removedDriverIncidents = oldList.Where(oldObj => newList.All(newObj => newObj.UniqueId != oldObj.UniqueId));

            foreach (var newDriverIncident in newDriverIncidents)
            {
                string addedDriverIncidentStr = $"Added {label} '{FormatValues("DriverIncidents.IncidentType", newDriverIncident.IncidentType)}' Dated: '{FormatValues("DriverIncidents.IncidentDate", newDriverIncident.IncidentDate)}'";

                if (newDriverIncident.ConvictionDate != null) addedDriverIncidentStr += $" Conviction Date: '{FormatValues("DriverIncidents.ConvictionDate", newDriverIncident.ConvictionDate)}'";

                result.Add(addedDriverIncidentStr);
            }

            foreach (var removedDriverIncident in removedDriverIncidents)
            {
                string removedDriverIncidentStr = $"Removed {label} '{FormatValues("DriverIncidents.IncidentType", removedDriverIncident.IncidentType)}' Dated: '{FormatValues("DriverIncidents.IncidentDate", removedDriverIncident.IncidentDate)}'";

                if (removedDriverIncident.ConvictionDate != null) removedDriverIncidentStr += $" Conviction Date: '{FormatValues("DriverIncidents.ConvictionDate", removedDriverIncident.ConvictionDate)}'";

                result.Add(removedDriverIncidentStr);
            }

            foreach (var oldIncident in oldList)
            {
                var currentIncident = newList.Where(newObj => newObj.UniqueId == oldIncident.UniqueId).FirstOrDefault();
                string incidentType = FormatValues("DriverIncidents.IncidentType", oldIncident.IncidentType);
                result.AddRange(Compare(oldIncident, currentIncident, nameof(DriverIncidents), $"{label.Trim()}: {incidentType} "));
            }

            return result;
        }

        private List<string> CompareList(IQueryable<EntityAddress> oldList, IQueryable<EntityAddress> newList)
        {
            var result = new List<string>();

            var addedAddress = newList.Where(newObj => oldList.All(oldObj => oldObj.CreatedDate != newObj.CreatedDate));
            var removedAddress = oldList.Where(oldObj => newList.All(newObj => newObj.CreatedDate != oldObj.CreatedDate));

            foreach (var address in addedAddress)
            {
                if (string.IsNullOrEmpty(address.Address.StreetAddress1)) continue;

                result.Add($"Added Garaging Address '{address.Address.StreetAddress1}, {address.Address.City}, {address.Address.State} {address.Address.ZipCode}'");
            }

            foreach (var address in removedAddress)
            {
                if (string.IsNullOrEmpty(address.Address.StreetAddress1)) continue;

                result.Add($"Removed Garaging Address '{address.Address.StreetAddress1}, {address.Address.City}, {address.Address.State} {address.Address.ZipCode}'");
            }

            foreach (var oldAddress in oldList)
            {
                if (string.IsNullOrEmpty(oldAddress.Address.StreetAddress1)) continue;

                var newAddress = newList.FirstOrDefault(x => x.CreatedDate == oldAddress.CreatedDate && x.Address.UniqueId == oldAddress.Address.UniqueId);

                if (newAddress is null) continue;

                var compareResult = Compare(oldAddress, newAddress, nameof(EntityAddress), $"{FormatValues("Address.AddressTypeId", oldAddress.AddressTypeId)} ");

                result.AddRange(compareResult);
            }

            return result;
        }

        private List<string> CompareList(IQueryable<RiskForm> oldList, IQueryable<RiskForm> newList)
        {
            var result = new List<string>();
            var addedForms = newList.Where(newObj => oldList.All(oldObj => oldObj.FormId != newObj.FormId && newObj.IsSelected == true));
            var removedForms = oldList.Where(oldObj => newList.All(newObj => newObj.FormId != oldObj.FormId));

            var exemptedFormIdForChecking = new string[] { "AdditionalInsuredBlanket", "DesignatedInsured", "LossPayeeEndorsement", "PrimaryAndNoncontributory", "WaiverOfTransferOfRights", "GeneralLiabilityEndorsement" }; // these forms are already checked and validated on Additional Interest section

            foreach (var addedForm in addedForms.Where(x => !exemptedFormIdForChecking.Contains(x.FormId)).OrderBy(x => x.Form.DecPageOrder))
            {
                result.Add($"Added Form '{addedForm.Form.Name}'");
            }

            foreach (var removedForm in removedForms.Where(x => !exemptedFormIdForChecking.Contains(x.FormId)).OrderBy(x => x.Form.DecPageOrder))
            {
                result.Add($"Removed Form '{removedForm.Form.Name}'");
            }

            foreach (var oldForm in oldList.OrderBy(x => x.Form.DecPageOrder))
            {
                //if (oldForm.Form.IsMandatory == true && oldForm.FormId != "MotorTruckCargoEndorsement") continue;
                if (exemptedFormIdForChecking.Contains(oldForm.FormId) && oldForm.FormId != "LossPayeeEndorsement") continue;
                var newForm = newList.FirstOrDefault(x => x.FormId == oldForm.FormId);

                if ((oldForm.IsSelected ?? false) == false && (newForm.IsSelected ?? false) == false && newForm.FormId == "PhysicalDamageLimitOfInsurance") continue; // skip if not selected

                if (newForm != null)
                {
                    string tag = string.Empty;
                    if (oldForm.IsSelected == (newForm.IsSelected ?? false))
                    {
                        if (oldForm.Other == newForm.Other) continue;

                        var eachFormChanges = new List<string>();
                        switch (newForm.FormId)
                        {
                            case "ExclusionRadiusEndorsement":
                                var newExclusionRadiusEndorsement = JsonConvert.DeserializeObject<ExclusionRadiusDTO>(newForm.Other);
                                var oldExclusionRadiusEndorsement = JsonConvert.DeserializeObject<ExclusionRadiusDTO>(oldForm.Other);
                                eachFormChanges.AddRange(Compare(oldExclusionRadiusEndorsement, newExclusionRadiusEndorsement, nameof(ExclusionRadiusDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "HiredPhysicalDamage":
                                var newHiredPhysicalDmg = JsonConvert.DeserializeObject<HiredPhysdamDTO>(newForm.Other);
                                var oldHiredPhysicalDmg = JsonConvert.DeserializeObject<HiredPhysdamDTO>(oldForm.Other);
                                eachFormChanges.AddRange(Compare(oldHiredPhysicalDmg, newHiredPhysicalDmg, nameof(HiredPhysdamDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "DriverRequirements":
                                var newDriverRequirements = JsonConvert.DeserializeObject<DriverRequirementDTO>(newForm.Other);
                                var oldDriverRequirements = JsonConvert.DeserializeObject<DriverRequirementDTO>(oldForm.Other);
                                eachFormChanges.AddRange(Compare(oldDriverRequirements, newDriverRequirements, nameof(DriverRequirementDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "TerritoryExclusion":
                                var newTerritoryExclusion = JsonConvert.DeserializeObject<TerritoryExclusionDTO>(newForm.Other);
                                var oldTerritoryExclusion = JsonConvert.DeserializeObject<TerritoryExclusionDTO>(oldForm.Other);
                                eachFormChanges.AddRange(Compare(oldTerritoryExclusion, newTerritoryExclusion, nameof(TerritoryExclusionDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "TrailerInterchangeCoverage":
                                var newTrailerInterchangeCoverage = JsonConvert.DeserializeObject<TrailerInterchangeDTO>(newForm.Other);
                                var oldTrailerInterchangeCoverage = JsonConvert.DeserializeObject<TrailerInterchangeDTO>(oldForm.Other);
                                eachFormChanges.AddRange(Compare(oldTrailerInterchangeCoverage, newTrailerInterchangeCoverage, nameof(TrailerInterchangeDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "MCS90":
                                var newMCS90 = JsonConvert.DeserializeObject<Mcs90DTO>(newForm.Other ?? "");
                                var oldMCS90 = JsonConvert.DeserializeObject<Mcs90DTO>(oldForm.Other ?? "");
                                if (oldMCS90 == null)
                                    oldMCS90 = GetMCS90Default(oldForm.RiskDetailId.Value);
                                eachFormChanges.AddRange(Compare(oldMCS90, newMCS90, nameof(Mcs90DTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "LossPayeeEndorsement":
                                var newLoss = JsonConvert.DeserializeObject<FormLossPayeeDTO>(newForm.Other ?? "");
                                var oldLoss = JsonConvert.DeserializeObject<FormLossPayeeDTO>(oldForm.Other ?? "");
                                eachFormChanges.AddRange(Compare(oldLoss, newLoss, nameof(FormLossPayeeDTO), $"Form '{oldForm?.Form?.Name}' ", true));
                                break;
                            case "PennsylvaniaAddCombFirstPartyBenefitsEndorsement":
                                if (oldForm.IsSelected == false && (newForm.IsSelected ?? false) == false) break;
                                var newPennsylvaniaAddCombFirstPartyBenefitsEndorsement = JsonConvert.DeserializeObject<FormPAaddcombFPBDTO>(newForm.Other ?? "");
                                var oldPennsylvaniaAddCombFirstPartyBenefitsEndorsement = JsonConvert.DeserializeObject<FormPAaddcombFPBDTO>(oldForm.Other ?? "");
                                eachFormChanges.AddRange(Compare(oldPennsylvaniaAddCombFirstPartyBenefitsEndorsement, newPennsylvaniaAddCombFirstPartyBenefitsEndorsement, nameof(FormPAaddcombFPBDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            case "CoveredAutoDesignationSymbol":
                                if (oldForm.IsSelected == false && (newForm.IsSelected ?? false) == false) break;
                                var newCoveredAutoDeisgnationSymbol = JsonConvert.DeserializeObject<CoveredAutoDesigSymbolDTO>(newForm.Other ?? "");
                                var oldCoveredAutoDeisgnationSymbol = JsonConvert.DeserializeObject<CoveredAutoDesigSymbolDTO>(oldForm.Other ?? "");
                                var oldSymbolTenResponses = _formsService.GetSymbolTenResponses((Guid)oldForm.RiskDetailId).Result;
                                var newSymbolTenResponses = _formsService.GetSymbolTenResponses((Guid)newForm.RiskDetailId).Result;

                                if (!string.IsNullOrEmpty(oldCoveredAutoDeisgnationSymbol?.CoveredAutoText))
                                    oldCoveredAutoDeisgnationSymbol.CoveredAutoText = oldSymbolTenResponses.FirstOrDefault(r => r.Id == int.Parse(oldCoveredAutoDeisgnationSymbol.CoveredAutoText))?.Description;

                                if(!string.IsNullOrEmpty(newCoveredAutoDeisgnationSymbol?.CoveredAutoText))
                                    newCoveredAutoDeisgnationSymbol.CoveredAutoText = newSymbolTenResponses.FirstOrDefault(r => r.Id == int.Parse(newCoveredAutoDeisgnationSymbol.CoveredAutoText))?.Description;

                                eachFormChanges.AddRange(Compare(oldCoveredAutoDeisgnationSymbol, newCoveredAutoDeisgnationSymbol, nameof(CoveredAutoDesigSymbolDTO), $"Form '{oldForm?.Form?.Name}' "));
                                break;
                            default:
                                break;
                        }

                        result.AddRange(eachFormChanges);
                        continue;
                    }
                    if ((oldForm.IsSelected ?? false) && !(newForm.IsSelected ?? false)) tag = "Removed";
                    if (!(oldForm.IsSelected ?? false) && (newForm.IsSelected ?? false)) tag = "Added";

                    if (newForm.FormId == "LossPayeeEndorsement" && tag == "Removed")
                    {
                        tag = null;
                        var newLoss = JsonConvert.DeserializeObject<FormLossPayeeDTO>(newForm.Other ?? "NONE");
                        result.Add($"Changed Form '{newForm?.Form?.Name}' Designation / Description of Autos To: blank");
                    }
                    if (newForm.FormId == "LossPayeeEndorsement" && tag == "Added")
                    {
                        tag = null;
                        var newLoss = JsonConvert.DeserializeObject<FormLossPayeeDTO>(newForm.Other ?? "");
                        if (!string.IsNullOrEmpty(newLoss?.DescriptionOfAutos))
                            result.Add($"Changed Form '{newForm?.Form?.Name}' Designation / Description of Autos To: '{newLoss.DescriptionOfAutos}'");
                        else
                            tag = "Added";
                    }

                    if (!string.IsNullOrWhiteSpace(tag))
                        result.Add($"{tag} Form '{oldForm?.Form?.Name}'");
                }
            }

            return result;
        }

        private Mcs90DTO GetMCS90Default(Guid riskDetailId)
        {
            var riskDetail = _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId).Result;
            var risk = _riskRepository.GetRiskByIdAsync(riskDetail.RiskId).Result;
            var brokerInfo = _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId).Result;
            var businessAddress = _entityAddressRepository.FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business && x.EntityId == riskDetail.Insured.EntityId,
                i => i.Address).Result;

            return new Mcs90DTO()
            {
                UsDotNumber = riskDetail.Insured.Entity.USDOTNumber,
                DateReceived = brokerInfo.EffectiveDate.Value,
                IssuedTo = riskDetail.Insured?.Entity?.CompanyName,
                State = businessAddress.Address.State,
                DatedAt = "Omaha, NE",
                PolicyDateDay = brokerInfo.EffectiveDate.Value.Day,
                PolicyDateMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(brokerInfo.EffectiveDate.Value.Month),
                PolicyDateYear = brokerInfo.EffectiveDate.Value.Year.ToString(),
                PolicyNumber = riskDetail.SubmissionNumber,
                EffectiveDate = brokerInfo.EffectiveDate.Value,
                InsuranceCompany = "Texas Insurance Company",
                CounterSignedBy = "Jeffrey A Silver",
                AccidentLimit = 750000
            };

        }

        private string FormatPropertyName(string propertyName)
        {
            return FieldLabels.Labels.ContainsKey(propertyName) ? FieldLabels.Labels.GetValueOrDefault(propertyName) : propertyName;
        }

        private string GetHistoryYear(int historyYear)
        {
            string label = string.Empty;
            switch (historyYear)
            {
                case 0:
                    label = "Current/Expiring";
                    break;
                case 1:
                    label = "1st Prior";
                    break;
                case 2:
                    label = "2nd Prior";
                    break;
                case 3:
                    label = "3rd Prior";
                    break;
                case 4:
                    label = "4th Prior";
                    break;
                default:
                    break;
            }

            return label;
        }

        private string FormatValues(string propertyName, object value,int? raterVersion = null)
        {
            string formattedValues = $"{value}";
            bool isRaterVersionSupported = (raterVersion ?? 7) >= 7 || raterVersion == 0;
            var grossVehicleWeight = new Dictionary<string, string>()
            {
                { "1", "1 - 10,000" },
                { "2", "10,001 - 20,000" },
                { "3", "20,001 - 45,000" },
                { "4", "45,001 +" },
            };

            var grossVehicleWeightCategory = new Dictionary<string, string>()
            {
                { "1", "Light" },
                { "2", "Medium" },
                { "3", "Heavy" },
                { "4", "Extra Heavy (Including tractors)" },
                { "5", "Light Trucks" },
                { "6", "Medium Trucks" },
                { "7", "Heavy Trucks" },
                { "8", "Extra Heavy Truck" },
            };

            var vehicleDescription = new Dictionary<string, string>()
            {
                { "38", "Delivery Van>10k GVW" },
                { "16", "Dry Freight Trailer" },
                { "21", "Dump Body Trailer" },
                { "2", "Dump Truck 0-16k GVW" },
                { "66", "Dump Truck 33-45k GVW" },
                { "65", "Dump Truck >45k GVW" },
                { "8", "Full size Van" },
                { "60", "Garbage Truck 0-45 GVW" },
                { "68", "Garbage Truck >45 GVW" },
                { "5", "Pickup<= 1/2 Ton 4x2" },
                { "6", "Pickup<= 1/2 Ton 4x4" },
                { "7", "Pickup >  1/2 Ton 4x4" },
                { "34", "Private Passenger Auto" },
                { "22", "Refrigerated Dry Freight Trailer" },
                { "44", "Refrigerated Truck 0-16k GVW" },
                { "45", "Refrigerated Truck 16-26k GVW" },
                { "46", "Refrigerated Truck >26k GVW" },
                { "37", "Step Van 0-10k GVW" },
                { "41", "Straight Truck 0-16k GVW" },
                { "42", "Straight Truck 16-26k GVW" },
                { "43", "Straight Truck >26K GVW" },
                { "17", "Tank Trailer" },
                { "1", "Tractor" },
                { "141", "Agricultural Hopper Truck" },
                { "102", "Auto Hauling Trailer" },
                { "131", "Bucket Truck/Cherry Picker >26K GVW" },
                { "129", "Bucket Truck/Cherry Picker 0-16k GVW" },
                { "130", "Bucket Truck/Cherry Picker 16-26k GVW" },
                { "98", "Bulk Commodity Trailer" },
                { "138", "Bus 16-60 passengers" },
                { "139", "Bus 61+ passengers" },
                { "137", "Bus 9-15 passengers" },
                { "140", "Car Carrier >16k GVW" },
                { "133", "Car Carrier 0-16 GVW" },
                { "117", "Catering Truck" },
                { "113", "Cement Mixer" },
                { "111", "Concession Trailer" },
                { "120", "Dump Truck 16-33k GVW" },
                { "92", "Flatbed Trailer" },
                { "114", "Flatbed Truck >26k GVW" },
                { "83", "Flatbed Truck 0-16 GVW" },
                { "121", "Flatbed Truck 16-26k GVW" },
                { "127", "Front Loader >45k GVW" },
                { "122", "Front Loader 0-45k" },
                { "132", "Gooseneck Trailer" },
                { "109", "Hearse" },
                { "95", "Horse Trailer(1-2 stalls)" },
                { "110", "Ice Cream Truck" },
                { "108", "Limousine" },
                { "94", "Livestock Trailer" },
                { "126", "Logging Trailer" },
                { "96", "Lowboy Trailer" },
                { "105", "Luxury Auto" },
                { "91", "Mini Van" },
                { "106", "Motor Home" },
                { "116", "Pickup >  1/2 Ton 4x2" },
                { "119", "Pickup w/ 5th Wheel 0-16k GVW" },
                { "93", "Pole Trailer" },
                { "136", "Pump Truck(Cement) >26k GVW" },
                { "134", "Pump Truck(Cement) 0-16k GVW" },
                { "135", "Pump Truck(Cement) 16-26k GVW" },
                { "100", "Rag Top Trailer" },
                { "128", "Roll On Vehicle >45k GVW" },
                { "123", "Roll On Vehicle 0-45 GVW" },
                { "103", "Sport Auto" },
                { "118", "Stake Truck >26k GVW" },
                { "89", "Stake Truck 0-16k GVW" },
                { "124", "Stake Truck 16-26k GVW" },
                { "90", "Tank Truck <= 1400 gals" },
                { "125", "Tank Truck > 1400 gals" },
                { "99", "Tilt Trailer" },
                { "84", "Tow Truck - one axle" },
                { "112", "Travel Trailer" },
                { "115", "TW Truck-two axle" },
                { "97", "Unidentified Trailer" },
                { "101", "Utility Trailer <= 1 ft." },
                { "104", "Utility Trailer > 12 ft" },
                { "107", "Utility Vehicle" },
                { "86", "Wheelchair Bus" },
                { "85", "Wheelchair Van" },
                // newly added dataset for rater V7
                { "142", "Boom Truck/Crane Truck >26k GVW" },
                { "143", "Boom Truck/Crane Truck 0-16k GVW" },
                { "144", "Boom Truck/Crane Truck 16-26k GVW" },
                { "145", "Full size Van (0-8 passengers)" },
                { "146", "Limousine <120 inches in Length" },
                { "147", "Limousine 121-180 inches in Length" },
                { "148", "Limousine >180 inches in length" },
                { "149", "Luxury Utility Vehicle" },
                { "150", "Tow Truck - one axle 0-26k GVW" },
                { "151", "Tow Truck - one axle >26k GVW" },
                { "152", "Utility Vehicle (except Luxury)" },
                { "153", "Wedge Trailer" },
                { "154", "Wheelchair Bus (16-60 passengers)" },
                { "155", "Wheelchair Bus (61+ passengers)" },
                { "156", "Wheelchair Van (0-8 passengers)" },
                { "157", "Wheelchair Van (9-15 passengers)" },

            };

            var businessClass = new Dictionary<string, string>()
            {
                { "10", "Agricultural Hauling" },
                { "11", "Airport Taxi Services" },
                { "12", "Amish Taxis" },
                { "13", "Auto Dealers" },
                { "14", "Auto Haulers" },
                { "15", "Auto Repair & Maintenance" },
                { "16", "Auto Salvage Haulers" },
                { "17", "Black Car Services" },
                { "18", "Body Shops" },
                { "19", "Building Materials & Hardware Stores" },
                { "20", "Carpentry" },
                { "21", "Catering Business" },
                { "22", "Coal Hauling" },
                { "23", "Concrete & Asphalt Work" },
                { "24", "Consulting, Legal & Engineering" },
                { "1", "Courier" },
                { "25", "Day Care (Child & Adult Services)" },
                { "3", "Debris Removal" },
                { "4", "Dirt, Sand & Gravel" },
                { "26", "Disinfecting & Pest Control Services" },
                { "27", "Door to Door Sales" },
                { "28", "Driving Schools" },
                { "29", "Dry Cleaning, Laundry & Carpet Cleaners" },
                { "30", "Drywall, Plastering & Insulation Work" },
                { "31", "Electrical Contractor" },
                { "32", "Entertainers & Musicians" },
                { "33", "Equipment Rental & Leasing" },
                { "34", "Escort Vehicles" },
                { "35", "Excavation Work" },
                { "36", "Expediters and Hotshotters" },
                { "37", "Farming & Livestock" },
                { "38", "Flooring" },
                { "39", "Florist" },
                { "40", "Food & Grocery Stores" },
                { "41", "Freight Forwarders" },
                { "42", "Funeral Services" },
                { "5", "Furniture & Home Furnishing Stores" },
                { "6", "Garbage & Trash" },
                { "43", "Gas & Service Stations" },
                { "44", "Glass & Glazing Work" },
                { "45", "Hazardous Materials" },
                { "46", "Heating & Air Conditioning" },
                { "47", "Heavy Construction - Road & Utilities" },
                { "48", "Hotel Shuttle Services" },
                { "7", "Household Movers" },
                { "49", "Ice Cream Vendors" },
                { "50", "Janitorial & Building Maintenance Services" },
                { "51", "Landscaping, snowplowing & Firewood" },
                { "52", "Limousine Services" },
                { "53", "Livestock Hauling" },
                { "54", "Logging & Log Transport" },
                { "8", "Machinery & Heavy Equipment" },
                { "55", "Manufacturing" },
                { "56", "Masonry, Plastering/Drywall, Carpenters, Floorwork (Renewals Only)" },
                { "57", "Masons" },
                { "58", "Meals on Wheels" },
                { "59", "Mobile Home Toters" },
                { "60", "Newspaper & Mail Delivery" },
                { "61", "Non Business" },
                { "62", "Non-Emergency Medical Transportation" },
                { "63", "Non-Residential & Industrial Builders" },
                { "64", "Other Trade Contractors" },
                { "2", "Other For-Hire Trucking Operations" },
                { "65", "Other Passenger Transportation (Not for Hire)" },
                { "66", "Other Retail Trade Operations" },
                { "67", "Out of State Contractors" },
                { "68", "Painting & Wallpaper Hanging" },
                { "69", "Personal Care - Beauty & Barber Shops" },
                { "70", "Plumbing" },
                { "71", "Prepared Food & Lunch Truck" },
                { "72", "Real Estate & Insurance" },
                { "73", "Refrigerated Goods" },
                { "74", "Religious Organizations" },
                { "75", "Repair Services" },
                { "76", "Residential Builders" },
                { "77", "Restaurant, Pizza & Fast Food Delivery" },
                { "78", "Retail Operations (Renewals Only)" },
                { "79", "Roofing, Siding & Sheet Metal Work" },
                { "9", "Scrap Metal, Scrap Auto & Recycling Services" },
                { "80", "Security Guard & Private Investigation" },
                { "81", "Septic Waste Removal" },
                { "82", "Services Not Otherwise Classified" },
                { "83", "Social & Health Services" },
                { "84", "Steel Hauling" },
                { "85", "Taxi Services" },
                { "86", "Towing & Service Stations (Renewals Only)" },
                { "87", "Towing Services" },
                { "88", "Visiting Nurse" },
                { "89", "Water Well Drilling Contractors" },
                { "90", "Wholesale Manufacturing (Renewals Only)" },
                { "91", "Wholesale Trade" },
            };

            var limitsSymbols = new Dictionary<string, string>()
            {
                { "1", "7, 8 & 9" },
                { "2", "7 & 9" },
                { "3", "2, 8 & 9" },
                { "4", "1) Any Auto" },
                { "5", "2) All Owned Autos" },
                { "6", "3) Owned Private Passenger Autos" },
                { "7", "4) Owned Autos Other Than Private Passenger" },
                { "8", "5) All Owned Autos Which Require No-Fault Coverage" },
                { "9", "6) Owned Autos Subject to Compulsory UM Law" },
                { "10", "7) Autos Specified on  Schedule" },
                { "11", "8) Hired Autos" },
                { "12", "9) Non-Owned Autos" },
                { "13", "7 & 8" },
                { "14", "10 - Use 2, 8 & 9 Pricing" },
                { "15", "10 - Use 7, 8 & 9 Pricing" }
            };

            var additionalInterestType = new Dictionary<string, string>()
            {
                { "1", "Additional Insured" },
                { "2", "Loss Payee" },
                { "3", "Breach of Warranty" },
                { "4", "Mortgagee" },
                { "5", "Co-Owner" },
                { "6", "Owner" },
                { "7", "Employee As Lessor" },
                { "8", "Registrant" },
                { "9", "Leaseback Owner" },
                { "10", "Trustee" },
                { "11", "Lienholder" },
                { "12", "Designated Insured" }
            };

            var useClass = new Dictionary<string, string>()
            {
                { "3", "Service" },
                { "4", "Heavy Loads/Special" },
                { "5", "Logging" },
                { "6", "Non-Business" },
                { "7", "Pizza/Prepared Food" },
                { "1", "Courier" },
                { "8", "Funeral" },
                { "9", "Child Care" },
                { "10", "Church" },
                { "11", "Social Service Passenger Transport" },
                { "12", "Livery" },
                { "2", "Commercial" },
                { "13", "Repossession" },
                { "14", "Coal Hauling" },
                { "15", "Towing 24/7" },

            };

            var incidentTypes = new Dictionary<string, string>()
            {
                { "1", "Majors" },
                { "2", "DUI/DWI/Drug" },
                { "3", "Minors (No Speed)" },
                { "4", "At-Fault" },
                { "5", "Major License Issues (Foreign, Unverified)" },
                { "6", "Not At-Fault" },
                { "7", "Equipment" },
                { "8", "Speed" },
                { "9", "Other" },
            };

            var addressTypeList = new Dictionary<string, string>()
            {
                { "1", "Billing Address" },
                { "2", "Business Address" },
                { "3", "Garaging Address" },
                { "4", "Mailing Address" },
            };

            if (propertyName == "RiskAdditionalInterest.AdditionalInterestTypeId" && value != null)
            {
                formattedValues = additionalInterestType.GetValueOrDefault($"{value}");
            }

            if ((propertyName == "TerritoryExclusionDTO.States" ||
                propertyName == "Vehicle.State" ||
                propertyName == "Vehicle.RegisteredState" ||
                propertyName == "Address.State" ||
                propertyName == "Driver.State") && value != null)
            {
                var states = GetStatesList().Result;
                var evaluatedStates = value.ToString()
                                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(stateCode => (string)states.FirstOrDefault(state => state.StateCode == stateCode)?.FullStateName?.Replace("[E]", ""))
                                            .OrderBy(x => x).ToArray();
                formattedValues = string.Join(", ", evaluatedStates);
            }

            if (propertyName == "Address.AddressTypeId")
            {
                formattedValues = addressTypeList.GetValueOrDefault($"{value}");
            }

            if (propertyName == "RiskSpecificCommoditiesHauled.PercentageOfCarry")
            {
                formattedValues = $"{value:N1}%";
            }

            if (FieldLabels.Labels.ContainsKey(propertyName) && propertyName.StartsWith(nameof(RiskCoverage)))
            {
                switch (propertyName)
                {
                    case "RiskCoverage.AutoBILimitId":
                        formattedValues = _limitList.AutoBILimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.UMBILimitId":
                        formattedValues = _limitList.UMBILimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.UMPDLimitId":
                        formattedValues = _limitList.UMPDLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.UIMBILimitId":
                        formattedValues = _limitList.UIMBILimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.UIMPDLimitId":
                        formattedValues = _limitList.UIMPDLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.MedPayLimitId":
                        formattedValues = _limitList.MedPayLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.PIPLimitId":
                        formattedValues = _limitList.PipLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.LiabilityDeductibleId":
                        formattedValues = _limitList.LiabilityDeductibleLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.CompDeductibleId":
                        formattedValues = _limitList.ComprehensiveLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.CollDeductibleId":
                        formattedValues = _limitList.CollisionLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.FireDeductibleId":
                        formattedValues = _limitList.ComprehensiveLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.GLBILimitId":
                        formattedValues = _limitList.GlLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.CargoLimitId":
                        formattedValues = _limitList.CargoLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.RefCargoLimitId":
                        formattedValues = _limitList.RefLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.TrailerInterchangeLimitId":
                        formattedValues = _limitList.TrailerInterchangeLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.NonOwnedTrailerLimitId":
                        formattedValues = _limitList.NonOwnedTrailerLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.TrailerTypeLimitId":
                        formattedValues = _limitList.TrailerTypeLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.PollutionCovLimitId":
                        formattedValues = _limitList.PollutionCovLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "RiskCoverage.ALSymbolId":
                    case "RiskCoverage.UMBILimitSymbolId":
                    case "RiskCoverage.UMPDLimitSymbolId":
                    case "RiskCoverage.UIMBILimitSymbolId":
                    case "RiskCoverage.UIMPDLimitSymbolId":
                    case "RiskCoverage.MedPayLimitSymbolId":
                    case "RiskCoverage.PIPLimitSymbolId":
                    case "RiskCoverage.CompDeductibleSymbolId":
                    case "RiskCoverage.CollDeductibleSymbolId":
                    case "RiskCoverage.FireDeductibleSymbolId":
                    case "RiskCoverage.CargoLimitSymbolId":
                    case "RiskCoverage.RefCargoLimitSymbolId":
                    case "RiskCoverage.TrailerInterchangeLimitSymbolId":
                    case "RiskCoverage.NonOwnedTrailerLimitSymbolId":
                        formattedValues = limitsSymbols.GetValueOrDefault($"{value}");
                        break;
                    default:
                        formattedValues = $"{value}";
                        break;
                }
            }

            if (FieldLabels.Labels.ContainsKey(propertyName) && propertyName.StartsWith(nameof(Vehicle)))
            {
                switch (propertyName)
                {
                    case "Vehicle.GrossVehicleWeightId":
                        formattedValues = grossVehicleWeight.GetValueOrDefault($"{value}");
                        break;
                    case "Vehicle.VehicleBusinessClassId":
                        formattedValues = businessClass.GetValueOrDefault($"{value}");
                        break;
                    case "Vehicle.UseClassId":
                        formattedValues = useClass.GetValueOrDefault($"{value}");
                        break;
                    case "Vehicle.VehicleDescriptionId":
                        formattedValues = vehicleDescription.GetValueOrDefault($"{value}");
                        break;
                    case "Vehicle.CoverageCollisionDeductibleId":
                        formattedValues = _limitList.CollisionLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "Vehicle.CoverageFireTheftDeductibleId":
                        formattedValues = _limitList.ComprehensiveLimits.FirstOrDefault(x => x.Id == int.Parse($"{value}"))?.LimitDisplay;
                        break;
                    case "Vehicle.StatedAmount":
                    case "Vehicle.AutoLiabilityPremium":
                    case "Vehicle.CargoPremium":
                    case "Vehicle.PhysicalDamagePremium":
                    case "Vehicle.TotalPremium":
                    case "Vehicle.CollisionPremium":
                    case "Vehicle.ComprehensivePremium":
                    case "Vehicle.RefrigerationPremium":
                        formattedValues = $"${value:N0}";
                        break;
                    case "Vehicle.IsCoverageFireTheft":
                    case "Vehicle.IsCoverageCollision":
                    case "Vehicle.HasCoverageCargo":
                    case "Vehicle.HasCoverageRefrigeration":
                    case "Vehicle.Radius":
                        formattedValues = $"{value:N0}";
                        break;
                    case "Vehicle.GrossVehicleWeightCategory":
                        if (isRaterVersionSupported) {
                            switch (int.Parse($"{value}")) {
                                case 1:
                                    value = 5;
                                    break;
                                case 2:
                                    value = 6;
                                    break;
                                case 3:
                                    value = 7;
                                    break;
                                case 4:
                                    value = 8;
                                    break;
                            }
                        } 
                        formattedValues = grossVehicleWeightCategory.GetValueOrDefault($"{value}");

                        break;
                    case "Vehicle.State":
                    case "Vehicle.RegisteredState":
                        break;
                    case "Vehicle.GaragingAddressId":
                        var entityAddress = _entityAddressRepository.GetEntityAddressByAddressId(Guid.Parse(value.ToString())).Result;
                        if (entityAddress != null)
                        {
                            formattedValues = $"{entityAddress.Address.StreetAddress1}, {entityAddress.Address.City}, {entityAddress.Address.State}, {entityAddress.Address.ZipCode}";
                        }
                        break;
                    default:
                        formattedValues = $"{value}";
                        break;
                }
            }

            if (FieldLabels.Labels.ContainsKey(propertyName) && propertyName.StartsWith(nameof(RiskCoveragePremium)))
            {
                switch (propertyName)
                {
                    case "RiskCoveragePremium.AutoLiabilityPremium":
                    case "RiskCoveragePremium.CargoPremium":
                    case "RiskCoveragePremium.PhysicalDamagePremium":
                    case "RiskCoveragePremium.GeneralLiabilityPremium":
                    case "RiskCoveragePremium.Premium":
                    case "RiskCoveragePremium.RiskMgrFeeAL":
                    case "RiskCoveragePremium.RiskMgrFeePD":
                    case "RiskCoveragePremium.AdditionalInsuredPremium":
                    case "RiskCoveragePremium.PrimaryNonContributoryPremium":
                    case "RiskCoveragePremium.WaiverOfSubrogationPremium":
                    case "RiskCoveragePremium.CollisionPremium":
                    case "RiskCoveragePremium.ComprehensivePremium":
                    case "RiskCoveragePremium.HiredPhysicalDamage":
                    case "RiskCoveragePremium.TrailerInterchange":
                    case "RiskCoveragePremium.ALManuscriptPremium":
                    case "RiskCoveragePremium.PDManuscriptPremium":
                        formattedValues = string.Format("${0:N2}", value);
                        break;
                    default:
                        formattedValues = $"{value}";
                        break;
                }
            }

            if (FieldLabels.Labels.ContainsKey(propertyName) && (propertyName.StartsWith(nameof(RiskHistory)) || propertyName.StartsWith(nameof(ClaimsHistory))))
            {
                switch (propertyName)
                {
                    case "RiskHistory.LiabilityLimits":
                    case "RiskHistory.LiabilityDeductible":
                    case "RiskHistory.CollisionDeductible":
                    case "RiskHistory.ComprehensiveDeductible":
                    case "RiskHistory.CargoLimits":
                    case "RiskHistory.RefrigeratedCargoLimits":
                    case "RiskHistory.AutoLiabilityPremiumPerVehicle":
                    case "RiskHistory.PhysicalDamagePremiumPerVehicle":
                    case "RiskHistory.TotalPremiumPerVehicle":
                    case "RiskHistory.NumberOfPowerUnitsAL":
                    case "RiskHistory.NumberOfPowerUnitsAPD":
                    case "ClaimsHistory.BiPiPmpIncLossTotal":
                    case "ClaimsHistory.BiPiPmpIncLoss100KCap":
                    case "ClaimsHistory.PropertyDamageIncLoss":
                    case "ClaimsHistory.NetLoss":
                    case "ClaimsHistory.CargoNetLoss":
                    case "ClaimsHistory.GeneralLiabilityNetLoss":
                    case "ClaimsHistory.PhysicalDamageNetLoss":
                        formattedValues = string.Format("${0:N0}", value);
                        break;
                    default:
                        formattedValues = $"{value}";
                        break;
                }
            }

            if (FieldLabels.Labels.ContainsKey(propertyName) && propertyName.StartsWith(nameof(DriverIncidents)))
            {
                switch (propertyName)
                {
                    case "DriverIncidents.IncidentType":
                        formattedValues = incidentTypes.GetValueOrDefault($"{value}");
                        break;
                    case "DriverIncidents.IncidentDate":
                    case "DriverIncidents.ConvictionDate":
                        var date = (DateTime?)value;
                        formattedValues = (date.HasValue) ? $"{date:MM/dd/yyyy}" : "";
                        break;
                }
            }

            return formattedValues;
        }

        private bool IsFormSelected(Guid riskDetailId, string formId)
        {
            var riskForm = _riskFormRepository.FindAsync(x => x.FormId == formId && x.RiskDetailId == riskDetailId).Result;
            return riskForm.IsSelected ?? false;
        }

        private RiskCoveragePremium ValidateEndorsementPremiums(RiskCoverage riskCoverage)
        {
            var riskDetailId = riskCoverage.RiskDetailId;
            var riskCoveragePremium = riskCoverage.RiskCoveragePremium;

            // excluding Manuscript; checked separately
            riskCoveragePremium.HiredPhysicalDamage = (riskCoveragePremium.HiredPhysicalDamage != null && !IsFormSelected(riskDetailId, "HiredPhysicalDamage")) ? 0 : riskCoveragePremium.HiredPhysicalDamage;
            riskCoveragePremium.TrailerInterchange = (riskCoveragePremium.TrailerInterchange != null && !IsFormSelected(riskDetailId, "TrailerInterchangeCoverage")) ? 0 : riskCoveragePremium.TrailerInterchange;

            return riskCoveragePremium;
        }

        private async Task<List<StateInfo>> GetStatesList()
        {
            var client = new HttpClient();
            if (!String.IsNullOrEmpty(Data.Common.Services.GetAuthToken()))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.Common.Services.GetAuthToken());
            var response = await client.GetAsync($"{_genericBaseUrl}/State");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<StateInfo>>(await response.Content.ReadAsStringAsync());
        }

        #endregion Compare Policy Details

        //For Additional Interests related forms effective dating
        public async Task<List<AdditionalInterestChange>> GetAdditionalInterestChanges(Guid currentRiskDetailId, Guid riskId)
        {
            var risk = await _riskRepository.GetByIdNoTrackingAsync(riskId);
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == riskId && riskDetail.Id != currentRiskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();

            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(currentRiskDetailId);
            await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId);

            await _bindingService.GetBindingByRiskIdAsync(riskId);
            var businessAddress = _entityAddressRepository.FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business && x.EntityId == currentRiskDetail.Insured.EntityId, i => i.Address).Result;
            _limitList = _limitsService.GetPerStateAsync(businessAddress.Address?.State, risk.RaterVersionId).Result;

            var changes = new List<AdditionalInterestChange>();

            var previousAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId, i => i.Entity);
            var currentAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == currentRiskDetailId, i => i.Entity);
            changes.AddRange(CompareAdditionalInterestList(previousAdditionalInterest.AsQueryable(), currentAdditionalInterest.AsQueryable()));

            return changes;
        }

        private IEnumerable<AdditionalInterestChange> CompareAdditionalInterestList(IQueryable<RiskAdditionalInterest> oldList, IQueryable<RiskAdditionalInterest> newList)
        {
            var result = new List<AdditionalInterestChange>();
            var addedInterests = newList.Where(newObj => oldList.All(oldObj => oldObj.AddProcessDate != newObj.AddProcessDate));

            foreach (var interest in addedInterests)
            {
                var interestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == interest.EntityId, e => e.Address).Result;
                //var interestAddress = $"{interestEntityAddress.Address.StreetAddress1}, {interestEntityAddress.Address.City}, {interestEntityAddress.Address.State}, {interestEntityAddress.Address.ZipCode}";
                //result.Add($"Added Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", interest.AdditionalInterestTypeId)}': '{interest.Entity.FullName}', '{interestAddress}'");
                var change = new AdditionalInterestChange { Id = interest.Id, ChangedDate = DateTime.Now };
                result.Add(change);
            }

            foreach (var oldInterest in oldList)
            {
                var interest = newList.FirstOrDefault(newObj => newObj.AddProcessDate.Ticks == oldInterest.AddProcessDate.Ticks);
                if (interest is null) continue;
                //var interestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == interest.EntityId, e => e.Address).Result;
                //string interestAddress = $"{interestEntityAddress.Address.StreetAddress1}, {interestEntityAddress.Address.City}, {interestEntityAddress.Address.State}, {interestEntityAddress.Address.ZipCode}";
                if (interest.RemoveProcessDate != null)
                {
                    //result.Add($"Removed Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", interest.AdditionalInterestTypeId)}': '{interest.Entity.FullName}', '{interestAddress}'");
                    var change = new AdditionalInterestChange { Id = interest.Id, ChangedDate = DateTime.Now };
                    result.Add(change);
                }
                else
                {
                    var oldInterestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == oldInterest.EntityId, e => e.Address).Result;
                    var currentInterestEntityAddress = _entityAddressRepository.FindAsync(x => x.EntityId == interest.EntityId, e => e.Address).Result;
                    //string oldInterestAddressStr = $"{oldInterestEntityAddress.Address.StreetAddress1}, {oldInterestEntityAddress.Address.City}, {oldInterestEntityAddress.Address.State}, {oldInterestEntityAddress.Address.ZipCode}";
                    //result.AddRange(Compare(oldInterest.Entity, interest.Entity, nameof(Entity), $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': "));
                    //result.AddRange(Compare(oldInterestEntityAddress.Address, currentInterestEntityAddress.Address, nameof(Address), $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': "));
                    //result.AddRange(Compare(oldInterest, interest, nameof(RiskAdditionalInterest), $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': "));

                    var change1 = Compare(oldInterest.Entity, interest.Entity, nameof(Entity),
                        $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': ");
                    var change2 = Compare(oldInterestEntityAddress.Address, currentInterestEntityAddress.Address,
                        nameof(Address),
                        $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': ");

                    var change3 = Compare(oldInterest, interest, nameof(RiskAdditionalInterest),
                        $"Additional Interest '{FormatValues("RiskAdditionalInterest.AdditionalInterestTypeId", oldInterest.AdditionalInterestTypeId)}' '{oldInterest.Entity.FullName}': ");

                    if (!change1.Any() && !change2.Any() && !change3.Any()) continue;
                    var change = new AdditionalInterestChange { Id = interest.Id, ChangedDate = DateTime.Now };
                    result.Add(change);
                }
            }

            return result;
        }
    }
}
