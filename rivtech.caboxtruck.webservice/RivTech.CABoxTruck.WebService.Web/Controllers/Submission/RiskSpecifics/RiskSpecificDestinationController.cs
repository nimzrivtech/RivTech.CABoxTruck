using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificDestinationController : ControllerBase
    {
        private readonly IRiskSpecificDestinationService _riskSpecificDestinationService;
        public RiskSpecificDestinationController(IRiskSpecificDestinationService riskSpecificDestinationService)
        {
            _riskSpecificDestinationService = riskSpecificDestinationService;
        }

        [HttpGet("list/{riskDetailId}")]
        [Produces(typeof(List<RiskSpecificDestinationDto>))]
        public async Task<IActionResult> GetAll([FromRoute] Guid riskDetailId)
        {
            var resources = await _riskSpecificDestinationService.GetAllAsync(riskDetailId);
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskSpecificDestinationDto))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskSpecificDestinationService.GetDestinationAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskSpecificDestinationDto))]
        public async Task<IActionResult> Post([FromBody] RiskSpecificDestinationDto resource)
        {
            return Ok(await _riskSpecificDestinationService.InsertDestinationAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskSpecificDestinationDto))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskSpecificDestinationDto resource)
        {
            try
            {
                if (resource.Id != id)
                {
                    return BadRequest();
                }

                return Ok(await _riskSpecificDestinationService.UpdateDestinationAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _riskSpecificDestinationService.RemoveDestinationAsync(id));
        }
    }
}
