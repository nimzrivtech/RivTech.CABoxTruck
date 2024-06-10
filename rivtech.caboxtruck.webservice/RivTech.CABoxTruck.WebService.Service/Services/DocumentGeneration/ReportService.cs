using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;
        private readonly ILog4NetService _logger;

        public ReportService(
            IConfiguration configuration,
            ILog4NetService logger
        ) {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateFileReportLink(ReportPayload payload)
        {
            var reportServiceSection = _configuration.GetSection("Report");
            var baseUrl = reportServiceSection.GetValue<string>("BaseUrl");
            var generateReportLinkEndPoint = reportServiceSection.GetValue<string>("GenerateReportLink");
            var certificateValidation = reportServiceSection.GetValue<bool>("CertificateValidation");

            var client = new RestClient(baseUrl);
            client.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds);
            var request = new RestRequest
            {
                Resource = generateReportLinkEndPoint,
                RequestFormat = DataFormat.Json,
                Method = Method.POST
            };

            var serializePayload = JsonConvert.SerializeObject(payload);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", serializePayload, ParameterType.RequestBody);
            // To Follow JWT Token

            if (!certificateValidation)
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            var response = await client.ExecuteAsync(request);
            if(!response.IsSuccessful)
            {
                _logger.Error(response.ErrorMessage, response.Content, nameof(GenerateFileReportLink));
            }

            //TODO: Return useful error message, not null;
            return response.IsSuccessful ? response.Content : null;
        }
    }
}
