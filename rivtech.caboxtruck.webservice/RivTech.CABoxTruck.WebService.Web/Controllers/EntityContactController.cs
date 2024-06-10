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
    public class EntityContactController : ControllerBase
    {
        private readonly IEntityContactService _entityContactService;

        public EntityContactController(IEntityContactService entityContactService)
        {
            _entityContactService = entityContactService;
        }

        [HttpGet]
        [Produces(typeof(List<EntityContactDTO>))]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _entityContactService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(EntityContactDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _entityContactService.GetEntityAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(EntityContactDTO))]
        public async Task<IActionResult> Post([FromBody] EntityContactDTO resource)
        {
            return Ok(await _entityContactService.InsertEntityAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(EntityDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] EntityContactDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _entityContactService.UpdateEntityAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(EntityContactDTO))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _entityContactService.RemoveEntityAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}