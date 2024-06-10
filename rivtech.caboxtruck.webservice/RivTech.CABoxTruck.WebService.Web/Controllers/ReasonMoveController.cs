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
    public class ReasonMoveController : ControllerBase
    {
        private readonly IReasonMoveService _service;

        public ReasonMoveController(IReasonMoveService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<ReasonMoveDTO>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _service.GetAllAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Get/{id}")]
        [Produces(typeof(ReasonMoveDTO))]
        public async Task<IActionResult> Get([FromRoute] Int16? id)
        {
            try
            {
                if (id == null) return BadRequest();
                var res = await _service.GetByIdAsync(id.Value);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Put/{id}")]
        [Produces(typeof(ReasonMoveDTO))]
        public async Task<IActionResult> Put([FromRoute] Int16? id, [FromBody] ReasonMoveDTO resource)
        {
            try
            {
                try
                {
                    if (id == null) return BadRequest();
                    if (resource.Id != id) return BadRequest();
                    return Ok(await _service.UpdateAsync(resource));
                }
                catch (ModelNotFoundException)
                {
                    return NotFound();
                }
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Post")]
        [Produces(typeof(ReasonMoveDTO))]
        public async Task<IActionResult> Post([FromBody] ReasonMoveDTO resource)
        {
            try
            {
                if (resource.Id != null) return BadRequest();
                return Ok(await _service.InsertAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("Delete/{id}")]
        [Produces(typeof(ReasonMoveDTO))]
        public async Task<IActionResult> Delete([FromRoute] Int16? id)
        {
            try
            {
                if (id == null) return BadRequest();
                await _service.RemoveAsync(id.Value);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
