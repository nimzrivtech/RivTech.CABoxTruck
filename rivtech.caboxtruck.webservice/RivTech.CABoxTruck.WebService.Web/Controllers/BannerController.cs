using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _service;

        public BannerController(IBannerService service)
        {
            _service = service;
        }


        [HttpGet]
        [Produces(typeof(BannerDTO))]
        public async Task<ActionResult<List<BannerDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}
