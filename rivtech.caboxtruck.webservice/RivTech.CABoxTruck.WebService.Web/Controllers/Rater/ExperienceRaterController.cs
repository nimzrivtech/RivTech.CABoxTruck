using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Text;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.DTO;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace RivTech.CABoxTruck.WebService.Controllers.Rater
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExperienceRaterController : ControllerBase
    {
        private readonly ICoverageLimitService _coverageLimitService;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IRaterService _raterService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ExperienceRaterController> _logger;
        private readonly IRaterInputValuesFactory _raterInputValuesFactory;
        private readonly IRiskService _riskService;
        private readonly IExperienceRaterVersionService _experienceRaterVersionService;

        public ExperienceRaterController(
            ILogger<ExperienceRaterController> logger,
            ICoverageLimitService coverageLimitService,
            IRaterService raterService,
            ICoverageHistoryService coverageHistoryService,
            IHttpClientFactory httpClientFactory,
            IRaterInputValuesFactory raterInputValuesFactory,
            IRiskService riskService,
            IExperienceRaterVersionService experienceRaterVersionService
        )
        {
            _logger = logger;
            _coverageLimitService = coverageLimitService;
            _coverageHistoryService = coverageHistoryService;
            _raterService = raterService;
            _httpClientFactory = httpClientFactory;
            _raterInputValuesFactory = raterInputValuesFactory;
            _riskService = riskService;
            _experienceRaterVersionService = experienceRaterVersionService;
        }

        [HttpGet]
        [Route("testconnection")]
        public async Task<string> TestConnection()
        {

            var client = _httpClientFactory.CreateClient("rater");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync($"api/PremiumRater/test");

            return (response.IsSuccessStatusCode) ? "Ok" : "Connection Failed.";
        }

        [HttpPost]
        [Route("create/{id}")]
        public async Task<ActionResult> Create(string id, [FromQuery] int raterVersionId)
        {
            bool isExists = await IsRaterExcelExists(id);
            if (isExists) return Ok();

            using var httpClient = _httpClientFactory.CreateClient("rater");
            var response = await httpClient.PostAsync($"api/ExperienceRater/create/{id}?raterVersionId={raterVersionId}", new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            return Ok(response);
        }

        private async Task<bool> IsRaterExcelExists(string id)
        {
            using var httpClient = _httpClientFactory.CreateClient("rater");
            var response = await httpClient.GetAsync($"api/ExperienceRater/exists/{id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
            var content = await response.Content.ReadAsStringAsync();
            return bool.Parse(content);
        }

        [HttpPost]
        [Route("download/{id}")]
        public async Task<ActionResult> Download(Guid id, [FromQuery] int raterVersionId, [FromBody] JObject requestBody)
        {
            string submissionNo = requestBody["SubmissionNo"].ToString();

            var riskDTO = await _riskService.GetByIdIncludeAsync(id);
            var inputValues = await _raterInputValuesFactory.CreateAsync(riskDTO);

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
            var content = new StringContent(JsonConvert.SerializeObject(inputValues), Encoding.UTF8, "application/json");
            var endpoint = $"api/ExperienceRater/download/{riskDTO.SubmissionNumber}?raterVersionId={raterVersionId}";
            var res = await httpclient.PostAsync(endpoint, content);
            if (!res.IsSuccessStatusCode)
            {
                return BadRequest(res.ReasonPhrase);
            }
            var contentBytes = await res.Content.ReadAsByteArrayAsync();

            var response = new FileContentResult(contentBytes, "application/vnd.ms-excel.sheet.binary.macroEnabled.12")
            {
                FileDownloadName = $"Experience Rater.xlsb"
            };

            return response;
        }

        [HttpPost]
        [Route("upload/{id}")]
        public async Task<ActionResult> Upload(string id)
        {
            IFormFile postedFile = Request.Form.Files[0];
            Guid riskDetailId = Guid.Parse(Request.Form["riskDetailId"]);
            try
            {
                if (postedFile.Length < 1)
                {
                    return BadRequest("No file found from request.");
                }

                using var stream = postedFile.OpenReadStream();
                byte[] content = new byte[postedFile.Length];
                stream.Read(content, 0, content.Length);
                var uploadResults = await _raterService.UploadExperienceRaterAsync(riskDetailId, postedFile.FileName, content);

                return new JsonResult(uploadResults);
            }
            catch (RaterException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("downloadlatest/{riskId}")]
        public async Task<ActionResult> Download(string riskId, [FromQuery] int raterVersionId, [FromBody] JObject requestBody)
        {
            string submissionNo = requestBody["SubmissionNo"].ToString();

            var riskDetailSubmission = await _riskService.GetSubmissionRiskDetailAsync(Guid.Parse(riskId));
            var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetailSubmission.Id);
            var inputValues = await _raterInputValuesFactory.CreateAsync(riskDTO);

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
            //var endpoint = $"api/ExperienceRater/DownloadLatest/{submissionNo}";
            //var res = await httpclient.PostAsync(endpoint, null);
            //if (!res.IsSuccessStatusCode)
            //{
            //    return BadRequest(res.ReasonPhrase);
            //}

            var content = new StringContent(JsonConvert.SerializeObject(inputValues), Encoding.UTF8, "application/json");
            var endpoint = $"api/ExperienceRater/UploadIssuance/{submissionNo}/{raterVersionId}";
            var res = await httpclient.PostAsync(endpoint, content);
            if (!res.IsSuccessStatusCode)
            {
                return BadRequest(res.ReasonPhrase);
            }
            var contentBytes = await res.Content.ReadAsByteArrayAsync();

            var response = new FileContentResult(contentBytes, "application/vnd.ms-excel.sheet.binary.macroEnabled.12")
            {
                FileDownloadName = $"Experience Rater.xlsb"
            };

            return response;
        }

        [HttpGet("ShouldUpdate/{riskId}")]
        public async Task<bool> ShouldUpdateVersion(Guid riskId)
        {
            return await _raterService.ShouldUpdateXpRaterAsync(riskId);
        }

        [HttpPut("UpdateVersion/{riskDetailId}")]
        public async Task Update(Guid riskDetailId)
        {
            await _experienceRaterVersionService.UpdateExperienceRaterAsync(riskDetailId);
        }

        [HttpPut("ConfirmVersionUpdate/{riskId}")]
        public async Task ConfirmVersionUpdate(Guid riskId)
        {
            await _experienceRaterVersionService.ConfirmVersionUpdateAsync(riskId);
        }

        [HttpGet("GetDto/{riskId}")]
        public async Task<IActionResult> GetDto([FromRoute] Guid riskId)
        {
            var dto = await _raterService.GetExperienceRaterDtoAsync(riskId);
            return Ok(dto);
        }

        [HttpPut("GetMismatchedVersions")]
        public async Task<IActionResult> FixMismatchedVersions()
        {
            var mismatchList = await _experienceRaterVersionService.GetRisksWithMismatchAsync();
            return Ok(mismatchList);
        }

        [HttpGet("AllowExpRaterDownload/{riskId}")]
        public async Task<bool> AllowExpRaterDownload([FromRoute] string riskId)
        {
            var riskDetailSubmission = await _riskService.GetSubmissionRiskDetailAsync(Guid.Parse(riskId));
            var expRaterDto = await _raterService.GetExperienceRaterDtoAsync(Guid.Parse(riskId));

            if (riskDetailSubmission.IsNewVenture == true && expRaterDto == null)
                return false;

            return true;
        }
    }
}
