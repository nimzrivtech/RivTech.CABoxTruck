using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class QuoteOptionController : ControllerBase
    {

        private readonly ILogger<QuoteOptionController> _logger;
        private readonly IQuoteOptionsService _quoteOptionsService;
        private readonly IBillingService _billingService;
        private readonly ITaxService _taxService;

        public QuoteOptionController(
            ILogger<QuoteOptionController> logger,
            IQuoteOptionsService quoteOptionsService,
            IBillingService billingService,
            ITaxService taxService
        )
        {
            _logger = logger;
            _quoteOptionsService = quoteOptionsService;
            _billingService = billingService;
            _taxService = taxService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskCoverageListDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                // var res = await _quoteOptionsService.GetByIdAsync(id);
                var res = await _quoteOptionsService.GetRiskCoverageAndPremiumByIdAsync(id);
                return Ok(new RiskCoverageListDTO() { RiskDetailId = id, RiskCoverages = res });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetPremium/{RiskId}")]
        [Produces(typeof(RiskCoverageListDTO))]
        public async Task<IActionResult> GetPremium([FromRoute] Guid RiskId)
        {
            try
            {
                var res = await _quoteOptionsService.GetRiskCoverageAndPremiumMetricsByRiskIdAsync(RiskId);
                return Ok(new RiskCoverageListDTO() { RiskDetailId = res.First().RiskDetailId, RiskCoverages = res });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskCoverageListDTO))]
        public async Task<IActionResult> SaveRiskCoverage([FromBody] RiskCoverageListDTO resource)
        {
            try
            {
                var data = await _quoteOptionsService.IsExistAsync(resource.RiskDetailId);
                if (!data)
                {
                    return Ok(await _quoteOptionsService.InsertAsync(resource));
                }
                return Ok(await _quoteOptionsService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPut]
        [Route("UpdatePaymentOption/{riskDetailId}")]
        public async Task<IActionResult> UpdatePaymentOption(Guid riskDetailId, [FromQuery] int option)
        {
            try
            {
                await _taxService.CalculateRiskTaxAsync(riskDetailId, option);
                await _billingService.UpdatePaymentOptions(riskDetailId, option);
                var coverages = await _quoteOptionsService.GetRiskCoverageAndPremiumByIdAsync(riskDetailId);
                var riskCov = coverages.First(x => x.OptionNumber == option);
                return Ok(riskCov);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("CopyOption/{riskDetailId}")]
        public async Task<IActionResult> CopyOption(Guid riskDetailId, [FromQuery] int optionToCopy)
        {
            var option = await _quoteOptionsService.CopyOptionAsync(riskDetailId, optionToCopy);
            return Ok(option);
        }

        [HttpDelete]
        [Route("DeleteOption/{riskDetailId}")]
        public async Task<IActionResult> DeleteOption(Guid riskDetailId, [FromQuery] Guid optionId)
        {
            await _quoteOptionsService.DeleteOptionAsync(riskDetailId, optionId);
            return Ok();
        }
    }
}