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
    public class RiskSpecificDriverInfoController : ControllerBase
    {
        private readonly IRiskSpecificDriverInfoService _riskSpecificDriverInfoService;
        public RiskSpecificDriverInfoController(IRiskSpecificDriverInfoService riskSpecificDriverInfoService)
        {
            _riskSpecificDriverInfoService = riskSpecificDriverInfoService;
        }

        [HttpGet]
        [Produces(typeof(List<RiskSpecificDriverInfoDto>))]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _riskSpecificDriverInfoService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskSpecificDriverInfoDto))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskSpecificDriverInfoService.GetDriverInfoAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskSpecificDriverInfoDto))]
        public async Task<IActionResult> Post([FromBody] RiskSpecificDriverInfoDto resource)
        {
            return Ok(await _riskSpecificDriverInfoService.InsertDriverInfoAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskSpecificDriverInfoDto))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskSpecificDriverInfoDto resource)
        {
            try
            {
                if (resource.Id != id)
                {
                    return BadRequest();
                }

                return Ok(await _riskSpecificDriverInfoService.UpdateDriverInfoAsync(resource));
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
                return Ok(_riskSpecificDriverInfoService.RemoveDriverInfoAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
