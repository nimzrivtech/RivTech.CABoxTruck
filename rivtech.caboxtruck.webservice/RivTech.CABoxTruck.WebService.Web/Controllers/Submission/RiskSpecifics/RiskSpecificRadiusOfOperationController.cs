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
    public class RiskSpecificRadiusOfOperationController : ControllerBase
    {
        private readonly IRiskSpecificRadiusOfOperationService _riskSpecificRadiusOfOperationService;
        public RiskSpecificRadiusOfOperationController(IRiskSpecificRadiusOfOperationService riskSpecificRadiusOfOperationService)
        {
            _riskSpecificRadiusOfOperationService = riskSpecificRadiusOfOperationService;
        }

        [HttpGet]
        [Produces(typeof(List<RiskSpecificRadiusOfOperationDto>))]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _riskSpecificRadiusOfOperationService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskSpecificRadiusOfOperationDto))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskSpecificRadiusOfOperationService.GetRadiusOfOperationAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskSpecificRadiusOfOperationDto))]
        public async Task<IActionResult> Post([FromBody] RiskSpecificRadiusOfOperationDto resource)
        {
            return Ok(await _riskSpecificRadiusOfOperationService.InsertRadiusOfOperationAsync(resource));
        }

        [HttpPut("risk/{riskDetailId}")]
        [Produces(typeof(RiskSpecificRadiusOfOperationDto))]
        public async Task<IActionResult> Put([FromRoute] Guid riskDetailId, [FromBody] RiskSpecificRadiusOfOperationDto resource)
        {
            return Ok(await _riskSpecificRadiusOfOperationService.UpdateRadiusOfOperationAsync(riskDetailId, resource));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                return Ok(_riskSpecificRadiusOfOperationService.RemoveRadiusOfOperationAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
