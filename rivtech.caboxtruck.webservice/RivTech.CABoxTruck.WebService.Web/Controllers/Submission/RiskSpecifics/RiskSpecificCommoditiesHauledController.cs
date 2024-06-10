using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificCommoditiesHauledController : ControllerBase
    {
        private readonly IRiskSpecificCommoditiesHauledService _service;

        public RiskSpecificCommoditiesHauledController(IRiskSpecificCommoditiesHauledService service)
        {
            _service = service;
        }

        [HttpGet("GetByRisk/{riskDetailId}")]
        [Produces(typeof(List<RiskSpecificDestinationDto>))]
        public async Task<IActionResult> GetByRisk([FromRoute] Guid riskDetailId)
        {
            var resources = await _service.GetByRiskDetailIdAsync(riskDetailId);
            return new ObjectResult(resources);
        }

        [HttpPost]
        [Produces(typeof(RiskSpecificCommoditiesHauledDto))]
        public async Task<IActionResult> Post([FromBody] RiskSpecificCommoditiesHauledDto resource)
        {
            return Ok(await _service.InsertAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskSpecificDestinationDto))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskSpecificCommoditiesHauledDto resource)
        {
            if (resource.Id != id) return BadRequest();

            return Ok(await _service.UpdateAsync(resource));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _service.RemoveAsync(id));
        }
    }
}