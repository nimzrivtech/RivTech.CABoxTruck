using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Web;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using System.Text;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Context;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using System.Diagnostics;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System.IO;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Options;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RaterService : IRaterService
    {
        private readonly ILogger<RaterService> _logger;
        private readonly IRiskService _riskService;
        private readonly IVehicleService _vehicleService;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IApplicantAdditionalInterestService _applicantAdditionalInterestService;
        private readonly ILimitsService _limitsService;
        private readonly IBillingService _billingService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQuoteOptionsService _quoteOptionsService;
        private readonly IRaterInputMapperResolver _raterInputMappingResolver;
        private readonly IRaterInputValuesFactory _raterInputValuesFactory;
        private readonly IMapper _mapper;
        private readonly IPremiumService _premiumService;
        private readonly AppDbContext _context;
        private readonly IBindingRepository _bindingRepository;
        private readonly IApplogService _applogService;
        private readonly IRiskCoverageRepository _riskCoverageRepository;
        private readonly IChangeComparerContext _changeComparerContext;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly ICoverageLimitService _coverageLimitService;
        private readonly IBindingService _bindingService;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly INameAndAddressService _nameAndAddressService;
        public RaterService(
            ILogger<RaterService> logger,
            IRiskService riskService,
            IVehicleService vehicleService,
            ICoverageHistoryService coverageHistoryService,
            IApplicantAdditionalInterestService riskAdditionalInterestService,
            IQuoteOptionsService quoteOptionsService,
            ILimitsService limitsService,
            IBillingService billingService,
            IHttpClientFactory httpClientFactory,
            IRaterInputMapperResolver raterInputMappingResolver,
            IRaterInputValuesFactory raterInputValuesFactory,
            IMapper mapper,
            IPremiumService premiumService,
            AppDbContext context,
            IBindingRepository bindingRepository,
            IApplogService applogService,
            IRiskCoverageRepository riskCoverageRepository,
            IChangeComparerContext changeComparerContext,
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository,
            ICoverageLimitService coverageLimitService,
            IBindingService bindingService,
            IVehicleRepository vehicleRepository,
            INameAndAddressService nameAndAddressService
            )
        {
            _logger = logger;
            _riskService = riskService;
            _vehicleService = vehicleService;
            _coverageHistoryService = coverageHistoryService;
            _applicantAdditionalInterestService = riskAdditionalInterestService;
            _quoteOptionsService = quoteOptionsService;
            _limitsService = limitsService;
            _billingService = billingService;
            _httpClientFactory = httpClientFactory;
            _raterInputMappingResolver = raterInputMappingResolver;
            _raterInputValuesFactory = raterInputValuesFactory;
            _mapper = mapper;
            _premiumService = premiumService;
            _context = context;
            _bindingRepository = bindingRepository;
            _applogService = applogService;
            _riskCoverageRepository = riskCoverageRepository;
            _changeComparerContext = changeComparerContext;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _coverageLimitService = coverageLimitService;
            _bindingService = bindingService;
            _vehicleRepository = vehicleRepository;
            _nameAndAddressService = nameAndAddressService;
        }

        public async Task<dynamic> MapFields(Guid id, JObject requestBody, int optionNo = 1, bool isPremiumMapping = false, bool? fromPolicy = null)
        {
            int ADDITIONAL_INSURED_PREMIUM = 50;
            int WAIVER_OF_SUBROGATION_PREMIUM = 250;
            int PRIMARY_AND_NONCONTRIBUTORY_PREMIUM = 500;
            var getRiskDto = await _riskService.GetByIdIncludeAsync(id);
            var getLimitsList = await _limitsService.GetPerStateAsync(getRiskDto?.NameAndAddress?.State, getRiskDto.RaterVersionId);

            var feeDetails = _billingService.GetFeeDetailsByState(getRiskDto?.NameAndAddress?.State);

            decimal RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.SingleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.SinglePDServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.MultiplePDServiceFee)?.Amount ?? 0;

            decimal RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == FeeDetails.FlatMultiplePDServiceFee)?.Amount ?? 0;

            // can we call the vehicle from getRiskDto similar to driver?
            var veh = (await _vehicleService.GetByRiskDetailIdIncludeAsync(id))?
                    //.Where(vehicle => (vehicle.Options ?? "").Split(",").Any(opt => opt == $"{optionNo}"))
                    .OrderBy(vehicle => vehicle.VIN).ToList();

            var covHistory = await _coverageHistoryService.GetByIdAsync(id);
            var additionalInterest = await _applicantAdditionalInterestService.GetByRiskDetailIdActiveAsync(id);

            RiskCoverageDTO rskCov = getRiskDto?.RiskCoverages?.Where(x => x.OptionNumber == optionNo).FirstOrDefault();

            var drivers = getRiskDto?.Drivers?
                //.Where(driver => (driver.Options ?? "").Split(",").Any(opt => opt == $"{optionNo}"))
                .OrderBy(driver => driver.LicenseNumber).ToList();

            var idMapping = new Dictionary<int, Guid>();

            foreach (var x in requestBody)
            {
                string moduleName = x.Key;
                JToken value = x.Value;

                switch (moduleName)
                {
                    case "SubmissionNo":
                        {
                            break;
                        }
                    case "Applicant":
                        {
                            var bussnsDet = getRiskDto?.BusinessDetails;
                            string useClass = ParseListToValue(bussnsDet?.MainUseId, requestBody[moduleName]["BusinessDetails"]["UseClass"]);
                            string accountCategory = ParseListToValue(bussnsDet?.AccountCategoryUWId, requestBody[moduleName]["BusinessDetails"]["AccountCategory"]); //Account Category (UW)
                            string businessType = ParseListToValue(bussnsDet?.BusinessTypeId, requestBody[moduleName]["BusinessDetails"]["BusinessType"]);
                            string newVenture = ParseListToValue(bussnsDet?.NewVenturePreviousExperienceId, requestBody[moduleName]["BusinessDetails"]["NewVenturePreviousExperience"]);
                            string mainGVW = ParseListToValue(bussnsDet?.MainGVWId, requestBody[moduleName]["BusinessDetails"]["MainGVW"]);
                            string secondaryBusinessGroup = ParseListToValue(bussnsDet?.SecondaryBusinessGroupId, requestBody[moduleName]["BusinessDetails"]["SecondaryBusinessGroup"]);

                            requestBody[moduleName]["BusinessDetails"]["UseClass"] = useClass;
                            requestBody[moduleName]["BusinessDetails"]["AccountCategory"] = accountCategory;
                            requestBody[moduleName]["BusinessDetails"]["BusinessType"] = businessType;
                            requestBody[moduleName]["BusinessDetails"]["ZipCode"] = $"'{getRiskDto?.NameAndAddress?.ZipCode}"; // getRiskDto?.NameAndAddress?.ZipCode:D5
                            requestBody[moduleName]["BusinessDetails"]["OperatingZipCode"] = $"'{getRiskDto?.NameAndAddress?.ZipCodeOperations}"; // getRiskDto?.NameAndAddress?.ZipCodeOperations:D5

                            requestBody[moduleName]["BusinessDetails"]["YearsInBusiness"] = bussnsDet?.YearsInBusiness <= 0 ? newVenture : bussnsDet?.YearsInBusiness.ToString(); //autogenerated from ui

                            requestBody[moduleName]["BusinessDetails"]["MainGVW"] = mainGVW;
                            requestBody[moduleName]["BusinessDetails"]["SecondaryBusinessGroup"] = secondaryBusinessGroup;
                            break;
                        }
                    case "Driver":
                        {
                            if (getRiskDto?.DriverHeader != null)
                            {
                                requestBody[moduleName]["UseDriverRatingTab"] = getRiskDto?.DriverHeader?.driverRatingTab.ToYesNo();
                                requestBody[moduleName]["UseAccidentsOrViolationInfo"] = getRiskDto?.DriverHeader?.accidentInfo.ToYesNo();
                                requestBody[moduleName]["KOUnder25"] = getRiskDto?.DriverHeader?.applyFilter?.Split(',').Any(x => x == "1").ToYesNo();
                                requestBody[moduleName]["KOAFOrMovingViolations"] = getRiskDto?.DriverHeader?.applyFilter?.Split(',').Any(x => x == "2").ToYesNo();
                            }

                            drivers = drivers.Where(x => ((x.Options ?? "").Split(',')).Contains(optionNo.ToString()) == true && x.IsValidated == true && x.IsValidatedKO == true).ToList();

                            if (drivers.Count > 0)
                            {
                                JArray myarray = new JArray();

                                foreach (var dvr in drivers)
                                {
                                    var jObject = new JObject
                                    {
                                        { "DriverID", dvr.Id },
                                        { "DateOfBirth", dvr.BirthDate?.ToString("MM/dd/yyyy") },
                                        { "Age", dvr.Age },
                                        { "OutOfState", dvr.IsOutOfState.ToYesNo() },
                                        { "YrsDrivingExperience", dvr.YrsDrivingExp ?? 0 },
                                        { "YrsCommercialDriverExperience", dvr.YrsCommDrivingExp },
                                        { "Majors", dvr.DriverIncidents.Any(x=>x.IncidentType == "1") ? "1" : "0" },
                                        { "Drug", dvr.DriverIncidents.Any(x=>x.IncidentType == "2")  ? "1" : "0"  },
                                        { "Minors", dvr.DriverIncidents.Any(x=>x.IncidentType == "3") ? "1" : "0"  },
                                        { "AtFault", dvr.DriverIncidents.Any(x=>x.IncidentType == "4") ? "1" : "0"  },
                                        { "MajorLicenseIssue", dvr.DriverIncidents.Any(x=>x.IncidentType == "5") ? "1" : "0"  },
                                        { "NAF", dvr.DriverIncidents.Any(x=>x.IncidentType == "6") ? "1" : "0"  },
                                        { "Equipment", dvr.DriverIncidents.Any(x=>x.IncidentType == "7") ? "1" : "0"  },
                                        { "Speed", dvr.DriverIncidents.Any(x=>x.IncidentType == "8") ? "1" : "0"  },
                                        { "Other", dvr.DriverIncidents.Any(x=>x.IncidentType == "9") ? "1" : "0"  },
                                    };

                                    myarray.Add(jObject);
                                }
                                requestBody[moduleName]["DriverList"] = myarray;
                            }

                            break;
                        }
                    case "Vehicle":
                        {

                            veh = veh.Where(x => ((x.Options ?? "").Split(',')).Contains(optionNo.ToString()) == true && x.IsValidated == true).ToList();
                            if (veh.Count == 0)
                                throw new RaterException($"No selected vehicles for option {optionNo}.");

                            if (veh.Count > 0)
                            {
                                JArray myarray = new JArray();
                                int autoLiabilityUnits = veh.Count; // all vehicles are autoliability units
                                int physicalDamageUnits = 0;

                                requestBody[moduleName]["TotalVehicles"] = veh.Count;
                                bool isPDLimitsEnabled = (rskCov.CollDeductibleId ?? 65) != 65 || ((rskCov.CompDeductibleId ?? 76) != 76 || (rskCov.FireDeductibleId ?? 76) != 76);

                                var row = 1;
                                foreach (var _veh in veh)
                                {
                                    short? compId = _veh.CoverageFireTheftDeductibleId ?? 76;
                                    short? collId = _veh.CoverageCollisionDeductibleId ?? 65;
                                    if (optionNo > 1)
                                    {
                                        if (rskCov.CompDeductibleId != null)
                                        {
                                            compId = rskCov.CompDeductibleId;
                                        }
                                        else if (rskCov.FireDeductibleId != null)
                                        {
                                            compId = rskCov.FireDeductibleId;
                                        }

                                        collId = rskCov.CollDeductibleId;
                                    }

                                    if (compId != 76 || collId != 65)
                                    {
                                        physicalDamageUnits++;
                                    }

                                    idMapping[row] = _veh.Id.Value;
                                    var jObject = new JObject
                                    {
                                        { "Number", row },
                                        { "VIN", _veh.VIN },
                                        { "Year", _veh.Year },
                                        { "VehicleType", _veh.VehicleType }, // { "VehicleType", parseListToValue(_veh.VehicleType, requestBody[moduleName]["VehicleDropdowns"]["VehicleType"])  },
                                        { "Description", ParseListToValue(_veh.VehicleDescriptionId, requestBody[moduleName]["VehicleDropdowns"]["Description"]) },
                                        { "CompDeductible", ParseListToValue(compId, requestBody[moduleName]["VehicleDropdowns"]["CompDeductible"]) },
                                        { "CollDeductible", ParseListToValue(collId, requestBody[moduleName]["VehicleDropdowns"]["CollDeductible"]) },
                                        { "CollStateAmount", _veh.StatedAmount },
                                        { "IncCargoLimit", _veh.HasCoverageCargo.ToYesNo() },
                                        { "IncRefCargoLimit",  _veh.HasCoverageRefrigeration.ToYesNo() },
                                        { "UseClass", ParseListToValue(_veh.UseClassId, requestBody[moduleName]["VehicleDropdowns"]["UseClass"]) },
                                        { "BusinessClass", ParseListToValue(_veh.VehicleBusinessClassId, requestBody[moduleName]["VehicleDropdowns"]["BusinessClass"]) },
                                        { "Radius", _veh.Radius },
                                    };

                                    myarray.Add(jObject);
                                    row++;
                                }

                                decimal riskManagementFeeAL = (autoLiabilityUnits > 1) ? (autoLiabilityUnits * RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT) : (autoLiabilityUnits == 1) ? RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT : 0;
                                decimal riskManagementFeePD = (physicalDamageUnits > 1) ? (physicalDamageUnits * RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT) : (physicalDamageUnits == 1) ? RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT : 0;

                                if (RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT > 0)
                                    riskManagementFeeAL = RISK_MANAGEMENT_FEE_FLAT_AL_MULTIPLE_UNIT;
                                if (RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT > 0)
                                    riskManagementFeePD = RISK_MANAGEMENT_FEE_FLAT_PD_MULTIPLE_UNIT;
                                requestBody[moduleName]["RiskManagementFeeAL"] = riskManagementFeeAL;
                                requestBody[moduleName]["RiskManagementFeePD"] = riskManagementFeePD;
                                requestBody[moduleName]["VehicleList"] = myarray;

                            }

                            //removing unnecessary nodes.
                            var remooveVehicleDropdowns = (requestBody[moduleName] as JObject);
                            var token = remooveVehicleDropdowns.SelectToken("VehicleDropdowns");
                            if (token != null) token.Parent.Remove();

                            break;
                        }
                    case "AdditionalInterest":
                        {
                            bool IsBlanketCoverageAdditionalInsured = getRiskDto?.BlanketCoverage?.IsAdditionalInsured ?? false;
                            bool IsBlanketCoveragePrimaryAndNonContributory = getRiskDto?.BlanketCoverage?.IsPrimaryNonContributory ?? false;
                            bool IsBlanketCoverageWaiverOfSubrogation = getRiskDto?.BlanketCoverage?.IsWaiverOfSubrogation ?? false;

                            if (additionalInterest.Count() > 0)
                            {
                                requestBody[moduleName]["NoOfAdditionalInsured"] = additionalInterest.Count(x => x.AdditionalInterestTypeId == AdditionalInterestType.AdditionalInsured.Id);
                                requestBody[moduleName]["WaiverOfSubrogation"] = additionalInterest.Any(x => x.IsWaiverOfSubrogation == true).ToYesNo();
                                requestBody[moduleName]["PrimaryNonContributoryCoverage"] = additionalInterest.Any(x => x.IsPrimaryNonContributory == true).ToYesNo();
                                // additional interest premiums & blanket coverage
                                requestBody[moduleName]["AdditionalInsuredPremium"] = (IsBlanketCoverageAdditionalInsured) ? 500 : (((int)requestBody[moduleName]["NoOfAdditionalInsured"]) * ADDITIONAL_INSURED_PREMIUM);
                                requestBody[moduleName]["PrimaryNonContributoryPremium"] = (IsBlanketCoveragePrimaryAndNonContributory) ? 1500 : (additionalInterest.Where(x => x.IsPrimaryNonContributory == true).Count() * PRIMARY_AND_NONCONTRIBUTORY_PREMIUM);
                                requestBody[moduleName]["WaiverOfSubrogationPremium"] = (IsBlanketCoverageWaiverOfSubrogation) ? 1000 : (additionalInterest.Where(x => x.IsWaiverOfSubrogation == true).Count() * WAIVER_OF_SUBROGATION_PREMIUM);
                            }
                            else
                            {
                                requestBody[moduleName]["AdditionalInsuredPremium"] = (IsBlanketCoverageAdditionalInsured) ? 500 : 0;
                                requestBody[moduleName]["PrimaryNonContributoryPremium"] = (IsBlanketCoveragePrimaryAndNonContributory) ? 1500 : 0;
                                requestBody[moduleName]["WaiverOfSubrogationPremium"] = (IsBlanketCoverageWaiverOfSubrogation) ? 1000 : 0;
                            }
                            break;
                        }
                    case "RiskSpecifics":
                        {
                            if (getRiskDto?.RadiusOfOperations != null)
                            {
                                string ratingType = ParseListToValue(getRiskDto?.RadiusOfOperations?.RatingTypeId, requestBody[moduleName]["DestinationInformation"]["RatingTypes"]);
                                requestBody[moduleName]["DestinationInformation"]["RatingType"] = ratingType;
                            }

                            var inputMapping = _raterInputMappingResolver.Resolve(moduleName);
                            if (inputMapping != null)
                            {
                                await inputMapping.Map(getRiskDto, rskCov, requestBody);
                            }
                            break;
                        }
                    case "Policy":
                    case "Broker":
                    case "HistoricalCoverage":
                    case "ClaimsHistory":
                    case "RequestedCoverages":
                        {
                            var inputMapping = _raterInputMappingResolver.Resolve(moduleName);
                            if (inputMapping != null)
                            {
                                await inputMapping.Map(getRiskDto, rskCov, requestBody);
                            }
                            break;
                        }
                }

            }

            if (isPremiumMapping)
            {
                var vehicleIdMappingKey = Guid.NewGuid();
                PremiumRaterVehicleFactorsDTO.VehicleIdMappings.Add(vehicleIdMappingKey, idMapping);
                requestBody["VehicleIdMappingKey"] = vehicleIdMappingKey.ToString();
            }

            return requestBody;
        }

        public async Task<List<string>> GetOptions(Guid id)
        {
            var res = await _quoteOptionsService.GetRiskCoverageAndPremiumByIdAsync(id);
            return res.Select(s => s.OptionNumber.ToString()).ToList();
        }

        public dynamic ParseListToValue(dynamic dbVal, JToken node)
        {
            if (dbVal != null)
            {
                var val = node
                        ?.Where(x => (string)x["value"] == dbVal.ToString())
                        ?.Select(c => (string)c["label"]).FirstOrDefault();
                return SanitizeStringVal(val);
            }
            else return null;
        }

        private string SanitizeStringVal(string str)
        {
            return str?.Replace('\u00A0', ' ').Replace("$", "");
        }


        public async Task<bool> CopyRaterAsync(string submissionNumber, string newSubmissionNumber)
        {
            try
            {
                // Create content.
                var content = new
                {
                    From = submissionNumber,
                    To = newSubmissionNumber
                };
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");

                // Send api request.
                using var httpClient = _httpClientFactory.CreateClient("rater");
                var endpoint = "api/ExperienceRater/CopyToNewSubmission";
                var response = await httpClient.PostAsync(endpoint, stringContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task DeleteArchive(string submissionNumber, int endorsementNumber)
        {
            var client = _httpClientFactory.CreateClient("rater");
            var qryParams = $"submissionNumber={submissionNumber}"
                            + $"&endorsementNumber={endorsementNumber}";
            var result = await client.DeleteAsync($"api/PremiumRaterArchive?{qryParams}");
            if (!result.IsSuccessStatusCode)
                Console.WriteLine(result.ReasonPhrase);
        }

        public async Task<string> PreloadPremiumRater(string submissionNumber, Guid riskDetailId, bool? fromPolicy)
        {
            //Guid _riskDetailId = Guid.Parse(riskDetailId);
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            var risk = await _riskService.GetByIdIncludeAsync(riskDetailId);
            var brokerinfo = risk.BrokerInfo;
            short yearsInBusiness = risk.BusinessDetails?.YearsInBusiness ?? 0;

            if (brokerinfo?.EffectiveDate == null)
            {
                throw new RaterException("Inception Date is Required.");
            }

            DateTime inceptionDate = brokerinfo.EffectiveDate.Value;

            short optionNum = 1;
            if (risk.Binding != null)
            {
                optionNum = risk.Binding.BindOptionId ?? 1;
            }
            var covPrem = risk.RiskCoverages.FirstOrDefault(c => c.OptionNumber == optionNum);

            if (covPrem?.LiabilityExperienceRatingFactor == null && covPrem?.LiabilityScheduleRatingFactor == null)
            {
                throw new RaterException("Liability Schedule RF and Liability Experience RF is not defined");
            }

            return await PreloadPremiumRater(submissionNumber, risk.RaterVersionId, fromPolicy ?? false);
        }

        public async Task<string> PreloadPremiumRater(string submissionNumber, int raterVersionId, bool fromPolicy = true)
        {
            //var options = await _raterService.GetOptions(riskDetailId);
            //if (fromPolicy != true && options.Count < 1)
            //    return new JsonResult(new { status = false, message = "No Option Selected." }, new JsonSerializerSettings());

            // itemCount =  // total Number of pool item from ui
            string url = $"api/PremiumRater/preload/{submissionNumber}?raterVersionId={raterVersionId}";
            if (fromPolicy == true)
                url += $"&fromPolicy={fromPolicy}";

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var preloadResponse = await httpClient.PostAsync(url, content);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException($"{result}. You may refresh your browser.");
            }

            return result;
        }


        public async Task UnloadPremiumRaterAsync(string submissionNumber)
        {
            string url = $"api/PremiumRater/force-unload-for-api/{submissionNumber}";

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var preloadResponse = await httpClient.DeleteAsync(url);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException($"{result}");
            }
        }

        public async Task CalculatePremiumAsync(Guid guid, int endorsementNumber, bool fromPolicy, int optionId = 0, bool forIssuance = false)
        {
            RiskDTO riskDTO = await _riskService.GetByIdIncludeAsync(guid);

            var requestBody = new
            {
                SubmissionNumber = riskDTO.SubmissionNumber,
                InceptionDate = riskDTO.BrokerInfo.EffectiveDate,
                OptionId = optionId,
                EndorsementNumber = endorsementNumber,
                InputValues = await _raterInputValuesFactory.CreateAsync(riskDTO)
            };

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"api/PremiumRater/rate/{riskDTO.SubmissionNumber}?&fromPolicy={fromPolicy}&forIssuance={forIssuance}", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            var result = await response.Content.ReadAsStringAsync();
            //int optionId = requestBody.SelectToken("OptionId").Value<int>();

            if (fromPolicy != true)
            {
                await _vehicleService.RemovePremiumRatingFactorOfUncheckedsAsync(guid, Convert.ToInt32(optionId));
            }

            result = await ExtractAndSaveRatingFactors(guid, result, fromPolicy, riskDTO);

            await _applogService.Save($"Policy Rater result", result);
        }

        public async Task CalculatePremiumAsync(RiskDTO riskDTO, int endorsementNumber, bool fromPolicy, int optionId = 0, bool forIssuance = false)
        {
            var requestBody = new
            {
                SubmissionNumber = riskDTO.SubmissionNumber,
                InceptionDate = riskDTO.BrokerInfo.EffectiveDate,
                OptionId = optionId,
                EndorsementNumber = endorsementNumber,
                InputValues = await _raterInputValuesFactory.CreateAsync(riskDTO)
            };

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"api/PremiumRater/rate/{riskDTO.SubmissionNumber}?&fromPolicy={fromPolicy}&forIssuance={forIssuance}", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            var result = await response.Content.ReadAsStringAsync();
            //int optionId = requestBody.SelectToken("OptionId").Value<int>();

            if (fromPolicy != true)
            {
                await _vehicleService.RemovePremiumRatingFactorOfUncheckedsAsync(riskDTO.Id.Value, Convert.ToInt32(optionId));
            }

            result = await ExtractAndSaveRatingFactors(riskDTO.Id.Value, result, fromPolicy, riskDTO);

            await _applogService.Save($"Policy Rater result", result);
        }

        public async Task<string> ExtractAndSaveRatingFactors(Guid guid, string calculatedResults, bool fromPolicy, RiskDTO riskDTO)
        {
            try
            {
                JObject obj = JObject.Parse(calculatedResults);
                bool isSuccess = obj.SelectToken("success")?.Value<bool?>() ?? false;
                if (!isSuccess)
                {
                    string errorMsg = obj.SelectToken("error")?.Value<string>();
                    throw new ArgumentException($"2-B. Error encountered while calculating premium. {errorMsg}");
                }

                string optionVal = obj.SelectToken("data.optionId")?.Value<string>();
                int optionId = string.IsNullOrWhiteSpace(optionVal) ? 0 : Convert.ToInt32(optionVal);

                // extract vehicle factors
                var resultVehicles = obj.SelectToken("data.result.Vehicle")?.Select(current => new PremiumRaterVehicleFactorsDTO()
                {
                    RiskDetailId = guid,
                    VehicleId = current.SelectToken("VehicleID")?.Value<string>(),
                    // ALPremium = current.SelectToken("ALPremium").Value<decimal?>(),
                    TotalPremium = current.SelectToken("TotalPremium")?.Value<decimal?>(),
                    TotalALPremium = current.SelectToken("ALPremium")?.Value<decimal?>(),
                    // TotalALAddOns = current.SelectToken("TotalALAddOns").Value<decimal?>(),
                    TotalPDPremium = current.SelectToken("PDPremium")?.Value<decimal?>(),
                    BIPremium = current.SelectToken("BIPremium")?.Value<decimal?>(),
                    PropDamagePremium = current.SelectToken("PropDamagePremium")?.Value<decimal?>(),
                    MedPayPremium = current.SelectToken("MedPayPremium")?.Value<decimal?>(),
                    PIPPremium = current.SelectToken("PIPPremium")?.Value<decimal?>(),
                    // PIOtherPremium = current.SelectToken("PIOtherPremium").Value<decimal?>(),
                    UMUIM_BI_Premium = current.SelectToken("UMUIM_BI_Premium")?.Value<decimal?>(),
                    UMUIM_PD_Premium = current.SelectToken("UMUIM_PD_Premium")?.Value<decimal?>(),
                    CompPremium = current.SelectToken("CompPremium")?.Value<decimal?>(),
                    FireTheftPremium = current.SelectToken("FireTheftPremium")?.Value<decimal?>(),
                    CollisionPremium = current.SelectToken("CollisionPremium")?.Value<decimal?>(),
                    CargoPremium = current.SelectToken("CargoPremium")?.Value<decimal?>(),
                    // NTLPremium = current.SelectToken("NTLPremium").Value<decimal?>(),
                    // BIRiskMgmtFee = current.SelectToken("BIRiskMgmtFee").Value<decimal?>(),
                    // PDRiskMgmtFee = current.SelectToken("PDRiskMgmtFee").Value<decimal?>(),
                    // StatedAmount = current.SelectToken("StatedAmount").Value<decimal?>(),
                    // TIVPercentage = current.SelectToken("TIVPercentage").Value<decimal?>(),
                    PerVehiclePremium = current.SelectToken("PerVehiclePremium")?.Value<decimal?>(),
                    Radius = riskDTO.RaterVersionId > 3 ? null : current.SelectToken("Radius")?.Value<decimal?>(),
                    RadiusTerritory = current.SelectToken("RadiusTerritory")?.Value<decimal?>(),
                    OptionId = optionId
                }).ToList();

                var vehiclesWithPremiumError = resultVehicles.Where(x => x.TotalPremium is null);
                if (vehiclesWithPremiumError.Any())
                {
                    throw new RaterException("Some VINs have error premium computed by rater.");
                }

                // Revert back the correct vehicle id. 
                string vehicleIdMappingKey = obj.SelectToken("data.inputValues.Vehicle.IdMappingKey")?.Value<string>();
                if (!string.IsNullOrWhiteSpace(vehicleIdMappingKey))
                {
                    var idMapping = PremiumRaterVehicleFactorsDTO.VehicleIdMappings[Guid.Parse(vehicleIdMappingKey)];
                    foreach (var veh in resultVehicles)
                        veh.VehicleId = idMapping[int.Parse(veh.VehicleId)].ToString();

                    foreach (var vehToken in obj.SelectToken("data.result.Vehicle"))
                        vehToken.SelectToken("VehicleID").Replace(idMapping[int.Parse(vehToken.SelectToken("VehicleID").Value<string>())].ToString());

                    PremiumRaterVehicleFactorsDTO.VehicleIdMappings.Remove(Guid.Parse(vehicleIdMappingKey));
                }

                //Add Refrigeration
                var vehicleRefs = obj.SelectToken("data.result.VehicleRef")?.Select(vr => new
                {
                    Value = vr?.SelectToken("value1")?.Value<decimal?>() + vr?.SelectToken("value2")?.Value<decimal?>()
                }).ToList();

                if (vehicleRefs.Any())
                {
                    for (int i = 0; i < resultVehicles.Count; i++)
                    {
                        var v = resultVehicles[i];
                        var vehicleRef = vehicleRefs.ElementAtOrDefault(i);
                        v.RefrigerationPremium = vehicleRef?.Value ?? 0;
                    }
                }

                // extract endorsement premiums
                var hiredPhysicalDmg = obj.SelectToken("data.inputValues.EndorsementPremiums")?.SelectToken("HiredPhysicalDamage")?.Value<decimal?>();
                var trailerInterchange = obj.SelectToken("data.inputValues.EndorsementPremiums")?.SelectToken("TrailerInterchange")?.Value<decimal?>();
                var alManuscriptPremium = obj.SelectToken("data.inputValues.EndorsementPremiums")?.SelectToken("ALManuscriptPremium")?.Value<decimal?>();
                var pdManuscriptPremium = obj.SelectToken("data.inputValues.EndorsementPremiums")?.SelectToken("PDManuscriptPremium")?.Value<decimal?>();
                var endorsementPremiums = (hiredPhysicalDmg ?? 0) + (trailerInterchange ?? 0);
                // extract Driver factors
                var objDriverList = obj.SelectToken("data.result.Driver")?.Select(current => new PremiumRaterDriverFactorsDTO()
                {
                    RiskDetailId = guid,
                    DriverID = current.SelectToken("DriverID")?.Value<string>(),
                    FiveYearPoints = current.SelectToken("FiveYearPoints")?.Value<string>(),
                    AgeFactor = current.SelectToken("AgeFactor")?.Value<string>(),
                    TotalFactor = current.SelectToken("TotalFactor")?.Value<string>(),
                    OptionId = optionId
                }).ToList();
                // extract other factors and premiums
                var objFactors = new PremiumRatingFactorsDTO()
                {
                    RiskDetailId = guid,
                    RiskCoverageId = Guid.Parse(obj.SelectToken("data.inputValues")?.SelectToken("RiskCoverageId")?.Value<string>()),
                    AccountDriverFactor = obj.SelectToken("data.result")?.SelectToken("AccountDriverFactor")?.Value<string>(),
                    AdditionalInsuredPremium = obj.SelectToken("data.inputValues.AdditionalInterest")?.SelectToken("AdditionalInsuredPremium")?.Value<string>(),
                    GeneralLiabilityPremium = obj.SelectToken("data.result")?.SelectToken("GeneralLiabilityPremium")?.Value<string>(),
                    PrimaryNonContributoryPremium = obj.SelectToken("data.inputValues.AdditionalInterest")?.SelectToken("PrimaryNonContributoryPremium")?.Value<string>(),
                    RiskManagementFeeAL = obj.SelectToken("data.inputValues.Vehicle")?.SelectToken("RiskManagementFeeAL")?.Value<string>(),
                    RiskManagementFeeAPD = obj.SelectToken("data.inputValues.Vehicle")?.SelectToken("RiskManagementFeePD")?.Value<string>(),
                    TotalCargoPremium = $"{resultVehicles.Sum(vehicles => vehicles.CargoPremium ?? 0.00m)}",
                    TotalLiabilityPremium = $"{resultVehicles.Sum(vehicles => vehicles.TotalALPremium ?? 0.00m) + (alManuscriptPremium ?? 0.00m)}",
                    TotalPhysDamPremium = $"{resultVehicles.Sum(vehicles => vehicles.TotalPDPremium ?? 0.00m) + endorsementPremiums + (pdManuscriptPremium ?? 0.00m)}",
                    ComprehensivePremium = $"{resultVehicles.Sum(vehicles => vehicles.CompPremium ?? 0.00m) + resultVehicles.Sum(vehicles => vehicles.FireTheftPremium ?? 0.00m)}",
                    CollisionPremium = $"{resultVehicles.Sum(vehicles => vehicles.CollisionPremium ?? 0.00m)}",
                    WaiverOfSubrogationPremium = obj.SelectToken("data.inputValues.AdditionalInterest")?.SelectToken("WaiverOfSubrogationPremium")?.Value<string>(),
                    HiredPhysicalDamage = $"{hiredPhysicalDmg ?? 0}",
                    TrailerInterchange = $"{trailerInterchange ?? 0}",
                    ALManuscriptPremium = $"{alManuscriptPremium ?? 0}",
                    PDManuscriptPremium = $"{pdManuscriptPremium ?? 0}",
                    OptionId = optionId,
                    VehicleFactors = resultVehicles,
                    DriverFactors = objDriverList,
                };

                if (riskDTO.RaterVersionId >= 7)
                {
                    objFactors.TrailerInterchangePremium = obj.SelectToken("data.result")?.SelectToken("TrailerInterchangePremium")?.Value<string>();
                    objFactors.NonOwnedTrailerPremium = obj.SelectToken("data.result")?.SelectToken("NonOwnedTrailerPremium")?.Value<string>();
                }

                if (!fromPolicy)
                {
                    var ratio = obj.SelectToken("data.result.DriverVehicleRatio")?.ToString();
                    objFactors.DriverVehicleRatio = ratio != null ? decimal.Parse(ratio) : (decimal?)null;
                }

                // save factors
                bool isFactorsSaved = objFactors != null && await SaveFactors(objFactors, riskDTO, fromPolicy, alManuscriptPremium);
                if (!isFactorsSaved)
                    throw new ArgumentException($"Error encountered while saving factors. The list of factors does not match with the number of factors saved.");

                return JsonConvert.SerializeObject(obj);
            }
            catch (RaterException e)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error encountered while saving premiums and factors. {e.Message}");
                throw;
            }
        }

        private async Task<bool> SaveFactors(PremiumRatingFactorsDTO factorsData, RiskDTO riskDTO, bool? fromPolicy = null, decimal? alManuscriptPremium = null)
        {
            bool isFactorsSaved = false;
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            var forEndorsementVehicleFactorDTOs = new List<PremiumRaterVehicleFactorsDTO>();
            var premiumFactorDTOs = new List<PremiumRaterVehicleFactorsDTO>();
            var risk = await _riskRepository.GetRiskByIdAsync(riskDTO.RiskId);

            var binding = await _bindingRepository.GetByRiskAsync(riskDTO.RiskId);

            //var optionNumber = fromPolicy != true ? binding.BindOptionId : factorsData.OptionId;
            var optionNumber = factorsData.OptionId;

            var existingRiskCoveragePremium = await _context.RiskCoveragePremium.FirstOrDefaultAsync(x => x.RiskCoverageId == factorsData.RiskCoverageId);

            executionStrategy.Execute(() =>
            {
                var sw = new Stopwatch();
                sw.Start();
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    #region Vehicle
                    foreach (PremiumRaterVehicleFactorsDTO vehicleFactor in factorsData.VehicleFactors)
                    {
                        premiumFactorDTOs.Add(vehicleFactor);
                        var veh = _context.Vehicle.FirstOrDefault(o => o.Id.ToString() == vehicleFactor.VehicleId);
                        if ((fromPolicy != true && veh != null))
                        {
                            veh.AutoLiabilityPremium = vehicleFactor.TotalALPremium ?? 0.00m;
                            veh.CargoPremium = vehicleFactor.CargoPremium ?? 0.00m;
                            veh.RefrigerationPremium = vehicleFactor.RefrigerationPremium ?? 0.00m;
                            veh.PhysicalDamagePremium = vehicleFactor.TotalPDPremium ?? 0.00m;
                            veh.ComprehensivePremium = (vehicleFactor.CompPremium ?? 0.00m) + (vehicleFactor.FireTheftPremium ?? 0.00m);
                            veh.CollisionPremium = vehicleFactor.CollisionPremium ?? 0.00m;
                            veh.TotalPremium = vehicleFactor.PerVehiclePremium ?? 0.00m;
                            veh.RadiusTerritory = vehicleFactor.RadiusTerritory ?? 0.00m;
                            veh.Radius = riskDTO.RaterVersionId > 3 ? veh.Radius : vehicleFactor.Radius ?? 0.00m;
                            //veh.ReassignProratedPremiums();
                            _context.SaveChanges();
                        }
                        else if (veh != null && fromPolicy == true)
                        {
                            forEndorsementVehicleFactorDTOs.Add(vehicleFactor);
                        }
                        //forEndorsementVehicleFactorDTOs.Add(vehicleFactor);
                    }
                    #endregion Vehicle

                    #region Vehicle Factors
                    // save vehicle factors 
                    foreach (PremiumRaterVehicleFactorsDTO vehicleFactor in factorsData.VehicleFactors)
                    {
                        var existingVehicles = _context.VehiclePremiumRatingFactor.Where(x => 
                            x.RiskDetailId == vehicleFactor.RiskDetailId 
                            && x.VehicleId == vehicleFactor.VehicleId 
                            && (optionNumber == 0 || (x.OptionId == optionNumber)));
                        // Remove existing records.
                        _context.VehiclePremiumRatingFactor.RemoveRange(existingVehicles);
                        // Add new.
                        _context.Add(_mapper.Map<VehiclePremiumRatingFactor>(vehicleFactor));

                        bool saveFailed = false;
                        do
                        {
                            try
                            {
                                saveFailed = false;
                                _context.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                saveFailed = true;
                                ex.Entries.Single().Reload();
                            }
                        } while (saveFailed);
                    }
                    #endregion Vehicle Factors

                    #region Driver Header 
                    var driverHeader = _context.DriverHeader.FirstOrDefault(o => o.RiskDetailId == factorsData.RiskDetailId);
                    if (driverHeader != null)
                    {
                        bool shouldUpdateAcctDriverFactor = (fromPolicy == false && factorsData.OptionId == 1) || (fromPolicy == true);
                        bool shouldUpdateDriverVehicleRatio = fromPolicy == false;

                        if (shouldUpdateAcctDriverFactor)
                            driverHeader.accDriverFactor = factorsData.AccountDriverFactor.ToString();

                        if (shouldUpdateDriverVehicleRatio)
                            driverHeader.driverVehicleRatio = factorsData.DriverVehicleRatio.ToString();

                        _context.SaveChanges();
                    }
                    #endregion Driver Header Account Driver Factor

                    #region Driver
                    foreach (PremiumRaterDriverFactorsDTO driverFactors in factorsData.DriverFactors)
                    {
                        //var Driver = _context.Driver.FirstOrDefault(o => o.RiskDetailId == driverFactors.RiskDetailId && o.Id.ToString() == driverFactors.DriverID);
                        var Driver = _context.RiskDetailDriver
                                .Include(i => i.Driver)
                                .Where(o => o.RiskDetailId == driverFactors.RiskDetailId && o.DriverId.ToString() == driverFactors.DriverID)
                                .Select(x => x.Driver)
                                .FirstOrDefault();

                        if (Driver != null)
                        {
                            // Fix for NEWCABT-783: Initially get 4 decimal places before reducing to 2 decimal places to prevent rounding errors
                            Driver.TotalFactor = decimal.Round(Convert.ToDecimal(driverFactors.TotalFactor), 4, MidpointRounding.AwayFromZero).ToString("N2");
                            Driver.AgeFactor = decimal.Round(Convert.ToDecimal(driverFactors.AgeFactor), 4, MidpointRounding.AwayFromZero).ToString("N2");
                            Driver.YearPoint = parseValueToDecimal(driverFactors.FiveYearPoints).ToString();
                            _context.SaveChanges();
                        }
                    }
                    #endregion Driver

                    #region Driver Factors
                    foreach (PremiumRaterDriverFactorsDTO driverFactors in factorsData.DriverFactors)
                    {
                        var existingDrivers = _context.DriverPremiumRatingFactor.FirstOrDefault(x => x.RiskDetailId == driverFactors.RiskDetailId && x.DriverID == driverFactors.DriverID && x.OptionId == optionNumber);
                        if (existingDrivers == null)
                        {
                            _context.Add(_mapper.Map<DriverPremiumRatingFactor>(driverFactors));
                        }
                        else
                        {
                            existingDrivers.RiskDetailId = driverFactors.RiskDetailId;
                            existingDrivers.DriverID = driverFactors.DriverID;
                            existingDrivers.FiveYearPoints = $"{parseValueToDecimal(driverFactors.FiveYearPoints)}";
                            //existingDrivers.DriverKey = driverFactors.DriverKey;
                            //existingDrivers.PointsFactor = driverFactors.PointsFactor;
                            //existingDrivers.YearsExperienceFactor = driverFactors.YearsExperienceFactor;
                            existingDrivers.AgeFactor = $"{decimal.Round(Convert.ToDecimal(driverFactors.AgeFactor), 4, MidpointRounding.AwayFromZero):N2}";
                            //existingDrivers.OutOfStateFactor = driverFactors.OutOfStateFactor;
                            existingDrivers.TotalFactor = $"{decimal.Round(Convert.ToDecimal(driverFactors.FiveYearPoints), 4, MidpointRounding.AwayFromZero):N2}";
                            //existingDrivers.Included = driverFactors.Included;
                            //existingDrivers.TotalFactorIncl = driverFactors.TotalFactorIncl;
                            //existingDrivers.FactorIncl_AddDriver = driverFactors.FactorIncl_AddDriver;
                            //existingDrivers.UnderAge25 = driverFactors.UnderAge25;
                            //existingDrivers.UnderAge25AndClean = driverFactors.UnderAge25AndClean;
                            //existingDrivers.HasMultipleAFOrMovingViolation = driverFactors.HasMultipleAFOrMovingViolation;
                            existingDrivers.OptionId = driverFactors.OptionId;
                            _context.Update(existingDrivers);
                        }
                    }
                    #endregion Driver Factors

                    #region Risk Coverage Premium
                    var existingRiskCoverageFromBind = _context.RiskCoverage.FirstOrDefault(x => x.RiskDetailId == riskDTO.Id);

                    if (existingRiskCoveragePremium == null)
                    {
                        decimal vehicleLevelPremium = factorsData.VehicleFactors.Sum(vehicles => vehicles.PerVehiclePremium) ?? 0.00m;
                        decimal totalPremium = parseValueToDecimal(factorsData.TotalCargoPremium) +
                                                parseValueToDecimal(factorsData.TotalLiabilityPremium) +
                                                parseValueToDecimal(factorsData.TotalPhysDamPremium) +
                                                parseValueToDecimal(factorsData.GeneralLiabilityPremium) +
                                                parseValueToDecimal(factorsData.AdditionalInsuredPremium) +
                                                parseValueToDecimal(factorsData.WaiverOfSubrogationPremium) +
                                                parseValueToDecimal(factorsData.PrimaryNonContributoryPremium) +
                                                parseValueToDecimal(factorsData.TrailerInterchangePremium) +
                                                parseValueToDecimal(factorsData.NonOwnedTrailerPremium);
                        decimal totalALPremium = parseValueToDecimal(factorsData.TotalLiabilityPremium) +
                                                 parseValueToDecimal(factorsData.AdditionalInsuredPremium) +
                                                 parseValueToDecimal(factorsData.WaiverOfSubrogationPremium) +
                                                 parseValueToDecimal(factorsData.PrimaryNonContributoryPremium);
                        decimal fees = parseValueToDecimal(factorsData.RiskManagementFeeAL) + parseValueToDecimal(factorsData.RiskManagementFeeAPD);

                        var riskCov = new RiskCoveragePremium()
                        {
                            RiskCoverageId = factorsData.RiskCoverageId,
                            AccountDriverFactor = parseValueToDecimal(factorsData.AccountDriverFactor),
                            AdditionalInsuredPremium = parseValueToDecimal(factorsData.AdditionalInsuredPremium),
                            AvgPerVehicle = (vehicleLevelPremium > 0 && factorsData.VehicleFactors.Count > 0) ? decimal.Round((vehicleLevelPremium / factorsData.VehicleFactors.Count), 2, MidpointRounding.AwayFromZero) : 0.00m,
                            GeneralLiabilityPremium = parseValueToDecimal(factorsData.GeneralLiabilityPremium),
                            Fees = decimal.Round(fees, 2, MidpointRounding.AwayFromZero),
                            //PremiumTaxesFees = parseValueToDecimal(factorsData.PremiumTaxesFees),
                            PrimaryNonContributoryPremium = parseValueToDecimal(factorsData.PrimaryNonContributoryPremium),
                            RiskMgrFeeAL = parseValueToDecimal(factorsData.RiskManagementFeeAL),
                            RiskMgrFeePD = parseValueToDecimal(factorsData.RiskManagementFeeAPD),
                            //SubTotalDue = parseValueToDecimal(factorsData.SubTotalDue),
                            //TotalAnnualPremiumWithPremiumTax = parseValueToDecimal(factorsData.TotalAnnualPremiumWithPremiumTax),
                            CargoPremium = parseValueToDecimal(factorsData.TotalCargoPremium),
                            AutoLiabilityPremium = decimal.Round(totalALPremium, 2, MidpointRounding.AwayFromZero),
                            //TotalOtherPolicyLevelPremium = parseValueToDecimal(factorsData.TotalOtherPolicyLevelPremium),
                            PhysicalDamagePremium = parseValueToDecimal(factorsData.TotalPhysDamPremium),
                            ComprehensivePremium = parseValueToDecimal(factorsData.ComprehensivePremium),
                            CollisionPremium = parseValueToDecimal(factorsData.CollisionPremium),
                            VehicleLevelPremium = decimal.Round(vehicleLevelPremium, 2, MidpointRounding.AwayFromZero),
                            Premium = decimal.Round(totalPremium, 2, MidpointRounding.AwayFromZero),
                            WaiverOfSubrogationPremium = parseValueToDecimal(factorsData.WaiverOfSubrogationPremium),
                            HiredPhysicalDamage = parseValueToDecimal(factorsData.HiredPhysicalDamage),
                            TrailerInterchange = parseValueToDecimal(factorsData.TrailerInterchange),
                            ALManuscriptPremium = parseValueToDecimal(factorsData.ALManuscriptPremium),
                            PDManuscriptPremium = parseValueToDecimal(factorsData.PDManuscriptPremium),
                            TrailerInterchangeAL = parseValueToDecimal(factorsData.TrailerInterchangePremium),
                            NonOwnedTrailerPremium = parseValueToDecimal(factorsData.NonOwnedTrailerPremium),
                            OptionId = optionNumber,
                        };

                        if (binding.CreditedOfficeId == 3)
                        {
                            var arsCommissionAmount = (totalALPremium - (alManuscriptPremium ?? 0.00m)) * (parseValueToDecimal(existingRiskCoverageFromBind.ARSCommission) / 100m);
                            riskCov.ARSCommissionAmount = decimal.Round(arsCommissionAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            riskCov.ARSCommissionAmount = 0;
                        }

                        _context.RiskCoveragePremium.Add(riskCov);
                    }
                    else
                    {
                        decimal vehicleLevelPremium = factorsData.VehicleFactors.Sum(vehicles => vehicles.PerVehiclePremium) ?? 0.00m;
                        decimal totalPremium = parseValueToDecimal(factorsData.TotalCargoPremium) +
                                                parseValueToDecimal(factorsData.TotalLiabilityPremium) +
                                                parseValueToDecimal(factorsData.TotalPhysDamPremium) +
                                                parseValueToDecimal(factorsData.GeneralLiabilityPremium) +
                                                parseValueToDecimal(factorsData.AdditionalInsuredPremium) +
                                                parseValueToDecimal(factorsData.WaiverOfSubrogationPremium) +
                                                parseValueToDecimal(factorsData.PrimaryNonContributoryPremium) +
                                                parseValueToDecimal(factorsData.TrailerInterchangePremium) +
                                                parseValueToDecimal(factorsData.NonOwnedTrailerPremium);
                        decimal totalALPremium = parseValueToDecimal(factorsData.TotalLiabilityPremium) +
                                                 parseValueToDecimal(factorsData.AdditionalInsuredPremium) +
                                                 parseValueToDecimal(factorsData.WaiverOfSubrogationPremium) +
                                                 parseValueToDecimal(factorsData.PrimaryNonContributoryPremium);
                        //decimal fees = parseValueToDecimal(factorsData.RiskManagementFeeAL) + parseValueToDecimal(factorsData.RiskManagementFeeAPD);

                        existingRiskCoveragePremium.RiskCoverageId = factorsData.RiskCoverageId;
                        existingRiskCoveragePremium.AccountDriverFactor = parseValueToDecimal(factorsData.AccountDriverFactor);
                        existingRiskCoveragePremium.AdditionalInsuredPremium = parseValueToDecimal(factorsData.AdditionalInsuredPremium);
                        existingRiskCoveragePremium.AvgPerVehicle = (vehicleLevelPremium > 0 && factorsData.VehicleFactors.Count > 0) ? decimal.Round((vehicleLevelPremium / factorsData.VehicleFactors.Count), 2, MidpointRounding.AwayFromZero) : 0.00m;
                        existingRiskCoveragePremium.GeneralLiabilityPremium = parseValueToDecimal(factorsData.GeneralLiabilityPremium);
                        //existingRiskCoveragePremium.Fees = decimal.Round(fees, 2, MidpointRounding.AwayFromZero);
                        //existingRiskCoveragePremium.PremiumTaxesFees = parseValueToDecimal(factorsData.PremiumTaxesFees);
                        existingRiskCoveragePremium.PrimaryNonContributoryPremium = parseValueToDecimal(factorsData.PrimaryNonContributoryPremium);
                        //existingRiskCoveragePremium.RiskMgrFeeAL = parseValueToDecimal(factorsData.RiskManagementFeeAL);
                        //existingRiskCoveragePremium.RiskMgrFeePD = parseValueToDecimal(factorsData.RiskManagementFeeAPD);
                        //existingRiskCoveragePremium.SubTotalDue = parseValueToDecimal(factorsData.SubTotalDue);
                        //existingRiskCoveragePremium.TotalAnnualPremiumWithPremiumTax = parseValueToDecimal(factorsData.TotalAnnualPremiumWithPremiumTax);
                        existingRiskCoveragePremium.CargoPremium = parseValueToDecimal(factorsData.TotalCargoPremium);
                        existingRiskCoveragePremium.AutoLiabilityPremium = decimal.Round(totalALPremium, 2, MidpointRounding.AwayFromZero);
                        //existingRiskCoveragePremium.TotalOtherPolicyLevelPremium = parseValueToDecimal(factorsData.TotalOtherPolicyLevelPremium);
                        existingRiskCoveragePremium.PhysicalDamagePremium = parseValueToDecimal(factorsData.TotalPhysDamPremium);
                        existingRiskCoveragePremium.ComprehensivePremium = parseValueToDecimal(factorsData.ComprehensivePremium);
                        existingRiskCoveragePremium.CollisionPremium = parseValueToDecimal(factorsData.CollisionPremium);
                        existingRiskCoveragePremium.VehicleLevelPremium = decimal.Round(vehicleLevelPremium, 2, MidpointRounding.AwayFromZero);
                        existingRiskCoveragePremium.Premium = decimal.Round(totalPremium, 2, MidpointRounding.AwayFromZero);
                        existingRiskCoveragePremium.WaiverOfSubrogationPremium = parseValueToDecimal(factorsData.WaiverOfSubrogationPremium);
                        existingRiskCoveragePremium.HiredPhysicalDamage = parseValueToDecimal(factorsData.HiredPhysicalDamage);
                        existingRiskCoveragePremium.TrailerInterchange = parseValueToDecimal(factorsData.TrailerInterchange);
                        existingRiskCoveragePremium.ALManuscriptPremium = parseValueToDecimal(factorsData.ALManuscriptPremium);
                        existingRiskCoveragePremium.PDManuscriptPremium = parseValueToDecimal(factorsData.PDManuscriptPremium);
                        existingRiskCoveragePremium.TrailerInterchangeAL = parseValueToDecimal(factorsData.TrailerInterchangePremium);
                        existingRiskCoveragePremium.NonOwnedTrailerPremium = parseValueToDecimal(factorsData.NonOwnedTrailerPremium);
                        existingRiskCoveragePremium.OptionId = optionNumber;

                        if (binding.CreditedOfficeId == 3 )
                        {
                            var arsCommissionAmount = (totalALPremium - (alManuscriptPremium ?? 0.00m)) * (parseValueToDecimal(existingRiskCoverageFromBind.ARSCommission) / 100m);
                            existingRiskCoveragePremium.ARSCommissionAmount = decimal.Round(arsCommissionAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            existingRiskCoveragePremium.ARSCommissionAmount = 0;
                        }

                        _context.Update<RiskCoveragePremium>(existingRiskCoveragePremium);
                    }
                    _context.SaveChanges();
                    #endregion Risk Coverage Premium

                    //_context.Entry(existingRiskCoveragePremium).Reload();
                    

                    isFactorsSaved = true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException($"3. Error encountered while saving Rating Factors. {e.Message}\n{e.StackTrace}");
                }
                finally
                {

                    sw.Stop();
                    Console.WriteLine($"Saving rating premium & factors took {sw.Elapsed.TotalSeconds}s.");
                }
                transaction.Commit();
            });

            existingRiskCoveragePremium = await _context.RiskCoveragePremium.FirstOrDefaultAsync(x => x.RiskCoverageId == factorsData.RiskCoverageId);
            if (existingRiskCoveragePremium != null & risk.CurrentRiskDetailId == null)
            {
                await UpdateRiskCoverageFee((Guid)riskDTO.Id, riskDTO.RiskId, (int)optionNumber);
            }


            foreach (var vehicleFactorDTO in forEndorsementVehicleFactorDTOs)
            {
                var veh = _context.Vehicle.FirstOrDefault(o => o.Id.ToString() == vehicleFactorDTO.VehicleId);
                if (veh != null)
                    _context.Entry(veh).State = EntityState.Detached;
                veh.RadiusTerritory = vehicleFactorDTO.RadiusTerritory ?? 0.00m;
                veh.Radius = riskDTO.RaterVersionId > 3 ? veh.Radius : vehicleFactorDTO.Radius ?? 0.00m;
                var vehDTO = _mapper.Map<VehicleDTO>(veh);
                vehDTO.isEndorsement = true;
                await _vehicleService.UpdateAsync(vehDTO);
            }

            return isFactorsSaved;
        }

        private decimal parseValueToDecimal(string value)
        {
            return (string.IsNullOrEmpty(value) || value == "ERROR") ? 0.00m : Math.Round(Convert.ToDecimal(value), 2, MidpointRounding.AwayFromZero);
        }
        private async Task UpdateRiskCoverageFee(Guid riskDetailId,Guid RiskId, int optionNumber) {

            var risk = await _riskRepository.GetRiskByIdAsync(RiskId);

            var nameAddress = await _nameAndAddressService.GetNameAndAddress(riskDetailId);
            var feeDetails = _billingService.GetFeeDetailsByState(nameAddress?.State);

            var currentRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);

            var riskCoverages = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(riskDetailId);
            var riskCoverage = riskCoverages.Where(x => x.OptionNumber == optionNumber).FirstOrDefault();
            var riskCoveragePremium = riskCoverage.RiskCoveragePremium;

            var currentAutoLiabilityUnits = 0;
            var previousAutoLiabilityUnits = 0;
            var currentPhysicalDamageUnits = 0;
            var withDeletedVIN = risk.CurrentRiskDetailId == null ? false : true;
            RiskCoverage currentRiskCoverage = currentRiskDetail.RiskCoverages?.FirstOrDefault(x => x.OptionNumber == optionNumber);


            var allCurrentVehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(riskDetailId, withDeleted: withDeletedVIN);
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

            decimal RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == DTO.Billing.Fees.FeeDetails.MultipleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == DTO.Billing.Fees.FeeDetails.SingleALServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_AL_MIDTERM = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == DTO.Billing.Fees.FeeDetails.MidtermALServiceFee)?.Amount ?? 0;


            decimal RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == DTO.Billing.Fees.FeeDetails.MultiplePDServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == DTO.Billing.Fees.FeeDetails.SinglePDServiceFee)?.Amount ?? 0;
            decimal RISK_MANAGEMENT_FEE_PD_MIDTERM = feeDetails?.FirstOrDefault(feeDetail => feeDetail.Description == DTO.Billing.Fees.FeeDetails.MidtermPDServiceFee)?.Amount ?? 0;


            var billingFees = await _billingService.GetBillingFeesAsync(RiskId);

            decimal alServiceFees = 0;
            decimal pdServiceFees = 0;

            var hasSingleALPDServiceFee = feeDetails?.Any(feeDetail => feeDetail.Description == FeeDetails.SingleALServiceFee &&
                feeDetail.Description == FeeDetails.SinglePDServiceFee) ?? false;
            if (billingFees.Any() || !hasSingleALPDServiceFee)
            {
                alServiceFees = billingFees.Where(a => a.Description == "Service Fee - AL")?.Sum(a => a.Amount) ?? 0;
                pdServiceFees = billingFees.Where(a => a.Description == "Service Fee - PD")?.Sum(a => a.Amount) ?? 0;


                if (currentAutoLiabilityUnits > 0)
                {
                    if (currentAutoLiabilityUnits > 1) alServiceFees += (RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT * currentAutoLiabilityUnits);
                    else alServiceFees += RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT;

                }

                if (optionNumber != 1)
                {
                    var hasPD = (riskCoverage.CompDeductibleId != null &&
                        riskCoverage.CompDeductibleId != LimitConstants.ComprehensiveNoCoverageId)
                    || (riskCoverage.FireDeductibleId != null &&
                        riskCoverage.FireDeductibleId != LimitConstants.ComprehensiveNoCoverageId)
                    || (riskCoverage.CollDeductibleId != null &&
                        riskCoverage.CollDeductibleId != LimitConstants.CollisionNoCoverageId);

                    if (hasPD)
                        currentPhysicalDamageUnits = allCurrentVehicles.Count;
                    else
                        currentPhysicalDamageUnits = 0;
                }

                if (currentPhysicalDamageUnits > 0)
                {
                    if (currentPhysicalDamageUnits > 1) pdServiceFees += (RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT * currentPhysicalDamageUnits);
                    else pdServiceFees += RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT;

                }

                riskCoveragePremium.RiskMgrFeeAL = alServiceFees;
                riskCoveragePremium.RiskMgrFeePD = pdServiceFees;
                decimal fees = alServiceFees + pdServiceFees;
                riskCoveragePremium.Fees = decimal.Round(fees, 2, MidpointRounding.AwayFromZero);

                
            }

            riskCoverage.RiskCoveragePremium = riskCoveragePremium;
            _riskCoverageRepository.Update(riskCoverage);
            await _riskCoverageRepository.SaveChangesAsync();
            
        }


        private bool IsIncludedInOption(string options, int option)
        {
            return !string.IsNullOrEmpty(options) && options.Contains(option.ToString());
        }

        public async Task UpdateRatingFactors(Guid riskDetailId, RatingFactorsDTO ratingFactors)
        {
            var riskDetail = await _riskService.GetByIdIncludeAsync(riskDetailId);
            var riskCov = await _riskCoverageRepository.FindAsync(riskDetail.RiskCoverages.First().Id);

            riskCov.LiabilityScheduleRatingFactor = ratingFactors.ALScheduleRF;
            riskCov.ApdScheduleRF = ratingFactors.PDScheduleRF;
            riskCov.CargoScheduleRF = ratingFactors.CargoScheduleRF;
            riskCov.GlScheduleRF = ratingFactors.GLScheduleRF;

            await _riskCoverageRepository.SaveChangesAsync();
            await _changeComparerContext.CompareChangesForRatingFactors(riskDetail.RiskId);
        }

        public async Task<RatingFactorsDTO> GetRatingFactorsAsync(Guid riskDetailId)
        {
            var riskDetail = await _riskService.GetByIdIncludeAsync(riskDetailId);
            if (riskDetail is null)
                return null;
            var riskCov = await _riskCoverageRepository.FindAsync(riskDetail.RiskCoverages.First().Id);

            return new RatingFactorsDTO
            {
                ALScheduleRF = riskCov.LiabilityScheduleRatingFactor.Value,
                PDScheduleRF = riskCov.ApdScheduleRF.Value,
                CargoScheduleRF = riskCov.CargoScheduleRF.Value,
                GLScheduleRF = riskCov.GlScheduleRF.Value
            };
        }


        public async Task<dynamic> GetRaterVersionAsync(DateTime effectiveDate)
        {
            string url = $"api/RaterVersion/GetByPolicyEffectiveDate/{effectiveDate:yyyy-MM-dd}";

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var preloadResponse = await httpClient.GetAsync(url);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException($"{result}");
            }

            return JsonConvert.DeserializeObject(result);
        }

        public async Task<RaterVersionDTO> GetRaterVersionAsync(int versionId, Guid riskDetailId, bool isRaterVersionLocked)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var raterVersion = await GetLatestRaterVersionAsync();

            if(versionId < raterVersion.VersionId &&
                riskDetail.Status != "Quoted" && !(riskDetail.IsProcessed ?? false) && !isRaterVersionLocked)
            {
                await UpdateToLatestRaterVersionAsync(riskDetail.RiskId);
                versionId = raterVersion.VersionId;
            }

            string url = $"api/RaterVersion/{versionId}";

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var preloadResponse = await httpClient.GetAsync(url);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException(preloadResponse.ReasonPhrase);
            }

            return JsonConvert.DeserializeObject<RaterVersionDTO>(result);
        }

        public async Task LockRaterVersionAsync(Guid riskId)
        {
            try
            {
                var risk = await _riskRepository.FindAsync(riskId);
                risk.LockRaterVerison();
                _riskRepository.Update(risk);
                await _riskRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error locking rater version of risk {riskId}.");
            }
        }


        public async Task<bool> ShouldUpdateXpRaterAsync(Guid riskId)
        {
            try
            {
                var risk = await _riskRepository.FindAsync(riskId);
                return risk.UpdateXPRaterVersion;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in {nameof(ShouldUpdateXpRaterAsync)}.");
                throw;
            }
        }


        public async Task<RaterVersionDTO> GetLatestRaterVersionAsync()
        {
            using var httpClient = _httpClientFactory.CreateClient("rater");
            string url = $"api/RaterVersion/latest";
            var preloadResponse = await httpClient.GetAsync(url);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException($"{result}");
            }

            return JsonConvert.DeserializeObject<RaterVersionDTO>(result);
        }

        public async Task<string> InitializeRaterVersionAsync(Guid riskId)
        {
            var risk = await _riskRepository.GetByIdNoTrackingAsync(riskId);
            if (risk.RaterVersionId > 0)
                return risk.RaterVersionId.ToString();

            var latestRaterVersion = await GetLatestRaterVersionAsync();
            risk.RaterVersionId = latestRaterVersion.VersionId;
            _riskRepository.Update(risk);
            await _riskRepository.SaveChangesAsync();
            return risk.RaterVersionId.ToString();
        }

        public async Task<ExperienceRaterDTO> GetExperienceRaterDtoAsync(Guid riskId)
        {
            var risk = await _riskRepository.GetByIdNoTrackingAsync(riskId);

            using var httpClient = _httpClientFactory.CreateClient("rater");
            string url = $"api/ExperienceRater/{risk.SubmissionNumber}";
            var preloadResponse = await httpClient.GetAsync(url);

            var result = await preloadResponse.Content.ReadAsStringAsync();
            if (!preloadResponse.IsSuccessStatusCode)
            {
                if (preloadResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result = "Error occurred from rater server";
                }
                throw new RaterException($"{result}");
            }

            return JsonConvert.DeserializeObject<ExperienceRaterDTO>(result);
        }

        public async Task<JObject> UploadExperienceRaterAsync(Guid riskDetailId, string filename, byte[] content)
        {
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var risk = await _riskRepository.GetByIdNoTrackingAsync(riskDetail.RiskId);

            // Create bytes content of uploaded file to be added on form content.
            var bytesContent = new ByteArrayContent(content);
            // Create form content to be sent on http.
            var multiContent = new MultipartFormDataContent
            {
                { bytesContent, "file", filename }
            };

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var response = await httpClient.PostAsync($"api/ExperienceRater/upload/{risk.SubmissionNumber}?raterVersionId={risk.RaterVersionId}", multiContent);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new RaterException(error);
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            JObject uploadResults = JObject.Parse(responseContent);

            var experienceRatingFactor = uploadResults.SelectToken("data.ALExperienceRF")?.Value<decimal?>();
            var scheduleRatingFactor = uploadResults.SelectToken("data.ALScheduleRF")?.Value<decimal?>();

            if (experienceRatingFactor == null || scheduleRatingFactor == null)
            {
                return uploadResults;
            }

            var options = await GetOptions(riskDetailId);

            foreach (string option in options)
            {
                SaveRiskCoverageDTO saveRiskCoverage = new SaveRiskCoverageDTO()
                {
                    RiskDetailId = riskDetailId,
                    OptionNumber = short.Parse(option),
                    LiabilityExperienceRatingFactor = decimal.Round(experienceRatingFactor ?? 0, 4, MidpointRounding.AwayFromZero),
                    LiabilityScheduleRatingFactor = decimal.Round(scheduleRatingFactor ?? 0, 4, MidpointRounding.AwayFromZero)
                };
                await _coverageLimitService.UpdateRatingFactorAsync(saveRiskCoverage);
            }

            for (int year = 0; year <= 4; year++)
            {
                RiskHistoryDTO riskHistory = new RiskHistoryDTO()
                {
                    RiskDetailId = riskDetailId,
                    HistoryYear = year,
                    NumberOfPowerUnitsAL = decimal.Round(uploadResults.SelectToken($"data.HIC")[year]?.SelectToken("NumberOfPowerUnitsAL").Value<decimal?>() ?? 0, 2, MidpointRounding.AwayFromZero),
                    NumberOfPowerUnitsAPD = decimal.Round(uploadResults.SelectToken($"data.HIC")[year]?.SelectToken("NumberOfPowerUnitsAPD").Value<decimal?>() ?? 0, 2, MidpointRounding.AwayFromZero)
                };

                await _coverageHistoryService.UpdatePowerUnitFieldsAsync(riskHistory);
            }

            return uploadResults;
        }

        public async Task UpdateToLatestRaterVersionAsync(Guid riskId, Guid? riskDetailId = null)
        {
            try 
            {
                var risk = await _riskRepository.FindAsync(riskId);
                var raterVersion = await GetLatestRaterVersionAsync();
                risk.RaterVersionId = raterVersion.VersionId;

                //for mapping
                bool isRaterVersionSupported = risk.RaterVersionId >= 7 || risk.RaterVersionId == 0;

                if (riskDetailId != null && isRaterVersionSupported)
                {
                    var newRiskDetailId = (Guid)riskDetailId;
                    var newRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(newRiskDetailId);

                    List<dynamic> versionMapping = new List<dynamic>
                    {
                       new { oldVersionId = 8, V7Id = 145 , type = LvDatasetType.VehicleDescription },
                       new { oldVersionId = 107, V7Id = 152 , type = LvDatasetType.VehicleDescription },
                       new { oldVersionId = 1, V7Id = 5 , type = LvDatasetType.MainGVW },
                       new { oldVersionId = 2, V7Id = 6 , type = LvDatasetType.MainGVW },
                       new { oldVersionId = 3, V7Id = 7 , type = LvDatasetType.MainGVW },
                       new { oldVersionId = 4, V7Id = 8 , type = LvDatasetType.MainGVW }
                    };

                    var mainGVWMap = versionMapping.FirstOrDefault(x => x.type == LvDatasetType.MainGVW && x.oldVersionId == newRiskDetail.MainGVWId);
                    newRiskDetail.MainGVWId = (short?)mainGVWMap?.V7Id ?? newRiskDetail.MainGVWId;
                    _riskDetailRepository.Update(newRiskDetail);

                    var bindData = await _bindingService.GetBindingByRiskIdAsync(newRiskDetail.RiskId);
                    int bindOption = (bindData != null) ? bindData?.BindOptionId ?? 1 : 1;

                    var oldRiskDetailVehicles = newRiskDetail.RiskDetailVehicles
                           // Include only those selected in bind option.
                           .Where(rdv => rdv.Vehicle.Options != null && rdv.Vehicle.Options.Split(',').Any(opt => opt.Equals(bindOption.ToString())))
                           .Where(rdv => rdv.Vehicle.DeletedDate == null) // Exclude deleted
                           .OrderBy(rdv => rdv.Vehicle.VIN)
                           .ToList();

                    
                    foreach (var riskDetailVehicle in oldRiskDetailVehicles)
                    {
                        var vehicle = await _vehicleRepository.GetByRiskDetailAndMainIdAsync(newRiskDetail.Id, riskDetailVehicle.Vehicle.MainId);  // Get vehicle.

                        var vehDescMap = versionMapping.FirstOrDefault(x => x.type == LvDatasetType.VehicleDescription && x.oldVersionId == vehicle.VehicleDescriptionId);
                        vehicle.VehicleDescriptionId = (short?)vehDescMap?.V7Id ?? vehicle.VehicleDescriptionId;
                        _vehicleRepository.Update(vehicle);
                    }

                    await _riskRepository.SaveChangesAsync();
                    await _riskDetailRepository.SaveChangesAsync();
                    await _vehicleRepository.SaveChangesAsync();

                    // GL Limits update
                    var riskCov = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == newRiskDetailId && x.OptionNumber == bindOption);
                    if (riskCov != null)
                    {
                        // GL
                        switch (riskCov.GLBILimitId.Value)
                        {
                            case 87:
                                riskCov.GLBILimitId = 882; // V0-6 to V7 GL dataset
                                break;
                            case 88:
                                riskCov.GLBILimitId = 883; // V0-6 to V7 GL dataset
                                break;
                            case 89:
                                riskCov.GLBILimitId = 884; // V0-6 to V7 GL dataset
                                break;
                            case 90:
                                riskCov.GLBILimitId = 885; // V0-6 to V7 GL dataset
                                break;
                            case 91:
                                riskCov.GLBILimitId = 886; // V0-6 to V7 GL dataset
                                break;
                            case 92:
                                riskCov.GLBILimitId = 887; // V0-6 to V7 GL dataset
                                break;
                        }

                        // Liability
                        switch (riskCov.LiabilityDeductibleId.Value)
                        {
                            case 93:
                                riskCov.LiabilityDeductibleId = 888; // V0-7 to V8 Liab dataset
                                break;
                            case 94:
                                riskCov.LiabilityDeductibleId = 890; 
                                break;
                            case 95:
                                riskCov.LiabilityDeductibleId = 891; 
                                break;
                            case 96:
                                riskCov.LiabilityDeductibleId = 892; 
                                break;
                            case 100:
                                riskCov.LiabilityDeductibleId = 893; 
                                break;
                            case 102:
                                riskCov.LiabilityDeductibleId = 894;
                                break;
                            case 103:
                                riskCov.LiabilityDeductibleId = 895;
                                break;
                            case 105:
                                riskCov.LiabilityDeductibleId = 897;
                                break;
                            case 106:
                                riskCov.LiabilityDeductibleId = 898;
                                break;
                            case 107:
                                riskCov.LiabilityDeductibleId = 901;
                                break;
                            case 108:
                                riskCov.LiabilityDeductibleId = 902;
                                break;
                            case 97:
                            case 98:
                            case 99:
                            case 101:
                            case 104:
                            case 109:
                                riskCov.LiabilityDeductibleId = null;
                                break;
                        }

                        _riskCoverageRepository.Update(riskCov);
                        await _riskCoverageRepository.SaveChangesAsync();
                    }
                }
                else
                {
                    await _riskRepository.SaveChangesAsync();
                }

            }
            catch (Exception ex) 
            {
                await _applogService.Save($"{ex.Message}\n{ex.StackTrace}", null);
                throw;
            }
           
        }
    }
}
