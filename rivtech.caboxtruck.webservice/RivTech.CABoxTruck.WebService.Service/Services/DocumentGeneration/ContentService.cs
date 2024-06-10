using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration
{
    public class ContentService : IContentService
    {

        private readonly ICacheService _cacheService;
        private readonly ILimitsService _limitsService;
        private readonly IPolicyContactsService _policyContactsService;
        private readonly IRiskPolicyContactService _riskPolicyContactService;
        private readonly INameAndAddressService _nameAndAddressService;
        private readonly ITaxService _taxService;
        private readonly IBillingService _billingService;
        private readonly IVehicleService _vehicleService;
        private readonly IRiskService _riskService;
        private readonly IFormsService _formsService;
        private readonly IConfiguration _configuration;
        private readonly ISLNumberService _slNumberService;
        private readonly IStateTaxRepository _stateTaxRepository;
        private readonly IBackgroundServiceChecker _backgroundServiceChecker;
        private readonly ITokenProvider _tokenProvider;
        private readonly IFormsService _formService;
        private readonly IPolicyHistoryService _policyHistoryService;
        private readonly IFormRepository _formRepository;
        private readonly IMapper _mapper;

        private readonly IConfigurationSection _genericServiceSettings;
        private readonly string _genericBaseUrl;

        private bool _isEndorsement;

        public ContentService(IRiskService riskService,
            ILimitsService limitsService,
            IPolicyContactsService policyContactsService,
            INameAndAddressService nameAndAddressService,
            ITaxService taxService,
            IBillingService billingService,
            IVehicleService vehicleService,
            ICacheService cacheService,
            IFormsService formsService,
            IConfiguration configuration, 
            IRiskPolicyContactService riskPolicyContactService,
            ISLNumberService sLNumberService,
            IBackgroundServiceChecker backgroundServiceChecker,
            IStateTaxRepository stateTaxRepository,
            ITokenProvider tokenProvider,
            IFormsService formService,
            IPolicyHistoryService policyHistoryService,
            IFormRepository formRepository,
            IMapper mapper)
        {
            _riskService = riskService;
            _limitsService = limitsService;
            _policyContactsService = policyContactsService;
            _nameAndAddressService = nameAndAddressService;
            _taxService = taxService;
            _billingService = billingService;
            _vehicleService = vehicleService;
            _cacheService = cacheService;
            _configuration = configuration;
            _formsService = formsService;
            _riskPolicyContactService = riskPolicyContactService;
            _slNumberService = sLNumberService;
            _stateTaxRepository = stateTaxRepository;
            _backgroundServiceChecker = backgroundServiceChecker;
            _tokenProvider = tokenProvider;
            _formService = formService;
            _policyHistoryService = policyHistoryService;
            _formRepository = formRepository;
            _mapper = mapper;

            _genericServiceSettings = _configuration.GetSection("GenericService");
            _genericBaseUrl = _genericServiceSettings.GetValue<string>("BaseUrl");
        }

        public async Task<ReportData> GetReportData(Guid riskDetailId, bool isEndorsement = false)
        {
            _isEndorsement = isEndorsement;
            var riskDto = await _riskService.GetByIdIncludeAsync(riskDetailId);
            return new ReportData(riskDto) { DataSets = await GetDataSets(riskDetailId) };
        }

        public async Task<Dictionary<string, List<dynamic>>> GetDataSets(Guid riskDetailId)
        {
            var riskDto = await _riskService.GetByIdIncludeAsync(riskDetailId);
            var limitsDataSets = await _limitsService.GetPerStateAsync(riskDto.NameAndAddress?.State, riskDto.RaterVersionId);
            var policyContacts = await _policyContactsService.GetAllPolicyContacts();
            var riskPolicyContacts = await _riskPolicyContactService.GetByRiskDetailIdAsync(riskDetailId);
            var nameAndAddress = await _nameAndAddressService.GetNameAndAddress(riskDetailId);
            var brokerAddresses = riskDto.BrokerInfo?.Agency?.Entity?.EntityAddresses;
            var paymentPortalLink = _configuration?.GetSection("PaymentPortal")?.GetValue<string>("BaseUrl");
            var symbolTenResponses = await _formService.GetSymbolTenResponses(riskDetailId);
            var manuscriptForm = _mapper.Map<List<FormDTO>>(await _formRepository.GetAsync(f => f.Id == "ManuscriptEndorsementFullEarn", entity => entity.FormVersions));

            var taxDetails = await _taxService.GetTaxDetails(riskDto.RiskCoverages.Select(rc => rc.RiskCoveragePremium?.TaxDetailsId ?? Guid.Empty).ToList());
            var states = await GetStatesList();

            var slInfo = _slNumberService.GetByCreditOfficeIdStateAsync(riskDto.Binding?.CreditedOfficeId, riskDto.NameAndAddress?.State).Result;
            var stateTaxes = _stateTaxRepository.GetAsync(riskDto.NameAndAddress?.State, riskDto.BrokerInfo.EffectiveDate.Value).Result;
            var stateTax = stateTaxes?.SLTaxes?.FirstOrDefault()?.Rate ?? 0.00m;
            var sfTax = stateTaxes?.StampingFeeTax?.Rate ?? 0.00m;

            //TODO: Check source DB/Generic service
            var slBrokers = new[] {
                new { Id = "Rivington", Description = "Rivington Partners", Address1 = "1120 6th Ave 21st Floor", CityStateZip = "New York, NY 10036" },
                new { Id = "Vale", Description = "Vale Insurance Partners", Address1 = "1120 6th Ave 21st Floor", CityStateZip = "New York, NY 10036" },
                new { Id = "Applied", Description = "Applied Risk Services", Address1 = "1120 6th Ave 21st Floor", CityStateZip = "New York, NY 10036" }
            };

            //TODO: Update when this dataset is transfered to db
            var mainUse = new[]
            {
                new {Id = 1, Description = "Courier"},
                new {Id = 2, Description = "Commercial"},
                new {Id = 3, Description = "Service"}
            };

            var commonValues = new[]
            {
                new { Id = "PaymentPortalLink", Value = paymentPortalLink }
            };

            var installmentSchedule = _isEndorsement ? await _billingService.GetEndorsementInstallmentSchedule(riskDto.RiskId):
                await _billingService.GetInstallmentSchedule(riskDto);

            var allForms = await _formsService.GetByRiskIdAsync(riskDetailId);

            var policyVehicles = await _vehicleService.GetByRiskDetailIdForEndorsementAsync(riskDetailId, true);

            var dataSets = new Dictionary<string, List<dynamic>>
            {
                {"BindRequirementsOption", await _cacheService.GetKeyValueList("BindRequirementsOption")},
                {"CreditedOffice", await _cacheService.GetKeyValueList("CreditedOffice")},
                {"QuoteConditionsOption", await _cacheService.GetKeyValueList("QuoteConditionsOption")},
                {"BusinessTypes", await _cacheService.GetKeyValueList("BusinessType")},
                {"Symbols", await _cacheService.GetKeyValueList("Symbol")},
                {"PolicyContacts", policyContacts?.Cast<dynamic>().ToList()},
                {"RiskPolicyContacts", riskPolicyContacts?.Cast<dynamic>().ToList()},
                {"Addresses", nameAndAddress.Addresses?.Cast<dynamic>().ToList()},
                {"BrokerAddresses", brokerAddresses?.Cast<dynamic>().ToList()},

                {"AutoBILimits", limitsDataSets.AutoBILimits.Cast<dynamic>().ToList()},
                {"AutoPDLimits", limitsDataSets.AutoPDLimits.Cast<dynamic>().ToList()},
                {"MedPayLimits", limitsDataSets.MedPayLimits.Cast<dynamic>().ToList()},
                {"PipLimits", limitsDataSets.PipLimits.Cast<dynamic>().ToList()},
                {"UMBILimits", limitsDataSets.UMBILimits.Cast<dynamic>().ToList()},
                {"UIMBILimits", limitsDataSets.UIMBILimits.Cast<dynamic>().ToList()},
                {"UMPDLimits", limitsDataSets.UMPDLimits.Cast<dynamic>().ToList()},
                {"UIMPDLimits", limitsDataSets.UIMPDLimits.Cast<dynamic>().ToList()},
                {"LiabilityDeductibleLimits", limitsDataSets.LiabilityDeductibleLimits.Cast<dynamic>().ToList()},
                {"CargoLimits", limitsDataSets.CargoLimits.Cast<dynamic>().ToList()},
                {"GlLimits", limitsDataSets.GlLimits.Cast<dynamic>().ToList()},
                {"RefLimits", limitsDataSets.RefLimits.Cast<dynamic>().ToList()},
                {"CollisionLimits", limitsDataSets.CollisionLimits.Cast<dynamic>().ToList()},
                {"ComprehensiveLimits", limitsDataSets.ComprehensiveLimits.Cast<dynamic>().ToList()},
                {"TrailerInterchangeLimits", limitsDataSets.TrailerInterchangeLimits.Cast<dynamic>().ToList()},
                {"NonOwnedTrailerLimits", limitsDataSets.NonOwnedTrailerLimits.Cast<dynamic>().ToList()},
                {"TaxDetails", taxDetails.Cast<dynamic>().ToList()},
                {"SLBrokers", slBrokers.Cast<dynamic>().ToList()},
                {"SLInfo", new List<dynamic> { slInfo } },
                {"MainUse", mainUse.Cast<dynamic>().ToList()},
                {"States", states.Cast<dynamic>().ToList()},
                {"InstallmentSchedule", installmentSchedule.Cast<dynamic>().ToList()},
                {"AllForms", allForms.Cast<dynamic>().ToList()},
                {"PolicyVehicles", policyVehicles.Cast<dynamic>().ToList()},
                {"VehiclePremiumChanges", new List<VehiclePremiumChange>().Cast<dynamic>().ToList()},
                {"CommonValues", commonValues.Cast<dynamic>().ToList()},
                {"StateTax", new List<dynamic> { stateTax.ToString("0.###") + "%" }},
                {"SFTax", new List<dynamic> { sfTax.ToString("0.###") + "%" }},
                {"QuoteDate", new List<dynamic> { riskDto.RequestedQuoteDate }},
                {"SymbolTenResponses", symbolTenResponses.Cast<dynamic>().ToList() },
                {"ManuscriptForm", manuscriptForm.Cast<dynamic>().ToList() },
            };

            return dataSets;
        }

        private async Task<List<StateInfo>> GetStatesList()
        {
            var client = new HttpClient();
            var token = Data.Common.Services.GetAuthToken();
            if (_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }
            if (!String.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{_genericBaseUrl}/State");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<StateInfo>>(await response.Content.ReadAsStringAsync());
        }

    }
}
