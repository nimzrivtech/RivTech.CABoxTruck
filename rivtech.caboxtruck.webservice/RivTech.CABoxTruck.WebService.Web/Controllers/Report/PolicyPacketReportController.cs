using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;

namespace RivTech.CABoxTruck.WebService.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyPacketReportController : ControllerBase
    {
        private readonly IPacketReportService _packetReportService;

        public PolicyPacketReportController(IPacketReportService packetReportService)
        {
            _packetReportService = packetReportService;
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GeneratePacketReport(List<RiskFormDTO> form)
        {
            try
            {
                var result = await _packetReportService.GeneratePolicyPacketReport(form);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}