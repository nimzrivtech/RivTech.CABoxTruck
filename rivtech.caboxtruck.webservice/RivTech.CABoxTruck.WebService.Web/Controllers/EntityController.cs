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
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        [Produces(typeof(List<EntityDTO>))]
        public async Task<IActionResult> GetEntities()
        {
            var resources = await _entityService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(EntityDTO))]
        public async Task<IActionResult> GetEntity([FromRoute] Guid id)
        {
            try
            {
                var res = await _entityService.GetEntityAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("PostEntity")]
        [Produces(typeof(EntityDTO))]
        public async Task<IActionResult> PostEntity([FromBody] EntityDTO resource)
        {
            return Ok(await _entityService.InsertEntityAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(EntityDTO))]
        public async Task<IActionResult> PutAgency([FromRoute] Guid id, [FromBody] EntityDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _entityService.UpdateEntityAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(EntityDTO))]
        public async Task<IActionResult> DeleteAgency([FromRoute] Guid id)
        {
            try
            {
                await _entityService.RemoveEntityAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}