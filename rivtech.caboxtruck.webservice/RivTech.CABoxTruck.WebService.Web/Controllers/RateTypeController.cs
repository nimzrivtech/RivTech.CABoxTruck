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
    public class RateTypesController : ControllerBase
    {
        private readonly IRateTypeService _rateTypeService;
        public RateTypesController(IRateTypeService rateTypeService)
        {
            _rateTypeService = rateTypeService;
        }

        [HttpGet]
        [Produces(typeof(List<RateTypeDTO>))]
        public async Task<IActionResult> GetRateTypes()
        {
            var resources = await _rateTypeService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RateTypeDTO))]
        public async Task<IActionResult> GetRateType([FromRoute] byte id)
        {
            try
            {
                var res = await _rateTypeService.GetRateTypeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RateTypeDTO))]
        public async Task<IActionResult> PostRateType([FromBody] RateTypeDTO resource)
        {
            return Ok(await _rateTypeService.InsertRateTypeAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RateTypeDTO))]
        public async Task<IActionResult> PutRateType([FromRoute] byte id, [FromBody] RateTypeDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _rateTypeService.UpdateRateTypeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(RateTypeDTO))]
        public async Task<IActionResult> DeleteRateType([FromRoute] byte id)
        {
            try
            {
                await _rateTypeService.RemoveRateTypeAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}