using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DataFixes.Cancellations;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.DataFix;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.DataFix
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataFixController : ControllerBase
    {
        private readonly IOOSDataFixService _oosDataFixService;
        private readonly ICancellationPDBreakdownDataFixer _cancellationPDBreakdownDataFixer;

        public DataFixController(
            IOOSDataFixService oosDataFixService,
            ICancellationPDBreakdownDataFixer cancellationPDBreakdownDataFixer
        ) {
            _oosDataFixService = oosDataFixService;
            _cancellationPDBreakdownDataFixer = cancellationPDBreakdownDataFixer;
        }


        [HttpPost("FixOOSData/{policyNumber}")]
        public async Task<IActionResult> FixOOSData([FromRoute] string policyNumber)
        {
            await _oosDataFixService.FixData(policyNumber);
            return Ok();
        }

        [HttpPost("FixDataWithCancelReinstate/{policyNumber}")]
        public async Task<IActionResult> FixDataWithCancelReinstate([FromRoute] string policyNumber)
        {
            await _oosDataFixService.FixDataWithCancelReinstate(policyNumber);
            return Ok();
        }

        [HttpPost("FixDataVehicleCompFT")]
        public async Task<IActionResult> FixDataVehicleCompFT()
        {
            await _oosDataFixService.FixDataVehicleCompFT();
            return Ok();
        }

        [HttpPost("FixPD-Comp-Coll-Cancel-Breakdowns")]
        public async Task<IActionResult> FixPDCompCollCancelBreakdowns()
        {
            await _cancellationPDBreakdownDataFixer.FixAsync();
            return Ok();
        }

        [HttpPost("FixPD-Comp-Coll-Cancel-Breakdowns-By-RiskDetailIds")]
        public async Task<IActionResult> FixPDCompCollCancelBreakdownsByRiskDetailIds([FromBody] FixPDComColDTO request)
        {
            await _cancellationPDBreakdownDataFixer.FixCollCompByRiskDetailAsync(request.RiskDetailIds);
            return Ok();
        }

    }
}
