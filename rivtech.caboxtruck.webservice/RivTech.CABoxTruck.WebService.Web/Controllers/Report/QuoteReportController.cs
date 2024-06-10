using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;

namespace RivTech.CABoxTruck.WebService.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuoteReportController : ControllerBase
    {
        private readonly IQuoteReportService _quoteReportService;
        private readonly IRiskService _riskService;

        public QuoteReportController(IQuoteReportService quoteReportService, IRiskService riskService)
        {
            _quoteReportService = quoteReportService;
            _riskService = riskService;
        }

        [HttpGet("GetQuoteReport/{id}")]
        public async Task<IActionResult> GetQuoteReport([FromRoute] Guid id)
        {
            //GET ALL risk entity
            try
            {
                var result = await _quoteReportService.GenerateQuoteReport(id);
                await _riskService.UpdateRiskStatusOnlyAsync(id, RiskDetailStatus.Quoted);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
