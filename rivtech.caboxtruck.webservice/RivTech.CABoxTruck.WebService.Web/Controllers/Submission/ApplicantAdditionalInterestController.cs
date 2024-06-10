using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicantAdditionalInterestController : ControllerBase
    {
        private readonly IApplicantAdditionalInterestService _applicantAdditionalInterestService;

        public ApplicantAdditionalInterestController(IApplicantAdditionalInterestService riskAdditionalInterestService)
        {
            _applicantAdditionalInterestService = riskAdditionalInterestService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskAdditionalInterestDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _applicantAdditionalInterestService.GetAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRisk/{riskDetailId}")]
        [Produces(typeof(List<RiskAdditionalInterestDTO>))]
        public async Task<IActionResult> GetByRisk([FromRoute] Guid riskDetailId, bool fromEndorsement = false)
        {
            var resources = await _applicantAdditionalInterestService.GetByRiskDetailIdAsync(riskDetailId, fromEndorsement);
            return new ObjectResult(resources);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RiskAdditionalInterestDTO resource)
        {
            return Ok(await _applicantAdditionalInterestService.InsertAsync(resource));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskAdditionalInterestDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _applicantAdditionalInterestService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, DateTime? expirationDate, bool fromEndorsement = false)
        {
            try
            {
                return Ok(await _applicantAdditionalInterestService.RemoveAsync(id, fromEndorsement, (expirationDate ?? DateTime.Now)));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("BlanketCoverage/{id}")]
        public async Task<IActionResult> UpdateBlanketCoverage([FromRoute] Guid id, [FromBody] BlanketCoverageDTO resource)
        {
            try
            {
                if (resource.RiskDetailId != id) return BadRequest();
                return Ok(await _applicantAdditionalInterestService.UpdateBlanketCoverage(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

      
        [HttpPost("AddtlInterestList")]
        [Produces("application/json")]
        public async Task<ActionResult<AdditionalInterestPagingDTO>> GetTaskListAsync([FromBody] AdditionalInterestFilterDTO filterDTO)
        {
            try
            {
                var res = await _applicantAdditionalInterestService.GetByRiskDetailIdAsync(filterDTO);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}