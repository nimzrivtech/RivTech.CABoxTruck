using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.AzureFunctions;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Emails.Invoices;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Emails;
using RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Emails;

namespace RivTech.CABoxTruck.WebService.Service.Services.Emails
{
    public class EndorsementEmailService : IEndorsementEmailService
    {
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskService _riskService;
        private readonly IAzureFunctionApiService _azureFunctionApiService;
        private readonly IEmailQueueService _emailQueueService;
        private readonly IConfiguration _configuration;

        public EndorsementEmailService(
            IRiskDetailRepository riskDetailRepository,
            IRiskService riskService,
            IAzureFunctionApiService azureFunctionApiService,
            IEmailQueueService emailQueueService,
            IConfiguration configuration
        )
        {
            _riskDetailRepository = riskDetailRepository;
            _riskService = riskService;
            _azureFunctionApiService = azureFunctionApiService;
            _emailQueueService = emailQueueService;
            _configuration = configuration;
        }

        public async Task<EndorsementEmailContentData> CreateContentData(Guid riskDetailId)
        {
            var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetailId);
            var nameAddress = riskDTO.NameAndAddress;

            var brokerAgentEntity = riskDTO.BrokerInfo.Agent.Entity;

            var data = new EndorsementEmailContentData(
                brokerAgentEntity.WorkEmailAddress,
                _configuration["Email:FromAddress"].ToString(),
                $"Endorsement: {nameAddress.BusinessName} {riskDTO.PolicyNumber}"
            )
            {
                PolicyNumber = riskDTO.PolicyNumber,
                BusinessName = nameAddress.BusinessName
            };

            return data;
        }

        public async Task<EmailQueue> CreateEmailQueueAsync(EndorsementEmailContentData contentData)
        {
            var streamContent = await _azureFunctionApiService.GenerateContentAsync(new ContentGeneratorRequest
            {
                TemplateName = "endorsementtemplate",
                TemplateType = "email",
                PayloadType = "endorsement",
                Payload = JsonConvert.SerializeObject(new
                {
                    contentData.PolicyNumber,
                    contentData.BusinessName
                }),
            });

            var stringContent = new StreamReader(streamContent).ReadToEnd();
            using MemoryStream memStream = new MemoryStream();
            streamContent.CopyTo(memStream);
            var attachment = new Uri(contentData.AttachmentPath.Trim('"'));

            var emailQueue = new EmailQueue
            {
                Address = contentData.ToEmail,
                FromAddress = contentData.FromEmail,
                Subject = contentData.Subject,
                Content = stringContent,
                AttachmentUrl = attachment.AbsoluteUri.Trim(),
                AttachmentFileName = contentData.AttachmentFilename,
                //Attachment = memStream.ToArray(),
                Type = "Endorsement",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            return emailQueue;
        }
    }
}
