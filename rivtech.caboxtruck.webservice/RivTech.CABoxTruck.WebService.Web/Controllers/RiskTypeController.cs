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
    public class RiskTypesController : ControllerBase
    {
        private readonly IRiskTypeService _riskTypeService;
        public RiskTypesController(IRiskTypeService riskTypeService)
        {
            _riskTypeService = riskTypeService;
        }

        [HttpGet]
        [Produces(typeof(List<RiskTypeDTO>))]
        public async Task<IActionResult> GetRiskTypes()
        {
            var resources = await _riskTypeService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskTypeDTO))]
        public async Task<IActionResult> GetRiskType([FromRoute] byte id)
        {
            try
            {
                var res = await _riskTypeService.GetRiskTypeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskTypeDTO))]
        public async Task<IActionResult> PostRiskType([FromBody] RiskTypeDTO resource)
        {
            return Ok(await _riskTypeService.InsertRiskTypeAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RiskTypeDTO))]
        public async Task<IActionResult> PutRiskType([FromRoute] byte id, [FromBody] RiskTypeDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _riskTypeService.UpdateRiskTypeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(RiskTypeDTO))]
        public async Task<IActionResult> DeleteRiskType([FromRoute] byte id)
        {
            try
            {
                await _riskTypeService.RemoveRiskTypeAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}