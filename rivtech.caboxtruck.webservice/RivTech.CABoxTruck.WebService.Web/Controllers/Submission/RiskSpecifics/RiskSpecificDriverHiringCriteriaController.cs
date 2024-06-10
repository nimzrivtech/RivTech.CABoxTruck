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
    public class RiskSpecificDriverHiringCriteriaController : ControllerBase
    {
        private readonly IRiskSpecificDriverHiringCriteriaService _riskSpecificDriverHiringCriteriaService;
        public RiskSpecificDriverHiringCriteriaController(IRiskSpecificDriverHiringCriteriaService riskSpecificDriverHiringCriteriaService)
        {
            _riskSpecificDriverHiringCriteriaService = riskSpecificDriverHiringCriteriaService;
        }

        [HttpGet]
        [Produces(typeof(List<RiskSpecificDriverHiringCriteriaDto>))]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _riskSpecificDriverHiringCriteriaService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskSpecificDriverHiringCriteriaDto))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskSpecificDriverHiringCriteriaService.GetDriverHiringCriteriaAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskSpecificDriverHiringCriteriaDto))]
        public async Task<IActionResult> Post([FromBody] RiskSpecificDriverHiringCriteriaDto resource)
        {
            return Ok(await _riskSpecificDriverHiringCriteriaService.InsertDriverHiringCriteriaAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskSpecificDriverHiringCriteriaDto))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskSpecificDriverHiringCriteriaDto resource)
        {
            try
            {
                if (resource.Id != id)
                {
                    return BadRequest();
                }

                return Ok(await _riskSpecificDriverHiringCriteriaService.UpdateDriverHiringCriteriaAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                return Ok(_riskSpecificDriverHiringCriteriaService.RemoveDriverHiringCriteriaAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
