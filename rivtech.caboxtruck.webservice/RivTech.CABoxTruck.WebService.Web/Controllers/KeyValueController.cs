using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class KeyValueController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public KeyValueController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("generic/{type}")]
        public async Task<IActionResult> GetGenericList(string type)
        {
            if (string.IsNullOrEmpty(type)) return BadRequest();

            try
            {
                var result = await _cacheService.GetKeyValueList(type);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}