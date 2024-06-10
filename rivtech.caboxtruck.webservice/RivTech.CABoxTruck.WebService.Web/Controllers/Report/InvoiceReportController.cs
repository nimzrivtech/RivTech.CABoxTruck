using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Enums;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;

namespace RivTech.CABoxTruck.WebService.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceReportController : ControllerBase
    {
        private readonly IInvoiceReportService _invoiceReportService;

        public InvoiceReportController(IInvoiceReportService invoiceReportService, IRiskService riskService)
        {
            _invoiceReportService = invoiceReportService;
        }

        [HttpPost("{riskId}")]
        public async Task<IActionResult> GetInvoiceReport([FromRoute] Guid riskId, [FromBody] InstallmentAndInvoiceDTO resource)
        {
            try
            {
                var invoice = new InvoiceDTO
                {
                    Id = resource.InvoiceId.GetValueOrDefault(),
                    RiskId = riskId,
                    InvoiceType = (InvoiceType)Enum.Parse(typeof(InvoiceType), resource.InstallmentType)
            };
                var result = await _invoiceReportService.GenerateInvoiceReport<string>(invoice, true);
                return Ok(result.Output);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] List<InvoiceDTO> invoices)
        {
            try
            {
                var result = await _invoiceReportService.GenerateInvoiceReport<string>(invoices);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("test")]
        public IActionResult Test([FromBody] InvoiceDTO invoice)
        {
            try
            {
                var testData = new { Message = "Connected Successfully", Parameter = invoice };
                return Ok(invoice);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}