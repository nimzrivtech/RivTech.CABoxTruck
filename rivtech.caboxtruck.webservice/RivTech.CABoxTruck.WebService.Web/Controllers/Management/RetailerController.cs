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
    public class RetailerController : ControllerBase
    {
        private readonly IRetailerService _retailerService;

        public RetailerController(IRetailerService retailerService)
        {
            _retailerService = retailerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _retailerService.GetAll());
        }

        [HttpGet("agency/{agencyId}")]
        public async Task<IActionResult> GetRetailerAgency([FromRoute] Guid agencyId)
        {
            return Ok(await _retailerService.GetAllByAgency(agencyId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _retailerService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RetailerDTO resource)
        {
            return Ok(await _retailerService.InsertAsync(resource));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RetailerDTO resource)
        {
            if (resource.Id != id) return BadRequest();
            return Ok(await _retailerService.UpdateAsync(resource));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _retailerService.RemoveAsync(id));
        }
    }
}