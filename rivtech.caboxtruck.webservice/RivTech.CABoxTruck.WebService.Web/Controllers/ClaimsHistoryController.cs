using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ClaimsHistoryController : ControllerBase
    {
        private readonly IClaimsHistoryService _claimsHistoryService;

        public ClaimsHistoryController(IClaimsHistoryService claimsHistoryService)
        {
            _claimsHistoryService = claimsHistoryService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(List<ClaimsHistoryDTO>))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _claimsHistoryService.GetByRiskDetailIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(List<ClaimsHistoryDTO>))]
        public async Task<IActionResult> Post([FromBody] List<ClaimsHistoryDTO> resource)
        {
            try
            {
                var data = await _claimsHistoryService.ExistsAsync(resource[0].RiskDetailId);
                return data
                    ? Ok(await _claimsHistoryService.UpdateAsync(resource))
                    : Ok(await _claimsHistoryService.InsertAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}