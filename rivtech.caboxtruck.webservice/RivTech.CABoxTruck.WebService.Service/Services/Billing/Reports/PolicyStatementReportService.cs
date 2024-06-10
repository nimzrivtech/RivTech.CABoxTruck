using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration.Reports;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Billing.Reports
{
    public interface IPolicyStatementReportService
    {
        Task<Stream> GenerateFileAsync(Guid riskId);
    }

    public class PolicyStatementReportService : IPolicyStatementReportService
    {
        private readonly ILog4NetService _errorLogger;
        private readonly IAzureFunctionApiService _azureFunctionService;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskService _riskService;
        private readonly IBillingService _billingService;

        public PolicyStatementReportService(
            ILog4NetService errorLogger,
            IAzureFunctionApiService azureFunctionService,
            IRiskRepository riskRepository,
            IRiskService riskService,
            IBillingService billingService
        )
        {
            _errorLogger = errorLogger;
            _azureFunctionService = azureFunctionService;
            _riskRepository = riskRepository;
            _riskService = riskService;
            _billingService = billingService;
        }

        public async Task<Stream> GenerateFileAsync(Guid riskId)
        {
            // Get data
            var data = await GetDataAsync(riskId);

            // Create payload for report service
            var contentRequest = new ContentGeneratorRequest
            {
                PayloadType = "PolicyStatement",
                Payload = JsonConvert.SerializeObject(data)
            };
            var contentResult = await _azureFunctionService.GenerateContentAsync(contentRequest);
            return contentResult;
        }

        private async Task<object> GetDataAsync(Guid riskId)
        {
            try
            {
                var risk = await _riskRepository.FindAsync(riskId);
                var riskDTO = await _riskService.GetByIdIncludeAsync(risk.CurrentRiskDetailId.Value);
                var brokerInfo = riskDTO.BrokerInfo;
                var broker = brokerInfo.Agency.Entity;
                var billing = await _billingService.GetBillingSummaryAsync(riskId);
                var invoices = await _billingService.GetInvoicesAsync(risk.Id);
                var payments = await _billingService.GetPaymentsAsync(risk.Id);

                var policyStatementReportDto = new PolicyStatementReportDTO
                {
                    UnderwriterCompany = "Texas Insurance Company",
                    PolicyNumber = risk.PolicyNumber,
                    PolicyPeriod = $"{brokerInfo.EffectiveDate:MM/dd/yyyy} - {brokerInfo.ExpirationDate:MM/dd/yyyy}",
                    Insured = riskDTO.NameAndAddress.BusinessName,
                    PastDueAmount = billing.PastDueAmount,
                    AccountBalance = billing.Balance + (billing.TotalCommissionBalance.HasValue ? billing.TotalCommissionBalance.Value : 0),
                    InvoiceTotal = billing.Billed + (billing.TotalCommissionBilled.HasValue ? billing.TotalCommissionBilled.Value : 0),
                    PaymentTotal = billing.Paid + (billing.TotalCommissionPaid.HasValue ? billing.TotalCommissionPaid.Value : 0),
                    DateTime = DateTime.Now,
                };

                policyStatementReportDto.Invoices = invoices.Where(x => x.VoidDate == null).
                    Select(x => new PolicyStatementReportDTO.PolicyStatementInvoiceDTO
                {
                    Number = x.InvoiceNumber,
                    DueDate = x.DueDate,
                    Amount = x.CurrentAmountInvoiced - x.BrokerCommission,
                    IsVoided = x.VoidDate != null
                }).ToArray();

                policyStatementReportDto.Payments = payments.Select(x => new PolicyStatementReportDTO.PolicyStatementPaymentDTO
                {
                    Number = x.Reference,
                    PostDate = x.EffectiveDate.AddHours(5), //TODO: NBTSUP-238: Ongoing checking of effective date
                    Amount = x.Amount,
                    IsVoided = x.VoidDate != null
                }).ToArray();

                return policyStatementReportDto;
            }
            catch (Exception ex)
            {
                _errorLogger.Error(ex);
                throw;
            }
        }
    }
}
