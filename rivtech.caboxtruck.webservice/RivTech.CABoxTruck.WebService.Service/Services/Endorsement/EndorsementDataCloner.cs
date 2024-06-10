using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Guid;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public class EndorsementDataCloner : IEndorsementDataCloner
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
        private readonly IRiskVehiclePremiumFactory _riskVehiclePremiumFactory;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskPolicyPremiumFactory _riskPolicyPremiumFactory;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
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
        private readonly IRiskService _riskService;
        private readonly string[] ValidSurplusLinesState = { "AL", "AZ", "CA", "CO", "DE", "GA", "IL", "KY", "MD", "NC", "NJ", "NV", "OH", "OR", "PA", "SC", "TN", "TX", "VA", "WA" };
        private List<string> _paths = new List<string>();
        private LimitListDTO _limitList = null;
        private const string STATESERVICEURL = "https://app-genericservices-test.azurewebsites.net/api/State";
        private static string currentCargoCoverageValue = string.Empty; // to temporarily hold value from risk coverage for later use on vehicle level
        private static string currentRefrigerationCoverageValue = string.Empty; // to temporarily hold value from risk coverage for later use on vehicle level

        public EndorsementDataCloner(
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
            IRiskService riskService,
            IMapper mapper,
            IRiskVehiclePremiumFactory riskVehiclePremiumFactory,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskPolicyPremiumFactory riskPolicyPremiumFactory,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository
        ) {
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
            _riskService = riskService;
            _mapper = mapper;
            _riskVehiclePremiumFactory = riskVehiclePremiumFactory;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskPolicyPremiumFactory = riskPolicyPremiumFactory;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
        }

        public async Task<IEndorsementData> CloneAsync(IEndorsementData endorsementData)
        {
            _context.ChangeTracker.Clear();

            var newRiskDetailId = NewGuid();

            Guid riskDetailId = endorsementData.RiskDetailId;

            var toCloneRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            _riskDetailRepository.Detach(toCloneRiskDetail);

            var bindData = await _bindingService.GetBindingByRiskIdAsync(toCloneRiskDetail.RiskId);
            int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

            var fileUploadDocuments = await _fileUploadDocumentRepository.GetAsync(x => x.RiskDetailId == riskDetailId);

            #region RiskDetail
            var newRiskDetail = new RiskDetail();
            newRiskDetail = toCloneRiskDetail.Clone();
            newRiskDetail.Id = newRiskDetailId;
            newRiskDetail.Status = RiskDetailStatus.Active;
            newRiskDetail.CreatedDate = DateTime.Now;
            newRiskDetail.SourceRiskDetailId = endorsementData.RiskDetailId;
            newRiskDetail.IsProcessed = false;
            //newRiskDetail.FirstIssueDate = DateTime.Now;
            #endregion

            // Previous RiskDetail
            var previousRiskDetails = await _riskDetailRepository.GetAsync(riskDetail => riskDetail.RiskId == toCloneRiskDetail.RiskId && riskDetail.Id != riskDetailId && riskDetail.Status != RiskDetailStatus.Canceled);
            var previousRiskDetailId = previousRiskDetails.OrderByDescending(riskDetail => riskDetail.UpdatedDate).Select(riskDetail => riskDetail.Id).First();
            var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(toCloneRiskDetail.SourceRiskDetailId.HasValue ? toCloneRiskDetail.SourceRiskDetailId.Value : previousRiskDetailId);
            var previousManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);
            var previousAdditionalInterest = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == previousRiskDetail.Id);

            #region Policy History
            // update only existing histories to new riskdetail
            var policyHistories = await _policyHistoryRepository.GetAsync(x => x.RiskId == toCloneRiskDetail.RiskId.ToString());
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
                    var isNewlyAdded = previousAdditionalInterest.All(oldObj => oldObj.AddProcessDate != item.AddProcessDate);
                    var newAdditionalInterest = SystemExtension.Clone<RiskAdditionalInterest>(item);
                    newAdditionalInterest.Id = Guid.NewGuid();
                    newAdditionalInterest.EffectiveDate = (item.RemoveProcessDate == null) && isNewlyAdded ? endorsementData.EffectiveDate : item.EffectiveDate;
                    newAdditionalInterest.ExpirationDate = (item.RemoveProcessDate >= policyHistories.LastOrDefault().DateCreated.Value) ? endorsementData.EffectiveDate : item.ExpirationDate;
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
            var riskForms = await _riskFormRepository.GetAsync(x => x.RiskDetailId == previousRiskDetailId);
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
                    var isNewlyAdded = previousManuscripts.All(oldObj => oldObj.CreatedDate != x.CreatedDate);
                    var newManuscripts = SystemExtension.Clone<RiskManuscripts>(x);
                    newManuscripts.Id = Empty;
                    newManuscripts.EffectiveDate = (x.IsProrate && x.DeletedDate == null) && isNewlyAdded ? endorsementData.EffectiveDate : x.EffectiveDate;
                    newManuscripts.ExpirationDate = (x.IsProrate && x.Premium != x.ProratedPremium && x.DeletedDate >= policyHistories.LastOrDefault().DateCreated.Value) ? endorsementData.EffectiveDate : x.ExpirationDate;
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

            #region FileUploadDocuments
            // Set all documents to IsSystemGenerated = true. Disable editing and deletion
            //if (fileUploadDocuments != null)
            //{
            //    foreach (var item in fileUploadDocuments)
            //    {
            //        item.IsSystemGenerated = true; // Set to true to disable editing and deletion after issuance.
            //    }
            //    _fileUploadDocumentRepository.UpdateRange(fileUploadDocuments);
            //}
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

            // The 'newRiskDetail.RiskDetailVehicles' after cloning from old RiskDetail
            // must be updated so we back up its reference.
            var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles.Where(rdv => rdv.Vehicle.IsIncludedInOption(bindOption)).ToList();
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
                if(endorsementData.EndorsementNumber == 0 && !(driver.Options?.Contains($"{bindOption}") ?? false))
                {
                    continue;
                }
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
            //var riskVehiclePremiums = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(riskDetailId);
            //foreach (var rvpColl in riskVehiclePremiums)
            //{
            //    var cloneRvpColl = await _riskVehiclePremiumFactory.CreateEmptyCollectionAsync(newRiskDetail.RiskId);
            //    foreach (var rvp in rvpColl.AsOrdered())
            //    {
            //        var cloneRvp = (RiskVehiclePremium)rvp.Clone();
            //        cloneRvp.RiskDetailId = newRiskDetailId;
            //        cloneRvpColl.AddOrUpdate(cloneRvp);
            //    }
            //    await _riskVehiclePremiumRepository.SaveAsync(cloneRvpColl);
            //}
            #endregion

            #region RiskPolicyPremium (GL)
            var riskPolicyPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetailId, Premium.TYPE_POLICY_GL);
            var cloneRppColl = await _riskPolicyPremiumFactory.CreateEmptyCollectionAsync(newRiskDetail.RiskId);
            foreach (var rpp in riskPolicyPremiums.Reverse())
            {
                var cloneRpp = (RiskPolicyPremium)rpp.Clone();
                cloneRpp.RiskDetailId = newRiskDetailId;
                cloneRppColl.AddOrUpdate(cloneRpp);
            }
            await _riskPolicyPremiumRepository.SaveAsync(cloneRppColl);
            #endregion

            var data = new EndorsementData(newRiskDetail, endorsementData.EndorsementChanges);
            data.RiskDTO = await _riskService.GetByIdIncludeAsync(newRiskDetailId);
            return data;
        }

        private dynamic SetId(dynamic entity, Guid riskDetailId)
        {
            entity.Id = Empty;
            entity.RiskDetailId = riskDetailId;
            return entity;
        }
    }
}
