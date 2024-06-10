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
    public class BusinessTypesController : ControllerBase
    {
        private readonly IBusinessTypeService _businessTypeService;
        public BusinessTypesController(IBusinessTypeService businessTypeService)
        {
            _businessTypeService = businessTypeService;
        }

        [HttpGet]
        [Produces(typeof(List<BusinessTypeDTO>))]
        public async Task<IActionResult> GetBusinessTypes()
        {
            var resources = await _businessTypeService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(BusinessTypeDTO))]
        public async Task<IActionResult> GetBusinessType([FromRoute] byte id)
        {
            try
            {
                var res = await _businessTypeService.GetBusinessTypeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(BusinessTypeDTO))]
        public async Task<IActionResult> PostBusinessType([FromBody] BusinessTypeDTO resource)
        {
            return Ok(await _businessTypeService.InsertBusinessTypeAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(BusinessTypeDTO))]
        public async Task<IActionResult> PutBusinessType([FromRoute] byte id, [FromBody] BusinessTypeDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _businessTypeService.UpdateBusinessTypeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(BusinessTypeDTO))]
        public async Task<IActionResult> DeleteBusinessType([FromRoute] byte id)
        {
            try
            {
                await _businessTypeService.RemoveBusinessTypeAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}