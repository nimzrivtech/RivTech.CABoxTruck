using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificDriverInfoPageController : ControllerBase
    {
        private readonly IRiskSpecificDriverInfoPageService _riskSpecificDriverInfoPageService;
        private readonly IDriverInfoOptionsService _driverInfoOptionsService;
        public RiskSpecificDriverInfoPageController(IRiskSpecificDriverInfoPageService riskSpecificDriverInfoPageService,
            IDriverInfoOptionsService driverInfoOptionsService)
        {
            _riskSpecificDriverInfoPageService = riskSpecificDriverInfoPageService;
            _driverInfoOptionsService = driverInfoOptionsService;
        }

        [HttpPut("{riskDetailId}")]
        [Produces(typeof(string))]
        public async Task<IActionResult> Put([FromRoute] Guid riskDetailId, [FromBody] RiskSpecificDriverInfoPageDto resource)
        {
            return Ok(await _riskSpecificDriverInfoPageService.UpdateDriverInfoAndHiringCriteriaAsync(riskDetailId, resource));
        }

        [HttpGet("options")]
        [Produces(typeof(DriverInfoOptionsDto))]
        public async Task<IActionResult> GetOptions()
        {
            var res = await _driverInfoOptionsService.GetAllAsync();
            return Ok(res);
        }
    }
}
