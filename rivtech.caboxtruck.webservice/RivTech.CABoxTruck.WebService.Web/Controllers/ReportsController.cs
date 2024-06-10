using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions;
using RivTech.CABoxTruck.WebService.Service.Services.Billing.Reports;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IAccountsReceivableService _accountsReceivableService;
        private readonly IPaymentPostingService _paymentPostingService;
        private readonly IBrokerCommissionService _brokerCommissionService;
        private readonly IPaymentReceiptService _paymentReceiptService;
        private readonly IPolicyStatementReportService _policyStatementReportService;
        private readonly IPremiumEarnedTaxReportService _premiumEarnedTaxReportService;
        private readonly IBorderauxReportService _borderauxReportService;
        private readonly ICovenirReportService _covenirReportService;
        private readonly IPreBindPaymentService _preBindPaymentService;
        private readonly IReportService _reportService;
        private readonly IAzureFunctionApiService _azureFunctionService;
        private readonly ISubmissionQuoteReportService _submissionQuoteReportService;
        private readonly IFilingsReportService _filingsReportService;
        private readonly IAgingReportService _agingReportService;

        public ReportsController(
            IAccountsReceivableService accountsReceivableService,
            IReportService reportService,
            IPaymentPostingService paymentPostingService,
            IPreBindPaymentService preBindPaymentService,
            IBrokerCommissionService brokerCommissionService,
            IPaymentReceiptService paymentReceiptService,
            IPolicyStatementReportService policyStatementReportService,
            IPremiumEarnedTaxReportService premiumEarnedTaxReportService,
            IBorderauxReportService borderauxReportService,
            ICovenirReportService covenirReportService,
            IAzureFunctionApiService azureFunctionService,
            ISubmissionQuoteReportService submissionQuoteReportService,
            IFilingsReportService filingsReportService,
            IAgingReportService agingReportService
        )
        {
            _accountsReceivableService = accountsReceivableService;
            _paymentPostingService = paymentPostingService;
            _brokerCommissionService = brokerCommissionService;
            _paymentReceiptService = paymentReceiptService;
            _policyStatementReportService = policyStatementReportService;
            _premiumEarnedTaxReportService = premiumEarnedTaxReportService;
            _borderauxReportService = borderauxReportService;
            _covenirReportService = covenirReportService;
            _reportService = reportService;
            _preBindPaymentService = preBindPaymentService;
            _azureFunctionService = azureFunctionService;
            _submissionQuoteReportService = submissionQuoteReportService;
            _filingsReportService = filingsReportService;
            _agingReportService = agingReportService;
        }

        [HttpPost("SearchAccountsReceivable")]
        [Produces(typeof(List<AccountsReceivableDTO>))]
        public async Task<IActionResult> SearchAccountsReceivable([FromBody] AccountsReceivableInputDTO input)
        {
            try
            {
                return Ok(await _accountsReceivableService.SearchAccountsReceivable(input));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GeneratePaymentPosting")]
        [Produces(typeof(List<PaymentPostingDTO>))]
        public async Task<IActionResult> GeneratePaymentPosting([FromBody] PaymentPostingInputDTO input)
        {
            try
            {
                return Ok(await _paymentPostingService.GeneratePaymentPosting(input));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GenerateBrokerCommission")]
        [Produces(typeof(List<BrokerCommissionDTO>))]
        public async Task<IActionResult> GenerateBrokerCommission([FromBody] BrokerCommissionInputDTO input)
        {
            try
            {
                return Ok(await _brokerCommissionService.GenerateBrokerCommission(input));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GeneratePreBindPayments")]
        [Produces(typeof(List<PreBindPaymentDTO>))]
        public async Task<IActionResult> GeneratePreBindPayments([FromBody] PreBindPaymentInputDTO input)
        {
            try
            {
                return Ok(await _preBindPaymentService.GeneratePreBindPayments(input));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GetReportStream")]
        [Produces(typeof(FileStreamResult))]
        public async Task<IActionResult> Post([FromBody] ReportRequestModel reportRequestModel)
        {
            reportRequestModel.PaymentRequest.isRaterV7 = reportRequestModel.IsRaterV7;
            return new FileStreamResult(await _reportService.GetReportStream(reportRequestModel.PaymentRequest),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = reportRequestModel.PaymentRequest.ReportFileName + ".xlsx" };
        }

        [HttpPost("GeneratePaymentReceipt/{riskId}/{paymentId}")]
        [Produces(typeof(FileContentResult))]
        public async Task<IActionResult> GeneratePaymentReceipt([FromRoute] Guid riskId, [FromRoute] Guid paymentId)
        {
            var data = await _paymentReceiptService.GenerateFileAsync(riskId, paymentId);
            return new FileStreamResult(data, "application/pdf") {
                FileDownloadName = $"payment_receipt_{paymentId}.pdf" 
            };
        }

        [HttpPost("GeneratePolicyStatement/{riskId}")]
        [Produces(typeof(FileContentResult))]
        public async Task<IActionResult> GeneratePolicyStatement([FromRoute] Guid riskId)
        {
            var data = await _policyStatementReportService.GenerateFileAsync(riskId);
            return new FileStreamResult(data, "application/pdf") {
                FileDownloadName = $"Policy Statement {riskId}.pdf" 
            };
        }

        [HttpPost("PremiumEarnedTaxReport")]
        [Produces(typeof(List<PremiumEarnedTaxReportDTO>))]
        public async Task<IActionResult> GetPremiumEarnedTaxReport([FromBody] PremiumEarnedTaxReportRequestDTO request)
        {
            try
            {
                return Ok(await _premiumEarnedTaxReportService.GetData(request));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GeneratePremiumEarnedTaxReport")]
        [Produces(typeof(List<PremiumEarnedTaxReportDTO>))]
        public async Task<IActionResult> GeneratePremiumEarnedTaxReport([FromBody] PremiumEarnedTaxReportRequestDTO request)
        {
            try
            {
                var data = await _premiumEarnedTaxReportService.GetData(request);

                // Create payload for report service
                var contentRequest = new ContentGeneratorRequest
                {
                    PayloadType = "PremiumEarnedTaxReport",
                    Payload = JsonConvert.SerializeObject(data)
                };

                return new FileStreamResult(await _azureFunctionService.GenerateContentAsync(contentRequest),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                { FileDownloadName = "PremiumEarnedTaxReport.xlsx" };
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("BorderauxReport")]
        [Produces(typeof(List<BorderauxReportDTO>))]
        public async Task<IActionResult> GetBorderauxReport([FromBody] BorderauxReportRequestDTO request)
        {
            try
            {
                return Ok(await _borderauxReportService.GetData(request));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("FilingsReport")]
        [Produces(typeof(List<FilingsReportDTO>))]
        public async Task<IActionResult> FilingsReport()
        {
            try
            {
                return Ok(await _filingsReportService.GetData());
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GenerateBordereauxReport")]
        [Produces(typeof(List<BorderauxReportDTO>))]
        public async Task<IActionResult> GenerateBordereauxReport([FromBody] BorderauxReportRequestDTO request)
        {
            try
            {
                var data = await _borderauxReportService.GetData(request);

                // Create payload for report service
                var contentRequest = new ContentGeneratorRequest
                {
                    PayloadType = "BorderauxReport",
                    Payload = JsonConvert.SerializeObject(data)
                };

                return new FileStreamResult(await _azureFunctionService.GenerateContentAsync(contentRequest),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                { FileDownloadName = "BorderauxReport.xlsx" };
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GenerateSubmissionQuoteReport")]
        [Produces(typeof(List<BorderauxReportDTO>))]
        public async Task<IActionResult> GenerateSubmissionQuoteReport([FromBody] SubmissionQuoteReportRequestDTO request)
        {
            try
            {
                var data = await _submissionQuoteReportService.GetData(request);

                // Create payload for report service
                var contentRequest = new ContentGeneratorRequest
                {
                    PayloadType = "SubmissionQuoteReport",
                    Payload = JsonConvert.SerializeObject(data)
                };

                return new FileStreamResult(await _azureFunctionService.GenerateContentAsync(contentRequest),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                { FileDownloadName = "SubmissionQuoteReport.xlsx" };
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("CovenirReport")]
        [Produces(typeof(List<CovenirReportDTO>))]
        public async Task<IActionResult> GetCovenirReport([FromBody] FTPDocumentFilesCountInputDTO request)
        {
            try
            {
                return Ok(await _covenirReportService.GetData(request));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GenerateAgingReport")]
        [Produces(typeof(List<AgingReportDTO>))]
        public async Task<IActionResult> GenerateAgingReportEasy([FromBody] AgingReportRequestDTO request)
        {
            string formattedDate = request.ToDate?.ToString("yyyy-MM-dd");
            formattedDate = formattedDate.Replace("-", "");

            try
            {
                return new FileStreamResult(await _agingReportService.GetReportStream(request),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                { FileDownloadName = "Aging Report RC Boxtruck" + formattedDate + ".xlsx" };
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("RCAgingReport")]
        [Produces(typeof(List<AgingReportDTO>))]
        public async Task<IActionResult> GetRCAgingReport([FromBody] AgingReportRequestDTO request)
        {
            try
            {
                return Ok(await _agingReportService.GetCAging("GenerateAgingReport", request));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("TICLiveryAgingReport")]
        [Produces(typeof(List<AgingReportDTO>))]
        public async Task<IActionResult> GetTICLiveryAgingReport([FromBody] AgingReportRequestDTO request)
        {
            try
            {
                return Ok(await _agingReportService.GetCAging("GenerateTICLiveryAgingReport", request));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("TICBoxtruckAgingReport")]
        [Produces(typeof(List<AgingReportDTO>))]
        public async Task<IActionResult> TICLiveryAgingReport([FromBody] AgingReportRequestDTO request)
        {
            try
            {
                return Ok(await _agingReportService.GetCAging("GenerateTICBoxtruckAgingReport", request));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}