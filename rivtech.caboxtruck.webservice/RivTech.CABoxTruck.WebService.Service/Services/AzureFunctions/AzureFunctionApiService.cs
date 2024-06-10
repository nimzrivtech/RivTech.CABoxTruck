using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.AzureFunctions;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions
{
    public class AzureFunctionApiService : IAzureFunctionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AzureFunctionApiService> _logger;
        private readonly AzureFunctionAppSettings _azureFunctionAppSettings;

        public AzureFunctionApiService(
            ILogger<AzureFunctionApiService> logger,
            HttpClient httpClient,
            IOptions<AzureFunctionAppSettings> options
        )
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            _azureFunctionAppSettings = options.Value;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method for azure function api '/api/ContentGeneratorFunction'.
        /// </summary>
        public async Task<Stream> GenerateContentAsync(ContentGeneratorRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var url = $"/api/ContentGeneratorFunction?code={_azureFunctionAppSettings.ContentGeneratorFunctionApiCode}";
                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return null;
        }

        ///// <summary>
        ///// Method for azure function api '/api/EmailProviderFunction'.
        ///// </summary>
        //public async Task SendEmailAsync(SendEmailRequestDTO request)
        //{
        //    var json = JsonConvert.SerializeObject(request);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    try
        //    {
        //        var response = await _httpClient.PostAsync(CreateEndpoint("/api/EmailProviderFunction"), content);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            throw new Exception(response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, e.Message);
        //    }
        //}
    }
}
