using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicantPersonnelController : ControllerBase
    {
        private readonly IEntityContactService _entityContactService;
        public ApplicantPersonnelController(IEntityContactService entityContactService)
        {
            _entityContactService = entityContactService;
        }

        [HttpGet]
        [Produces(typeof(List<EntityContactDTO>))]
        public async Task<IActionResult> GetAll([FromRoute] Guid entityId)
        {
            var resources = await _entityContactService.GetAllAsync(entityId);
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
        public async Task<IActionResult> Post([FromBody] EntityContactDTO resource)
        {
            await _entityContactService.InsertEntityAsync(resource);

            return Ok();
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
    }
}
