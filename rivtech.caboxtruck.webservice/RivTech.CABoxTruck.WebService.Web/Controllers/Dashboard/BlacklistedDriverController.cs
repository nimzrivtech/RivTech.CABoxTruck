using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BlacklistedDriverController : ControllerBase
    {
        private readonly IBlacklistedDriverService _blacklistedDriverService;

        public BlacklistedDriverController(IBlacklistedDriverService blacklistedDriverService)
        {
            _blacklistedDriverService = blacklistedDriverService;
        }

        [HttpPost("AddUpdate")]
        [Produces(typeof(BlacklistedDriverDTO))]
        public async Task<IActionResult> AddUpdate([FromBody] BlacklistedDriverDTO resource)
        {
            try
            {
                return Ok(await _blacklistedDriverService.AddOrUpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("SearchDriver")]
        [Produces(typeof(IEnumerable<BlacklistedDriverDTO>))]
        public async Task<IActionResult> SearchDriver([FromQuery] string driverName, [FromQuery] string licenseNumber)
        {
            try
            {
                return Ok(await _blacklistedDriverService.SearchDriver(driverName, licenseNumber));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("ValidateDriver")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> ValidateDriver([FromQuery] string driverName, [FromQuery] string licenseNumber)
        {
            try
            {
                return Ok(await _blacklistedDriverService.ValidateDriver(driverName, licenseNumber));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
