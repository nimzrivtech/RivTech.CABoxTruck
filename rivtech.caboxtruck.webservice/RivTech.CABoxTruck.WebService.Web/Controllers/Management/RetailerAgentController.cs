using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers.Management
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RetailerAgentController : ControllerBase
    {
        private readonly IRetailerAgentService _retailerAgentService;

        public RetailerAgentController(IRetailerAgentService retailerAgentAgentService)
        {
            _retailerAgentService = retailerAgentAgentService;
        }

        [HttpGet("byretailer/{id}")]
        public async Task<IActionResult> GetByRetailer([FromRoute] Guid id)
        {
            return Ok(await _retailerAgentService.GetByRetailerId(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _retailerAgentService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _retailerAgentService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RetailerAgentDTO resource)
        {
            if (resource.Id != id) return BadRequest();
            return Ok(await _retailerAgentService.UpdateAsync(resource));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RetailerAgentDTO resource)
        {
            return Ok(await _retailerAgentService.InsertAsync(resource));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _retailerAgentService.RemoveAsync(id));
        }
    }
}