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
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Emails
{
    public class InvoiceEmailService : IInvoiceEmailService
    {
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskService _riskService;
        private readonly IAzureFunctionApiService _azureFunctionApiService;
        private readonly IEmailQueueService _emailQueueService;
        private readonly IConfiguration _configuration;
        private readonly IRiskPolicyContactService _riskPolicyContactService;

        public InvoiceEmailService(
            IRiskDetailRepository riskDetailRepository,
            IRiskService riskService,
            IAzureFunctionApiService azureFunctionApiService,
            IEmailQueueService emailQueueService,
            IConfiguration configuration,
            IRiskPolicyContactService riskPolicyContactService
        )
        {
            _riskDetailRepository = riskDetailRepository;
            _riskService = riskService;
            _azureFunctionApiService = azureFunctionApiService;
            _emailQueueService = emailQueueService;
            _configuration = configuration;
            _riskPolicyContactService = riskPolicyContactService;
        }

        public async Task<AgentInvoiceEmailContentData> CreateContentData(InvoiceDTO invoice, bool isFinanced = false)
        {
            var riskDetail = await _riskDetailRepository.GetLatestRiskDetailByRiskIdAsync(invoice.RiskId);
            var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetail.Id);
            var nameAddress = riskDTO.NameAndAddress;

            // Default Email to Policy Contacts Billing for 
            var policyContacts = await _riskPolicyContactService.GetByRiskDetailIdAsync(riskDetail.Id);
            var toEmail = policyContacts.Where(x => x.TypeId == (short)AddressTypesEnum.Billing).FirstOrDefault().Email;
            if (isFinanced)
                toEmail = riskDTO.BrokerInfo.Agent.Entity.WorkEmailAddress;

            var data = new AgentInvoiceEmailContentData(
                toEmail,
                _configuration["Email:FromAddress"].ToString(),
                $"Policy Invoice: {nameAddress.BusinessName} {riskDTO.PolicyNumber}"
            )
            {
                PolicyNumber = riskDTO.PolicyNumber,
                BusinessName = nameAddress.BusinessName,
                InvoiceNumber = invoice.InvoiceNumber
            };

            return data;
        }

        public async Task<EmailQueue> CreateEmailQueueAsync(AgentInvoiceEmailContentData contentData)
        {
            var streamContent = await _azureFunctionApiService.GenerateContentAsync(new ContentGeneratorRequest
            {
                TemplateName = "agentinvoicetemplate",
                TemplateType = "email",
                PayloadType = "agent invoice",
                Payload = JsonConvert.SerializeObject(new
                {
                    contentData.PolicyNumber,
                    contentData.BusinessName,
                    contentData.InvoiceNumber
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
                Type = "Agent Invoice",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            return emailQueue;
        }
    }
}
