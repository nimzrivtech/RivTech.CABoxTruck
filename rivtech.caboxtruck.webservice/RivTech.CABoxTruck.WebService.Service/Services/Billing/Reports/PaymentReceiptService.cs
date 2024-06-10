using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO.Billing.Payments;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Billing.Reports
{
    public interface IPaymentReceiptService
    {
        Task<Stream> GenerateFileAsync(Guid riskId, Guid paymentId);
        Task<PaymentReceiptDTO> GetDataAsync(Guid riskId, Guid paymentId);
    }

    public class PaymentReceiptService : IPaymentReceiptService
    {
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskService _riskService;
        private readonly IBillingService _billingService;
        private readonly ILog4NetService _errorLogService;
        private readonly IAzureFunctionApiService _azureFunctionService;
        private readonly IRiskPolicyContactService _riskPolicyContactService;

        public PaymentReceiptService(
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository,
            IRiskService riskService,
            IBillingService billingService,
            ILog4NetService errorLogService,
            IAzureFunctionApiService azureFunctionService,
            IRiskPolicyContactService riskPolicyContactService
        )
        {
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskService = riskService;
            _billingService = billingService;
            _errorLogService = errorLogService;
            _azureFunctionService = azureFunctionService;
            _riskPolicyContactService = riskPolicyContactService;
        }

        public async Task<PaymentReceiptDTO> GetDataAsync(Guid riskId, Guid paymentId)
        {
            try
            {
                var risk = await _riskRepository.FindAsync(riskId);
                var riskDetail = await _riskDetailRepository.GetLastRiskDetailByRiskIdAsync(riskId);
                var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetail.Id);
                var paymentDetailsDto = await _billingService.GetPaymentDetails(riskId, paymentId);
                var billing = riskDTO.RiskPolicyContacts.First(rpc => rpc.TypeId == PolicyContactTypes.Billing.Id);
                var billingAddress = riskDTO.NameAndAddress.Addresses.FirstOrDefault(x => x.AddressTypeId == PolicyContactTypes.Billing.Id);

                var brokerInfo = riskDTO.BrokerInfo;
                var broker = brokerInfo.Agency.Entity;
                var brokerAddress = broker.EntityAddresses.ToList().FirstOrDefault()?.Address;

                var riskDetailContacts = await _riskPolicyContactService.GetByRiskDetailIdAsync(risk.ForEndorsementRiskDetailId.Value);
                var billingContact = riskDetailContacts.FirstOrDefault(x => x.TypeId == PolicyContactTypes.Billing.Id);

                var paymentReceiptDto = new PaymentReceiptDTO();
                paymentReceiptDto.UnderwriterCompany = "Texas Insurance Company";
                paymentReceiptDto.PolicyNumber = risk.PolicyNumber;
                paymentReceiptDto.PolicyPeriod = $"{brokerInfo.EffectiveDate:MM/dd/yyyy} - {brokerInfo.ExpirationDate:MM/dd/yyyy}";
                paymentReceiptDto.Insured = riskDTO.NameAndAddress.BusinessName;

                paymentReceiptDto.BillingAddress = $"{billingContact.FullName}"
                                                    + $"\n{paymentReceiptDto.Insured}"
                                                    + $"\n{billingAddress.Address.StreetAddress1}"
                                                    + $"\n{billingAddress.Address.City} {billingAddress.Address.State} {billingAddress.Address.ZipCode}"
                                                    + $"\n{FormatPhone(billingContact.Phone)}"
                                                    + $"\n{billingContact.Email}";

                paymentReceiptDto.Broker = broker.CompanyName;
                if (brokerAddress != null)
                {
                    paymentReceiptDto.Broker += "\n" + brokerAddress.StreetAddress1
                    + (!string.IsNullOrWhiteSpace(brokerAddress.StreetAddress2) ? $"\n{brokerAddress.StreetAddress2}" : "")
                    + $"\n{brokerAddress.City} {brokerAddress.StateCode} {brokerAddress.ZipCode}";
                }
                if (!string.IsNullOrWhiteSpace(broker.WorkPhone))
                {
                    paymentReceiptDto.Broker  += $"\n{FormatPhone(broker.WorkPhone)}";
                }
                paymentReceiptDto.Broker += $"\n{broker.WorkEmailAddress}";

                paymentReceiptDto.Amount = paymentDetailsDto.Amount;
                paymentReceiptDto.PaymentMethod = paymentDetailsDto.PaymentMethod;
                paymentReceiptDto.ConfirmationNumber = paymentDetailsDto.Reference;
                paymentReceiptDto.DateTime = paymentDetailsDto.PostDate;

                return paymentReceiptDto;
            }
            catch (Exception ex)
            {
                _errorLogService.Error(ex);
                throw;
            }

        }

        private string FormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || !long.TryParse(phone, out _))
                return phone;

            if (phone.Length == 10)
                return string.Format("{0:(###) ###-####}", long.Parse(phone));

            return phone;
        }

        public async Task<Stream> GenerateFileAsync(Guid riskId, Guid paymentId)
        {
            // Get data
            var data = await GetDataAsync(riskId, paymentId);

            // Create payload for report service
            var contentRequest = new ContentGeneratorRequest
            {
                PayloadType = "PaymentReceipt",
                Payload = JsonConvert.SerializeObject(data)
            };
            var contentResult = await _azureFunctionService.GenerateContentAsync(contentRequest);
            return contentResult;
        }
    }
}
