using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.Applicant
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManuscriptsController : ControllerBase
    {
        private readonly IRiskManuscriptsService _manuscriptsService;

        public ManuscriptsController(IRiskManuscriptsService manuscriptsService)
        {
            _manuscriptsService = manuscriptsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _manuscriptsService.GetByRiskIdAllAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RiskManuscriptsDTO resource)
        {
            return Ok(await _manuscriptsService.InsertAsync(resource));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskManuscriptsDTO resource)
        {
            if (resource.Id != id.ToString()) return BadRequest();
            return Ok(await _manuscriptsService.UpdateAsync(resource));
        }

        [HttpPut("Dates/{id}")]
        public async Task<IActionResult> PutDates([FromRoute] Guid id, [FromBody] RiskManuscriptsDTO resource)
        {
            if (resource.RiskDetailId != id) return BadRequest();
            return Ok(await _manuscriptsService.UpdateDatesAsync(resource));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, DateTime? expirationDate, bool fromEndorsement = false)
        {
            return Ok(await _manuscriptsService.RemoveAsync(id, fromEndorsement, (expirationDate ?? DateTime.Now)));
        }

        [HttpPost("ManuscriptList")]
        [Produces("application/json")]
        public async Task<ActionResult<ManuscriptPagingDTO>>  GetRiskAllAsync([FromBody] ManuscriptFilterDTO filterDTO)
        {
            return Ok(await _manuscriptsService.GetByRiskIdAllAsync(filterDTO));
        }
    }
}