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
    public class AdditionalTermsController : ControllerBase
    {
        private readonly IAdditionalTermService _additionalTermService;

        public AdditionalTermsController(IAdditionalTermService additionalTermService)
        {
            _additionalTermService = additionalTermService;
        }

        [HttpGet]
        [Produces(typeof(List<AdditionalTermDTO>))]
        public async Task<IActionResult> GetAdditionalTerms()
        {
            var resources = await _additionalTermService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(AdditionalTermDTO))]
        public async Task<IActionResult> GetAdditionalTerm([FromRoute] byte id)
        {
            try
            {
                var res = await _additionalTermService.GetAdditionalTermAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(AdditionalTermDTO))]
        public async Task<IActionResult> PostAdditionalTerm([FromBody] AdditionalTermDTO resource)
        {
            return Ok(await _additionalTermService.InsertAdditionalTermAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(AdditionalTermDTO))]
        public async Task<IActionResult> PutAdditionalTerm([FromRoute] byte id, [FromBody] AdditionalTermDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _additionalTermService.UpdateAdditionalTermAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(AdditionalTermDTO))]
        public async Task<IActionResult> DeleteAdditionalTerm([FromRoute] byte id)
        {
            try
            {
                await _additionalTermService.RemoveAdditionalTermAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}