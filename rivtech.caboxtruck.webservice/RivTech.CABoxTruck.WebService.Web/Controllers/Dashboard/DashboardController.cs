using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Dashboard;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetDashboardCount")]
        [Produces(typeof(List<DashboardCountDto>))]
        public async Task<IActionResult> GetDashboardCount()
        {
            //GET ALL risk entity
            try
            {
                var res = _dashboardService.GetDashboardCount();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetRiskByPolicyNumber")]
        [Produces(typeof(DashboardPolicySearchDTO))]
        public async Task<IActionResult> GetRiskByPolicyNumber([FromQuery] string policyNumber)
        {
            try
            {
                var result = await _dashboardService.GetRiskByPolicyNumber(policyNumber);
                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
             
        }
    }
}
