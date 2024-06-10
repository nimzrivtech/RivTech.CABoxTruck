using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Fees;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.Billing.Application.DTO.Views;
using RivTech.CABoxTruck.WebService.DTO.Billing.Payments;
using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace RivTech.CABoxTruck.WebService.Service.Services.Billing
{
    public class BillingService : IBillingService
    {
        public IConfiguration Configuration { get; }
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IStateTaxRepository _stateTaxRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly IMapper _mapper;
        private readonly IBackgroundServiceChecker _backgroundServiceChecker;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfigurationSection _billingServiceSettings;
        private readonly string _billingBaseUrl;
        private readonly HttpClient _client;
        private readonly ILog4NetService _errorLogservice;
        private readonly IApplogService _applogService;
        private readonly ITaxDetailsRepository _taxDetailsRepository;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public BillingService(
            IConfiguration configuration,
            IHttpClientFactory factory,
            IRiskDetailRepository riskDetailRepository,
            IEntityAddressRepository entityAddressRepository,
            IBrokerInfoRepository brokerInfoRepository,
            IStateTaxRepository stateTaxRepository,
            ITaxDetailsRepository taxDetailsRepository,
            IRiskManuscriptsRepository riskManuscriptsRepository,
            IFeeRepository feeRepository,
            IVehicleRepository vehicleRepository,
            ILog4NetService errorLogservice,
            IApplogService applogService,
            IMapper mapper,
            IBackgroundServiceChecker backgroundServiceChecker,
            ITokenProvider tokenProvider
        )
        {
            Configuration = configuration;
            _riskDetailRepository = riskDetailRepository;
            _brokerInfoRepository = brokerInfoRepository;
            _stateTaxRepository = stateTaxRepository;
            _entityAddressRepository = entityAddressRepository;
            _taxDetailsRepository = taxDetailsRepository;
            _riskManuscriptsRepository = riskManuscriptsRepository;
            _feeRepository = feeRepository;
            _vehicleRepository = vehicleRepository;
            _errorLogservice = errorLogservice;
            _applogService = applogService;
            _mapper = mapper;
            _backgroundServiceChecker = backgroundServiceChecker;
            _tokenProvider = tokenProvider;
            _client = factory.CreateClient("billingClient");
            _billingServiceSettings = Configuration.GetSection("BillingService");
            _billingBaseUrl = _billingServiceSettings.GetValue<string>("BaseUrl");
        }

        public async Task UpdatePaymentOptions(Guid riskDetailId, int? optionId = null)
        {
            var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(riskDetail.RiskId);
            if (!brokerInfo.EffectiveDate.HasValue)
                throw new InvalidOperationException("Cannot calculate tax without effective date.");

            var businessAddress = await _entityAddressRepository
                    .FindAsync(x => x.AddressTypeId == (short)AddressTypesEnum.Business
                    && x.EntityId == riskDetail.Insured.EntityId, i => i.Address);
            StateTax stateTax = await _stateTaxRepository.GetAsync(businessAddress.Address.State, brokerInfo.EffectiveDate.Value);

            var nonTaxableFees = _feeRepository.GetNonTaxableTypesByState(businessAddress.Address.State, brokerInfo.EffectiveDate.Value);
            bool hasTaxableInstallmentFee = !nonTaxableFees.Any(ntf => ntf.Id.Equals(FeeKind.InstallmentFee.Id));
            bool hasTaxableRiskManagementFeeAL = !nonTaxableFees.Any(ntf => ntf.Id.Equals(FeeKind.RiskManagementFeeAL.Id));
            bool hasTaxableRiskManagementFeePD = !nonTaxableFees.Any(ntf => ntf.Id.Equals(FeeKind.RiskManagementFeePD.Id));
            decimal taxRate = hasTaxableInstallmentFee ? stateTax.TotalRate : 0;
            decimal flatTaxRate = (stateTax.StampingFeeTaxSubType == StampingFeeTaxType.Flat.Id && taxRate > 0) ? stateTax.StampingFeeTax.Rate : 0m;

            var quoteOptions = riskDetail.RiskCoverages.Where(riskcoverages => riskcoverages.RiskCoveragePremium != null);
            foreach (var riskCov in quoteOptions)
            {
                if (optionId.HasValue && riskCov.OptionNumber != optionId)
                    continue;
                var riskCovPrem = riskCov.RiskCoveragePremium;
                if (riskCovPrem.Premium is null)
                    continue;
                //throw new InvalidOperationException("Total Premium is required to calculate payment option.");

                if (riskCov.DepositPct == 100)
                    riskCov.NumberOfInstallments = null;
                if (riskCov.NumberOfInstallments is null && riskCov.DepositPct != 100)
                    riskCov.NumberOfInstallments = 10;

                riskCovPrem.Fees = riskCovPrem.RiskMgrFeeAL + riskCovPrem.RiskMgrFeePD; // always exclude installment fee tax

                riskCovPrem.TaxesAndStampingFees = RecalculateTaxesAndStampingFee(riskCovPrem, taxRate, flatTaxRate);

                var policyTotals = new PolicyTotals(riskCovPrem.Premium.Value, riskCovPrem.Fees ?? 0, riskCovPrem.TaxesAndStampingFees ?? 0);
                riskCovPrem.TotalAmounDueFull = policyTotals.Total;

                //riskCov.RecalculateDepositPremium();
                if (!riskCov.IsDepositPremiumUpdated())
                    riskCov.RecalculateDepositPremium();
                var paymentOptionsDetails = new PaymentOptionDetails(
                    policyTotals, 
                    riskCov.DepositPremium.Value, 
                    taxRate, 
                    riskCov.NumberOfInstallments ?? 0,
                    flatTaxRate);
                riskCovPrem.DepositAmount = paymentOptionsDetails.DepositAmount;
                riskCovPrem.TotalAmountDueInstallment = paymentOptionsDetails.TotalWithInstallments;

                if (paymentOptionsDetails.InstallmentDetails != null)
                {
                    var installmentDetails = paymentOptionsDetails.InstallmentDetails;
                    riskCovPrem.InstallmentFee = installmentDetails.Fee;
                    riskCovPrem.PerInstallment = installmentDetails.PerInstallmentDue;
                    riskCovPrem.PerInstallmentPremium = installmentDetails.Premium;
                    riskCovPrem.PerInstallmentFee = installmentDetails.Fee;
                    riskCovPrem.PerInstallmentTax = installmentDetails.Tax;
                    riskCovPrem.TotalInstallmentFee = installmentDetails.TotalInstallmentFee;
                    // include total installment fees in Fees
                    riskCovPrem.Fees = installmentDetails.TotalInstallmentFee + riskCovPrem.RiskMgrFeeAL + riskCovPrem.RiskMgrFeePD;
                    riskCovPrem.TaxesAndStampingFees = RecalculateTaxesAndStampingFee(riskCovPrem, taxRate, flatTaxRate);
                }
                else
                {
                    riskCovPrem.InstallmentFee = 0;
                    riskCovPrem.PerInstallment = 0;
                    riskCovPrem.PerInstallmentPremium = 0;
                    riskCovPrem.PerInstallmentFee = 0;
                    riskCovPrem.PerInstallmentTax = 0;
                    riskCovPrem.TotalInstallmentFee = 0;
                }
            }

            _riskDetailRepository.Update(riskDetail);
            await _riskDetailRepository.SaveChangesAsync();
        }

        public List<FeeDetails> GetFeeDetailsByState(string stateCode)
        {
            var result = _feeRepository.GetFeeDetailsByState(stateCode);
            return result;
        }

        private decimal RecalculateTaxesAndStampingFee(RiskCoveragePremium riskCoveragePremium, decimal taxRate, decimal flatTaxRate)
        {
            if (taxRate > 0)
            {
                return (((riskCoveragePremium.Premium ?? 0) + (riskCoveragePremium.Fees ?? 0)) * (taxRate / 100) + flatTaxRate).RoundTo2Decimals();
            }

            return riskCoveragePremium?.TaxesAndStampingFees ?? 0;
        }

        public BillingBindResponseDTO ProcessBillingBind(
            string policyNumber,
            DateTime effectiveDate,
            RiskDetail riskDetail,
            Binding bindingDetails,
            RiskCoverage riskCoverage,
            List<Vehicle> vehicles,
            string authorizeId = null)
        {
            try
            {
                var billingBindData = GetBillingBindData(policyNumber, effectiveDate, riskDetail, bindingDetails, riskCoverage, vehicles);
                billingBindData.TransactionId = authorizeId;

                return ExecuteBillingBind(riskDetail.RiskId, billingBindData).Result;
            }
            catch (BillingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BillingBindResponseDTO> ProcessBillingBindEndorsement(
            Guid riskId,
            DateTime effectiveDate,
            List<EndorsementPendingChange> pendingChange,
            DateTime? policyEffectiveDate,
            string authorizeId = null)
        {
            try
            {
                BillingEndorsementDTO billingEndorsementData = new BillingEndorsementDTO
                {
                    RiskId = riskId,
                    EndorsementEffectiveDate = effectiveDate,
                    PolicyEffectiveDate = policyEffectiveDate,
                };

                billingEndorsementData.TransactionDetails = GetTransactionDetailsEndorsement(pendingChange).ToArray();

                if (billingEndorsementData.TransactionDetails.Count() > 0)
                {
                    return await ExecuteBillingEndorsement(riskId, billingEndorsementData);
                }

                return null;
            }
            catch (BillingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BillingCancellationDTO> ProcessBillingCancellation(Guid riskId, DateTime effectiveDate, IEnumerable<CancellationBreakdown> breakdowns, bool isFlatCancel)
        {
            try
            {
                BillingCancellationDTO billingCancellationData = new BillingCancellationDTO
                {
                    RiskId = riskId,
                    IsFlatCancel = isFlatCancel,
                    EndorsementEffectiveDate = effectiveDate
                };

                billingCancellationData.TransactionDetails = GetTransactionDetailsCancellation(breakdowns).ToArray();

                if (billingCancellationData.TransactionDetails.Count() > 0)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(billingCancellationData));
                    return await ExecuteBillingCancellation(riskId, billingCancellationData);
                }
                return null;
            }
            catch (BillingException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex.Message, nameof(ProcessBillingCancellation));
                throw;
            }
        }

        private IEnumerable<TransactionDetailDTO> GetTransactionDetailsCancellation(IEnumerable<CancellationBreakdown> breakdowns)
        {
            var transactionDetails = new List<TransactionDetailDTO>();

            var serviceFeeALs = breakdowns.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeeAL);
            var serviceFeePDs = breakdowns.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeePD);

            breakdowns = breakdowns.Where(x => !serviceFeeALs.Contains(x) && !serviceFeePDs.Contains(x));
            bool hasMinPremiumAdjustment = breakdowns.Sum(x => x.MinimumPremiumAdjustment) > 0; // upon cancellation total min premium adj should be greater than 0

            #region Combine CompFT and Coll amount items into PD 

            // Get compft and colls item in breakdown.
            var breakdownCompCollFts = breakdowns.Where(x => 
                x.AmountSubTypeId == BillingConstants.VehiclePDPCompFt
                || x.AmountSubTypeId == BillingConstants.VehiclePDPColl);

            // Remove compft and colls item in breakdown.
            breakdowns = breakdowns.Where(x => !breakdownCompCollFts.Contains(x));

            // Group compft and colls by vin then create a new breakdown with sum of each amounts.
            var groupedPDs = breakdownCompCollFts.GroupBy(x => x.Description);
            var pdBreakdowns = new List<CancellationBreakdown>();
            foreach(var pdBreakdown in groupedPDs)
            {
                pdBreakdowns.Add(new CancellationBreakdown
                {
                    AmountSubTypeId = BillingConstants.VehiclePDP,
                    RiskDetailId = breakdowns.First().RiskDetailId,
                    Description = pdBreakdown.Key,
                    ActiveAmount = pdBreakdown.Sum(x => x.ActiveAmount),
                    CancelledAmount = pdBreakdown.Sum(x => x.CancelledAmount),
                    AmountReduction = pdBreakdown.Sum(x => x.AmountReduction),
                    AnnualMinimumPremium = pdBreakdown.Sum(x => x.AnnualMinimumPremium),
                    MinimumPremium = pdBreakdown.Sum(x => x.MinimumPremium),
                    MinimumPremiumAdjustment = pdBreakdown.Sum(x => x.MinimumPremiumAdjustment),
                    ShortRatePremium = pdBreakdown.Sum(x => x.ShortRatePremium),
                });
            }

            // add create PD breakdowns back to overall breakdown.
            breakdowns = breakdowns.Concat(pdBreakdowns);

            #endregion

            foreach (var breakdown in breakdowns)
            {
                if (breakdown.AmountReduction != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        Amount = Math.Round(breakdown.AmountReduction, 2, MidpointRounding.AwayFromZero),
                        AmountSubType = new AmountSubTypeDTO { Id = breakdown.AmountSubTypeId },
                        Description = breakdown.Description
                    });
                }

                if (breakdown.MinimumPremiumAdjustment != 0 && hasMinPremiumAdjustment)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        Amount = Math.Round(breakdown.MinimumPremiumAdjustment, 2, MidpointRounding.AwayFromZero),
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GetPremiumMPA(breakdown.AmountSubTypeId) },
                        Description = breakdown.Description
                    });
                }

                if (breakdown.ShortRatePremium > 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        Amount = Math.Round(breakdown.ShortRatePremium, 2, MidpointRounding.AwayFromZero),
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GetPremiumSRP(breakdown.AmountSubTypeId) },
                        Description = breakdown.Description
                    });
                }
            }

            if (serviceFeeALs.Any())
            {
                transactionDetails.Add(new TransactionDetailDTO
                {
                    Amount = Math.Round(serviceFeeALs.Sum(x => x.AmountReduction), 2, MidpointRounding.AwayFromZero),
                    AmountSubType = new AmountSubTypeDTO { Id = serviceFeeALs.First().AmountSubTypeId },
                    Description = serviceFeeALs.First().Description
                });
            }

            if (serviceFeePDs.Any())
            {
                transactionDetails.Add(new TransactionDetailDTO
                {
                    Amount = Math.Round(serviceFeePDs.Sum(x => x.AmountReduction), 2, MidpointRounding.AwayFromZero),
                    AmountSubType = new AmountSubTypeDTO { Id = serviceFeePDs.First().AmountSubTypeId },
                    Description = serviceFeeALs.First().Description
                });
            }

            return transactionDetails;
        }

        private IEnumerable<TransactionDetailDTO> GetTransactionDetailsReinstatement(IEnumerable<ReinstatementBreakdown> breakdowns)
        {
            var transactionDetails = new List<TransactionDetailDTO>();

            var serviceFeeALs = breakdowns.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeeAL);
            var serviceFeePDs = breakdowns.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeePD);

            breakdowns = breakdowns.Where(x => !serviceFeeALs.Contains(x) && !serviceFeePDs.Contains(x));
            bool hasMinPremiumAdjustment = breakdowns.Sum(x => x.MinimumPremiumAdjustment) < 0; // upon reinstatement total min premium adj to be reversed should be less than 0

            #region Combine CompFT and Coll amount items into PD 

            // Get compft and colls item in breakdown.
            var breakdownCompCollFts = breakdowns.Where(x =>
                x.AmountSubTypeId == BillingConstants.VehiclePDPCompFt
                || x.AmountSubTypeId == BillingConstants.VehiclePDPColl);

            // Remove compft and colls item in breakdown.
            breakdowns = breakdowns.Where(x => !breakdownCompCollFts.Contains(x));

            // Group compft and colls by vin then create a new breakdown with sum of each amounts.
            var groupedPDs = breakdownCompCollFts.GroupBy(x => x.Description);
            var pdBreakdowns = new List<ReinstatementBreakdown>();
            foreach (var pdBreakdown in groupedPDs)
            {
                pdBreakdowns.Add(new ReinstatementBreakdown
                {
                    AmountSubTypeId = BillingConstants.VehiclePDP,
                    RiskDetailId = breakdowns.First().RiskDetailId,
                    Description = pdBreakdown.Key,
                    Amount = pdBreakdown.Sum(x => x.Amount),
                    MinimumPremiumAdjustment = pdBreakdown.Sum(x => x.MinimumPremiumAdjustment),
                    ShortRatePremium = pdBreakdown.Sum(x => x.ShortRatePremium),
                });
            }

            // add create PD breakdowns back to overall breakdown.
            breakdowns = breakdowns.Concat(pdBreakdowns);

            #endregion

            foreach (var breakdown in breakdowns)
            {
                if (breakdown.Amount != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        Amount = Math.Round(breakdown.Amount, 2, MidpointRounding.AwayFromZero),
                        AmountSubType = new AmountSubTypeDTO { Id = breakdown.AmountSubTypeId },
                        Description = breakdown.Description
                    });
                }

                if (breakdown.MinimumPremiumAdjustment != 0 && hasMinPremiumAdjustment)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        Amount = Math.Round(breakdown.MinimumPremiumAdjustment, 2, MidpointRounding.AwayFromZero),
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GetPremiumMPA(breakdown.AmountSubTypeId) },
                        Description = breakdown.Description
                    });
                }

                if (breakdown.ShortRatePremium != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        Amount = Math.Round(breakdown.ShortRatePremium, 2, MidpointRounding.AwayFromZero),
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GetPremiumSRP(breakdown.AmountSubTypeId) },
                        Description = breakdown.Description
                    });
                }
            }

            if (serviceFeeALs.Any())
            {
                transactionDetails.Add(new TransactionDetailDTO
                {
                    Amount = Math.Round(serviceFeeALs.Sum(x => x.Amount), 2, MidpointRounding.AwayFromZero),
                    AmountSubType = new AmountSubTypeDTO { Id = serviceFeeALs.First().AmountSubTypeId },
                    Description = serviceFeeALs.First().Description
                });
            }

            if (serviceFeePDs.Any())
            {
                transactionDetails.Add(new TransactionDetailDTO
                {
                    Amount = Math.Round(serviceFeePDs.Sum(x => x.Amount), 2, MidpointRounding.AwayFromZero),
                    AmountSubType = new AmountSubTypeDTO { Id = serviceFeePDs.First().AmountSubTypeId },
                    Description = serviceFeeALs.First().Description
                });
            }

            return transactionDetails;
        }
        private async Task<BillingCancellationDTO> ExecuteBillingCancellation(Guid riskId, BillingCancellationDTO billingCancellationData)
        {
            try
            {
                billingCancellationData.AppId = _billingServiceSettings.GetValue<string>("AppId");
                var temp = JsonConvert.SerializeObject(billingCancellationData);

                await _applogService.Save($"BillingEndo:{riskId}", temp);

                string stringContent = JsonConvert.SerializeObject(billingCancellationData);
                Console.WriteLine(stringContent);
                using HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/PolicyCancellation", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                return JsonTool.DeserializeObject<BillingCancellationDTO>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task ProcessBillingReinstatement(Guid riskId, IEnumerable<ReinstatementBreakdown> breakdowns, DateTime? policyEffectiveDate, bool isFlatCancel)
        {
            try
            {
                BillingReinstatementDTO billingReinstatementData = new BillingReinstatementDTO
                {
                    RiskId = riskId,
                    PolicyEffectiveDate = policyEffectiveDate,
                    IsFlatCancel = isFlatCancel
                };

                billingReinstatementData.TransactionDetails = GetTransactionDetailsReinstatement(breakdowns).ToArray();

                if (billingReinstatementData.TransactionDetails.Count() > 0)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(billingReinstatementData));
                    await ExecuteBillingReinstatement(riskId, billingReinstatementData);
                }
            }
            catch (BillingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex.Message, nameof(ProcessBillingCancellation));
                throw;
            }
        }

        private async Task<BillingCancellationDTO> ExecuteBillingReinstatement(Guid riskId, BillingReinstatementDTO billingReinstatementDTO)
        {
            try
            {
                billingReinstatementDTO.AppId = _billingServiceSettings.GetValue<string>("AppId");
                var temp = JsonConvert.SerializeObject(billingReinstatementDTO);

                await _applogService.Save($"BillingEndo:{riskId}", temp);

                string stringContent = JsonConvert.SerializeObject(billingReinstatementDTO);
                using HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/Reinstatement/reinstate", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse?.Message}");
                }

                return JsonTool.DeserializeObject<BillingCancellationDTO>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> AuthorizePayment(string policyNumber, DateTime effectiveDate, RiskDetail riskDetail, Binding bindingDetails, RiskCoverage riskCoverage, List<Vehicle> vehicles)
        {
            try
            {
                var billingBindData = GetBillingBindData(policyNumber, effectiveDate, riskDetail, bindingDetails, riskCoverage, vehicles);
                return await ExecuteAuthorizePayment(riskDetail.Id, billingBindData);
            }
            catch (BillingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> ExecuteAuthorizePayment(Guid riskDetailId, BillingBindDTO billingBindData)
        {
            try
            {
                billingBindData.AppId = _billingServiceSettings.GetValue<string>("AppId");

                using (HttpContent content = new StringContent(JsonConvert.SerializeObject(billingBindData), Encoding.UTF8, "application/json"))
                {
                    await AddAuthHeaderToken(content);

                    var response = await _client.PostAsync($"{_billingBaseUrl}/payment/authorize/{riskDetailId}", content);

                    var strResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(strResponse);
                        throw new Exception($"{objFailedResponse.Message}");
                    }

                    return strResponse;
                }
            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
                throw new BillingException(ex.Message);
            }
        }

        private async Task AddAuthHeaderToken(HttpContent httpContent)
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var token = Data.Common.Services.GetAuthToken();
            if (_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }

            if (!String.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (Data.Common.Services.GetUserDate().HasValue)
                httpContent.Headers.Add("userdate", Data.Common.Services.GetUserDate().Value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        private async Task AddAuthHeaderToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var token = Data.Common.Services.GetAuthToken();
            if (_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }

            if (!String.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private BillingBindDTO GetBillingBindData(
            string policyNumber,
            DateTime effectiveDate,
            RiskDetail riskDetail,
            Binding bindingDetails,
            RiskCoverage riskCoverage,
            List<Vehicle> vehicles)
        {
            string paymentMethod = null;

            var numberOfInstallments = riskCoverage.NumberOfInstallments;
            string payPlanOption = bindingDetails.PaymentTypesId.HasValue ? ConvertToBillingPayplanOption(bindingDetails.PaymentTypesId.Value, numberOfInstallments) : null;

            BillingBindDTO billingBindData = new BillingBindDTO
            {
                EffectiveDate = effectiveDate,
                InstrumentId = paymentMethod,
                paymentPlan = payPlanOption,
                PolicyNumber = policyNumber,
                BillingDetail = new BillingDetailDTO()
            };

            billingBindData.WillPayLater = true; // Temporary
            billingBindData.Email = riskDetail.Insured.Entity.WorkEmailAddress;
            billingBindData.PercentOfOutstanding = Percentage(riskCoverage.DepositPct.Value);
            billingBindData.TotalAmountCharged = decimal.Round(riskCoverage.RiskCoveragePremium.TotalAmounDueFull.Value, 2, MidpointRounding.AwayFromZero);
            billingBindData.alpCommission = Percentage(Convert.ToDecimal(riskCoverage.BrokerCommisionAL));
            billingBindData.pdpCommission = Percentage(Convert.ToDecimal(riskCoverage.BrokerCommisionPD));
            billingBindData.ARSCommission = !string.IsNullOrEmpty(riskCoverage.ARSCommission) ? Percentage(Convert.ToDecimal(riskCoverage.ARSCommission)) : 0;
            billingBindData.StateCode = riskDetail.Insured.Entity.EntityAddresses.Find(x => x.AddressTypeId == null).Address.State;

            var taxDetails = _taxDetailsRepository.FindAsync(x => x.Id == riskCoverage.RiskCoveragePremium.TaxDetailsId.Value).Result;
            var totalManuscriptAL = 0m;
            var totalManuscriptPD = 0m;
            var manuscriptAL = _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetail.Id && x.IsSelected.Value == true && x.IsActive && x.PremiumType == "AL").Result;
            if (manuscriptAL != null)
                totalManuscriptAL = manuscriptAL.ToList().Sum(x => x.Premium);
            var manuscriptPD = _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == riskDetail.Id && x.IsSelected.Value == true && x.IsActive && x.PremiumType == "PD").Result;
            if (manuscriptPD != null)
                totalManuscriptPD = manuscriptPD.ToList().Sum(x => x.Premium);

            billingBindData.transactionDetails = GetTransactionDetails(vehicles, taxDetails, riskCoverage, totalManuscriptAL, totalManuscriptPD).ToArray();

            return billingBindData;
        }

        private async Task<BillingBindResponseDTO> ExecuteBillingBind(Guid riskId, BillingBindDTO billingBindData)
        {
            try
            {
                billingBindData.AppId = _billingServiceSettings.GetValue<string>("AppId");
                var temp = JsonConvert.SerializeObject(billingBindData);

                await _applogService.Save($"BillingBind:{riskId}", temp);

                using (HttpContent content = new StringContent(JsonConvert.SerializeObject(billingBindData), Encoding.UTF8, "application/json"))
                {
                    await AddAuthHeaderToken(content);
                    var response = await _client.PostAsync($"{_billingBaseUrl}/Billing/bind/{riskId}", content);

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                        throw new Exception($"{objFailedResponse.Message}");
                    }

                    return JsonTool.DeserializeObject<BillingBindResponseDTO>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        private async Task<BillingBindResponseDTO> ExecuteBillingEndorsement(Guid riskId, BillingEndorsementDTO billingEndorsementData)
        {
            try
            {
                billingEndorsementData.AppId = _billingServiceSettings.GetValue<string>("AppId");
                var temp = JsonConvert.SerializeObject(billingEndorsementData);

                await _applogService.Save($"BillingEndo:{riskId}", temp);

                using (HttpContent content = new StringContent(JsonConvert.SerializeObject(billingEndorsementData), Encoding.UTF8, "application/json"))
                {
                    await AddAuthHeaderToken(content);
                    var response = await _client.PostAsync($"{_billingBaseUrl}/Endorsement", content);

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                        throw new Exception($"{objFailedResponse.Message}");
                    }

                    return JsonTool.DeserializeObject<BillingBindResponseDTO>(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        private List<TransactionDetailDTO> GetTransactionDetails(
            List<Vehicle> vehicles,
            Data.Entity.Billing.TaxDetailsData taxDetails,
            RiskCoverage riskCoverage,
            decimal totalManuscriptAL,
            decimal totalManuscriptPD)
        {
            List<TransactionDetailDTO> transactionDetails = new List<TransactionDetailDTO>();

            foreach (var vehicle in vehicles)
            {
                transactionDetails.Add(new TransactionDetailDTO
                {
                    AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleALP },
                    Amount = decimal.Round(vehicle.AutoLiabilityPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                    Description = vehicle.VIN
                });

                transactionDetails.Add(new TransactionDetailDTO
                {
                    AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehicleCP },
                    Amount = decimal.Round(vehicle.CargoPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                    Description = vehicle.VIN
                });

                transactionDetails.Add(new TransactionDetailDTO
                {
                    AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.VehiclePDP },
                    Amount = decimal.Round(vehicle.PhysicalDamagePremium ?? 0, 2, MidpointRounding.AwayFromZero),
                    Description = vehicle.VIN
                });
            }

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeeAL },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.RiskMgrFeeAL ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Service Fee - AL"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeePD },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.RiskMgrFeePD ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Service Fee - PD"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GeneralLiabilityPremium },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.GeneralLiabilityPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "General Liability"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangePremium },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.TrailerInterchangeAL ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Trailer Interchange"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.NonOwnedTrailerPremium },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.NonOwnedTrailerPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Non-Owned Trailer"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.AdditionalInsured },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.AdditionalInsuredPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Additional Insured"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.WaiverOfSubrogation },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.WaiverOfSubrogationPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Waiver Of Subrogation"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.PrimaryAndNoncontributory },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.PrimaryNonContributoryPremium ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Primary And Noncontributory"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangeEndst },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.TrailerInterchange ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Trailer Interchange Endst"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.HiredPhysicalDamageEndst },
                Amount = decimal.Round(riskCoverage.RiskCoveragePremium.HiredPhysicalDamage ?? 0, 2, MidpointRounding.AwayFromZero),
                Description = "Hired Physical Damage"
            });

            //transactionDetails.Add(new TransactionDetailDTO
            //{
            //    AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.InstallmentFee },
            //    Amount = decimal.Round(riskCoverage.RiskCoveragePremium.InstallmentFee.Value, 2, MidpointRounding.AwayFromZero)
            //});

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.SurplusLinesTax },
                Amount = decimal.Round((taxDetails?.TotalSLTax ?? 0), 2, MidpointRounding.AwayFromZero),
                Description = "Surplus Lines Tax"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.StampingFee },
                Amount = decimal.Round((taxDetails?.TotalStampingFee ?? 0), 2, MidpointRounding.AwayFromZero),
                Description = "Stamping Fee"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ALManuscriptPremium },
                Amount = decimal.Round(totalManuscriptAL, 2, MidpointRounding.AwayFromZero),
                Description = "AL Manuscript"
            });

            transactionDetails.Add(new TransactionDetailDTO
            {
                AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.PDManuscriptPremium },
                Amount = decimal.Round(totalManuscriptPD, 2, MidpointRounding.AwayFromZero),
                Description = "PD Manuscript"
            });

            return transactionDetails;
        }

        private List<TransactionDetailDTO> GetTransactionDetailsEndorsement(List<EndorsementPendingChange> pendingChange)
        {
            List<TransactionDetailDTO> transactionDetails = new List<TransactionDetailDTO>();

            var vehicles = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.VehicleALP ||
                                                    x.AmountSubTypeId == BillingConstants.VehicleCP ||
                                                    x.AmountSubTypeId == BillingConstants.VehiclePDP).ToList();
            foreach (var vehicle in vehicles)
            {
                var vehiclePremDiff = vehicle.PremiumDifference ?? 0;
                if (vehiclePremDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = vehicle.AmountSubTypeId },
                        Amount = decimal.Round(vehiclePremDiff, 2, MidpointRounding.AwayFromZero),
                        Description = vehicle.Description
                    });
                }
            }

            var glPrem = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.GeneralLiabilityPremium).FirstOrDefault();
            if (glPrem != null)
            {
                var glPremDiff = glPrem.PremiumDifference ?? 0;
                if (glPremDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.GeneralLiabilityPremium },
                        Amount = decimal.Round(glPremDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "General Liability"
                    });
                }
            }

            var TrailerInterchangePrem = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.TrailerInterchangePremium).FirstOrDefault();
            if (TrailerInterchangePrem != null)
            {
                var TrailerInterchangePremDiff = TrailerInterchangePrem.PremiumDifference ?? 0;
                if (TrailerInterchangePremDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangePremium },
                        Amount = decimal.Round(TrailerInterchangePremDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Trailer Interchange"
                    });
                }
            }

            var NonOwnedTrailerPrem = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.NonOwnedTrailerPremium).FirstOrDefault();
            if (NonOwnedTrailerPrem != null)
            {
                var NonOwnedTrailerPremDiff = NonOwnedTrailerPrem.PremiumDifference ?? 0;
                if (NonOwnedTrailerPremDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.NonOwnedTrailerPremium },
                        Amount = decimal.Round(NonOwnedTrailerPremDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Non-Owned Trailer"
                    });
                }
            }


            var addtnlInsured = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.AdditionalInsured).FirstOrDefault();
            if (addtnlInsured != null)
            {
                var addtnlInsuredDiff = addtnlInsured.PremiumDifference ?? 0;
                if (addtnlInsuredDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.AdditionalInsured },
                        Amount = decimal.Round(addtnlInsuredDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Additional Insured"
                    });
                }
            }

            var waiverOfSubrogation = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.WaiverOfSubrogation).FirstOrDefault();
            if (waiverOfSubrogation != null)
            {
                var waiverOfSubrogationDiff = waiverOfSubrogation.PremiumDifference ?? 0;
                if (waiverOfSubrogationDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.WaiverOfSubrogation },
                        Amount = decimal.Round(waiverOfSubrogationDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Waiver Of Subrogation"
                    });
                }
            }

            var primaryAndNoncontributory = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.PrimaryAndNoncontributory).FirstOrDefault();
            if (primaryAndNoncontributory != null)
            {
                var primaryAndNoncontributoryDiff = primaryAndNoncontributory.PremiumDifference ?? 0;
                if (primaryAndNoncontributoryDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.PrimaryAndNoncontributory },
                        Amount = decimal.Round(primaryAndNoncontributoryDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Primary And Noncontributory"
                    });
                }
            }

            var trailerInterchangeEndst = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.TrailerInterchangeEndst).FirstOrDefault();
            if (trailerInterchangeEndst != null)
            {
                var trailerInterchangeEndstDiff = trailerInterchangeEndst.PremiumDifference ?? 0;
                if (trailerInterchangeEndstDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.TrailerInterchangeEndst },
                        Amount = decimal.Round(trailerInterchangeEndstDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Trailer Interchange Endst"
                    });
                }
            }

            var hiredPhysicalDamageEndst = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.HiredPhysicalDamageEndst).FirstOrDefault();
            if (hiredPhysicalDamageEndst != null)
            {
                var hiredPhysicalDamageEndstDiff = hiredPhysicalDamageEndst.PremiumDifference ?? 0;
                if (hiredPhysicalDamageEndstDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.HiredPhysicalDamageEndst },
                        Amount = decimal.Round(hiredPhysicalDamageEndstDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Hired Physical Damage"
                    });
                }
            }

            var aLManuscriptPremium = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.ALManuscriptPremium).FirstOrDefault();
            if (aLManuscriptPremium != null)
            {
                var aLManuscriptPremiumDiff = aLManuscriptPremium.PremiumDifference ?? 0;
                if (aLManuscriptPremiumDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ALManuscriptPremium },
                        Amount = decimal.Round(aLManuscriptPremiumDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "AL Manuscript"
                    });
                }
            }

            var pDManuscriptPremium = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.PDManuscriptPremium).FirstOrDefault();
            if (pDManuscriptPremium != null)
            {
                var pDManuscriptPremiumDiff = pDManuscriptPremium.PremiumDifference ?? 0;
                if (pDManuscriptPremiumDiff != 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.PDManuscriptPremium },
                        Amount = decimal.Round(pDManuscriptPremiumDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "PD Manuscript"
                    });
                }
            }

            var serviceFeeAL = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeeAL).FirstOrDefault();
            if (serviceFeeAL != null)
            {
                var serviceFeeALDiff = serviceFeeAL.PremiumDifference ?? 0;
                if (serviceFeeALDiff > 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeeAL },
                        Amount = decimal.Round(serviceFeeALDiff, 2, MidpointRounding.AwayFromZero),
                        Description = "Service Fee - AL"
                    });
                }
            }

            var serviceFeePD = pendingChange.Where(x => x.AmountSubTypeId == BillingConstants.ServiceFeePD).FirstOrDefault();
            if (serviceFeePD != null)
            {
                var serviceFeePDDiff = serviceFeePD.PremiumDifference ?? 0;
                if (serviceFeePDDiff > 0)
                {
                    transactionDetails.Add(new TransactionDetailDTO
                    {
                        AmountSubType = new AmountSubTypeDTO { Id = BillingConstants.ServiceFeePD },
                        Amount = decimal.Round(serviceFeePD.PremiumDifference.Value, 2, MidpointRounding.AwayFromZero),
                        Description = "Service Fee - PD"
                    });
                }
            }

            return transactionDetails;
        }

        private string ConvertToBillingPayplanOption(short paymentTypeId, int? numberOfInstallment)
        {
            string result;

            switch (paymentTypeId)
            {
                case BillingConstants.PaymentTypeFullPay:
                    result = BillingConstants.PayPlanFullPay;
                    break;
                case BillingConstants.PaymentTypePremiumFinanced:
                    result = BillingConstants.PayPlanPremiumFinanced;
                    break;
                case BillingConstants.PaymentTypeInstallment:
                    switch (numberOfInstallment.Value)
                    {
                        case 1:
                            result = InstallmentOption.Full;
                            break;
                        case 2:
                            result = InstallmentOption.Two;
                            break;
                        case 3:
                            result = InstallmentOption.Three;
                            break;
                        case 4:
                            result = InstallmentOption.Four;
                            break;
                        case 5:
                            result = InstallmentOption.Five;
                            break;
                        case 6:
                            result = InstallmentOption.Six;
                            break;
                        case 7:
                            result = InstallmentOption.Seven;
                            break;
                        case 8:
                            result = InstallmentOption.Eight;
                            break;
                        case 9:
                            result = InstallmentOption.Nine;
                            break;
                        case 10:
                            result = InstallmentOption.Ten;
                            break;
                        case 11:
                            result = InstallmentOption.Eleven;
                            break;
                        default:
                            throw new Exception("Invalid Number of Installment");
                    }
                    break;
                default:
                    throw new Exception("Invalid Pay Plan Option");
            }

            return result;
        }

        private decimal Percentage(decimal value)
        {
            return value / 100.0m;
        }

        public async Task<BillingComputeRiskTaxResponseDTO> ProcessComputeRiskTax(Guid riskId, DateTime effectiveDate, string stateCode, List<TransactionDetailDTO> transactionDetails, decimal? installmentCount)
        {
            try
            {
                BillingComputeRiskTaxDTO computeRiskTaxDTO = new BillingComputeRiskTaxDTO
                {
                    EffectiveDate = effectiveDate,
                    StateCode = stateCode,
                    TransactionDetails = transactionDetails,
                    InstallmentCount = installmentCount
                };

                return await ComputeRiskTax(riskId, computeRiskTaxDTO);
            }
            catch (BillingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<BillingComputeRiskTaxResponseDTO> ComputeRiskTax(Guid riskId, BillingComputeRiskTaxDTO computeRiskTaxDTO)
        {
            try
            {
                using (HttpContent content = new StringContent(JsonConvert.SerializeObject(computeRiskTaxDTO), Encoding.UTF8, "application/json"))
                {
                    await AddAuthHeaderToken(content);
                    var response = await _client.PostAsync($"{_billingBaseUrl}/Billing/calculate-risk-tax/{riskId}", content);

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                        throw new Exception($"{objFailedResponse.Message}");
                    }

                    BillingComputeRiskTaxResponseDTO computeRiskTaxResponseDTO = JsonTool.DeserializeObject<BillingComputeRiskTaxResponseDTO>(jsonResponse);

                    return computeRiskTaxResponseDTO;
                }
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<InstallmentAndInvoiceDTO>> GetInstallmentSchedule(RiskDTO risk)
        {
            var installmentPlan = new List<InstallmentAndInvoiceDTO>();
            try
            {
                var optionId = risk.Binding?.BindOptionId ?? 1;
                var riskCoverage = risk.RiskCoverages.FirstOrDefault(x => x.OptionNumber == optionId);

                var vehicles = risk.Vehicles.Where(x => x.Options != null && x.Options.Contains(optionId.ToString()) && x.IsActive != false).ToList();
                List<Vehicle> vehicleWithPremiums = new List<Vehicle>();

                foreach (VehicleDTO vehicle in vehicles)
                {
                    Vehicle tempVehicle = _vehicleRepository.GetById(vehicle.Id ?? Guid.Empty);
                    if (tempVehicle != null)
                    {
                        vehicleWithPremiums.Add(tempVehicle);
                    }
                }
                var taxDetails = _taxDetailsRepository.FindAsync(x => x.Id == riskCoverage.RiskCoveragePremium.TaxDetailsId.Value).Result;

                var requestParam = new InstallmentInvoiceBeforeBindRequestDTO
                {
                    EffectiveDate = risk.BrokerInfo?.EffectiveDate ?? DateTime.Now,
                    PaymentPlan = ConvertToBillingPayplanOption(risk.Binding.PaymentTypesId.GetValueOrDefault(), riskCoverage?.NumberOfInstallments)
                };

                var totalManuscriptAL = risk.RiskManuscripts.Where(m => m.IsSelected.GetValueOrDefault() && m.PremiumType == "AL").Sum(m => m.Premium);
                var totalManuscriptPD = risk.RiskManuscripts.Where(m => m.IsSelected.GetValueOrDefault() && m.PremiumType == "PD").Sum(m => m.Premium);

                requestParam.TransactionDetails = GetTransactionDetails(vehicleWithPremiums
                    , taxDetails, _mapper.Map<RiskCoverage>(riskCoverage), totalManuscriptAL, totalManuscriptPD);

                requestParam.AppId = _billingServiceSettings.GetValue<string>("AppId");
                requestParam.PercentOfOutstanding = Percentage(riskCoverage.DepositPct.GetValueOrDefault());
                requestParam.StateCode = risk.NameAndAddress.State;

                using HttpContent content = new StringContent(JsonConvert.SerializeObject(requestParam), Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/Billing/installment-and-invoice-before-bind", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                var responseData = JsonTool.DeserializeObject<List<InstallmentAndInvoiceDTO>>(jsonResponse);
                return responseData;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                return installmentPlan;
            }
        }

        public async Task<List<InstallmentAndInvoiceDTO>> GetEndorsementInstallmentSchedule(Guid riskId)
        {
            var installmentSchedule = new List<InstallmentAndInvoiceDTO>();
            try
            {
                await AddAuthHeaderToken();
                var response = await _client.GetAsync($"{_billingBaseUrl}/Billing/installment-and-invoice/{riskId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                var responseData = JsonTool.DeserializeObject<List<InstallmentAndInvoiceDTO>>(jsonResponse);
                return responseData;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                return installmentSchedule;
            }
        }

        public async Task<List<PnocDTO>> GetRisksForNoticeOfCancellation(DateTime effectiveDate)
        {
            var riskKVpair = new List<PnocDTO>();
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/Risk/get-risks-for-notice-cancellation", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                var responseData = JsonTool.DeserializeObject<List<PnocDTO>>(jsonResponse);
                return responseData;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                return riskKVpair;
            }
        }

        public async Task<List<Guid>> GetRisksForReinstate(DateTime effectiveDate)
        {
            var riskIds = new List<Guid>();
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/Risk/get-risks-for-reinstate", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                var responseData = JsonTool.DeserializeObject<List<Guid>>(jsonResponse);
                return responseData;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                return riskIds;
            }
        }

        public async Task<BillingSummaryDTO> GetBillingSummaryAsync(Guid riskId)
        {
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.GetAsync($"{_billingBaseUrl}/Billing/Summary/{riskId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                var responseData = JsonTool.DeserializeObject<BillingSummaryDTO>(jsonResponse);
                return responseData;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw;
            }
        }

        public async Task<IEnumerable<BillingFeeDTO>> GetBillingFeesAsync(Guid riskId)
        {
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.GetAsync($"{_billingBaseUrl}/Transaction/fee/{riskId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                var responseData = JsonTool.DeserializeObject<IEnumerable<BillingFeeDTO>>(jsonResponse);
                return responseData;
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw;
            }
        }

        public async Task<PaymentDetailsViewDto> GetPaymentDetails(Guid riskId, Guid paymentId)
        {
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.GetAsync($"{_billingBaseUrl}/Payment/details/{riskId}/{paymentId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }
                return JsonTool.DeserializeObject<PaymentDetailsViewDto>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw;
            }
        }

        public async Task<IEnumerable<InvoiceDTO>> GetInvoicesAsync(Guid riskId)
        {
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.GetAsync($"{_billingBaseUrl}/Invoice/{riskId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }
                return JsonTool.DeserializeObject<List<InvoiceDTO>>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw;
            }
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentsAsync(Guid riskId)
        {
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.GetAsync($"{_billingBaseUrl}/Payment/GetAll/{riskId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }
                return JsonTool.DeserializeObject<List<PaymentDTO>>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw;
            }

        }
        public async Task<List<InvoiceDTO>> GetInvoicesByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.GetAsync($"{_billingBaseUrl}/Invoice/InvoiceDetailReport/{fromDate.ToString("MM-dd-yyyy")}/{toDate.ToString("MM-dd-yyyy")}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }
                return JsonTool.DeserializeObject<List<InvoiceDTO>>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw;
            }
        }

        public async Task<StateTax> GetStateTaxAsync(DateTime effectiveDate, string stateCode)
        {
            try
            {
                GetStateTaxDTO getStateTaxDTO = new GetStateTaxDTO() { 
                    EffectiveDate = effectiveDate,
                    StateCode = stateCode
                };

                string stringContent = JsonConvert.SerializeObject(getStateTaxDTO);
                Console.WriteLine(stringContent);
                using HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/Billing/getstatetax", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }

                return JsonTool.DeserializeObject<StateTax>(jsonResponse);
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateTransactionPolicyHistoryId(Guid riskId, long policyHistoryId)
        {
            try
            {
                UpdatePolicyHistoryIdRequestDTO updatePolicyHistoryIdRequestDTO = new UpdatePolicyHistoryIdRequestDTO()
                {
                    RiskId = riskId,
                    PolicyHistoryId = policyHistoryId
                };

                string stringContent = JsonConvert.SerializeObject(updatePolicyHistoryIdRequestDTO);
                Console.WriteLine(stringContent);
                using HttpContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                await AddAuthHeaderToken(content);
                var response = await _client.PostAsync($"{_billingBaseUrl}/Transaction/UpdatePolicyHistoryId", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                    throw new Exception($"{objFailedResponse.Message}");
                }
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
