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
    public class BrokerInfoController : ControllerBase
    {
        private readonly IBrokerInfoService _service;
        private readonly IPolicyContactsService _policyContactsService;

        public BrokerInfoController(
            IBrokerInfoService service,
            IPolicyContactsService policyContactsService
            )
        {
            _service = service;
            _policyContactsService = policyContactsService;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<BrokerInfoDTO>))]
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

        [HttpGet("GetByRisk/{id}")]
        [Produces(typeof(BrokerInfoDTO))]
        public async Task<IActionResult> GetByRisk([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByRiskDetailIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Get/{id}")]
        [Produces(typeof(BrokerInfoDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Put/{id}")]
        [Produces(typeof(BrokerInfoDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] BrokerInfoDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                var result = await _service.UpdateAsync(resource);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Post")]
        [Produces(typeof(BrokerInfoDTO))]
        public async Task<IActionResult> Post([FromBody] BrokerInfoDTO resource)
        {
            try
            {
                if (resource.Id != null) return BadRequest();
                var result = await _service.InsertAsync(resource);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("Delete/{id}")]
        [Produces(typeof(BrokerInfoDTO))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();
                await _service.RemoveAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetPolicyContacts")]
        public async Task<IActionResult> GetPolicyContacts()
        {
            try
            {
                var res = await _policyContactsService.GetAllPolicyContacts();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
