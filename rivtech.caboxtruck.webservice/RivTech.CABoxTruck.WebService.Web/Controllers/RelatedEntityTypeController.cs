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
    public class RelatedEntityTypesController : ControllerBase
    {
        private readonly IRelatedEntityTypeService _relatedEntityTypeService;
        public RelatedEntityTypesController(IRelatedEntityTypeService relatedEntityTypeService)
        {
            _relatedEntityTypeService = relatedEntityTypeService;
        }

        [HttpGet]
        [Produces(typeof(List<RelatedEntityTypeDTO>))]
        public async Task<IActionResult> GetRelatedEntityTypes()
        {
            var resources = await _relatedEntityTypeService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RelatedEntityTypeDTO))]
        public async Task<IActionResult> GetRelatedEntityType([FromRoute] byte id)
        {
            try
            {
                var res = await _relatedEntityTypeService.GetRelatedEntityTypeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RelatedEntityTypeDTO))]
        public async Task<IActionResult> PostRelatedEntityType([FromBody] RelatedEntityTypeDTO resource)
        {
            return Ok(await _relatedEntityTypeService.InsertRelatedEntityTypeAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(RelatedEntityTypeDTO))]
        public async Task<IActionResult> PutRelatedEntityType([FromRoute] byte id, [FromBody] RelatedEntityTypeDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _relatedEntityTypeService.UpdateRelatedEntityTypeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(RelatedEntityTypeDTO))]
        public async Task<IActionResult> DeleteRelatedEntityType([FromRoute] byte id)
        {
            try
            {
                await _relatedEntityTypeService.RemoveRelatedEntityTypeAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}