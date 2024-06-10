using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission;
using RivTech.CABoxTruck.WebService.Service.IServices.Risk;

namespace RivTech.CABoxTruck.WebService.Controllers.Risk
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSummaryController : ControllerBase
    {
        private readonly IRiskSummaryService _riskSummaryService;

        public RiskSummaryController(IRiskSummaryService riskSummaryService)
        {
            _riskSummaryService = riskSummaryService;
        }

        [HttpPost("summary/list")]
        [Produces(typeof(List<RiskSummaryDto>))]
        public async Task<IActionResult> GetRiskSummaryList([FromBody] SubmissionFilterDto filter)
        {
            return Ok(await _riskSummaryService.GetFilteredSummaryListAsync(filter));
        }
    }
}
