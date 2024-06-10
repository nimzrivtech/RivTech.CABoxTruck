using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificMaintenanceSafetyController : ControllerBase
    {
        private readonly IRiskSpecificMaintenanceSafetyService _maintenanceSafetyService;
        public RiskSpecificMaintenanceSafetyController(
            IRiskSpecificMaintenanceSafetyService riskSpecificMaintenanceSafetyService
        )
        {
            _maintenanceSafetyService = riskSpecificMaintenanceSafetyService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveMaintenanceSafety([FromBody] RiskSpecificMaintenanceSafetyDTO maintenanceSafety)
        {
            try
            {
                var res = await _maintenanceSafetyService.SaveMaintenanceSafety(maintenanceSafety);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}