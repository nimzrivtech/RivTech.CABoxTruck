using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LimitController : ControllerBase
    {
        private readonly ICoverageLimitService _coverageLimitService;
        private readonly ILimitsService _limitsService;

        public LimitController(ICoverageLimitService coverageLimitService, ILimitsService limitsService)
        {
            _coverageLimitService = coverageLimitService;
            _limitsService = limitsService;
        }

        [HttpGet("getlimits/{stateCode}/{raterVersionId}")]
        [Produces(typeof(LimitListDTO))]
        public async Task<IActionResult> GetAllLimitList(string stateCode, int? raterVersionId = null)
        {
            try
            {
                var result = await _limitsService.GetPerStateAsync(stateCode, raterVersionId);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskCoverageDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _coverageLimitService.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskCoverageDTO))]
        public async Task<IActionResult> SaveRiskCoverage([FromBody] SaveRiskCoverageDTO resource)
        {
            try
            {
                var data = await _coverageLimitService.IsExistAsync(resource.RiskDetailId);
                if(!data)
                {
                    return Ok(await _coverageLimitService.InsertAsync(resource));
                }
                return Ok(await _coverageLimitService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
            
        }

        [HttpPost("setdefaults")]
        public async Task<IActionResult> SetDefaults([FromBody] SaveRiskCoverageDTO resource)
        {
            try
            {
                var data = await _coverageLimitService.IsExistAsync(resource.RiskDetailId);
                await _coverageLimitService.UpdateLimitDataAsync(resource);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }

        }
    }
}