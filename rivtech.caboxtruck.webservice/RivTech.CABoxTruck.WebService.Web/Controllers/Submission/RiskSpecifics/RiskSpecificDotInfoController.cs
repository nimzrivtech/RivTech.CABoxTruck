using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificDotInfoController : ControllerBase
    {
        private readonly IRiskSpecificDotInfoService _riskSpecificDotInfoService;
        private readonly IDotInfoOptionsService _dotInfoOptionsService;
        public RiskSpecificDotInfoController(IRiskSpecificDotInfoService riskSpecificDotInfoService, IDotInfoOptionsService dotInfoOptionsService)
        {
            _riskSpecificDotInfoService = riskSpecificDotInfoService;
            _dotInfoOptionsService = dotInfoOptionsService;
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskSpecificDotInfoDto))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskSpecificDotInfoDto resource)
        {
            return Ok(await _riskSpecificDotInfoService.UpdateDotInfoAsync(id, resource));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                return Ok(_riskSpecificDotInfoService.RemoveDotInfoAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("options")]
        [Produces(typeof(DriverInfoOptionsDto))]
        public async Task<IActionResult> GetOptions()
        {
            var res = await _dotInfoOptionsService.GetAllAsync();
            return Ok(res);
        }
    }
}
