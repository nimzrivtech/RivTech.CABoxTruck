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
    [ApiController]
    [Authorize]
    public class RiskStatusTypesController : ControllerBase
    {
        private readonly IRiskStatusTypeService _riskStatusTypeService;
        public RiskStatusTypesController(IRiskStatusTypeService riskStatusTypeService)
        {
            _riskStatusTypeService = riskStatusTypeService;
        }

        [HttpGet]
        [Produces(typeof(List<RiskStatusTypeDTO>))]
        public async Task<IActionResult> GetRiskStatusTypes()
        {
            var resources = await _riskStatusTypeService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskStatusTypeDTO))]
        public async Task<IActionResult> GetRiskStatusType([FromRoute] byte id)
        {
            try
            {
                var res = await _riskStatusTypeService.GetRiskStatusTypeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskStatusTypeDTO))]
        public async Task<IActionResult> PostRiskStatusType([FromBody] RiskStatusTypeDTO resource)
        {
            return Ok(await _riskStatusTypeService.InsertRiskStatusTypeAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskStatusTypeDTO))]
        public async Task<IActionResult> PutRiskStatusType([FromRoute] byte id, [FromBody] RiskStatusTypeDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _riskStatusTypeService.UpdateRiskStatusTypeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(RiskStatusTypeDTO))]
        public async Task<IActionResult> DeleteRiskStatusType([FromRoute] byte id)
        {
            try
            {
                await _riskStatusTypeService.RemoveRiskStatusTypeAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}