using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RivTech.CABoxTruck.WebService.Controllers.Rater
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PremiumRaterController : ControllerBase
    {
        private readonly string raterUploadFilePath;
        private readonly IRaterService _raterService;
        private readonly ILogger<PremiumRaterController> log;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITaxService _taxService;
        private readonly IBillingService _billingService;
        private readonly IEndorsementService _endorsementService;
        private readonly IVehicleService _vehicleService;
        private readonly IRaterInputValuesFactory _raterInputValuesFactory;
        private readonly IRiskService _riskService;
        private readonly IPremiumService _premiumService;
        private readonly IRiskPremiumTransactionService _riskPremiumTransactionService;
        private readonly IApplogService _applogService;

        public PremiumRaterController(
            ILogger<PremiumRaterController> logger,
            IRaterService raterService,
            AppDbContext context,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            ITaxService taxService,
            IBillingService billingService,
            IEndorsementService endorsementService,
            IVehicleService vehicleService,
            IRaterInputValuesFactory raterInputValuesFactory,
            IRiskService riskService,
            IPremiumService premiumService,
            IRiskPremiumTransactionService riskPremiumTransactionService,
            IApplogService applogService
        )
        {
            log = logger;
            _raterService = raterService;
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _taxService = taxService;
            _billingService = billingService;
            _endorsementService = endorsementService;
            _vehicleService = vehicleService;
            _raterInputValuesFactory = raterInputValuesFactory;
            _riskService = riskService;
            _premiumService = premiumService;
            _applogService = applogService;
            _riskPremiumTransactionService = riskPremiumTransactionService;
        }

        [HttpGet]
        [Route("testconnection")]
        public async Task<string> TestConnection()
        {
            var client = _httpClientFactory.CreateClient("rater");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync($"api/PremiumRater/test");

            return (response.IsSuccessStatusCode) ? "Ok" : "Connection Failed.";
        }

        [HttpPost]
        [Route("calculatePremium")]
        public async Task<ActionResult> CalculatePremium([FromBody] JObject raterRequest, [FromQuery] bool? fromPolicy = null, bool? forIssuance = null, DateTime? effectiveDate = null)
        {
            Guid riskDetailId = Guid.Parse(raterRequest.SelectToken("riskDetailId")?.Value<string>() ?? "");
            string submissionNumber = raterRequest.SelectToken("SubmissionNo")?.Value<string>();
            DateTime? inceptionDate = raterRequest.SelectToken("Broker.EffectiveDate")?.Value<DateTime?>();
            string optionId = raterRequest.SelectToken("OptionId").Value<string>();

            try
            {
                // Preload rater
                await _raterService.PreloadPremiumRater(submissionNumber, riskDetailId, fromPolicy);

                // Create input values
                //var inputValues = await _raterService.MapFields(riskDetailId, raterRequest, int.Parse(optionId), true, fromPolicy);
                var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetailId);
                var inputValues = await _raterInputValuesFactory.CreateAsync(riskDTO, int.Parse(optionId));

                int? endorsementNumber = null;
                if (forIssuance == true)
                {
                    endorsementNumber = await _endorsementService.GetNextEndorsementNumber(riskDetailId);
                }

                raterRequest = new JObject(
                    new JProperty("SubmissionNumber", submissionNumber),
                    new JProperty("InceptionDate", inceptionDate),
                    new JProperty("OptionId", optionId),
                    new JProperty("EndorsementNumber", endorsementNumber),
                    new JProperty("InputValues", JsonConvert.DeserializeObject(JsonConvert.SerializeObject(inputValues)))
                );

                string results = string.Empty;


                results = await CalculateAsync(raterRequest, riskDTO, fromPolicy, forIssuance);

                return new JsonResult(JObject.Parse(results));
            }
            catch (RaterException e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("preload/{submissionNumber}")]
        public async Task<JsonResult> Preload(string submissionNumber, Guid riskDetailId, bool? fromPolicy = null)
        {
            try
            {
                var result = await _raterService.PreloadPremiumRater(submissionNumber, riskDetailId, fromPolicy);
                return new JsonResult(new
                {
                    status = true,
                    message = result
                });
            }
            catch (RaterException e)
            {
                log.LogError(e, e.Message);
                return new JsonResult(new
                {
                    status = false,
                    message = e.Message
                });
            }
            catch (Exception e)
            {
                log.LogError(e, "Error preloading rater");
                return new JsonResult(new { status = false, message = "There was an error trying to pre-load the rating file. " + e.Message, sysErr = e.Message }, new JsonSerializerSettings());
            }
        }

        [HttpPost]
        [Route("unload/{submissionNumber}")]
        public async Task Unload(string submissionNumber, string riskDetailId = "", bool ispageunload = false, bool? fromPolicy = null)
        {
            Guid _riskDetailId = Guid.Parse(riskDetailId);
            // always pass the option count to cross reference on the running excel engine
            var options = await _raterService.GetOptions(_riskDetailId);
            using var httpClient = _httpClientFactory.CreateClient("rater");
            int count = ispageunload ? 0 : options.Count;

            var url = $"api/PremiumRater/unload/{submissionNumber}?itemCount={count}";
            url += $"&fromPolicy={fromPolicy}";

            await httpClient.DeleteAsync(url);
        }

        private async Task<string> CalculateAsync(JObject requestBody, RiskDTO riskDTO, bool? fromPolicy = null, bool? forIssuance = null)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient("rater");
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"api/PremiumRater/rate/{riskDTO.SubmissionNumber}?&fromPolicy={fromPolicy}&forIssuance={forIssuance}", content);
                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);
                var result = await response.Content.ReadAsStringAsync();
                int optionId = requestBody.SelectToken("OptionId").Value<int>();

                if (fromPolicy != true)
                {
                    await _vehicleService.RemovePremiumRatingFactorOfUncheckedsAsync(riskDTO.Id.Value, Convert.ToInt32(optionId));
                }

                result = await _raterService.ExtractAndSaveRatingFactors(riskDTO.Id.Value, result, fromPolicy ?? false, riskDTO);
                await _premiumService.UpdateRaterPremiumsAsync(riskDTO.Id.Value, fromPolicy, optionId);
                await _riskPremiumTransactionService.CalculateAsync(riskDTO.Id.Value);


                if (fromPolicy == true)
                {
                    await _taxService.CalculateRiskTaxAsync(riskDTO.Id.Value, isEndorsement: true);
                    await _endorsementService.CalculatePremiumChanges(riskDTO.Id.Value, riskDTO.EffectiveDate.Value, optionId);
                }
                else
                {
                    await _taxService.CalculateRiskTaxAsync(riskDTO.Id.Value, optionId);
                    await _billingService.UpdatePaymentOptions(riskDTO.Id.Value, optionId);
                    await _taxService.CalculateRiskTaxAsync(riskDTO.Id.Value, optionId); // Recalculate Tax. Temp fix.. need to study more - Mac 04112023
                }

                await _applogService.Save($"Submission Rater result", result);

                return result.Replace("ERROR", "0.00");
            }
            catch (RaterException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception($"Error encountered while calculating premium. {e.Message}", e);
            }
        }

        private decimal parseValueToDecimal(string value)
        {
            return (string.IsNullOrEmpty(value) || value == "ERROR") ? 0.00m : Math.Round(Convert.ToDecimal(value), 2, MidpointRounding.AwayFromZero);
        }

        [HttpGet("GetRatingFactors/{riskDetailId}")]
        public async Task<IActionResult> GetRatingFactors([FromRoute] Guid riskDetailId)
        {
            var dto = await _raterService.GetRatingFactorsAsync(riskDetailId);
            if (dto is null)
                return NotFound();
            return Ok(dto);
        }

        [HttpPut]
        [Route("UpdateRatingFactors/{riskDetailId}")]
        public async Task<RatingFactorsDTO> UpdateRatingFactors([FromRoute] Guid riskDetailId, [FromBody] RatingFactorsDTO ratingFactors)
        {
            await _raterService.UpdateRatingFactors(riskDetailId, ratingFactors);
            return ratingFactors;
        }

        [HttpGet("GetRaterVersion/{riskDetailId}")]
        public async Task<dynamic> GetRaterVersion([FromRoute] Guid riskDetailId)
        {
            var risk = await _riskService.GetByIdIncludeAsync(riskDetailId);
            var raterVersion = await _raterService.GetRaterVersionAsync(risk.RaterVersionId, riskDetailId, risk.IsRaterVersionLocked);
            return Ok(raterVersion);
        }

        [HttpGet("GetRaterVersionOfEffectiveDate/{effectiveDate}")]
        public async Task<dynamic> GetRaterVersion([FromRoute] DateTime effectiveDate)
        {
            var raterVersion = await _raterService.GetRaterVersionAsync(effectiveDate);
            return Ok(raterVersion);
        }


        [HttpPut("InitializeVersion/{riskId}")]
        public async Task<dynamic> InitializeRaterVersion([FromRoute] Guid riskId)
        {
            var raterVersion = await _raterService.InitializeRaterVersionAsync(riskId);
            return Ok(raterVersion);
        }

        [HttpPost]
        [Route("download")]
        public async Task<ActionResult> Download([FromBody] JObject requestBody)
        {
            string submissionNo = requestBody["SubmissionNo"].ToString();

            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };

            //using var httpclient = new HttpClient();
            using var httpclient = _httpClientFactory.CreateClient("rater");
            //var content = new StringContent(JsonConvert.SerializeObject(inputValues), Encoding.UTF8, "application/json");
            var endpoint = $"api/PremiumRaterArchive/DownloadLatest/{submissionNo}";
            var res = await httpclient.PostAsync(endpoint, null);
            if (!res.IsSuccessStatusCode)
            {
                return BadRequest(res.ReasonPhrase);
            }
            var contentBytes = await res.Content.ReadAsByteArrayAsync();

            var response = new FileContentResult(contentBytes, "application/vnd.ms-excel.sheet.binary.macroEnabled.12")
            {
                FileDownloadName = $"Premium Rater.xlsb"
            };

            return response;
        }
    }
}
